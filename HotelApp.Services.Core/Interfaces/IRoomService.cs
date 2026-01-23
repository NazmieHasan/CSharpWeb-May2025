namespace HotelApp.Services.Core.Interfaces
{
    using HotelApp.Web.ViewModels.Booking;
    using HotelApp.Web.ViewModels.Room;

    public interface IRoomService
    {
        Task<RoomDetailsViewModel?> GetRoomDetailsByIdAsync(string? id);

        Task<IEnumerable<AllRoomsIndexViewModel>> FindRoomByDateArrivaleAndDateDepartureAsync(FindRoomInputModel inputModel);

        Task<IEnumerable<AllRoomsIndexViewModel>> FindRoomByDateArrivaleDateDepartureAndCategoryAsync(FindRoomInputModel inputModel);
    }
}
