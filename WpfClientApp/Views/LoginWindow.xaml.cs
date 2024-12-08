using System.Windows;
using System.Windows.Controls;
using WpfClientApp.ViewModels;


namespace WpfClientApp.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();            
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var loginVM = ((Button)sender).DataContext as LoginVM;
            loginVM.Password = PasswordTextBox.Password;
            DialogResult = true;
            Close();
        }
    }
}

//<TextBox x:Name="PasswordTextBox" Grid.Row="1" Grid.Column="1" Margin="5" Padding="3"
//VerticalAlignment = "Center" Text = "{Binding Password, Mode=OneWayToSource}" />