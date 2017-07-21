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
            var stack = Navigation.NavigationStack;
            if (stack[stack.Count - 1].GetType() != typeof(ScottCognitiveApp.Views.ComputerVisionPage))
                await Navigation.PushAsync(new ScottCognitiveApp.Views.ComputerVisionPage());
            
        }

        private async void AIRobotButton_OnClicked(object sender, EventArgs e)
        {
            var stack = Navigation.NavigationStack;
            if (stack[stack.Count - 1].GetType() != typeof(ScottCognitiveApp.Views.BotPage))
                await Navigation.PushAsync(new ScottCognitiveApp.Views.BotPage());
        }


    }

}
