using System;

namespace CanteenManagementApp.MVVM.Model
{
    public class Receipt
    {   
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public DateTime DateTime { get; set; }
        public string PaymentMethod { get; set; }
        public float Total { get; set; }
    }
}
