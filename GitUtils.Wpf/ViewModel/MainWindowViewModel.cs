using GitUtils.Lib;
using GitUtils.Wpf.Model;
using GitUtils.Wpf.Service.Interface;
using System.Collections.ObjectModel;
using System.IO;

namespace GitUtils.Wpf.ViewModel;

public class MainWindowViewModel : BaseViewModel
{
    private string _selectedInputFolderPath;
    private string _selectedOutputFolderPath;
    private ObservableCollection<CommitItemViewModel> _items;
    private CommitHash _beforeCommitHash;
    private CommitHash _afterCommitHash;
    private readonly ISearchCommit _searchCommit;


    public string SelectedInputFolderPath
    {
        get => _selectedInputFolderPath;
        set
        {
            _selectedInputFolderPath = value;
            OnPropertyChanged(nameof(SelectedInputFolderPath));
        }
    }

    public string SelectedOutputFolderPath
    {
        get => _selectedOutputFolderPath;
        set
        {
            _selectedOutputFolderPath = value;
            if (string.IsNullOrEmpty(_selectedOutputFolderPath) == false)
            {
                if (Directory.Exists(_selectedOutputFolderPath)) return;
                Directory.CreateDirectory(_selectedOutputFolderPath);
            }
            OnPropertyChanged(nameof(SelectedOutputFolderPath));
        }
    }

    public CommitHash BeforeCommitHash
    {
        get => _beforeCommitHash;
        set
        {
            _beforeCommitHash = value;
            OnPropertyChanged(nameof(BeforeCommitHash));
        }
    }

    public CommitHash AfterCommitHash
    {
        get => _afterCommitHash;
        set
        {
            _afterCommitHash = value;
            OnPropertyChanged(nameof(AfterCommitHash));
        }
    }

    public ObservableCollection<CommitItemViewModel> Items
    {
        get => this._items;
        set
        {
            this._items = value;
            OnPropertyChanged(nameof(Items));
        }
    }

    public MainWindowViewModel(ISearchCommit searchCommit)
    {
        this._searchCommit = searchCommit;
        this._selectedOutputFolderPath = string.Empty;
        this._selectedInputFolderPath = string.Empty;
        this._beforeCommitHash = CommitHash.EmptyHash;
        this._afterCommitHash = CommitHash.EmptyHash;
        this._items = new ObservableCollection<CommitItemViewModel>();
    }

    public void GetCommits()
    {
        if (string.IsNullOrEmpty(this.SelectedInputFolderPath)) return;

        var commits = _searchCommit.SearchCommits(this.SelectedInputFolderPath, this._beforeCommitHash, this._afterCommitHash);
        Items = new ObservableCollection<CommitItemViewModel>(commits.Select(x=> new CommitItemViewModel(x)));
    }

    public void OutputFiles()
    {
        if (IsValidFolderPath()) return;

        foreach (var vm in Items)
        {
            if (!vm.IsChecked) continue;

            var commitHash = CommitHash.CreateCommitHash(vm.CommitHash);
            var commit = _searchCommit.SearchCommit(this.SelectedInputFolderPath, commitHash);
            
            var previousCommit = commit.Parents.FirstOrDefault();
            if (previousCommit is null)
            {
                // TODO:あとで
                return;
            }

            // TODO: ViewModelは知りたくない
            var repo = new Repository(this.SelectedInputFolderPath);
            repo.OutputChanges(previousCommit, commit, this.SelectedOutputFolderPath);
        }
    }

    public void OutputFiles2()
    {
        if (IsValidFolderPath()) return;

        var firstCommitViewModel = Items.First();
        var firstCommitHash = CommitHash.CreateCommitHash(firstCommitViewModel.CommitHash);
        var firstCommit = _searchCommit.SearchCommit(this.SelectedInputFolderPath, firstCommitHash);
        // TODO: コミットが一つだった場合
        foreach (var vm in Items.Skip(1))
        {
            if (!vm.IsChecked) continue;
            var commitHash = CommitHash.CreateCommitHash(vm.CommitHash);
            var commit = _searchCommit.SearchCommit(this.SelectedInputFolderPath, commitHash);

            // TODO: ViewModelは知りたくない
            var repo = new Repository(this.SelectedInputFolderPath);
            repo.OutputChangesAndOldChanges(firstCommit, commit, this.SelectedOutputFolderPath);
        }
    }

    private bool IsValidFolderPath()
    {
        // TODO: 正規表現とかで絞りたい
        return string.IsNullOrEmpty(this.SelectedInputFolderPath)
               && string.IsNullOrEmpty(this.SelectedOutputFolderPath);
    }
}