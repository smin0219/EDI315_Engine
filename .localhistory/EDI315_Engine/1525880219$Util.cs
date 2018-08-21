using EDI315_Engine.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI315_Engine
{
    public class Util
    {
        protected DBContext context;

        string FileLocation = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
        string FileName = "log_";

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
                FileName += DateTime.Today.ToString("yyyy_MM_dd");

                if (!Directory.Exists(FileLocation))
                {
                    Directory.CreateDirectory(FileLocation);
                }
                if (!File.Exists(FileName))
                {
                    StreamWriter sw = File.CreateText(FileName);
                    sw.Close();
                }

                using (StreamWriter sw = new StreamWriter(fullFilePathName, true, Encoding.GetEncoding("iso-8859-1")))
                {
                    sw.Write(txtBody);
                    sw.Close();
                }
            }
        }
    }
}
