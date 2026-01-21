using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using TetrisGame.Wpf.Models;

namespace TetrisGame.Wpf.ViewModels;

/// <summary>
/// ゲーム全体を管理する ViewModel
/// </summary>
public class GameViewModel : INotifyPropertyChanged
{
    private readonly GameEngine _gameEngine;
    private int _currentScore;
    private int _currentLevel;
    private bool _isGameOver;

    /// <summary>
    /// ゲームフィールドの全セル（200セル = 20行 × 10列）
    /// </summary>
    public ObservableCollection<CellViewModel> Cells { get; }

    /// <summary>
    /// 現在のスコア
    /// </summary>
    public int CurrentScore
    {
        get => _currentScore;
        set
        {
            if (_currentScore != value)
            {
                _currentScore = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// 現在のレベル
    /// </summary>
    public int CurrentLevel
    {
        get => _currentLevel;
        set
        {
            if (_currentLevel != value)
            {
                _currentLevel = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// ゲームオーバー状態
    /// </summary>
    public bool IsGameOver
    {
        get => _isGameOver;
        set
        {
            if (_isGameOver != value)
            {
                _isGameOver = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// ゲーム開始コマンド
    /// </summary>
    public ICommand StartGameCommand { get; }

    /// <summary>
    /// ゲーム再スタートコマンド
    /// </summary>
    public ICommand RestartGameCommand { get; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public GameViewModel()
    {
        _gameEngine = new GameEngine();
        Cells = new ObservableCollection<CellViewModel>();

        // 200セル（20行×10列）を初期化
        for (int i = 0; i < GameField.Height * GameField.Width; i++)
        {
            Cells.Add(new CellViewModel());
        }

        // コマンドの初期化
        StartGameCommand = new RelayCommand(_ => StartGame());
        RestartGameCommand = new RelayCommand(_ => RestartGame());
    }

    /// <summary>
    /// ゲームを開始
    /// </summary>
    private void StartGame()
    {
        _gameEngine.NewGame();
        UpdateField();
        UpdateGameState();
    }

    /// <summary>
    /// ゲームを再スタート
    /// </summary>
    private void RestartGame()
    {
        StartGame();
    }

    /// <summary>
    /// ゲームフィールドを更新（Model の状態を ViewModel に反映）
    /// </summary>
    public void UpdateField()
    {
        // フィールドの全セルをクリア
        for (int y = 0; y < GameField.Height; y++)
        {
            for (int x = 0; x < GameField.Width; x++)
            {
                int index = y * GameField.Width + x;
                int cellValue = _gameEngine.GameField.GetCell(x, y);

                if (cellValue == 0)
                {
                    // 空のセル
                    Cells[index].DisplayText = "□";
                    Cells[index].CellColor = Brushes.White;
                }
                else
                {
                    // ブロックが存在するセル
                    Cells[index].DisplayText = "■";
                    Cells[index].CellColor = GetColorBrush((TetrominoType)cellValue);
                }
            }
        }

        // 現在のテトリミノを描画
        if (_gameEngine.CurrentTetromino != null)
        {
            var tetromino = _gameEngine.CurrentTetromino;
            for (int ty = 0; ty < 4; ty++)
            {
                for (int tx = 0; tx < 4; tx++)
                {
                    if (tetromino.Shape[ty, tx])
                    {
                        int fieldX = tetromino.X + tx;
                        int fieldY = tetromino.Y + ty;

                        // フィールド範囲内かチェック
                        if (fieldX >= 0 && fieldX < GameField.Width && fieldY >= 0 && fieldY < GameField.Height)
                        {
                            int index = fieldY * GameField.Width + fieldX;
                            Cells[index].DisplayText = "■";
                            Cells[index].CellColor = GetColorBrush(tetromino.Type);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// ゲーム状態を更新
    /// </summary>
    private void UpdateGameState()
    {
        CurrentScore = _gameEngine.Score;
        CurrentLevel = _gameEngine.Level;
        IsGameOver = _gameEngine.State == GameEngine.GameState.GameOver;
    }

    /// <summary>
    /// テトリミノタイプに応じた色を取得
    /// </summary>
    private Brush GetColorBrush(TetrominoType type)
    {
        return type switch
        {
            TetrominoType.I => Brushes.Cyan,
            TetrominoType.O => Brushes.Yellow,
            TetrominoType.T => new SolidColorBrush(Color.FromRgb(128, 0, 128)), // Purple
            TetrominoType.S => Brushes.Green,
            TetrominoType.Z => Brushes.Red,
            TetrominoType.J => Brushes.Blue,
            TetrominoType.L => Brushes.Orange,
            _ => Brushes.White
        };
    }

    /// <summary>
    /// 左に移動
    /// </summary>
    public void MoveLeft()
    {
        if (_gameEngine.MoveLeft())
        {
            UpdateField();
        }
    }

    /// <summary>
    /// 右に移動
    /// </summary>
    public void MoveRight()
    {
        if (_gameEngine.MoveRight())
        {
            UpdateField();
        }
    }

    /// <summary>
    /// 下に移動
    /// </summary>
    public void MoveDown()
    {
        if (_gameEngine.MoveDown())
        {
            UpdateField();
            UpdateGameState();
        }
    }

    /// <summary>
    /// 回転
    /// </summary>
    public void Rotate()
    {
        if (_gameEngine.Rotate())
        {
            UpdateField();
        }
    }

    /// <summary>
    /// ハードドロップ
    /// </summary>
    public void HardDrop()
    {
        _gameEngine.HardDrop();
        UpdateField();
        UpdateGameState();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
