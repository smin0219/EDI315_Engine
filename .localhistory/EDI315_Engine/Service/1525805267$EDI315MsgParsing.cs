using EDI315_Engine.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EDI315_Engine.Service
{
    public class EDI315MsgParsing
    {
        protected LogFile logFile;
        protected DBContext context;

        List<string> EDI315_HEADERS = new List<string> { "ISA", "GS", "ST", "B4", "N9", "Q2", "R4", "DTM", "SE", "GE", "IEA" };

        public EDI315MsgParsing()
        {
            logFile = new LogFile();
            context = new DBContext();
        }
        public bool MeesageParsing(string msg_body, int msg_idnum)
        {
            bool result = false;

            try
            {
                #region Initialize values
                string[] msgBody;
                string[] rowLine;
                string header;

                EDI315 dbEDI315 = new EDI315();
                EDI315_Detail dbEDI315_Detail = new EDI315_Detail();
                List<EDI315_Detail_N9> dbEDI315_Detail_N9List = new List<EDI315_Detail_N9>();
                List<EDI315_Detail_R4> dbEDI315_Detail_R4List = new List<EDI315_Detail_R4>();

                int convertToInt = 0;
                decimal convertToDecimal = 0;
                #endregion

                msg_body = Regex.Replace(msg_body, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
                msgBody = msg_body.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                foreach (string row in msgBody)
                {
                    if (row != null && row.Count() > 0)
                    {
                        rowLine = row.Split('*');

                        header = "";
                        if (EDI315_HEADERS.Any(x => x.ToUpper() == rowLine[0]))
                            header = rowLine[0];

                        switch (header)
                        {
                            case "ISA":
                                #region ISA
                                dbEDI315 = new EDI315()
                                {
                                    msg_idnum = msg_idnum,
                                    ISA_auth_info_qualifier = rowLine[1] ?? "",
                                    ISA_auth_info = rowLine[2] ?? "",
                                    ISA_security_info_qualifier = rowLine[3] ?? "",
                                    ISA_security_info = rowLine[4] ?? "",
                                    ISA_interchange_id_qualifier_1 = rowLine[5] ?? "",
                                    ISA_interchange_sender_id = rowLine[6] ?? "",
                                    ISA_interchange_id_qualifier_2 = rowLine[7] ?? "",
                                    ISA_interchange_receiver_id = rowLine[8] ?? "",
                                    ISA_interchage_date = rowLine[9] ?? "",
                                    ISA_interchange_time = rowLine[10] ?? "",
                                    ISA_interchange_control_standards_id = rowLine[11] ?? "",
                                    ISA_interchange_control_version_number = rowLine[12] ?? "",
                                    ISA_interchange_control_number = rowLine[13] ?? "",
                                    ISA_ack_requested = rowLine[14] ?? "",
                                    ISA_usage_indicator = rowLine[15] ?? "",
                                    ISA_component_element_separator = rowLine[16] ?? "",
                                };
                                #endregion
                                break;

                            case "GS":
                                // Table EDI315 insert here.
                                #region GS
                                dbEDI315.GS_functional_id_code = rowLine[1] ?? "";
                                dbEDI315.GS_app_sender_code = rowLine[2] ?? "";
                                dbEDI315.GS_app_receiver_code = rowLine[3] ?? "";
                                dbEDI315.GS_date = rowLine[4] ?? "";
                                dbEDI315.GS_time = rowLine[5] ?? "";
                                dbEDI315.GS_group_control_number = rowLine[6] ?? "";
                                dbEDI315.GS_reponsible_agency_code = rowLine[7] ?? "";
                                dbEDI315.GS_industry_id_code = rowLine[8] ?? "";

                                /* Insert table EDI315 after done with ISA & GS */
                                if (dbEDI315.msg_idnum != 0)
                                {
                                    using (DBContext context = new DBContext())
                                    {
                                        /* check DB is open */

                                        dbEDI315.created_date = DateTime.Now;
                                        context.EDI315.Add(dbEDI315);
                                        context.SaveChanges();

                                        context.Dispose();
                                    }
                                }
                                #endregion
                                break;
                            case "ST":
                                dbEDI315_Detail = new EDI315_Detail();

                                break;
                            case "SE":
                                // insert Table EDI315_Detail, EDI315_Detail_N9, EDI315_Detail_R4 & EDI315_Detail_R4_DTM
                                break;

                            case "B4":
                                #region B4
                                if (dbEDI315 == null || dbEDI315.EDI315_idnum == 0)
                                    continue;

                                dbEDI315_Detail.B4_SHC = rowLine[1];

                                convertToInt = 0;
                                if (rowLine[2] != null && rowLine[2].Trim() != string.Empty && Int32.TryParse(rowLine[2], out convertToInt))
                                    dbEDI315_Detail.B4_request_number = convertToInt;

                                dbEDI315_Detail.B4_status_code = rowLine[3];
                                dbEDI315_Detail.B4_date = rowLine[4] ?? "";
                                dbEDI315_Detail.B4_status_time = rowLine[5] ?? "";
                                dbEDI315_Detail.B4_status_location = rowLine[6] ?? "";

                                dbEDI315_Detail.B4_equip_initial = rowLine[7];
                                dbEDI315_Detail.B4_equip_number = rowLine[8];
                                dbEDI315_Detail.B4_equip_status_code = rowLine[9];
                                dbEDI315_Detail.B4_equip_type = rowLine[10];
                                dbEDI315_Detail.B4_location_code = rowLine[11];
                                dbEDI315_Detail.B4_location_id = rowLine[12];

                                if (rowLine.Count() > 13)
                                {
                                    convertToInt = 0;
                                    if (rowLine[13] != null && rowLine[13].Trim() != string.Empty && Int32.TryParse(rowLine[13], out convertToInt))
                                        dbEDI315_Detail.B4_equip_check_digit = convertToInt;
                                }
                                #endregion
                                break;

                            case "Q2":
                                #region Q2
                                if (dbEDI315 == null || dbEDI315.EDI315_idnum == 0)
                                    continue;

                                dbEDI315_Detail.Q2_vessel_code = rowLine[1];
                                dbEDI315_Detail.Q2_country_code = rowLine[2];
                                dbEDI315_Detail.Q2_date_1 = rowLine[3];
                                dbEDI315_Detail.Q2_date_2 = rowLine[4];
                                dbEDI315_Detail.Q2_date_3 = rowLine[5];

                                convertToInt = 0;
                                if (rowLine[6] != null && rowLine[6].Trim() != string.Empty && Int32.TryParse(rowLine[6], out convertToInt))
                                    dbEDI315_Detail.Q2_lading_quantity = convertToInt;

                                convertToDecimal = 0;
                                if (rowLine[7] != null && rowLine[7].Trim() != string.Empty && Decimal.TryParse(rowLine[7], out convertToDecimal))
                                    dbEDI315_Detail.Q2_weight = convertToDecimal;

                                dbEDI315_Detail.Q2_weight_qualifier = rowLine[8];
                                dbEDI315_Detail.Q2_voyage_number = rowLine[9];
                                dbEDI315_Detail.Q2_reference_id_qualifier = rowLine[10];
                                dbEDI315_Detail.Q2_reference_id = rowLine[11];
                                dbEDI315_Detail.Q2_vessel_code_qualifier = rowLine[12];
                                dbEDI315_Detail.Q2_vessel_name = rowLine[13];

                                convertToDecimal = 0;
                                if (rowLine[14] != null && rowLine[14].Trim() != string.Empty && Decimal.TryParse(rowLine[14], out convertToDecimal))
                                    dbEDI315_Detail.Q2_volume = convertToDecimal;

                                dbEDI315_Detail.Q2_volume_unit_qualifier = rowLine[15];
                                dbEDI315_Detail.Q2_weight_unit_code = rowLine[16];
                                #endregion
                                break;

                            case "N9":
                                #region N9
                                if (dbEDI315 == null || dbEDI315.EDI315_idnum == 0)
                                    continue;

                                EDI315_Detail_N9 dbTemp = new EDI315_Detail_N9();

                                dbTemp.reference_id_qualifier = rowLine[1];

                                if (rowLine.Count() > 2)
                                    dbTemp.reference_id = rowLine[2];

                                if (rowLine.Count() > 3)
                                    dbTemp.free_form_description = rowLine[3];

                                if (rowLine.Count() > 4)
                                    dbTemp.free_form_description = rowLine[4];

                                if (rowLine.Count() > 5)
                                    dbTemp.date = rowLine[5];

                                if (rowLine.Count() > 6)
                                    dbTemp.time = rowLine[6];

                                if (rowLine.Count() > 7)
                                    dbTemp.time_code = rowLine[7];

                                dbEDI315_Detail_N9List.Add(dbTemp);
                                #endregion
                                break;

                            case "R4":
                                break;

                            case "DTM":
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                result = false;
            }

            return result;
        }

        private bool dbConnectionCheck()
        {
            bool isConnected = false;
            //
            int checkCount = 0;
            while (checkCount < 5)
            {
                try
                {
                    using (DBContext context = new DBContext())
                    {
                        context.Database.Connection.Open();
                        context.Dispose();
                        break;
                    }
                }
                catch
                {
                    checkCount++;
                }
            }
            return isConnected;
        }
    }
}
