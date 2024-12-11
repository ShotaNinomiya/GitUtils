using GitUtils.Wpf.Model;
using GitUtils.Wpf.Service;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.IO;
using GitUtils.Lib;

namespace GitUtils.Wpf.ViewModel;

public class MainWindowViewModel : BaseViewModel
{
    private string? _selectedFolderPath;
    private string? _selectedOutputFolderPath;
    private ObservableCollection<CommitItemViewModel> _items;
    private readonly CommitHash _beforeCommitHash;
    private readonly CommitHash _afterCommitHash;
    private readonly ISearchCommit _searchCommit;


    public string? SelectedFolderPath
    {
        get => _selectedFolderPath;
        set
        {
            _selectedFolderPath = value;
            OnPropertyChanged(nameof(SelectedFolderPath));
        }
    }

    public string? SelectedOutputFolderPath
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

    public string BeforeCommitHash
    {
        get => _beforeCommitHash.Hash;
        set
        {
            if (string.IsNullOrEmpty(value)
                || _beforeCommitHash.Hash == value) return;

            _beforeCommitHash.Hash = value;
            OnPropertyChanged(nameof(BeforeCommitHash));
        }
    }

    public string AfterCommitHash
    {
        get => _afterCommitHash.Hash;
        set
        {
            if (string.IsNullOrEmpty(value)
                || _afterCommitHash.Hash == value) return;

            _afterCommitHash.Hash = value;
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
        this._beforeCommitHash = new CommitHash();
        this._afterCommitHash = new CommitHash();
        this._items = new ObservableCollection<CommitItemViewModel>();
    }

    public void GetCommits()
    {
        if (string.IsNullOrEmpty(this.SelectedFolderPath)) return;

        var commits = _searchCommit.SearchCommits(this.SelectedFolderPath, this._beforeCommitHash, this._afterCommitHash);
        Items = new ObservableCollection<CommitItemViewModel>(commits.Select(x=> new CommitItemViewModel(x)));
    }

    public void OutputFiles()
    {
        foreach (var vm in Items)
        {
            if (!vm.IsChecked) continue;

            var commit = _searchCommit.SearchCommit(this.SelectedFolderPath, new CommitHash() { Hash = vm.CommitHash });
            
            var previousCommit = commit.Parents.FirstOrDefault();
            if (previousCommit is null)
            {
                // TODO:あとで
                return;
            }

            // TODO: ViewModelは知りたくない
            var repo = new Repository(this.SelectedFolderPath);
            repo.OutputChanges(previousCommit, commit, this.SelectedOutputFolderPath);
        }
    }

    public void OutputFiles2()
    {
        var firstCommitViewModel = Items.First();
        var firstCommit = _searchCommit.SearchCommit(this.SelectedFolderPath, new CommitHash() { Hash = firstCommitViewModel.CommitHash });
        foreach (var vm in Items.Skip(1))
        {
            if (!vm.IsChecked) continue;

            var commit = _searchCommit.SearchCommit(this.SelectedFolderPath, new CommitHash() { Hash = vm.CommitHash });

            // TODO: ViewModelは知りたくない
            var repo = new Repository(this.SelectedFolderPath);
            repo.OutputChangesAndOldChanges(firstCommit, commit, this.SelectedOutputFolderPath);
        }
    }
}