namespace HotelApp.Web.ViewModels.Admin.BookingManagement
{
    using HotelApp.Web.ViewModels.Admin.StayManagement;
    using HotelApp.Web.ViewModels.Admin.PaymentManagement;

    public class BookingManagementDetailsViewModel : BookingManagementIndexViewModel
    {
        public string UserEmail { get; set; } = null!;

        public string Owner { get; set; } = null!;

        public bool IsForAnotherPerson { get; set; }

        public string? ManagerEmail { get; set; }

        public string RoomId { get; set; } = null!;

        public string Room { get; set; } = null!;

        public string RoomCategory { get; set; } = null!;

        public decimal TotalAmount { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal RemainingAmount { get; set; }

        public int DaysCount { get; set; }

        public int AdultsCount { get; set; }

        public int ChildCount { get; set; }

        public int BabyCount { get; set; }

        public int AllowedGuestCount { get; set; }

        // Add payment or Add stay or null
        public string AllowedOperation { get; set; } = "None";

        public IEnumerable<StayManagementDetailsViewModel> Stays { get; set; } = new List<StayManagementDetailsViewModel>();

        public IEnumerable<PaymentManagementDetailsViewModel> Payments { get; set; } = new List<PaymentManagementDetailsViewModel>();

    }
}
