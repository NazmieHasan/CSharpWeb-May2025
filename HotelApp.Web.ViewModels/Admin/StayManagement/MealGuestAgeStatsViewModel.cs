namespace HotelApp.Web.ViewModels.Admin.StayManagement
{
    public class MealGuestAgeStatsViewModel
    {
        public int BreakfastAdults { get; set; }

        public int BreakfastChildren { get; set; }

        public int BreakfastBabies { get; set; }

        public int BreakfastGuest => BreakfastAdults + BreakfastChildren + BreakfastBabies;

        public int LunchAdults { get; set; }

        public int LunchChildren { get; set; }

        public int LunchBabies { get; set; }

        public int LunchGuest => LunchAdults + LunchChildren + LunchBabies;

        public int DinnerAdults { get; set; }

        public int DinnerChildren { get; set; }

        public int DinnerBabies { get; set; }

        public int DinnerGuest => DinnerAdults + DinnerChildren + DinnerBabies;
    }
}
