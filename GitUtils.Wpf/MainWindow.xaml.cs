using GitUtils.Wpf.Service;
using GitUtils.Wpf.ViewModel;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace GitUtils.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        this.DataContext = viewModel;
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

        if (this.DataContext is not MainWindowViewModel vm) return;

        vm.SelectedFolderPath = folderName;
        this.SelectedFolderPath.Text = folderName;
    }

    private void SelectedOutputFolder_OnClick(object sender, RoutedEventArgs e)
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

        if (this.DataContext is not MainWindowViewModel vm) return;

        vm.SelectedOutputFolderPath = folderName;
        this.SelectedOutputFolderPath.Text = folderName;
    }

    private void AfterCommitHashTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is not TextBox textBox) return;
        if (this.DataContext is not MainWindowViewModel vm) return;

        vm.AfterCommitHash = textBox.Text;
    }

    private void BeforeCommitHashTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is not TextBox textBox) return;
        if (this.DataContext is not MainWindowViewModel vm) return;

        vm.BeforeCommitHash = textBox.Text;
    }

    private void SearchButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is not MainWindowViewModel vm) return;

        vm.GetCommits();
    }

    private void OutputButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is not MainWindowViewModel vm) return;

        vm.OutputFiles();
    }

    private void OutputButton2_OnClick(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is not MainWindowViewModel vm) return;

        vm.OutputFiles2();
    }
}