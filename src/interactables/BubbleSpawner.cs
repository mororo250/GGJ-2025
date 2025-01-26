using Godot;

public partial class BubbleSpawner : Node2D
{
	[Export] private PackedScene _bubblePrefab;
	[Export] private double _timToSpawn = 5;
	private double _timerAccumulator = 0;

	public override void _Process(double delta)
	{
		_timerAccumulator += delta;
		if (_timerAccumulator >= _timToSpawn)
		{
			_timerAccumulator = 0;
			CreateBubble();
		}	
	}
	
	private void CreateBubble()
	{
		Bubbles bubble = _bubblePrefab.Instantiate<Bubbles>();
		AddChild(bubble);
		bubble.GlobalPosition = GlobalPosition + new Vector2(0, -10);
	}
}
