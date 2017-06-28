using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ScottCognitiveApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        //take photo button clicked 
        private async void ComputerVisionTestButton_OnClicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new CognitiveServices.Views.ComputerVisionPage());
        }

        //pick photo button clicked 
        private async void EmotionTestButton_OnClicked(object sender, EventArgs e)
        {


            await Navigation.PushAsync(new CognitiveServices.Views.EmotionPage());

        }

        //take Video button clicked 
        private async void OcrTestButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CognitiveServices.Views.OcrPage());
        }

        //pick Video button clicked 
        private async void TextAnalyticsTestButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CognitiveServices.Views.TextAnalyticsPage());
        }

        private async void AIRobotButton_OnClicked(object sender, EventArgs e)
        {

        }


    }
}
