using EDI315_Engine.Context;
using EDI315_Engine.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EDI315_Engine.Service
{
    public class MessageParsing
    {
        protected Util util = null;
        protected DBContext context = null;

        B4 b4 = null;
        Q2 q2 = null;
        List<N9> n9_list = null;
        List<R4> r4_list = null;
        List<string> EDI315_headers = new List<string> { "ISA", "GS", "ST", "B4", "N9", "Q2", "R4", "DTM", "SE", "GE", "IEA" };
        public MessageParsing()
        {
            util = new Util();
            context = new DBContext();
        }
        /// <summary>
        /// Parse the message from EDI_Message and insert into entities.
        /// </summary>
        /// <param name="msg_type">Hard coded as "315"</param>
        /// <param name="msg_body">Message body that needs to be parsed</param>
        /// <param name="msg_idnum"></param>
        /// <returns></returns>
        public bool ParseMessage(string msg_type, string msg_body, int msg_idnum)
        {
            bool result = false;

            #region Initialize Variables
            string msgType = msg_type;
            string msgBody = msg_body;
            int msgIdnum = msg_idnum;
            string[] msgArr = null;
            string[] currentRowTemp = null;
            string[] currentRow = new string[30];
            string header = "";
            string logMsg = "";
            int convertToInt = 0;
            decimal convertToDecimal = 0;

            EDI_Messages messages = new EDI_Messages();
            Container container = new Container();
            #endregion

            msgBody = Regex.Replace(msg_body, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
            msgArr = msgBody.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            foreach(string row in msgArr)
            {
                string msg;
                // Clear array before insert new row into array
                if (currentRowTemp != null)
                {
                    Array.Clear(currentRowTemp, 0, currentRowTemp.Length);
                }
                if (currentRow != null)
                {
                    Array.Clear(currentRow, 0, currentRow.Length);
                }

                // Insert data into dynamically allocated memory array, and
                // copy to statically allocated memory array.
                msg = row.Trim();
                currentRowTemp = msg.Split(new[] { "*" }, StringSplitOptions.None);
                Array.Copy(currentRowTemp, 0, currentRow, 0, currentRowTemp.Length);

                // Begin to parse engine.
                header = "";

                // Check whether the message header is valid or not.
                if (EDI315_headers.Any(x => x.ToUpper() == currentRow[0]))
                {
                    header = currentRow[0];
                }
                else
                {
                    logMsg = util.buildLogMsg("ParseMessage", "Invalid format message.");
                    util.insertLog_text(logMsg);
                    result = false;
                    break;
                }

                //Add parsed message into entity lists.
                    
                if(row != null && row.Length > 0)
                {
                    convertToInt = 0;
                    convertToDecimal = 0;
                    switch (header)
                    {
                        case "ISA": break;
                        case "GS": break;
                        case "ST":
                            #region ST
                            if (currentRow[1] == "315")
                            {
                                b4 = new B4();
                                q2 = new Q2();
                                n9_list = new List<N9>();
                                r4_list = new List<R4>();
                            }
                            else
                            {
                                logMsg = util.buildLogMsg("ParseMessage", "ST: Invalid Transaction Set Identifier Code (ST01/143).");
                                util.insertLog_text(logMsg);
                            }
                            #endregion
                            break;
                        case "B4":
                            #region
                            b4.special_handling_code = currentRow[1];
                            Int32.TryParse(currentRow[2], out convertToInt);
                            b4.inquiry_request_number = convertToInt;
                            b4.shipment_status_code = currentRow[3];
                            b4.date = currentRow[4];
                            b4.status_time = currentRow[5];
                            b4.status_location = currentRow[6];
                            b4.equipment_initial = currentRow[7];
                            b4.equipment_number = currentRow[8];
                            b4.equipment_status_code = currentRow[9];
                            b4.equipment_type = currentRow[10];
                            b4.location_identifier = currentRow[11];
                            b4.location_qualifier = currentRow[12];
                            b4.equipment_number_check_digit = currentRow[13];
                            #endregion
                            break;
                        case "N9":
                            #region N9
                            N9 n9 = new N9();

                            n9.reference_identification_qualifier = currentRow[1];
                            n9.reference_identification = currentRow[2];
                            n9.free_form_description = currentRow[3];
                            n9.date = currentRow[4];
                            n9.time = currentRow[5];
                            n9.time_code = currentRow[6];

                            n9_list.Add(n9);
                            #endregion
                            break;
                        case "Q2":
                            #region Q2 
                            q2.vessel_code = currentRow[1];
                            q2.country_code = currentRow[2];
                            q2.date = currentRow[3];
                            q2.scheduled_sailing_date = currentRow[4];
                            q2.scheduled_discharge_date = currentRow[5];
                            Int32.TryParse(currentRow[6], out convertToInt);
                            q2.landing_quantity = convertToInt;
                            Decimal.TryParse(currentRow[7], out convertToDecimal);
                            q2.weight = convertToDecimal;
                            q2.weight_qualifier = currentRow[8];
                            q2.voyage_number = currentRow[9];
                            q2.reference_identification_qualifier = currentRow[10];
                            q2.reference_identification = currentRow[11];
                            q2.vessel_code_qualifier = currentRow[12];
                            q2.vessel_name = currentRow[13];
                            Decimal.TryParse(currentRow[7], out convertToDecimal);
                            q2.volume = convertToDecimal;
                            q2.volume_unit_qualifier = currentRow[15];
                            q2.weight_unit_code = currentRow[16];
                            #endregion
                            break;
                        case "R4":
                            #region R4
                            R4 r4 = new R4();
                            r4.port_terminal_function_code = currentRow[1];
                            r4.location_qualifier = currentRow[2];
                            r4.location_identifier = currentRow[3];
                            r4.port_name = currentRow[4];
                            r4.country_code = currentRow[5];
                            r4.state_province_code = currentRow[6];
                            r4.dtm = new DTM();
                            r4_list.Add(r4);
                            #endregion
                            break;
                        case "DTM":
                            #region DTM
                            DTM dtm = r4_list.Last().dtm;
                            Int32.TryParse(currentRow[1], out convertToInt);
                            dtm.date_time_qualifier = convertToInt;
                            dtm.date = currentRow[2];
                            dtm.time = currentRow[3];
                            dtm.time_code = currentRow[4];
                            #endregion
                            break;
                        case "SE":
                            #region SE
                            UpdateDB();
                            #endregion
                            break;
                        case "GE": break;
                        case "IEA": break;
                        default:
                            logMsg = util.buildLogMsg("ParseMessage", "Invalid header");
                            util.insertLog_text(logMsg);
                            break;
                    }
                }
                if(header == "IEA")
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// Update all data from entities to created/existing row in DB.
        /// </summary>
        private void UpdateDB()
        {
            string BM_number = null;
            string EQ_number = null;
            string logMsg = "";
            bool isBMExist = false;
            bool isEQExist = false;

            Container container = null;

            /* 
             * Get MBL and Container number first from the parsed data to check 
             * Whether the data that has same MBL and Container number has been processed or not.
             */

            foreach (N9 n9 in n9_list)
            {
                if(n9.reference_identification_qualifier == "BM")
                {
                    BM_number = n9.reference_identification;
                    isBMExist = true;
                }
                if(n9.reference_identification_qualifier == "EQ")
                {
                    EQ_number = n9.reference_identification;
                    isEQExist = true;
                }
            }

            /*
             * Initialize DB Container entity before inserting into DB
             */ 

            if(isBMExist==true)
            {
                //IF MBL and container number both exist
                if(isBMExist==true && isEQExist == true)
                {
                    container = context.Container.Where(x => x.MBL_number == BM_number && x.container_number == EQ_number).SingleOrDefault();
                    if (container == null)
                    {
                        container = new Container{MBL_number=BM_number, container_number=EQ_number, created_date=DateTime.Now};
                        context.Container.Add(container);
                    }
                }
                //If only MBL number exists.
                else
                {
                    container = context.Container.Where(x => x.MBL_number == BM_number && x.container_number == null).SingleOrDefault();
                    if (container == null)
                    {
                        container = new Container { MBL_number = BM_number, created_date = DateTime.Now };
                        context.Container.Add(container);
                    }
                }
                UpdateEntities(container);
                context.SaveChanges();
            }
            else
            {
                logMsg = util.buildLogMsg("UpdateDB", "Invalid format: " +
                                "MBL number does not exist");
                util.insertLog_text(logMsg);
            }
        }
        private Container UpdateEntities(Container container)
        {
            container = UpdateB4(container);
            container = UpdateN9(container);
            container = UpdateQ2(container);
            container = UpdateR4(container);
            return container;
        }
        private Container UpdateB4(Container container)
        {
            container.shipment_status_code = b4.shipment_status_code;
            container.equipment_type = b4.equipment_type;
            container.equipment_status_code = b4.equipment_status_code;

            switch (b4.shipment_status_code)
            {

                case "D":
                    container.actual_door_delivery_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.actual_door_delivery_location = b4.status_location;
                    break;
                case "I":
                    container.arrival_at_first_port_of_load_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.arrival_at_first_port_of_load_location = b4.status_location;
                    break;
                case "AE":
                    container.loaded_on_board_at_first_port_of_load_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.loaded_on_board_at_first_port_of_load_location = b4.status_location;
                    break;
                case "AF":
                    container.actual_door_pickup_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.actual_door_pickup_location = b4.status_location;
                    break;
                case "AL":
                    container.first_loaded_on_rail_under_outbound_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.first_loaded_on_rail_under_outbound_location = b4.status_location;
                    break;
                case "AM":
                    container.loaded_on_truck_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.loaded_on_truck_location = b4.status_location;
                    break;
                case "AR":
                    container.arrival_at_last_intermodal_hub_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.arrival_at_last_intermodal_hub_location = b4.status_location;
                    break;
                case "CR":
                    container.carrier_released_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.carrier_released_location = b4.status_location;
                    break;
                case "CT":
                    container.customs_released_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.customs_released_location = b4.status_location;
                    break;
                case "CU":
                    container.carrier_and_customs_released_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.carrier_and_customs_released_location = b4.status_location;
                    break;
                case "EE":
                    container.empty_container_picked_up_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.empty_container_picked_up_location = b4.status_location;
                    break;
                case "NO":
                    container.freight_charges_settled_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.freight_charges_settled_location = b4.status_location;
                    break;
                case "OA":
                    container.full_container_received_by_carrier_at_origin_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.full_container_received_by_carrier_at_origin_location = b4.status_location;
                    break;
                case "PA":
                    container.customs_hold_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.customs_hold_location = b4.status_location;
                    break;
                case "RD":
                    container.empty_container_returned_to_carrier_at_destination_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.empty_container_returned_to_carrier_at_destination_location = b4.status_location;
                    break;
                case "RL":
                    container.departure_from_first_intermodal_hub_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.departure_from_first_intermodal_hub_location = b4.status_location;
                    break;
                case "UR":
                    container.last_deramp_under_inbound_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.last_deramp_under_inbound_location = b4.status_location;
                    break;
                case "UV":
                    container.discharged_from_vessel_at_last_port_of_discharged_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.discharged_from_vessel_at_last_port_of_discharged_location = b4.status_location;
                    break;
                case "VA":
                    container.last_vessel_arrival_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.last_vessel_arrival_location = b4.status_location;
                    break;
                case "VD":
                    container.first_vessel_departure_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.first_vessel_departure_location = b4.status_location;
                    break;
                case "W1":
                    container.gate_out_full_at_inland_terminal_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.gate_out_full_at_inland_terminal_location = b4.status_location;
                    break;
                case "W2":
                    container.gate_in_full_at_inland_terminal_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.gate_in_full_at_inland_terminal_location = b4.status_location;
                    break;
                case "W3":
                    container.equipment_delayed_due_to_transportation_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.equipment_delayed_due_to_transportation_location = b4.status_location;
                    break;
                case "W4":
                    container.arrived_at_facility_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.arrived_at_facility_location = b4.status_location;
                    break;
                case "W5":
                    container.departed_from_facility_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.departed_from_facility_location = b4.status_location;
                    break;
                case "W6":
                    container.loaded_at_port_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.loaded_at_port_location = b4.status_location;
                    break;
                case "W7":
                    container.vessel_arrival_at_port_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.vessel_arrival_at_port_location = b4.status_location;
                    break;
                case "W8":
                    container.discharged_from_vessel_at_port_of_discharge_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.discharged_from_vessel_at_port_of_discharge_location = b4.status_location;
                    break;
                case "X1":
                    container.full_container_received_by_carrier_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.full_container_received_by_carrier_location = b4.status_location;
                    break;
                case "X2":
                    container.vessel_departure_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.vessel_departure_location = b4.status_location;
                    break;
                case "X3":
                    container.container_repacked_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.container_repacked_location = b4.status_location;
                    break;
                case "X4":
                    container.container_vanned_at_origin_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.container_vanned_at_origin_location = b4.status_location;
                    break;
                case "X5":
                    container.container_devanned_at_origin_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.container_devanned_at_origin_location = b4.status_location;
                    break;
                case "X6":
                    container.container_vanned_at_destination_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.container_vanned_at_destination_location = b4.status_location;
                    break;
                case "X7":
                    container.container_devanned_at_destination_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.container_devanned_at_destination_location = b4.status_location;
                    break;
                case "X8":
                    container.container_transferred_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.container_transferred_location = b4.status_location;
                    break;
                case "X9":
                    container.carrier_held_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.carrier_held_location = b4.status_location;
                    break;
                case "Y1":
                    container.container_available_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.container_available_location = b4.status_location;
                    break;
                case "Y2":
                    container.arrival_at_intermodal_hub_by_rail_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.arrival_at_intermodal_hub_by_rail_location = b4.status_location;
                    break;
                case "Y3":
                    container.loaded_on_rail_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.loaded_on_rail_location = b4.status_location;
                    break;
                case "Y4":
                    container.rail_move_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.rail_move_location = b4.status_location;
                    break;
                case "Y5":
                    container.loaded_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.loaded_location = b4.status_location;
                    break;
                case "Y7":
                    container.discharged_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.discharged_location = b4.status_location;
                    break;
                case "Y9":
                    container.container_picked_up_from_port_of_discharge_transhipment_port_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.container_picked_up_from_port_of_discharge_transhipment_port_location = b4.status_location;
                    break;
                case "Z1":
                    container.last_deramp_under_outbound_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.last_deramp_under_outbound_location = b4.status_location;
                    break;
                case "Z2":
                    container.transhipment_vessel_arrival_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.transhipment_vessel_arrival_location = b4.status_location;
                    break;
                case "Z3":
                    container.loaded_at_port_of_transhipment_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.loaded_at_port_of_transhipment_location = b4.status_location;
                    break;
                case "Z4":
                    container.discharged_at_port_of_transhipment_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.discharged_at_port_of_transhipment_location = b4.status_location;
                    break;
                case "Z5":
                    container.transhipment_vessel_departure_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.transhipment_vessel_departure_location = b4.status_location;
                    break;
                case "Z6":
                    container.intermodal_departure_from_last_port_of_discharge_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.intermodal_departure_from_last_port_of_discharge_location = b4.status_location;
                    break;
                case "Z7":
                    container.first_loaded_on_rail_under_inbound_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.first_loaded_on_rail_under_inbound_location = b4.status_location;
                    break;
                case "Z8":
                    container.picked_up_at_final_destination_for_delivery_time = util.changeDateTimeFormat(b4.date, b4.status_time);
                    container.picked_up_at_final_destination_for_delivery_location = b4.status_location;
                    break;
                default:
                    string logMsg = util.buildLogMsg("UpdateB4", "Invalid format: shipment status code");
                    util.insertLog_text(logMsg);
                    break;
            }

            return container;
        }
        private Container UpdateN9(Container container)
        {
            foreach(N9 n9 in n9_list)
            {
                switch (n9.reference_identification_qualifier)
                {
                    // BM (MBL number) and EQ (Container number) is already inserted in UpdateDB method. 
                    case "BM": break;
                    case "EQ": break;
                    case "BN":
                        container.booking_number = n9.reference_identification;
                        break;
                    case "CR":
                        container.booking_number = n9.reference_identification;
                        break;
                    case "SCA":
                        container.booking_number = n9.reference_identification;
                        break;
                    case "SN":
                        container.seal_number = n9.reference_identification;
                        break;
                    case "4M":
                        container.service_type = n9.reference_identification;
                        break;
                    case "SI":
                        container.shipper_reference_number = n9.reference_identification;
                        break;
                    case "P8":
                        container.pickup_number = n9.reference_identification;
                        break;
                    case "PO":
                        container.purchase_order_number = n9.reference_identification;
                        break;
                    case "IT":
                        container.IT_number = n9.reference_identification;
                        break;
                    default:
                        string logMsg = util.buildLogMsg("UpdateN9", "Invalid format reference_identification_qualifier");
                        util.insertLog_text(logMsg);
                        break;
                }
             
            }


            return container;
        }
        private Container UpdateQ2(Container container)
        {
            container.vessel_code = q2.vessel_code_qualifier;
            container.vessel_name = q2.vessel_name;
            container.voyage_number = q2.voyage_number;
            container.lading_quantity = q2.landing_quantity;
            container.weight = q2.weight;
            container.volume = q2.volume;
            container.scheduled_sailing_date = util.changeDateTimeFormat(q2.scheduled_sailing_date, null);
            container.scheduled_discharge_date = util.changeDateTimeFormat(q2.scheduled_discharge_date, null);

            return container;
        }
        private Container UpdateR4(Container container)
        {
            bool isLocationIdentifierExist = false;
            bool isPortNameExist = false;
            string logMsg = "";

            foreach(R4 r4 in r4_list)
            {
                switch (r4.port_terminal_function_code)
                {
                    case "R":
                        //If location qualifier presents, location identifier must be present also.
                        container.place_of_receipt_location_qualifier = r4.location_qualifier;

                        if (r4.location_identifier != null)
                        {
                            container.place_of_receipt_location_identifier = r4.location_identifier;
                            isLocationIdentifierExist = true;
                        }
                        if (r4.port_name != null)
                        {
                            container.place_of_receipt_portname = r4.port_name;
                            isPortNameExist = true;
                        }

                        //Either one of these or both must exist; Otherwise invalid format
                        if (isLocationIdentifierExist == false && isPortNameExist == false)
                        {
                            logMsg = util.buildLogMsg("UpdateR4", "Invalid format: " +
                                "Both location identification and port name do not exist");
                            util.insertLog_text(logMsg);
                            break;
                        }

                        container.place_of_receipt_country = r4.country_code;
                        container.place_of_receipt_datetime = util.changeDateTimeFormat(r4.dtm.date, r4.dtm.time);
                        break;

                    case "L":
                        //If location qualifier presents, location identifier must be present also.
                        container.port_of_loading_location_qualifier = r4.location_qualifier;

                        if (r4.location_identifier != null)
                        {
                            container.port_of_loading_location_identifier = r4.location_identifier;
                            isLocationIdentifierExist = true;
                        }
                        if (r4.port_name != null)
                        {
                            container.port_of_loading_portname = r4.port_name;
                            isPortNameExist = true;
                        }

                        //Either one of these or both must exist; Otherwise invalid format
                        if (isLocationIdentifierExist == false && isPortNameExist == false)
                        {
                            logMsg = util.buildLogMsg("UpdateR4", "Invalid format: " +
                                "Both location identifier and port name do not exist");
                            util.insertLog_text(logMsg);
                            break;
                        }

                        container.port_of_loading_country = r4.country_code;
                        container.port_of_loading_datetime = util.changeDateTimeFormat(r4.dtm.date, r4.dtm.time);
                        break;

                    case "D":
                        //If location qualifier presents, location identifier must be present also.
                        container.port_of_discharge_location_qualifier = r4.location_qualifier;

                        if (r4.location_identifier != null)
                        {
                            container.port_of_discharge_location_identifier = r4.location_identifier;
                            isLocationIdentifierExist = true;
                        }
                        if (r4.port_name != null)
                        {
                            container.port_of_discharge_portname = r4.port_name;
                            isPortNameExist = true;
                        }

                        //Either one of these or both must exist; Otherwise invalid format
                        if (isLocationIdentifierExist == false && isPortNameExist == false)
                        {
                            logMsg = util.buildLogMsg("UpdateR4", "Invalid format: " +
                                "Both location identification and port name do not exist");
                            util.insertLog_text(logMsg);
                            break;
                        }

                        container.port_of_discharge_country = r4.country_code;
                        container.port_of_discharge_datetime = util.changeDateTimeFormat(r4.dtm.date, r4.dtm.time);
                        break;

                    case "E": 
                        //If location qualifier presents, location identifier must be present also.
                        container.place_of_delivery_location_qualifier = r4.location_qualifier;

                        if (r4.location_identifier != null)
                        {
                            container.place_of_delivery_location_identifier = r4.location_identifier;
                            isLocationIdentifierExist = true;
                        }
                        if (r4.port_name != null)
                        {
                            container.place_of_delivery_portname = r4.port_name;
                            isPortNameExist = true;
                        }

                        //Either one of these or both must exist; Otherwise invalid format
                        if (isLocationIdentifierExist == false && isPortNameExist == false)
                        {
                            logMsg = util.buildLogMsg("UpdateR4", "Invalid format: " +
                                "Both location identification and port name do not exist");
                            util.insertLog_text(logMsg);
                            break;
                        }

                        container.place_of_delivery_country = r4.country_code;
                        container.place_of_delivery_datetime = util.changeDateTimeFormat(r4.dtm.date, r4.dtm.time);
                        break;

                    case "M": 
                        //If location qualifier presents, location identifier must be present also.
                        container.MBL_destination_location_qualifier = r4.location_qualifier;

                        if (r4.location_identifier != null)
                        {
                            container.MBL_destination_location_identifier = r4.location_identifier;
                            isLocationIdentifierExist = true;
                        }
                        if (r4.port_name != null)
                        {
                            container.MBL_destination_portname = r4.port_name;
                            isPortNameExist = true;
                        }

                        //Either one of these or both must exist; Otherwise invalid format
                        if (isLocationIdentifierExist == false && isPortNameExist == false)
                        {
                            logMsg = util.buildLogMsg("UpdateR4", "Invalid format: " +
                                "Both location identifier and port name do not exist");
                            util.insertLog_text(logMsg);
                            break;
                        }

                        container.MBL_destination_country = r4.country_code;
                        container.MBL_destination_datetime = util.changeDateTimeFormat(r4.dtm.date, r4.dtm.time);
                        break;

                    case "5": break;

                    default:
                        logMsg = util.buildLogMsg("UpdateR4", "Invalid format: " +
                                "There is no location qualifier in the message");
                        util.insertLog_text(logMsg);
                        break;
                }
            }
            return container;
        }
    }
}
