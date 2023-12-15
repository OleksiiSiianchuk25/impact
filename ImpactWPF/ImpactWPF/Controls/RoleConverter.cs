// <copyright file="RoleConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ImpactWPF.Controls
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class RoleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string role)
            {
                switch (role)
                {
                    case "ROLE_VOLUNTEER":
                        return "Волонтер";
                    case "ROLE_ORDERER":
                        return "Замовник";
                    case "ROLE_ADMIN":
                        return "Адмін";
                    default:
                        return role;
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
