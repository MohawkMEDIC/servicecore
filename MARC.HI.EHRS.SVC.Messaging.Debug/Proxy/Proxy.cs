using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using MARC.HI.EHRS.SVC.Core.Services;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Messaging.Debug.Proxy
{
    public class Proxy : IMessageHandlerService
    {
        // The thread the master server listens on
        private Thread m_serverThread = null;

        // TCP Listener
        private TcpListener m_tcpListener = null;

        // Manual Reset event ensures that one client can connect properly
        private ManualResetEvent m_clientConnected = new ManualResetEvent(false);

        // JF : Bug 2015 - Requires a list of active threads
        private List<Thread> m_activeConnectionThreads = new List<Thread>(10);

        // Synchronization object
        private Object m_syncObject = new object();

        /// <summary>
        /// Start the socket service
        /// </summary>
        private void StartSocketService()
        {
            // Listener for connections
            try
            {
                //IPEndPoint localIp = new IPEndPoint(m_configuration.BindAddress, m_configuration.BindPort);
                this.m_tcpListener = new TcpListener(9999);
                this.m_tcpListener.Start();
                ///Trace.TraceInformation("Started TCP proxy on '{0}'", localIp);
                while (true)
                {
                    m_clientConnected.Reset();
                    this.m_tcpListener.BeginAcceptTcpClient(new AsyncCallback(OnBeginAccept), null);
                    m_clientConnected.WaitOne();
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
            }
            finally
            {

                // Shut down threads
                Trace.TraceInformation("Shutting down client threads");
                foreach (var thd in this.m_activeConnectionThreads)
                    try
                    {
                        thd.Abort(); // Abort the thread
                    }
                    catch{}

                if (this.m_tcpListener != null)
                    this.m_tcpListener.Stop();
            }
        }

        /// <summary>
        /// Begin the acceptance of the client tcp connection
        /// </summary>
        private void OnBeginAccept(IAsyncResult ar)
        {

            TcpClient client = null;

            // JF - Bug 2015
            // Try to fork a new thread to handle the client connection
            IPEndPoint destination = null;
            TcpClient destinationClient = null;
            NetworkStream destinationStream = null;

            try
            {
                // Accept the incoming request
                client = this.m_tcpListener.EndAcceptTcpClient(ar);

                // Allow waiting thread to proceed
                m_clientConnected.Set();

                // TODO: Configuration
                TimeSpan timeout = new TimeSpan(0, 0, 0, 1, 0);
                // Now time to buffer and read the destination ... 

                NetworkStream clientStream = client.GetStream();
                // Backlog of data to be sent
                List<byte> receivedBytes = new List<byte>(),
                    sentBytes = new List<byte>();
                bool nullReceive = false;

                while (client.Connected && ((destinationClient != null && destinationClient.Connected) ^ (destinationClient == null)))
                {

                    int br = 0; // bytes read
                    byte[] buffer = new byte[2048];
                    DateTime lastReceivedTime = DateTime.Now;

                    // Write back data!
                    if (destinationStream != null)
                    {
                        // Wait for data
                        while (!destinationStream.DataAvailable)
                            if (DateTime.Now.Subtract(lastReceivedTime) > timeout)
                                break;

                        while (destinationStream.DataAvailable)
                        {
                            br = destinationStream.Read(buffer, 0, 2048);
                            if (br == 2048)
                                sentBytes.AddRange(buffer);
                            else
                                sentBytes.AddRange(buffer.Take(br));
                            Thread.Sleep(50);
                        }

                        // Send back
                        if (sentBytes.Count > 0)
                        {
                            clientStream.Write(sentBytes.ToArray(), 0, sentBytes.Count);
                            clientStream.Flush();
                            Trace.TraceInformation("Received {0} bytes from {1}...", sentBytes.Count, destinationClient.Client.RemoteEndPoint);
                            StoreMessage(System.Text.Encoding.UTF8.GetString(sentBytes.ToArray()));
                            sentBytes.Clear();
                        }
                        else if (nullReceive && !clientStream.DataAvailable)
                            throw new TimeoutException();
                    }
                    else if (nullReceive)
                        break;


                    // Wait for data
                    lastReceivedTime = DateTime.Now;
                    while (!clientStream.DataAvailable)
                        if (DateTime.Now.Subtract(lastReceivedTime) > timeout)
                            break;

                    // Read data
                    while (clientStream.DataAvailable)
                    {
                        br = clientStream.Read(buffer, 0, 2048);
                        if (br == 2048)
                            receivedBytes.AddRange(buffer);
                        else
                            receivedBytes.AddRange(buffer.Take(br));
                    }

                    if (receivedBytes.Count == 0)
                    {
                        nullReceive = true;
                        continue;
                    }

                    Trace.TraceInformation("Received {0} bytes from {1}...", receivedBytes.Count, client.Client.RemoteEndPoint);
                    String bufferData = System.Text.Encoding.UTF8.GetString(receivedBytes.ToArray());

                    // Do we have a place to send?
                    if (destination == null)
                        destination = this.ReadDestination(bufferData);
                    if (destinationClient == null && destination != null)
                    {
                        Trace.TraceInformation("Proxy to {0}", destination);
                        destinationClient = new TcpClient(AddressFamily.InterNetwork);
                        destinationClient.Connect(destination);
                        destinationStream = destinationClient.GetStream();
                    }
                    if (destinationStream != null)
                    {
                        destinationStream.Write(receivedBytes.ToArray(), 0, receivedBytes.Count);
                        destinationStream.Flush();
                    }
                    // TODO: Store this
                    StoreMessage(bufferData);
                    receivedBytes.Clear();
                }

            }
            catch (TimeoutException)
            {
                // Do nothing!
            }
            catch (ObjectDisposedException)
            { } // Do nothing
            catch (Exception e)
            {
                if (client != null)
                    Trace.TraceError("Could not establish connection with '{0}'. Error: {1}", client.Client.RemoteEndPoint, e);
                else
                    Trace.TraceError("Should not be here, error occurred establishing connection : {0}", e);
            }
            finally
            {
                if (destinationStream != null)
                    destinationStream.Close();
                if (destinationClient != null)
                    destinationClient.Close();
                if (client != null)
                    client.Close();
            }
        }

        /// <summary>
        /// Store a message
        /// </summary>
        private void StoreMessage(String bufferData)
        {
            // First attempt to determine if the received bytes are HTTP traffic or MLLP traffic
            IMessagePersistenceService imp = this.Context.GetService(typeof(IMessagePersistenceService)) as IMessagePersistenceService;
            if (imp == null)
                return;

            Regex re = new Regex(@"^\vMSH\|(.*?\|){8}(.*?)\|");
            var match = re.Match(bufferData);
            if (match.Success)
            {
                string msgId = match.Groups[2].Value;


                // Now MSA?
                re = new Regex(@".*?MSA\|(.*?\|)(.*?)(\r|\|).*");
                match = re.Match(bufferData);
                if (match.Success)
                {
                    string rspId = match.Groups[2].Value;
                    imp.PersistResultMessage(msgId, rspId, new MemoryStream(System.Text.Encoding.UTF8.GetBytes(bufferData)));
                }
                else
                    imp.PersistMessage(msgId, new MemoryStream(System.Text.Encoding.UTF8.GetBytes(bufferData)));

            }
            else
            {
                re = new Regex(@"^(HTTP/1..|POST|PUT|CONNECT|GET|LOCK|UNLOCK|OPTIONS)\s(.*?)\s.*$", RegexOptions.Multiline);
                // Find the payload
                string payload = bufferData;
                if (re.IsMatch(payload))
                    payload = payload.Substring(bufferData.IndexOf("\r\n\r\n"));
                XmlDocument payloadDocument = new XmlDocument();
                try
                {
                    payloadDocument.LoadXml(payload);
                    XmlNode messageIdNode = payloadDocument.SelectSingleNode("//*[local-name() = 'MessageID' and namespace-uri() = 'http://www.w3.org/2005/08/addressing']"),
                        relatesToNode = payloadDocument.SelectSingleNode("//*[local-name() = 'RelatesTo'  and namespace-uri() = 'http://www.w3.org/2005/08/addressing']");
                    if (messageIdNode != null) // request
                        imp.PersistMessage(messageIdNode.InnerText, new MemoryStream(System.Text.Encoding.UTF8.GetBytes(payload)));
                    else if(relatesToNode != null)
                        imp.PersistResultMessage(Guid.NewGuid().ToString(), relatesToNode.InnerText, new MemoryStream(System.Text.Encoding.UTF8.GetBytes(payload)));
                }
                catch (Exception e)
                {
                    Trace.TraceError(e.ToString());
                    // Don't worry if not XML   
                }


            }

        }

        private IPEndPoint ReadDestination(String bufferData)
        {

            // Get the data
            Regex re = new Regex(@"^(POST|PUT|CONNECT|GET|LOCK|UNLOCK|OPTIONS)\s(.*?)\s.*$", RegexOptions.Multiline);
            var match = re.Match(bufferData);
            // TODO: Make this betterer!

            if (match.Success) // HTTP traffic!
            {
                Uri destUri = new Uri(match.Groups[2].Value, UriKind.Absolute);
                IPAddress dest = null;
                int port = 80;
                if (destUri.HostNameType == UriHostNameType.Dns)
                    dest = Dns.GetHostAddresses(destUri.Host)[0];// resolve host
                else
                    dest = IPAddress.Parse(destUri.Host);

                if (!destUri.IsDefaultPort)
                    port = destUri.Port;
                return new IPEndPoint(dest, port);
            }
            else
            {

                re = new Regex("^\vMSH\\|\\^\\~\\\\\\&\\|?.*\\|?.*\\|?(.*\\|.*)\\|?.*$", RegexOptions.Multiline);
                // TODO: Do a RECP|FAC lookup
                match = re.Match(bufferData);
                if (match.Success)
                    return new IPEndPoint(IPAddress.Parse("142.222.45.80"), 2100); // Get desintation
            }
            return null;
        }

        /// <summary>
        /// Start the TCP notification service
        /// </summary>
        public bool Start()
        {
            this.m_serverThread = new Thread(new ThreadStart(StartSocketService));
            this.m_serverThread.IsBackground = true;
            this.m_serverThread.Start();

            return true;
        }

        /// <summary>
        /// Stop the TCP notification service
        /// </summary>
        public bool Stop()
        {
            this.m_serverThread.Abort();
            return true;
        }

        #region IUsesHostContext Members

        public IServiceProvider Context
        {
            get;
            set;
        }

        #endregion

    }
}
