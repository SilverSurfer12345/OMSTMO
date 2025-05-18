using System;
using System.Configuration; // Add this namespace
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace OrderManagement.Model
{
    public partial class frmAddFoodItem : Form
    {
        public int id = 0;

        // Replace MainClass.conString with DatabaseManager.ConnectionString
        private static readonly string conString = DatabaseManager.ConnectionString;

        public frmAddFoodItem(string preSelectedItem)
        {
            InitializeComponent();

            // Detach event handlers to prevent double subscription
            btnSave.Click -= btnSave_Click;
            btnClose.Click -= btnClose_Click;
            btnBrowse.Click -= btnBrowse_Click;

            // Subscribe to the Click event of the Save button only once in the constructor
            btnSave.Click += btnSave_Click;
            // Subscribe to the Click event of the Close button
            btnClose.Click += btnClose_Click;
            // Subscribe to the Click event of the Browse button
            btnBrowse.Click += btnBrowse_Click;

            // Call a method to populate the ComboBox with category data
            PopulateCategoryComboBox(preSelectedItem);
        }

        private void PopulateCategoryComboBox(string preSelectedItem)
        {
            // Clear existing items
            cbCategoryItem.Items.Clear();

            try
            {
                using (SqlConnection con = new SqlConnection(conString)) // Use the new conString
                {
                    con.Open();

                    // Fetch category data from the database
                    string query = "SELECT catName FROM category";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Read and load the data from the reader
                        while (reader.Read())
                        {
                            cbCategoryItem.Items.Add(reader["catName"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected Error\nPlease contact support\n" + ex.Message);
            }
            // Preselect an item
            cbCategoryItem.SelectedItem = preSelectedItem;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Check if txtItemName is not empty and a category is selected
            if (!string.IsNullOrEmpty(txtItemName.Text) && cbCategoryItem.SelectedItem != null)
            {
                string itemName = txtItemName.Text;
                string selectedCategory = cbCategoryItem.SelectedItem.ToString();

                // Parse the price from txtPrice.Text
                if (decimal.TryParse(txtPrice.Text, out decimal price))
                {
                    // Save the data to the database
                    SaveDataToDatabase(itemName, selectedCategory, price, txtImage.Image); // Pass the Image parameter
                }
                else
                {
                    MessageBox.Show("Invalid price format. Please enter a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // Display a message indicating that both fields are required
                MessageBox.Show("Please enter the item name and select a category before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

       private void SaveDataToDatabase(string itemName, string selectedCategory, decimal price, Image image)
{
    try
    {
        // First create a safe copy of the image to avoid file locking issues
        Image imageCopy = null;
        bool isImageChanged = image != null; // Track if image is being changed
        
        if (image != null)
        {
            try
            {
                // Create a new bitmap copy to ensure the image isn't locked
                imageCopy = new Bitmap(image);
                Console.WriteLine($"Created image copy: {imageCopy.Width}x{imageCopy.Height}");
            }
            catch (Exception imgEx)
            {
                Console.WriteLine($"Error creating image copy: {imgEx.Message}");
                // Continue without the image if there's an error
            }
        }
        
        using (SqlConnection con = new SqlConnection(conString))
        {
            con.Open();
            using (SqlTransaction transaction = con.BeginTransaction())
            {
                try
                {
                    string query;
                    
                    if (id == 0)
                    {
                        // New item - always include image field
                        query = "INSERT INTO foodItems (category, Item, price, icon) VALUES (@category, @itemName, @price, @icon)";
                    }
                    else
                    {
                        // Update - only update image if a new one is provided
                        if (isImageChanged)
                        {
                            query = "UPDATE foodItems SET category = @category, Item = @itemName, price = @price, icon = @icon WHERE Id = @id";
                        }
                        else
                        {
                            // Don't modify the existing image
                            query = "UPDATE foodItems SET category = @category, Item = @itemName, price = @price WHERE Id = @id";
                        }
                    }
                    
                    using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                    {
                        if (id != 0)
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                            
                        cmd.Parameters.Add("@category", SqlDbType.VarChar, 50).Value = selectedCategory;
                        cmd.Parameters.Add("@itemName", SqlDbType.VarChar, 100).Value = itemName;
                        cmd.Parameters.Add("@price", SqlDbType.Decimal).Value = price;
                        
                        // Only add the icon parameter if we're inserting or explicitly updating it
                        if (id == 0 || isImageChanged)
                        {
                            // Try to convert the image to byte array safely
                            byte[] imageBytes = null;
                            if (imageCopy != null)
                            {
                                try
                                {
                                    imageBytes = ImageToByteArray(imageCopy);
                                    Console.WriteLine($"Converted image to byte array: {imageBytes.Length} bytes");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Failed to convert image: {ex.Message}");
                                    // Continue without the image if conversion fails
                                }
                            }
                            
                            // Use DBNull.Value if no image data
                            cmd.Parameters.Add("@icon", SqlDbType.VarBinary, -1).Value = 
                                (imageBytes != null && imageBytes.Length > 0) ? (object)imageBytes : DBNull.Value;
                        }
                        
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            transaction.Commit();
                            MessageBox.Show("Food item saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            transaction.Rollback();
                            MessageBox.Show("Failed to save food item. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Database error: " + ex.Message);
                }
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // Create an instance of OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set the file dialog properties
            openFileDialog.Title = "Select an Image";
            openFileDialog.Filter = "Image Files (*.bmp; *.jpg; *.png; *.gif)|*.bmp;*.jpg;*.png;*.gif|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;

            // Show the file dialog and check if the user selected a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the selected file name
                string selectedFileName = openFileDialog.FileName;
                try
                {
                    // Display the selected image in the PictureBox
                    txtImage.Image = System.Drawing.Image.FromFile(selectedFileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading the image:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private byte[] ImageToByteArray(Image image)
        {
            // Define max dimensions for the image
            int maxWidth = 200;
            int maxHeight = 150;

            // Resize the image to reduce size
            using (Image resizedImage = ResizeImage(image, maxWidth, maxHeight))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Use JPEG format for better compression
                System.Drawing.Imaging.ImageCodecInfo jpegEncoder = GetEncoder(System.Drawing.Imaging.ImageFormat.Jpeg);
                System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters(1);
                encoderParams.Param[0] = new System.Drawing.Imaging.EncoderParameter(
                    System.Drawing.Imaging.Encoder.Quality, 75L); // 75% quality

                resizedImage.Save(memoryStream, jpegEncoder, encoderParams);
                return memoryStream.ToArray();
            }
        }

        private Image ResizeImage(Image image, int maxWidth, int maxHeight)
        {
            if (image == null)
                return null;

            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return newImage;
        }

        private System.Drawing.Imaging.ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
        {
            var codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private void btnAutoMatchImages_Click(object sender, EventArgs e)
        {
            AutoMatchSingleFoodItemWithImage();
        }

        /// <summary>
        /// Automatically finds and loads a matching image for the current food item only
        /// </summary>
        private void AutoMatchSingleFoodItemWithImage()
        {
            try
            {
                // Get the current item name from the textbox
                string itemName = txtItemName.Text;

                if (string.IsNullOrEmpty(itemName))
                {
                    MessageBox.Show("Please enter a food item name first.",
                        "No Item Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the executable path and calculate the relative path to images
                string executablePath = Path.GetDirectoryName(Application.ExecutablePath);
                // Navigate up from bin/Debug to the project root
                string projectRoot = Path.GetFullPath(Path.Combine(executablePath, @"..\.."));
                string imagesFolder = Path.Combine(projectRoot, "Resources", "OMS FOOD PICTURES");

                Console.WriteLine($"Looking for images in: {imagesFolder}");

                if (!Directory.Exists(imagesFolder))
                {
                    MessageBox.Show($"Images folder not found at: {imagesFolder}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Get all image files from the resources folder
                string[] imageFiles = Directory.GetFiles(imagesFolder, "*.*")
                    .Where(file => {
                        string ext = Path.GetExtension(file).ToLower();
                        return ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".bmp" || ext == ".gif";
                    }).ToArray();

                // Normalize the item name (remove spaces, special chars)
                string normalizedItemName = new string(itemName.Where(c => char.IsLetterOrDigit(c)).ToArray()).ToLower();

                // Try to find a matching image
                string matchedImagePath = null;

                foreach (string imagePath in imageFiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(imagePath).ToLower();
                    string normalizedFileName = new string(fileName.Where(c => char.IsLetterOrDigit(c)).ToArray());

                    // Check if the item name is similar to the file name
                    if (normalizedFileName.Contains(normalizedItemName) || normalizedItemName.Contains(normalizedFileName))
                    {
                        matchedImagePath = imagePath;
                        break;
                    }
                }

                // If a match is found, update the UI
                if (matchedImagePath != null)
                {
                    try
                    {
                        // Load image but create a bitmap copy to avoid file locks
                        using (Image originalImg = Image.FromFile(matchedImagePath))
                        {
                            // Create a copy that won't be locked by the file system
                            Bitmap imgCopy = new Bitmap(originalImg);
                            
                            // Update the PictureBox with the copy
                            txtImage.Image = imgCopy;
                            
                            MessageBox.Show($"Found and loaded matching image: {Path.GetFileName(matchedImagePath)}\n\nClick 'Save' to update the database with this image.",
                                "Match Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception imgEx)
                    {
                        MessageBox.Show($"Error loading image: {imgEx.Message}", "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"No matching image found for '{itemName}'. Please check the image folder or use the Browse button to select an image manually.",
                        "No Match Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error matching food item with image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}