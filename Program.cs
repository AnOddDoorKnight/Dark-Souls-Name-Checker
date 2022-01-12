global using System.Collections.Generic;
global using System;
namespace DarkSoulsNameChecker;

static class Master
{
	static Master()
	{
		Console.Title = "Dark Souls Name Checker";
	}
	static void Main()
	{
		//Console.Write("Choose\n1. Ds1\n2. Ds2\n 3. Ds3");
		Console.WriteLine(DSNameChecker(Game.Ds3));
		// Pause before ending
		Console.WriteLine("Press any key to quit!");
		Console.ReadKey(false);
	}
}
public class DSNameChecker
{
	internal string[] input = new string[2];
	internal List<HiddenNameSection> badWords;
	internal bool nameChanged, nameTooLong, hasBadWords;
	public DSNameChecker(Game game)
	{
		Console.Write("Input Name: ");
		input[0] = Console.ReadLine() ?? "";
		input[1] = input[0];
		HashSet<bool> discrepancies = new();
		nameTooLong = NameLengthCheck(input[0], BlockList.lengths[game]), out input[0] ?? input[0]);
		discrepancies.Add(nameTooLong);
		hasBadWords = NameBadWordsCheck(input[0],BlockList.disallowedTerms[game], out input[0], out badWords);
		discrepancies.Add(hasBadWords);
		nameChanged = discrepancies.Contains(true);
	}
	private static bool NameLengthCheck(in string input, in byte maxLength, out string? output)
	{
		if (input.Length > maxLength)
		{
			output = input.Remove(maxLength);
			return true;
		}
		output = null;
		return false;
	}
	static bool NameBadWordsCheck(in string input, in string[] badWords, out string output, out List<HiddenNameSection> outputList)
	{
		outputList = new List<HiddenNameSection>();
		bool var = false;
		foreach (string i in badWords)
			if (input.Contains(i)) 
			{
				var = true;
				outputList.Add(new HiddenNameSection(input.IndexOf(i), i.Length));
			}
		output = input;
		foreach (HiddenNameSection i in outputList)
			output = i.Apply(output);
		return var;
	}
	public override string ToString()
	{   // Results
		bool discrepancy = discrepancies.Contains(true);
		return $"Your character name is{(discrepancy ? " not" : "")} allowed" // Reads bool discrepancy as human.
			+ $"{(nameTooLong ? "\nYour name is too long" : "")}" // Mentions if name too long, hides otherwise.
			+ $"{(discrepancy ? $"\nOld: {input[0]}, New: {input[1]}" : "")}"); // shows results if bool discrepancy is enabled.
	}
}
public struct HiddenNameSection
{
	public int start, length;
	public readonly string? original = null;
	public HiddenNameSection(string original, int start)
	{
		this.original = original;
		this.start = start;
		this.length = original.length;
	}
	public HiddenNameSection(int start, int length)
	{
		this.start = start;
		this.length = length;
	}
	public string Apply(string input) => input.Remove(start, length).Insert(start, Value);
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
	Ds2
	Ds3
}