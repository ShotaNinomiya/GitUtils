using LibGit2Sharp;

namespace GitUtils.Lib;

public class Commit
{
    public string Author { get; init; }
    public string Email { get; init; }
    public string CommitHash { get; init; }
    public DateTimeOffset When { get; init; }
    public string MessageShort { get; init; }
    public string Message { get; init; }
    public IEnumerable<Commit> Parents { get; init; }
    public bool HasParents => Parents.Any();

    internal LibGit2Sharp.Commit RawCommit { get; init; }

    public Commit(string commitHash, IRepository repo)
    {
        var commit = repo.Lookup<LibGit2Sharp.Commit>(commitHash);
        if (commit == null)
            throw new ArgumentException();

        this.RawCommit = commit;

        this.Author = commit.Author.Name;
        this.Email = commit.Author.Email;
        this.CommitHash = commitHash;
        this.When = commit.Author.When;
        this.MessageShort = commit.MessageShort;
        this.Message = commit.Message;
        this.Parents = commit.Parents.Select(x=>new Commit(x));
    }

    internal Commit(LibGit2Sharp.Commit commit)
    {
        this.RawCommit = commit;

        this.Author = commit.Author.Name;
        this.Email = commit.Author.Email;
        this.CommitHash = commit.Sha;
        this.When = commit.Author.When;
        this.MessageShort = commit.MessageShort;
        this.Message = commit.Message;
        this.Parents = commit.Parents.Select(x => new Commit(x));
    }
}