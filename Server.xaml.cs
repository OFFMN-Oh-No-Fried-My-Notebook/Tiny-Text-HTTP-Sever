using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Ini; 
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace Tiny_Text_HTTP_Sever
{
    /// <summary>
    /// Server.xaml 的交互逻辑
    /// </summary>
    public partial class Server : Window
    {
        public Server()
        {
            InitializeComponent();
            statusText.Text = string.Empty;
            Startbutton.IsEnabled = true;
            stopButton.IsEnabled = false;
            
        }
        //read TEMP.ini
        /*
         [Serverinfo]
Port=80
DefaultFile=C:\Users\f1437\source\repos\Tiny Text HTTP Sever\bin\Debug\net9.0-windows\blogs\index.html
IP=169.254.150.132

         */
        private static string ip()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddIniFile("TEMP.ini", optional: true, reloadOnChange: true) // 添加对 INI 文件的支持
                .Build();
            var serverInfo = config.GetSection("Serverinfo");
            if (!string.IsNullOrEmpty(serverInfo["IP"]))
            {
#pragma warning disable CS8603 // 可能返回 null 引用。
                return serverInfo["IP"];
#pragma warning restore CS8603 // 可能返回 null 引用。
            }
            else
            {
                return "127.0.0.1";
            }
        }
        private static int port()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddIniFile("TEMP.ini", optional: true, reloadOnChange: true) // 添加对 INI 文件的支持
                .Build();
            var serverInfo = config.GetSection("Serverinfo");
            var portValue = serverInfo["Port"];
            if (!string.IsNullOrEmpty(portValue))
            {
                return int.Parse(portValue);
            }
            else
            {
                return 80;
            }
        }
        private static string dir_indexhtml()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddIniFile("TEMP.ini", optional: true, reloadOnChange: true) // 添加对 INI 文件的支持
                .Build();
            var serverInfo = config.GetSection("Serverinfo");//read DefaultFile
            if (!string.IsNullOrEmpty(serverInfo["DefaultFile"]))
            {
#pragma warning disable CS8603 // 可能返回 null 引用。
                return serverInfo["DefaultFile"];
#pragma warning restore CS8603 // 可能返回 null 引用。
            }
            else
            {
                return Directory.GetCurrentDirectory() + "\\blogs\\index.html";
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        //use HTTP.sys
        public HttpListener lisener=new HttpListener();
        public CancellationTokenSource cts = new CancellationTokenSource();//what's that?
        public async void startserver()
        {
            
            
            await Task.Run(async () =>
            {

                
                lisener.Prefixes.Add("http://" + ip() + ":" + port() + "/");
                try { 
                    lisener.Start();
                    Application.Current.Dispatcher.Invoke(() => {
                        statusText.Text = "服务器运行中...";
                        Startbutton.IsEnabled = false;
                        stopButton.IsEnabled = true;
                    });
                    while (!cts.Token.IsCancellationRequested)
                    {
                        try {
                            var context = await lisener.GetContextAsync().WaitAsync(cts.Token);
                            ProcessRequest(context);
                        } catch (OperationCanceledException) { break;}catch (Exception ex) { Debug.WriteLine($"请求错误: {ex.Message}"); }
                    }
                }
                catch (Exception e) {
                    MessageBox.Show(e.Message); 
                    return;
                }
                finally
                {
                    SafeStopListener();
                }
            });
        }
        private void SafeStopListener()
        {
            try
            {
                lisener?.Stop();
                lisener?.Close();
            }
            catch (Exception) { }
            Application.Current.Dispatcher.Invoke(() =>
            {
                statusText.Text = "服务器已停止";
                Startbutton.IsEnabled = true;
                stopButton.IsEnabled = false;
            });
        }
        private void ProcessRequest(HttpListenerContext context)
        {
            try
            {
                
                string htmlFilePath = dir_indexhtml();

                if (File.Exists(htmlFilePath))
                {
                    // 从文件读取HTML内容
                    string htmlContent = File.ReadAllText(htmlFilePath, Encoding.UTF8);
                    ServeHtmlResponse(context.Response, htmlContent);
                }
                else
                {
                    // 文件不存在，返回默认页面
                    ServeDefaultHtml(context.Response);
                }
            }
            catch (Exception ex)
            {
                // 出错时返回错误页面
                ServeErrorResponse(context.Response, $"加载页面错误: {ex.Message}");
            }
            finally
            {
                context.Response.OutputStream.Close();
            }
        }

        private void ServeHtmlResponse(HttpListenerResponse response, string htmlContent)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(htmlContent);
            response.ContentType = "text/html; charset=utf-8";
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
        }

        private void ServeDefaultHtml(HttpListenerResponse response)
        {
            string defaultHtml = @"
<!DOCTYPE html>
<html>
<head><title>我的博客</title></head>
<body>
    <h1>欢迎！</h1>
    <p>配置文件中的HTML文件不存在，显示默认页面。</p>
    <p>时间: " + DateTime.Now.ToString() + @"</p>
</body>
</html>";

            ServeHtmlResponse(response, defaultHtml);
        }

        private void ServeErrorResponse(HttpListenerResponse response, string errorMessage)
        {
            string errorHtml = $@"
<!DOCTYPE html>
<html>
<head><title>错误</title></head>
<body>
    <h1>服务器错误</h1>
    <p>{errorMessage}</p>
</body>
</html>";

            response.StatusCode = 500;
            ServeHtmlResponse(response, errorHtml);
        }

        System.Timers.Timer timer=new System.Timers.Timer(1000);
        public void timer_tick(object sender, System.Timers.ElapsedEventArgs e) {
            webBrowser.Refresh();
            webBrowser.Navigate(new Uri("http://127.0.0.1:8080/"));
            webBrowser.Refresh();
            webBrowser.Navigate(new Uri("http://"+ip()+":"+port()+"/"));
        }

        private void Startbutton_Click(object sender, RoutedEventArgs e)
        {
            startserver();
            timer.Start();
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            SafeStopListener();
            timer.Stop();
            timer.Dispose();
            base.OnClosed(e);
        }
    }
}
