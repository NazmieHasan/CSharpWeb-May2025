namespace HotelApp.Web.ViewModels.Admin.StayManagement
{
    public class GuestAgeStatsViewModel
    {
        public int Babies { get; set; }  
        
        public int Children { get; set; } 
        
        public int Adults { get; set; }     
        
        public int TotalGuests => Babies + Children + Adults;
    }
}
