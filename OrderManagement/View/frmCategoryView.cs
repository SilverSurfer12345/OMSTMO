using OrderManagement.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Linq;


namespace OrderManagement.View
{
    public partial class frmCategoryView : SampleView
    {
        private string selectedCategory;
        public frmCategoryView()
        {
            InitializeComponent();
        }

        public void RefreshDataGrids()
        {
            // Reload data into both data grids
            GetData();
            loadFoodItemData();

            // Refresh the foodItemView grid
            foodItemView.Refresh();
        }

        public void GetData()
        {
            string qry = "Select * From category where catName like '%" + txtSearch.Text + "%' ";
            ListBox lb = new ListBox();

            // Check if lb is not null before iterating over its items
            if (lb != null)
            {
                // Add more column names if needed
                // lb.Items.Add("columnName2");
                // lb.Items.Add("columnName3");

                // Add columns to the DataGridView
                foreach (var columnName in lb.Items)
                {
                    if (!categoryDataView.Columns.Contains(columnName.ToString()))
                    {
                        categoryDataView.Columns.Add(columnName.ToString(), columnName.ToString());
                    }
                }

                // Load data using MainClass.LoadData
                MainClass.LoadData(qry, categoryDataView, lb);
            }
            else
            {
                // Handle the case where lb is null (e.g., log, display a message, etc.)
                MessageBox.Show("ListBox (lb) is not initialized.");
            }
        }

        private void frmCategoryView_Load(object sender, EventArgs e)
        {
            GetData();
        }

        public override void btnAdd_Click(object sender, EventArgs e)
        {
            using (frmCategoryAdd frm = new frmCategoryAdd())
            {
                DialogResult result = frm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    GetData();
                }
            }
        }

        public override void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(categoryDataView.CurrentRow.Cells["dgvid"].Value);

                if (categoryDataView.CurrentCell.OwningColumn.Name == "dgvedit")
                {
                    using (frmCategoryAdd frm = new frmCategoryAdd())
                    {
                        frm.id = id;
                        frm.txtName.Text = Convert.ToString(categoryDataView.CurrentRow.Cells["dgvCatName"].Value);
                        DialogResult result = frm.ShowDialog();

                        if (result == DialogResult.OK)
                        {
                            GetData();
                        }
                    }
                }

                if (categoryDataView.CurrentCell.OwningColumn.Name == "dgvdel")
                {
                    if (MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string qry = "DELETE FROM category WHERE catID = @id";
                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters.Add("@id", id);

                        if (MainClass.SQL(qry, parameters) > 0)
                        {
                            MessageBox.Show("Delete successful.");
                            GetData();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete. Please try again.");
                        }
                    }
                }
            }
            loadFoodItemData();
        }


        public void loadFoodItemData()
        {

            if (categoryDataView.CurrentCell.OwningColumn.Name == "dgvCatName")
            {
                selectedCategory = Convert.ToString(categoryDataView.CurrentRow.Cells["dgvCatName"].Value);
                var paramName = "@category";
                var query = "SELECT Id, Item, price, icon FROM foodItems WHERE category = @category";
                DataTable dt = MainClass.getDataWithImageFromTable(query, paramName, selectedCategory);
                foodItemView.DataSource = dt;

                if (dt != null)
                {
                    foreach (DataGridViewRow row in foodItemView.Rows)
                    {
                        if (!(row.Cells["dgvFoodImage"].Value is DBNull))
                        {
                            byte[] imageBytes = (byte[])row.Cells["dgvFoodImage"].Value;
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                Image image = Image.FromStream(ms);
                                row.Cells["dgvFoodImage"].Value = image;
                            }
                        }
                        else
                        {
                            row.Cells["dgvFoodImage"].Value = new Bitmap(1, 1); // Assign a blank image if the cell is empty
                        }
                    }
                }
            }
        }

        private void btnAddFoodItem_Click(object sender, EventArgs e)
        {
            // Instantiate the frmAddFoodItem form
            using (frmAddFoodItem frm = new frmAddFoodItem(selectedCategory))
            {
                DialogResult result = frm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    loadFoodItemData();
                }
            }
        }

        private void lblAddFoodItem_Click(object sender, EventArgs e)
        {
            // Your code for handling the "Add Food Item" label click
        }

        //category datagridview button click
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {


            if (e.RowIndex >= 0)
            {
                lblFoodItemSearch.Visible = true;
                txtFoodItemSearch.Visible = true;
                txtFoodItemSearch.Enabled = true;

                // Make btnAddFoodItem and lblAddFoodItem visible when clicking on a row
                btnAddFoodItem.Visible = true;
                lblAddFoodItem.Visible = true;

                int id = Convert.ToInt32(categoryDataView.CurrentRow.Cells["dgvid"].Value);

                if (categoryDataView.CurrentCell.OwningColumn.Name == "dgvedit")
                {
                    using (frmCategoryAdd frm = new frmCategoryAdd())
                    {
                        frm.id = id;
                        frm.txtName.Text = Convert.ToString(categoryDataView.CurrentRow.Cells["dgvCatName"].Value);
                        DialogResult result = frm.ShowDialog();

                        if (result == DialogResult.OK)
                        {
                            GetData();
                        }
                    }
                }

                if (categoryDataView.CurrentCell.OwningColumn.Name == "dgvdel")
                {
                    if (MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string qry = "DELETE FROM category WHERE catID = @id";
                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters.Add("@id", id);

                        if (MainClass.SQL(qry, parameters) > 0)
                        {
                            MessageBox.Show("Delete successful.");
                            GetData();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete. Please try again.");
                        }
                    }
                }
            }
            loadFoodItemData();
        }

        private void foodItemView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedCategory = Convert.ToString(categoryDataView.CurrentRow.Cells["dgvCatName"].Value);
                int id = Convert.ToInt32(foodItemView.CurrentRow.Cells["dgvFoodItemId"].Value);

                if (foodItemView.CurrentCell.OwningColumn.Name == "dgvItemEdit")
                {
                    using (frmAddFoodItem frm = new frmAddFoodItem(selectedCategory))
                    {
                        frm.id = id;
                        frm.txtItemName.Text = Convert.ToString(foodItemView.CurrentRow.Cells["dgvFoodItemItem"].Value);
                        frm.txtPrice.Text = Convert.ToString(foodItemView.CurrentRow.Cells["dgvFoodItemPrice"].Value);
                        frm.cbCategoryItem.SelectedItem = selectedCategory;
                        
                        // Load the existing image if available
                        if (!(foodItemView.CurrentRow.Cells["dgvFoodImage"].Value is DBNull) && 
                            !(foodItemView.CurrentRow.Cells["dgvFoodImage"].Value is Bitmap bitmap && bitmap.Width == 1 && bitmap.Height == 1))
                        {
                            // If it's an image, use it directly
                            if (foodItemView.CurrentRow.Cells["dgvFoodImage"].Value is Image existingImage)
                            {
                                frm.txtImage.Image = new Bitmap(existingImage);
                            }
                            // If it's still byte[], convert it to image
                            else if (foodItemView.CurrentRow.Cells["dgvFoodImage"].Value is byte[] imageBytes)
                            {
                                try
                                {
                                    using (MemoryStream ms = new MemoryStream(imageBytes))
                                    {
                                        frm.txtImage.Image = Image.FromStream(ms);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error loading image: " + ex.Message);
                                }
                            }
                        }
                        
                        DialogResult result = frm.ShowDialog();

                        if (result == DialogResult.OK)
                        {
                            loadFoodItemData();
                        }
                    }
                }
                if (foodItemView.CurrentCell.OwningColumn.Name == "dgvItemDel")
                {
                    if (MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string qry = "DELETE FROM foodItems WHERE Id = @id";
                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters.Add("@id", id);

                        if (MainClass.SQL(qry, parameters) > 0)
                        {
                            MessageBox.Show("Delete successful.");
                            loadFoodItemData();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete. Please try again.");
                        }
                    }
                }
            }

        }

        private void txtFoodItemSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFoodItemSearch.Text))
            {
                // If the search box is empty, load all food items
                loadFoodItemData();
            }
            else
            {
                // If the search box is not empty, filter the food items
                var paramName = "@category";
                var query = "SELECT Id, Item, price, icon FROM foodItems WHERE category = @category AND Item LIKE '%" + txtFoodItemSearch.Text + "%'";
                DataTable dt = MainClass.getDataWithImageFromTable(query, paramName, selectedCategory);
                foodItemView.DataSource = dt;

                if (dt != null)
                {
                    foreach (DataGridViewRow row in foodItemView.Rows)
                    {
                        if (!(row.Cells["dgvFoodImage"].Value is DBNull))
                        {
                            byte[] imageBytes = (byte[])row.Cells["dgvFoodImage"].Value;
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                Image image = Image.FromStream(ms);
                                row.Cells["dgvFoodImage"].Value = image;
                            }
                        }
                        else
                        {
                            row.Cells["dgvFoodImage"].Value = new Bitmap(1, 1); // Assign a blank image if the cell is empty
                        }
                    }
                }
            }
        }

        private void AutoMatchFoodItemsWithImages()
        {
            try
            {
                // Get the executable path and calculate the relative path to images
                string executablePath = Path.GetDirectoryName(Application.ExecutablePath);
                // Navigate up from bin/Debug to the project root
                string projectRoot = Path.GetFullPath(Path.Combine(executablePath, @"..\.."));
                string imagesFolder = Path.Combine(projectRoot, "Resources", "OMS FOOD PICTURES");
                
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
                
                // Get all food items from the database
                string query = "SELECT Id, Item FROM foodItems";
                DataTable foodItems = MainClass.getDataFromTable(query, new Dictionary<string, object>());
                
                int updateCount = 0;
                
                if (foodItems != null && foodItems.Rows.Count > 0)
                {
                    foreach (DataRow row in foodItems.Rows)
                    {
                        int id = Convert.ToInt32(row["Id"]);
                        string itemName = row["Item"].ToString();
                        
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
                        
                        // If a match is found, update the database
                        if (matchedImagePath != null)
                        {
                            using (Image img = Image.FromFile(matchedImagePath))
                            {
                                byte[] imageData = ImageToByteArray(img);
                                
                                string updateQuery = "UPDATE foodItems SET icon = @icon WHERE Id = @id";
                                Dictionary<string, object> parameters = new Dictionary<string, object>
                                {
                                    { "@id", id },
                                    { "@icon", imageData }
                                };
                                
                                int result = MainClass.SQL(updateQuery, parameters);
                                if (result > 0)
                                {
                                    updateCount++;
                                }
                            }
                        }
                    }
                    
                    MessageBox.Show($"Successfully updated {updateCount} food items with matching images!", 
                        "Auto-Match Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                    // Refresh the data grids to show the updated images
                    RefreshDataGrids();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error matching food items with images: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Add helper method to convert Image to byte array (copy from frmAddFoodItem.cs)
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
            AutoMatchFoodItemsWithImages();
        }

        private byte[] GetFoodItemImage(int itemId)
        {
            try
            {
                // Query to fetch just the image for this specific item
                string query = "SELECT icon FROM foodItems WHERE Id = @id";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@id", itemId }
                };
                
                DataTable result = MainClass.getDataFromTable(query, parameters);
                
                if (result != null && result.Rows.Count > 0 && !(result.Rows[0]["icon"] is DBNull))
                {
                    return (byte[])result.Rows[0]["icon"];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting food item image: " + ex.Message);
            }
            
            return null;
        }
    }
}
