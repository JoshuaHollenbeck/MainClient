using MainClient.Services;
using MainClient.Utilities;
using MainClient._Model;
using System.Windows;
using System.Windows.Input;
using System;

namespace MainClient._ViewModel
{
    class AddNotesVM : ViewModelBase
    {
        private const int MaxCharacters = 255;
        public Action CloseNote { get; set; }
        private string _noteText;
        public string NoteText
        {
            get => _noteText;
            set
            {
                if (_noteText != value)
                {
                    _noteText = value;
                    OnPropertyChanged(nameof(NoteText));
                    OnPropertyChanged(nameof(CharactersRemaining));
                }
            }
        }
        
        public string CharactersRemaining
        {
            get { return $"{NoteText?.Length ?? 0} / {MaxCharacters}"; }
        }

        public ICommand AddNoteCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public AddNotesVM()
        {
            AddNoteCommand = new RelayCommand(ExecuteAddNote);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        public Action<string> CloseAndLoadAccountAction { get; set; }

        private void ExecuteAddNote(object parameter)
        {
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            string repId = RepIdService.Instance.RepId;

            try
            {
                if (NoteText != null)
                {
                    AddNotesModel.InsertAcctNotesByAcctNum(accountNumber, NoteText, repId);
                    NoteText = string.Empty;
                    
                    CloseAndLoadAccountAction?.Invoke(accountNumber);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add note: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteCancel(object parameter)
        {
            NoteText = string.Empty;
            CloseNote?.Invoke();
        }
    }
}