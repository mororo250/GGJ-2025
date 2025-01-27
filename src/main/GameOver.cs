using Godot;
using System;

public partial class GameOver : Control
{
	[Export] private AnimatedSprite2D _animatedSprite2D;
	[Export] private Node2D           _gameNode;
	[Signal] public delegate void RetryButtonPressedEventHandler();
	public override void _Ready()
	{
		_animatedSprite2D.Play(_animatedSprite2D.Animation);
	}
	
	public void SetVisibility(bool visible)
	{
		Visible = visible;
		OnVisibilityChanged();
	}

	private void OnVisibilityChanged()
	{
		if (Visible)
		{
			_gameNode.ProcessMode = Node.ProcessModeEnum.Disabled;
			AudioManager.Instance.PlayGlobalSFX(AudioType.GameOver, 0.6f);
		}
		else
		{
			_gameNode.ProcessMode                        = Node.ProcessModeEnum.Inherit;
		}
	}
	
	private void OnNotButtonPressed()
	{
		AudioManager.Instance.PlayGlobalSFX(AudioType.Click);
		SceneManager.Instance.LoadMainMenu();
	}
	
	private void OnRetryButtonPressed()
	{
		AudioManager.Instance.PlayGlobalSFX(AudioType.Click);
		SetVisibility(false);
		EmitSignal(SignalName.RetryButtonPressed);
	}
}
