using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI315_Engine.Entity
{
    class B4
    {   
        //B4O1
        string special_handling_code { get; set; }
        //B402
        int inquiry_request_number { get; set; }
        //B403
        char[] shipment_status_code { get; set; }
        //B404
        string date { get; set; }
        //B405
        string status_time { get; set; }
        //B406
        string status_location { get; set; }
        //B407
        string equipment_initial { get; set; }
        //B408
        string equipment_number { get; set; }
        //B409
        string equipment_status_code { get; set; }
        //B410
        string equipment_type { get; set; }
        //B411
        string location_code { get; set; }
        //B412
        string location_identifier { get; set; }
        //B413
        string equipment_number_check_digit { get; set; }
    }
}
