using Godot;
using System.Collections.Generic;
using Godot.Collections;

public partial class BubblesManager : Node
{
	[Export] private PackedScene _bubblePrefab;
	private List<Bubbles> _bubblesList = new ();
	
	public void CreateBubble(Vector2 position)
	{
		Bubbles bubble = _bubblePrefab.Instantiate<Bubbles>();
		AddChild(bubble);
		bubble.Position = position;
		_bubblesList.Add(bubble);
	}
	
	public override void _Ready()
	{
		Array<Node> nodes = GetChildren();

		foreach (Node node in nodes)
		{
			if (node is Bubbles bubble)
			{
				_bubblesList.Add(bubble);
			}
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouse eventMouse)
		{
			if (eventMouse.ButtonMask == MouseButtonMask.Left)
			{
				CreateBubble(eventMouse.Position);
			}
		}
	}
}
