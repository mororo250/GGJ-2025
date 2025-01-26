using Godot;
using System;

public partial class SceneManager : Node
{
	static private SceneManager _instance;
	static public SceneManager Instance => _instance;
	
	[Export] private PackedScene _mainMenu;
	[Export] private PackedScene _mainGame;
	
	private Node _currentScene;
	
	public override void _Ready()
	{
		_instance     = this;
		_currentScene = GetTree().CurrentScene;
	}
	
	public void LoadMainMenu()
	{
		ChangeScene(_mainMenu);
	}
	
	public void LoadGame()
	{
		ChangeScene(_mainGame);
	}
	
	private void ChangeScene(PackedScene scene)
	{
		if (_currentScene != null)
		{
			_currentScene.QueueFree();
		}
		
		_currentScene = scene.Instantiate();
		GetTree().Root.AddChild(_currentScene);
		GetTree().CurrentScene = _currentScene;
	}
}
