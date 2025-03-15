using BusinessObjects;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IRoomTypeService
    {
        List<RoomType> GetRoomTypes();
        RoomType GetRoomTypeByID(int id);
        void AddRoomType(RoomType roomType);
        void UpdateRoomType(RoomType roomType);
        void DeleteRoomType(int id);
    }
}