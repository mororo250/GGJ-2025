using CCJ2025.main;
using Godot;

public partial class TrashCounter : Control
{
	[Export] private Label _label;
	
	public override void _Ready()
	{
		SetCounter(0);
	}
	
	
	private void SetCounter(uint counter)
	{
		_label.Text = $"{counter}/{GameState.CurrentLevelTrashCount}";
	}
}
