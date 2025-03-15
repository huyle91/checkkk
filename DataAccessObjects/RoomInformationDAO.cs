using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessObjects
{
    public class RoomInformationDAO
    {
        // Singleton pattern
        private static RoomInformationDAO instance = null;
        private static readonly object instanceLock = new object();
        private RoomInformationDAO() { }
        public static RoomInformationDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new RoomInformationDAO();
                    }
                    return instance;
                }
            }
        }

        // Create a static list to store room information data (in-memory database)
        private static List<RoomInformation> roomList = new List<RoomInformation>
        {
            new RoomInformation
            {
                RoomID = 1,
                RoomNumber = "101",
                RoomDescription = "Cozy standard room with city view",
                RoomMaxCapacity = 2,
                RoomStatus = 1, // Active
                RoomPricePerDate = 100.00M,
                RoomTypeID = 1
            },
            new RoomInformation
            {
                RoomID = 2,
                RoomNumber = "201",
                RoomDescription = "Spacious deluxe room with ocean view",
                RoomMaxCapacity = 3,
                RoomStatus = 1, // Active
                RoomPricePerDate = 150.00M,
                RoomTypeID = 2
            },
            new RoomInformation
            {
                RoomID = 3,
                RoomNumber = "301",
                RoomDescription = "Luxury suite with balcony",
                RoomMaxCapacity = 4,
                RoomStatus = 1, // Active
                RoomPricePerDate = 250.00M,
                RoomTypeID = 3
            }
        };

        // Implement CRUD operations for RoomInformation
        public List<RoomInformation> GetRoomList() => roomList.Where(r => r.RoomStatus == 1).ToList();

        public RoomInformation GetRoomByID(int roomID)
        {
            return roomList.SingleOrDefault(r => r.RoomID == roomID);
        }

        public List<RoomInformation> GetRoomsByType(int roomTypeID)
        {
            return roomList.Where(r => r.RoomTypeID == roomTypeID && r.RoomStatus == 1).ToList();
        }

        public void AddRoom(RoomInformation room)
        {
            // Auto-generate ID if list is not empty
            if (roomList.Count > 0)
            {
                room.RoomID = roomList.Max(r => r.RoomID) + 1;
            }
            else
            {
                room.RoomID = 1;
            }
            roomList.Add(room);
        }

        public void UpdateRoom(RoomInformation room)
        {
            RoomInformation existingRoom = GetRoomByID(room.RoomID);
            if (existingRoom != null)
            {
                existingRoom.RoomNumber = room.RoomNumber;
                existingRoom.RoomDescription = room.RoomDescription;
                existingRoom.RoomMaxCapacity = room.RoomMaxCapacity;
                existingRoom.RoomStatus = room.RoomStatus;
                existingRoom.RoomPricePerDate = room.RoomPricePerDate;
                existingRoom.RoomTypeID = room.RoomTypeID;
            }
        }

        public void DeleteRoom(int roomID)
        {
            RoomInformation room = GetRoomByID(roomID);
            if (room != null)
            {
                // Soft delete by changing status to 2 (Deleted)
                room.RoomStatus = 2;
            }
        }

        public List<RoomInformation> SearchRooms(string searchValue)
        {
            return roomList.Where(r =>
                (r.RoomNumber != null && r.RoomNumber.Contains(searchValue, StringComparison.OrdinalIgnoreCase)) ||
                (r.RoomDescription != null && r.RoomDescription.Contains(searchValue, StringComparison.OrdinalIgnoreCase)) ||
                r.RoomMaxCapacity.ToString().Contains(searchValue) ||
                r.RoomPricePerDate.ToString().Contains(searchValue)
            ).ToList();
        }
    }
}