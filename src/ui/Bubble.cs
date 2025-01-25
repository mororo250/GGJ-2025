using Godot;
using System;

public partial class Bubble : AnimatedSprite2D
{
	public void OnAnimationFinished()
	{
		if (Animation == "Bloom")
		{
			Animation = "Idle";
		}
		else if (Animation == "Pop")
		{
			Visible = false;
		}
	}
}
