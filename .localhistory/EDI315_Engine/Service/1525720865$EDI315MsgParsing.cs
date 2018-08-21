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
            
            try
            {
                #region Initialize values
                string[] msgBody;
                string[] rowBody;
                string header;

                #endregion

                orgMsg = Regex.Replace(orgMsg, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
                msgBody = orgMsg.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                foreach(string row in msgBody)
                {
                    if(row != string.Empty && row.Count() > 0)
                    {
                        rowBody = row.Split('*');
                        header = row[0];

                        switch()
    
                    }
                    }

            }
            catch(Exception ex)
            {

                result = false;
            }

            return result;
        }
    }
}
