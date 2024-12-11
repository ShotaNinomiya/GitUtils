using GitUtils.Wpf.Model;
using LibGit2Sharp;
using Commit = GitUtils.Lib.Commit;
using Repository = GitUtils.Lib.Repository;

namespace GitUtils.Wpf.Service;

public class CommitSearcher : ISearchCommit
{
    private static Repository CreateRepository(string path)
    {
        return new Repository(path);
    }

    public Commit SearchCommit(string path, CommitHash commitHash)
    {
        var repo = CreateRepository(path);
        return repo.GetCommit(commitHash.Hash);
    }

    public IEnumerable<Commit> SearchCommits(string path, CommitHash beforeCommitHash, CommitHash afterCommitHash)
    {
        var repo = CreateRepository(path);
        var beforeCommit = repo.GetCommit(beforeCommitHash.Hash);
        var afterCommit = repo.GetCommit(afterCommitHash.Hash);
        return repo.GetLog(beforeCommit, afterCommit);
    }


}