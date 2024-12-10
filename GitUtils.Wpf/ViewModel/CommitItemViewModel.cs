using GitUtils.Lib;

namespace GitUtils.Wpf.ViewModel;

public class CommitItemViewModel : BaseViewModel
{
    private string? _date;
    private string? _commitHash;
    private string? _author;
    private string? _message;
    private bool _isChecked;

    public string? Date
    {
        get => _date;
        set
        {
            _date = value;
            OnPropertyChanged(nameof(Date));
        }
    }

    public string? CommitHash
    {
        get => _commitHash;
        set
        {
            _commitHash = value;
            OnPropertyChanged(nameof(CommitHash));
        }
    }

    public string? Author
    {
        get => _author;
        set
        {
            _author = value;
            OnPropertyChanged(nameof(Author));
        }
    }

    public string? Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged(nameof(Message));
        }
    }

    public bool IsChecked
    {
        get => _isChecked;
        set
        {
            _isChecked = value;
            OnPropertyChanged(nameof(IsChecked));
        }
    }

    public CommitItemViewModel(Commit commit)
    {
        this._author = commit.Author;
        this._message = commit.Message;
        this._date = commit.When.ToString();
        this._commitHash = commit.CommitHash;
    }
}