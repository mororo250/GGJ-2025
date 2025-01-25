using Godot;
using System.Collections.Generic;

public partial class PatrolList : Node2D
{
	[Signal]public delegate void PatrolPointedEventHandler(Node node); 
	private List<Area2D> _patrolPath = new();
	
	public override void _Ready()
	{
		var nodes = GetChildren();
		foreach (var node in nodes)
		{
			if (node is Area2D patrolPoint)
			{
				_patrolPath.Add(patrolPoint);
				patrolPoint.BodyEntered += OnPatrolPointed;
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnPatrolPointed(Node body)
	{
		EmitSignal(SignalName.PatrolPointed, body);
	}
}
