namespace DarkSoulsNameChecker;
partial record BlockList
{
	// Thanks to https://github.com/omgftw/DarkSouls3CensorCheck for this Magnificient List!
	public static byte GetNameLength(Game game) => game switch
	{
		Game.Ds3 => 16,
		_ => throw new NotImplementedException(),
	};
	public static byte? TryGetNameLength(Game game)
	{
		try
		{
			return GetNameLength(game);
		}
		catch (NotImplementedException)
		{
			return null;
		}
	}
}