namespace HotelApp.Services.Tests
{
    using Moq;
    using MockQueryable.Moq;

    using Core;
    using Core.Interfaces;
    using Data.Models;
    using Data.Repository.Interfaces;
    using Web.ViewModels.Room;
    
    [TestFixture]
    public class RoomServiceTests
    {
        private Mock<IRoomRepository> roomRepositoryMock;
        private Mock<IBookingRepository> bookingRepositoryMock;
        private Mock<IStayRepository> stayRepositoryMock;

        private IRoomService roomService;

        [SetUp]
        public void Setup()
        {
            this.roomRepositoryMock = new Mock<IRoomRepository>(MockBehavior.Strict);
            this.bookingRepositoryMock = new Mock<IBookingRepository>(MockBehavior.Strict);
            this.stayRepositoryMock = new Mock<IStayRepository>(MockBehavior.Strict);

            this.roomService = new RoomService(
                this.roomRepositoryMock.Object,
                this.bookingRepositoryMock.Object,
                this.stayRepositoryMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        [Test]
        public async Task GetRoomDetailsByIdAsyncShouldReturnNullWithNullRoomId()
        {
            var roomVm = await this.roomService.GetRoomDetailsByIdAsync(null);

            Assert.IsNull(roomVm, "GetRoomDetailsByIdAsync should return null with null roomId!");
        }


        [Test]
        public async Task GetRoomDetailsByIdAsyncShouldReturnRoomDetailsWithExistingRoomId()
        {
            string nonExistingRoomId = "018e23fa-4511-4ced-9532-bd2c200e57cb";

            var category = new Category
            {
                Id = 1,
                Name = "Standard",
                Description = "Modern and stylish design for you",
                Beds = 2,
                Price = 100.00M,
                ImageUrl = "https://cdn.pixabay.com/photo/2015/11/06/11/45/interior-1026452_960_720.jpg"
            };

            List<Room> roomList = new List<Room>()
            {
                new Room()
                {
                    Id = Guid.Parse("50fc7855-3fc5-4c4c-a494-29eaa51e1035"),
                    Name = "101",
                    Category = category,
                    CategoryId = category.Id
                },
                new Room()
                {
                    Id = Guid.Parse("83388c3e-dd01-4268-b46a-e3151e464969"),
                    Name = "102",
                    Category = category,
                    CategoryId = category.Id
                }
            };

            IQueryable<Room> roomQueryable = roomList.BuildMock();

            this.roomRepositoryMock
                .Setup(rr => rr.GetAllAttached())
                .Returns(roomQueryable);

            RoomDetailsViewModel? roomVm = await this.roomService
                .GetRoomDetailsByIdAsync(nonExistingRoomId);

            Assert.IsNull(roomVm, "GetRoomAsync should return null for non-existing Ids!");
        }


        [Test]
        public async Task GetRoomAsyncShouldReturnViewModelHavingCorrespondingDataWithValidId()
        {
            var category = new Category
            {
                Id = 1,
                Name = "Standard",
                Description = "Modern and stylish design for you",
                Beds = 2,
                Price = 100.00M,
                ImageUrl = "https://cdn.pixabay.com/photo/2015/11/06/11/45/interior-1026452_960_720.jpg"
            };

            List<Room> roomList = new List<Room>()
            {
                new Room()
                {
                    Id = Guid.Parse("50fc7855-3fc5-4c4c-a494-29eaa51e1035"),
                    Name = "101",
                    Category = category,
                    CategoryId = category.Id
                },
                new Room()
                {
                    Id = Guid.Parse("83388c3e-dd01-4268-b46a-e3151e464969"),
                    Name = "102",
                    Category = category,
                    CategoryId = category.Id
                }
            };

            Room searchedRoom = roomList.First();

            IQueryable<Room> roomQueryable = roomList
                .BuildMock();

            this.roomRepositoryMock
                .Setup(rr => rr.GetAllAttached())
                .Returns(roomQueryable);

            RoomDetailsViewModel? roomVm = await this.roomService
                .GetRoomDetailsByIdAsync(searchedRoom.Id.ToString());

            Assert.IsNotNull(roomVm);
            Assert.AreEqual(searchedRoom.Name, roomVm!.Name, "Room name should be copied to the ViewModel!");
        }

        // TODO FindRoomByDateArrivaleAndDateDepartureAsync
        // TODO FindRoomByDateArrivaleDateDepartureAndCategoryAsync

    }
}
