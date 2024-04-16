using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PicTools
{
    /// <summary>
    /// ColorEditor.xaml 的交互逻辑
    /// </summary>
    public partial class ColorEditor : UserControl
    {
        public Color Value
        {
            get { 
                return (Color)GetValue(ValueProperty); 
            }
            set { 
                SetValue(ValueProperty, value); 
            }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(Color), typeof(ColorEditor), new PropertyMetadata(Colors.Transparent,OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorEditor editor = (ColorEditor)d;
            editor.BackgroundColorPicker.SelectedBrush = new SolidColorBrush((Color)e.NewValue);
        }

        public ColorEditor()
        {
            InitializeComponent();
            this.BackgroundColorPicker.MouseDown += BackgroundColorPicker_MouseDown;
        }

        private void BackgroundColorPicker_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void BackgroundColorPicker_Confirmed(object sender, HandyControl.Data.FunctionEventArgs<Color> e)
        {
            Popup picker = (Popup)this.BackgroundColorPickerPopup;
            if (picker != null)
            {
                picker.IsOpen = false;
            }

            Value = BackgroundColorPicker.SelectedBrush.Color;
        }

        private void BackgroundColorPicker_Canceled(object sender, EventArgs e)
        {
            Popup picker = this.BackgroundColorPickerPopup;
            if (picker != null)
            {
                picker.IsOpen = false;
            }

            if (PreviousColor != null)
            {
                Value = PreviousColor;
            }
        }
        private Color PreviousColor ;
        private void BackgroundColorPickerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.BackgroundColorPickerPopup.IsOpen = true;

            if (this.Value != null)
            {
                PreviousColor = Value;
            }
        }
    }
}
