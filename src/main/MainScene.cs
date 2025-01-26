using agents;
using Godot;

public partial class MainScene : Node
{
	[Export] public PackedScene Level1;
	[Export] public PackedScene Level2;

	[Export] private Node2D _map;
	[Export] private Player _player;
	private          Level  _currentLevel;
	
	public override void _EnterTree()
	{
		GetWindow().ContentScaleFactor = 4;

		_currentLevel = Level1.Instantiate() as Level;
		_map.AddChild(_currentLevel);
	}
	
	public override void _Process(double delta)
	{
		if (_player.TrashCount > _currentLevel.VitoryCondition)
		{
			_map.RemoveChild(_currentLevel);
			_currentLevel = Level2.Instantiate() as Level;
			_map.AddChild(_currentLevel);
		}
	}
	
}
