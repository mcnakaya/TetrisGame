namespace TetrisGame.Wpf.Models
{
    /// <summary>
    /// テトリスゲームのロジック全体を統括するクラス
    /// </summary>
    public class GameEngine
    {
        /// <summary>ゲーム状態を表す列挙型</summary>
        public enum GameState
        {
            Playing,
            Paused,
            GameOver
        }

        private readonly Random _random = new Random();

        /// <summary>現在のテトリミノ</summary>
        public Tetromino? CurrentTetromino { get; private set; }

        /// <summary>ゲームフィールド</summary>
        public GameField GameField { get; private set; }

        /// <summary>現在のスコア</summary>
        public int Score { get; private set; }

        /// <summary>現在のレベル</summary>
        public int Level { get; private set; }

        /// <summary>累計消去ライン数</summary>
        public int TotalLinesCleared { get; private set; }

        /// <summary>現在のゲーム状態</summary>
        public GameState State { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameEngine()
        {
            GameField = new GameField();
            State = GameState.Playing;
            NewGame();
        }

        /// <summary>
        /// 新しいゲームを開始
        /// </summary>
        public void NewGame()
        {
            GameField.Clear();
            Score = 0;
            Level = 1;
            TotalLinesCleared = 0;
            State = GameState.Playing;
            SpawnTetromino();
        }

        /// <summary>
        /// 新しいテトリミノを生成
        /// </summary>
        private void SpawnTetromino()
        {
            // ランダムにテトリミノの種類を選択（None以外の1-7）
            int randomType = _random.Next(1, 8);
            TetrominoType type = (TetrominoType)randomType;

            CurrentTetromino = new Tetromino(type);

            // 初期位置に配置できるかチェック
            if (!GameField.CanPlaceTetromino(CurrentTetromino, 0, 0))
            {
                // ゲームオーバー
                State = GameState.GameOver;
            }
        }

        /// <summary>
        /// テトリミノを左に移動
        /// </summary>
        /// <returns>移動できたらtrue</returns>
        public bool MoveLeft()
        {
            if (State != GameState.Playing || CurrentTetromino == null)
                return false;

            if (GameField.CanPlaceTetromino(CurrentTetromino, -1, 0))
            {
                CurrentTetromino.X--;
                return true;
            }

            return false;
        }

        /// <summary>
        /// テトリミノを右に移動
        /// </summary>
        /// <returns>移動できたらtrue</returns>
        public bool MoveRight()
        {
            if (State != GameState.Playing || CurrentTetromino == null)
                return false;

            if (GameField.CanPlaceTetromino(CurrentTetromino, 1, 0))
            {
                CurrentTetromino.X++;
                return true;
            }

            return false;
        }

        /// <summary>
        /// テトリミノを下に移動
        /// </summary>
        /// <returns>移動できたらtrue、固定されたらfalse</returns>
        public bool MoveDown()
        {
            if (State != GameState.Playing || CurrentTetromino == null)
                return false;

            if (GameField.CanPlaceTetromino(CurrentTetromino, 0, 1))
            {
                CurrentTetromino.Y++;
                return true;
            }
            else
            {
                // 固定処理
                LockTetromino();
                return false;
            }
        }

        /// <summary>
        /// テトリミノを時計回りに回転
        /// </summary>
        /// <returns>回転できたらtrue</returns>
        public bool Rotate()
        {
            if (State != GameState.Playing || CurrentTetromino == null)
                return false;

            // 回転を試みる
            CurrentTetromino.RotateClockwise();

            // 回転後に配置可能かチェック
            if (GameField.CanPlaceTetromino(CurrentTetromino, 0, 0))
            {
                return true;
            }
            else
            {
                // 配置不可能なら3回回転して元に戻す
                CurrentTetromino.RotateClockwise();
                CurrentTetromino.RotateClockwise();
                CurrentTetromino.RotateClockwise();
                return false;
            }
        }

        /// <summary>
        /// テトリミノをフィールドに固定し、次のテトリミノを生成
        /// </summary>
        private void LockTetromino()
        {
            if (CurrentTetromino == null)
                return;

            // テトリミノをフィールドに固定
            GameField.PlaceTetromino(CurrentTetromino);

            // ライン消去チェック
            int linesCleared = CheckAndClearLines();

            // スコア更新
            if (linesCleared > 0)
            {
                UpdateScore(linesCleared);
            }

            // 次のテトリミノを生成
            SpawnTetromino();
        }

        /// <summary>
        /// 完成したラインをチェックして消去
        /// </summary>
        /// <returns>消去したライン数</returns>
        private int CheckAndClearLines()
        {
            int linesCleared = 0;

            // 下から順に走査
            for (int y = GameField.Height - 1; y >= 0; y--)
            {
                bool isLineFull = true;

                // 行が完全に埋まっているかチェック
                for (int x = 0; x < GameField.Width; x++)
                {
                    if (GameField.GetCell(x, y) == 0)
                    {
                        isLineFull = false;
                        break;
                    }
                }

                if (isLineFull)
                {
                    // ラインを削除（上の行を下にシフト）
                    RemoveLine(y);
                    linesCleared++;
                    y++; // 同じ行を再度チェック（シフト後）
                }
            }

            return linesCleared;
        }

        /// <summary>
        /// 指定行を削除し、上の行を下にシフト
        /// </summary>
        private void RemoveLine(int lineY)
        {
            GameField.RemoveLine(lineY);
        }

        /// <summary>
        /// スコアとレベルを更新
        /// </summary>
        private void UpdateScore(int linesCleared)
        {
            // スコアリングルール
            int points = linesCleared switch
            {
                1 => 100,
                2 => 300,
                3 => 500,
                4 => 800,
                _ => 0
            };

            Score += points * Level; // レベルに応じてスコアを倍増

            // 累計消去ライン数を更新
            TotalLinesCleared += linesCleared;

            // レベルアップ（10ライン消去ごと）
            Level = (TotalLinesCleared / 10) + 1;
        }

        /// <summary>
        /// テトリミノを底まで即座に落下（ハードドロップ）
        /// </summary>
        public void HardDrop()
        {
            if (State != GameState.Playing || CurrentTetromino == null)
                return;

            while (MoveDown())
            {
                // 下に移動できなくなるまで繰り返す
            }
        }
    }
}
