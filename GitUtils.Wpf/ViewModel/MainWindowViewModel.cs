using System.Collections.ObjectModel;

namespace GitUtils.Wpf.ViewModel;

public class MainWindowViewModel : BaseViewModel
{
    public FolderSelectorViewModel FolderSelectorViewModel { get; set; } = new();
    public CommitHashViewModel CommitHash1ViewModel { get; set; } = new();
    public CommitHashViewModel CommitHash2ViewModel { get; set; } = new();
    public ObservableCollection<CommitItemViewModel> Items { get; set; } = new();
}