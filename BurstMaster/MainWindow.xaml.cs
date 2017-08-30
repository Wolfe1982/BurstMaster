using System;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;
using System.Windows.Controls;
using EliteMMO.API;
using System.Windows.Media;
using System.ComponentModel;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace BurstMaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        public ListBox processids = new ListBox();

        public static EliteAPI api;

        public int firstSelect = 0;

        private static BackgroundWorker backgroundWorker;


        public MainWindow()
        {

            InitializeComponent();


            backgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;

            if (File.Exists("eliteapi.dll") && File.Exists("elitemmo.api.dll"))
            {
                var pol = Process.GetProcessesByName("pol");

                if (pol.Length < 1)
                {
                    MessageBox.Show("FFXI not found");
                }
                else
                {

                    for (var i = 0; i < pol.Length; i++)
                    {
                        this.POLID.Items.Add(pol[i].MainWindowTitle);
                        this.processids.Items.Add(pol[i].Id);
                    }
                    this.POLID.SelectedIndex = 0;
                    this.processids.SelectedIndex = 0;

                }
            }

            else
            {
                MessageBox.Show("This program can not function without EliteMMO.API.dll and EliteAPI.dll");
                Application.Current.Shutdown();
            }
        }

        private void SelectPOLIDButton_Click(object sender, RoutedEventArgs e)
        {
            this.processids.SelectedIndex = this.POLID.SelectedIndex;
            api = new EliteAPI((int)this.processids.SelectedItem);
            this.SelectPOLID.Content = "SELECTED";
            this.SelectPOLID.Background = Brushes.LightGreen;

            EliteAPI.ChatEntry cl = api.Chat.GetNextChatLine();
            while (cl != null) cl = api.Chat.GetNextChatLine();

            if (firstSelect == 0)
            {
                backgroundWorker.RunWorkerAsync();
                firstSelect = 1;
            }






        }



        public void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {


            EliteAPI.ChatEntry cl = api.Chat.GetNextChatLine();

            while (cl != null)
            {

               
















                cl = api.Chat.GetNextChatLine();
            }
            Thread.Sleep(TimeSpan.FromSeconds(0.1));
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Thread.Sleep(500);
            backgroundWorker.RunWorkerAsync();
        }











    }
}
