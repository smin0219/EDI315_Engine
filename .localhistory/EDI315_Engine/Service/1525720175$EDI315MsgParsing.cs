using EDI315_Engine.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI315_Engine.Service
{
    public class EDI315MsgParsing
    {
        protected LogFile logFile;
        protected DBContext context;

        public EDI315MsgParsing()
        {
            logFile = new LogFile();
            context = new DBContext();
        }
        public bool MeesageParsing(string msgBody)
        {
            bool result = false;

            try
            {

            }
            catch(Exception ex)
            {

                result = false;
            }

            return result;
        }
    }
}
