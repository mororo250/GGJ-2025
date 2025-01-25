using Godot;
using System.Collections.Generic;
using agents;
using Godot.Collections;

public enum SharkState
{
	Swimming,
	Attacking,
	Cooldown,
}

public partial class Shark : CharacterBody2D
{
	[Export] private Player _player;
	[Export] private Node2D       _patrolList;
	private          List<Area2D> _patrolPath = new();
	
	[Export] private float _swiminSpeed = 50; // pixels per second
	[Export] private float _attackSpeed = 80; //  pixels per seconds
	[Export] private uint  _cooldownTime = 0; // frames
	[Export] private uint  _attackDamage = 250; 
	private uint  _currentCooldownTime = 0; // frames
	
	
	[Export] private AnimatedSprite2D _animatedSprite;

	// shark State
	SharkState _state = SharkState.Swimming;
	private          int _currentPatrolPoint = 0;

	public override void _Ready()
	{
		Array<Node> patrolPoints = _patrolList.GetChildren();
		foreach (Node node in patrolPoints)
		{
			if (node is Area2D patrolPoint)
			{
				_patrolPath.Add(patrolPoint);
			}
		}
		
		_animatedSprite.Play("Swim");

	}

	public override void _PhysicsProcess(double delta)
	{
		switch (_state)
		{
			case SharkState.Swimming:
				Swim();
				break;
			case SharkState.Attacking:
				Attack();
				break;
			case SharkState.Cooldown:
				Cooldown();
				break;
		}

		MoveAndSlide();
	}

	private void Swim()
	{
		if (_patrolPath.Count == 0)
			return;
		
		Vector2 target = _patrolPath[_currentPatrolPoint].GlobalPosition;
		Vector2 direction = target - GlobalPosition;
		direction = direction.Normalized();
		Velocity = direction * _swiminSpeed;
	}
	
	private void Cooldown()
	{
		Velocity = Vector2.Zero;
		_currentCooldownTime--;
		if (_cooldownTime == 0)
		{
			_state = SharkState.Swimming;
			_animatedSprite.Play("Swim");
		}
	}

	private void Attack()
	{
		// Get the closest patrol point
		// If distance is bigger than x 
		// Stop going after it
		
		Vector2 target = _player.GlobalPosition;
		Vector2 direction = target - GlobalPosition;
		direction = direction.Normalized();
		Velocity = direction * _attackSpeed;
	}
	
		public override void _Process(double delta)
	{
		UpdateSprite();
	}

	private void UpdateSprite()
	{
		if (Velocity.X < 0)
		{
			Transform = Transform2D.FlipX.Translated(Transform.Origin);
		}
		else if (Velocity.X > 0)
		{
			Transform = Transform2D.Identity.Translated(Transform.Origin);
		}
	}
	
	private void OnPatrolPointBodyEntered(Node body)
	{
		if (body != this)
			return;
		
		_currentPatrolPoint++;
		if (_currentPatrolPoint >= _patrolPath.Count)
		{ 
			_currentPatrolPoint = 0;
		}
	}

	private void OnPlayerSpotted(Node node)
	{
		if (_currentCooldownTime > 0)
			return;
		
		if (node is Player player)
		{
			_state = SharkState.Attacking;
			_animatedSprite.Play("Attack");
		}
	}
	
	private void OnBodyEntered(Node body)
	{
		if (body is Player player)
		{
			player.TakeDamage(_attackDamage);
			_player.ApplyImpulse(Velocity * 2);
			_state               = SharkState.Cooldown;
			_currentCooldownTime = _cooldownTime;
		}
	}
}
