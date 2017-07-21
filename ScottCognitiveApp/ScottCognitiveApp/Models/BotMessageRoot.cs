using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ScottCognitiveApp.Models
{
    public class BotMessageRoot
    {
        public List<ChatMessage> Messages { get; set; }
        public string Watermark { get; set; }
    }
}