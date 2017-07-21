using System;
using System.Collections.Generic;
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
            return _textTemplate;
        }
    }
}
