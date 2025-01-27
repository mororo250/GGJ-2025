using Godot;

public partial class MainMenu : Control
{
	public override void _Ready()
	{
		AudioManager.Instance.PlayMusic(AudioType.MusicMenu, 0.45f);
	}

	private void OnQUitButtonPressed()
	{
		GetTree().Quit();
	}
	
	private void OnNewGameButtonPressed()
	{
		AudioManager.Instance.PlayGlobalSFX(AudioType.Click);
		SceneManager.Instance.LoadGame();
	}
}
