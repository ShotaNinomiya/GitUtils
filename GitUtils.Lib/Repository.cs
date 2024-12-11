using LibGit2Sharp;

namespace GitUtils.Lib;

public class Repository
{
    private readonly LibGit2Sharp.Repository _repository;

    public Repository(string repoPath)
    {
        _repository = new LibGit2Sharp.Repository(repoPath);
    }

    public Commit GetCommit(string hash)
    {
        var commit = _repository.Lookup<LibGit2Sharp.Commit>(hash);
        return new Commit(commit);
    }

    internal CommitFilter GetFilter(Commit startCommit, Commit endCommit, CommitSortStrategies strategies)
    {
        return new CommitFilter
        {
            IncludeReachableFrom = endCommit.RawCommit,
            ExcludeReachableFrom = startCommit.Parents.Select(x=>x.RawCommit),
            SortBy = strategies
        };
    }

    public IEnumerable<Commit> GetLog(Commit startCommit, Commit endCommit)
    {
        var filter = GetFilter(startCommit,endCommit, CommitSortStrategies.Topological | CommitSortStrategies.Time);
        return _repository.Commits.QueryBy(filter).Reverse().Select(x=>new Commit(x));
    }

    public void OutputChanges(Commit startCommit, Commit endCommit, string path)
    {
        var previousCommit = startCommit.RawCommit;
        var nextCommit = endCommit.RawCommit;
        var contentPath = Path.Combine(path, nextCommit.Id.Sha);
        var changes = _repository.Diff.Compare<TreeChanges>(previousCommit.Tree, nextCommit.Tree);
        foreach (var change in changes)
        {
            switch (change.Status)
            {
                case ChangeKind.Added:
                case ChangeKind.Modified:
                case ChangeKind.Renamed:
                case ChangeKind.Copied:
                {
                    if (nextCommit.Tree[change.Path]?.Target is not Blob newBlob)
                    {
                        // Blobが取得できない場合はスキップ
                        continue;
                    }

                    var fileOutputPath = Path.Combine(contentPath, change.Path.Replace('/', Path.DirectorySeparatorChar));

                    var directory = Path.GetDirectoryName(fileOutputPath);
                    if (directory is null) continue;

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    Content.OutputFile(newBlob, fileOutputPath);

                    break;
                }
                case ChangeKind.Deleted:
                    break;
                case ChangeKind.Unmodified:
                    break;
                case ChangeKind.Ignored:
                    break;
                case ChangeKind.Untracked:
                    break;
                case ChangeKind.TypeChanged:
                    break;
                case ChangeKind.Unreadable:
                    break;
                case ChangeKind.Conflicted:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }


    // TODO: 後で名前変える
    public void OutputChangesAndOldChanges(Commit firstCommit, Commit commit, string? selectedOutputFolderPath)
    {
        var previousCommit = firstCommit.RawCommit;
        var nextCommit = commit.RawCommit;
        var contentPath = Path.Combine(selectedOutputFolderPath, nextCommit.Id.Sha);
        var changes = _repository.Diff.Compare<TreeChanges>(previousCommit.Tree, nextCommit.Tree);
        foreach (var change in changes)
        {
            switch (change.Status)
            {
                case ChangeKind.Added:
                case ChangeKind.Modified:
                case ChangeKind.Renamed:
                case ChangeKind.Copied:
                {
                    if (nextCommit.Tree[change.Path]?.Target is not Blob newBlob)
                    {
                        // Blobが取得できない場合はスキップ
                        continue;
                    }

                    var fileOutputPath = Path.Combine(contentPath, change.Path.Replace('/', Path.DirectorySeparatorChar));

                    var directory = Path.GetDirectoryName(fileOutputPath);
                    if (directory is null) continue;

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    Content.OutputFile(newBlob, fileOutputPath);

                    break;
                }
                case ChangeKind.Deleted:
                    break;
                case ChangeKind.Unmodified:
                    break;
                case ChangeKind.Ignored:
                    break;
                case ChangeKind.Untracked:
                    break;
                case ChangeKind.TypeChanged:
                    break;
                case ChangeKind.Unreadable:
                    break;
                case ChangeKind.Conflicted:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var contentPath2 = Path.Combine(selectedOutputFolderPath, previousCommit.Id.Sha);
            foreach (var change2 in changes)
            {
                // parentCommit側にファイルが存在する変更のみ抽出
                // つまり Deleted, Modified, Renamed, Copied
                // AddedはparentCommit側には存在しないので前の状態は無し
                if (change2.Status != ChangeKind.Deleted &&
                    change2.Status != ChangeKind.Modified &&
                    change2.Status != ChangeKind.Renamed &&
                    change2.Status != ChangeKind.Copied) continue;
                // parentCommit側のファイルを取得（oldPathを使用）
                var oldPath = change2.OldPath ?? change2.Path;
                // Renamedの場合はOldPathが元のパス
                // DeletedやModifiedでもOldPathが有効
                Blob oldBlob = previousCommit.Tree[oldPath]?.Target as Blob;

                if (oldBlob == null)
                {
                    // Blobが取得できない場合はスキップ
                    continue;
                }

                // 出力先ファイルパス
                string fileOutputPath = Path.Combine(contentPath2, oldPath.Replace('/', Path.DirectorySeparatorChar));
                string directory = Path.GetDirectoryName(fileOutputPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // ファイル出力
                Content.OutputFile(oldBlob, fileOutputPath);
            }
        }
    }
}