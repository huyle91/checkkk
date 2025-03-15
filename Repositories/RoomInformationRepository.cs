using BusinessObjects;
using DataAccessObjects;
using Repositories.Interfaces;
using System.Collections.Generic;

namespace Repositories
{
    public class RoomInformationRepository : IRoomInformationRepository
    {
        public void AddRoom(RoomInformation room) => RoomInformationDAO.Instance.AddRoom(room);

        public void DeleteRoom(int id) => RoomInformationDAO.Instance.DeleteRoom(id);

        public RoomInformation GetRoomByID(int id) => RoomInformationDAO.Instance.GetRoomByID(id);

        public List<RoomInformation> GetRoomsByType(int roomTypeID) =>
            RoomInformationDAO.Instance.GetRoomsByType(roomTypeID);

        public List<RoomInformation> GetRooms() => RoomInformationDAO.Instance.GetRoomList();

        public void UpdateRoom(RoomInformation room) => RoomInformationDAO.Instance.UpdateRoom(room);

        public List<RoomInformation> SearchRooms(string searchValue) =>
            RoomInformationDAO.Instance.SearchRooms(searchValue);
    }
}