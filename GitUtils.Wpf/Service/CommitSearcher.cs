using GitUtils.Lib;
using GitUtils.Wpf.Model;
using GitUtils.Wpf.Service.Interface;

namespace GitUtils.Wpf.Service;

public class CommitSearcher : ISearchCommit
{
    private readonly IRepositoryCreator _repositoryCreator;
    public CommitSearcher(IRepositoryCreator repositoryCreator)
    {
        this._repositoryCreator = repositoryCreator;
    }
    public Commit SearchCommit(string path, CommitHash commitHash)
    {
        var repo = _repositoryCreator.Create(path);
        return repo.GetCommit(commitHash.Hash);
    }

    public IEnumerable<Commit> SearchCommits(string path, CommitHash beforeCommitHash, CommitHash afterCommitHash)
    {
        var repo = _repositoryCreator.Create(path);
        var beforeCommit = repo.GetCommit(beforeCommitHash.Hash);
        var afterCommit = repo.GetCommit(afterCommitHash.Hash);
        return repo.GetLog(beforeCommit, afterCommit);
    }
}
