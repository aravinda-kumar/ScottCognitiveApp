using Plugin.Media;
using Plugin.Media.Abstractions;
using ScottCognitiveApp.Bot;
using ScottCognitiveApp.Common;
using ScottCognitiveApp.Models;
using ScottCognitiveApp.Models.Image;
using ScottCognitiveApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace ScottCognitiveApp.ViewModels
{
    class BotViewModel : INotifyPropertyChanged
    {

        private string userName = "Scott";
        private string botName = "Bot02";
        private AzureDataService azureDataService;
        private BotConnector _botConnector;
        private Conversation _lastConversation;
        public ObservableCollection<ChatMessage> _conversationList = new ObservableCollection<ChatMessage>();

        private ImageResult _imageResult;
        private string _imageUrl;
        private Stream _imageStream;
        private string _errorMessage;
        private bool _isBusy;
        private string _entryText;
        private string[] myImages;
        private ChatMessage _chatMessageTemp;

        private const string ComputerVisionApiKey = "2a55f74b65344ac7b9f00cde32bf40a7";
        private readonly ComputerVisionService _computerVisionService = new ComputerVisionService(ComputerVisionApiKey);

        public ImageResult ImageResult
        {
            get { return _imageResult; }
            set
            {
                _imageResult = value;
                OnPropertyChanged();
            }
        }

        

        public ObservableCollection<ChatMessage> ConversationList
        {
            get { return _conversationList; }
            set { _conversationList = value; }
        }
        
        string loadingMessage;
        public string LoadingMessage
        {
            get { return loadingMessage; }
            set { loadingMessage = value; OnPropertyChanged(); }
        }

        ICommand loadMessagesCommand;
        public ICommand LoadChatMessagesCommand =>
            loadMessagesCommand ?? (loadMessagesCommand = new Command(async () => await ExecuteLoadMessagesCommandAsync()));


        async Task ExecuteLoadMessagesCommandAsync()
        {
            try
            {
                LoadingMessage = "Loading Messages...";
                IsBusy = true;
                var messages = await azureDataService.GetMessages();

                _conversationList.Clear();

                

                foreach (ChatMessage message in messages)
                {
                    _conversationList.Add(message);
                }
                

            }
            catch (Exception ex)
            {

                await Application.Current.MainPage.DisplayAlert("Sync Error", "Unable to sync chat messages, you may be offline", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }


        public Command TakePhotoCommand
        {
            get
            {
                return new Command(async () =>
                {
                    // Don't forget to install nuget package Xam.Plugin.Media 
                    // on all Solution projects
                    await CrossMedia.Current.Initialize();

                    var mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());

                    _imageStream = mediaFile?.GetStream();

                    ImageUrl = mediaFile?.Path;

                    IsBusy = true;

                    try
                    {
                        ImageResult = null;
                        ErrorMessage = string.Empty;

                        ImageResult = await _computerVisionService.AnalyseImageStreamAsync(_imageStream);
                    }
                    catch (Exception exception)
                    {
                        ErrorMessage = exception.Message;
                    }

                    IsBusy = false;

                });
            }
        }

        public Command PickPhotoCommand
        {
            get
            {
                return new Command(async () =>
                {
                    // Don't forget to install nuget package Xam.Plugin.Media 
                    // on all Solution projects
                    await CrossMedia.Current.Initialize();

                    var mediaFile = await CrossMedia.Current.PickPhotoAsync();

                    _imageStream = mediaFile?.GetStream();

                    ImageUrl = mediaFile?.Path;

                    IsBusy = true;

                    try
                    {
                        ImageResult = null;
                        ErrorMessage = string.Empty;

                        ImageResult = await _computerVisionService.AnalyseImageStreamAsync(_imageStream);

                        /***
                        int imageNum = 1;
                        string[] _images = new string[imageNum];
                        _images[0] = ImageUrl;
                        Attachment[] _attachments = new Attachment[imageNum];

                        _attachments[0] = new Attachment
                        {
                            ContentType = "image/jpg",
                            Stream = _imageStream,
                            Url = ImageUrl
                        };

                        var imageToSend = new ChatMessage
                        {
                            From = userName,
                            DateUtc = DateTime.UtcNow,
                            Images = _images,
                            Attachments = _attachments,
                            ConversationId = _lastConversation.ConversationId
                        };
                        **/
                        

                        //_conversationList.Add(imageToSend);
                        
                        var messageFeedback = new ChatMessage
                        {
                            From = botName,
                            DateUtc = DateTime.UtcNow,
                            Text = "I guess the picture shows that "+ ImageResult.Description.Captions.FirstOrDefault().Text + ".",
                            ConversationId = _lastConversation.ConversationId
                        };
                        
                        //var botResponse = await _botConnector.SendMessage(messageToSend);
                        
                        

                        await azureDataService.AddMessage(messageFeedback);

                        _conversationList.Add(messageFeedback);
                        

                        //ImageResult = await _computerVisionService.AnalyseImageStreamAsync(_imageStream);
                    }
                    catch (Exception exception)
                    {
                        ErrorMessage = exception.Message;
                    }

                    IsBusy = false;

                });
            }
        }


        
        public string EntryText
        {
            get { return _entryText; }
            set
            {
                _entryText = value;
                OnPropertyChanged();
            }
        }


        private ICommand _sendMessageCommand;
        public ICommand SendMessageCommand
        {
            get
            {
                return _sendMessageCommand;
            }
            set
            {
                _sendMessageCommand = value;
            }
        }

        private ICommand _removeMessageCommand;
        public ICommand RemoveMessageCommand
        {
            get
            {
                return _removeMessageCommand;
            }
            set
            {
                _removeMessageCommand = value;
            }
        }


        public BotViewModel()
        {
            azureDataService = DependencyService.Get<AzureDataService>();
            _botConnector = new BotConnector();
            setupBotConnectorAsync();
            EntryText = _entryText;

            SendMessageCommand = new RelayCommand(new Action<object>(SendMessage));
            RemoveMessageCommand = new RelayCommand(new Action<object>(RemoveMessage));
        }


        private async void SendMessage(object message)
        {
            if (!string.IsNullOrWhiteSpace(EntryText))
            {
                var mytext = EntryText;


                EntryText = string.Empty;

                var messageToSend = new ChatMessage
                {
                    From = userName,
                    Text = mytext,
                    ConversationId = _lastConversation.ConversationId,
                    DateUtc = DateTime.UtcNow
                };
                
                _conversationList.Add(messageToSend);

                await azureDataService.AddMessage(messageToSend);
                
                var botResponse = await _botConnector.SendMessage(messageToSend);

                

                var messageFromBot = new ChatMessage
                {
                    From = botName,
                    Text = botResponse.Text,
                    ConversationId = _lastConversation.ConversationId,
                    DateUtc = DateTime.UtcNow
                };
                

                await azureDataService.AddMessage(messageFromBot);
                _conversationList.Add(messageFromBot);
            }
        }

        private async void RemoveMessage(object message)
        {
            if (message != null)
            {
                ChatMessage back_message = await azureDataService.GetMessage(((ChatMessage)message).Id);

                if (back_message != null)
                {
                    _conversationList.Remove((ChatMessage)message);
                    await azureDataService.RemoveMessage(back_message);
                    

                }
            }
        }


        private async Task setupBotConnectorAsync()
        {
            _lastConversation = await _botConnector.Setup();
        }

        


        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                _imageUrl = value;
                OnPropertyChanged();
            }
        }


        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
