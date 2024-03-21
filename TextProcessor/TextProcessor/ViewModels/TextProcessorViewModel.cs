using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TextProcessor.Models;

namespace TextProcessor.ViewModels
{
    public class TextProcessorViewModel : INotifyPropertyChanged
    {
        public ICommand ProcessCommand => new RelayCommand(async p => await Process());
        public ICommand AddFileCommand => new RelayCommand(p => AddFile());

        public int MinWordLength
        {
            get { return _minWordLength; }
            set
            {
                if (_minWordLength != value)
                {
                    _minWordLength = value;
                    OnPropertyChanged(nameof(MinWordLength));
                }
            }
        }
        private int _minWordLength;

        public bool IsProcess
        {
            get { return _isProcess; }
            set
            {
                if (_isProcess != value)
                {
                    _isProcess = value;
                    OnPropertyChanged(nameof(IsProcess));
                }
            }
        }
        private bool _isProcess = false;

        public bool RemovePunctuation
        {
            get { return _removePunctuation; }
            set
            {
                if (_removePunctuation != value)
                {
                    _removePunctuation = value;
                    OnPropertyChanged(nameof(RemovePunctuation));
                }
            }
        }
        private bool _removePunctuation;

        public static ObservableCollection<string> InputFiles { get; } = new ObservableCollection<string>();

        public TextProcessorViewModel()
        {

        }

        private async Task Process()
        {
            this.IsProcess = true;
            TextProcessorModel model = new TextProcessorModel();
            foreach (string inputFile in InputFiles)
            {
                await model.ProcessText(inputFile, inputFile, MinWordLength, RemovePunctuation);
            }
            this.IsProcess = false;
        }

        private void AddFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    InputFiles.Add(fileName);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
