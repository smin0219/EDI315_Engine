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
        int vessel_code { get; set; }
        //Q202
        char[] country_code { get; set; }
        //Q203
        string date { get; set; }
        //Q204
        string scheduled_sailing_date { get; set; }
        //Q205
        string scheduled_discharge_date { get; set; }
        //Q206
        int landing_quantity { get; set; }
        //Q207
        decimal weight { get; set; }
        //Q208
        string weight_qualifier { get; set; }
        //Q209
        string voyage_number { get; set; }
        //Q210
        string reference_identification_qualifier { get; set; }
        //Q211
        string reference_identification { get; set; }
        //Q212
        string vessel_code_qualifier { get; set; }
        //Q213
        string vessel_name { get; set; }
        //Q214
        decimal volume { get; set; }
        //Q215
        string volume_unit_qualifier { get; set; }
        //Q216
        string weight_unit_code { get; set; }
    }
}
