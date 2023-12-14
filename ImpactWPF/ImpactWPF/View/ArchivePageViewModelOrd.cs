using EfCore.context;
using EfCore.entity;
using EfCore.service.impl;
using ImpactWPF.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace ImpactWPF.View
{
    public class ArchivePageViewModelOrd : INotifyPropertyChanged
    {
        private readonly RequestServiceImpl requestService = new RequestServiceImpl(new ImpactDbContext());
        private ObservableCollection<Request> _requests;
        private AtchivePageOrd _archivePage;

        public ArchivePageViewModelOrd(AtchivePageOrd archivePage)
        {
            _archivePage = archivePage;
        }

        private bool isActivatedFilter;
        public bool IsActivatedFilter
        {
            get { return isActivatedFilter; }
            set
            {
                if (isActivatedFilter != value)
                {
                    isActivatedFilter = value;
                    OnPropertyChanged(nameof(IsActivatedFilter));
                }
            }
        }

        private bool isDeactivatedFilter;
        public bool IsDeactivatedFilter
        {
            get { return isDeactivatedFilter; }
            set
            {
                if (isDeactivatedFilter != value)
                {
                    isDeactivatedFilter = value;
                    OnPropertyChanged(nameof(IsDeactivatedFilter));
                }
            }
        }

        private DateTime fromDateFilter;
        public DateTime FromDateFilter
        {
            get { return fromDateFilter; }
            set
            {
                if (fromDateFilter != value)
                {
                    fromDateFilter = value;
                    OnPropertyChanged(nameof(FromDateFilter));
                }
            }
        }

        private DateTime toDateFilter;
        public DateTime ToDateFilter
        {
            get { return toDateFilter; }
            set
            {
                if (toDateFilter != value)
                {
                    toDateFilter = value;
                    OnPropertyChanged(nameof(ToDateFilter));
                }
            }
        }

        public DateTime SelectedFromDate { get; set; }
        public DateTime SelectedToDate { get; set; }

        public void OnSelectedFromDateChanged(DateTime selectedDate)
        {
            SelectedFromDate = selectedDate;
            OnPropertyChanged(nameof(SelectedFromDate));
        }

        public void OnSelectedToDateChanged(DateTime selectedDate)
        {
            SelectedToDate = selectedDate;
            OnPropertyChanged(nameof(SelectedToDate));
        }

        public ICommand ApplyFilterCommand { get; private set; }

        public void FilterData()
        {
            LoadArchiveRequests();

            var filteredData = Requests.Where(item =>
                (IsActivatedFilter && item.RequestStatusId == 1) ||
                (IsDeactivatedFilter && item.RequestStatusId == 2) ||
                ((item.CreatedAt >= SelectedFromDate) && (item.CreatedAt <= SelectedToDate))
            );

            Requests = new ObservableCollection<Request>(filteredData.ToList());
            OnPropertyChanged(nameof(Requests));
        }

        private bool IsDateInRange(DateTime createdAt)
        {
            return (createdAt >= SelectedFromDate) && (createdAt <= SelectedToDate);
        }

        public void LoadArchiveRequests()
        {
            List<Request> archiveOrders = requestService.GetOrdersByEmail(UserSession.Instance.UserEmail);
            Requests = new ObservableCollection<Request>(archiveOrders);
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

        // Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
