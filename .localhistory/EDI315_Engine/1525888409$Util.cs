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

        string FileLocation = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath) + @"\Log";
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
                catch (DbEntityValidationException ex)
                {
                    checkCount++;

                    string logMsg = "Function: dbConnectionCheck\r\n";
                    logMsg += "Attempt Count: " + checkCount + " / 5\r\n";

                    foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                    {
                        // Get entry
                        DbEntityEntry entry = item.Entry;
                        string entityTypeName = entry.Entity.GetType().Name;

                        foreach (DbValidationError subItem in item.ValidationErrors)
                        {
                            logMsg += string.Format("Error '{0}' occurred in {1} at {2}", subItem.ErrorMessage, entityTypeName, subItem.PropertyName) + "\r\n";
                        }
                    }
                    insetLog_TextFile("315", 0, 0, 0, logMsg);

                    // 30 min wait
                    System.Threading.Thread.Sleep((1000 * 60) * 30);
                }
                catch (Exception ex)
                {
                    checkCount++;

                    // create text file
                    string logMsg = "Function: dbConnectionCheck\r\n";
                    logMsg += "Attempt Count: " + checkCount + " / 5\r\n";
                    logMsg += "Log Message: " + ex.ToString() + "\r\n";
                    insetLog_TextFile("315", 0, 0, 0, logMsg);

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
                    Engine_Log dbLog = new Engine_Log()
                    {
                        msg_type = msgType,
                        msg_idnum = (msg_idnum != 0 ? (int?)msg_idnum : null),
                        EDI_idnum = (EDI_idnum != 0 ? (int?)EDI_idnum : null),
                        Detail_idnum = (Detail_idnum != 0 ? (int?)Detail_idnum : null),
                        log_msg = logMsg,
                        created_date = DateTime.Now
                    };
                    context.Engine_Log.Add(dbLog);
                    context.SaveChanges();
                    context.Dispose();
                }
            }
            else
            {
                insetLog_TextFile(msgType, msg_idnum, EDI_idnum, Detail_idnum, logMsg);
            }
        }

        private void insetLog_TextFile(string msgType, int msg_idnum, int EDI_idnum, int Detail_idnum, string logMsg)
        {
            // create text file
            FileName += DateTime.Today.ToString("yyyy_MM_dd") + ".txt";

            string textHeader = "Date: " + DateTime.Now.ToString();
            textHeader += "\r\nmstType: " + msgType;
            textHeader += "\r\nmsg_idnum: ";
            if (msg_idnum != 0)
                textHeader += msg_idnum;

            textHeader += "\r\nEDI_idnum: ";
            if (EDI_idnum != 0)
                textHeader += EDI_idnum;

            textHeader += "\r\nDetail_idnum: ";
            if (Detail_idnum != 0)
                textHeader += Detail_idnum;

            logMsg = textHeader + "\r\n" + "Error msg: " + logMsg + "\r\n";
            logMsg += "\r\n\r\n";

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
                sw.Write(logMsg);
                sw.Close();
            }
        }
    }
}
