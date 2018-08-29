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
            string[] msgBodyArr = null;
            string[] currentRowTemp = null;
            string[] currentRow = new string[30];
            string header = "";
            int count = 1;
            int convertToInt = 0;
            decimal convertToDecimal = 0;

            EDI_Messages messages = new EDI_Messages();
            Container container = new Container();
            #endregion

            msgBody = Regex.Replace(msg_body, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
            msgBodyArr = msgBody.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            foreach(string row in msgBodyArr)
            {
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

                currentRowTemp = row.Split(new[] { "*" }, StringSplitOptions.None);
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
                    string logMsg = util.buildLogMsg("ParseMessage", "Invalid format message.");
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
                                string logMsg = util.buildLogMsg("ParseMessage", "ST: Invalid Transaction Set Identifier Code (ST01/143).");
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
                            b4.shipment_status_datetime = DateTime.Parse(currentRow[4]);
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
                            q2.scheduled_sailing_date = DateTime.Parse(currentRow[4]);
                            q2.scheduled_discharge_date = DateTime.Parse(currentRow[5]);
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
            string MBLRefId = null;
            string containerRefId = null;
            string isMBLExist = null;
            string isContainerExist = null;
            bool isBMChecked = false;
            bool isEQChecked = false;

            Container container = null;

            /* Get MBL and Container number first from the parsed data to check 
             * Whether the data that has same MBL and Container number has been processed or not.
             */
            
            foreach(N9 n9 in n9_list)
            {
                if (isBMChecked == true && isEQChecked == true)
                {
                    break;
                }
                if(n9.reference_identification_qualifier == "BM")
                {
                    MBLRefId = n9.reference_identification;
                    isMBLExist = (context.Container.Where(x => x.MBL_number == MBLRefId).Select(x => x.MBL_number)).SingleOrDefault();
                    isBMChecked = true;
                }
                
                if (n9.reference_identification_qualifier == "EQ")
                {
                    containerRefId = n9.reference_identification;
                    isContainerExist = (context.Container.Where(x => x.MBL_number == MBLRefId && x.container_number == containerRefId).Select(x => x.container_number)).SingleOrDefault();
                    isEQChecked = true;
                }
            }

            /* 
             * Same MBL has been processed before with this MBL number.
             * Update data into the existing MBL. 
             */
            

            if (isMBLExist != null)
            {
                // If the data has same MBL and container number in DB.
                if (isContainerExist != null)
                {
                    container = (context.Container.Where(x => x.MBL_number == MBLRefId && x.container_number == containerRefId)).SingleOrDefault();
                    container = UpdateEntities(container);
                }
                // If the data has same MBL number only.
                else
                {
                    container = (context.Container.Where(x => x.MBL_number == MBLRefId)).SingleOrDefault();
                    container = UpdateEntities(container);
                }
            }
            
            /* 
             * If the data does not have same MBL and container number in DB.
             * Create a row with this MBL number and insert data accordingly. 
             */

            else
            {
                if(containerRefId == null)
                {
                    container = new Container { MBL_number = MBLRefId, created_date = DateTime.Now };
                    container = UpdateEntities(container);
                }
                else
                {
                    container = new Container { MBL_number = MBLRefId, container_number = containerRefId, created_date = DateTime.Now };
                    container = UpdateEntities(container);
                }   
            }

            context.Container.Add(container);
            context.SaveChanges();
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
            container.equipment_type = b4.equipment_type;
            container.shipment_status_code = b4.shipment_status_code;
            container.shipment_status_datetime = b4.shipment_status_datetime;

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
            container.vessel_code = q2.vessel_code;
            container.vessel_name = q2.vessel_name;
            container.voyage_number = q2.voyage_number;
            container.lading_quantity = q2.landing_quantity;
            container.weight = q2.weight;
            container.volume = q2.volume;
            container.scheduled_sailing_date = q2.scheduled_sailing_date;
            container.scheduled_discharge_date = q2.scheduled_discharge_date;

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
                        container.place_of_receipt_datetime = DateTime.Parse(r4.dtm.date + r4.dtm.time);
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
                        container.port_of_loading_datetime = DateTime.Parse(r4.dtm.date + r4.dtm.time);
                        break;

                        /// 여기까지 했음 container. 이름 바꾸는 것

                    case "D":
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
                        container.place_of_receipt_datetime = DateTime.Parse(r4.dtm.date + r4.dtm.time);
                        break;

                    case "E": break;

                    case "M": break;

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
