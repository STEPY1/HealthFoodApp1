namespace HealthFoodApp.Models
{
    public class Restaurant
    {
        public string Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Rating { get; set; }
        public string DeliveryTime { get; set; }
        public string DeliveryFee { get; set; }
        public string OriginalDeliveryFee { get; set; }
        public List<string> Tags { get; set; }
        public string HealthMatch { get; set; }
        public string HealthReason { get; set; }
        public string Discount { get; set; }
        public string DiscountReason { get; set; }
        public string Address { get; set; }
        public string Hours { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public List<PopularItem> PopularItems { get; set; }
        public List<Review> Reviews { get; set; }
        public string DiscountDisplay => !string.IsNullOrEmpty(Discount) && !string.IsNullOrEmpty(DiscountReason)
            ? $"{Discount} unlocked: {DiscountReason}"
            : null;
    }

    public class PopularItem
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string Image { get; set; }
    }

    public class Review
    {
        public string Name { get; set; }
        public string Rating { get; set; }
        public string Comment { get; set; }
    }
}