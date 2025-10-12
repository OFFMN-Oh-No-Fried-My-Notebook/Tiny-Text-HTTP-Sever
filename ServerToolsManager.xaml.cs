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

namespace Tiny_Text_HTTP_Sever
{
    /// <summary>
    /// ServerToolsManager.xaml 的交互逻辑
    /// </summary>
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
    }
}
