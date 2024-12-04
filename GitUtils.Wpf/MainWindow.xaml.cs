using System.Collections.ObjectModel;
using GitUtils.Wpf.ViewModel;
using System.Windows;

namespace GitUtils.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindowViewModel ViewModel { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        ViewModel = new MainWindowViewModel
        {
            CommitHash1ViewModel = new CommitHashViewModel(),
            CommitHash2ViewModel = new CommitHashViewModel(),
            FolderSelectorViewModel = new FolderSelectorViewModel(),
            Items = new ObservableCollection<CommitItemViewModel>()
            {
                new ()
                {
                    Author = "ShotaNinomiya",
                    CommitHash = "123abc",
                    Date = "2024/12/4 22:22",
                    IsChecked = true,
                    Message = "fix: 色々な修正をした。"
                }
            }
        };
        DataContext = ViewModel;
    }
}