using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using GitUtils.Wpf.Service;
using GitUtils.Wpf.ViewModel;
using GitUtils.Wpf.Service.Interface;

namespace GitUtils.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // ViewModelの登録
        services.AddSingleton<MainWindowViewModel>();

        // Viewの登録
        services.AddSingleton<MainWindow>();

        // 他のサービス
        services.AddTransient<ISearchCommit, CommitSearcher>();
        services.AddTransient<IRepositoryCreator, RepositoryCreator>();
    }
}