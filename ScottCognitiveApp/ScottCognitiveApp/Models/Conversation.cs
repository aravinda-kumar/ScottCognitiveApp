using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ScottCognitiveApp.Models
{
    public class Conversation
    {
        public string ConversationId { get; set; }
        public string Token { get; set; }
        public string ETag { get; set; }
    }
}