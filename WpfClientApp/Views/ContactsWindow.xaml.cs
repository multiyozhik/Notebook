using System.Windows;
using WpfClientApp.Services;
using WpfClientApp.ViewModels;

namespace WpfClientApp.Views
{
    /// <summary>
    /// Логика взаимодействия для Contacts.xaml
    /// </summary>
    public partial class ContactsWindow : Window
    {
        private readonly ContactsApi contactsApi;
        public ContactsWindow()
        {
            InitializeComponent();
            
            contactsApi = new ContactsApi(
                    new System.Uri(BaseRoute.baseAddress));

            //api-сервис контактов в конструктор DataContext окна
            DataContext = new ContactsVM(contactsApi); 

            this.Loaded += ContactsWindow_Loaded;
        }

        //при загрузке окна уже должен отображаться список контактов
        //даже для неавторизованных пользователей
        private async void ContactsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var window = (Window)sender;
            var contactsVM = (ContactsVM)window.DataContext;
            contactsVM.ContactsList = await contactsApi.GetContacts();
        }
    }
}
