namespace WebAPItest.Models
{
    public class FoodItem
    {
        public string? Id { get; set; }
        public string? Image { get; set; }
        public string? Title { get; set; }
        public string? Restaurant { get; set; }
        public string? Price { get; set; }
        public string? OriginalPrice { get; set; }
        public string? Discount { get; set; }
        public string? DiscountReason { get; set; }
        public string? HealthTag { get; set; }
        public string? HealthReason { get; set; }
        public string? Description { get; set; }
        public Nutrition? Nutrition { get; set; }
        public List<string>? Ingredients { get; set; }
        public bool IsHealthFocused { get; set; }
    }

    public class Nutrition
    {
        public int Calories { get; set; }
        public string? Protein { get; set; }
        public string? Carbs { get; set; }
        public string? Fat { get; set; }
    }
}
