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
        protected EDI315MsgParsing edi315MsgParsing;
        protected Util util;
        protected DBContext context;

        // to control Listview in mainForm 
        public delegate void addListBoxItem(string strItem);
        public event addListBoxItem addListItemMainForm;

        public EngineService()
        {
            util = new Util();
            edi315MsgParsing = new EDI315MsgParsing();
            context = new DBContext();
        }

        public void RunEngine(string msgType)
        {
            List<EDI_Messages> ediMsgList = getMessages(msgType);

            if(ediMsgList != null && ediMsgList.Count() > 0)
            {
                foreach(EDI_Messages dbRow in ediMsgList)
                {
                    // clear process before
                    edi315MsgParsing.rollbackProcess(dbRow.msg_idnum, dbRow.msg_type, "");
                    bool result = edi315MsgParsing.MeesageParsing(msgType, dbRow.msg_body, dbRow.msg_idnum);

                    if(result)
                    {
                        updateMSGStatus(dbRow, dbRow.msg_idnum, msgType, "Y");
                        addListItemMainForm(dbRow .file_name + " proccessed - " + DateTime.Now);
                    }
                    else
                    {
                        updateMSGStatus(dbRow, dbRow.msg_idnum, msgType, "E");
                        addListItemMainForm(dbRow.file_name + " error - " + DateTime.Now);
                    }
                }
            }
            else
            {
                addListItemMainForm("No message - " + DateTime.Now);
            }
        }

        private List<EDI_Messages> getMessages(string msgType)
        {
            List<EDI_Messages> list = new List<EDI_Messages>();
            try
            {
                if(util.dbConnectionCheck())
                {
                    using (DBContext context = new DBContext())
                    {
                        list = context.EDI_Messages.Where(x => x.msg_type == msgType && x.process_status == "N").ToList();

                        /* DB context. END. */
                        context.Dispose();
                    }
                }
                else
                {
                    string logMsg = "Date: " + DateTime.Now.ToString();
                    logMsg += "\r\nFunction: getMessages";
                    logMsg += "\r\nError Message: Not able to access DB.";
                    util.insertLog_TextFile(msgType, 0, 0, 0, logMsg);
                }
            }
            catch (DbEntityValidationException ex)
            {
                string logMsg = "Date: " + DateTime.Now.ToString();
                logMsg += "\r\nFunction: getMessages";
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

                util.insertLog(msgType, 0, 0, 0, logMsg);
            }
            catch (Exception ex)
            {
                string logMsg = "Date: " + DateTime.Now.ToString();
                logMsg += "\r\nFunction: getMessages";
                logMsg += "\r\nError Message: \r\n";
                logMsg += ex.ToString();

                util.insertLog(msgType, 0, 0, 0, logMsg);
                list = new List<EDI_Messages>();
            }
            return list;
        }
        private void updateMSGStatus(EDI_Messages dbEDIMessage, int msg_idnum, string msgType, string status)
        {
            try
            {
                if (util.dbConnectionCheck())
                {
                    using (DBContext context = new DBContext())
                    {
                        dbEDIMessage.process_status = status;

                        /* DB context. END. */
                        context.Dispose();
                    }
                }
                else
                {
                    string logMsg = "Date: " + DateTime.Now.ToString();
                    logMsg += "\r\nFunction: updateMSGStatus";
                    logMsg += "\r\nError Message: Not able to access DB.";
                    util.insertLog_TextFile(msgType, 0, 0, 0, logMsg);

                    edi315MsgParsing.rollbackProcess(msg_idnum, msgType, "");
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
                edi315MsgParsing.rollbackProcess(msg_idnum, msgType, "");
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
                edi315MsgParsing.rollbackProcess(msg_idnum, msgType, "");
                #endregion
            }
        }
        private void checkDataExists(int msg_idnum, string msg_type)
        {

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
