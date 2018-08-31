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
            bool result = false;
            int idnum = 0;
            List<EDI_Messages> ediMsgList = GetMsg(msgType);

            if (ediMsgList != null && ediMsgList.Count() > 0)
            {
                foreach(EDI_Messages row in ediMsgList)
                {
                    idnum = row.msg_idnum;
                    result = messageParsing.ParseMessage(msgType, row.msg_body, row.msg_idnum);
                }

                if(result == true)
                {
                    addListItemMainForm("Message parsed successfully. - " + DateTime.Now);
                    UpdateMsgStatus(idnum, msgType, "Y");
                }
                else
                {
                    addListItemMainForm("Error occured while parsing a message. - " + DateTime.Now);
                }
            }
            else
            {
                addListItemMainForm("There is no message. - " + DateTime.Now);
            }
            
        }
        private List<EDI_Messages> GetMsg(string msgType)
        {
            List<EDI_Messages> list = new List<EDI_Messages>();

            if (util.checkDBConnection())
            {
                using(DBContext context = new DBContext())
                {
                    list = context.EDI_Messages.Where(x => x.msg_type == msgType && x.process_status == "N").ToList();
                    context.Dispose();
                }
            }
            return list;
        }
        private void UpdateMsgStatus(int msg_idnum, string msgType, string status)
        {
            try
            {
                if (util.checkDBConnection())
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
                    logMsg += "\r\n\r\n=====================================================================";
                    logMsg += "=============================================================================";
                    logMsg += "=============================================================================";
                    util.insertLog_text(logMsg);
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

                util.insertLog_DB(msgType, msg_idnum, 0, 0, logMsg);
                #endregion
            }
            catch (Exception ex)
            {
                #region Exception
                string logMsg = "Date: " + DateTime.Now.ToString();
                logMsg += "\r\nFunction: updateMSGStatus";
                logMsg += "\r\nError Message: \r\n";
                logMsg += ex.ToString();
                util.insertLog_DB(msgType, msg_idnum, 0, 0, logMsg);
                #endregion
            }
        }
    }
}
