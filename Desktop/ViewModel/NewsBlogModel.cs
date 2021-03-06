﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Desktop.Model;
using NewsBlog.Persistence;
using NewsBlog.Persistence.DTOs;

namespace Desktop.ViewModel
{
    public class NewsBlogModel : ViewModelBase
    {
        private ObservableCollection<ArticleDTO> _articles;
        private ObservableCollection<PictureDTO> _pictures;
        private ArticleDTO _selectedArticle;
        private readonly INewsBlogService _service;
        public ArticleDTO EditedArticle { get; private set; }

        public event EventHandler ExitApplication;

        public event EventHandler ArticleCreatingStarted;

        public event EventHandler ArticleCreatingFinished;

        public event EventHandler ArticleDeleteFinished;

        public event EventHandler ArticleEditingStarted;

        public event EventHandler ArticleEditingFinished;

        public event EventHandler<PictureEventArgs> AddPictureStarted;

        public DelegateCommand UpdateArticleCommand { get; private set; }

        public DelegateCommand CreateArticleCommand { get; private set; }

        public DelegateCommand DeleteArticleCommand { get; private set; }

        public DelegateCommand AddPictureCommand { get; private set; }

        public DelegateCommand SaveChangesCommand { get; private set; }

        public DelegateCommand CancelChangesCommand { get; private set; }

        public DelegateCommand ExitCommand { get; private set; }

        public ObservableCollection<ArticleDTO> Articles
        {
            get => _articles;
            set
            {
                _articles = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PictureDTO> Pictures
        {
            get => _pictures;
            set
            {
                _pictures = value;
                OnPropertyChanged();
            }
        }

        public ArticleDTO SelectedArticle
        {
            get { return _selectedArticle; }
            set
            {
                if (_selectedArticle != value)
                {
                    _selectedArticle = value;
                    OnPropertyChanged();
                }
            }
        }

        public DelegateCommand SelectCommand { get; private set; }

        public NewsBlogModel(INewsBlogService service)
        {
            _service = service;
            LoadAsync();

            ExitCommand = new DelegateCommand(param => OnExitApplication());
            AddPictureCommand = new DelegateCommand(param => OnAddPictureStarted((param as ArticleDTO).Id));
            CreateArticleCommand = new DelegateCommand(param => CreateArticle());
            UpdateArticleCommand = new DelegateCommand(param => UpdateArticle(param as ArticleDTO));
            DeleteArticleCommand = new DelegateCommand(param => DeleteArticle(param as ArticleDTO));
            SaveChangesCommand = new DelegateCommand(param => SaveChanges());
            CancelChangesCommand = new DelegateCommand(param => CancelChanges());
        }

        private void CreateArticle()
        {
            /*if (article == null)
                return;*/

            EditedArticle = new ArticleDTO
            {
                Id = 0,
                Title = "",
                Author = "",
                UserId = "",
                Date = DateTime.Now,
                Summary = "",
                Content = "",
                Leading = false
            };

            Pictures = new ObservableCollection<PictureDTO>();

            OnArticleCreatingStarted();
        }

        private async void UpdateArticle(ArticleDTO article)
        {
            if (article == null)
                return;

            EditedArticle = new ArticleDTO
            {
                Id = article.Id,
                Title = article.Title,
                Author = article.Author,
                UserId = article.UserId,
                Date = article.Date,
                Summary = article.Summary,
                Content = article.Content,
                Leading = article.Leading
            };

            await LoadPicturesAsync();

            OnArticleEditingStarted();
        }

        private async void DeleteArticle(ArticleDTO article)
        {
            if (article != null && MessageBox.Show("Törlés?", "Megerősítés", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                await _service.DeleteArticle(article.Id);
                EditedArticle = null;

                LoadAsync();
                OnArticleDeleteFinished();
            }
        }

        private void OnArticleCreatingStarted()
        {
            if (ArticleCreatingStarted != null)
                ArticleCreatingStarted(this, EventArgs.Empty);
        }

        private void OnArticleCreatingFinished()
        {
            if (ArticleCreatingFinished != null)
                ArticleCreatingFinished(this, EventArgs.Empty);
        }

        private void OnArticleDeleteFinished()
        {
            if (ArticleDeleteFinished != null)
                ArticleDeleteFinished(this, EventArgs.Empty);
        }

        private void OnArticleEditingStarted()
        {
            if (ArticleEditingStarted != null)
                ArticleEditingStarted(this, EventArgs.Empty);
        }

        private void OnArticleEditingFinished()
        {
            if (ArticleEditingFinished != null)
                ArticleEditingFinished(this, EventArgs.Empty);
        }

        private void OnAddPictureStarted(int articleId)
        {
            if (AddPictureStarted != null)
                AddPictureStarted(this, new PictureEventArgs { ArticleId = articleId });
        }

        private async void SaveChanges()
        {
            if (String.IsNullOrWhiteSpace(EditedArticle.Title))
            {
                OnMessageApplication("A cím nincs megadva!");
                return;
            }
            if (String.IsNullOrWhiteSpace(EditedArticle.Summary))
            {
                OnMessageApplication("Az összefoglaló nincs megadva!");
                return;
            }
            if (String.IsNullOrWhiteSpace(EditedArticle.Content))
            {
                OnMessageApplication("A tartalom nincs megadva!");
                return;
            }
            if (EditedArticle.Leading == true && Pictures.Count < 1)
            {
                OnMessageApplication("Vezető cikkhez 1 kép minimum");
                return;
            }

            if (EditedArticle.Summary.Length > 1000)
            {
                OnMessageApplication("Túl hosszú az összefoglaló");
                return;
            }

            // mentés
            if (EditedArticle.Id == 0) // új
            {
                await _service.CreateArticle(EditedArticle, Pictures);
                Articles.Add(EditedArticle);
                //await _service.AddImagesAsync(Pictures);
                SelectedArticle = EditedArticle;
            }
            else // Frissítés
            {
                await _service.UpdateArticle(EditedArticle);
                await _service.AddImagesAsync(Pictures);
            }

            EditedArticle = null;
            LoadAsync();
            OnArticleEditingFinished();
        }

        private void CancelChanges()
        {
            EditedArticle = null;
            OnArticleEditingFinished();
        }

        private void OnExitApplication()
        {
            if (ExitApplication != null)
                ExitApplication(this, EventArgs.Empty);
        }

        public async void LoadAsync()
        {
            try
            {
                Articles = new ObservableCollection<ArticleDTO>(await _service.LoadArticlesAsync());
                Pictures = new ObservableCollection<PictureDTO>();
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }

        public async Task<bool> LoadPicturesAsync()
        {
            try
            {
                var test = new ObservableCollection<PictureDTO>(await _service.LoadPicturesAsync(EditedArticle.Id));
                Pictures = test;
                return true;
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
                return false;
            }
        }
    }
}