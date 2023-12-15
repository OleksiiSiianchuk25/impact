// <copyright file="ArchiveCardControl1HelpersBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ImpactWPF.Controls
{
    using System.Windows;
    using EfCore.entity;

    internal static class ArchiveCardControl1HelpersBase
    {
        private static readonly DependencyProperty ArchiveRequestPropertyValue =
        DependencyProperty.Register("ArchiveRequest", typeof(Request), typeof(ArchiveCardControl1));

        public static DependencyProperty ArchiveRequestProperty => ArchiveRequestPropertyValue;
    }
}