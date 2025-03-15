using BusinessObjects;
using DataAccessObjects;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;

namespace Repositories
{
    public class BookingReservationRepository : IBookingReservationRepository
    {
        public void AddBooking(BookingReservation booking) => BookingReservationDAO.Instance.AddBooking(booking);

        public void DeleteBooking(int id) => BookingReservationDAO.Instance.DeleteBooking(id);

        public BookingReservation GetBookingByID(int id) => BookingReservationDAO.Instance.GetBookingByID(id);

        public List<BookingReservation> GetBookingsByCustomerID(int customerID) =>
            BookingReservationDAO.Instance.GetBookingsByCustomerID(customerID);

        public List<BookingReservation> GetBookingsByPeriod(DateTime startDate, DateTime endDate) =>
            BookingReservationDAO.Instance.GetBookingsByPeriod(startDate, endDate);

        public List<BookingReservation> GetBookings() => BookingReservationDAO.Instance.GetBookingList();

        public void UpdateBooking(BookingReservation booking) => BookingReservationDAO.Instance.UpdateBooking(booking);

        public bool IsRoomAvailable(int roomID, DateTime startDate, DateTime endDate, int? excludeBookingID = null) =>
            BookingReservationDAO.Instance.IsRoomAvailable(roomID, startDate, endDate, excludeBookingID);
    }
}