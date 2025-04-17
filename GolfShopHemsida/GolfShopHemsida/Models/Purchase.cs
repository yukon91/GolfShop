using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GolfShopHemsida.Models
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }

        public string GolfShopUserId { get; set; }

        [ForeignKey("GolfShopUserId")]
        public GolfShopUser User { get; set; }

        public DateTime PurchaseDate { get; set; }

        public string ProductName { get; set; }

        public decimal TotalAmount { get; set; }
    }

}