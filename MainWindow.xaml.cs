using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Tiny_Text_HTTP_Sever
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }
        public void ALL_MousesLeave(object sender, MouseEventArgs e)
        {
            label1.Content = "Tiny Text HTTP Server v1.0.0";
        }
        public void ServerMgr_MouseEnter(object sender, MouseEventArgs e)
        {
            label1.Content = "提示：开始管理您的服务器";
        }
        public void About_MouseEnter(object sender, MouseEventArgs e)
        {
            label1.Content = "提示：关于 Tiny Text HTTP Server";
        }
        public void Help_MouseEnter(object sender, MouseEventArgs e)
        {
            label1.Content = "提示：打开帮助页面";
        }
        public void Setting_MouseEnter(object sender, MouseEventArgs e)
        {
            label1.Content = "提示：打开设置页面";
        }
        public void Quit_MouseEnter(object sender, MouseEventArgs e)
        {
            label1.Content = "提示：退出 Tiny Text HTTP Server";
        }
        public void OpenSR_MouseEnter(object sender, MouseEventArgs e)
        {
            label1.Content = "提示：打开关于此软件所遵守的开源协议";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            label1.Content = "Tiny Text HTTP Server v1.0.0";
            if (!Directory.Exists("blogs"))
            {
                Directory.CreateDirectory("blogs");
            }
            if (!Directory.Exists("blogs\\WWW"))
            {
                Directory.CreateDirectory("blogs\\WWW");
            }
            if (!File.Exists("index.html"))
            {
                using (StreamWriter sw = File.CreateText("blogs\\index.html"))
                {
                    sw.WriteLine("<html>");
                    sw.WriteLine("<head>");
                    sw.WriteLine("<title>Tiny Text HTTP Server</title>");
                    sw.WriteLine("</head>");
                    sw.WriteLine("<body>");
                    sw.WriteLine("<h1>Welcome to Tiny Text HTTP Server!</h1>");
                    sw.WriteLine("<p>This is a simple HTTP server that serves text files.</p>");
                    sw.WriteLine("</body>");
                    sw.WriteLine("</html>");
                }
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ServerToolsManager serverToolsWindow = new ServerToolsManager();

            serverToolsWindow.ShowDialog();
        }

        private void Button_about_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tiny Text HTTP Server v1.0.0\n\nDeveloped by OFFMN-Oh-No-Fried-My-Notebook.", "About Tiny Text HTTP Server", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_help_Click(object sender, RoutedEventArgs e)
        {
            Help help = new Help();
            help.ShowDialog();
        }

        private void Button_opensr_L_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_setting_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_quit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}