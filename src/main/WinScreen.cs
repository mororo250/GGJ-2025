using Godot;
using System;

public partial class WinScreen : Control
{
	[Export] private AnimatedSprite2D _animatedSprite2D;
	[Export] private Node2D           _gameNode;

	public override void _Ready()
	{
		_animatedSprite2D.AnimationFinished += OnAnimationFinished;
	}

	public void OnVisibilityChanged()
	{
		if (Visible)
		{
			_gameNode.ProcessMode = Node.ProcessModeEnum.Disabled;
			_animatedSprite2D.Play(_animatedSprite2D.Animation);

			GetWindow().ContentScaleFactor = 6;
			AudioManager.Instance.PlayGlobalSFX(AudioType.GameOver, 0.6f);
		}
		else
		{
			_gameNode.ProcessMode = Node.ProcessModeEnum.Inherit;
			GetWindow().ContentScaleFactor = 4;
		}
	}

	private void OnAnimationFinished()
	{
		Visible = false;
		OnVisibilityChanged();
	}
}
