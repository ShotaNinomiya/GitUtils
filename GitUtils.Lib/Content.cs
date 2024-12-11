using LibGit2Sharp;

namespace GitUtils.Lib;

public static class Content
{
    public static void OutputFile(Blob blob, string fileOutputPath)
    {
        using var contentStream = blob.GetContentStream();
        using var fileStream = File.Create(fileOutputPath);
        contentStream.CopyTo(fileStream);
    }

    public static void ExtractTreeEntryFromCommit(Commit commit, string currentPath)
    {
        foreach (var entry in commit.RawCommit.Tree)
        {
            ExtractTreeEntry(entry, currentPath);
        }
    }

    private static void ExtractTreeEntry(TreeEntry entry, string currentPath)
    {
        switch (entry.TargetType)
        {
            case TreeEntryTargetType.Blob:
            {
                // ファイルの場合
                var blob = (Blob)entry.Target;
                var filePath = Path.Combine(currentPath, entry.Path.Replace('/', Path.DirectorySeparatorChar));

                // ファイルのディレクトリが存在しない場合は作成
                var directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // ファイルの内容を取得して保存
                using var contentStream = blob.GetContentStream();
                using var fileStream = File.Create(filePath);
                contentStream.CopyTo(fileStream);
                break;
            }
            case TreeEntryTargetType.Tree:
            {
                // フォルダの場合、再帰的に処理
                Tree tree = (Tree)entry.Target;
                foreach (var childEntry in tree)
                {
                    ExtractTreeEntry(childEntry, currentPath);
                }

                break;
            }
            case TreeEntryTargetType.GitLink:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
