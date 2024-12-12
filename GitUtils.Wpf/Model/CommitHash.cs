using System.Text.RegularExpressions;

namespace GitUtils.Wpf.Model;

public class CommitHash
{
    private const string Empty = "0000000000000000000000000000000000000000";

    public string Hash { get; init; }
    public string HashShort => Hash[..8];
    public static CommitHash EmptyHash => CreateCommitHash(Empty);

    public CommitHash(string hash)
    {
        this.Hash = hash;
    }

    public CommitHash(CommitHash commitHash)
    {
        this.Hash = commitHash.Hash;
    }

    public static CommitHash CreateCommitHash(string hash)
    {
        return IsFullGitHash(hash) ? new CommitHash(hash) : EmptyHash;
    }

    public static CommitHash CreateCommitHash(CommitHash commitHash)
    {
        return new CommitHash(commitHash);
    }

    public static bool IsFullGitHash(string input)
    {
        // 長さが40であるか
        return input is { Length: 40 } &&
               Regex.IsMatch(input, @"^[0-9a-fA-F]{40}$");
    }
}