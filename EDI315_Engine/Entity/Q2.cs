using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI315_Engine.Entity
{
    class Q2
    {
        int lading_quantity { get; set; }
        decimal weight { get; set; }
        decimal volume { get; set; }
        string vessel_code { get; set; }
        string vessel_name { get; set; }
        string voyage_number { get; set;}
        string scheduled_sailing_date { get; set; }
        string scheduled_discharge_date { get; set; }
        
    }
}
