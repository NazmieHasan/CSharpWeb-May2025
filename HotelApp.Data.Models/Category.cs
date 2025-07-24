namespace HotelApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    public class Category
    {
        [Comment("Category identifier")]
        public int Id { get; set; }

        [Comment("Category name")]
        public string Name { get; set; } = null!;

        [Comment("Category description")]
        public string Description { get; set; } = null!;

        [Comment("Category beds count")]
        public int Beds { get; set; }

        [Comment("Category price")]
        public decimal Price { get; set; }

        [Comment("Category image URL")]
        public string ImageUrl { get; set; } = null!;

        // TODO: Extract the property with Id to BaseDeletableModel
        [Comment("Shows if category is deleted")]
        public bool IsDeleted { get; set; }
    }
}
