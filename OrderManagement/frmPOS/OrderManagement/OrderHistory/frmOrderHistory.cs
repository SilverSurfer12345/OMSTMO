using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OrderManagement.Presenter; // Import the presenter namespace
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace OrderManagement.View
{
    public partial class frmOrderHistory : Form, IOrderHistoryView
    {
        private IOrderHistoryPresenter _presenter;
        private bool isFormatting = false; // Used for CellFormatting event to prevent recursion

        public frmOrderHistory()
        {
            InitializeComponent();
            _presenter = new OrderHistoryPresenter(this); // Initialize the presenter

            // Wire up UI events to presenter events
            this.Load += (s, e) => _presenter.HandleViewLoaded();
            dtpFromDate.ValueChanged += (s, e) => _presenter.HandleFromDateChanged();
            dtpToDate.ValueChanged += (s, e) => _presenter.HandleToDateChanged();
            cmbOrderType.SelectedIndexChanged += (s, e) => _presenter.HandleOrderTypeFilterChanged();
            cmbPaymentType.SelectedIndexChanged += (s, e) => _presenter.HandlePaymentTypeFilterChanged();
            txtSearchBox.TextChanged += (s, e) => _presenter.HandleSearchBoxTextChanged();
            btnToday.Click += (s, e) => _presenter.HandleTodayClicked();
            btnDownload.Click += (s, e) => _presenter.HandleDownloadClicked();
            btnPrint.Click += (s, e) => _presenter.HandlePrintClicked();
            btnReset.Click += (s, e) => _presenter.HandleResetFiltersClicked();
            btnCalculations.Click += (s, e) => _presenter.HandleCalculationsClicked();
            dgvOrderHistoryView.CellContentClick += (s, e) => _presenter.HandleCellContentClick(e);
            this.FormClosing += (s, e) => _presenter.HandleFormClosing(e);


            // UI-specific DataGridView event handlers
            dgvOrderHistoryView.CellFormatting += dgvOrderHistoryView_CellFormatting;
            dgvOrderHistoryView.CellPainting += dgvOrderHistoryView_CellPainting;

            // Call presenter's initialize method
            _presenter.Initialize();
        }

        // --- IOrderHistoryView Implementation ---

        public object OrderHistoryDataSource
        {
            get => dgvOrderHistoryView.DataSource;
            set => dgvOrderHistoryView.DataSource = value;
        }

        public string SelectedOrderType
        {
            get => cmbOrderType.SelectedItem?.ToString() ?? "ALL";
            set => cmbOrderType.SelectedItem = value;
        }

        public string SelectedPaymentType
        {
            get => cmbPaymentType.SelectedItem?.ToString() ?? "ALL";
            set => cmbPaymentType.SelectedItem = value;
        }

        public string SearchText
        {
            get => txtSearchBox.Text;
            set => txtSearchBox.Text = value;
        }

        public DateTime FromDate
        {
            get => dtpFromDate.Value;
            set => dtpFromDate.Value = value;
        }

        public DateTime ToDate
        {
            get => dtpToDate.Value;
            set => dtpToDate.Value = value;
        }

        public string TotalValueText
        {
            get => txtTotalValue.Text;
            set => txtTotalValue.Text = value;
        }

        public void ShowMessage(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon) => MessageBox.Show(message, title, buttons, icon);
        public DialogResult ShowConfirmation(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon) => MessageBox.Show(message, title, buttons, icon);
        public void CloseView() => this.Close();
        public void RefreshOrderHistoryGrid() => dgvOrderHistoryView.Refresh();

        public void SetOrderTypeItems(IEnumerable<string> items)
        {
            cmbOrderType.Items.Clear();
            cmbOrderType.Items.AddRange(items.ToArray());
            if (cmbOrderType.Items.Contains("ALL"))
            {
                cmbOrderType.SelectedItem = "ALL";
            }
            else if (cmbOrderType.Items.Count > 0)
            {
                cmbOrderType.SelectedIndex = 0;
            }
        }

        public void SetPaymentTypeItems(IEnumerable<string> items)
        {
            cmbPaymentType.Items.Clear();
            cmbPaymentType.Items.AddRange(items.ToArray());
            if (cmbPaymentType.Items.Contains("ALL"))
            {
                cmbPaymentType.SelectedItem = "ALL";
            }
            else if (cmbPaymentType.Items.Count > 0)
            {
                cmbPaymentType.SelectedIndex = 0;
            }
        }

        public void SetDateRangeWithoutEvents(DateTime from, DateTime to)
        {
            // Temporarily unsubscribe to prevent event firing during programmatic change
            dtpFromDate.ValueChanged -= (s, e) => _presenter.HandleFromDateChanged();
            dtpToDate.ValueChanged -= (s, e) => _presenter.HandleToDateChanged();

            dtpFromDate.Value = from;
            dtpToDate.Value = to;

            // Re-subscribe
            dtpFromDate.ValueChanged += (s, e) => _presenter.HandleFromDateChanged();
            dtpToDate.ValueChanged += (s, e) => _presenter.HandleToDateChanged();
        }

        public void SetColumnOrder(string[] columnOrder)
        {
            // Ensure columns exist and are properly formatted before setting the data source
            EnsureColumnsExist();

            foreach (string colName in columnOrder)
            {
                if (dgvOrderHistoryView.Columns.Contains(colName))
                {
                    dgvOrderHistoryView.Columns[colName].DisplayIndex = Array.IndexOf(columnOrder, colName);
                }
            }
        }

        public void ShowCalculationsForm(Form calculationsForm)
        {
            calculationsForm.ShowDialog(this); // Show as a dialog parented by this form
        }

        // Events (View raises these, Presenter subscribes)
        public event EventHandler ViewLoaded;
        public event EventHandler OrderTypeFilterChanged;
        public event EventHandler PaymentTypeFilterChanged;
        public event EventHandler SearchBoxTextChanged;
        public event EventHandler FromDateChanged;
        public event EventHandler ToDateChanged;
        public event EventHandler TodayClicked;
        public event EventHandler DownloadClicked;
        public event EventHandler PrintClicked;
        public event EventHandler ResetFiltersClicked;
        public event EventHandler<DataGridViewCellEventArgs> CellContentClicked;
        public event EventHandler CalculationsClicked;
        public event FormClosingEventHandler FormClosingConfirmed;


        // --- UI-specific methods (remain in View) ---

        private void EnsureColumnsExist()
        {
            // Define column mappings: DataPropertyName -> HeaderText
            Dictionary<string, string> columnMappings = new Dictionary<string, string>
            {
                { "dgvOrderId", "Order ID" },
                { "dgvName", "Customer Name" },
                { "dgvAddress", "Address" },
                { "dgvOrderType", "Order Type" },
                { "dgvOrderDate", "Order Date" },
                { "dgvTotalPrice", "Total Price" },
                { "dgvPayment", "Payment" },
                { "dgvView", "View" },
                { "dgvCancel", "Cancel" },
                { "dgvDelete", "Delete" }
            };

            foreach (var mapping in columnMappings)
            {
                string columnName = mapping.Key;
                string headerText = mapping.Value;

                if (!dgvOrderHistoryView.Columns.Contains(columnName))
                {
                    DataGridViewColumn column;

                    if (columnName == "dgvDelete")
                    {
                        column = new DataGridViewButtonColumn
                        {
                            Name = columnName,
                            HeaderText = headerText,
                            Text = "DELETE",
                            UseColumnTextForButtonValue = true
                        };
                    }
                    else if (columnName == "dgvCancel")
                    {
                        column = new DataGridViewButtonColumn
                        {
                            Name = columnName,
                            HeaderText = headerText,
                            UseColumnTextForButtonValue = false // Text is set in CellFormatting
                        };
                    }
                    else if (columnName == "dgvView")
                    {
                        column = new DataGridViewButtonColumn
                        {
                            Name = columnName,
                            HeaderText = headerText,
                            Text = "VIEW",
                            UseColumnTextForButtonValue = true
                        };
                    }
                    else
                    {
                        column = new DataGridViewTextBoxColumn
                        {
                            Name = columnName,
                            HeaderText = headerText,
                            DataPropertyName = columnName
                        };
                    }

                    dgvOrderHistoryView.Columns.Add(column);
                }
                else
                {
                    // Update the header text for existing columns
                    dgvOrderHistoryView.Columns[columnName].HeaderText = headerText;
                }
            }

            // Format specific columns
            if (dgvOrderHistoryView.Columns.Contains("dgvOrderDate"))
            {
                dgvOrderHistoryView.Columns["dgvOrderDate"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }

            if (dgvOrderHistoryView.Columns.Contains("dgvTotalPrice"))
            {
                dgvOrderHistoryView.Columns["dgvTotalPrice"].DefaultCellStyle.Format = "£0.00";
            }
        }

        private void dgvOrderHistoryView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Ensure the column exists before trying to access its value
                if (dgvOrderHistoryView.Columns.Contains("dgvPayment"))
                {
                    string paymentStatus = dgvOrderHistoryView.Rows[e.RowIndex].Cells["dgvPayment"].Value?.ToString();

                    if (paymentStatus != null && paymentStatus.Equals("CANCELLED", StringComparison.OrdinalIgnoreCase))
                    {
                        if (e.ColumnIndex != dgvOrderHistoryView.Columns["dgvCancel"].Index) // Don't draw line on the cancel button itself
                        {
                            e.PaintBackground(e.ClipBounds, true);
                            e.PaintContent(e.ClipBounds);

                            using (Pen p = new Pen(Color.Red, 2))
                            {
                                Point pt1 = new Point(e.CellBounds.Left, e.CellBounds.Top + e.CellBounds.Height / 2);
                                Point pt2 = new Point(e.CellBounds.Right - 1, e.CellBounds.Top + e.CellBounds.Height / 2);
                                e.Graphics.DrawLine(p, pt1, pt2);
                            }

                            e.Handled = true;
                        }
                    }
                }
            }
        }

        private void dgvOrderHistoryView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string cancelColumnName = "dgvCancel";
            string paymentColumnName = "dgvPayment";

            // Prevent recursion
            if (isFormatting) return;

            if (e.RowIndex >= 0 && e.ColumnIndex == dgvOrderHistoryView.Columns[cancelColumnName].Index)
            {
                isFormatting = true; // Set flag to true

                object paymentStatusObj = dgvOrderHistoryView.Rows[e.RowIndex].Cells[paymentColumnName].Value;
                if (paymentStatusObj != null)
                {
                    string paymentStatus = paymentStatusObj.ToString();
                    e.Value = paymentStatus.Equals("CANCELLED", StringComparison.OrdinalIgnoreCase) ? "UNCANCEL" : "CANCEL";
                }
                isFormatting = false; // Reset flag
            }
        }

        // Static field to track the current instance
        private static frmOrderHistory currentInstance;

        public static void ShowOrderHistory()
        {
            // Check if the current instance exists and is still valid
            if (currentInstance != null && !currentInstance.IsDisposed)
            {
                // If it's minimized, restore it
                if (currentInstance.WindowState == FormWindowState.Minimized)
                    currentInstance.WindowState = FormWindowState.Normal;

                // Bring it to the front
                currentInstance.BringToFront();
                currentInstance.Focus();
            }
            else
            {
                // Create a new instance
                currentInstance = new frmOrderHistory();

                // Subscribe to the FormClosed event to clear the reference
                currentInstance.FormClosed += (s, e) => currentInstance = null;

                // Show the form
                currentInstance.Show();
            }
        }

        // Add a method to close the current instance
        public static void CloseOrderHistory()
        {
            if (currentInstance != null && !currentInstance.IsDisposed)
            {
                currentInstance.Close();
                currentInstance = null;
            }
        }
    }
}
