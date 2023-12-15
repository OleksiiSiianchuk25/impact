// <copyright file="HomePageViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ImpactWPF.View
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using EfCore.context;
    using EfCore.entity;
    using EfCore.service.impl;
    using ImpactWPF.Pages;

    internal class HomePageViewModel : INotifyPropertyChanged
    {
        private readonly RequestServiceImpl requestService = new RequestServiceImpl(new ImpactDbContext());
        private ObservableCollection<Request> requests;
        private int currentPage = 1;
        private int pageSize = 12;
        private int currentRowCount = 3;
        private int additionalRowCount = 3;
        private readonly HomePage homePage;

        public HomePageViewModel(HomePage homePage)
        {
            this.homePage = homePage;
            this.SearchTerm = string.Empty;
            this.SelectedCategories = new List<string>();
        }

        private string searchTerm;

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
            List<Request> initialRequests = this.requestService.GetActivePropositions(this.PageSize);
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
                List<Request> searchResults = this.requestService.GetPropositionsByName(searchTerm, this.PageSize);
                this.Requests = new ObservableCollection<Request>(searchResults);
            }
            else
            {
                this.LoadInitialRequests();
            }
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void LoadMoreRequests()
        {
            this.currentPage++;

            List<Request> moreRequests = this.requestService.GetMoreActivePropositions(this.currentPage, this.pageSize);

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

        // Додайте нове поле для збереження обраних категорій
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

        // Оновлюємо метод для фільтрації за категоріями
        public void FilterRequestsByCategories()
        {
            if (this.SelectedCategories != null && this.SelectedCategories.Any())
            {
                List<Request> filteredRequests = this.requestService.GetActivePropositionsByCategories(this.SelectedCategories, this.PageSize);
                this.Requests = new ObservableCollection<Request>(filteredRequests);
            }
            else
            {
                // Обробляємо випадок, коли категорії не обрані (показуємо всі активні пропозиції)
                this.LoadInitialRequests();
            }
        }

        public void UpdateLoadMoreButtonMargin(double newMarginTop)
        {
            this.homePage.Button_LoadMore.Margin = new System.Windows.Thickness(0, newMarginTop, 0, 0);
        }
    }
}
