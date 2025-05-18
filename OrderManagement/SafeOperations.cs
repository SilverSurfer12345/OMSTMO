using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Text;
using System.Data;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using OrderManagement.View;
using OrderManagement.Model;
using System.Linq;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace OrderManagement.Model
{
    public static class SafeOperations
    {
        public static bool SafeGetValue(DataRow row, string columnName, out object value)
        {
            value = null;
            if (row == null || !row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
            {
                return false;
            }

            value = row[columnName];
            return true;
        }

        public static string SafeGetString(DataRow row, string columnName)
        {
            if (SafeGetValue(row, columnName, out object value))
            {
                return value.ToString();
            }
            return string.Empty;
        }

        public static int SafeGetInt(DataRow row, string columnName)
        {
            if (SafeGetValue(row, columnName, out object value) && int.TryParse(value.ToString(), out int result))
            {
                return result;
            }
            return 0;
        }

        public static bool SafeGetCellValue(DataGridView grid, int rowIndex, string columnName, out object value)
        {
            value = null;
            if (grid == null || rowIndex < 0 || rowIndex >= grid.Rows.Count)
            {
                return false;
            }

            DataGridViewCell cell = grid.Rows[rowIndex].Cells[columnName];
            if (cell == null || cell.Value == null)
            {
                return false;
            }

            value = cell.Value;
            return true;
        }

        public static string SafeGetCellString(DataGridView grid, int rowIndex, string columnName)
        {
            if (SafeGetCellValue(grid, rowIndex, columnName, out object value))
            {
                return value.ToString();
            }
            return string.Empty;
        }

        public static int SafeGetCellInt(DataGridView grid, int rowIndex, string columnName)
        {
            if (SafeGetCellValue(grid, rowIndex, columnName, out object value) && int.TryParse(value.ToString(), out int result))
            {
                return result;
            }
            return 0;
        }

        public static decimal SafeGetCellDecimal(DataGridView grid, int rowIndex, string columnName)
        {
            if (SafeGetCellValue(grid, rowIndex, columnName, out object value) && decimal.TryParse(value.ToString(), out decimal result))
            {
                return result;
            }
            return 0;
        }
    }
}