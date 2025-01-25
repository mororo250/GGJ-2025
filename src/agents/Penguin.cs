using agents;
using Godot;

public partial class Penguin : AnimatedSprite2D
{
	private bool _playerIsNearby;

	public override void _Ready()
	{
		Play("Locked");
	}

	private void OnPlayerDetectorBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			_playerIsNearby = true;
		}
	}
	
	private void OnPlayerDetectorBodyLeave(Node2D body)
	{
		if (body is Player)
		{
			_playerIsNearby = false;
		}
	}
	
	private void Interact()
	{
		Play("Happy");
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (!_playerIsNearby)
			return;
		
		if (@event is InputEventKey keyEvent)
		{
			if (keyEvent.IsPressed())
			{
				switch (keyEvent.Keycode)
				{
					case Key.E:
						Interact();
						break;
				}
			}
		}
	}
}
