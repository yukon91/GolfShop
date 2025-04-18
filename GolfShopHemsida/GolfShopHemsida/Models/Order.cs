﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GolfShopHemsida.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public string UserId { get; set; }  
        public GolfShopUser User { get; set; }  

        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }

}
