using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HF_16_StarryNight.View
{
    /// <summary>
    /// Interaction logic for BeesOnAStarryNight.xaml
    /// </summary>
    public partial class BeesOnAStarryNight : Window
    {
        ViewModel.BeeStarViewModel viewModel;

        public BeesOnAStarryNight()
        {
            InitializeComponent();
            viewModel = FindResource("viewModel") as ViewModel.BeeStarViewModel; ;
        }

        private void SizeChangedHandler(object sender, SizeChangedEventArgs e)
        {
            viewModel.PlayAreaSize = new Size(e.NewSize.Width, e.NewSize.Height);
        }
    }
}
