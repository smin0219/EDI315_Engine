using EDI315_Engine.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI315_Engine
{
    public class Util
    {
        protected DBContext context;

        string FileLocation = ""
        
        public Util()
        {
            context = new DBContext();
        }
        public bool dbConnectionCheck()
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
                catch(Exception ex)
                {
                    checkCount++;

                    // create text file
                    //string logMsg = "Function: dbConnectionCheck\r\nAttempt Count: " + checkCount + " / 5\r\nLog Message: " + ex.ToString();
                    //insertLog("DB", 0, 0, 0, logMsg);

                    // 30 min wait
                    System.Threading.Thread.Sleep((1000 * 60) * 30);
                }
            }
            return isConnected;
        }
        public void insertLog(string msgType, int msg_idnum, int EDI_idnum, int Detail_idnum, string logMsg)
        {
            if (dbConnectionCheck())
            {
                using (DBContext context = new DBContext())
                {
                    Pasring_Log dbLog = new Pasring_Log()
                    {
                        msg_type = msgType,
                        msg_idnum = (msg_idnum != 0 ? (int?)msg_idnum : null),
                        EDI_idnum = (EDI_idnum != 0 ? (int?)EDI_idnum : null),
                        Detail_idnum = (Detail_idnum != 0 ? (int?)Detail_idnum : null),
                        log_msg = logMsg,
                        created_date = DateTime.Now
                    };
                    context.Pasring_Log.Add(dbLog);
                    context.SaveChanges();
                    context.Dispose();
                }
            }
            else
            {
                // create text file

                if (!Directory.Exists(fileLocation))
                {
                    Directory.CreateDirectory(fileLocation);
                }
                if (!File.Exists(fullFilePathName))
                {
                    StreamWriter sw = File.CreateText(fullFilePathName);
                    sw.Close();
                }
            }
        }
    }
}
