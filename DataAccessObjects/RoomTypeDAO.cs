using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessObjects
{
    public class RoomTypeDAO
    {
        // Singleton pattern
        private static RoomTypeDAO instance = null;
        private static readonly object instanceLock = new object();
        private RoomTypeDAO() { }
        public static RoomTypeDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new RoomTypeDAO();
                    }
                    return instance;
                }
            }
        }

        // Create a static list to store room type data (in-memory database)
        private static List<RoomType> roomTypeList = new List<RoomType>
        {
            new RoomType
            {
                RoomTypeID = 1,
                RoomTypeName = "Standard",
                TypeDescription = "Standard room with basic amenities",
                TypenNote = "Affordable option for budget travelers"
            },
            new RoomType
            {
                RoomTypeID = 2,
                RoomTypeName = "Deluxe",
                TypeDescription = "Spacious room with premium amenities",
                TypenNote = "Perfect for comfort seekers"
            },
            new RoomType
            {
                RoomTypeID = 3,
                RoomTypeName = "Suite",
                TypeDescription = "Luxury suite with separate living area",
                TypenNote = "Ideal for extended stays and premium experience"
            }
        };

        // Implement CRUD operations for RoomType
        public List<RoomType> GetRoomTypeList() => roomTypeList;

        public RoomType GetRoomTypeByID(int roomTypeID)
        {
            return roomTypeList.SingleOrDefault(rt => rt.RoomTypeID == roomTypeID);
        }

        public void AddRoomType(RoomType roomType)
        {
            // Auto-generate ID if list is not empty
            if (roomTypeList.Count > 0)
            {
                roomType.RoomTypeID = roomTypeList.Max(rt => rt.RoomTypeID) + 1;
            }
            else
            {
                roomType.RoomTypeID = 1;
            }
            roomTypeList.Add(roomType);
        }

        public void UpdateRoomType(RoomType roomType)
        {
            RoomType existingRoomType = GetRoomTypeByID(roomType.RoomTypeID);
            if (existingRoomType != null)
            {
                existingRoomType.RoomTypeName = roomType.RoomTypeName;
                existingRoomType.TypeDescription = roomType.TypeDescription;
                existingRoomType.TypenNote = roomType.TypenNote;
            }
        }

        public void DeleteRoomType(int roomTypeID)
        {
            RoomType roomType = GetRoomTypeByID(roomTypeID);
            if (roomType != null)
            {
                roomTypeList.Remove(roomType);
            }
        }
    }
}