﻿using System;
using System.Configuration;
using System.Windows;
using Desktop.Model;
using Desktop.View;
using Desktop.ViewModel;

namespace Desktop
{
    public partial class App : Application
    {
        private INewsBlogService _service;
        private NewsBlogModel _mainViewModel;
        private LoginViewModel _loginViewModel;
        private MainWindow _view;
        private LoginWindow _loginView;
        private ArticleEditorWindow _editorView;

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

        private void ViewModel_ExitApplication(object sender, EventArgs e)
        {
            Shutdown();
        }

        private void ViewModel_LoginSuccess(object sender, EventArgs e)
        {
            _mainViewModel = new NewsBlogModel(_service);
            _mainViewModel.MessageApplication += ViewModel_MessageApplication;
            _mainViewModel.ArticleCreatingStarted += new EventHandler(MainViewModel_ArticleEditingStarted);
            _mainViewModel.ArticleCreatingFinished += new EventHandler(MainViewModel_ArticleEditingFinished);
            _mainViewModel.ArticleDeleteFinished += new EventHandler(MainViewModel_ArticleDeleteFinished);
            _mainViewModel.ArticleEditingStarted += new EventHandler(MainViewModel_ArticleEditingStarted);
            _mainViewModel.ArticleEditingFinished += new EventHandler(MainViewModel_ArticleEditingFinished);
            _mainViewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);

            _view = new MainWindow
            {
                DataContext = _mainViewModel
            };

            _view.Show();
            _loginView.Close();
        }

        private void MainViewModel_ArticleDeleteFinished(object sender, EventArgs e)
        {
            MessageBox.Show("Törlés elvégezve", "Bank", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void MainViewModel_ArticleEditingStarted(object sender, EventArgs e)
        {
            _editorView = new ArticleEditorWindow(); // külön szerkesztő dialógus az épületekre
            _editorView.DataContext = _mainViewModel;
            _editorView.Show();
        }

        private void MainViewModel_ArticleEditingFinished(object sender, EventArgs e)
        {
            _editorView.Close();
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