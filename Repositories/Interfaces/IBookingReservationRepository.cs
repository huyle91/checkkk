using BusinessObjects;
using System;
using System.Collections.Generic;

namespace Repositories.Interfaces
{
    public interface IBookingReservationRepository
    {
        List<BookingReservation> GetBookings();
        BookingReservation GetBookingByID(int id);
        List<BookingReservation> GetBookingsByCustomerID(int customerID);
        List<BookingReservation> GetBookingsByPeriod(DateTime startDate, DateTime endDate);
        void AddBooking(BookingReservation booking);
        void UpdateBooking(BookingReservation booking);
        void DeleteBooking(int id);
        bool IsRoomAvailable(int roomID, DateTime startDate, DateTime endDate, int? excludeBookingID = null);
    }
}
