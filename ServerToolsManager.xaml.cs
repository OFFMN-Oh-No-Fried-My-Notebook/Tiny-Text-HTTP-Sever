using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
using System.Timers;

namespace Tiny_Text_HTTP_Sever
{
    /// <summary>
    /// ServerToolsManager.xaml 的交互逻辑
    /// </summary>
    /// 

    

    public partial class ServerToolsManager : Window
    {
        private List<IPAddress> GetAllIPAddresses()
        {
            var addresses = new List<IPAddress>();

            try
            {
                var hostEntry = Dns.GetHostEntry(Dns.GetHostName());
                addresses.AddRange(hostEntry.AddressList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取IP地址失败: {ex.Message}");
            }

            return addresses;
        }

        // 获取首选IP地址（优先公网IPv4）
        private string GetPreferredIP()
        {
            var addresses = GetAllIPAddresses();

            // 优先公网IPv4
            var publicIPv4 = addresses.FirstOrDefault(ip =>
                ip.AddressFamily == AddressFamily.InterNetwork &&
                !ip.ToString().StartsWith("192.168.") &&
                !ip.ToString().StartsWith("10.") &&
                !ip.ToString().StartsWith("172."));

            if (publicIPv4 != null)
                return publicIPv4.ToString();

            // 其次私有IPv4
            var privateIPv4 = addresses.FirstOrDefault(ip =>
                ip.AddressFamily == AddressFamily.InterNetwork);

            if (privateIPv4 != null)
                return privateIPv4.ToString();

            // 然后IPv6
            var ipv6 = addresses.FirstOrDefault(ip =>
                ip.AddressFamily == AddressFamily.InterNetworkV6);

            return ipv6?.ToString() ?? "127.0.0.1"; // 默认本地回环地址
        }
        public ServerToolsManager()
        {
            InitializeComponent();
            port.Text = "80";
            ip.Text = GetPreferredIP();
            if (File.Exists(Directory.GetCurrentDirectory() + "\\blogs\\index.html"))
            {
                dir.Text = Directory.GetCurrentDirectory() + "\\blogs\\index.html";
            }
            else
            {
                dir.Text = "Default file not found!";
            }
            if (!File.Exists(Directory.GetCurrentDirectory() + "\\TEMP.ini"))
            {
                try
                {
                    File.WriteAllLines(Directory.GetCurrentDirectory() + "\\TEMP.ini", new string[]
                    {
                        "[Serverinfo]",
                        "Port=80",
                        "DefaultFile=" + Directory.GetCurrentDirectory() + "\\blogs\\index.html",
                        "IP=" + GetPreferredIP()
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("创建配置文件失败: " + ex.Message);
                }
                /*
                using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "\\TEMP.ini"))
                {
                    sw.WriteLine("[Serverinfo]");
                    sw.WriteLine("Port=80");
                    sw.WriteLine("DefaultFile=" + Directory.GetCurrentDirectory() + "\\blogs\\index.html");
                    sw.WriteLine("IP=" + GetPreferredIP());
                */
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(File.Exists(Directory.GetCurrentDirectory() + "\\TEMP.ini"))
            {
                File.Delete(Directory.GetCurrentDirectory() + "\\TEMP.ini");
            }
            
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "选择默认打开的文件",
                Filter = "INI 文件 (*.ini)|*.ini|所有文件 (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true&&!string.IsNullOrEmpty(openFileDialog.FileName))
            {
                File.Copy(openFileDialog.FileName, Directory.GetCurrentDirectory() + "\\TEMP.ini");
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "\\TEMP.ini"))
            {
                File.Delete(Directory.GetCurrentDirectory() + "\\TEMP.ini");
            }
            try
            {
                File.WriteAllLines(Directory.GetCurrentDirectory() + "\\TEMP.ini", new string[]
                {
                        "[Serverinfo]",
                        "Port=80",
                        "DefaultFile=" + Directory.GetCurrentDirectory() + "\\blogs\\index.html",
                        "IP=" + GetPreferredIP()
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("创建配置文件失败: " + ex.Message);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "\\TEMP.ini"))
            {
                File.Delete(Directory.GetCurrentDirectory() + "\\TEMP.ini");
            }
            try
            {
                File.WriteAllLines(Directory.GetCurrentDirectory() + "\\TEMP.ini", new string[]
                {
                        "[Serverinfo]",
                        "Port=" + port.Text,
                        "DefaultFile=" + dir.Text,
                        "IP=" + ip.Text,
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("创建配置文件失败: " + ex.Message);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Server server= new Server();
            server.ShowDialog();
        }

        private string pagegroup_HeaderText = "配置向导";
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SAVE_TRUE.Visibility= Visibility.Hidden;
            button_done.Visibility= Visibility.Hidden;
            button_ip.Visibility= Visibility.Hidden;
            button_dir.Visibility= Visibility.Hidden;
            button_port.Visibility= Visibility.Hidden;
            l_dir.Visibility= Visibility.Hidden;
            dir.Visibility= Visibility.Hidden;
            l_ip.Visibility= Visibility.Hidden;
            ip.Visibility= Visibility.Hidden;
            l_port.Visibility= Visibility.Hidden;
            port.Visibility= Visibility.Hidden;
            main_text_label.Content= "欢迎使用配置向导！\n\n此向导将引导您完成服务器的基本配置。请按照以下步骤进行设置：\n\n1. 设置端口号：指定服务器监听的端口号，默认为80。\n\n2. 设置默认文件：选择服务器启动时默认打开的文件路径。\n\n3. 设置IP地址：指定服务器绑定的IP地址，默认为本机IP地址。\n\n完成以上步骤后，点击“完成”按钮保存配置并启动服务器。";
        }
        private void button_welcome_Click(object sender, RoutedEventArgs e)
        {
            welcomeliabel.Content="设置端口号以供服务器监听连接请求。";
            welcomeBGTEXT.Content= "设置端口";
            button_welcome.Visibility= Visibility.Hidden;
            All_bar.Value = 25;
            l_port.Visibility= Visibility.Visible;
            port.Visibility= Visibility.Visible;

        }

        private void button_port_Click(object sender, RoutedEventArgs e)
        {
            welcomeBGTEXT.Content = "设置默认文件";
        }

        private void button_dir_Click(object sender, RoutedEventArgs e)
        {
            welcomeBGTEXT.Content = "设置IP地址";
        }

        private void button_ip_Click(object sender, RoutedEventArgs e)
        {
           welcomeBGTEXT.Content = "完成";
            
        }
        private void button_done_Click(object sender, RoutedEventArgs e)
        {
            welcomeBGTEXT.Content = "SAVE";
        }
        private void SAVE_TRUE_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "\\TEMP.ini"))
            {
                File.Delete(Directory.GetCurrentDirectory() + "\\TEMP.ini");
            }
            try
            {
                File.WriteAllLines(Directory.GetCurrentDirectory() + "\\TEMP.ini", new string[]
                {
                        "[Serverinfo]",
                        "Port=" + port.Text,
                        "DefaultFile=" + dir.Text,
                        "IP=" + ip.Text,
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("创建配置文件失败: " + ex.Message);
            }
            Server server = new Server();
            server.ShowDialog();
        }
    }
}
