using Godot;
using Godot.Collections;
using trash;

namespace agents;

public partial class Chain : Node2D
{
	[Export] private StaticBody2D     _chainTip;
	[Export] private Area2D           _chainTipArea;
	[Export] private Sprite2D         _chainSprite;
	[Export] private AnimatedSprite2D _chainTipSprite;
	[Export] private int              _chainMaxLength = 16;

	private          float        _length = 0;
	private          bool		  _isChaining;
	private Trash _collectedTrash;
	
	[Signal] public delegate void TrashCollectedEventHandler();

	public float Length
	{
		get => _length;
		private set
		{
			const int spriteHeight = 4;
			_chainSprite.RegionRect = new Rect2(0, 0, 3, value * spriteHeight);
			
			// 1 is to account the radius of the chain tip
			_chainTip.Position = new Vector2(0, spriteHeight * (value - 1));
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
		
		UpdateChainLength();
		FreeTrash();
	}
	
	private void UpdateChainLength()
	{
		if (_isChaining)
		{
			if(_length < 0.5f)
				_chainTipSprite.Play("Opening");
			
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

	private void FreeTrash()
	{
		if (_length == 0 && _collectedTrash != null)
		{
			EmitSignal(SignalName.TrashCollected);
			_collectedTrash.QueueFree();
			_collectedTrash = null;
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
			
			_chainTipSprite.Play("Closing");
		}
	}
}