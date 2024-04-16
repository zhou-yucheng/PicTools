using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PicTools
{
    public class EditorControl : UserControl
    {
        public Boolean IsReadOnly
        {
            get { return (Boolean)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(Boolean), typeof(SizeInput), new PropertyMetadata(false));
    }
}
