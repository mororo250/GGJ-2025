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
	[Signal] public delegate void TrashCollectedEventHandler();

	[Export] private uint _oxygenMaxValue = 500;
	[Export] private uint _healthMaxValue  = 1000;
	[Export] private uint _oxygenDecayRate = 1;

	// stats
	private uint _trashCount;
	private uint _health;
	private uint _oxygen;
	public  uint TrashCount => _trashCount;
	public  uint Health     => _trashCount;
	public  uint Oxygen     => _trashCount;

	public void RecoverOxygen()
	{
		_oxygen = _oxygenMaxValue;
	}
	
	public override void _Ready()
	{
		_chain.TrashCollected += OnTrashColected;
		_health = _healthMaxValue;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		UpdateMovement();
		UpdateOxygen();
		
		if (_health <= 0)
		{
			// Game over screen
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
			_health -= 1;
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
		EmitSignal(SignalName.TrashCollected);
	}
}
