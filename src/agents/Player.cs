using System;
using CCJ2025.agents;
using Godot;

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
	[Export] private float _impulse = 1.0f;
	private  MovementDirection _movementDirection = MovementDirection.None;

	// Chain
	[Signal] public delegate void  ChainingEventHandler(bool IsChaning);
	[Export] private         Chain _chain;
	[Export] private         int   _chainMaxLength = 128;
	private                  bool  _isChaining     = false;
	
	public override void _Ready()
	{
	}

	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 impulse = Vector2.Zero;

		if ((_movementDirection & MovementDirection.Up) != 0)
			impulse.Y = -_impulse;

		if ((_movementDirection & MovementDirection.Down) != 0)
			impulse.Y += _impulse;

		if ((_movementDirection & MovementDirection.Left) != 0)
			impulse.X -= _impulse;

		if ((_movementDirection & MovementDirection.Right) != 0)
			impulse.X += _impulse;

		ApplyCentralImpulse(impulse);


		if (_isChaining)
		{
			_chain.Length += 0.5f;
			if (_chain.Length >= _chainMaxLength)
			{
				EmitSignal(nameof(ChainingEventHandler), true);
				_chain.Length = _chainMaxLength;
			}
		}
		else if (_chain.Length > 0)
		{
			_chain.Length -= 0.5f;
			if (_chain.Length < 0)
				_chain.Length = 0;
		}	
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
						_isChaining = true;
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
						_isChaining = false;
						break;
				}
			}
		}
	}
}
