using Godot;
using System.Collections.Generic;
using agents;
using Godot.Collections;

public partial class Fish : CharacterBody2D
{
	[Export] private Node2D       _patrolList;
	[Export] private AnimatedSprite2D _animatedSprite; 
	private          List<Area2D> _patrolPath = new();
	
	[Export] private float _swiminSpeed = 50; // pixels per second
	[Export] private uint  _hitDamage = 250; 
	private          int _currentPatrolPoint = 0;

	public override void _Ready()
	{
		_animatedSprite.Play(_animatedSprite.Animation);
		Array<Node> patrolPoints = _patrolList.GetChildren();
		foreach (Node node in patrolPoints)
		{
			if (node is Area2D patrolPoint)
			{
				_patrolPath.Add(patrolPoint);
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Swim();
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
	
	public override void _Process(double delta)
	{
		UpdateSprite();
	}

	private void UpdateSprite()
	{
		if (Velocity.X > 0)
		{
			Transform = Transform2D.FlipX.Translated(Transform.Origin);
		}
		else if (Velocity.X < 0)
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
	
	private void OnBodyEntered(Node body)
	{
		if (body is Player player)
		{
			player.TakeDamage(_hitDamage);
			player.ApplyImpulse(Velocity * 1.5f);
		}
	}
}
