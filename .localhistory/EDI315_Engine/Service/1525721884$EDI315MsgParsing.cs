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

        List<string> EDI315_HEADERS = new List<string> {"ISA", "GS", "ST", "B4", "N9", "Q2", "R4", "DTM", "SE", "GE", "IEA" };

        public EDI315MsgParsing()
        {
            logFile = new LogFile();
            context = new DBContext();
        }
        public bool MeesageParsing(string orgMsg)
        {
            bool result = false;
            
            try
            {
                #region Initialize values
                string[] msgBody;
                string[] rowLine;
                string header;

                #endregion

                orgMsg = Regex.Replace(orgMsg, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
                msgBody = orgMsg.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

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
                                //do somethind
                                #endregion
                                break;
                            case "GS":
                                #region GS

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
