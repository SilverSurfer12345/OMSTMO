// CustomerManager.cs (Conceptual)
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OrderManagement.Model
{
    public class CustomerManager
    {
        public class CustomerDto
        {
            public int Id { get; set; }
            public string Forename { get; set; }
            public string Surname { get; set; }
            public string TelephoneNo { get; set; }
            public string Email { get; set; }
            public string HouseNameNumber { get; set; }
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string AddressLine3 { get; set; }
            public string AddressLine4 { get; set; }
            public string Postcode { get; set; }
            public int PreviousOrdersCount { get; set; } // Assuming this is stored or calculated
        }

        public CustomerDto GetCustomerDetails(int customerId)
        {
            string query = "SELECT * FROM customers WHERE Id = @Id";
            var parameters = new Dictionary<string, object> { { "@Id", customerId } };
            DataTable dt = DatabaseManager.ExecuteQuery(query, parameters);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new CustomerDto
                {
                    Id = SafeOperations.SafeGetInt(row, "Id"),
                    Forename = SafeOperations.SafeGetString(row, "forename"),
                    Surname = SafeOperations.SafeGetString(row, "surname"),
                    TelephoneNo = SafeOperations.SafeGetString(row, "telephoneNo"),
                    Email = SafeOperations.SafeGetString(row, "Email"),
                    HouseNameNumber = SafeOperations.SafeGetString(row, "houseNameNumber"),
                    AddressLine1 = SafeOperations.SafeGetString(row, "AddressLine1"),
                    AddressLine2 = SafeOperations.SafeGetString(row, "AddressLine2"),
                    AddressLine3 = SafeOperations.SafeGetString(row, "AddressLine3"),
                    AddressLine4 = SafeOperations.SafeGetString(row, "AddressLine4"),
                    Postcode = SafeOperations.SafeGetString(row, "Postcode"),
                    PreviousOrdersCount = SafeOperations.SafeGetInt(row, "previousOrders") // Assuming this column exists
                };
            }
            return null;
        }

        public CustomerDto GetCustomerByTelephone(string telephoneNo)
        {
            string query = "SELECT * FROM customers WHERE telephoneNo = @telephoneNo";
            var parameters = new Dictionary<string, object> { { "@telephoneNo", telephoneNo } };
            DataTable dt = DatabaseManager.ExecuteQuery(query, parameters);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new CustomerDto
                {
                    Id = SafeOperations.SafeGetInt(row, "Id"),
                    Forename = SafeOperations.SafeGetString(row, "forename"),
                    Surname = SafeOperations.SafeGetString(row, "surname"),
                    TelephoneNo = SafeOperations.SafeGetString(row, "telephoneNo"),
                    Email = SafeOperations.SafeGetString(row, "Email"),
                    HouseNameNumber = SafeOperations.SafeGetString(row, "houseNameNumber"),
                    AddressLine1 = SafeOperations.SafeGetString(row, "AddressLine1"),
                    AddressLine2 = SafeOperations.SafeGetString(row, "AddressLine2"),
                    AddressLine3 = SafeOperations.SafeGetString(row, "AddressLine3"),
                    AddressLine4 = SafeOperations.SafeGetString(row, "AddressLine4"),
                    Postcode = SafeOperations.SafeGetString(row, "Postcode"),
                    PreviousOrdersCount = SafeOperations.SafeGetInt(row, "previousOrders")
                };
            }
            return null;
        }

        public int CreateCustomer(string forename, string surname, string telephoneNo, string address)
        {
            string query = @"
                INSERT INTO customers (forename, surname, telephoneNo, AddressLine1, previousOrders)
                VALUES (@forename, @surname, @telephoneNo, @address, 0);
                SELECT SCOPE_IDENTITY();";
            var parameters = new Dictionary<string, object>
            {
                { "@forename", forename }, { "@surname", surname },
                { "@telephoneNo", string.IsNullOrEmpty(telephoneNo) ? DBNull.Value : (object)telephoneNo },
                { "@address", string.IsNullOrEmpty(address) ? DBNull.Value : (object)address }
            };
            object result = DatabaseManager.ExecuteScalar(query, parameters);
            return (result == null || result == DBNull.Value) ? -1 : Convert.ToInt32(result);
        }

        public List<CustomerDto> SearchCustomersByTelephone(string telephoneNoPrefix)
        {
            string query = "SELECT TOP 10 * FROM customers WHERE telephoneNo LIKE @telephoneNo + '%'";
            var parameters = new Dictionary<string, object> { { "@telephoneNo", telephoneNoPrefix } };
            DataTable dt = DatabaseManager.ExecuteQuery(query, parameters);
            List<CustomerDto> customers = new List<CustomerDto>();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    customers.Add(new CustomerDto
                    {
                        Id = SafeOperations.SafeGetInt(row, "Id"),
                        Forename = SafeOperations.SafeGetString(row, "forename"),
                        Surname = SafeOperations.SafeGetString(row, "surname"),
                        TelephoneNo = SafeOperations.SafeGetString(row, "telephoneNo"),
                        Email = SafeOperations.SafeGetString(row, "Email"),
                        HouseNameNumber = SafeOperations.SafeGetString(row, "houseNameNumber"),
                        AddressLine1 = SafeOperations.SafeGetString(row, "AddressLine1"),
                        AddressLine2 = SafeOperations.SafeGetString(row, "AddressLine2"),
                        AddressLine3 = SafeOperations.SafeGetString(row, "AddressLine3"),
                        AddressLine4 = SafeOperations.SafeGetString(row, "AddressLine4"),
                        Postcode = SafeOperations.SafeGetString(row, "Postcode")
                    });
                }
            }
            return customers;
        }

        public string GetCustomerForename(int customerId)
        {
            var customer = GetCustomerDetails(customerId);
            return customer?.Forename ?? string.Empty;
        }

        public string GetCustomerSurname(int customerId)
        {
            var customer = GetCustomerDetails(customerId);
            return customer?.Surname ?? string.Empty;
        }
    }
}