using Godot;

public partial class TrashCounter : Control
{
	[Export] private Label _label;
	private void SetCounter(int counter)
	{
		_label.Text = counter.ToString();
	}
}
