namespace GitUtils.Wpf.ViewModel;

public class CommitHashViewModel : BaseViewModel
{
    private string? _commitHash;
    private string? _name;
    public string? CommitHash
    {
        get => _commitHash;
        set
        {
            _commitHash = value;
            OnPropertyChanged(nameof(CommitHash));
        }
    }

    public string? Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }
}