using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Desktop.Model;
using NewsBlog.Persistence;
using NewsBlog.Persistence.DTOs;

namespace Desktop.ViewModel
{
    public class NewsBlogModel : ViewModelBase
    {
        private ObservableCollection<ArticleDTO> _articles;
        private ArticleDTO _selectedArticle;
        private readonly INewsBlogService _service;
        public ArticleDTO EditedArticle { get; private set; }

        public event EventHandler ExitApplication;

        public event EventHandler ArticleCreatingStarted;

        public event EventHandler ArticleCreatingFinished;

        public event EventHandler ArticleEditingStarted;

        public event EventHandler ArticleEditingFinished;

        public DelegateCommand UpdateArticleCommand { get; private set; }

        public DelegateCommand CreateArticleCommand { get; private set; }

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
            CreateArticleCommand = new DelegateCommand(param => CreateArticle());
            UpdateArticleCommand = new DelegateCommand(param => UpdateArticle(param as ArticleDTO));
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

            OnArticleCreatingStarted();
        }

        private void UpdateArticle(ArticleDTO article)
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

            OnArticleEditingStarted();
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

        private async void SaveChanges()
        {
            if (String.IsNullOrEmpty(EditedArticle.Title))
            {
                OnMessageApplication("A cím nincs megadva!");
                return;
            }
            if (EditedArticle.Summary == null)
            {
                OnMessageApplication("Az összefoglaló nincs megadva!");
                return;
            }
            if (EditedArticle.Content == null)
            {
                OnMessageApplication("A tartalom nincs megadva!");
                return;
            }
            /*if (EditedArticle.Leading == true)
            {
                OnMessageApplication("KÉPFELTÖLTÉS");
                return;
            }*/

            // mentés
            if (EditedArticle.Id == 0) // új
            {
                await _service.CreateArticle(EditedArticle);
                Articles.Add(EditedArticle);
                SelectedArticle = EditedArticle;
            }
            else // Frissítés
            {
                await _service.UpdateArticle(EditedArticle);
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
                var test = new ObservableCollection<ArticleDTO>(await _service.LoadArticlesAsync());
                Articles = test;
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }
    }
}