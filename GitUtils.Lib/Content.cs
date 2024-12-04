using LibGit2Sharp;

namespace GitUtils.Lib;

public static class Content
{
    public static string Read(Blob blob)
    {
        using var contentStream = blob.GetContentStream();
        using var reader = new StreamReader(contentStream);
        return reader.ReadToEnd();
    }
}
