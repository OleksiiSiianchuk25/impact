using EfCore.context;
using EfCore.entity;
using EfCore.service.impl;
using ImpactWPF.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImpactWPF.View
{
    public class HomePageOrdersViewModel : INotifyPropertyChanged
    {
        private readonly RequestServiceImpl requestService = new RequestServiceImpl(new ImpactDbContext());
        private ObservableCollection<Request> _requests;
        private int _currentPage = 1;  // Track the current page
        private int _pageSize = 12;    // Number of requests per page
        private int _currentRowCount = 3;  // Initial number of rows
        private int _additionalRowCount = 3;  // Number of additional rows to load
        private HomePageOrders _homePage;

        public HomePageOrdersViewModel(HomePageOrders homePage)
        {
            _homePage = homePage;
            SearchTerm = string.Empty;
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
            List<Request> initialRequests = requestService.GetActiveOrders(PageSize);
            Requests = new ObservableCollection<Request>(initialRequests);
        }

        private void PerformSearch(string searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                List<Request> searchResults = requestService.GetOrdersByName(searchTerm, PageSize);
                Requests = new ObservableCollection<Request>(searchResults);
            }
            else
            {
                LoadInitialRequests();
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
            List<Request> moreRequests = requestService.GetMoreActiveOrders(_currentPage, _pageSize);

            if (moreRequests.Any())
            {
                foreach (var request in moreRequests)
                {
                    _requests.Add(request);
                }
                _currentPage++;

                if (moreRequests.Count < 4)
                {
                    CurrentRowCount += 1;
                }
                else if (moreRequests.Count < 7)
                {
                    CurrentRowCount += 2;
                }
                else
                {
                    CurrentRowCount += AdditionalRowCount;
                }

                double newMarginTop = 133 + (CurrentRowCount) * 360;

                UpdateLoadMoreButtonMargin(newMarginTop);
            }
        }

        public void UpdateLoadMoreButtonMargin(double newMarginTop)
        {
            _homePage.Button_LoadMore.Margin = new Thickness(0, newMarginTop, 0, 0);
        }
    }
}
