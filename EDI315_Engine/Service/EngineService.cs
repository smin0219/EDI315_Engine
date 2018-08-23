using EDI315_Engine.Context;
using EDI315_Engine.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI315_Engine
{
    public class EngineService
    {
        protected MessageParsing messageParsing;
        protected Util util;
        protected DBContext context;

        // to control Listview in mainForm 
        public delegate void addListBoxItem(string strItem);
        public event addListBoxItem addListItemMainForm;

        public EngineService()
        {
            util = new Util();
            messageParsing = new MessageParsing();
            context = new DBContext();
        }

        public void RunEngine(string msgType)
        {
            List<EDI_Messages> ediMsgList = GetMessage(msgType);
        }

        private List<EDI_Messages> GetMessage(string msgType)
        {
            List<EDI_Messages> list = new List<EDI_Messages>();

            try
            {

            }
            catch
            {

            }
            return list;
        }
        private void updateMSGStatus(int msg_idnum, string msgType, string status)
        {
            try
            {
                if (util.dbConnectionCheck())
                {
                    using (DBContext context = new DBContext())
                    {
                        EDI_Messages dbEdiMsg = context.EDI_Messages.Where(x => x.msg_idnum == msg_idnum).FirstOrDefault();
                        if(dbEdiMsg != null && dbEdiMsg.msg_idnum != 0)
                        {
                            dbEdiMsg.process_status = status;
                            context.SaveChanges();
                        }

                        /* DB context. END. */
                        context.Dispose();
                    }
                }
                else
                {
                    string logMsg = "Date: " + DateTime.Now.ToString();
                    logMsg += "\r\nFunction: updateMSGStatus";
                    logMsg += "\r\nError Message: Not able to access DB.";
                    util.insertLog_TextFile(logMsg);
                }
            }
            catch (DbEntityValidationException ex)
            {
                #region Exception
                string logMsg = "Date: " + DateTime.Now.ToString();
                logMsg += "\r\nFunction: updateMSGStatus";
                logMsg += "\r\nError Message: ";

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

                util.insertLog(msgType, msg_idnum, 0, 0, logMsg);
                messageParsing.rollbackProcess(msg_idnum, msgType, "");
                #endregion
            }
            catch (Exception ex)
            {
                #region Exception
                string logMsg = "Date: " + DateTime.Now.ToString();
                logMsg += "\r\nFunction: updateMSGStatus";
                logMsg += "\r\nError Message: \r\n";
                logMsg += ex.ToString();
                util.insertLog(msgType, msg_idnum, 0, 0, logMsg);
                messageParsing.rollbackProcess(msg_idnum, msgType, "");
                #endregion
            }
        }
        private void checkDataExists(int msg_idnum, string msg_type)
        {
            try
            {
                using (DBContext context = new DBContext())
                {
                    if(context.EDI315.Where(x=>x.msg_idnum == msg_idnum).Any())
                        messageParsing.rollbackProcess(msg_idnum, msg_type, "");

                    context.Dispose();
                }
            }
            catch { }
        }

        public void test()
        {
            // if process done, update status
            for (int i = 0; i < 3; i++)
            {
                util.insertLog("315", 0, 0, 0, "error message here");
                addListItemMainForm(" proccessed: " + DateTime.Now);
                System.Threading.Thread.Sleep(5000);
            }
        }
    }
}
