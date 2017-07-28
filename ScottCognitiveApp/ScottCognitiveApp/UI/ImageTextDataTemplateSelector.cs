using ScottCognitiveApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ScottCognitiveApp.UI
{
    public class ImageTextDataTemplateSelector : DataTemplateSelector
    {

        public DataTemplate _receiveTemplate = new DataTemplate(typeof(ReceiveTemplate));
        public DataTemplate _sendTemplate = new DataTemplate(typeof(SendTemplate));

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var _chatMessage = item as ChatMessage;

            /***
             if (_chatMessage.From == "Scott")
            {
                return _sendTemplate;
            }

            **/
            //red

            if (_chatMessage != null)
            {
                if (_chatMessage.From == "Scott")
                {
                    return _sendTemplate;
                }
            }

            return _receiveTemplate;
            
            
        }
    }
}
