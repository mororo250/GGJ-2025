using Godot;
using System.Diagnostics;

public partial class Timer : Control
{
	private Stopwatch _timer;
	[Export] private Label _label;
	
	private void OnPauseButtonPressed() => _timer.Stop();
	private void OnResumeButtonPressed() => _timer.Start();
	
	public override void _Ready()
	{
		_timer = new Stopwatch();
		_timer.Start();
	}
	
	public override void _Process(double delta)
	{
		if (Engine.GetFramesDrawn() == 0)
			_timer.Start();
		
		_label.Text = _timer.Elapsed.ToString(@"mm\:ss");
	}
}
