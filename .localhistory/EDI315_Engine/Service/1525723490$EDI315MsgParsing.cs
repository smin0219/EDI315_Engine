﻿using EDI315_Engine.Context;
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

                /* ISA Init. */
                string ISA_auth_info_qualifier;
                string ISA_auth_info;
                string ISA_security_info_qualifier;
                string ISA_security_info;
                string ISA_interchange_id_qualifier_1;
                string ISA_interchange_sender_id;
                string ISA_interchange_id_qualifier_2;
                string ISA_interchange_receiver_id;
                string ISA_interchage_date;
                string ISA_interchange_time;
                string ISA_interchange_control_standards_id;
                string ISA_interchange_control_version_number;
                string ISA_interchange_control_number;
                string ISA_ack_requested;
                string ISA_usage_indicator;
                string ISA_component_element_separator;

                string GS_functional_id_code;
                string GS_app_sender_code;
                string GS_app_receiver_code;
                string GS_date;
                string GS_time;
                string GS_group_control_number;
                string GS_reponsible_agency_code;
                string GS_industry_id_code;
                #endregion

                msg_body = Regex.Replace(msg_body, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
                msgBody = msg_body.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                foreach(string row in msgBody)
                {
                    if(row != null && row.Count() > 0)
                    {
                        rowLine = row.Split('*');

                        header = "";
                        if (EDI315_HEADERS.Any(x => x.ToUpper() == rowLine[0]))
                        {
                            header = rowLine[0];
                        }

                        switch (header)
                        {
                            case "ISA":
                                #region ISA
                                ISA_auth_info_qualifier = rowLine[1] ?? "";
                                ISA_auth_info = rowLine[2] ?? "";
                                ISA_security_info_qualifier = rowLine[3] ?? "";
                                ISA_security_info = rowLine[4] ?? "";
                                ISA_interchange_id_qualifier_1 = rowLine[5] ?? "";
                                ISA_interchange_sender_id = rowLine[6] ?? "";
                                ISA_interchange_id_qualifier_2 = rowLine[7] ?? "";
                                ISA_interchange_receiver_id = rowLine[8] ?? "";
                                ISA_interchage_date = rowLine[9] ?? "";
                                ISA_interchange_time = rowLine[10] ?? "";
                                ISA_interchange_control_standards_id = rowLine[11] ?? "";
                                ISA_interchange_control_version_number = rowLine[12] ?? "";
                                ISA_interchange_control_number = rowLine[13] ?? "";
                                ISA_ack_requested = rowLine[14] ?? "";
                                ISA_usage_indicator = rowLine[15] ?? "";
                                ISA_component_element_separator = rowLine[16] ?? "";
                                #endregion
                                break;
                            case "GS":
                                #region GS
                                GS_functional_id_code = rowLine[1] ?? "";
                                GS_app_sender_code = rowLine[2] ?? "";
                                GS_app_receiver_code = rowLine[3] ?? "";
                                GS_date = rowLine[4] ?? "";
                                GS_time = rowLine[5] ?? "";
                                GS_group_control_number = rowLine[6];
                                GS_reponsible_agency_code = rowLine[7] ?? "";
                                GS_industry_id_code = rowLine[8] ?? "";

                                /* Insert table EDI315 after done with ISA & GS */
                                using (DBContext context = new DBContext())
                                {

                                }
                                #endregion
                                break;
                        }
                    }
                }
            }
            catch(Exception ex)
            {

                result = false;
            }

            return result;
        }
    }
}
