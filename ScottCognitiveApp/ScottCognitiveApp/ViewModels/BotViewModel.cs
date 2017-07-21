using Plugin.Media;
using Plugin.Media.Abstractions;
using ScottCognitiveApp.Bot;
using ScottCognitiveApp.Common;
using ScottCognitiveApp.Models;
using ScottCognitiveApp.Models.Image;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private BotConnector _botConnector;
        private Conversation _lastConversation;
        public ObservableCollection<ChatMessage> _conversationList = new ObservableCollection<ChatMessage>();

        private ImageResult _imageResult;
        private string _imageUrl;
        private Stream _imageStream;
        private string _errorMessage;
        private bool _isBusy;
        private string _entryText;


        public ObservableCollection<ChatMessage> ConversationList
        {
            get { return _conversationList; }
            set { _conversationList = value; }
        }

        public BotViewModel()
        {
            _botConnector = new BotConnector();
            setupBotConnectorAsync();
            EntryText = _entryText;
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


        

        public ICommand SendMessage
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    if (!string.IsNullOrWhiteSpace(EntryText))
                    {
                        var mytext = EntryText;

                        EntryText = string.Empty;
                        
                        var messageToSend = new ChatMessage() { From = userName, Text = mytext, ConversationId = _lastConversation.ConversationId };

                        _conversationList.Add(messageToSend);

                        var botResponse = await _botConnector.SendMessage(userName, mytext);

                        _conversationList.Add(botResponse);
                    }
                    
                });
            }
        }



        

        private async Task setupBotConnectorAsync()
        {
            _lastConversation = await _botConnector.Setup();
        }

        public ImageResult ImageResult
        {
            get { return _imageResult; }
            set
            {
                _imageResult = value;
                OnPropertyChanged();
            }
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
