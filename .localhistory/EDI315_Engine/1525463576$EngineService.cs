﻿using EDI315_Engine.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI315_Engine
{
    public class EngineService
    {
        protected LogFile logFile = new LogFile();
        protected DBContext context = new DBContext();

        public void test()
        {
            logFile.createLog();
        }
        public void RunEngine()
        {

        }
        private List<EDI_Messages> getMessages()
        {
            List<EDI_Messages> list = context.EDI_Messages.Where(x=>x.msg_type == "315" && x.process_status == "N")
        }
    }
}
