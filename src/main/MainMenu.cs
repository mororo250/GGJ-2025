using Godot;

public partial class MainMenu : Control
{
	public override void _EnterTree()
	{
		Window window = GetViewport().GetWindow();
		window.ContentScaleFactor = 1;
	}

	public override void _Ready()
	{
		AudioManager.Instance.PlayMusic(AudioType.MusicMenu, 0.35f);
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
