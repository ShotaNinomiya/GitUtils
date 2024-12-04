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
    public FolderSelectorViewModel ViewModel { get; set; }

    public FolderSelectorControl()
    {
        InitializeComponent();
        ViewModel = new FolderSelectorViewModel();
        DataContext = ViewModel;
    }

    private void SelectFolderButton_Click(object sender, RoutedEventArgs e)
    {
        // Configure open folder dialog box
        OpenFolderDialog dialog = new()
        {
            Multiselect = false,
            Title = "Select a folder"
        };

        // Show open folder dialog box
        bool? result = dialog.ShowDialog();

        // Process open folder dialog box results
        if (result == true && !string.IsNullOrWhiteSpace(dialog.FolderName))
        {
            SelectedFolderPath.Text = dialog.FolderName;
        }
    }
}
