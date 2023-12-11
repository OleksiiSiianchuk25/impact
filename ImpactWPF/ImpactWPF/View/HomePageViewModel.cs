using EfCore.context;
using EfCore.entity;
using EfCore.service.impl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpactWPF.View
{
    class HomePageViewModel : INotifyPropertyChanged
    {
        private readonly RequestServiceImpl requestService = new RequestServiceImpl(new ImpactDbContext());
        private ObservableCollection<Request> _requests;
        private int _currentPage = 1;  // Track the current page
        private int _pageSize = 12;    // Number of requests per page

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void LoadMoreRequests()
        {
            List<Request> moreRequests = requestService.GetMoreRequests(_currentPage, _pageSize);

            if (moreRequests.Any())
            {
                foreach (var request in moreRequests)
                {
                    _requests.Add(request);
                }
                _currentPage++;
            }
        }
    }
}
