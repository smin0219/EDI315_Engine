﻿using EDI315_Engine.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI315_Engine.Service
{
    private DBContext context = new DBContext();

    public class EDI315MsgParsing
    {
        public bool MeesageParsing(string msgBody)
        {
            bool result = false;
            return result;
        }
    }
}
