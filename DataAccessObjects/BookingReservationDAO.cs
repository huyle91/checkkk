using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessObjects
{
    public class BookingReservationDAO
    {
        // Singleton pattern
        private static BookingReservationDAO instance = null;
        private static readonly object instanceLock = new object();
        private BookingReservationDAO() { }
        public static BookingReservationDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BookingReservationDAO();
                    }
                    return instance;
                }
            }
        }

        // Create a static list to store booking reservation data (in-memory database)
        private static List<BookingReservation> bookingList = new List<BookingReservation>
        {
            new BookingReservation
            {
                BookingID = 1,
                CustomerID = 2,
                RoomID = 1,
                StartDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now.AddDays(-3),
                TotalPrice = 200.00M,
                BookingStatus = 3, // Completed
                BookingDate = DateTime.Now.AddDays(-10)
            },
            new BookingReservation
            {
                BookingID = 2,
                CustomerID = 2,
                RoomID = 2,
                StartDate = DateTime.Now.AddDays(5),
                EndDate = DateTime.Now.AddDays(7),
                TotalPrice = 300.00M,
                BookingStatus = 1, // Active
                BookingDate = DateTime.Now.AddDays(-2)
            }
        };

        // Implement CRUD operations for BookingReservation
        public List<BookingReservation> GetBookingList() => bookingList;

        public BookingReservation GetBookingByID(int bookingID)
        {
            return bookingList.SingleOrDefault(b => b.BookingID == bookingID);
        }

        public List<BookingReservation> GetBookingsByCustomerID(int customerID)
        {
            return bookingList.Where(b => b.CustomerID == customerID).ToList();
        }

        public List<BookingReservation> GetBookingsByPeriod(DateTime startDate, DateTime endDate)
        {
            return bookingList.Where(b =>
                (b.StartDate >= startDate && b.StartDate <= endDate) ||
                (b.EndDate >= startDate && b.EndDate <= endDate)
            ).OrderByDescending(b => b.BookingDate).ToList();
        }

        public void AddBooking(BookingReservation booking)
        {
            // Auto-generate ID if list is not empty
            if (bookingList.Count > 0)
            {
                booking.BookingID = bookingList.Max(b => b.BookingID) + 1;
            }
            else
            {
                booking.BookingID = 1;
            }
            bookingList.Add(booking);
        }

        public void UpdateBooking(BookingReservation booking)
        {
            BookingReservation existingBooking = GetBookingByID(booking.BookingID);
            if (existingBooking != null)
            {
                existingBooking.CustomerID = booking.CustomerID;
                existingBooking.RoomID = booking.RoomID;
                existingBooking.StartDate = booking.StartDate;
                existingBooking.EndDate = booking.EndDate;
                existingBooking.TotalPrice = booking.TotalPrice;
                existingBooking.BookingStatus = booking.BookingStatus;
                existingBooking.BookingDate = booking.BookingDate;
            }
        }

        public void DeleteBooking(int bookingID)
        {
            BookingReservation booking = GetBookingByID(bookingID);
            if (booking != null)
            {
                // Soft delete by changing status to 2 (Cancelled)
                booking.BookingStatus = 2;
            }
        }

        public bool IsRoomAvailable(int roomID, DateTime startDate, DateTime endDate, int? excludeBookingID = null)
        {
            // Check if there are any overlapping bookings for the room
            return !bookingList.Any(b =>
                b.RoomID == roomID &&
                b.BookingStatus == 1 && // Active booking
                (excludeBookingID == null || b.BookingID != excludeBookingID) &&
                (
                    // New booking starts during existing booking
                    (startDate >= b.StartDate && startDate <= b.EndDate) ||
                    // New booking ends during existing booking
                    (endDate >= b.StartDate && endDate <= b.EndDate) ||
                    // New booking encompasses existing booking
                    (startDate <= b.StartDate && endDate >= b.EndDate)
                )
            );
        }
    }
}