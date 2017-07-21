using ScottCognitiveApp.Bot;
using ScottCognitiveApp.Models;
using ScottCognitiveApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScottCognitiveApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BotPage : ContentPage
	{
        
        public BotPage()
        {
            InitializeComponent();
            BindingContext = new BotViewModel();
            
        }
        

    }
}