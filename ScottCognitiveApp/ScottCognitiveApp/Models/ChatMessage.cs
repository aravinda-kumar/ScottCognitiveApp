using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using System.IO;

namespace ScottCognitiveApp.Models
{
    public class ChatMessage
    {
        [Newtonsoft.Json.JsonProperty("Id")]
        public string Id { get; set; }

        [Microsoft.WindowsAzure.MobileServices.Version]
        public string AzureVersion { get; set; }

        public string ConversationId { get; set; }
        public DateTime DateUtc { get; set; }
        public string From { get; set; }
        public string Text { get; set; }
        public string ChannelData { get; set; }
        public string[] Images { get; set; }
        public Attachment[] Attachments { get; set; }
        public string ETag { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string DateDisplay { get { return DateUtc.ToLocalTime().ToString("d"); } }


    }

    public class Attachment
    {
        public string Url { get; set; }
        public Stream Stream { get; set; }
        public string ContentType { get; set; }
    }
}