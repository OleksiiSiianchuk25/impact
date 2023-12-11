using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpactWPF.Controls
{
    public class ArchiveCardData : INotifyPropertyChanged
    {
        private string requestName;
        public string RequestName
        {
            get { return requestName; }
            set
            {
                if (requestName != value)
                {
                    requestName = value;
                    OnPropertyChanged(nameof(RequestName));
                }
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        private string location;
        public string Location
        {
            get { return location; }
            set
            {
                if (location != value)
                {
                    location = value;
                    OnPropertyChanged(nameof(Location));
                }
            }
        }

        private string contactEmail;
        public string ContactEmail
        {
            get { return contactEmail; }
            set
            {
                if (contactEmail != value)
                {
                    contactEmail = value;
                    OnPropertyChanged(nameof(ContactEmail));
                }
            }
        }

        private string contactPhone;
        public string ContactPhone
        {
            get { return contactPhone; }
            set
            {
                if (contactPhone != value)
                {
                    contactPhone = value;
                    OnPropertyChanged(nameof(ContactPhone));
                }
            }
        }

        private DateTime createdAt;
        public DateTime CreatedAt
        {
            get { return createdAt; }
            set
            {
                if (createdAt != value)
                {
                    createdAt = value;
                    OnPropertyChanged(nameof(CreatedAt));
                }
            }
        }

        // Implement INotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
