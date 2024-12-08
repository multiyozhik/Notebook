using System.Collections.Generic;
using System.ComponentModel;
using WpfClientApp.Services;

namespace WpfClientApp.ViewModels
{
    class AppUsersVM : INotifyPropertyChanged //класс-DataContext для главного окна ContactsWindow
    {
        private readonly AuthUsersApi authUsersApi;

        private List<AppUser>? appUsersList;
        public List<AppUser>? AppUsersList //отслеживаем изменения коллекции контактов
        {
            get => appUsersList;
            set
            {
                appUsersList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AppUsersList)));//вызываем PropertyChanged
            }
        }

        private AppUser? selectedUser;
        public AppUser? SelectedUser //отслеживаем изменение выделенного контакта
        {
            get => selectedUser;
            set
            {
                selectedUser = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedUser)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;//реализуем INotifyPropertyChanged

        public AppUsersVM(AuthUsersApi authUsersApi) // конструктор
        {
            this.authUsersApi = authUsersApi;
        }

        private AsyncRelayCommand deleteUserCommand;
        public AsyncRelayCommand DeleteUserCommand
        {
            get => deleteUserCommand ??= new AsyncRelayCommand(async obj =>
            {
                if (SelectedUser is null) return;

                await authUsersApi.DeleteUser(SelectedUser.Id);
                AppUsersList = await authUsersApi.GetUsers();
            });
        }
    }
}
