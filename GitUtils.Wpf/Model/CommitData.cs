using GitUtils.Lib;

namespace GitUtils.Wpf.Model;

public class CommitData
{
    public string Author { get; set; }
    public string Email { get; set; }
    public CommitHash CommitHash { get; set; }
    public DateTimeOffset DateTime { get; set; }
    public string Message { get; set; }
    public string MessageLong { get; set; }

    public CommitData(CommitHash commitHash, Repository repo)
    {
        if(commitHash is null || string.IsNullOrEmpty(commitHash.Hash))
            throw new ArgumentNullException(nameof(commitHash));

        var commit = repo.GetCommit(commitHash.Hash);
        if (commit == null)
            throw new ArgumentException();

        this.Author = commit.Author;
        this.Email = commit.Email;
        this.CommitHash = commitHash;
        this.DateTime = commit.When;
        this.Message = commit.MessageShort;
        this.MessageLong = commit.Message;
    }
}