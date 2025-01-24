using Godot;

namespace trash;

public partial class Trash : RigidBody2D
{
	[Export] private AnimatedSprite2D _animatedSprite2D;
	
	public override void _Ready()
	{
		_animatedSprite2D.Play("default");
	}
}
