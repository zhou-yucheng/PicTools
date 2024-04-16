using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PicTools
{

    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private Dictionary<string, object> values = new Dictionary<string, object>();

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event PropertyChangedEventHandler PropertyChanged;

        private string GetPropertyName(LambdaExpression lambda)
        {
            MemberExpression operand;
            if (lambda.Body is UnaryExpression)
            {
                UnaryExpression body = lambda.Body as UnaryExpression;
                operand = body.Operand as MemberExpression;
            }
            else
            {
                operand = lambda.Body as MemberExpression;
            }
            ConstantExpression expression2 = operand.Expression as ConstantExpression;
            PropertyInfo member = operand.Member as PropertyInfo;
            return member.Name;
        }

        protected T GetValue<T>(Expression<Func<T>> property)
        {
            LambdaExpression lambda = property;
            if (lambda == null)
            {
                throw new ArgumentException("无效的视图模型属性定义.");
            }
            string propertyName = this.GetPropertyName(lambda);
            return this.GetValueInternal<T>(propertyName);
        }

        private T GetValueInternal<T>(string propertyName)
        {
            object obj2;
            if (this.values.TryGetValue(propertyName, out obj2))
            {
                return (T)obj2;
            }
            return default(T);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void SetValue<T>(Expression<Func<T>> property, T value)
        {
            LambdaExpression lambda = property;
            if (lambda == null)
            {
                throw new ArgumentException("无效的视图模型的属性定义.");
            }
            string propertyName = this.GetPropertyName(lambda);
            if (!object.Equals(this.GetValueInternal<T>(propertyName), value))
            {
                this.values[propertyName] = value;
                if (value is BitmapSource)
                {
                    BitmapSource res = this.values[propertyName] as BitmapSource;
                    res.Freeze();
                }
                this.OnPropertyChanged(propertyName);
            }
        }
    }

    public class RelayCommand : ICommand
    {
        public RelayCommand(Action<object> action)
        {
            ExecuteCommand = action;
        }
        //A method prototype without return value.
        public Action<object> ExecuteCommand = null;
        //A method prototype return a bool type.
        public Func<object, bool> CanExecuteCommand = null;
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (CanExecuteCommand != null)
            {
                return this.CanExecuteCommand(parameter);
            }
            else
            {
                return true;
            }
        }

        public void Execute(object parameter)
        {
            if (this.ExecuteCommand != null) this.ExecuteCommand(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }

}