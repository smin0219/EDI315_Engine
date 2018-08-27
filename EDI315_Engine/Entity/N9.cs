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
        public string reference_identification_qualifier { get; set; }
        //N902
        public string reference_identification { get; set; }
        //N903
        public string free_form_description { get; set; }
        //N904
        public string date { get; set; }
        //N905
        public string time { get; set; }
        //N906
        public string time_code { get; set; }
    }
}
