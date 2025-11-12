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

        public static class Room
        {
            // Error messages
            public const string NameRequiredMessage = "Name is required.";
            public const string NameMinLengthMessage = "Name must be at least 1 characters.";
            public const string NameMaxLengthMessage = "Name cannot exceed 5 characters.";

            public const string CategoryRequiredMessage = "Please select a category.";

            public const string ServiceCreateError =
                "Fatal error occurred while adding your category! Please try again later!";
        }

        public static class Booking
        {
            // Error messages
            public const string AdultRequiredMessage = "Adult is required.";
            public const string AdultsMinCountMessage = "Adult min count is 1";

            public const string DateArrivalPastMessage = "Arrival date cannot be in the past.";
            public const string DateDepartureBeforeArrivalMessage = "Departure date must be after arrival date.";

            public const string ServiceCreateError = "Error occurred while adding the booking! Please, send valid data!";
            public const string ServiceCreateExceptionError ="Unexpected error occurred while adding the booking! Please contact developer team!";
        }
    }
}
