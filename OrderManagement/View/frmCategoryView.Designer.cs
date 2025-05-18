namespace OrderManagement.View
{
    partial class frmCategoryView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label3 = new System.Windows.Forms.Label();
            this.categoryDataView = new System.Windows.Forms.DataGridView();
            this.dgvid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCatName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvdel = new System.Windows.Forms.DataGridViewImageColumn();
            this.dgvedit = new System.Windows.Forms.DataGridViewImageColumn();
            this.catIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.catNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.categoryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rMDataSet = new OrderManagement.RMDataSet();
            this.categoryTableAdapter = new OrderManagement.RMDataSetTableAdapters.categoryTableAdapter();
            this.foodItemView = new System.Windows.Forms.DataGridView();
            this.dgvFoodItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvFoodItemItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvFoodItemPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvFoodImage = new System.Windows.Forms.DataGridViewImageColumn();
            this.dgvItemEdit = new System.Windows.Forms.DataGridViewImageColumn();
            this.dgvItemDel = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnAddFoodItem = new System.Windows.Forms.PictureBox();
            this.lblAddFoodItem = new System.Windows.Forms.Label();
            this.txtFoodItemSearch = new System.Windows.Forms.TextBox();
            this.lblFoodItemSearch = new System.Windows.Forms.Label();
            this.btnAutoMatchImages = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.categoryDataView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.categoryBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.foodItemView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddFoodItem)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 110);
            this.label1.Size = new System.Drawing.Size(155, 28);
            this.label1.Text = "Category Search";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(484, 77);
            this.label2.Size = new System.Drawing.Size(0, 38);
            this.label2.Text = "";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(12, 143);
            this.txtSearch.Size = new System.Drawing.Size(259, 50);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(23, 26);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(84, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(206, 38);
            this.label3.TabIndex = 6;
            this.label3.Text = "Add Categories";
            // 
            // categoryDataView
            // 
            this.categoryDataView.AllowUserToAddRows = false;
            this.categoryDataView.AllowUserToDeleteRows = false;
            this.categoryDataView.AutoGenerateColumns = false;
            this.categoryDataView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.categoryDataView.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(234)))), ((int)(((byte)(237)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.categoryDataView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.categoryDataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.categoryDataView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvid,
            this.dgvCatName,
            this.dgvdel,
            this.dgvedit,
            this.catIDDataGridViewTextBoxColumn,
            this.catNameDataGridViewTextBoxColumn});
            this.categoryDataView.DataSource = this.categoryBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.categoryDataView.DefaultCellStyle = dataGridViewCellStyle2;
            this.categoryDataView.Location = new System.Drawing.Point(12, 189);
            this.categoryDataView.Name = "categoryDataView";
            this.categoryDataView.ReadOnly = true;
            this.categoryDataView.RowHeadersVisible = false;
            this.categoryDataView.RowHeadersWidth = 62;
            this.categoryDataView.RowTemplate.Height = 28;
            this.categoryDataView.Size = new System.Drawing.Size(259, 754);
            this.categoryDataView.TabIndex = 7;
            this.categoryDataView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // dgvid
            // 
            this.dgvid.DataPropertyName = "catID";
            this.dgvid.HeaderText = "id";
            this.dgvid.MinimumWidth = 8;
            this.dgvid.Name = "dgvid";
            this.dgvid.ReadOnly = true;
            this.dgvid.Visible = false;
            this.dgvid.Width = 26;
            // 
            // dgvCatName
            // 
            this.dgvCatName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvCatName.DataPropertyName = "catName";
            this.dgvCatName.HeaderText = "Category";
            this.dgvCatName.MinimumWidth = 8;
            this.dgvCatName.Name = "dgvCatName";
            this.dgvCatName.ReadOnly = true;
            // 
            // dgvdel
            // 
            this.dgvdel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvdel.FillWeight = 50F;
            this.dgvdel.HeaderText = "";
            this.dgvdel.Image = global::OrderManagement.Properties.Resources.recycle_icon1;
            this.dgvdel.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dgvdel.MinimumWidth = 50;
            this.dgvdel.Name = "dgvdel";
            this.dgvdel.ReadOnly = true;
            // 
            // dgvedit
            // 
            this.dgvedit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvedit.FillWeight = 50F;
            this.dgvedit.HeaderText = "";
            this.dgvedit.Image = global::OrderManagement.Properties.Resources.pen_tool_icon1;
            this.dgvedit.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dgvedit.MinimumWidth = 50;
            this.dgvedit.Name = "dgvedit";
            this.dgvedit.ReadOnly = true;
            // 
            // catIDDataGridViewTextBoxColumn
            // 
            this.catIDDataGridViewTextBoxColumn.DataPropertyName = "catID";
            this.catIDDataGridViewTextBoxColumn.HeaderText = "catID";
            this.catIDDataGridViewTextBoxColumn.MinimumWidth = 8;
            this.catIDDataGridViewTextBoxColumn.Name = "catIDDataGridViewTextBoxColumn";
            this.catIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.catIDDataGridViewTextBoxColumn.Visible = false;
            this.catIDDataGridViewTextBoxColumn.Width = 66;
            // 
            // catNameDataGridViewTextBoxColumn
            // 
            this.catNameDataGridViewTextBoxColumn.DataPropertyName = "catName";
            this.catNameDataGridViewTextBoxColumn.HeaderText = "catName";
            this.catNameDataGridViewTextBoxColumn.MinimumWidth = 8;
            this.catNameDataGridViewTextBoxColumn.Name = "catNameDataGridViewTextBoxColumn";
            this.catNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.catNameDataGridViewTextBoxColumn.Visible = false;
            this.catNameDataGridViewTextBoxColumn.Width = 88;
            // 
            // categoryBindingSource
            // 
            this.categoryBindingSource.DataMember = "category";
            this.categoryBindingSource.DataSource = this.rMDataSet;
            // 
            // rMDataSet
            // 
            this.rMDataSet.DataSetName = "RMDataSet";
            this.rMDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // categoryTableAdapter
            // 
            this.categoryTableAdapter.ClearBeforeFill = true;
            // 
            // foodItemView
            // 
            this.foodItemView.AllowUserToAddRows = false;
            this.foodItemView.AllowUserToDeleteRows = false;
            this.foodItemView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.foodItemView.BackgroundColor = System.Drawing.Color.White;
            this.foodItemView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.foodItemView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvFoodItemId,
            this.dgvFoodItemItem,
            this.dgvFoodItemPrice,
            this.dgvFoodImage,
            this.dgvItemEdit,
            this.dgvItemDel});
            this.foodItemView.Location = new System.Drawing.Point(277, 189);
            this.foodItemView.Name = "foodItemView";
            this.foodItemView.RowHeadersVisible = false;
            this.foodItemView.RowHeadersWidth = 62;
            this.foodItemView.Size = new System.Drawing.Size(482, 754);
            this.foodItemView.TabIndex = 8;
            this.foodItemView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.foodItemView_CellContentClick);
            // 
            // dgvFoodItemId
            // 
            this.dgvFoodItemId.DataPropertyName = "Id";
            this.dgvFoodItemId.HeaderText = "ID";
            this.dgvFoodItemId.MinimumWidth = 8;
            this.dgvFoodItemId.Name = "dgvFoodItemId";
            this.dgvFoodItemId.Visible = false;
            this.dgvFoodItemId.Width = 37;
            // 
            // dgvFoodItemItem
            // 
            this.dgvFoodItemItem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvFoodItemItem.DataPropertyName = "item";
            this.dgvFoodItemItem.HeaderText = "Item";
            this.dgvFoodItemItem.MinimumWidth = 8;
            this.dgvFoodItemItem.Name = "dgvFoodItemItem";
            this.dgvFoodItemItem.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvFoodItemPrice
            // 
            this.dgvFoodItemPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvFoodItemPrice.DataPropertyName = "price";
            this.dgvFoodItemPrice.HeaderText = "Price";
            this.dgvFoodItemPrice.MinimumWidth = 8;
            this.dgvFoodItemPrice.Name = "dgvFoodItemPrice";
            this.dgvFoodItemPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvFoodImage
            // 
            this.dgvFoodImage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvFoodImage.DataPropertyName = "icon";
            this.dgvFoodImage.HeaderText = "Image";
            this.dgvFoodImage.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dgvFoodImage.MinimumWidth = 8;
            this.dgvFoodImage.Name = "dgvFoodImage";
            this.dgvFoodImage.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // dgvItemEdit
            // 
            this.dgvItemEdit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvItemEdit.FillWeight = 50F;
            this.dgvItemEdit.HeaderText = "";
            this.dgvItemEdit.Image = global::OrderManagement.Properties.Resources.pen_tool_icon;
            this.dgvItemEdit.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dgvItemEdit.MinimumWidth = 50;
            this.dgvItemEdit.Name = "dgvItemEdit";
            // 
            // dgvItemDel
            // 
            this.dgvItemDel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvItemDel.FillWeight = 50F;
            this.dgvItemDel.HeaderText = "";
            this.dgvItemDel.Image = global::OrderManagement.Properties.Resources.recycle_icon;
            this.dgvItemDel.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dgvItemDel.MinimumWidth = 50;
            this.dgvItemDel.Name = "dgvItemDel";
            // 
            // btnAddFoodItem
            // 
            this.btnAddFoodItem.Image = global::OrderManagement.Properties.Resources.plus_square_icon1;
            this.btnAddFoodItem.Location = new System.Drawing.Point(283, 26);
            this.btnAddFoodItem.Name = "btnAddFoodItem";
            this.btnAddFoodItem.Size = new System.Drawing.Size(55, 55);
            this.btnAddFoodItem.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnAddFoodItem.TabIndex = 10;
            this.btnAddFoodItem.TabStop = false;
            this.btnAddFoodItem.Click += new System.EventHandler(this.btnAddFoodItem_Click);
            // 
            // lblAddFoodItem
            // 
            this.lblAddFoodItem.AutoSize = true;
            this.lblAddFoodItem.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddFoodItem.Location = new System.Drawing.Point(349, 40);
            this.lblAddFoodItem.Name = "lblAddFoodItem";
            this.lblAddFoodItem.Size = new System.Drawing.Size(200, 38);
            this.lblAddFoodItem.TabIndex = 11;
            this.lblAddFoodItem.Text = "Add Food Item";
            this.lblAddFoodItem.Click += new System.EventHandler(this.lblAddFoodItem_Click);
            // 
            // txtFoodItemSearch
            // 
            this.txtFoodItemSearch.Enabled = false;
            this.txtFoodItemSearch.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFoodItemSearch.Location = new System.Drawing.Point(277, 143);
            this.txtFoodItemSearch.Name = "txtFoodItemSearch";
            this.txtFoodItemSearch.Size = new System.Drawing.Size(482, 50);
            this.txtFoodItemSearch.TabIndex = 12;
            this.txtFoodItemSearch.TextChanged += new System.EventHandler(this.txtFoodItemSearch_TextChanged);
            // 
            // lblFoodItemSearch
            // 
            this.lblFoodItemSearch.AutoSize = true;
            this.lblFoodItemSearch.Location = new System.Drawing.Point(279, 110);
            this.lblFoodItemSearch.Name = "lblFoodItemSearch";
            this.lblFoodItemSearch.Size = new System.Drawing.Size(114, 28);
            this.lblFoodItemSearch.TabIndex = 13;
            this.lblFoodItemSearch.Text = "Item Search";
            // 
            // btnAutoMatchImages
            // 
            this.btnAutoMatchImages.Location = new System.Drawing.Point(555, 30);
            this.btnAutoMatchImages.Name = "btnAutoMatchImages";
            this.btnAutoMatchImages.Size = new System.Drawing.Size(158, 65);
            this.btnAutoMatchImages.TabIndex = 14;
            this.btnAutoMatchImages.Text = "Auto-Match All Images";
            this.btnAutoMatchImages.UseVisualStyleBackColor = true;
            this.btnAutoMatchImages.Click += new System.EventHandler(this.btnAutoMatchImages_Click);
            // 
            // frmCategoryView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1456, 943);
            this.Controls.Add(this.btnAutoMatchImages);
            this.Controls.Add(this.lblFoodItemSearch);
            this.Controls.Add(this.txtFoodItemSearch);
            this.Controls.Add(this.lblAddFoodItem);
            this.Controls.Add(this.btnAddFoodItem);
            this.Controls.Add(this.foodItemView);
            this.Controls.Add(this.categoryDataView);
            this.Controls.Add(this.label3);
            this.Name = "frmCategoryView";
            this.Text = "frmCategoryView";
            this.Load += new System.EventHandler(this.frmCategoryView_Load);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.categoryDataView, 0);
            this.Controls.SetChildIndex(this.foodItemView, 0);
            this.Controls.SetChildIndex(this.btnAddFoodItem, 0);
            this.Controls.SetChildIndex(this.lblAddFoodItem, 0);
            this.Controls.SetChildIndex(this.txtFoodItemSearch, 0);
            this.Controls.SetChildIndex(this.lblFoodItemSearch, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtSearch, 0);
            this.Controls.SetChildIndex(this.btnAdd, 0);
            this.Controls.SetChildIndex(this.btnAutoMatchImages, 0);
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.categoryDataView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.categoryBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.foodItemView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddFoodItem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView categoryDataView;
        private RMDataSet rMDataSet;
        private System.Windows.Forms.BindingSource categoryBindingSource;
        private RMDataSetTableAdapters.categoryTableAdapter categoryTableAdapter;
        public System.Windows.Forms.PictureBox btnAddFoodItem;
        public System.Windows.Forms.Label lblAddFoodItem;
        public System.Windows.Forms.TextBox txtFoodItemSearch;
        public System.Windows.Forms.DataGridView foodItemView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvCatName;
        private System.Windows.Forms.DataGridViewImageColumn dgvdel;
        private System.Windows.Forms.DataGridViewImageColumn dgvedit;
        private System.Windows.Forms.DataGridViewTextBoxColumn catIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn catNameDataGridViewTextBoxColumn;
        public System.Windows.Forms.Label lblFoodItemSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvFoodItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvFoodItemItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvFoodItemPrice;
        private System.Windows.Forms.DataGridViewImageColumn dgvFoodImage;
        private System.Windows.Forms.DataGridViewImageColumn dgvItemEdit;
        private System.Windows.Forms.DataGridViewImageColumn dgvItemDel;
        private System.Windows.Forms.Button btnAutoMatchImages;
    }
}