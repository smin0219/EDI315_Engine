using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI315_Engine.Entity
{
    class R4
    {
        //R401
        public string port_terminal_function_code { get; set; }
        //R402
        public string location_qualifier { get; set; }
        //R403
        public string location_identifier { get; set; }
        //R404
        public string port_name { get; set; }
        //R405
        public string country_code { get; set; }
        //R408
        public string state_province_code { get; set; }
        //DTM
        public DTM dtm { get; set; }
    }
}
