using GitUtils.Wpf.ViewModel;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace GitUtils.Wpf.View;

/// <summary>
/// FolderSelectorControl.xaml の相互作用ロジック
/// </summary>
public partial class FolderSelectorControl : UserControl
{

    public FolderSelectorControl()
    {
        InitializeComponent();
        DataContext = this;
    }

    private void SelectFolderButton_Click(object sender, RoutedEventArgs e)
    {
        // Configure open folder dialog box
        var dialog = new OpenFolderDialog()
        {
            Multiselect = false,
            Title = "Select a folder"
        };

        // Show open folder dialog box
        var result = dialog.ShowDialog();
        var folderName = dialog.FolderName;

        // Process open folder dialog box results
        if (result != true || string.IsNullOrWhiteSpace(folderName)) return;

        if (this.DataContext is not FolderSelectorViewModel vm) return;
        
        vm.SelectedFolderPath = folderName;
        SelectedFolderPath.Text = folderName;
    }
}
