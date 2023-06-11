using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ElectricityMetering.Core.Controllers;

namespace ElectricityMetering.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SignInController _signInController = new SignInController();

        private Dictionary<string, Window> _windowsByRoleName = new Dictionary<string, Window>
        {
            { Properties.Resources.President, new PresidentWindow()},
            { Properties.Resources.Electrician, new ElectricianWindow()},
        };

        private readonly SolidColorBrush _successColor = new SolidColorBrush(Color.FromRgb(74, 225, 127));
        private readonly SolidColorBrush _failureColor = new SolidColorBrush(Color.FromRgb(214, 64, 64));

        public MainWindow()
        {
            InitializeComponent();
        }

        public async void ButtonSignIn_Click(object sender, RoutedEventArgs e)
        {
            string roleName = RoleInput.Text;
            string password = PasswordInput.Password;

            if (!(await _signInController.PasswordIsCorrectAsync(roleName, password)))
            {
                PasswordInput.Clear();
                
                PasswordInput.BorderBrush = _failureColor;
                TextBlockPassword.Text = Properties.Resources.UncorrectPassord;
                TextBlockPassword.Foreground = _failureColor;

                return;
            }

            PasswordInput.BorderBrush = _successColor;
            TextBlockPassword.Text = Properties.Resources.CorrectPassword;
            TextBlockPassword.Foreground = _successColor;
            RoleInput.BorderBrush = _successColor;
            TextBlockRole.Foreground = _successColor;

            _windowsByRoleName[roleName].Show();
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
             DragMove();
        }

        private void ButtonHide_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
