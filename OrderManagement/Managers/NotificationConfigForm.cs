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
    public class NotificationConfigManager
    {
        private readonly string configPath;

        public NotificationConfigManager(string configPath)
        {
            this.configPath = configPath;
        }

        public OrderNotificationConfig LoadConfig()
        {
            if (!File.Exists(configPath))
            {
                return CreateDefaultConfig();
            }

            try
            {
                var configJson = File.ReadAllText(configPath);
                return JsonConvert.DeserializeObject<OrderNotificationConfig>(configJson) ?? CreateDefaultConfig();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading config: {ex.Message}");
                return CreateDefaultConfig();
            }
        }

        public bool SaveConfig(OrderNotificationConfig config)
        {
            try
            {
                string configJson = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(configPath, configJson);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving config: {ex.Message}");
                return false;
            }
        }

        private OrderNotificationConfig CreateDefaultConfig()
        {
            return new OrderNotificationConfig
            {
                Enabled = "No",
                CollectionOrder = new OrderAlertConfig { HighAlertMinutes = 20, MediumAlertMinutes = 10 },
                DeliveryOrder = new OrderAlertConfig { HighAlertMinutes = 60, MediumAlertMinutes = 45 }
            };
        }

        public static string GetConfigPath()
        {
            try
            {
                string solutionDir = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;
                if (string.IsNullOrEmpty(solutionDir))
                {
                    return null;
                }

                string configPath = Path.Combine(solutionDir, "OrderManagement", "config", "orderNotificationConfig.json");
                return configPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting config path: {ex.Message}");
                return null;
            }
        }
    }
}