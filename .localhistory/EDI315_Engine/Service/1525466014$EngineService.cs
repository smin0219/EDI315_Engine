using EDI315_Engine.Context;
using EDI315_Engine.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI315_Engine
{
    public class EngineService
    {
        private LogFile logFile = new LogFile();
        private EDI315MsgParsing edi315MsgParsing = new EDI315MsgParsing();

        private DBContext context = new DBContext();

        mainForm mainForm;
        public EngineService(mainForm _mainForm)
        {
            mainForm = _mainForm;
        }

        public void test()
        {
            logFile.createLog();
            form_main.addListBox("");
        }
        public void RunEngine()
        {
            List<EDI_Messages> ediMsgList = getMessages();
            if(ediMsgList != null && ediMsgList.Count() > 0)
            {
                foreach(EDI_Messages dbRow in ediMsgList)
                {
                    bool result = edi315MsgParsing.MeesageParsing(dbRow.msg_body);

                    // if process done, update status
                    if(result)
                        updateMSGStatus(dbRow);
                }
            }
        }


        private List<EDI_Messages> getMessages()
        {
            List<EDI_Messages> list = new List<EDI_Messages>();
            try
            {
                using (DBContext context = new DBContext())
                {
                    list = context.EDI_Messages.Where(x => x.msg_type == "315" && x.process_status == "N").ToList();

                    context.Dispose();
                }
            }
            catch(Exception ex)
            {
                list = new List<EDI_Messages>();
            }
            return list;
        }
        private void updateMSGStatus(EDI_Messages dbEDIMessage)
        {
            try
            {
                using (DBContext context = new DBContext())
                {
                    dbEDIMessage.process_status = "Y";
                    context.Dispose();
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
