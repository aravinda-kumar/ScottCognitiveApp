using ScottCognitiveApp.Models;
using ScottCognitiveApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScottCognitiveApp.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendTemplate : ViewCell
    {

        public SendTemplate()
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