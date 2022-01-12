global using System.Collections.Generic;
global using System;
namespace DarkSoulsNameChecker;

static class Master
{
	static internal Dictionary<string, string> input = new();
	static internal List<HiddenNameSection> hidden = new();
	static internal HashSet<bool> discrepancies = new();
	static Master()
	{
		Console.Title = "Dark Souls Name Checker";
	}
	static void Main()
	{
		//Console.Write("Choose\n1. Ds1\n2. Ds2\n 3. Ds3");
		DarkSouls3Checker();
		// Pause before ending
		Console.WriteLine("Press any key to quit!");
		Console.ReadKey(false);
	}
	static void DarkSouls3Checker()
	{
		Console.WriteLine("Running Dark Souls 3 Name Checker...");
		// This is input
		Console.Write("Input Name: ");
		input.Add("value", Console.ReadLine() ?? string.Empty);
		input.Add("defaultValue", input["value"]);
		// Checks Name/String Length, will remove extra characters before any name checking
		discrepancies.Add(NameLengthCheck(16));
		// Checks if theres bad words in the name/string
		discrepancies.Add(NameBadWordsCheck(BlockList.DarkSouls3DisallowedTerms));
		Write(discrepancies.Contains(true));
	}
	static void Write(bool discrepancy)
	{   // Results
		Console.WriteLine($"Your character name is{(!discrepancy ? "" : " not")} allowed" // Reads bool discrepancy as human.
			+ $"{(input["defaultValue"].Length > 16 ? "\nYour name is too long" : "")}" // Mentions if name too long, hides otherwise.
			+ $"{(discrepancy ? $"\nOld: {input["defaultValue"]}, New: {input["value"]}" : "")}"); // shows results if bool discrepancy is enabled.
	}
	static bool NameBadWordsCheck(in string[] badWords)
	{
		bool var = false;
		foreach (string i in badWords)
			if (!input["value"].Contains(i)) continue;
			else
			{
				var = true;
				hidden.Add(new HiddenNameSection(input["value"].IndexOf(i), i.Length));
			}
		return var;
	}
	// Measures name length, replaces with nothing when its longer than specified and returns true
	static bool NameLengthCheck(in byte maxLength = 16)
	{
		if (input["value"].Length > maxLength)
		{
			input["value"] = input["value"].Remove(maxLength);
			return true;
		}
		return false;
	}
	// Replaces the word with hashtags; normal word => hashtags with the length of word
}
public struct HiddenNameSection
{
	public int start, length;
	public string Value => ToString();
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