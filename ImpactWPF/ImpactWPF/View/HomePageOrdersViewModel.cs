// <copyright file="HomePageOrdersViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ImpactWPF.View
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using EfCore.context;
    using EfCore.entity;
    using EfCore.service.impl;
    using ImpactWPF.Pages;

    public class HomePageOrdersViewModel : INotifyPropertyChanged
    {
        private readonly HomePageOrders homePage;
        private readonly RequestServiceImpl requestServiceр = new (new ImpactDbContext());
        private ObservableCollection<Request> requests = new ObservableCollection<Request>();
        private int currentPage = 1;  // Track the current page
        private int pageSize = 12;    // Number of requests per page
        private int currentRowCount = 3;  // Initial number of rows
        private int additionalRowCount = 3;  // Number of additional rows to
        private string searchTerm;

        public HomePageOrdersViewModel(HomePageOrders homePage)
        {
            this.homePage = homePage;
            this.SearchTerm = string.Empty;
            this.SelectedCategories = new List<string>();
        }

        public int PageSize
        {
            get
            {
                return this.pageSize;
            }

            set
            {
                this.pageSize = value;
                this.OnPropertyChanged(nameof(this.PageSize));
            }
        }

        public string SearchTerm
        {
            get
            {
                return this.searchTerm;
            }

            set
            {
                this.searchTerm = value;
                this.OnPropertyChanged(nameof(this.SearchTerm));
                this.PerformSearch(this.searchTerm);
            }
        }

        public void LoadInitialRequests()
        {
            List<Request> initialRequests = this.requestServiceр.GetActiveOrders(this.PageSize);
            this.Requests = new ObservableCollection<Request>(initialRequests);

            double newMarginTop = 352;

            if (initialRequests.Count < 4)
            {
                this.CurrentRowCount = 1;
            }
            else if (initialRequests.Count < 9)
            {
                this.CurrentRowCount = 2;
                newMarginTop = 754;
            }
            else
            {
                this.CurrentRowCount = 3;
                newMarginTop = 1106;
            }

            this.UpdateLoadMoreButtonMargin(newMarginTop);
        }

        private void PerformSearch(string searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                List<Request> searchResults = this.requestServiceр.GetOrdersByName(searchTerm, this.PageSize);
                this.Requests = new ObservableCollection<Request>(searchResults);
            }
            else
            {
                this.LoadInitialRequests();
            }
        }

        public ObservableCollection<Request> Requests
        {
            get
            {
                return this.requests;
            }

            set
            {
                this.requests = value;
                this.OnPropertyChanged(nameof(this.Requests));
            }
        }

        public int CurrentRowCount
        {
            get
            {
                return this.currentRowCount;
            }

            set
            {
                this.currentRowCount = value;
                this.OnPropertyChanged(nameof(this.CurrentRowCount));
            }
        }

        public int AdditionalRowCount
        {
            get
            {
                return this.additionalRowCount;
            }

            set
            {
                this.additionalRowCount = value;
                this.OnPropertyChanged(nameof(this.AdditionalRowCount));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void LoadMoreRequests()
        {
            this.currentPage++;

            List<Request> moreRequests = this.requestServiceр.GetMoreActiveOrders(this.currentPage, this.pageSize);

            if (moreRequests.Any())
            {
                foreach (var request in moreRequests)
                {
                    this.requests.Add(request);
                }

                if (moreRequests.Count < 5)
                {
                    this.CurrentRowCount += 1;
                }
                else if (moreRequests.Count < 9)
                {
                    this.CurrentRowCount += 2;
                }
                else
                {
                    this.CurrentRowCount += this.AdditionalRowCount;
                }

                double newMarginTop = this.CurrentRowCount * 360;

                this.UpdateLoadMoreButtonMargin(newMarginTop);
            }
        }

        private List<string> selectedCategories;

        public List<string> SelectedCategories
        {
            get
            {
                return this.selectedCategories;
            }

            set
            {
                this.selectedCategories = value;
                this.OnPropertyChanged(nameof(this.SelectedCategories));
                this.FilterRequestsByCategories();
            }
        }

        public void FilterRequestsByCategories()
        {
            if (this.SelectedCategories != null && this.SelectedCategories.Any())
            {
                List<Request> filteredRequests = this.requestServiceр.GetActiveOrdersByCategories(this.SelectedCategories, this.PageSize);
                this.Requests = new ObservableCollection<Request>(filteredRequests);
            }
            else
            {
                this.LoadInitialRequests();
            }
        }

        public void UpdateLoadMoreButtonMargin(double newMarginTop)
        {
            this.homePage.Button_LoadMore.Margin = new Thickness(0, newMarginTop, 0, 0);
        }
    }
}
