using Godot;

public partial class Level : Node2D
{
	[Export] private Node2D _playerPos;
	public Vector2 PlayerPosition => _playerPos.GlobalPosition;
}
