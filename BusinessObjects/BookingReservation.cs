using System;

namespace BusinessObjects
{
    public class BookingReservation
    {
        public int BookingID { get; set; }
        public int CustomerID { get; set; }
        public int RoomID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int BookingStatus { get; set; } // 1: Active, 2: Cancelled, 3: Completed
        public DateTime BookingDate { get; set; }

        // Navigation properties (not stored in database, just for convenience)
        public Customer Customer { get; set; }
        public RoomInformation Room { get; set; }
    }
}