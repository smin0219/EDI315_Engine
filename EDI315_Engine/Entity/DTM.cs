using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI315_Engine.Entity
{
    class DTM
    {
        //DTM01
        int date_time_qualifier { get; set; }
        //DTM02
        string date { get; set; }
        //DTM03
        string time { get; set; }
        //DTM04
        char[] time_code { get; set; }
    }
}
