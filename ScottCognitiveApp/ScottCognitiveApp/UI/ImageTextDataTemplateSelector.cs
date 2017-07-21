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

        public DataTemplate _textTemplate = new DataTemplate(typeof(TextTemplate));
        public DataTemplate _imageTemplate = new DataTemplate(typeof(ImageTemplate));

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var _chatMessage = item as ChatMessage;

            Debug.WriteLine(_chatMessage.Images == null);

            if (_chatMessage.Images != null && _chatMessage.Images.Length > 0)
            {
                return _imageTemplate;
            }

            return _textTemplate;
        }
    }
}
