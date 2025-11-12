namespace HotelApp.Data.Common
{
    public static class EntityConstants
    {
        public static class Category
        {
            /// <summary>
            /// Category Name should be at least 3 characters and up to 20 characters.
            /// </summary>
            public const int NameMinLength = 3;

            /// <summary>
            /// Category Name should be able to store text with length up to 20 characters.
            /// </summary>
            public const int NameMaxLength = 20;

            /// <summary>
            /// Category Description must be at least 10 characters.
            /// </summary>
            public const int DescriptionMinLength = 10;

            /// <summary>
            /// Category Description should be able to store text with length up to 1000 characters.
            /// </summary>
            public const int DescriptionMaxLength = 1000;

            /// <summary>
            /// Category Beds should be between 1 and 10 count.
            /// </summary>
            public const int BedsMin = 1;

            /// <summary>
            /// Category Beds should be between 1 and 10 count.
            /// </summary>
            public const int BedsMax = 10;

            /// <summary>
            /// Maximum allowed length for image URL.
            /// </summary>
            public const int ImageUrlMaxLength = 2048;
        }

        public static class Room
        {
            /// <summary>
            /// Room Name should be at least 1 characters and up to 5 characters.
            /// </summary>
            public const int NameMinLength = 1;

            /// <summary>
            /// Room Name should be able to store text with length up to 5 characters.
            /// </summary>
            public const int NameMaxLength = 5;
        }

        public static class Manager
        {
            public const int EmailMinLength = 5;
        }
    }
}
