namespace CCJ2025.main;

public static class GameState
{
	public static uint CurrentLevel     = 1;
	public static uint Level1TrashCount = 12;
	public static uint Level2TrashCount = 14;
	public static uint CurrentLevelTrashCount => CurrentLevel switch
	{
		1 => Level1TrashCount,
		2 => Level2TrashCount,
		_ => 0
	};
}