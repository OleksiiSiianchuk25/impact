// <copyright file="ArchiveCardData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ImpactWPF.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ArchiveCardData : INotifyPropertyChanged
    {
        private string requestName;

        public string RequestName
        {
            get
            {
                return this.requestName;
            }

            set
            {
                if (this.requestName != value)
                {
                    this.requestName = value;
                    this.OnPropertyChanged(nameof(this.RequestName));
                }
            }
        }

        private string description;

        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                if (this.description != value)
                {
                    this.description = value;
                    this.OnPropertyChanged(nameof(this.Description));
                }
            }
        }

        private string location;

        public string Location
        {
            get
            {
                return this.location;
            }

            set
            {
                if (this.location != value)
                {
                    this.location = value;
                    this.OnPropertyChanged(nameof(this.Location));
                }
            }
        }

        private string contactEmail;

        public string ContactEmail
        {
            get
            {
                return this.contactEmail;
            }

            set
            {
                if (this.contactEmail != value)
                {
                    this.contactEmail = value;
                    this.OnPropertyChanged(nameof(this.ContactEmail));
                }
            }
        }

        private string contactPhone;

        public string ContactPhone
        {
            get
            {
                return this.contactPhone;
            }

            set
            {
                if (this.contactPhone != value)
                {
                    this.contactPhone = value;
                    this.OnPropertyChanged(nameof(this.ContactPhone));
                }
            }
        }

        private DateTime createdAt;

        public DateTime CreatedAt
        {
            get
            {
                return this.createdAt;
            }

            set
            {
                if (this.createdAt != value)
                {
                    this.createdAt = value;
                    this.OnPropertyChanged(nameof(this.CreatedAt));
                }
            }
        }

        // Implement INotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
