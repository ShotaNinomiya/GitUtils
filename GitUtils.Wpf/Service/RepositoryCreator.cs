using GitUtils.Lib;
using GitUtils.Wpf.Service.Interface;

namespace GitUtils.Wpf.Service;

public class RepositoryCreator : IRepositoryCreator
{
    public Repository Create(string path)
    {
        return new Repository(path);
    }
}