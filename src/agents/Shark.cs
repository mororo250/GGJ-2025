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
	[Export] private Node2D       _patrolList;
	private          List<Area2D> _patrolPath = new();
	
	[Export] private float  _swiminSpeed  = 50; // pixels per second
	[Export] private float  _attackSpeed  = 65; //  pixels per seconds
	[Export] private uint   _cooldownTime = 90;  // frames
	[Export] private uint   _attackDamage = 250; 
	[Export] private uint   _attackRange  = 250; // pixels
	private          Player _player;
	private          uint   _currentCooldownTime = 0; // frames
	
	
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
		if (_currentCooldownTime == 0)
		{
			_state = SharkState.Swimming;
		}
	}

	private void Attack()
	{
		int closestPatrolPoint = ClosestPatrolPoint();
		Vector2 patrolPointPos = _patrolPath[closestPatrolPoint].GlobalPosition;
		if (GlobalPosition.DistanceTo(patrolPointPos) > _attackRange || _player == null)
		{
			_currentPatrolPoint = closestPatrolPoint;
			_state = SharkState.Swimming;
			_animatedSprite.Play("Swim");
			return;
		}
		
		Vector2 target = _player.GlobalPosition;
		Vector2 direction = target - GlobalPosition;
		direction = direction.Normalized();
		Velocity = direction * _attackSpeed;
	}
	
	private int ClosestPatrolPoint()
	{
		float minDistance = float.MaxValue;
		int   closestPatrolPoint = 0;
		for (int i = 0; i < _patrolPath.Count; i++)
		{
			float distance = GlobalPosition.DistanceTo(_patrolPath[i].GlobalPosition);
			if (distance < minDistance)
			{
				minDistance = distance;
				closestPatrolPoint = i;
			}
		}

		return closestPatrolPoint;
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
	
	private void OnPatrolPointBodyEntered(Area2D body)
	{
		if (body != _patrolPath[_currentPatrolPoint])
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
		
		int closestPatrolPoint = ClosestPatrolPoint();
		if (GlobalPosition.DistanceTo(_patrolPath[closestPatrolPoint].GlobalPosition) > _attackRange)
			return;
		
		
		if (node is Player player)
		{
			_state = SharkState.Attacking;
			_animatedSprite.Play("Attack");
			_player = player;
		}
	}
	
	private void OnBodyEntered(Node body)
	{
		if (body is Player player)
		{
			player.TakeDamage(_attackDamage);
			player.ApplyImpulse(Velocity * 2);
			_state = SharkState.Cooldown;
			_currentCooldownTime = _cooldownTime;
			_animatedSprite.Play("Swim");
		}
	}
}
