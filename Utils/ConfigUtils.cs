using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PicTools
{
    public class ConfigUtils
    {
        private static string _rootpath;
        public static string RootPath
        {
            get
            {
                if (string.IsNullOrEmpty(_rootpath))
                {
                    var appdata = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                    _rootpath = Path.Combine(appdata, @"ozon");

                    if (!Directory.Exists(_rootpath))
                    {
                        Directory.CreateDirectory(_rootpath);
                    }
                }
                return _rootpath;
            }

        }

        /// <summary>
        /// 所在的模块名称
        /// </summary>
        public static string ModuleName
        {
            get
            {
                string modulename = "unknown";
                if (Assembly.GetEntryAssembly() != null && !string.IsNullOrEmpty(Assembly.GetEntryAssembly().FullName))
                {
                    modulename = Assembly.GetEntryAssembly().FullName;
                    modulename = modulename.Split(',')[0];
                }

                return modulename;
            }
        }

        public static string ConfigPath
        {
            get
            {
                string path = Path.GetFullPath(Path.Combine(RootPath, ModuleName + @"/Config"));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        public static string ModulePath
        {
            get
            {
                string path = Path.GetFullPath(Path.Combine(RootPath, ModuleName));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }


        public static string GetConfigPath(string filename)
        {
            return Path.Combine(ConfigPath, filename);
        }

        public static string GetModulePath(string filename,string folder = null)
        {
            if (string.IsNullOrEmpty(folder))
            {
                return Path.Combine(ModulePath, filename);
            }
            else {
                return Path.Combine(ModulePath, folder, filename);
            }
            
        }

        public static string GetPath(string filename)
        {
            string codeBase = Assembly.GetEntryAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));

            DirectoryInfo di = new DirectoryInfo(path);

            string rtn = Path.Combine(di.FullName, filename);
            return rtn;
        }

        public static void SaveConfig(string fileName, string value)
        {
            string file = GetConfigPath(fileName);

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            // 创建文件
            FileStream fs = new FileStream(file, FileMode.Create, FileAccess.ReadWrite); //可以指定盘符，也可以指定任意文件名，还可以为word等文件
            StreamWriter sw = new StreamWriter(fs); // 创建写入流
            sw.WriteLine(value);

            sw.Close(); //关闭文件
        }


        public static T ReadConfig<T>(string fileName)
        {
            string val = string.Empty;
            string file = GetConfigPath(fileName);

            if (File.Exists(file))
            {
                val = File.ReadAllText(file);
                return JsonUtils.JsonDeserialize<T>(val);
            }
            else {
                return default(T);
            }
            
        }

        public static void Save<T>(string fileName, T value)
        {
            var file = GetConfigPath(fileName);
            try
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }

                string text = JsonUtils.JsonSerializer(value);
               
                // 创建文件
                //可以指定盘符，也可以指定任意文件名，还可以为word等文件
                FileStream fs = new FileStream(file, FileMode.Create, FileAccess.ReadWrite);
                // 创建写入流
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(text);
                //关闭文件
                sw.Close();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
