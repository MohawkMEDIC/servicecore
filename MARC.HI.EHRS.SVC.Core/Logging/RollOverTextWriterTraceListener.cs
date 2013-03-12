/**
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
 * Date: 18-10-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MARC.HI.EHRS.SVC.Core.Logging
{
    /// <summary>
    /// Timed Trace listener
    /// </summary>
    public class RollOverTextWriterTraceListener : TraceListener
    {
        string _fileName;
        System.DateTime _currentDate;
        System.IO.StreamWriter _traceWriter;

        public RollOverTextWriterTraceListener(string fileName)
        {
            // Pass in the path of the logfile (ie. C:\Logs\MyAppLog.log)
            // The logfile will actually be created with a yyyymmdd format appended to the filename
            _fileName = fileName;
            _traceWriter = new StreamWriter(generateFilename(), true);
            _traceWriter.AutoFlush = true;
        }

        public override void Write(string value)
        {
            checkRollover();
            _traceWriter.Write("{1}", DateTime.Now, value);
        }

        public override void WriteLine(string value)
        {
            checkRollover();
            _traceWriter.WriteLine("{0}:{1}", DateTime.Now, value);
            
        }

        private string generateFilename()
        {
            _currentDate = System.DateTime.Today;

            return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
               Path.GetFileNameWithoutExtension(_fileName) + "_" +
               _currentDate.ToString("yyyyMMdd") + Path.GetExtension(_fileName));
        }

        private void checkRollover()
        {
            // If the date has changed, close the current stream and create a new file for today's date
            if (_currentDate.CompareTo(System.DateTime.Today) != 0)
            {
                _traceWriter.Close();
                _traceWriter = new StreamWriter(generateFilename(), true);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _traceWriter.Close();
            }
        }

    }
}
