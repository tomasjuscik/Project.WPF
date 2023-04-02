namespace Project.WPF.Interfaces
{
    public interface ITextFileProcessingModel
    {
        void ProcessFileText(string text);
        void Cancel();
        event EventHandler<Dictionary<string, int>>? ProcessComplete;
        event EventHandler<int>? ProgressChanged;
        event EventHandler ProcessCanceled;
    }
}
