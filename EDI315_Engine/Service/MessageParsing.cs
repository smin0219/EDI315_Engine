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
        protected Util util;
        protected DBContext context;

        List<B4> b4_list = new List<B4>();
        List<N9> n9_list = new List<N9>();
        List<Q2> q2_list = new List<Q2>();
        List<R4> r4_list = new List<R4>();

        List<string> EDI315_headers = new List<string> { "ISA", "GS", "ST", "B4", "N9", "Q2", "R4", "DTM", "SE", "GE", "IEA" };

        public MessageParsing()
        {
            util = new Util();
            context = new DBContext();
        }

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
                    Array.Clear(currentRowTemp, 0, currentRowTemp.Length);

                if (currentRow != null)
                    Array.Clear(currentRow, 0, currentRow.Length);

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
                    string logMsg = "Date: " + DateTime.Now.ToString();
                    logMsg += "\r\nFunction: ParseMessage";
                    logMsg += "\r\nError Message: Invalid format message.";
                    logMsg += "\r\n\r\n=====================================================================";
                    logMsg += "=============================================================================";
                    logMsg += "=============================================================================";
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
                        case "ST": break;
                        case "IEA": break;
                        case "B4":
                            #region
                            B4 b4 = new B4();
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

                            b4_list.Add(b4);
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
                            Q2 q2 = new Q2();
                            Int32.TryParse(currentRow[1], out convertToInt);
                            q2.vessel_code = convertToInt;
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

                            q2_list.Add(q2);
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
                            DTM dtm = r4_list.Last().dtm;
                            Int32.TryParse(currentRow[1], out convertToInt);
                            #region DTM
                            dtm.date_time_qualifier = convertToInt;
                            dtm.date = currentRow[2];
                            dtm.time = currentRow[3];
                            dtm.time_code = currentRow[4];
                            #endregion
                            break;
                    }
                }
                if(header == "IEA")
                {
                    result = true;
                    break;
                }
            }

            insertDataIntoDB();

            return result;
        }

        private void insertDataIntoDB()
        {
           // if(n9_list.)
        }

    }
}
