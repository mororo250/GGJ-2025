using System.Collections.Generic;
using Godot;
using agents;

public partial class OxygenBar : Control
{
	// todo: remove direct access to player, pass current oxygen as a parameter
	[Export] Player _player;
	[Export] PackedScene _bubblePrefab;

	private List<AnimatedSprite2D> _bubblesList = new();
	
	public override void _Ready()
	{
		uint numberOfBubble = _player.Oxygen / 100u;
		for (uint i = 0; i < numberOfBubble; i++)
		{
			AnimatedSprite2D bubble = _bubblePrefab.Instantiate<AnimatedSprite2D>();
			AddChild(bubble);
			_bubblesList.Add(bubble);
			bubble.Position = new Vector2(10 + i * 20, 10);
		}
	}

	public override void _Process(double delta)
	{
		uint numberOfBubble = _player.Oxygen / 100u;
		if (numberOfBubble == _bubblesList.Count)
			return;
		
		if (numberOfBubble > _bubblesList.Count)
		{
			for (int i = _bubblesList.Count; i < numberOfBubble; i++)
			{
				AnimatedSprite2D bubble = _bubblePrefab.Instantiate<AnimatedSprite2D>();
				AddChild(bubble);
				_bubblesList.Add(bubble);
				bubble.Position = new Vector2(10 + i * 20, 10);
				_bubblesList[i].Play("Pop");
			}
		}
		else
		{
			for (int i = _bubblesList.Count - 1; i >= numberOfBubble; i--)
			{
				_bubblesList[i].Play("Pop");
			}
		}
	}
}
