using BusinessObjects;
using DataAccessObjects;
using Repositories.Interfaces;
using System.Collections.Generic;

namespace Repositories
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        public void AddRoomType(RoomType roomType) => RoomTypeDAO.Instance.AddRoomType(roomType);

        public void DeleteRoomType(int id) => RoomTypeDAO.Instance.DeleteRoomType(id);

        public RoomType GetRoomTypeByID(int id) => RoomTypeDAO.Instance.GetRoomTypeByID(id);

        public List<RoomType> GetRoomTypes() => RoomTypeDAO.Instance.GetRoomTypeList();

        public void UpdateRoomType(RoomType roomType) => RoomTypeDAO.Instance.UpdateRoomType(roomType);
    }
}