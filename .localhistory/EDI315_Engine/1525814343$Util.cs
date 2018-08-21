using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI315_Engine
{
    public class Util
    {
        public Util()
        {
            util = new Util();
            edi315MsgParsing = new EDI315MsgParsing();
            context = new DBContext();
        }
        public void createLog()
        {

        }
    }
}
