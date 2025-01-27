using Godot;
using System;

public partial class WinScreen : Control
{
	[Export] private AnimatedSprite2D _animatedSprite2D;
	[Export] private Node2D           _gameNode;

	public override void _Ready()
	{
		_animatedSprite2D.AnimationFinished += OnAnimationFinished;
		_animatedSprite2D.Play(_animatedSprite2D.Animation);
	}

	public void OnVisibilityChanged()
	{
		if (Visible)
		{
			_gameNode.ProcessMode = Node.ProcessModeEnum.Disabled;
			_animatedSprite2D.Play(_animatedSprite2D.Animation);

			AudioManager.Instance.PlayGlobalSFX(AudioType.GameOver, 0.6f);
		}
		else
		{
			_gameNode.ProcessMode          = Node.ProcessModeEnum.Inherit;
		}
	}

	private void OnAnimationFinished()
	{
		Visible = false;
		OnVisibilityChanged();
	}
}
