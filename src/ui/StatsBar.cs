using System.Collections.Generic;
using Godot;
using agents;
using Godot.Collections;

public enum Stats
{
	Oxygen,
	Health,
}

public partial class StatsBar : Control
{
	// todo: remove direct access to player, pass current oxygen as a parameter
	[Export] private Player _player;
	[Export] private Stats _stats = Stats.Oxygen;

	private       List<AnimatedSprite2D> _bubblesList           = new();
	private const uint                   MaxNumberOfBubble      = 4;
	private       uint                   _currentNumberOfBubble = MaxNumberOfBubble;

	public override void _Ready()
	{
		int         i        = 0;
		Array<Node> children = GetChildren();
		foreach (Node node in children)
		{
			if (node is AnimatedSprite2D bubble)
			{
				_bubblesList.Add(bubble);
				bubble.Play("Idle");
			}
			i++;
		}
	}

	public override void _Process(double delta)
	{
		uint numberOfBubble = GetNumberOfBubble();
		if (numberOfBubble == _currentNumberOfBubble)
			return;
		
		if (numberOfBubble > _currentNumberOfBubble)
		{
			for (int i = (int)_currentNumberOfBubble; i < numberOfBubble; i++)
			{
				_bubblesList[i].Visible = true;
				_bubblesList[i].Play("Bloom");
				AudioManager.Instance.PlayGlobalSFX(AudioType.OxygenAlert, 0.3f);
			}
		}
		else if (numberOfBubble < _currentNumberOfBubble)
		{
			for (int i = (int)_currentNumberOfBubble - 1; i >= numberOfBubble; i--)
			{
				_bubblesList[i].Play("Pop");
				AudioManager.Instance.PlayGlobalSFX(AudioType.OxygenAlert, 0.3f);
			}
		}
		_currentNumberOfBubble = numberOfBubble;
	}
	
	private uint GetNumberOfBubble()
	{
		switch (_stats)
		{
			case Stats.Oxygen:
				float oxygenPr = (float)_player.Oxygen / _player.OxygenMaxValue;
				return oxygenPr > 0.75f ? 4u : oxygenPr > 0.5f ? 3u : oxygenPr > 0.25f ? 2u : oxygenPr > 0.0f ? 1u : 0u;
			case Stats.Health:
				float healthpr = (float)_player.Health / _player.HealthMaxValue;
				return healthpr > 0.75f ? 4u : healthpr > 0.5f ? 3u : healthpr > 0.25f ? 2u : healthpr > 0.0f ? 1u : 0u;
			default:
				return 0;
		}
	}
}
