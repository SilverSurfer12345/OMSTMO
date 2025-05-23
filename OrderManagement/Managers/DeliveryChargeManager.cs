using OrderManagement.View;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OrderManagement.View;

namespace OrderManagement.Model
{
    /// <summary>
    /// Centralized manager for all delivery charge functionality
    /// </summary>
    public static class DeliveryChargeManager
    {
        #region Public Methods

        /// <summary>
        /// Initializes delivery charge controls and hooks up event handlers
        /// </summary>
        public static void Initialize(frmPOS posForm)
        {
            try
            {
                // Find and set up UI elements on the POS form
                InitializeUI(posForm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing delivery charge: {ex.Message}");
            }
        }

        /// <summary>
        /// Calculate delivery charge based on postcode
        /// </summary>
        public static decimal CalculateDeliveryCharge(string postcode)
        {
            // Default charge
            decimal defaultCharge = 0.00m;

            if (string.IsNullOrWhiteSpace(postcode))
                return defaultCharge;

            // Clean up the postcode
            postcode = postcode.ToUpper().Replace(" ", "");

            try
            {
                // Always use the hardcoded connection string from DatabaseManager
                string connectionString = OrderManagement.DatabaseManager.ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Query to find the most specific matching postcode prefix
                    string query = "SELECT TOP 1 Charge FROM DeliveryCharges WHERE @postcode LIKE PostcodePrefix + '%' ORDER BY LEN(PostcodePrefix) DESC";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@postcode", postcode);
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToDecimal(result);
                        }
                    }

                    // If no specific match found, get the default charge
                    using (SqlCommand cmd = new SqlCommand("SELECT Charge FROM DeliveryCharges WHERE PostcodePrefix = 'DEFAULT'", conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving delivery charge: " + ex.Message);
            }

            return defaultCharge;
        }

        /// <summary>
        /// Auto-calculate delivery charge based on customer postcode
        /// </summary>
        public static void AutoCalculateDeliveryCharge(frmPOS posForm)
        {
            try
            {
                string postcode = ExtractPostcode(posForm);

                // Calculate delivery charge
                decimal charge = CalculateDeliveryCharge(postcode);

                // Update the delivery charge textbox
                if (posForm.Controls.Find("txtDeliveryCharge", true).FirstOrDefault() is TextBox txtDeliveryCharge)
                {
                    txtDeliveryCharge.Text = $"£{charge:0.00}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error auto-calculating delivery charge: {ex.Message}");
            }
        }

        /// <summary>
        /// Open delivery charge management form
        /// </summary>
        public static void ManageDeliveryCharges(frmPOS posForm)
        {
            try
            {
                // Show the delivery charges management form
                using (View.frmDeliveryCharges deliveryChargesForm = new View.frmDeliveryCharges())
                {
                    deliveryChargesForm.ShowDialog();

                    // Check if delivery mode is active
                    if (posForm.GetType().GetField("currentOrderType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(posForm)?.ToString() == "DELIVERY")
                    {
                        // Recalculate delivery charge
                        AutoCalculateDeliveryCharge(posForm);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error managing delivery charges: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Updates delivery charge when order type changes
        /// </summary>
        public static void OrderTypeChanged(frmPOS posForm, string newOrderType)
        {
            try
            {
                // Find delivery charge controls
                var txtDeliveryCharge = posForm.Controls.Find("txtDeliveryCharge", true).FirstOrDefault() as TextBox;
                var btnDeliveryChargeAmend = posForm.Controls.Find("btnDeliveryChargeAmend", true).FirstOrDefault() as Button;

                if (txtDeliveryCharge != null && btnDeliveryChargeAmend != null)
                {
                    bool isDelivery = newOrderType == "DELIVERY";

                    // Show/hide delivery charge controls
                    txtDeliveryCharge.Visible = isDelivery;
                    btnDeliveryChargeAmend.Visible = isDelivery;

                    // Calculate delivery charge if needed
                    if (isDelivery)
                    {
                        AutoCalculateDeliveryCharge(posForm);
                    }
                    else
                    {
                        txtDeliveryCharge.Text = "£0.00";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling order type change: {ex.Message}");
            }
        }

        /// <summary>
        /// Get delivery charge value from the textbox
        /// </summary>
        public static decimal GetDeliveryCharge(frmPOS posForm)
        {
            try
            {
                if (posForm.Controls.Find("txtDeliveryCharge", true).FirstOrDefault() is TextBox txtDeliveryCharge)
                {
                    decimal.TryParse(txtDeliveryCharge.Text.Replace("£", ""), out decimal charge);
                    return charge;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting delivery charge: {ex.Message}");
            }

            return 0;
        }

        /// <summary>
        /// Set delivery charge textbox value
        /// </summary>
        public static void SetDeliveryCharge(frmPOS posForm, decimal charge)
        {
            try
            {
                if (posForm.Controls.Find("txtDeliveryCharge", true).FirstOrDefault() is TextBox txtDeliveryCharge)
                {
                    txtDeliveryCharge.Text = $"£{charge:0.00}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting delivery charge: {ex.Message}");
            }
        }

        /// <summary>
        /// Ensure delivery charges table exists in database
        /// </summary>
        public static void EnsureDeliveryChargesTableExists()
        {
            try
            {
                string connectionString = OrderManagement.DatabaseManager.ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if table exists
                    string checkTableQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DeliveryCharges'";
                    using (SqlCommand checkCmd = new SqlCommand(checkTableQuery, conn))
                    {
                        int tableExists = (int)checkCmd.ExecuteScalar();

                        if (tableExists == 0)
                        {
                            // Create table if it doesn't exist
                            string createTableQuery = @"
                            CREATE TABLE DeliveryCharges (
                                PostcodePrefix NVARCHAR(10) PRIMARY KEY,
                                Charge DECIMAL(10, 2) NOT NULL
                            )";

                            using (SqlCommand createCmd = new SqlCommand(createTableQuery, conn))
                            {
                                createCmd.ExecuteNonQuery();
                            }

                            // Add default entry
                            string insertDefaultQuery = "INSERT INTO DeliveryCharges (PostcodePrefix, Charge) VALUES ('DEFAULT', 0.00)";
                            using (SqlCommand insertCmd = new SqlCommand(insertDefaultQuery, conn))
                            {
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ensuring delivery charges table exists: {ex.Message}");
            }
        }

        #endregion

        #region Private Helper Methods

        private static void InitializeUI(frmPOS posForm)
        {
            // Hook up delivery charge button click event
            var btnDeliveryChargeAmend = posForm.Controls.Find("btnDeliveryChargeAmend", true).FirstOrDefault() as Button;
            if (btnDeliveryChargeAmend != null)
            {
                // Remove existing event handlers to avoid duplicates
                var clickEvent = typeof(Control).GetField("EventClick", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
                if (clickEvent != null)
                {
                    object obj = clickEvent.GetValue(null);
                    var eventsProp = typeof(Component).GetProperty("Events", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var eventList = eventsProp.GetValue(btnDeliveryChargeAmend, null) as EventHandlerList;
                    eventList?.RemoveHandler(obj, eventList[obj]);
                }

                // Add our event handler
                btnDeliveryChargeAmend.Click += (s, e) => ManageDeliveryCharges(posForm);
            }

            // Ensure delivery charges table exists
            EnsureDeliveryChargesTableExists();
        }

        private static string ExtractPostcode(frmPOS posForm)
        {
            string postcode = "";

            // Try to get postcode from customer data
            var customerPostcodeField = posForm.GetType().GetField("customerPostcode",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (customerPostcodeField != null)
            {
                string customerPostcode = customerPostcodeField.GetValue(posForm) as string;
                if (!string.IsNullOrEmpty(customerPostcode))
                {
                    return customerPostcode;
                }
            }

            // Try to extract from address display
            if (posForm.Controls.Find("lblAddressDisplay", true).FirstOrDefault() is TextBox lblAddressDisplay)
            {
                if (!string.IsNullOrEmpty(lblAddressDisplay.Text))
                {
                    string[] parts = lblAddressDisplay.Text.Split(',');
                    if (parts.Length > 0)
                    {
                        postcode = parts[parts.Length - 1].Trim();
                    }
                }
            }

            return postcode;
        }

        #endregion
    }
}
