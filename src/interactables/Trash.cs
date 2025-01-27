using System;
using Godot;
using Godot.Collections;

namespace trash;

public partial class Trash : RigidBody2D
{
	[Export] private AnimatedSprite2D _animatedSprite2D;
	
	
	public override void _Ready()
	{
		SpriteFrames spriteFrames = _animatedSprite2D.GetSpriteFrames();
		string[] animations = spriteFrames.GetAnimationNames();
		int randomAnimation = new Random().Next(0, animations.Length - 1);
		_animatedSprite2D.Play(animations[randomAnimation]);
	}
	
	public void SetFreeze(bool freeze)
	{
		Freeze = freeze;
		if (!freeze)
			_animatedSprite2D.Stop();
		else
			_animatedSprite2D.Play(_animatedSprite2D.Animation);
		
	}
}
