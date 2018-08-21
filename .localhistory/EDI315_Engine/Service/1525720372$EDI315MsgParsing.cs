using EDI315_Engine.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public bool MeesageParsing(string orgMsg)
        {
            bool result = false;
            string[] 

            msgBody = Regex.Replace(msgBody, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
            
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
