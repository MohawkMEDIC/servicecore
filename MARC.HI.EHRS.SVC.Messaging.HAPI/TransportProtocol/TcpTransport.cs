﻿using MARC.HI.EHRS.SVC.Messaging.HAPI.Configuration;

/*
 * Copyright 2012-2013 Mohawk College of Applied Arts and Technology
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you
 * may not use this file except in compliance with the License. You may
 * obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 *
 * User: fyfej
 * Date: 13-8-2012
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MARC.HI.EHRS.SVC.Messaging.HAPI.TransportProtocol
{
	/// <summary>
	/// Transport protocol for TCP
	/// </summary>
	[Description("ER7 over TCP")]
	public class TcpTransport : ITransportProtocol
	{
		#region ITransportProtocol Members

		// The socket
		private TcpListener m_listener;

		// Will run while true
		private bool m_run = true;

		/// <summary>
		/// Message has been received
		/// </summary>
		public event EventHandler<Hl7MessageReceivedEventArgs> MessageReceived;

		/// <summary>
		/// An enumeration of valid bytes
		/// </summary>
		public enum ByteType : byte
		{
			EOT = 0x04,
			ENQ = 0x05,
			ACK = 0x06,
			BEL = 0x07,
			BS = 0x08,
			TAB = 0x09,
			LF = 0x0a,
			VTAB = 0x0b,
			CR = (byte)'\r',
			NACK = 0x15,
			EOF = 0x1a,
			ESC = 0x1b,
			FS = 0x1c
		}

		/// <summary>
		/// Gets the name of the protocol
		/// </summary>
		public string ProtocolName
		{
			get { return "tcp"; }
		}

		/// <summary>
		/// Setup configuration
		/// </summary>
		public void SetupConfiguration(ServiceDefinition definition)
		{
		}

		/// <summary>
		/// Start the handler
		/// </summary>
		public void Start(IPEndPoint bind, ServiceHandler handler)
		{
			this.m_listener = new TcpListener(bind);
			this.m_listener.Start();
			Trace.TraceInformation("TCP Transport bound to {0}", bind);

			while (m_run) // run the service
			{
				// Client
				TcpClient client = this.m_listener.AcceptTcpClient();
				Thread clientThread = new Thread(ReceiveMessage);
				clientThread.IsBackground = true;
				clientThread.Start(client);
			}
		}

		/// <summary>
		/// Stop the thread
		/// </summary>
		public void Stop()
		{
			this.m_run = false;
			this.m_listener.Stop();
			Trace.TraceInformation("TCP Transport stopped");
		}

		/// <summary>
		/// Receive and process message
		/// </summary>
		private void ReceiveMessage(object client)
		{
			TcpClient tcpClient = client as TcpClient;
			NetworkStream stream = tcpClient.GetStream();
			try
			{
				// Now read to a string
				NHapi.Base.Parser.PipeParser parser = new NHapi.Base.Parser.PipeParser();

				StringBuilder messageData = new StringBuilder();
				byte[] buffer = new byte[1024];
				while (stream.DataAvailable)
				{
					int br = stream.Read(buffer, 0, 1024);
					messageData.Append(Encoding.ASCII.GetString(buffer, 0, br));
				}

				var message = parser.Parse(messageData.ToString());
				var localEp = tcpClient.Client.LocalEndPoint as IPEndPoint;
				var remoteEp = tcpClient.Client.RemoteEndPoint as IPEndPoint;
				Uri localEndpoint = new Uri(String.Format("tcp://{0}:{1}", localEp.Address, localEp.Port));
				Uri remoteEndpoint = new Uri(String.Format("tcp://{0}:{1}", remoteEp.Address, remoteEp.Port));
				var messageArgs = new Hl7MessageReceivedEventArgs(message, localEndpoint, remoteEndpoint, DateTime.Now);

				this.MessageReceived(this, messageArgs);

				// Send the response back
				StreamWriter writer = new StreamWriter(stream);
				if (messageArgs.Response != null)
				{
					writer.Write(parser.Encode(messageArgs.Response));
					writer.Flush();
				}
			}
			catch (Exception e)
			{
				Trace.TraceError(e.ToString());
				// TODO: NACK
			}
			finally
			{
				stream.Close();
			}
		}

		#endregion ITransportProtocol Members

		#region ITransportProtocol Members

		/// <summary>
		/// Configuration options
		/// </summary>
		public object ConfigurationObject
		{
			get { return null; }
		}

		/// <summary>
		/// Serialize configuration
		/// </summary>
		public List<KeyValuePair<String, String>> SerializeConfiguration()
		{
			return new List<KeyValuePair<string, string>>();
		}

		#endregion ITransportProtocol Members
	}
}