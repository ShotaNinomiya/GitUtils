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
        var changes = _repository.Diff.Compare<TreeChanges>(previousCommit.Tree, nextCommit.Tree);
        foreach (var change in changes)
        {
            // 変更後にファイルが存在する場合のみ出力
            if (change.Status == ChangeKind.Added ||
                change.Status == ChangeKind.Modified ||
                change.Status == ChangeKind.Renamed ||
                change.Status == ChangeKind.Copied)
            {
                // 対象ファイルのBlobを取得
                var newBlob = nextCommit.Tree[change.Path]?.Target as Blob;
                if (newBlob == null)
                {
                    // Blobが取得できない場合はスキップ
                    continue;
                }

                // 出力するファイルのパスを作成
                string fileOutputPath = Path.Combine(path, change.Path.Replace('/', Path.DirectorySeparatorChar));

                // ディレクトリが存在しない場合は作成
                string directory = Path.GetDirectoryName(fileOutputPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // ファイルを出力
                using (var contentStream = newBlob.GetContentStream())
                using (var fileStream = File.Create(fileOutputPath))
                {
                    contentStream.CopyTo(fileStream);
                }

                Console.WriteLine($"出力: {fileOutputPath} (変更タイプ: {change.Status})");
            }
            else if (change.Status == ChangeKind.Deleted)
            {
                // 削除されたファイルは新規コンテンツがないため今回はスキップ
                // 必要に応じて、削除されたファイル一覧を出力するなどの対応を行ってください
            }
        }
    }
}