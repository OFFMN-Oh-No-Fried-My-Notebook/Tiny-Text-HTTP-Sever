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
using System.Net;
using System.Timers;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Xps.Serialization;

namespace Tiny_Text_HTTP_Sever
{
    /// <summary>
    /// ServerConfigEdit.xaml 的交互逻辑
    /// </summary>
    public partial class ServerConfigEdit : Window
    {
        private System.Timers.Timer refresh_timer = new System.Timers.Timer(50);
        private int step_counter = 0;
        private const string myAppName = "Tiny Text HTTP Server";
        

        private string ErrorModeToString(int mode)
        {
            return mode switch
            {
                0 => "TS 0001",//write error log
                1 => "TS 0002",//step counter error
                2 => "TS 0003",//read default ini file error
                _ => "Unknown Error Mode",
            };


        }

        public ServerConfigEdit()
        {
            InitializeComponent();
            refresh_timer.Elapsed += Refresh_Timer_Elapsed;
            port_input.Visibility = Visibility.Hidden;
            port_text.Visibility = Visibility.Hidden;
            refresh_timer.Start();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            switch (step_counter) {
                case 0:
                    step_counter++;
                    Next.Visibility=Visibility.Hidden;
                    port_input.Visibility=Visibility.Visible;
                    port_text.Visibility=Visibility.Visible;
                    break;
                default:
                break;
            }
        }

        private void Refresh_Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            if(this.Dispatcher==null)
            {
                refresh_timer.Stop();
                refresh_timer.Dispose();
                return;
            }

            this.Dispatcher.Invoke(async () => {
                switch (step_counter) {
                    case 1:
                        if(port_input.Text.Length>0)
                        {
                            Next.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            Next.Visibility = Visibility.Hidden;
                        }
                        break;
                    default:
                        await Task.Run(() =>
                        {
                            try
                            {
                                string erroecode =ErrorModeToString(1);
                                
                                File.AppendAllText(".\\log.txt", $"{erroecode}[{DateTime.Now:yyyy-MM-dd HH:mm:ss}][{myAppName} - ERROR] Unknown step_counter value: " + step_counter.ToString()+Environment.NewLine);
                            }
                            catch
                            {
                                MessageBox.Show("Fatal Error: Unable to write to log file. /n Debug info: step_counter=" + step_counter.ToString(), "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);

                            }
                        });
                        break;

                }
            });
        }

    }
}
