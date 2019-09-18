using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace RandomListSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> nameList;
        private List<string> winnerList;
        private RandomManager rndManager;
        private StatManager statsManager;

        private decimal simAmount = 0.0m;

        public MainWindow()
        {
            InitializeComponent();
            nameList = new List<string>();
            winnerList = new List<string>();
            rndManager = new RandomManager();
            statsManager = new StatManager(rndManager);

            SimProgressBar.Visibility = Visibility.Hidden;

        }

        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";

            if(openFileDialog.ShowDialog() == true)
            {
                var data = File.ReadAllText(openFileDialog.FileName);
                LoadDataIntoList(data);
            }

            DisplayBlock.Text = "File Load Success";
        }

        private void LoadDataIntoList(string data)
        {
            nameList = new List<string>();

            var splitResults =  data.Split('\n');
            foreach(var name in splitResults)
            {
                var trimmedName = name.Trim('\r');
                nameList.Add(trimmedName);
            }
        }

        private void PickOneButton_Click(object sender, RoutedEventArgs e)
        {
            var indexOfWinner = rndManager.GetRandomIndex(nameList.ToArray(), winnerList.ToArray());
            var winner = nameList[indexOfWinner];

            if (winnerList.Contains(winner))
            {
                throw new ApplicationException("Winner not unique!");
            }

            winnerList.Add(winner);

            DisplayBlock.Text = "Winner: " + winner;
        }

        private void RandomSimulationButton_Click(object sender, RoutedEventArgs e)
        {
            simAmount = decimal.Parse(SimNumberText.Text);
            SimProgressBar.Value = 0;
            SimProgressBar.IsIndeterminate = true;            
            SimProgressBar.Visibility = Visibility.Visible;
            statsManager.ResetData();

            /*BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += UpdateSim;
            worker.ProgressChanged += UpdateProgressBar;

            worker.RunWorkerAsync();*/

            for (int i = 0; i < simAmount; i++)
            {
                statsManager.ProcessTurn(nameList.ToArray());
            }

            //Fix a rendering issue            
            SimProgressBar.IsIndeterminate = false;
            SimProgressBar.Value = 100;

            statsManager.SaveTurnStats();
        }

        /*private void UpdateProgressBar(object sender, ProgressChangedEventArgs e)
        {
            SimProgressBar.Value = e.ProgressPercentage;
        }

        private void UpdateSim(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < simAmount; i++)
            {
                statsManager.ProcessTurn(nameList.ToArray());
                var progressValue = i / simAmount * 100.0m;
                (sender as BackgroundWorker).ReportProgress((int)progressValue);
            }
        }*/
    }
}
