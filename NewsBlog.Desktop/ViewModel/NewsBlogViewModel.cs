using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NewsBlog.Desktop.Model;
using NewsBlog.Persistence;

namespace NewsBlog.Desktop.ViewModel
{
    public class NewsBlogViewModel : ViewModelBase
    {
        private ObservableCollection<Article> _articles;
        private Article _article;
        private readonly INewsBlogService _service;

        public ObservableCollection<Article> Articles
        {
            get => _articles;
            set
            {
                _articles = value;
                OnPropertyChanged();
            }
        }

        public Article Article
        {
            get => _article;
            set
            {
                _article = value;
                OnPropertyChanged();
            }
        }

        public NewsBlogViewModel(INewsBlogService service)
        {
            _service = service;
            LoadAsync();

            //SelectCommand = new DelegateCommand(LoadArticle);
        }

        public DelegateCommand SelectCommand { get; private set; }

        public async void LoadAsync()
        {
            try
            {
                var test = new ObservableCollection<Article>(await _service.LoadArticlesAsync());
                Articles = test;
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }

        /*public async void LoadArticle(object param)
        {
            try
            {
                var test = await _service.LoadArticleAsync((param).Id);
                Article = new Article(await _service.LoadArticleAsync((param).Id));
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }*/
    }
}
