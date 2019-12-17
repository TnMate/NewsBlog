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
        private readonly INewsBlogService _service;

        public ObservableCollection<ArticleDTO> Articles
        {
            get => _articles;
            set
            {
                _articles = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand SelectCommand { get; private set; }

        public NewsBlogModel(INewsBlogService service)
        {
            _service = service;
            LoadAsync();
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