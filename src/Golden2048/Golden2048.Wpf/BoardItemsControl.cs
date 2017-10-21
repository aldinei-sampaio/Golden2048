using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Golden2048.Wpf
{
    public class BoardItemsControl : ItemsControl
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new Image();
        }
    }
}
