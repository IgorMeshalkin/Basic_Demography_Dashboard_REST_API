using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicDmgAPI.Util
{
    public class ConditionalNumber
    {
        private const int CONDITIONAL_NUMBER = -1001;

        public static decimal Get()
        {
            return CONDITIONAL_NUMBER;
        }
    }
}