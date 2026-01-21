using System.Windows.Media;

namespace TetrisGame.Wpf.Models
{
    /// <summary>
    /// テトリミノ（テトリスの各ブロック）を表すクラス
    /// </summary>
    public class Tetromino
    {
        /// <summary>テトリミノの種類</summary>
        public TetrominoType Type { get; }

        /// <summary>テトリミノの形状（4×4配列、true=ブロック有、false=空）</summary>
        public bool[,] Shape { get; private set; }

        /// <summary>フィールド上のX座標</summary>
        public int X { get; set; }

        /// <summary>フィールド上のY座標</summary>
        public int Y { get; set; }

        /// <summary>テトリミノの色</summary>
        public Color Color { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">テトリミノの種類</param>
        public Tetromino(TetrominoType type)
        {
            Type = type;
            Shape = GetInitialShape(type);
            Color = GetColor(type);
            X = 3; // 初期位置（フィールド中央付近）
            Y = 0;
        }

        /// <summary>
        /// テトリミノを時計回りに90度回転
        /// </summary>
        public void RotateClockwise()
        {
            // O型は回転しても同じ形なのでスキップ
            if (Type == TetrominoType.O)
                return;

            int size = 4;
            bool[,] rotated = new bool[size, size];

            // 転置行列を作成してから列を逆順にする（時計回り90度回転）
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    rotated[x, size - 1 - y] = Shape[y, x];
                }
            }

            Shape = rotated;
        }

        /// <summary>
        /// テトリミノの種類に応じた初期形状を取得
        /// </summary>
        private static bool[,] GetInitialShape(TetrominoType type)
        {
            return type switch
            {
                TetrominoType.I => new bool[,]
                {
                    { false, false, false, false },
                    { true,  true,  true,  true  },
                    { false, false, false, false },
                    { false, false, false, false }
                },
                TetrominoType.O => new bool[,]
                {
                    { false, false, false, false },
                    { false, true,  true,  false },
                    { false, true,  true,  false },
                    { false, false, false, false }
                },
                TetrominoType.T => new bool[,]
                {
                    { false, false, false, false },
                    { false, true,  true,  true  },
                    { false, false, true,  false },
                    { false, false, false, false }
                },
                TetrominoType.S => new bool[,]
                {
                    { false, false, false, false },
                    { false, false, true,  true  },
                    { false, true,  true,  false },
                    { false, false, false, false }
                },
                TetrominoType.Z => new bool[,]
                {
                    { false, false, false, false },
                    { false, true,  true,  false },
                    { false, false, true,  true  },
                    { false, false, false, false }
                },
                TetrominoType.J => new bool[,]
                {
                    { false, false, false, false },
                    { false, true,  true,  true  },
                    { false, false, false, true  },
                    { false, false, false, false }
                },
                TetrominoType.L => new bool[,]
                {
                    { false, false, false, false },
                    { false, true,  true,  true  },
                    { false, true,  false, false },
                    { false, false, false, false }
                },
                _ => new bool[4, 4]
            };
        }

        /// <summary>
        /// テトリミノの種類に応じた色を取得
        /// </summary>
        private static Color GetColor(TetrominoType type)
        {
            return type switch
            {
                TetrominoType.I => Colors.Cyan,
                TetrominoType.O => Colors.Yellow,
                TetrominoType.T => Colors.Purple,
                TetrominoType.S => Colors.Green,
                TetrominoType.Z => Colors.Red,
                TetrominoType.J => Colors.Blue,
                TetrominoType.L => Colors.Orange,
                _ => Colors.Gray
            };
        }
    }
}
