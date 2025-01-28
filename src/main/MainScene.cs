using agents;
using CCJ2025.main;
using Godot;

public partial class MainScene : Node
{
	[Export] public PackedScene Level1;
	[Export] public PackedScene Level2;

	[Export] private Node2D _map;
	[Export] private Player _player;
	[Export] private Camera2D _camera;
	private          Level  _currentLevel;
	
	[Export] private GameOver _gameOverScreen;
	[Export] private WinScreen  _victoryScreen;
	
	public override void _EnterTree()
	{
		GameState.CurrentLevel = 1;
		InitLevel(Level1.Instantiate() as Level);
		_player.Init();
	}
	
	public override void _Process(double delta)
	{
		if (_player.TrashCount == GameState.CurrentLevelTrashCount)
		{
			FinishLevel();
			GameState.CurrentLevel++;

			if (GameState.CurrentLevel > 2)
			{
				return;
			}

			InitLevel(Level2.Instantiate() as Level);
		}
	}

	private void InitLevel(Level level)
	{
		_currentLevel          = level;
		_player.GlobalPosition = _currentLevel.PlayerPosition;
		_camera.GlobalPosition = _player.GlobalPosition;
		_player.Init();
		_map.AddChild(_currentLevel);
		AudioManager.Instance.PlayMusic(AudioType.MusicMainTheme, 0.45f);
	}
	
	private void FinishLevel()
	{
		_victoryScreen.Visible = true;
		_victoryScreen.OnVisibilityChanged();
		_map.RemoveChild(_currentLevel);
		_currentLevel.QueueFree();
	}


	private void OnGameOver() => _gameOverScreen.SetVisibility(true);

	private void OnRetry()
	{
		InitLevel(Level1.Instantiate() as Level);
	}
}
