using GitUtils.Wpf.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace GitUtils.Wpf.View;

/// <summary>
/// CommitItemControl.xaml の相互作用ロジック
/// </summary>
public partial class CommitItemControl : UserControl
{
    public CommitItemControl()
    {
        InitializeComponent();
        DataContext = this;
    }
}
