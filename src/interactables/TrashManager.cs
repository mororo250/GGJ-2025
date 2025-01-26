using agents;
using Godot;

namespace trash;

public partial class TrashManager : Node2D
{
	[Export] private PackedScene _trashPrefab;
	
	public void SpawnTrash(Vector2 position, string trashType = "Plastico")
	{
		Trash trash = (Trash)_trashPrefab.Instantiate();
		AddChild(trash);
		trash.Position = position;
		trash.Animation = trashType;
		trash.ApplyImpulse(new Vector2(-15, -35));
	}

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
}
