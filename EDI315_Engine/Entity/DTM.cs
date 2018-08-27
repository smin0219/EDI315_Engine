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
        public int date_time_qualifier { get; set; }
        //DTM02
        public string date { get; set; }
        //DTM03
        public string time { get; set; }
        //DTM04
        public string time_code { get; set; }
    }
}
