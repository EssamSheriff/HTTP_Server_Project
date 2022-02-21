using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Logger
    {
        static StreamWriter sr = new StreamWriter("log.txt");
        public static void LogException(Exception ex)
        {
            // TODO: Create log file named log.txt to log exception details in it
            DateTime dateTime = DateTime.Now;
            //Datetime:
            sr.WriteLine("Date: {0}/{1}/{2}",
                      dateTime.Day, dateTime.Month,
                      dateTime.Year);
            sr.WriteLine("Time: {0}:{1}:{2}",
                      dateTime.Hour, dateTime.Minute,
                      dateTime.Second);
            //message:
            sr.WriteLine(ex.Message);
            sr.WriteLine("****************");
            sr.Flush();

            // for each exception write its details associated with datetime

        }
    }
}
