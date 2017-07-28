using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ScottCognitiveApp.UI;
using ScottCognitiveApp.ViewModels;
using ScottCognitiveApp.Models;

namespace ScottCognitiveApp.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceiveTemplate : ViewCell
    {
        public ReceiveTemplate()
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