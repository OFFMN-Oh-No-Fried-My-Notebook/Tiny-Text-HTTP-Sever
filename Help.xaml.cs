using System;
using System.Collections.Generic;
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
    /// Help.xaml 的交互逻辑
    /// </summary>
    public partial class Help : Window
    {
        private string key1 = "";
        private string pwd()
        {
            return "当前路径：" + Environment.CurrentDirectory;
        }

        public Help()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                key1 = (string)selectedItem.Content;
            }
            switch (key1)
            {

                default:
                    textbox1.Text = "请选择帮助主题";
                    break;
            }
        }
    }
}
