namespace GitUtils.Wpf.ViewModel;

public class CommitHashViewModel : BaseViewModel
{
    private string? _commitHash;
    public string? CommitHash
    {
        get => _commitHash;
        set
        {
            _commitHash = value;
            OnPropertyChanged(nameof(CommitHash));
        }
    }
}