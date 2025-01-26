using System;
using Godot;
using trash;

namespace agents;

// Todo: Do proper implementation of controls with actions
[Flags]
enum MovementDirection
{
	None  = 0,
	Up    = 1 << 0,
	Down  = 1 << 1,
	Left  = 1 << 2,
	Right = 1 << 3
}

public partial class Player : RigidBody2D
{
	// Movement
	[Export] private Sprite2D _sprite2D;
	[Export] private float _impulse = 1.0f;
	private  MovementDirection _movementDirection = MovementDirection.None;

	// Chain
	[Export] private Chain _chain;
	[Signal] public delegate void TrashCollectedEventHandler(uint trashCount);
	[Signal] public delegate void GameOverEventHandler();

	[Export] private uint _oxygenMaxValue  = 4000;
	[Export] private uint _healthMaxValue  = 1000;
	[Export] private uint _oxygenDecayRate = 1;
	[Export] private  uint _basicDamage = 250;
	public           uint OxygenMaxValue => _oxygenMaxValue;
	public           uint HealthMaxValue => _healthMaxValue;

	// stats
	private uint _trashCount;
	private uint _health;
	private uint _oxygen;
	public  uint TrashCount => _trashCount;
	public  uint Health     => _health;
	public  uint Oxygen     => _oxygen;
	
	// flash animation
	private         double _flashTime       = 1;
	[Export]private double _flashDuration   = 0.1;
	private         uint _flashCounter    = 0;
	[Export]private uint _numberOfFlashes = 5;
	private         bool  _flash           = false;
	
	AudioStreamPlayer _soundPlayer;

	public void RecoverOxygen()
	{
		_oxygen = _oxygenMaxValue;
	}
	
	public void TakeDamage(uint damage)
	{
		_health -= damage;
		_flash = true;
		AudioManager.Instance.PlaySFX(AudioType.Damage, GlobalPosition, 200, 0.35f);
	}
	
	public override void _Ready()
	{
		_chain.TrashCollected += OnTrashColected;
		Init();
	}

	private void Init()
	{
		_soundPlayer     = AudioManager.Instance.PlayGlobalSFXLoop(AudioType.Submarine, 0.3f);
		_oxygen          = _oxygenMaxValue;
		_health          = _healthMaxValue;
	}

	public override void _Process(double delta)
	{
		Flash(delta);
	}

	private void Flash(double delta)
	{
		if (_flash)
		{
			_flashTime += delta;
			if (_flashTime >= _flashDuration)
			{
				if (_flashCounter >= _numberOfFlashes)
				{
					_flashTime		   = 1;
					_flashCounter      = 0;
					_flash             = false;
					_sprite2D.Modulate = Colors.White;
					return;
				}
				
				_flashTime = 0;
				_flashCounter++;
				_sprite2D.Modulate = _sprite2D.Modulate == Colors.White ? Colors.Red : Colors.White;
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_movementDirection == MovementDirection.None)
		{
			if (_soundPlayer.Playing)
				_soundPlayer.Stop();
		}
		else
		{
			if (!_soundPlayer.Playing)
				_soundPlayer.Play();
		}
		
		if (delta == 0)
			return;
		
		UpdateMovement();
		UpdateOxygen();
		
		if (_health == 0)
		{
			_soundPlayer.Stop();
			EmitSignal(SignalName.GameOver);
		}
	}

	private void UpdateMovement()
	{
		Vector2 impulse = Vector2.Zero;
		_sprite2D.Frame = 0;

		if ((_movementDirection & MovementDirection.Up) != 0)
			impulse.Y = -_impulse;

		if ((_movementDirection & MovementDirection.Down) != 0)
			impulse.Y += _impulse;

		if ((_movementDirection & MovementDirection.Left) != 0)
		{
			_sprite2D.Frame =  1;
			impulse.X       -= _impulse;
		}

		if ((_movementDirection & MovementDirection.Right) != 0)
		{
			_sprite2D.Frame =  3;
			impulse.X       += _impulse;
		}

		ApplyCentralImpulse(impulse);
	}

	private void UpdateOxygen()
	{
		if (_oxygen <= 0)
		{
			TakeDamage(_oxygenDecayRate);
			if (_health == 0)
				return;

			_oxygen = 0;
		}
		else 
			_oxygen -= _oxygenDecayRate;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent)
		{
			if (keyEvent.IsPressed())
			{
				switch (keyEvent.Keycode)
				{
					case Key.W:
						_movementDirection |= MovementDirection.Up;
						break;
					case Key.S:
						_movementDirection |= MovementDirection.Down;
						break;
					case Key.A:
						_movementDirection |= MovementDirection.Left;
						break;
					case Key.D:
						_movementDirection |= MovementDirection.Right;
						break;
					case Key.Space:
						_chain.OnChaining(true);
						break;
				}
			}
			else if (keyEvent.IsReleased())
			{
				switch (keyEvent.Keycode)
				{
					case Key.W:
						_movementDirection &= ~MovementDirection.Up;
						break;
					case Key.S:
						_movementDirection &= ~MovementDirection.Down;
						break;
					case Key.A:
						_movementDirection &= ~MovementDirection.Left;
						break;
					case Key.D:
						_movementDirection &= ~MovementDirection.Right;
						break;
					case Key.Space:
						_chain.OnChaining(false);
						break;
				}
			}
		}
	}

	public void OnTrashColected()
	{
		_trashCount++;
		EmitSignal(SignalName.TrashCollected, _trashCount);
	}

	private void HandleCollisisonAgainstTerrain(Node body)
	{
		if (body is TileMapLayer tileMap)
		{
			uint damage = (uint)(LinearVelocity.Length() / 80 * _basicDamage);
			TakeDamage(damage);
		}
	}
}
