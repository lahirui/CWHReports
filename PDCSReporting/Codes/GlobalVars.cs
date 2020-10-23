using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS
{
    public class GlobalVars
    {
        public enum LogonType : int
        {
            Interactive = 2,
            Network = 3,
            Batch = 4,
            Service = 5,
            Unlock = 7,
            NetworkCleartText = 8,
            NewCredentials = 9,
        };
        public enum LogonProvider : int
        {
            Default = 0,
        }
        public static string LOGIN_NAME;
    }
}