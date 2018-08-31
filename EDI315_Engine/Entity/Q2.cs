using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI315_Engine.Entity
{
    class Q2
    {
        //Q201
        public string vessel_code { get; set; }
        //Q202
        public string country_code { get; set; }
        //Q203
        public string date { get; set; }
        //Q204
        public string scheduled_sailing_date { get; set; }
        //Q205
        public string scheduled_discharge_date { get; set; }
        //Q206
        public int landing_quantity { get; set; }
        //Q207
        public decimal weight { get; set; }
        //Q208
        public string weight_qualifier { get; set; }
        //Q209
        public string voyage_number { get; set; }
        //Q210
        public string reference_identification_qualifier { get; set; }
        //Q211
        public string reference_identification { get; set; }
        //Q212
        public string vessel_code_qualifier { get; set; }
        //Q213
        public string vessel_name { get; set; }
        //Q214
        public decimal volume { get; set; }
        //Q215
        public string volume_unit_qualifier { get; set; }
        //Q216
        public string weight_unit_code { get; set; }
    }
}
