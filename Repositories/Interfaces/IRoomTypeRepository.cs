using BusinessObjects;
using System.Collections.Generic;

namespace Repositories.Interfaces
{
    public interface IRoomTypeRepository
    {
        List<RoomType> GetRoomTypes();
        RoomType GetRoomTypeByID(int id);
        void AddRoomType(RoomType roomType);
        void UpdateRoomType(RoomType roomType);
        void DeleteRoomType(int id);
    }
}