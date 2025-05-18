using System;
using System.Collections.Generic;
using System.Data; // Added this namespace for DataTable
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.helpers
{
    class DatabaseHelper
    {
        public static DataTable GetCustomerByTelephone(string telephoneNo)
        {
            string query = "SELECT * FROM customers WHERE telephoneNo = @telephoneNo";
            var parameters = new Dictionary<string, object> { { "@telephoneNo", telephoneNo } };
            return MainClass.getDataFromTable(query, parameters);
        }
    }
}
