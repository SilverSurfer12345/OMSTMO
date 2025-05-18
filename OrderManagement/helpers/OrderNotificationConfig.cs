using System;

namespace OrderManagement.Model
{
    public class OrderNotificationConfig
    {
        public string Enabled { get; set; } = "No";
        public OrderAlertConfig CollectionOrder { get; set; } = new OrderAlertConfig();
        public OrderAlertConfig DeliveryOrder { get; set; } = new OrderAlertConfig();
    }

    public class OrderAlertConfig
    {
        public int HighAlertMinutes { get; set; } = 20;
        public int MediumAlertMinutes { get; set; } = 10;
    }
}