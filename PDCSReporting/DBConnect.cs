using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PDCSReporting
{
    public class DBConnect
    {
        public static SqlConnection NewCon;
        public static string ConStr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        public static SqlConnection GetConnection()
        {
            NewCon = new SqlConnection(ConStr);

            return NewCon;
        }
    }
}