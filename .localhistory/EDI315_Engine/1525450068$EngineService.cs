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

        public void test()
        {
            logFile.createLog();
        }
    }
}
