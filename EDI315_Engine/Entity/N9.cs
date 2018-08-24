using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI315_Engine.Entity
{
    class N9
    {
        //N901
        char[] reference_identification_qualifier { get; set; }
        //N902
        string reference_identification { get; set; }
        //N903
        string free_form_description { get; set; }
        //N904
        string date { get; set; }
        //N905
        string time { get; set; }
        //N906
        string time_code { get; set; }
    }
}
