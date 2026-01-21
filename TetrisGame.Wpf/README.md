# Tetris Game

## プロジェクト概要

.NET C# を使用した古典的なテトリスゲームの実装です。

## 技術スタック

- **言語**: C#
- **フレームワーク**: .NET 9.0 WPF
- **UIアーキテクチャ**: MVVM パターン
- **表示方式**: アスキー文字による色付き表示（TextBlock + 等幅フォント）

## ディレクトリ構造

```
TetrisGame.Wpf/
 Models/         - ゲームロジック層
 ViewModels/     - MVVM の ViewModel 層
 Views/          - XAML による View 層
 App.xaml        - アプリケーションエントリポイント
 README.md       - このファイル
```

## ビルド実行手順

### ビルド

```powershell
dotnet build
```

### 実行

```powershell
dotnet run
```

## 開発計画

1. **Step001**: プロジェクト作成と基本構成のセットアップ 
2. **Step002**: Model 層の実装（Tetromino, GameField）
3. **Step003**: Model 層の実装（GameEngine）
4. **Step004**: ViewModel 層の実装
5. **Step005**: View 層の実装（UI構築）
6. **Step006**: キーボード入力と自動落下の実装
7. **Step007**: 統合テストとデバッグ

## ライセンス

 2025 マルチコンピューティング株式会社
