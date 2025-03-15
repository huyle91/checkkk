using BusinessObjects;
using Repositories;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Services
{
    public class BookingReservationService : IBookingReservationService
    {
        private IBookingReservationRepository bookingRepository;

        public BookingReservationService()
        {
            bookingRepository = new BookingReservationRepository();
        }

        public void AddBooking(BookingReservation booking) => bookingRepository.AddBooking(booking);

        public void DeleteBooking(int id) => bookingRepository.DeleteBooking(id);

        public BookingReservation GetBookingByID(int id) => bookingRepository.GetBookingByID(id);

        public List<BookingReservation> GetBookingsByCustomerID(int customerID) =>
            bookingRepository.GetBookingsByCustomerID(customerID);

        public List<BookingReservation> GetBookingsByPeriod(DateTime startDate, DateTime endDate) =>
            bookingRepository.GetBookingsByPeriod(startDate, endDate);

        public List<BookingReservation> GetBookings() => bookingRepository.GetBookings();

        public void UpdateBooking(BookingReservation booking) => bookingRepository.UpdateBooking(booking);

        public bool IsRoomAvailable(int roomID, DateTime startDate, DateTime endDate, int? excludeBookingID = null) =>
            bookingRepository.IsRoomAvailable(roomID, startDate, endDate, excludeBookingID);
    }
}