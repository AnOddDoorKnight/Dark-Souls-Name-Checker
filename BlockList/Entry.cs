namespace DarkSoulsNameChecker;
partial record BlockList
{
	public static string[] GetList(Game game) => game switch
	{
		Game.DsR => darkSoulsRemastered,
		Game.Ds3 => darkSouls3,
		Game.Ds2 => darkSouls2SotFS,
		_ => throw new NotImplementedException()
	};
}