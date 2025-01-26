using agents;
using Godot;

public enum BubbleState
{
	Bloom,
	Idle,
	Pop,
}

public partial class Bubbles : RigidBody2D
{
	[Export]private Area2D _area2D;
	[Export]private AnimatedSprite2D _animatedSprite2D;
	private BubbleState _bubbleState = BubbleState.Bloom;
	
	public override void _Ready()
	{
		_area2D.BodyEntered += OnCollision;
		Freeze              =  true;
		_animatedSprite2D.Play("Bloom");
		AudioManager.Instance.PlaySFX(AudioType.BubbleBegin, GlobalPosition, 1000,0.3f);
		_animatedSprite2D.AnimationFinished += OnAnimationFinished;
	}

	public void OnCollision(Node node)
	{
		_animatedSprite2D.Play("Pop");
		AudioManager.Instance.PlaySFX(AudioType.BubbleBegin, GlobalPosition, 1000, 0.3f);

		_bubbleState = BubbleState.Pop;
		
		if (node is Player player)
		{
			player.RecoverOxygen();
		}
	}
	
	public void OnAnimationFinished()
	{
		switch (_bubbleState)
		{
			case BubbleState.Bloom:
				Freeze       = false;
				_bubbleState = BubbleState.Idle;
				_animatedSprite2D.Play("Idle");
				break;
			case BubbleState.Pop:
				QueueFree();
				break;
		}
	}
}
