using Godot;
using Godot.Collections;
using trash;

namespace agents;

public partial class Chain : Node2D
{
	[Export] private StaticBody2D _chainTip;
	[Export] private Area2D       _chainTipArea;
	[Export] private Sprite2D     _chainSprite;
	[Export] private int          _chainMaxLength = 16;

	private          float        _length = 0;
	private          bool		  _isChaining;
	private Trash _collectedTrash;
	
	[Signal] public delegate void TrashCollectedEventHandler();

	public float Length
	{
		get => _length;
		private set
		{
			const int spriteSize = 64;
			_chainSprite.RegionRect = new Rect2(0, 0, spriteSize, value * spriteSize);
			
			const int distanceBetweenChainLinks = 2;
			// 1 is to account the radius of the chain tip
			_chainTip.Position = new Vector2(0, distanceBetweenChainLinks * (value + 1));
			_length            = value;
		}
	}
	
	public override void _Ready()
	{
		_chainTipArea.Monitoring = true;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (_collectedTrash != null)
			_collectedTrash.GlobalPosition = _chainTip.GlobalPosition;
		
		if (_isChaining)
		{
			Length += 0.5f;
			if (Length >= _chainMaxLength)
			{
				Length = _chainMaxLength;
			}
		}
		else if (Length > 0)
		{
			Length -= 0.5f;
			if (Length < 0)
				Length = 0;
		}	

		// Free trash
		if (_length == 0 && _collectedTrash != null)
		{
			EmitSignal(SignalName.TrashCollected);
			_collectedTrash.QueueFree();
			_collectedTrash = null;
		}
	}
	
	private void UpdateChainLength()
	{
		if (_isChaining)
		{
			Length += 0.5f;
			if (Length >= _chainMaxLength)
			{
				Length = _chainMaxLength;
			}
		}
		else if (Length > 0)
		{
			Length -= 0.5f;
			if (Length < 0)
				Length = 0;
		}
	}

	public void OnChaining(bool isChaning)
	{
		_isChaining = isChaning;
		if (!isChaning)
		{
			Array<Node2D> nodes = _chainTipArea.GetOverlappingBodies();
			foreach (var node in nodes)
			{
				if (node is Trash trash)
				{
					_collectedTrash        = trash;
					_collectedTrash.Freeze = true;
					return;
				}
			}
		}
	}
}