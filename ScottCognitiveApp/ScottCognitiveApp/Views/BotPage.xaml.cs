
using ScottCognitiveApp.ViewModels;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System;
using System.Collections.Generic;

using ScottCognitiveApp.Helpers;
using ScottCognitiveApp.Models;

namespace ScottCognitiveApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BotPage : ContentPage
	{
        BotViewModel vm;
        public BotPage()
        {
            InitializeComponent();
            BindingContext = vm = ViewModelLocator.MainViewModel;
            
        }
        


        protected override void OnAppearing()
        {
            base.OnAppearing();
            

            vm.LoadChatMessagesCommand.Execute(null);

        }
        
        


    }
}