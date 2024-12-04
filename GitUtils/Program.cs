// See https://aka.ms/new-console-template for more information

using GitUtils.Lib;
using LibGit2Sharp;

const string repoPath = @"C:\sdd-projects\NEX";
using var repo = new Repository(repoPath);
var startCommit = repo.Lookup<Commit>("ff72f57bd2e3101c465a0003bc86b3dbc82e4c20");
var endCommit = repo.Lookup<Commit>("d06277cb2b281c3e0e58f04376a2b40c795aae35");

var filter = new CommitFilter
{
    IncludeReachableFrom = endCommit,
    ExcludeReachableFrom = startCommit.Parents, // 開始コミットの親を除外
    SortBy = CommitSortStrategies.Topological | CommitSortStrategies.Time
};

var commits = repo.Commits.QueryBy(filter).Reverse(); // 開始コミットから順にするためにReverseを使用

Commit previousCommit = startCommit;
foreach (var commit in commits)
{
    var changes = repo.Diff.Compare<TreeChanges>(previousCommit.Tree, commit.Tree);

    Console.WriteLine($"コミットID: {commit.Id}");
    Console.WriteLine($"コミットメッセージ: {commit.MessageShort}");
    Console.WriteLine($"作成者: {commit.Author.Name} <{commit.Author.Email}>");
    Console.WriteLine($"日時: {commit.Author.When}");
    Console.WriteLine("--------------------------------------");

    foreach (var change in changes)
    {
        Console.WriteLine($"変更タイプ: {change.Status}");
        Console.WriteLine($"ファイルパス: {change.OldPath} -> {change.Path}");

        // 元のファイルの内容を取得
        var oldBlob = previousCommit.Tree[change.OldPath]?.Target as Blob;
        var oldContent = oldBlob != null ? Content.Read(oldBlob) : null;

        // 変更後のファイルの内容を取得
        var newBlob = commit.Tree[change.Path]?.Target as Blob;
        var newContent = newBlob != null ? Content.Read(newBlob) : null;

        // 内容を出力
        Console.WriteLine("---- 元のファイル ----");
        Console.WriteLine(oldContent);
        Console.WriteLine("---- 変更後のファイル ----");
        Console.WriteLine(newContent);
        Console.WriteLine("======================================");
    }

    previousCommit = commit;
}
