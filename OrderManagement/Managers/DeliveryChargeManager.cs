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
    /// Provides delivery charge calculation and management logic.
    /// </summary>
    public static class DeliveryChargeManager
    {
        /// <summary>
        /// Calculates the delivery charge for a given postcode.
        /// </summary>
        public static decimal CalculateDeliveryCharge(string postcode)
        {
            if (string.IsNullOrWhiteSpace(postcode))
                return 0m;

            string normalizedPostcode = postcode.Replace(" ", "").ToUpperInvariant();

            try
            {
                using (SqlConnection con = new SqlConnection(OrderManagement.DatabaseManager.ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(@"
                        SELECT TOP 1 Charge
                        FROM DeliveryCharges
                        WHERE UPPER(@postcode) LIKE UPPER(PostcodePrefix) + '%'
                        ORDER BY LEN(PostcodePrefix) DESC
                    ", con))
                    {
                        cmd.Parameters.AddWithValue("@postcode", normalizedPostcode);

                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            return Convert.ToDecimal(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving delivery charge: " + ex.Message);
            }
            return 0m;
        }

        /// <summary>
        /// Sets the delivery charge in the POS form based on the current postcode.
        /// </summary>
        public static void AutoSetDeliveryCharge(frmPOS posForm)
        {
            string postcode = ExtractPostcodeFromForm(posForm);
            decimal charge = CalculateDeliveryCharge(postcode);
            if (posForm.Controls.Find("txtDeliveryCharge", true).FirstOrDefault() is TextBox txtDeliveryCharge)
            {
                txtDeliveryCharge.Text = $"£{charge:0.00}";
            }
        }

        /// <summary>
        /// Extracts the postcode from the POS form (from address or a dedicated field).
        /// </summary>
        private static string ExtractPostcodeFromForm(frmPOS posForm)
        {
            // Try to get postcode from a field/property if you have one
            var postcodeField = posForm.GetType().GetField("customerPostcode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (postcodeField != null)
            {
                string customerPostcode = postcodeField.GetValue(posForm) as string;
                if (!string.IsNullOrEmpty(customerPostcode))
                    return customerPostcode;
            }
            // Fallback: try to parse from address display
            if (posForm.Controls.Find("txtAddressDisplay", true).FirstOrDefault() is TextBox txtAddressDisplay)
            {
                string[] parts = txtAddressDisplay.Text.Split(',');
                if (parts.Length > 0)
                    return parts[parts.Length - 1].Trim();
            }
            return "";
        }

        /// <summary>
        /// Opens the delivery charge management form.
        /// </summary>
        public static void ShowDeliveryChargeManager(frmPOS posForm)
        {
            using (var form = new frmDeliveryCharges())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // If order type is delivery, recalculate
                    if (posForm.Controls.Find("txtDeliveryCharge", true).FirstOrDefault() is TextBox txtDeliveryCharge
                        && posForm.Controls.Find("btnDel", true).FirstOrDefault() is Button btnDel
                        && btnDel.BackColor == System.Drawing.Color.Green)
                    {
                        AutoSetDeliveryCharge(posForm);
                    }
                }
            }
        }
    }
}