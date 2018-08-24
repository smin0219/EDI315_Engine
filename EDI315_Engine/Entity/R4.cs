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
        string port_terminal_function_code { get; set; }
        //R402
        char[] location_qualifier { get; set; }
        //R403
        string location_identifier { get; set; }
        //R404
        string port_name { get; set; }
        //R405
        char[] country_code { get; set; }
        //R408
        char[] state_province_code { get; set; }
        //DTM
        DTM dtm { get; set; }
    }
}
