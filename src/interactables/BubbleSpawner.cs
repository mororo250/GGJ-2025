using Godot;

public partial class BubbleSpawner : AnimatedSprite2D
{
	[Export] private PackedScene _bubblePrefab;
	[Export] private double _timeToSpawn = 5;
	private double _timerAccumulator = 0;

	public override void _Process(double delta)
	{
		_timerAccumulator += delta;
		if (_timerAccumulator >= _timeToSpawn)
		{
			_timerAccumulator = 0;
			CreateBubble();
			Play("Close");
		}	
	}
	
	private void CreateBubble()
	{
		Bubbles bubble = _bubblePrefab.Instantiate<Bubbles>();
		AddChild(bubble);
		bubble.GlobalPosition = GlobalPosition + new Vector2(0, -10);
	}
	
	private void OnAnimationFinished()
	{
		if (Animation == "Close")
			Play("Open");
	}
}
