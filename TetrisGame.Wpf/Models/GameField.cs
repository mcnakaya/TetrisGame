namespace TetrisGame.Wpf.Models
{
    /// <summary>
    /// テトリスのゲームフィールドを管理するクラス
    /// </summary>
    public class GameField
    {
        /// <summary>フィールドの幅（列数）</summary>
        public const int Width = 10;

        /// <summary>フィールドの高さ（行数）</summary>
        public const int Height = 20;

        /// <summary>フィールドの状態（0=空、1-7=テトリミノの種類）</summary>
        private readonly int[,] _field;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameField()
        {
            _field = new int[Height, Width];
            Clear();
        }

        /// <summary>
        /// フィールドをクリア（全セルを0にする）
        /// </summary>
        public void Clear()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    _field[y, x] = 0;
                }
            }
        }

        /// <summary>
        /// 指定位置のセルの値を取得
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <returns>セルの値（0=空、1-7=テトリミノの種類）</returns>
        public int GetCell(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return -1; // 範囲外

            return _field[y, x];
        }

        /// <summary>
        /// テトリミノを指定位置に配置可能かチェック
        /// </summary>
        /// <param name="tetromino">配置するテトリミノ</param>
        /// <param name="offsetX">X座標のオフセット</param>
        /// <param name="offsetY">Y座標のオフセット</param>
        /// <returns>配置可能ならtrue</returns>
        public bool CanPlaceTetromino(Tetromino tetromino, int offsetX, int offsetY)
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (!tetromino.Shape[y, x])
                        continue; // このセルにブロックがない

                    int fieldX = tetromino.X + x + offsetX;
                    int fieldY = tetromino.Y + y + offsetY;

                    // 境界チェック
                    if (fieldX < 0 || fieldX >= Width || fieldY < 0 || fieldY >= Height)
                        return false;

                    // 衝突チェック（既存ブロックとの重なり）
                    if (_field[fieldY, fieldX] != 0)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// テトリミノをフィールドに固定
        /// </summary>
        /// <param name="tetromino">固定するテトリミノ</param>
        public void PlaceTetromino(Tetromino tetromino)
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (!tetromino.Shape[y, x])
                        continue;

                    int fieldX = tetromino.X + x;
                    int fieldY = tetromino.Y + y;

                    if (fieldX >= 0 && fieldX < Width && fieldY >= 0 && fieldY < Height)
                    {
                        _field[fieldY, fieldX] = (int)tetromino.Type;
                    }
                }
            }
        }

        /// <summary>
        /// フィールドの状態を2次元配列で取得（デバッグ用）
        /// </summary>
        /// <returns>フィールドのコピー</returns>
        public int[,] GetFieldCopy()
        {
            int[,] copy = new int[Height, Width];
            Array.Copy(_field, copy, _field.Length);
            return copy;
        }
    }
}
