namespace GitUtils.Wpf.ViewModel;

public class FolderSelectorViewModel : BaseViewModel
{
    private string? _selectedFolderPath;
    public string? SelectedFolderPath
    {
        get => _selectedFolderPath;
        set
        {
            _selectedFolderPath = value;
            OnPropertyChanged(nameof(SelectedFolderPath));
        }
    }
}