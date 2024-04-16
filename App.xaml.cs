using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace PicTools
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private static System.Threading.Mutex mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                mutex = new System.Threading.Mutex(true, "PicTools." + ConfigUtils.ModuleName);
                if (mutex.WaitOne(0, false))
                {
                    this.DispatcherUnhandledException += App_DispatcherUnhandledException;
                    AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                    this.Startup += Application_Startup;

                    base.OnStartup(e);
                }
                else
                {
                    HandyControl.Controls.MessageBox.Show("程序已经在运行！", "提示");
                    this.Shutdown();
                }

            }
            catch (Exception ex)
            {
            }

        }

        public void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 处理未捕获的异常（WPF窗体）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                e.Handled = true;
                Exception ex = e.Exception.InnerException != null ? e.Exception.InnerException : e.Exception;
                HandyControl.Controls.MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
                Application.Current.Shutdown();
                e.Handled = true;

            }
            catch (Exception ex)
            {
                HandyControl.Controls.MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
            }

        }

        /// <summary>
        /// 处理未捕获的异常（非WPF窗体）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception error = (Exception)e.ExceptionObject;
                Exception ex = error.InnerException != null ? error.InnerException : error;

                Dispatcher.BeginInvoke(
                   new Action(() => HandyControl.Controls.MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None)));
            }
            catch (Exception ex)
            {
                HandyControl.Controls.MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
            }

            Application.Current.Shutdown();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Process.GetCurrentProcess().Kill();
        }
    }
}
