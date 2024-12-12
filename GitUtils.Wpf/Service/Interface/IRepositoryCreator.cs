using GitUtils.Lib;

namespace GitUtils.Wpf.Service.Interface;

public interface IRepositoryCreator
{
    Repository Create(string path);
}