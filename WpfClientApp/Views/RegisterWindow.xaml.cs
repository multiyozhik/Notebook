using System.Windows;
using System.Windows.Controls;
using WpfClientApp.ViewModels;

namespace WpfClientApp.Views
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }      

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registerVM = ((Button)sender).DataContext as RegisterVM;
            registerVM.Password = PasswordTextBox.Password;
            registerVM.ConfirmPassword = ConfirmPasswordTextBox.Password;
            if (registerVM.Password != registerVM.ConfirmPassword)
            {
                MessageBox.Show("Ошибка при вводе пароля подтверждения");
            }
            else
            {
                DialogResult = true;
                Close();
            }
        }
    }
}
