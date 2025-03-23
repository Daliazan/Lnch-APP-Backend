namespace Backend.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Cuisine { get; set; } = string.Empty;
        public int Distance { get; set; }
        public string PriceRange { get; set; } = string.Empty;
    }
}
 