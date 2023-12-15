// <copyright file="ArchivePageViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ImpactWPF.View
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;
    using EfCore.context;
    using EfCore.entity;
    using EfCore.service.impl;
    using ImpactWPF.Pages;

    public class ArchivePageViewModel : INotifyPropertyChanged
    {
        private readonly RequestServiceImpl requestService = new RequestServiceImpl(new ImpactDbContext());
        private ObservableCollection<Request> requests;
        private readonly AtchivePage archivePage;

        public ArchivePageViewModel(AtchivePage archivePage)
        {
            this.archivePage = archivePage;
        }

        private bool isActivatedFilter;

        public bool IsActivatedFilter
        {
            get
            {
                return this.isActivatedFilter;
            }

            set
            {
                if (this.isActivatedFilter != value)
                {
                    this.isActivatedFilter = value;
                    this.OnPropertyChanged(nameof(this.IsActivatedFilter));
                }
            }
        }

        private bool isDeactivatedFilter;

        public bool IsDeactivatedFilter
        {
            get
            {
                return this.isDeactivatedFilter;
            }

            set
            {
                if (this.isDeactivatedFilter != value)
                {
                    this.isDeactivatedFilter = value;
                    this.OnPropertyChanged(nameof(this.IsDeactivatedFilter));
                }
            }
        }

        private DateTime fromDateFilter;

        public DateTime FromDateFilter
        {
            get
            {
                return this.fromDateFilter;
            }

            set
            {
                if (this.fromDateFilter != value)
                {
                    this.fromDateFilter = value;
                    this.OnPropertyChanged(nameof(this.FromDateFilter));
                }
            }
        }

        private DateTime toDateFilter;

        public DateTime ToDateFilter
        {
            get
            {
                return this.toDateFilter;
            }

            set
            {
                if (this.toDateFilter != value)
                {
                    this.toDateFilter = value;
                    this.OnPropertyChanged(nameof(this.ToDateFilter));
                }
            }
        }

        public DateTime SelectedFromDate { get; set; }

        public DateTime SelectedToDate { get; set; }

        public void OnSelectedFromDateChanged(DateTime selectedDate)
        {
            this.SelectedFromDate = selectedDate;
            this.OnPropertyChanged(nameof(this.SelectedFromDate));
        }

        public void OnSelectedToDateChanged(DateTime selectedDate)
        {
            this.SelectedToDate = selectedDate;
            this.OnPropertyChanged(nameof(this.SelectedToDate));
        }

        public ICommand ApplyFilterCommand { get; private set; }

        public void FilterData()
        {
            this.LoadArchiveRequests();

            var filteredData = this.Requests.Where(item =>
               (this.IsActivatedFilter && item.RequestStatusId == 1) ||
               (this.IsDeactivatedFilter && item.RequestStatusId == 2) ||
               ((item.CreatedAt >= this.SelectedFromDate) && (item.CreatedAt <= this.SelectedToDate)));

            this.Requests = new ObservableCollection<Request>(filteredData.ToList());
            this.OnPropertyChanged(nameof(this.Requests));
        }

        public void LoadArchiveRequests()
        {
            List<Request> archiveRequests = this.requestService.GetPropositionsByEmail(UserSession.Instance.UserEmail);
            this.Requests = new ObservableCollection<Request>(archiveRequests);
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

        // Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
