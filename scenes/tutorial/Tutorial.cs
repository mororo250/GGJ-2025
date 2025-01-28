using Godot;
using System;
using System.Diagnostics;

public partial class Tutorial : Node2D
{
	private Stopwatch _stopwatch = new Stopwatch();
	
	public void OnEnterScreen()
	{
		_stopwatch.Start();
		GD.Print("Tutorial entered the screen");
	}
	
	public override void _Process(double delta)
	{
		if (_stopwatch.ElapsedMilliseconds >= 6000)
		{
			_stopwatch.Stop();
			GD.Print("Tutorial finished");
			QueueFree();
		}
	}
}
