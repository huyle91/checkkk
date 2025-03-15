using BusinessObjects;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IRoomInformationService
    {
        List<RoomInformation> GetRooms();
        RoomInformation GetRoomByID(int id);
        List<RoomInformation> GetRoomsByType(int roomTypeID);
        void AddRoom(RoomInformation room);
        void UpdateRoom(RoomInformation room);
        void DeleteRoom(int id);
        List<RoomInformation> SearchRooms(string searchValue);
    }
}