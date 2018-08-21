using EDI315_Engine.Context;
using EDI315_Engine.Service;
using System;
using System.Collections.Generic;
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

        public void RunEngine()
        {
            List<EDI_Messages> ediMsgList = getMessages();

            if(ediMsgList != null && ediMsgList.Count() > 0)
            {
                foreach(EDI_Messages dbRow in ediMsgList)
                {
                    bool result = edi315MsgParsing.MeesageParsing(dbRow.msg_body, dbRow.msg_idnum);

                    if(result)
                    {
                        updateMSGStatus(dbRow, dbRow.msg_idnum, "Y");
                        addListItemMainForm(dbRow .file_name + " proccessed - " + DateTime.Now);
                    }
                    else
                    {
                        updateMSGStatus(dbRow, dbRow.msg_idnum, "N");
                        addListItemMainForm(dbRow.file_name + " error - " + DateTime.Now);
                    }
                }
            }
            else
            {
                addListItemMainForm("No message - " + DateTime.Now);
            }
        }

        private List<EDI_Messages> getMessages()
        {
            List<EDI_Messages> list = new List<EDI_Messages>();
            try
            {
                if(util.dbConnectionCheck())
                {
                    using (DBContext context = new DBContext())
                    {
                        list = context.EDI_Messages.Where(x => x.msg_type == "315" && x.process_status == "N").ToList();
                        context.Dispose();
                    }
                }
                else
                {
                    string logMsg = "Function: getMessages\r\n";
                    logMsg += "Error Message: Not able to access DB.\r\n";
                    util.insertLog("315", 0, 0, 0, logMsg);
                }
            }
            catch (DbEntityValidationException ex)
            {
                string logMsg = "Function: getMessages\r\nError Message: " + ex.Message;
                util.insertLog("315", 0, 0, 0, logMsg);
            }
            catch (Exception ex)
            {
                util.insertLog("315", 0, 0, 0, ex.ToString());
                list = new List<EDI_Messages>();
            }
            return list;
        }
        private void updateMSGStatus(EDI_Messages dbEDIMessage, int msg_idnum, string status)
        {
            try
            {
                if (util.dbConnectionCheck())
                {
                    using (DBContext context = new DBContext())
                    {
                        dbEDIMessage.process_status = status;
                        context.Dispose();
                    }
                }
                else
                {
                    string logMsg = "Function: updateMSGStatus\r\n";
                    logMsg += "Error Message: Not able to access DB.\r\n";
                    util.insertLog("315", 0, 0, 0, logMsg);

                    edi315MsgParsing.rollbackProcess(msg_idnum);
                }
            }
            catch (DbEntityValidationException ex)
            {
                string logMsg = "Function: updateMSGStatus\r\nError Message: " + ex.Message;
                util.insertLog("315", msg_idnum, 0, 0, logMsg);
            }
            catch (Exception ex)
            {
                string logMsg = "Function: updateMSGStatus\r\nError Message: " + ex.ToString();
                util.insertLog("315", msg_idnum, 0, 0, logMsg);
            }
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
