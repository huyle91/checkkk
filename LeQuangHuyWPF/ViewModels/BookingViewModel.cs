using BusinessObjects;
using LeQuangHuyWPF.Utils;
using Services.Interfaces;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LeQuangHuyWPF.ViewModels
{
    public class BookingViewModel : ViewModelBase
    {
        private readonly IBookingReservationService _bookingService;
        private readonly ICustomerService _customerService;
        private readonly IRoomInformationService _roomService;
        private readonly IRoomTypeService _roomTypeService;

        private BookingReservation _selectedBooking;
        public BookingReservation SelectedBooking
        {
            get => _selectedBooking;
            set
            {
                if (SetProperty(ref _selectedBooking, value))
                {
                    // When selection changes, copy values to edit properties
                    if (_selectedBooking != null)
                    {
                        BookingID = _selectedBooking.BookingID;
                        SelectedCustomerID = _selectedBooking.CustomerID;
                        SelectedRoomID = _selectedBooking.RoomID;
                        StartDate = _selectedBooking.StartDate;
                        EndDate = _selectedBooking.EndDate;
                        TotalPrice = _selectedBooking.TotalPrice;
                    }

                    // Update command states
                    (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (EditCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private bool _isAdmin;
        public bool IsAdmin
        {
            get => _isAdmin;
            set => SetProperty(ref _isAdmin, value);
        }


        private System.Collections.ObjectModel.ObservableCollection<BookingReservation> _bookings;
        public System.Collections.ObjectModel.ObservableCollection<BookingReservation> Bookings
        {
            get => _bookings;
            set => SetProperty(ref _bookings, value);
        }

        private System.Collections.ObjectModel.ObservableCollection<Customer> _customers;
        public System.Collections.ObjectModel.ObservableCollection<Customer> Customers
        {
            get => _customers;
            set => SetProperty(ref _customers, value);
        }

        private System.Collections.ObjectModel.ObservableCollection<RoomInformation> _rooms;
        public System.Collections.ObjectModel.ObservableCollection<RoomInformation> Rooms
        {
            get => _rooms;
            set => SetProperty(ref _rooms, value);
        }

        // Properties for adding/editing booking
        private int _bookingID;
        public int BookingID
        {
            get => _bookingID;
            set => SetProperty(ref _bookingID, value);
        }

        private int _selectedCustomerID;
        public int SelectedCustomerID
        {
            get => _selectedCustomerID;
            set => SetProperty(ref _selectedCustomerID, value);
        }

        private int _selectedRoomID;
        public int SelectedRoomID
        {
            get => _selectedRoomID;
            set
            {
                if (SetProperty(ref _selectedRoomID, value))
                {
                    UpdateTotalPrice();
                }
            }
        }

        private System.DateTime _startDate = System.DateTime.Now;
        public System.DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (SetProperty(ref _startDate, value))
                {
                    UpdateTotalPrice();
                }
            }
        }

        private System.DateTime _endDate = System.DateTime.Now.AddDays(1);
        public System.DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (SetProperty(ref _endDate, value))
                {
                    UpdateTotalPrice();
                }
            }
        }

        private decimal _totalPrice;
        public decimal TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }

        private System.DateTime _reportStartDate = System.DateTime.Now.AddMonths(-1);
        public System.DateTime ReportStartDate
        {
            get => _reportStartDate;
            set
            {
                if (SetProperty(ref _reportStartDate, value))
                {
                    (GenerateReportCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private System.DateTime _reportEndDate = System.DateTime.Now;
        public System.DateTime ReportEndDate
        {
            get => _reportEndDate;
            set
            {
                if (SetProperty(ref _reportEndDate, value))
                {
                    (GenerateReportCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        // Commands
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand GenerateReportCommand { get; }
        public ICommand RefreshCommand { get; }

        public BookingViewModel()
        {
            _bookingService = new BookingReservationService();
            _customerService = new CustomerService();
            _roomService = new RoomInformationService();
            _roomTypeService = new RoomTypeService();

            Bookings = new System.Collections.ObjectModel.ObservableCollection<BookingReservation>();
            Customers = new System.Collections.ObjectModel.ObservableCollection<Customer>();
            Rooms = new System.Collections.ObjectModel.ObservableCollection<RoomInformation>();

            AddCommand = new RelayCommand(ExecuteAdd);
            EditCommand = new RelayCommand(ExecuteEdit, CanExecuteEdit);
            DeleteCommand = new RelayCommand(ExecuteDelete, CanExecuteDelete);
            SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
            CancelCommand = new RelayCommand(ExecuteCancel);
            GenerateReportCommand = new RelayCommand(ExecuteGenerateReport, CanExecuteGenerateReport);
            RefreshCommand = new RelayCommand(ExecuteRefresh);

            LoadData();
        }

        private void LoadData()
        {
            // Load customers
            Customers.Clear();
            foreach (var customer in _customerService.GetCustomers())
            {
                Customers.Add(customer);
            }

            // Load rooms
            Rooms.Clear();
            foreach (var room in _roomService.GetRooms())
            {
                room.RoomType = _roomTypeService.GetRoomTypeByID(room.RoomTypeID);
                Rooms.Add(room);
            }

            // Load bookings
            LoadBookings();
        }

        private void LoadBookings()
        {
            Bookings.Clear();

            // If customer is not admin, only show their bookings
            if (!SessionManager.IsAdmin && SessionManager.CurrentUser != null)
            {
                foreach (var booking in _bookingService.GetBookingsByCustomerID(SessionManager.CurrentUser.CustomerID))
                {
                    // Set navigation properties
                    booking.Customer = _customerService.GetCustomerByID(booking.CustomerID);
                    booking.Room = _roomService.GetRoomByID(booking.RoomID);
                    if (booking.Room != null)
                    {
                        booking.Room.RoomType = _roomTypeService.GetRoomTypeByID(booking.Room.RoomTypeID);
                    }

                    Bookings.Add(booking);
                }
            }
            else
            {
                // Admin sees all bookings
                foreach (var booking in _bookingService.GetBookings())
                {
                    // Set navigation properties
                    booking.Customer = _customerService.GetCustomerByID(booking.CustomerID);
                    booking.Room = _roomService.GetRoomByID(booking.RoomID);
                    if (booking.Room != null)
                    {
                        booking.Room.RoomType = _roomTypeService.GetRoomTypeByID(booking.Room.RoomTypeID);
                    }

                    Bookings.Add(booking);
                }
            }
        }

        private void UpdateTotalPrice()
        {
            if (SelectedRoomID > 0 && EndDate > StartDate)
            {
                var room = _roomService.GetRoomByID(SelectedRoomID);
                if (room != null)
                {
                    int days = (int)(EndDate - StartDate).TotalDays;
                    TotalPrice = room.RoomPricePerDate * days;
                }
            }
        }

        private void ExecuteAdd(object obj)
        {
            // Clear form for new booking
            BookingID = 0;

            // If customer is logged in, use their ID
            if (!SessionManager.IsAdmin && SessionManager.CurrentUser != null)
            {
                SelectedCustomerID = SessionManager.CurrentUser.CustomerID;
            }
            else if (Customers.Count > 0)
            {
                SelectedCustomerID = Customers[0].CustomerID;
            }

            // Set default room if available
            if (Rooms.Count > 0)
            {
                SelectedRoomID = Rooms[0].RoomID;
            }

            StartDate = System.DateTime.Now;
            EndDate = System.DateTime.Now.AddDays(1);
            UpdateTotalPrice();

            // Show dialog to add new booking
            if (obj is System.Action<bool> showDialog)
            {
                showDialog(true);
            }
        }

        private bool CanExecuteEdit(object obj) => SelectedBooking != null;

        private void ExecuteEdit(object obj)
        {
            // Values already copied in SelectedBooking setter

            // Show dialog to edit booking
            if (obj is System.Action<bool> showDialog)
            {
                showDialog(true);
            }
        }

        private bool CanExecuteDelete(object obj) => SelectedBooking != null;

        private void ExecuteDelete(object obj)
        {
            if (SelectedBooking != null)
            {
                if (MessageBoxHelper.Confirm($"Are you sure you want to cancel this booking?"))
                {
                    try
                    {
                        _bookingService.DeleteBooking(SelectedBooking.BookingID);

                        // Update status in the UI
                        SelectedBooking.BookingStatus = 2; // Cancelled

                        // Refresh the list
                        LoadBookings();

                        MessageBoxHelper.ShowInfo("Booking cancelled successfully.");
                    }
                    catch (System.Exception ex)
                    {
                        MessageBoxHelper.ShowError($"Error cancelling booking: {ex.Message}");
                    }
                }
            }
        }

        private bool CanExecuteSave(object obj) =>
            SelectedCustomerID > 0 &&
            SelectedRoomID > 0 &&
            EndDate > StartDate;

        private void ExecuteSave(object obj)
        {
            try
            {
                // Check if room is available for the selected dates
                bool isAvailable = _bookingService.IsRoomAvailable(
                    SelectedRoomID,
                    StartDate,
                    EndDate,
                    BookingID == 0 ? null : (int?)BookingID
                );

                if (!isAvailable)
                {
                    MessageBoxHelper.ShowError("The selected room is not available for the chosen dates.");
                    return;
                }

                var booking = new BookingReservation
                {
                    BookingID = BookingID,
                    CustomerID = SelectedCustomerID,
                    RoomID = SelectedRoomID,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    TotalPrice = TotalPrice,
                    BookingStatus = 1, // Active
                    BookingDate = System.DateTime.Now
                };

                if (BookingID == 0) // New booking
                {
                    _bookingService.AddBooking(booking);

                    // Set navigation properties for UI
                    booking.Customer = _customerService.GetCustomerByID(booking.CustomerID);
                    booking.Room = _roomService.GetRoomByID(booking.RoomID);
                    if (booking.Room != null)
                    {
                        booking.Room.RoomType = _roomTypeService.GetRoomTypeByID(booking.Room.RoomTypeID);
                    }

                    Bookings.Add(booking);
                    MessageBoxHelper.ShowInfo("Booking added successfully.");
                }
                else // Update existing booking
                {
                    _bookingService.UpdateBooking(booking);

                    // Refresh the list to show updated data
                    LoadBookings();

                    MessageBoxHelper.ShowInfo("Booking updated successfully.");
                }

                // Close dialog
                if (obj is System.Action<bool> showDialog)
                {
                    showDialog(false);
                }
            }
            catch (System.Exception ex)
            {
                MessageBoxHelper.ShowError($"Error saving booking: {ex.Message}");
            }
        }

        private void ExecuteCancel(object obj)
        {
            // Close dialog
            if (obj is System.Action<bool> showDialog)
            {
                showDialog(false);
            }
        }

        private bool CanExecuteGenerateReport(object obj) => ReportEndDate >= ReportStartDate;

        private void ExecuteGenerateReport(object obj)
        {
            Bookings.Clear();

            foreach (var booking in _bookingService.GetBookingsByPeriod(ReportStartDate, ReportEndDate))
            {
                // Set navigation properties
                booking.Customer = _customerService.GetCustomerByID(booking.CustomerID);
                booking.Room = _roomService.GetRoomByID(booking.RoomID);
                if (booking.Room != null)
                {
                    booking.Room.RoomType = _roomTypeService.GetRoomTypeByID(booking.Room.RoomTypeID);
                }

                Bookings.Add(booking);
            }

            if (Bookings.Count == 0)
            {
                MessageBoxHelper.ShowInfo("No bookings found for the selected period.");
            }
        }

        private void ExecuteRefresh(object obj)
        {
            LoadBookings();
        }
    }
}