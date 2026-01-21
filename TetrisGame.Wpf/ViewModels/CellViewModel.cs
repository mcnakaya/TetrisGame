using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace TetrisGame.Wpf.ViewModels;

/// <summary>
/// ゲームフィールドの各セルを表示するための ViewModel
/// </summary>
public class CellViewModel : INotifyPropertyChanged
{
    private string _displayText = "□";
    private Brush _cellColor = Brushes.White;

    /// <summary>
    /// セルの表示文字（■ または □）
    /// </summary>
    public string DisplayText
    {
        get => _displayText;
        set
        {
            if (_displayText != value)
            {
                _displayText = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// セルの色（Brush 型）
    /// </summary>
    public Brush CellColor
    {
        get => _cellColor;
        set
        {
            if (_cellColor != value)
            {
                _cellColor = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
