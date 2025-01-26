using Godot;
using System;

public partial class PauseMenu : Control
{
	[Signal] public delegate void ResumeButtonPressedEventHandler();
	[Signal] public delegate void PauseMenuOpenEventHandler();

	private void OnResumeButtonPressed()
	{
		AudioManager.Instance.PlayGlobalSFX(AudioType.Click);
		Engine.TimeScale = 1;
		EmitSignal(SignalName.ResumeButtonPressed);
		Hide();
	}
	
	private void OnMainMenuButtonPressed()
	{
		AudioManager.Instance.PlayGlobalSFX(AudioType.Click);
		SceneManager.Instance.LoadMainMenu();
	}
	
	private void OnPauseMenuOpen()
	{
		Engine.TimeScale = 0;
		EmitSignal(SignalName.PauseMenuOpen);
		Show();
	}
	
	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent)
		{
			if (keyEvent.IsPressed())
			{
				switch (keyEvent.Keycode)
				{
					case Key.Escape:
						if (Visible)
							OnResumeButtonPressed();
						else
							OnPauseMenuOpen();
						break;
				}
			}
		}
	}
}
