namespace HotelApp.Web.ViewModels.Admin.BookingManagement
{
    using HotelApp.Web.ViewModels.Admin.StayManagement;
    using HotelApp.Web.ViewModels.Admin.PaymentManagement;

    public class BookingManagementDetailsViewModel
    {
        public string Id { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public string UserEmail { get; set; } = null!;

        public string? ManagerEmail { get; set; }

        public string Room { get; set; } = null!;

        public string RoomCategory { get; set; } = null!;

        public decimal TotalAmount { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal RemainingAmount { get; set; }

        public string Status { get; set; } = null!;

        public DateOnly DateArrival { get; set; }

        public DateOnly DateDeparture { get; set; }

        public int DaysCount { get; set; }

        public int AdultsCount { get; set; }

        public int ChildCount { get; set; }

        public int BabyCount { get; set; }

        public bool IsDeleted { get; set; }

        public int AllowedGuestCount { get; set; }

        public IEnumerable<StayManagementDetailsViewModel> Stays { get; set; } = new List<StayManagementDetailsViewModel>();

        public IEnumerable<PaymentManagementDetailsViewModel> Payments { get; set; } = new List<PaymentManagementDetailsViewModel>();

    }
}
