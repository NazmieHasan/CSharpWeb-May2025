namespace HotelApp.Web.ViewModels
{
    public static class ValidationMessages
    {
        public static class Category
        {
            // Error messages
            public const string NameRequiredMessage = "Name is required.";
            public const string NameMinLengthMessage = "Name must be at least 3 characters.";
            public const string NameMaxLengthMessage = "Name cannot exceed 20 characters.";

            public const string DescriptionRequiredMessage = "Description is required.";
            public const string DescriptionMinLengthMessage = "Description must be at least 10 characters.";
            public const string DescriptionMaxLengthMessage = "Description cannot exceed 1000 characters.";

            public const string BedsRequiredMessage = "Beds count is required.";
            public const string BedsRangeMessage = "Beds count must be between 1 and 10.";

            public const string ImageUrlMaxLengthMessage = "Image URL cannot exceed 2048 characters.";

            public const string ServiceCreateError =
                "Fatal error occurred while adding your category! Please try again later!";
        }
    }
}
