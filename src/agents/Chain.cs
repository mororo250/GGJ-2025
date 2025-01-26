using System;
using Godot;
using Godot.Collections;
using trash;

namespace agents;

public partial class Chain : Node2D
{
	[Export] private StaticBody2D     _chainTip;
	[Export] private Area2D           _chainTipArea;
	[Export] private AnimatedSprite2D _chainTipSprite;
	[Export] private Sprite2D         _chainSprite;
	[Export] private int              _chainMaxLength = 16;

	private          float        _length = 0;
	private          bool		  _isChaining;
	private Trash _collectedTrash;
	
	private AudioStreamPlayer2D _soundPlayer;
	
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
			Length += 0.5f;
			if (Length >= _chainMaxLength)
			{
				Length = _chainMaxLength;
			}
		}
		else if (Length > 0)
		{
			Length -= 0.5f;
			if (Length <= 0)
			{
				Length = 0;
			}
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
		if (!isChaning)
		{
			Array<Node2D> nodes = _chainTipArea.GetOverlappingBodies();
			foreach (var node in nodes)
			{
				if (node is Trash trash)
				{
					_collectedTrash = trash;
					_collectedTrash.SetFreeze(true);
					break;
				}
			}

			if (_isChaining != isChaning)
			{
				if (_soundPlayer != null && GodotObject.IsInstanceValid(_soundPlayer))
				{
					_soundPlayer.Stop();
					_soundPlayer.QueueFree();
				}

				_soundPlayer = AudioManager.Instance.PlaySFX(AudioType.Chain, GlobalPosition,
					600, 0.6f);
			}
		}
		else
		{
			if (_isChaining != isChaning)
			{
				_chainTipSprite.Play("Opening");
				
				if (_soundPlayer != null && IsInstanceValid(_soundPlayer))
				{
					_soundPlayer.Stop();
					_soundPlayer.QueueFree();
				}

				_soundPlayer = AudioManager.Instance.PlaySFX(AudioType.Chain, GlobalPosition,
					600, 0.6f);
			}
		}
		
		_isChaining = isChaning;
	}
}