using EfCore.context;
using EfCore.entity;
using EfCore.service.impl;
using ImpactWPF.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ImpactWPF.View
{
    class HomePageViewModel : INotifyPropertyChanged
    {
        private readonly RequestServiceImpl requestService = new RequestServiceImpl(new ImpactDbContext());
        private ObservableCollection<Request> _requests;
        private int _currentPage = 1;
        private int _pageSize = 12;
        private int _currentRowCount = 3;
        private int _additionalRowCount = 3;
        private HomePage _homePage;

        public HomePageViewModel(HomePage homePage)
        {
            _homePage = homePage;
            SearchTerm = string.Empty;
            SelectedCategories = new List<string>();
        }

        private string _searchTerm;

        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                _searchTerm = value;
                OnPropertyChanged(nameof(SearchTerm));
                PerformSearch(_searchTerm);
            }
        }

        public void LoadInitialRequests()
        {
            List<Request> initialRequests = requestService.GetActivePropositions(PageSize);
            Requests = new ObservableCollection<Request>(initialRequests);

            double newMarginTop = 352;

            if (initialRequests.Count < 4)
            {
                CurrentRowCount = 1;
            }
            else if (initialRequests.Count < 9)
            {
                CurrentRowCount = 2;
                newMarginTop = 754;
            }
            else
            {
                CurrentRowCount = 3;
                newMarginTop = 1106;
            }

            UpdateLoadMoreButtonMargin(newMarginTop);
        }

        private void PerformSearch(string searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                List<Request> searchResults = requestService.GetPropositionsByName(searchTerm, PageSize);
                Requests = new ObservableCollection<Request>(searchResults);
            }
            else
            {
                LoadInitialRequests();
            }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value;
                OnPropertyChanged(nameof(PageSize));
            }
        }

        public ObservableCollection<Request> Requests
        {
            get { return _requests; }
            set
            {
                _requests = value;
                OnPropertyChanged(nameof(Requests));
            }
        }

        public int CurrentRowCount
        {
            get { return _currentRowCount; }
            set
            {
                _currentRowCount = value;
                OnPropertyChanged(nameof(CurrentRowCount));
            }
        }

        public int AdditionalRowCount
        {
            get { return _additionalRowCount; }
            set
            {
                _additionalRowCount = value;
                OnPropertyChanged(nameof(AdditionalRowCount));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void LoadMoreRequests()
        {
            _currentPage++;

            List<Request> moreRequests = requestService.GetMoreActivePropositions(_currentPage, _pageSize);

            if (moreRequests.Any())
            {
                foreach (var request in moreRequests)
                {
                    _requests.Add(request);
                }

                if (moreRequests.Count < 5)
                {
                    CurrentRowCount += 1;
                }
                else if (moreRequests.Count < 9)
                {
                    CurrentRowCount += 2;
                }
                else
                {
                    CurrentRowCount += AdditionalRowCount;
                }

                double newMarginTop = (CurrentRowCount) * 360;

                UpdateLoadMoreButtonMargin(newMarginTop);
            }
        }

        // Додайте нове поле для збереження обраних категорій
        private List<string> _selectedCategories;

        public List<string> SelectedCategories
        {
            get { return _selectedCategories; }
            set
            {
                _selectedCategories = value;
                OnPropertyChanged(nameof(SelectedCategories));

                // Оновлюємо фільтрацію при зміні обраних категорій
                FilterRequestsByCategories();
            }
        }

        // Оновлюємо метод для фільтрації за категоріями
        public void FilterRequestsByCategories()
        {
            if (SelectedCategories != null && SelectedCategories.Any())
            {
                List<Request> filteredRequests = requestService.GetActivePropositionsByCategories(SelectedCategories, PageSize);
                Requests = new ObservableCollection<Request>(filteredRequests);
            }
            else
            {
                // Обробляємо випадок, коли категорії не обрані (показуємо всі активні пропозиції)
                LoadInitialRequests();
            }
        }



        public void UpdateLoadMoreButtonMargin(double newMarginTop)
        {
            _homePage.Button_LoadMore.Margin = new System.Windows.Thickness(0, newMarginTop, 0, 0);
        }
    }
}
