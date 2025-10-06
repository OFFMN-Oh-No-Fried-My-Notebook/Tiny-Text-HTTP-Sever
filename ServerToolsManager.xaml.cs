using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace Tiny_Text_HTTP_Sever
{
    /// <summary>
    /// ServerToolsManager.xaml 的交互逻辑
    /// </summary>
    public partial class ServerToolsManager : Window
    {
        public ServerToolsManager()
        {
            InitializeComponent();
            port.Text = "80";
            if(File.Exists(Directory.GetCurrentDirectory() + "\\blogs\\index.html"))
            {
                dir.Text = Directory.GetCurrentDirectory() + "\\blogs\\index.html";
            }
            else
            {
                dir.Text = "Default file not found, please set manually";
            }
            
        }
    }
}
