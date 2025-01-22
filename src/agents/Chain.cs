using Godot;

namespace CCJ2025.agents;

public partial class Chain : Node2D
{
	[Export] private StaticBody2D _chainTip;
	[Export] private Sprite2D	  _chainSprite;

	public float Length
	{
		get => _length;
		set
		{
			
			const int spriteSize = 64;
			_chainSprite.RegionRect = new Rect2(0, 0, 1, value * spriteSize);
			
			// 8 is the distance between the chain links
			// 1 is to account the radius of the chain tip
			_chainTip.Position = new Vector2(0, 8 * (value + 1));
			
			_length                 = value;
		}
	}
	
	private float _length = 0;
	
	
}