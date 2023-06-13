using System;
using System.Collections.Generic;

public class GreibachFormConverter
{
    private Dictionary<char, HashSet<string>> productionRules;

    public GreibachFormConverter(Dictionary<char, HashSet<string>> productionRules)
    {
        this.productionRules = productionRules;
    }

    public Dictionary<char, HashSet<string>> ConvertToGreibachForm()
    {
        Dictionary<char, HashSet<string>> convertedRules = new Dictionary<char, HashSet<string>>();

        foreach (var rule in productionRules)
        {
            char nonTerminal = rule.Key;
            HashSet<string> productions = new HashSet<string>();

            foreach (var production in rule.Value)
            {
                if (production.Length > 1)
                {
                    for (int i = 0; i < production.Length; i++)
                    {
                        char symbol = production[i];

                        if (!IsNonTerminal(symbol))
                        {
                            char newNonTerminal = GetNewNonTerminal();
                            productions.Add(newNonTerminal.ToString());
                            convertedRules[newNonTerminal] = new HashSet<string> { symbol.ToString() };
                            symbol = newNonTerminal;
                        }

                        if (i == production.Length - 1)
                            productions.Add(symbol.ToString());
                        else
                            productions.Add(symbol.ToString() + production.Substring(i + 1));
                    }
                }
                else
                {
                    productions.Add(production);
                }
            }

            convertedRules[nonTerminal] = productions;
        }

        return convertedRules;
    }

    private bool IsNonTerminal(char symbol)
    {
        return char.IsUpper(symbol);
    }

    private char GetNewNonTerminal()
    {
        char newNonTerminal = 'A';
        while (productionRules.ContainsKey(newNonTerminal))
        {
            newNonTerminal++;
        }
        return newNonTerminal;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Dictionary<char, HashSet<string>> productionRules = new Dictionary<char, HashSet<string>>
        {
            { 'S', new HashSet<string> { "AB", "SCB" } },
            { 'A', new HashSet<string> { "aA", "C" } },
            { 'B', new HashSet<string> { "bB", "b" } },
            { 'C', new HashSet<string> { "cC", "d" } }
        };

        GreibachFormConverter converter = new GreibachFormConverter(productionRules);
        Dictionary<char, HashSet<string>> convertedRules = converter.ConvertToGreibachForm();

        foreach (var rule in convertedRules)
        {
            char nonTerminal = rule.Key;
            HashSet<string> productions = rule.Value;

            Console.Write(nonTerminal + " -> ");
            foreach (var production in productions)
            {
                Console.Write(production + " ");
            }
            Console.WriteLine();
        }
    }
}
