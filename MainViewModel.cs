using HandyControl.Controls;
using Newtonsoft.Json.Linq;
using PicTools.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PicTools
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            ImageFiles = new BindingList<ImageFile>();
            TextTemplate = "[编号]";
            StartIndex = 1;
            UpdateTitle();
            FontSize = 16;
            Width = 16;
            Height = 16;
            ShowFileName = false;

            Foreground = Colors.Red;
            Background = Colors.Transparent;

            var fonts = new List<FontFamilyEx>();
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                // FontFamily.Source contains the font family name.
                fonts.Add(new FontFamilyEx(fontFamily));
            }

            fonts = fonts.OrderBy(t => t.lang).ToList();
            DefaultFonts = fonts;
            SelectedFont = fonts.FirstOrDefault();
        }

        public string ImageFolder
        {
            get { return GetValue(() => ImageFolder); }
            set { SetValue(() => ImageFolder, value); }
        }

        public string TextTemplate
        {
            get { return GetValue(() => TextTemplate); }
            set
            {
                SetValue(() => TextTemplate, value);
                UpdateTitle();
            }
        }

        private void UpdateTitle()
        {
            if (ImageFiles != null && TextTemplate != null)
            {
                Task.Run(() =>
                {
                    foreach (ImageFile file in ImageFiles)
                    {
                        int index = ImageFiles.IndexOf(file);
                        long finalIndex = index + StartIndex;
                        file.Title = TextTemplate.Replace("[编号]", finalIndex.ToString());
                    }
                });
            }
        }

        public long StartIndex
        {
            get { return GetValue(() => StartIndex); }
            set { SetValue(() => StartIndex, value); UpdateTitle(); }
        }

        public List<FontFamilyEx> DefaultFonts
        {
            get { return GetValue(() => DefaultFonts); }
            set { SetValue(() => DefaultFonts, value); }
        }

        public FontFamilyEx SelectedFont
        {
            get { return GetValue(() => SelectedFont); }
            set { SetValue(() => SelectedFont, value); }
        }

        public int FontSize
        {
            get { return GetValue(() => FontSize); }
            set { SetValue(() => FontSize, value); }
        }

        public double FirstWidth
        {
            get { return GetValue(() => FirstWidth); }
            set { SetValue(() => FirstWidth, value); }
        }

        public double Width
        {
            get { return GetValue(() => Width); }
            set { SetValue(() => Width, value); }
        }

        public double Height
        {
            get { return GetValue(() => Height); }
            set { SetValue(() => Height, value); }
        }

        public double X
        {
            get { return GetValue(() => X); }
            set { SetValue(() => X, value); }
        }

        public double Y
        {
            get { return GetValue(() => Y); }
            set { SetValue(() => Y, value); }
        }

        public bool ShowFileName
        {
            get { return GetValue(() => ShowFileName); }
            set { SetValue(() => ShowFileName, value); }
        }

        public double Radius
        {
            get { return GetValue(() => Radius); }
            set { SetValue(() => Radius, value); }
        }

        public HorizontalAlignment HorizontalAlignment
        {
            get { return GetValue(() => HorizontalAlignment); }
            set { SetValue(() => HorizontalAlignment, value); }
        }

        public VerticalAlignment VerticalAlignment
        {
            get { return GetValue(() => VerticalAlignment); }
            set { SetValue(() => VerticalAlignment, value); }
        }

        public Color Foreground
        {
            get { return GetValue(() => Foreground); }
            set { SetValue(() => Foreground, value); }
        }

        public Color Background
        {
            get { return GetValue(() => Background); }
            set { SetValue(() => Background, value); }
        }


        public bool IsLoading
        {
            get { return GetValue(() => IsLoading); }
            set { SetValue(() => IsLoading, value); }
        }


        public BindingList<ImageFile> ImageFiles
        {
            get { return GetValue(() => ImageFiles); }
            set { SetValue(() => ImageFiles, value); }
        }

        public ICommand SelectFolderCommand
        {
            get
            {
                return new RelayCommand((sender) =>
                {
                    using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                    {
                        dialog.Description = "请选择包含待处理图片的文件夹";
                        System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                        if (result == System.Windows.Forms.DialogResult.OK)
                        {
                            ImageFolder = dialog.SelectedPath;

                            LoadFiles();
                        }

                    }
                });
            }
        }

        private void LoadFiles() {
            Task.Run(() =>
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(ImageFolder);
                if (directoryInfo.Exists)
                {
                    var files = new BindingList<ImageFile>();
                    foreach (var file in directoryInfo.GetFiles())
                    {
                        string ext = file.Extension.ToLower();

                        if (ext == ".jpg" || ext == ".png" || ext == ".jpeg")
                        {
                            files.Add(new ImageFile(file.FullName, file.Name));
                        }
                    }
                    if (files.Count > 0)
                    {
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            ImageFiles = files;
                            UpdateTitle();
                        });
                        UpdateFirstImageWidth();
                    }
                }
            });
        }

        private void UpdateFirstImageWidth() {

            var fFile = ImageFiles.FirstOrDefault();
            if (fFile == null) return;

            var bitmap = new BitmapImage(new Uri(fFile.FilePath, UriKind.Absolute));
            this.FirstWidth = bitmap.Width;
        }


        public ICommand SaveMergeCommand
        {
            get
            {
                return new RelayCommand((sender) =>
                {
                    if (this.ImageFiles.Count <= 0)
                    {
                        Growl.WarningGlobal("请先选择文件目录");
                        return;
                    }

                    IsLoading = true;
                    try
                    {
                        ItemsControl itemsControl = sender as ItemsControl;
                        if (sender == null) return;

                        string savePath = Path.Combine(ImageFolder, DateTime.Now.ToString("yyyyMMddHHmmss"));
                        if ((!Directory.Exists(savePath)))
                        {
                            Directory.CreateDirectory(savePath);
                        }

                        string fileName = Path.Combine(savePath, "拼图.png");
                        itemsControl.SaveAsImage(fileName);

                        Growl.SuccessGlobal("已生成到:" + fileName);
                        LoadFiles();
                    }
                    catch (Exception ex)
                    {
                        Growl.ErrorGlobal(ex.Message);
                    }
                    finally
                    {
                        IsLoading = false;
                    }

                });
            }
        }

        public ICommand SaveSingleCommand
        {
            get
            {
                return new RelayCommand((sender) =>
                {
                    if (this.ImageFiles.Count <= 0)
                    {
                        Growl.WarningGlobal("请先选择文件目录");
                        return;
                    }

                    IsLoading = true;

                    try
                    {
                        ItemsControl itemsControl = sender as ItemsControl;
                        if (sender == null) return;

                        string savePath = Path.Combine(ImageFolder, DateTime.Now.ToString("yyyyMMddHHmmss"));
                        if ((!Directory.Exists(savePath)))
                        {
                            Directory.CreateDirectory(savePath);
                        }

                        // 遍历当前DependencyObject的子元素
                        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(itemsControl); i++)
                        {
                            DependencyObject child = VisualTreeHelper.GetChild(itemsControl, i);

                            if (child is StackPanel)
                            {
                                StackPanel panel = (StackPanel)child;

                                foreach (FrameworkElement item in panel.Children)
                                {
                                    ImageFile data = (ImageFile)item.DataContext;
                                    string fileName = Path.Combine(savePath, data.FileName);
                                    item.SaveAsImage(fileName);
                                }

                            }
                        }

                        Growl.SuccessGlobal("已生成到:" + savePath);
                        LoadFiles();
                    }

                    catch (Exception ex)
                    {
                        Growl.ErrorGlobal(ex.Message);
                    }
                    finally
                    {
                        IsLoading = false;
                    }

                });
            }
        }

        public ICommand SwitchHAlign
        {
            get
            {
                return new RelayCommand((sender) =>
                {
                    if (sender == null) return;

                    var value = sender as string;

                    if (value == "Left") this.HorizontalAlignment = HorizontalAlignment.Left;
                    if (value == "Center") this.HorizontalAlignment = HorizontalAlignment.Center;
                    if (value == "Right") this.HorizontalAlignment = HorizontalAlignment.Right;
                });
            }
        }


        public ICommand SwitchVAlign
        {
            get
            {
                return new RelayCommand((sender) =>
                {
                    if (sender == null) return;

                    var value = sender as string;

                    if (value == "Top") this.VerticalAlignment = VerticalAlignment.Top;
                    if (value == "Center") this.VerticalAlignment = VerticalAlignment.Center;
                    if (value == "Bottom") this.VerticalAlignment = VerticalAlignment.Bottom;
                });
            }
        }

    }




    public class ImageFile : ViewModelBase
    {
        public ImageFile() { }


        public ImageFile(string filePath)
        {
            this.FilePath = filePath;
        }

        public ImageFile(string filePath, string fileName)
        {
            this.FilePath = filePath;
            this.FileName = fileName;
        }

        public string FilePath
        {
            get { return GetValue(() => FilePath); }
            set { SetValue(() => FilePath, value); }
        }

        public string FileName
        {
            get { return GetValue(() => FileName); }
            set { SetValue(() => FileName, value); }
        }

        public string Title
        {
            get { return GetValue(() => Title); }
            set { SetValue(() => Title, value); }
        }
    }
}
