namespace Project.WPF.Impl.ViewModel
{
    using Microsoft.Win32;
    using Project.WPF.Impl.Model;
    using Project.WPF.Interfaces;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class TextFileProcessingViewModel : INotifyPropertyChanged
    {
        #region Fields
        private readonly ITextFileProcessingModel textFileProcessingModel;
        private string _status = string.Empty;
        private long _maximumPercent;
        private string _percentCompleteToShow = string.Empty;
        private float _percentComplete;
        private Dictionary<string, int>? _result;
        private bool _isCancelEn;
        private string _text = string.Empty;
        private ICommand? _cancelCommand;
        private ICommand? _browserCommand;
        private ICommand? _processCommand;
        #endregion

        #region Constructors
        public TextFileProcessingViewModel()
        {
            this.textFileProcessingModel = new TextFileProcessingModel();
            this.textFileProcessingModel.ProcessComplete += ProcessComplete;
            this.textFileProcessingModel.ProgressChanged += ProgressChanged;
            this.textFileProcessingModel.ProcessCanceled += ProcessCanceled;
        }
        #endregion 

        #region Properties
        public string Status
        {
            get
            {
                return this._status;
            }
            set
            {
                this._status = value;
                this.OnPropertyChanged(nameof(Status));
            }
        }

        public long MaximumPercent
        {
            get
            {
                return this._maximumPercent;
            }
            set
            {
                this._maximumPercent = value;
                this.OnPropertyChanged(nameof(MaximumPercent));
            }
        }

        public string PercentCompleteToShow
        {
            get
            {
                return this._percentCompleteToShow;
            }
            set
            {
                this._percentCompleteToShow = value;
                this.OnPropertyChanged(nameof(PercentCompleteToShow));
            }
        }

        public float PercentComplete
        {
            get
            {
                return this._percentComplete;
            }
            set
            {
                this._percentComplete = value;
                this.OnPropertyChanged(nameof(PercentComplete));
            }
        }


        public Dictionary<string, int>? Result
        {
            get
            {
                return this._result;
            }
            set
            {
                this._result = value;
                this.OnPropertyChanged(nameof(Result));
            }
        }

        public bool IsCancelEn
        {
            get
            {
                return this._isCancelEn;
            }
            set
            {
                this._isCancelEn = value;
                this.OnPropertyChanged(nameof(IsCancelEn));
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                if (this._cancelCommand == null)
                {
                    this._cancelCommand = new CommandHandler(() => this.Cancel());
                }

                return this._cancelCommand;
            }
        }

        public ICommand BrowserCommand
        {
            get
            {
                if (this._browserCommand == null)
                {
                    this._browserCommand = new CommandHandler(() => this.OpenTextFile());
                }

                return this._browserCommand;
            }
        }

        public ICommand ProcessCommand
        {
            get
            {
                if (this._processCommand == null)
                {
                    this._processCommand = new CommandHandler(() => this.ProcessTextFile());
                }

                return this._processCommand;
            }
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        #region Methods
        private async void Cancel()
        {
            await Task.Run(() => this.textFileProcessingModel.Cancel());
        }

        private void OpenTextFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                this._text = File.ReadAllText(fileDialog.FileName);
            }
        }

        private async void ProcessTextFile()
        {
            Result = null;
            Status = "Processing.....";
            IsCancelEn = true;
            await Task.Run(() => this.textFileProcessingModel.ProcessFileText(this._text));
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ProcessComplete(object? sender, Dictionary<string, int> e)
        {
            Result = e.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            if (Status != "Canceled")
            {
                Status = "Complete";
            }
            IsCancelEn = false;
        }

        private void ProcessCanceled(object? sender, System.EventArgs e)
        {
            Status = "Canceled";
        }

        private void ProgressChanged(object? sender, int e)
        {
            PercentComplete = e;
            PercentCompleteToShow = e.ToString() + "%";
        }

        #endregion
    }
}
