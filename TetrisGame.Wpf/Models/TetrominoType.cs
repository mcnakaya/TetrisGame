namespace TetrisGame.Wpf.Models
{
    /// <summary>
    /// テトリミノの種類を表す列挙型
    /// </summary>
    public enum TetrominoType
    {
        /// <summary>空のセル</summary>
        None = 0,

        /// <summary>I型テトリミノ（縦棒）</summary>
        I = 1,

        /// <summary>O型テトリミノ（正方形）</summary>
        O = 2,

        /// <summary>T型テトリミノ（T字）</summary>
        T = 3,

        /// <summary>S型テトリミノ（S字）</summary>
        S = 4,

        /// <summary>Z型テトリミノ（Z字）</summary>
        Z = 5,

        /// <summary>J型テトリミノ（逆L字）</summary>
        J = 6,

        /// <summary>L型テトリミノ（L字）</summary>
        L = 7
    }
}
