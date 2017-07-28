using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottCognitiveApp.ViewModels
{
    class ViewModelLocator
    {

        private static BotViewModel _botViewModel = new BotViewModel();

        public static BotViewModel MainViewModel
        {
            get
            {
                return _botViewModel;
            }
        }

    }
}
