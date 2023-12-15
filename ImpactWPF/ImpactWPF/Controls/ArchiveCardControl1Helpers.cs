// <copyright file="ArchiveCardControl1Helpers.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ImpactWPF.Controls
{
    using System.Windows;
    using EfCore.entity;

    internal static class ArchiveCardControl1Helpers
    {
        private static readonly DependencyProperty ArchiveRequestPropertyValue =
        DependencyProperty.Register("ArchiveRequest", typeof(Request), typeof(ArchiveCardControl1));

        public static DependencyProperty ArchiveRequestProperty => ArchiveRequestPropertyValue;
    }
}