using BusinessObjects;
using Repositories;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Collections.Generic;

namespace Services
{
    public class RoomTypeService : IRoomTypeService
    {
        private IRoomTypeRepository roomTypeRepository;

        public RoomTypeService()
        {
            roomTypeRepository = new RoomTypeRepository();
        }

        public void AddRoomType(RoomType roomType) => roomTypeRepository.AddRoomType(roomType);

        public void DeleteRoomType(int id) => roomTypeRepository.DeleteRoomType(id);

        public RoomType GetRoomTypeByID(int id) => roomTypeRepository.GetRoomTypeByID(id);

        public List<RoomType> GetRoomTypes() => roomTypeRepository.GetRoomTypes();

        public void UpdateRoomType(RoomType roomType) => roomTypeRepository.UpdateRoomType(roomType);
    }
}