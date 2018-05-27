using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.IO;

namespace BloodDonation.Requirements
{
    public class Utilities
    {
        public void WriteEventLogRecord(string LogName, string Source, string LogMessage, EventLogEntryType LogType)
        {
            EventLog LogInstance;
            LogInstance = new EventLog();

            if (!System.Diagnostics.EventLog.SourceExists(Source))
            {
                System.Diagnostics.EventLog.CreateEventSource(Source, LogName);
            }
            LogInstance.Source = Source;
            LogInstance.WriteEntry(LogMessage, LogType);
            LogInstance.Dispose();
        }
    }
}