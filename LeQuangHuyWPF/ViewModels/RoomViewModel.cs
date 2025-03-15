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
    public class RoomViewModel : ViewModelBase
    {
        private readonly IRoomInformationService _roomService;
        private readonly IRoomTypeService _roomTypeService;

        private RoomInformation _selectedRoom;
        public RoomInformation SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                if (SetProperty(ref _selectedRoom, value))
                {
                    // When selection changes, copy values to edit properties
                    if (_selectedRoom != null)
                    {
                        RoomID = _selectedRoom.RoomID;
                        RoomNumber = _selectedRoom.RoomNumber;
                        RoomDescription = _selectedRoom.RoomDescription;
                        RoomMaxCapacity = _selectedRoom.RoomMaxCapacity;
                        RoomPricePerDate = _selectedRoom.RoomPricePerDate;
                        SelectedRoomTypeID = _selectedRoom.RoomTypeID;
                    }

                    // Update command states
                    (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (EditCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private System.Collections.ObjectModel.ObservableCollection<RoomInformation> _rooms;
        public System.Collections.ObjectModel.ObservableCollection<RoomInformation> Rooms
        {
            get => _rooms;
            set => SetProperty(ref _rooms, value);
        }

        private System.Collections.ObjectModel.ObservableCollection<RoomType> _roomTypes;
        public System.Collections.ObjectModel.ObservableCollection<RoomType> RoomTypes
        {
            get => _roomTypes;
            set => SetProperty(ref _roomTypes, value);
        }

        // Properties for adding/editing room
        private int _roomID;
        public int RoomID
        {
            get => _roomID;
            set => SetProperty(ref _roomID, value);
        }

        private string _roomNumber;
        public string RoomNumber
        {
            get => _roomNumber;
            set => SetProperty(ref _roomNumber, value);
        }

        private string _roomDescription;
        public string RoomDescription
        {
            get => _roomDescription;
            set => SetProperty(ref _roomDescription, value);
        }

        private int _roomMaxCapacity;
        public int RoomMaxCapacity
        {
            get => _roomMaxCapacity;
            set => SetProperty(ref _roomMaxCapacity, value);
        }

        private decimal _roomPricePerDate;
        public decimal RoomPricePerDate
        {
            get => _roomPricePerDate;
            set => SetProperty(ref _roomPricePerDate, value);
        }

        private int _selectedRoomTypeID;
        public int SelectedRoomTypeID
        {
            get => _selectedRoomTypeID;
            set => SetProperty(ref _selectedRoomTypeID, value);
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    (SearchCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        // Commands
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand RefreshCommand { get; }

        public RoomViewModel()
        {
            _roomService = new RoomInformationService();
            _roomTypeService = new RoomTypeService();
            Rooms = new System.Collections.ObjectModel.ObservableCollection<RoomInformation>();
            RoomTypes = new System.Collections.ObjectModel.ObservableCollection<RoomType>();

            AddCommand = new RelayCommand(ExecuteAdd);
            EditCommand = new RelayCommand(ExecuteEdit, CanExecuteEdit);
            DeleteCommand = new RelayCommand(ExecuteDelete, CanExecuteDelete);
            SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
            CancelCommand = new RelayCommand(ExecuteCancel);
            SearchCommand = new RelayCommand(ExecuteSearch, CanExecuteSearch);
            RefreshCommand = new RelayCommand(ExecuteRefresh);

            LoadRooms();
            LoadRoomTypes();
        }

        private void LoadRooms()
        {
            Rooms.Clear();
            foreach (var room in _roomService.GetRooms())
            {
                // Get room type information
                room.RoomType = _roomTypeService.GetRoomTypeByID(room.RoomTypeID);
                Rooms.Add(room);
            }
        }

        private void LoadRoomTypes()
        {
            RoomTypes.Clear();
            foreach (var roomType in _roomTypeService.GetRoomTypes())
            {
                RoomTypes.Add(roomType);
            }
        }

        private void ExecuteAdd(object obj)
        {
            // Clear form for new room
            RoomID = 0;
            RoomNumber = string.Empty;
            RoomDescription = string.Empty;
            RoomMaxCapacity = 1;
            RoomPricePerDate = 0;
            SelectedRoomTypeID = RoomTypes.Count > 0 ? RoomTypes[0].RoomTypeID : 0;

            // Show dialog to add new room
            if (obj is System.Action<bool> showDialog)
            {
                showDialog(true);
            }
        }

        private bool CanExecuteEdit(object obj) => SelectedRoom != null;

        private void ExecuteEdit(object obj)
        {
            // Values already copied in SelectedRoom setter

            // Show dialog to edit room
            if (obj is System.Action<bool> showDialog)
            {
                showDialog(true);
            }
        }

        private bool CanExecuteDelete(object obj) => SelectedRoom != null;

        private void ExecuteDelete(object obj)
        {
            if (SelectedRoom != null)
            {
                if (MessageBoxHelper.Confirm($"Are you sure you want to delete room '{SelectedRoom.RoomNumber}'?"))
                {
                    try
                    {
                        _roomService.DeleteRoom(SelectedRoom.RoomID);
                        Rooms.Remove(SelectedRoom);
                        MessageBoxHelper.ShowInfo("Room deleted successfully.");
                    }
                    catch (System.Exception ex)
                    {
                        MessageBoxHelper.ShowError($"Error deleting room: {ex.Message}");
                    }
                }
            }
        }

        private bool CanExecuteSave(object obj) =>
            !string.IsNullOrWhiteSpace(RoomNumber) &&
            RoomMaxCapacity > 0 &&
            RoomPricePerDate > 0 &&
            SelectedRoomTypeID > 0;

        private void ExecuteSave(object obj)
        {
            try
            {
                var room = new RoomInformation
                {
                    RoomID = RoomID,
                    RoomNumber = RoomNumber,
                    RoomDescription = RoomDescription,
                    RoomMaxCapacity = RoomMaxCapacity,
                    RoomPricePerDate = RoomPricePerDate,
                    RoomTypeID = SelectedRoomTypeID,
                    RoomStatus = 1 // Active
                };

                if (RoomID == 0) // New room
                {
                    _roomService.AddRoom(room);
                    room.RoomType = _roomTypeService.GetRoomTypeByID(room.RoomTypeID);
                    Rooms.Add(room);
                    MessageBoxHelper.ShowInfo("Room added successfully.");
                }
                else // Update existing room
                {
                    _roomService.UpdateRoom(room);

                    // Find and update the room in the collection
                    var index = Rooms.IndexOf(SelectedRoom);
                    if (index >= 0)
                    {
                        room.RoomType = _roomTypeService.GetRoomTypeByID(room.RoomTypeID);
                        Rooms[index] = room;
                    }

                    MessageBoxHelper.ShowInfo("Room updated successfully.");
                }

                // Close dialog
                if (obj is System.Action<bool> showDialog)
                {
                    showDialog(false);
                }
            }
            catch (System.Exception ex)
            {
                MessageBoxHelper.ShowError($"Error saving room: {ex.Message}");
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

        private bool CanExecuteSearch(object obj) => !string.IsNullOrWhiteSpace(SearchText);

        private void ExecuteSearch(object obj)
        {
            Rooms.Clear();
            foreach (var room in _roomService.SearchRooms(SearchText))
            {
                room.RoomType = _roomTypeService.GetRoomTypeByID(room.RoomTypeID);
                Rooms.Add(room);
            }
        }

        private void ExecuteRefresh(object obj)
        {
            SearchText = string.Empty;
            LoadRooms();
        }
    }
}
