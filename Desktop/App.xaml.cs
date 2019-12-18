using Microsoft.Win32;
using System;
using System.Windows;
using Desktop.Model;
using Desktop.View;
using Desktop.ViewModel;
using NewsBlog.Persistence.DTOs;
using System.Configuration;

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
            _mainViewModel.AddPictureStarted += new EventHandler<PictureEventArgs>(MainViewModel_AddPictureStarted);
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

        private void MainViewModel_AddPictureStarted(object sender, PictureEventArgs e)
        {
            try
            {
                // egy dialógusablakban bekérjük a fájlnevet
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.CheckFileExists = true;
                dialog.Filter = "Képfájlok|*.jpg;*.jpeg;*.bmp;*.tif;*.gif;*.png;";
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                Boolean? result = dialog.ShowDialog();

                if (result == true)
                {
                    // kép létrehozása (a megfelelő méretekkel)

                    var test = new PictureDTO
                    {
                        ArticleId = e.ArticleId,
                        Image = PictureHandler.OpenAndResize(dialog.FileName, 100)
                    };

                    //_service.AddImageAsync(test);
                    _mainViewModel.Pictures.Add(test);
                    _editorView.DataContext = _mainViewModel;
                    _editorView.Show();
                }
            }
            catch { }
        }

        private void MainViewModel_ArticleDeleteFinished(object sender, EventArgs e)
        {
            MessageBox.Show("Törlés elvégezve", "Article", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            _view.Focus();
            _view.Show();
        }

        private void MainViewModel_ArticleEditingStarted(object sender, EventArgs e)
        {
            _editorView = new ArticleEditorWindow(); // külön szerkesztő dialógus az épületekre
            _editorView.DataContext = _mainViewModel;
            _view.Hide();
            _editorView.Show();
        }

        private void MainViewModel_ArticleEditingFinished(object sender, EventArgs e)
        {
            _editorView.Close();
            _view.Show();
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