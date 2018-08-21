﻿using EDI315_Engine.Context;
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

        string FileLocation = @"C:\Engines_Log";
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

                    string logMsg = "Date: " + DateTime.Now.ToString();
                    logMsg += "\r\nFunction: dbConnectionCheck";
                    logMsg += "\r\nAttempt Count: " + checkCount + " / 5";

                    foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                    {
                        // Get entry
                        DbEntityEntry entry = item.Entry;
                        string entityTypeName = entry.Entity.GetType().Name;

                        foreach (DbValidationError subItem in item.ValidationErrors)
                        {
                            logMsg += string.Format("\r\nError '{0}' occurred in {1} at {2}", subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                        }
                    }
                    insertLog_TextFile("315", 0, 0, 0, logMsg);

                    // 30 min wait
                    System.Threading.Thread.Sleep((1000 * 60) * 30);
                }
                catch (Exception ex)
                {
                    checkCount++;

                    // create text file
                    string logMsg = "Function: dbConnectionCheck\r\n";
                    logMsg += "Attempt Count: " + checkCount + " / 5\r\n";
                    logMsg += "Error Message: \r\n" + ex.ToString() + "\r\n";
                    insertLog_TextFile("315", 0, 0, 0, logMsg);

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
                logMsg += "\r\nValues Info:";
                logMsg += "\r\nmstType: " + msgType;

                if (msg_idnum != 0)
                    logMsg += "\r\nmsg_idnum: " + msg_idnum;

                if (EDI_idnum != 0)
                    logMsg += "\r\nEDI_idnum: " + EDI_idnum;

                if (Detail_idnum != 0)
                    logMsg += "\r\nDetail_idnum: " + Detail_idnum;

                insertLog_TextFile(msgType, msg_idnum, EDI_idnum, Detail_idnum, logMsg);
            }
        }

        public void insertLog_TextFile(string msgType, int msg_idnum, int EDI_idnum, int Detail_idnum, string logMsg)
        {
            // create text file
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
                sw.Write(logMsg);
                sw.Close();
            }
        }
    }
}
