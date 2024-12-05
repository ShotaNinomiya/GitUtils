using System.Windows.Controls;
using GitUtils.Wpf.ViewModel;

namespace GitUtils.Wpf.View;

/// <summary>
/// CommitHashControl.xaml の相互作用ロジック
/// </summary>
public partial class CommitHashControl : UserControl
{
    public CommitHashControl()
    {
        InitializeComponent();
        DataContext = this;
    }
}
