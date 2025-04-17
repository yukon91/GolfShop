using System.ComponentModel.DataAnnotations;

namespace GolfShopHemsida.Models
{
    public class ItemInputModel
    {
        [Required(ErrorMessage = "Måste ha ett namn")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Måste ha en beskrivning")]
        public string Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Priset måste va större än 0")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Måste va 0 eller större")]
        public int Stock { get; set; }
    }
}