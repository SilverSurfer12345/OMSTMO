using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
using System.Windows.Forms;

namespace OrderManagement.Model
{
    public partial class frmCustomerAdd : SampleAdd
    {
        // Declare an event using EventHandler delegate
        public event EventHandler SaveClicked;

        public frmCustomerAdd()
        {
            InitializeComponent();


        }



        public int id = 0;
        public static string telephoneNumber;

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrEmpty(txtForename.Text))
                {
                    MessageBox.Show("Please enter a forename.", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtForename.Focus();
                    return;
                }

                // Prepare the SQL query
                string query;
                Dictionary<string, object> parameters = new Dictionary<string, object>();

                if (id == 0)
                {
                    // Insert new customer
                    query = @"
                INSERT INTO customers (forename, surname, telephoneNo, Email, houseNameNumber, AddressLine1, AddressLine2, AddressLine3, AddressLine4, Postcode)
                VALUES (@forename, @surname, @telephoneNo, @Email, @houseNameNumber, @AddressLine1, @AddressLine2, @AddressLine3, @AddressLine4, @Postcode);
                SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    // Update existing customer
                    query = @"
                UPDATE customers 
                SET forename = @forename, surname = @surname, telephoneNo = @telephoneNo, Email = @Email, 
                    houseNameNumber = @houseNameNumber, AddressLine1 = @AddressLine1, AddressLine2 = @AddressLine2, 
                    AddressLine3 = @AddressLine3, AddressLine4 = @AddressLine4, Postcode = @Postcode
                WHERE Id = @Id;";

                    parameters.Add("@Id", id);
                }

                // Add parameters
                parameters.Add("@forename", txtForename.Text);
                parameters.Add("@surname", txtSurname.Text);
                parameters.Add("@telephoneNo", txtTelephoneNo.Text);
                parameters.Add("@Email", txtEmail.Text);
                parameters.Add("@houseNameNumber", txtHouseNameNumber.Text);
                parameters.Add("@AddressLine1", txtAddressLine1.Text);
                parameters.Add("@AddressLine2", txtAddressLine2.Text);
                parameters.Add("@AddressLine3", txtAddressLine3.Text);
                parameters.Add("@AddressLine4", txtAddressLine4.Text);
                parameters.Add("@Postcode", txtPostcode.Text);

                // Execute the query
                if (id == 0)
                {
                    // For insert, get the new ID
                    object result = DatabaseManager.ExecuteScalar(query, parameters);
                    if (result != null)
                    {
                        id = Convert.ToInt32(result);
                        MessageBox.Show("Customer added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    // For update, just execute the query
                    int rowsAffected = DatabaseManager.ExecuteNonQuery(query, parameters);
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                // Set the static telephoneNumber property
                telephoneNumber = txtTelephoneNo.Text;

                // Trigger the SaveClicked event
                SaveClicked?.Invoke(this, EventArgs.Empty);

                // Close the form
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving customer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Force a checkpoint to ensure data is written to disk
            DatabaseManager.ForceCheckpoint();
        }




        private void frmCustomerAdd_Load(object sender, EventArgs e)
        {

        }

        private void frmCustomerAdd_Load_1(object sender, EventArgs e)
        {

        }

        private void txtPostcode_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtHouseNameNumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {

        }

        private void txtForename_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnPostcodeFind_Click(object sender, EventArgs e)
        {
            try
            {
                // Assuming txtPostcode contains the user-entered postcode
                string postcode = txtPostcode.Text.Trim();

                if (string.IsNullOrEmpty(postcode))
                {
                    MessageBox.Show("Please enter a postcode first.");
                    return;
                }

                Cursor = Cursors.WaitCursor;

                // Create an instance of HttpClient
                using (HttpClient httpClient = new HttpClient())
                {
                    // Set the User-Agent header
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "YourUserAgentString");

                    try
                    {
                        // Use the OSM Nominatim API for reverse geocoding
                        string apiUrl = $"https://nominatim.openstreetmap.org/search?postalcode={postcode}&format=json";

                        // Send the GET request and get the response
                        HttpResponseMessage response = httpClient.GetAsync(apiUrl).Result;

                        // Check if the request was successful
                        if (response.IsSuccessStatusCode)
                        {
                            // Read the response content as a string
                            string resultContent = response.Content.ReadAsStringAsync().Result;

                            // Parse the JSON response
                            dynamic result = MainClass.ParseJsonResponse(resultContent);

                            if (result != null && result.Count > 0)
                            {
                                // Assuming the first result is the most relevant
                                dynamic location = result[0];
                                string lat = location.lat;
                                string lon = location.lon;

                                // Perform reverse geocoding to get detailed address information
                                apiUrl = $"https://nominatim.openstreetmap.org/reverse?lat={lat}&lon={lon}&format=json";
                                HttpResponseMessage reverseResponse = httpClient.GetAsync(apiUrl).Result;

                                // Check if the reverse geocoding request was successful
                                if (reverseResponse.IsSuccessStatusCode)
                                {
                                    // Read the reverse geocoding response content as a string
                                    string reverseResultContent = reverseResponse.Content.ReadAsStringAsync().Result;

                                    // Parse the reverse geocoding JSON response
                                    dynamic reverseResult = MainClass.ParseJsonResponse(reverseResultContent);

                                    if (reverseResult != null)
                                    {
                                        string displayName = reverseResult.display_name;

                                        string[] parts = displayName.Split(',');

                                        string addressLine1 = reverseResult.address.road; // Change 'road' to the appropriate OSM attribute
                                        string addressLine3 = reverseResult.address.suburb; // Change 'suburb' to the appropriate OSM attribute
                                        string addressLine4 = reverseResult.address.city;
                                        string addressLine5 = reverseResult.address.state_district;

                                        // Populate the text boxes with the retrieved address information
                                        txtAddressLine1.Text = addressLine1;
                                        if (parts.Length >= 4)
                                        {
                                            txtAddressLine3.Text = parts[4].Trim();
                                            if (txtAddressLine3.Text == "England")
                                            {
                                                txtAddressLine3.Text = addressLine3;
                                            }
                                            if (txtAddressLine3.Text == addressLine5)
                                            {
                                                txtAddressLine3.Text = parts[2].Trim();
                                            }
                                        }
                                        else
                                        {
                                            txtAddressLine3.Text = addressLine3;
                                        }
                                        txtAddressLine4.Text = addressLine4;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Failed to retrieve detailed address information.");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show($"Reverse geocoding request failed with status code: {reverseResponse.StatusCode}");
                                }
                            }
                            else
                            {
                                MessageBox.Show("No results found for the given postcode.");
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Request failed with status code: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle network errors gracefully
                        MessageBox.Show("Could not connect to address lookup service. Please enter the address manually.\n\nError: " +
                            (ex.InnerException?.Message ?? ex.Message),
                            "Network Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    
    }
}

