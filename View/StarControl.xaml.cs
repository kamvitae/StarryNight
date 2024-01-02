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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace HF_16_StarryNight.View
{
    /// <summary>
    /// Interaction logic for StarControl.xaml
    /// </summary>
    public sealed partial class StarControl : UserControl
    {
        Storyboard fadeInStoryboard = new Storyboard();
        Storyboard fadeOutStoryboard = new Storyboard();
        public StarControl()
        {
            InitializeComponent();
            fadeInStoryboard = FindResource("fadeInStoryboard") as Storyboard; // important as fuck. this was missing
            fadeOutStoryboard = FindResource("fadeOutStoryboard") as Storyboard;
        }
        public void FadeIn()
        {
           fadeInStoryboard.Begin();
        }
        public void FadeOut()
        {
           fadeOutStoryboard.Begin();
        }
    }
}
