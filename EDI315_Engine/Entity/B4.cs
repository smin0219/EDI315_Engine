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
        public string special_handling_code { get; set; }
        //B402
        public int inquiry_request_number { get; set; }
        //B403
        public string shipment_status_code { get; set; }
        //B404
        public DateTime shipment_status_datetime { get; set; }
        //B405
        public string status_time { get; set; }
        //B406
        public string status_location { get; set; }
        //B407
        public string equipment_initial { get; set; }
        //B408
        public string equipment_number { get; set; }
        //B409
        public string equipment_status_code { get; set; }
        //B410
        public string equipment_type { get; set; }
        //B411
        public string location_identifier { get; set; }
        //B412
        public string location_qualifier { get; set; }
        //B413
        public string equipment_number_check_digit { get; set; }
    }
}
