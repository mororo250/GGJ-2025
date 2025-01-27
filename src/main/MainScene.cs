using agents;
using Godot;

public partial class MainScene : Node
{
	[Export] public PackedScene Level1;
	[Export] public PackedScene Level2;

	[Export] private Node2D _map;
	[Export] private Player _player;
	private          Level  _currentLevel;
	
	[Export] private GameOver _gameOverScreen;
	[Export] private WinScreen  _victoryScreen;
	
	public override void _EnterTree()
	{
		_currentLevel          = Level1.Instantiate() as Level;
		_player.GlobalPosition = _currentLevel.PlayerPosition;
		_map.AddChild(_currentLevel);
	}
	
	public override void _Process(double delta)
	{
		if (_player.TrashCount == _currentLevel.VitoryCondition)
		{
			_victoryScreen.Visible = true;
			_victoryScreen.OnVisibilityChanged();
			_map.RemoveChild(_currentLevel);
			_currentLevel          = Level2.Instantiate() as Level;
			_player.GlobalPosition = _currentLevel.PlayerPosition;
			_player.Init();

			// todo: initialize player again
			_map.AddChild(_currentLevel);
		}
	}


	private void OnGameOver() => _gameOverScreen.SetVisibility(true);

	private void OnRetry()
	{
		_player.GlobalPosition = _currentLevel.PlayerPosition;
		_player.Init();
	}
}
