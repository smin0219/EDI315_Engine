using EDI315_Engine.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI315_Engine
{
    public class Util
    {
        protected DBContext context;

        public Util()
        {
            context = new DBContext();
        }

        /// <summary>
        /// Check DB Connection. If there is a connection problem, 
        /// leave log messages and try to connect again after 30 min.
        /// </summary>
        /// <returns>True: If it is connected, otherwise return False</returns>

        public bool checkDBConnection()
        {
            bool isConnected = false;
            int checkCount = 0;

            while (checkCount < 5)
            {
                try
                {
                    using (DBContext context = new DBContext())
                    {
                        context.Database.Connection.Open();
                        context.Dispose();

                        isConnected = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    checkCount++;

                    // Create a text file if it does not exist to leave log.
                    string logMsg = "Date: " + DateTime.Now.ToString();
                    logMsg += "\r\nFunction: checkDBConnection";
                    logMsg += "\r\nAttempt Count: " + checkCount + " / 5";
                    logMsg += "\r\nError Message: \r\n" + ex.ToString();
                    logMsg += "\r\n\r\n=====================================================================";
                    logMsg += "=============================================================================";
                    logMsg += "=============================================================================";
                    insertLog_text(logMsg);

                    // Wait for 30 min and try again
                    //System.Threading.Thread.Sleep((1000 * 60) * 30);
                }
            }
            return isConnected;
        }

        /// <summary>
        /// Insert log messages to the DB
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="msg_idnum"></param>
        /// <param name="EDI_idnum"></param>
        /// <param name="Detail_idnum"></param>
        /// <param name="logMsg"></param>

        public void insertLog_DB(string msgType, int msg_idnum, int EDI_idnum, int Detail_idnum, string logMsg)
        {
            if (checkDBConnection())
            {
                using (DBContext context = new DBContext())
                {
                    Engines_Log DBLog = new Engines_Log()
                    {
                        msg_type = msgType,
                        msg_idnum = (msg_idnum != 0 ? (int?)msg_idnum : null),
                        EDI_idnum = (EDI_idnum != 0 ? (int?)EDI_idnum : null),
                        Detail_idnum = (Detail_idnum != 0 ? (int?)Detail_idnum : null),
                        log_msg = logMsg,
                        created_date = DateTime.Now
                    };
                    context.Engines_Log.Add(DBLog);
                    context.SaveChanges();
                    context.Dispose();
                }
            }
        }

        /// <summary>
        /// Create a text file (if it does not exist) and insert log messages
        /// into the file.
        /// </summary>
        /// <param name="logMsg">Log message to insert into the created/exist file</param>

        public void insertLog_text(string logMsg)
        {
            string FileLocation = @"C:\Engines_Log";
            string FileName = "315_log_";

            FileName += DateTime.Today.ToString("yyyy_MM_dd") + ".txt";

            string filePathName = Path.Combine(FileLocation, FileName);
            if (!Directory.Exists(FileLocation))
            {
                Directory.CreateDirectory(FileLocation);
            }
            if (!File.Exists(filePathName))
            {
                StreamWriter sw = File.CreateText(filePathName);
                sw.Close();
            }

            using (StreamWriter sw = new StreamWriter(filePathName, true, Encoding.GetEncoding("iso-8859-1")))
            {
                sw.Write(logMsg + Environment.NewLine);
                sw.Close();
            }
        }

        public string buildLogMsg(string function, string errMsg)
        {
            string logMsg = "";

            logMsg += "Date: " + DateTime.Now.ToString();
            logMsg += "\r\nFunction: "+function;
            logMsg += "\r\nError Message: \r\n" + errMsg;
            logMsg += "\r\n\r\n=====================================================================";
            logMsg += "=============================================================================";
            logMsg += "=============================================================================";

            return logMsg;
        }
    }
}
