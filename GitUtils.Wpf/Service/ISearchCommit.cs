using GitUtils.Lib;
using GitUtils.Wpf.Model;

namespace GitUtils.Wpf.Service;

public interface ISearchCommit
{
    Commit SearchCommit(string path, CommitHash commitHash);
    IEnumerable<Commit> SearchCommits(string path, CommitHash beforeCommitHash, CommitHash afterCommitHash);
}