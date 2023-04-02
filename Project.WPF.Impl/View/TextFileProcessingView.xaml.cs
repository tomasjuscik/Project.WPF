namespace Project.WPF.Impl.View
{
    using Project.WPF.Impl.ViewModel;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for TextFileProcessingView.xaml
    /// </summary>
    public partial class TextFileProcessingView : Page
    {
        public TextFileProcessingView()
        {
            InitializeComponent();
            this.DataContext = new TextFileProcessingViewModel();
        }
    }
}
