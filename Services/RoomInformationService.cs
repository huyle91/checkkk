using BusinessObjects;
using Repositories;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Collections.Generic;

namespace Services
{
    public class RoomInformationService : IRoomInformationService
    {
        private IRoomInformationRepository roomRepository;

        public RoomInformationService()
        {
            roomRepository = new RoomInformationRepository();
        }

        public void AddRoom(RoomInformation room) => roomRepository.AddRoom(room);

        public void DeleteRoom(int id) => roomRepository.DeleteRoom(id);

        public RoomInformation GetRoomByID(int id) => roomRepository.GetRoomByID(id);

        public List<RoomInformation> GetRoomsByType(int roomTypeID) => roomRepository.GetRoomsByType(roomTypeID);

        public List<RoomInformation> GetRooms() => roomRepository.GetRooms();

        public void UpdateRoom(RoomInformation room) => roomRepository.UpdateRoom(room);

        public List<RoomInformation> SearchRooms(string searchValue) => roomRepository.SearchRooms(searchValue);
    }
}