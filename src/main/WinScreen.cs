using Godot;
using System;
using CCJ2025.main;

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
			AudioManager.Instance.StopMusic();
			_animatedSprite2D.Play(_animatedSprite2D.Animation);

			AudioManager.Instance.PlayGlobalSFX(AudioType.Win, 0.6f);
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
		if (GameState.CurrentLevel > 2)
		{
			SceneManager.Instance.LoadMainMenu();
		}
	}
}
