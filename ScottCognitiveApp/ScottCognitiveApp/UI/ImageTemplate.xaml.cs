using ScottCognitiveApp.Models;
using ScottCognitiveApp.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScottCognitiveApp.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageTemplate : ViewCell
    {
        public ImageTemplate()
        {
            InitializeComponent();
        }
        

        public void OnDelete(object sender, EventArgs e)
        {
            var message = BindingContext as ChatMessage;

            var vm = ViewModelLocator.MainViewModel;

            vm.RemoveMessageCommand.Execute(message);
        }
    }
}