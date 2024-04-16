using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.IO;
using System.Windows.Threading;
using HandyControl.Tools.Extension;

namespace PicTools.Utils
{
    public static class UiEx
    {
        public static RenderTargetBitmap RenderTargetControl(this FrameworkElement VisualElement)
        {
            RenderTargetBitmap targetBitmap =
                  new RenderTargetBitmap((int)VisualElement.RenderSize.Width,
                                                                      (int)VisualElement.RenderSize.Height,
                                                                      96, 96,
                                                                      PixelFormats.Default);
         

            VisualElement.Measure(VisualElement.RenderSize); //Important
            VisualElement.Arrange(new Rect(VisualElement.RenderSize)); //Important

            targetBitmap.Render(VisualElement);
            return targetBitmap;
        }

        public static void SaveAsImage(this FrameworkElement VisualElement, string filePath)
        {
           RenderTargetBitmap targetBitmap = RenderTargetControl(VisualElement);
           SaveRenderTargetBitmapToFile(targetBitmap, filePath);

            VisualElement.Hide();
            VisualElement.Show();
        }

        public static void SaveRenderTargetBitmapToFile(RenderTargetBitmap renderTarget, string filePath)
        {
            // 创建BitmapEncoder对象，用于保存位图到文件
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTarget));

            // 打开文件流，用于保存文件
            using (System.IO.Stream fileStream = System.IO.File.Create(filePath))
            {
                // 将位图保存到文件流中
                encoder.Save(fileStream);
            }
        }
    }
}
