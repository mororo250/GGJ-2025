using Godot;

namespace trash;

public partial class Trash : RigidBody2D
{
	[Export] private AnimatedSprite2D _animatedSprite2D;
	[Export] private string           _animation;
	
	public string Animation
	{
		get => _animation;
		set
		{
			_animation = value;
			if (!Freeze)
				_animatedSprite2D.Play(_animation);
		}
	}
	
	public override void _Ready()
	{
		_animatedSprite2D.Play(_animatedSprite2D.Animation);
	}
	
	public void SetFreeze(bool freeze)
	{
		Freeze = freeze;
		if (!freeze)
			_animatedSprite2D.Stop();
		else
			_animatedSprite2D.Play(_animation);
		
	}
}
