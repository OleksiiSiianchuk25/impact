using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using EfCore.entity;

namespace ImpactWPF.Controls
{
    public class ArchiveCardTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ActiveCardTemplate { get; set; }
        public DataTemplate InactiveCardTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Request request)
            {
                if (request.RequestStatusId == 1)
                {
                    return ActiveCardTemplate;
                }
                else if (request.RequestStatusId == 2)
                {
                    return InactiveCardTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
