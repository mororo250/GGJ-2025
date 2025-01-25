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
	[Export] private uint  _cooldownTime = 120; // frames
	
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

		UpdateSprite();
		MoveAndSlide();
	}

	private void UpdateSprite()
	{
		if (Velocity.X < 0)
		{
			_animatedSprite.FlipH = true;
		}
		else if (Velocity.X > 0)
		{
			_animatedSprite.FlipH = false;
		}
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
		_cooldownTime--;
		if (_cooldownTime == 0)
		{
			_state = SharkState.Swimming;
			_animatedSprite.Play("Swim");
			_cooldownTime = 120;
		}
	}

	private void Attack()
	{
		Vector2 target = _player.GlobalPosition;
		Vector2 direction = target - GlobalPosition;
		direction = direction.Normalized();
		Velocity = direction * _attackSpeed;
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
		if (_cooldownTime > 0)
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
			// player.TakeDamage();
			_player.ApplyImpulse(Velocity * 10);
			_state = SharkState.Cooldown;
		}
	}
}
