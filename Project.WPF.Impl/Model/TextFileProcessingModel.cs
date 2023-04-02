namespace Project.WPF.Impl.Model
{
    using Project.WPF.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class TextFileProcessingModel : ITextFileProcessingModel
    {
        #region Fields
        private Dictionary<string, int>? _repeatedWordCount;
        private bool _processCanceled;
        #endregion

        #region Contructors
        public TextFileProcessingModel()
        {
        }
        #endregion

        #region Events
        public event EventHandler<Dictionary<string, int>>? ProcessComplete;
        public event EventHandler<int>? ProgressChanged;
        public event EventHandler? ProcessCanceled;
        #endregion 

        #region Methods
        public void ProcessFileText(string text)
        {
            try
            {
                this._processCanceled = false;
                this._repeatedWordCount = new Dictionary<string, int>();
                int helpNum = 1;
                var words = text.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    //Sleep for better progress bar visualization
                    Thread.Sleep(100);
                    if (this._processCanceled)
                    {
                        this.ProcessCanceled?.Invoke(this, EventArgs.Empty);
                        break;
                    }

                    if (this._repeatedWordCount.ContainsKey(word))
                    {
                        int value = this._repeatedWordCount[word];
                        this._repeatedWordCount[word] = value + 1;
                    }
                    else
                    {
                        this._repeatedWordCount.Add(word, 1);
                    }
                    int percentComplete = Convert.ToInt32(100 * helpNum / words.Length);
                    this.ProgressChanged?.Invoke(this, percentComplete);
                    helpNum++;
                }
            }
            catch
            {
                this.ProcessCanceled?.Invoke(this, EventArgs.Empty);
            }

            if (this._repeatedWordCount != null)
            {
                this.ProcessComplete?.Invoke(this, this._repeatedWordCount);
            }
        }

        public void Cancel()
        {
            this._processCanceled = true;
        }
        #endregion
    }
}
