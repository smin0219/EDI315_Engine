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
        protected DBContext context;

        public EDI315MsgParsing()
        {
            context = new DBContext();
        }
        public bool MeesageParsing(string msgBody)
        {
            bool result = false;

            try
            {

            }
            catch
            {
                result false;
            }

            return result;
        }
    }
}
