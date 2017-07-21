using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using ScottCognitiveApp.Models.Image;
using ScottCognitiveApp.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace ScottCognitiveApp.ViewModels
{
    public class ComputerVisionViewModel : INotifyPropertyChanged
    {

        private ImageResult _imageResult;
        /// <summary>
        /// Get a subscription key from:
        /// https://www.microsoft.com/cognitive-services/en-us/subscriptions
        /// The following API Key may stop working at anytime, so get your own!
        /// </summary>
        private const string ComputerVisionApiKey = "d5fdc78fad5b4cf98fce5df15146426d";
        private readonly ComputerVisionService _computerVisionService = new ComputerVisionService(ComputerVisionApiKey);
        private string _imageUrl;
        private Stream _imageStream;
        private string _errorMessage;
        private bool _isBusy;

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

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
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
                    }
                    catch (Exception exception)
                    {
                        ErrorMessage = exception.Message;
                    }

                    IsBusy = false;

                });
            }
        }






        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
