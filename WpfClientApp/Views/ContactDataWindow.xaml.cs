using System.Windows;


namespace WpfClientApp.Views
{
    /// <summary>
    /// Логика взаимодействия для ContactDataWindow.xaml
    /// </summary>
    public partial class ContactDataWindow : Window
    {
        public ContactDataWindow()
        {
            InitializeComponent();
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

    }
}
