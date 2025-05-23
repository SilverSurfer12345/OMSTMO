// SafeOperations.cs (Existing helper, but ensure it's robust)
using System;
using System.Data;
using System.Windows.Forms;

namespace OrderManagement.Model
{
    public static class SafeOperations
    {
        public static string SafeGetString(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                return row[columnName].ToString();
            }
            return string.Empty;
        }

        public static int SafeGetInt(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                int result;
                if (int.TryParse(row[columnName].ToString(), out result))
                {
                    return result;
                }
            }
            return 0; // Default or error value
        }

        public static decimal SafeGetDecimal(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                decimal result;
                if (decimal.TryParse(row[columnName].ToString(), out result))
                {
                    return result;
                }
            }
            return 0m; // Default or error value
        }

        public static DateTime SafeGetDateTime(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                DateTime result;
                if (DateTime.TryParse(row[columnName].ToString(), out result))
                {
                    return result;
                }
            }
            return DateTime.MinValue; // Default or error value
        }

        // For DataGridView cells
        public static string SafeGetCellString(DataGridView dataGridView, int rowIndex, string columnName)
        {
            if (rowIndex >= 0 && rowIndex < dataGridView.Rows.Count &&
                dataGridView.Columns.Contains(columnName) &&
                dataGridView.Rows[rowIndex].Cells[columnName].Value != null)
            {
                return dataGridView.Rows[rowIndex].Cells[columnName].Value.ToString();
            }
            return string.Empty;
        }

        public static int SafeGetCellInt(DataGridView dataGridView, int rowIndex, string columnName)
        {
            if (rowIndex >= 0 && rowIndex < dataGridView.Rows.Count &&
                dataGridView.Columns.Contains(columnName) &&
                dataGridView.Rows[rowIndex].Cells[columnName].Value != null)
            {
                int result;
                if (int.TryParse(dataGridView.Rows[rowIndex].Cells[columnName].Value.ToString(), out result))
                {
                    return result;
                }
            }
            return 0;
        }

        public static decimal SafeGetCellDecimal(DataGridView dataGridView, int rowIndex, string columnName)
        {
            if (rowIndex >= 0 && rowIndex < dataGridView.Rows.Count &&
                dataGridView.Columns.Contains(columnName) &&
                dataGridView.Rows[rowIndex].Cells[columnName].Value != null)
            {
                decimal result;
                if (decimal.TryParse(dataGridView.Rows[rowIndex].Cells[columnName].Value.ToString(), out result))
                {
                    return result;
                }
            }
            return 0m;
        }
    }
}