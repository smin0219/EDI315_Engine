using EDI315_Engine.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDI315_Engine
{
    public class Util
    {
        protected DBContext context;
        
        public Util()
        {
            context = new DBContext();
        }
        public bool dbConnectionCheck()
        {
            bool isConnected = false;

            int checkCount = 0;
            while (checkCount < 5)
            {
                try
                {
                    using (DBContext context = new DBContext())
                    {
                        context.Database.Connection.Open();
                        context.Dispose();

                        isConnected = true;
                        break;
                    }
                }
                catch
                {
                    checkCount++;

                    // 30 min wait
                    System.Threading.Thread.Sleep((1000 * 60) * 30);
                }
            }
            return isConnected;
        }
    }
}
