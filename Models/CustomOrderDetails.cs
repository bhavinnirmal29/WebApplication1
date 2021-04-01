using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class CustomOrderDetails
    {
        public int ProdId { get; set; }
        public string ProdName { get; set; }
        public int OrderId { get; set; }
        public int TotalAmount { get; set; } 
        public string PaymentMode { get; set; }
        public string Status1 { get; set; }
    }
}