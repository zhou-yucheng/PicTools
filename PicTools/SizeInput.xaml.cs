using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// SizeInput.xaml 的交互逻辑
    /// </summary>
    public partial class SizeInput : EditorControl
    {
        
        public Size Value
        {
            get { return (Size)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(Size), typeof(SizeInput), new PropertyMetadata(new Size(), OnSizeChangedCallback));


        public Double WidthValue
        {
            get { return (Double)GetValue(WidthValueProperty); }
            set { SetValue(WidthValueProperty, value); }
        }
        public static readonly DependencyProperty WidthValueProperty =
            DependencyProperty.Register("WidthValue", typeof(Double), typeof(SizeInput), new PropertyMetadata(0.0, OnValueChangedCallback));

        public double HeightValue
        {
            get { return (double)GetValue(HeightValueProperty); }
            set { SetValue(HeightValueProperty, value); }
        }
        public static readonly DependencyProperty HeightValueProperty =
            DependencyProperty.Register("HeightValue", typeof(double), typeof(SizeInput), new PropertyMetadata(0.0, OnValueChangedCallback));


        public Double XValue
        {
            get { return (Double)GetValue(XValueProperty); }
            set { SetValue(XValueProperty, value); }
        }
        public static readonly DependencyProperty XValueProperty =
            DependencyProperty.Register("XValue", typeof(Double), typeof(SizeInput), new PropertyMetadata(0.0, OnValueChangedCallback));

        public double YValue
        {
            get { return (double)GetValue(YValueProperty); }
            set { SetValue(YValueProperty, value); }
        }
        public static readonly DependencyProperty YValueProperty =
            DependencyProperty.Register("YValue", typeof(double), typeof(SizeInput), new PropertyMetadata(0.0, OnValueChangedCallback));


        public double RadiusValue
        {
            get { return (double)GetValue(RadiusValueProperty); }
            set { SetValue(RadiusValueProperty, value); }
        }
        public static readonly DependencyProperty RadiusValueProperty =
            DependencyProperty.Register("RadiusValue", typeof(double), typeof(SizeInput), new PropertyMetadata(0.0, OnValueChangedCallback));

        private static void OnSizeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SizeInput editor = (SizeInput)d;
            Size newVal = (Size)e.NewValue;
            if (editor.WidthValue != newVal.Width)
            {
                editor.WidthValue = newVal.Width;
            }
            if (editor.HeightValue != newVal.Height)
            {
                editor.HeightValue = newVal.Height;
            }
        }
        private static void OnValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SizeInput editor = (SizeInput)d;
            if (editor.Value.Width != editor.WidthValue || editor.Value.Height != editor.HeightValue)
            {
                editor.Value = new Size(editor.WidthValue, editor.HeightValue);
            }
            
        }
        public SizeInput()
        {
            InitializeComponent();
            this.WidthPart.DataContext = this;
            this.HeightPart.DataContext = this;
            this.XPart.DataContext = this;
            this.YPart.DataContext = this;
            this.RadiusPart.DataContext = this;
        }
    }
}
