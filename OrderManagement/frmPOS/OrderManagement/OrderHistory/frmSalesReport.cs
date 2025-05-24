using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderManagement.View
{
    public partial class frmSalesReport : Form
    {
        public frmSalesReport()
        {
            InitializeComponent();
        }

        public void LoadReportData(DateTime fromDate, DateTime toDate,
                                  Dictionary<string, int> orderTypeCount,
                                  Dictionary<string, decimal> orderTypeTotal,
                                  Dictionary<string, int> paymentTypeCount,
                                  Dictionary<string, decimal> paymentTypeTotal,
                                  int totalOrders, decimal grandTotal)
        {
            lblDateRange.Text = $"Sales Report: {fromDate:dd/MM/yyyy} to {toDate:dd/MM/yyyy}";

            // Clear existing controls
            tblReport.Controls.Clear();
            tblReport.RowStyles.Clear();
            tblReport.RowCount = 0;

            int row = 0;

            // Header
            AddHeaderRow(tblReport, "SALES SUMMARY", row++, 2);

            // Order Type Breakdown
            AddHeaderRow(tblReport, "ORDER TYPES", row++, 2);

            foreach (var orderType in orderTypeCount.Keys)
            {
                AddDataRow(tblReport, orderType,
                          $"{orderTypeCount[orderType]} orders - {orderTypeTotal[orderType]:C2}", row++);
            }

            AddLineSeparator(tblReport, row++, 2);

            // Payment Type Breakdown
            AddHeaderRow(tblReport, "PAYMENT TYPES", row++, 2);

            foreach (var paymentType in paymentTypeCount.Keys)
            {
                AddDataRow(tblReport, paymentType,
                          $"{paymentTypeCount[paymentType]} payments - {paymentTypeTotal[paymentType]:C2}", row++);
            }

            AddLineSeparator(tblReport, row++, 2);

            // Summary
            AddSummaryRow(tblReport, "TOTAL ORDERS:", totalOrders.ToString(), row++);
            AddSummaryRow(tblReport, "GRAND TOTAL:", grandTotal.ToString("C2"), row++);
        }

        private void AddHeaderRow(TableLayoutPanel table, string text, int row, int columnSpan)
        {
            Label header = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Margin = new Padding(5, 10, 5, 5)
            };

            EnsureRowExists(table, row);
            table.Controls.Add(header, 0, row);
            table.SetColumnSpan(header, columnSpan);
        }

        private void AddDataRow(TableLayoutPanel table, string label, string value, int row)
        {
            Label labelControl = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(15, 3, 5, 3)
            };

            Label valueControl = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Fill,
                Margin = new Padding(5, 3, 15, 3)
            };

            EnsureRowExists(table, row);
            table.Controls.Add(labelControl, 0, row);
            table.Controls.Add(valueControl, 1, row);
        }

        private void AddSummaryRow(TableLayoutPanel table, string label, string value, int row)
        {
            Label labelControl = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(15, 5, 5, 5)
            };

            Label valueControl = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Fill,
                Margin = new Padding(5, 5, 15, 5)
            };

            EnsureRowExists(table, row);
            table.Controls.Add(labelControl, 0, row);
            table.Controls.Add(valueControl, 1, row);
        }

        private void AddLineSeparator(TableLayoutPanel table, int row, int columnSpan)
        {
            Panel separator = new Panel
            {
                Height = 1,
                BackColor = Color.Gray,
                Dock = DockStyle.Fill,
                Margin = new Padding(10, 5, 10, 5)
            };

            EnsureRowExists(table, row);
            table.Controls.Add(separator, 0, row);
            table.SetColumnSpan(separator, columnSpan);
        }

        private void EnsureRowExists(TableLayoutPanel table, int rowIndex)
        {
            while (table.RowCount <= rowIndex)
            {
                table.RowCount++;
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }
        }
    }
}