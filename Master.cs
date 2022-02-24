global using System.Collections.Generic;
global using System;
using static System.Console;
namespace DarkSoulsNameChecker;

static class Master
{
	static Master()
	{
		Title = "Dark Souls Name Checker";
	}
	static void Main()
	{
		//Console.Write("Choose\n1. Ds1\n2. Ds2\n 3. Ds3");
		WriteLine("Choose what game: ");
		foreach (Game i in Enum.GetValues<Game>())
			WriteLine($"{i.GetHashCode()}. {i}");
		Game game = Enum.Parse<Game>(ReadLine() ?? string.Empty);
		WriteLine(DSNameChecker.CreateViaPlayerInput(game));
		// Pause before ending
		WriteLine("Press any key to quit!");
		ReadKey(false);
	}
}
public class DSNameChecker
{
	internal string[] input = new string[2];
	internal List<HiddenNameSection> badWords;
	internal bool nameChanged, nameTooLong, hasBadWords;
	public DSNameChecker(Game game, string name)
	{
		input[0] = name; input[1] = name;
		HashSet<bool> discrepancies = new();
		nameTooLong = NameLengthCheck(ref input[0], BlockList.TryGetNameLength(game));
		discrepancies.Add(nameTooLong);
		hasBadWords = NameBadWordsCheck(ref input[0],BlockList.GetList(game), out badWords);
		discrepancies.Add(hasBadWords);
		nameChanged = discrepancies.Contains(true);
	}
	public static DSNameChecker CreateViaPlayerInput(Game game)
	{
		string[] input = new string[2];
		Write("Input Name: ");
		input[0] = ReadLine() ?? "";
		input[1] = input[0];
		return new DSNameChecker(game, input[0]);
	}
	private static bool NameLengthCheck(ref string input, in byte? maxLength)
	{
		if (maxLength == null) return false;
		if (input.Length > maxLength)
		{
			input = input.Remove(maxLength.Value);
			return true;
		}
		return false;
	}
	private static bool NameBadWordsCheck(ref string input, in string[] badWords, out List<HiddenNameSection> outputList)
	{
		outputList = new List<HiddenNameSection>();
		bool var = false;
		foreach (string i in badWords)
			if (input.Contains(i)) 
			{
				var = true;
				outputList.Add(new HiddenNameSection(input.IndexOf(i), i.Length));
				input = outputList[^1].Apply(input);
			}
		foreach (HiddenNameSection i in outputList)
			input = i.Apply(input);
		return var;
	}
	public override string ToString() => 
		$"Your character name is{(nameChanged ? " not" : "")} allowed" // Reads bool discrepancy as human.
		+ $"{(nameTooLong ? "\nYour name is too long" : "")}" // Mentions if name too long, hides otherwise.
		+ $"{(nameChanged ? $"\nOld: {input[1]}, New: {input[0]}" : "")}"; // shows results if bool discrepancy is enabled.
}
public struct HiddenNameSection
{
	public int start, length;
	public readonly string? original = null; // Entirely optional, you can remove this
	private readonly string hashtagString = "";
	public HiddenNameSection(string original, int start)
	{
		this.original = original;
		this.start = start;
		length = original.Length;
		for (int i = 0; i < original.Length; i++)
			hashtagString += "*";
	}
	public HiddenNameSection(int start, int length)
	{
		this.start = start;
		this.length = length;
		for (int i = 0; i < length; i++)
			hashtagString += "*";
	}
	public string Apply(string input) => input.Remove(start, length).Insert(start, hashtagString);
	public override string ToString()
	{
		string foo = "";
		for (int i = 0; i < length; i++)
			foo += "*";
		return foo;
	}
}
public enum Game
{
	DsR,
	Ds2,
	Ds3,
}