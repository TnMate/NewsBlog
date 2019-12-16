using System;
using System.Configuration;
using System.Windows;
using NewsBlog.Desktop.Model;
using NewsBlog.Desktop.View;
using NewsBlog.Desktop.ViewModel;

namespace NewsBlog.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private INewsBlogService _service;
        private NewsBlogViewModel _mainViewModel;
        private LoginViewModel _loginViewModel;
        private MainWindow _view;
        private LoginWindow _loginView;

        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _service = new NewsBlogService(ConfigurationManager.AppSettings["baseAddress"]);

            _loginViewModel = new LoginViewModel(_service);

            _loginViewModel.ExitApplication += ViewModel_ExitApplication;
            _loginViewModel.MessageApplication += ViewModel_MessageApplication;
            _loginViewModel.LoginSuccess += ViewModel_LoginSuccess;
            _loginViewModel.LoginFailed += ViewModel_LoginFailed;

            _loginView = new LoginWindow
            {
                DataContext = _loginViewModel
            };
            _loginView.Show();
        }

        public async void App_Exit(object sender, ExitEventArgs e)
        {
            if (_service.IsUserLoggedIn)
            {
                await _service.LogoutAsync();
            }
        }

        private void ViewModel_LoginSuccess(object sender, EventArgs e)
        {
            _mainViewModel = new NewsBlogViewModel(_service);
            _mainViewModel.MessageApplication += ViewModel_MessageApplication;

            _view = new MainWindow
            {
                DataContext = _mainViewModel
            };

            _view.Show();
            _loginView.Close();
        }

        private void ViewModel_ExitApplication(object sender, EventArgs e)
        {
            Shutdown();
        }

        private void ViewModel_LoginFailed(object sender, EventArgs e)
        {
            MessageBox.Show("A bejelentkezés sikertelen!", "Bank", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void ViewModel_MessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "Bank", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}
