using Godot;

public partial class Level : Node2D
{
	[Export] private Node2D _playerPos;
	[Export] private uint   _victoryCondition;
	public           uint   VitoryCondition => _victoryCondition;
	public Vector2 PlayerPosition => _playerPos.GlobalPosition;
}
