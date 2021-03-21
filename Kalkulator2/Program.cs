using System;
using System.Data;

namespace Kalkulator2
{
    class Program
    {
        private static bool checkCharacters(string input, char[] allowedCharacters)
        {
            bool correct = true;
            foreach(char c in input)
            {
                correct = false;
                foreach(char d in allowedCharacters)
                {
                    if (c == d) correct = true;
                }
                if (!correct) return correct;
            }
            return correct;
        }
        private static double calculate(string input)
        {
            Console.WriteLine($"Input: {input}");
            char[] allowedCharacters = 
            {
                '1','2','3','4','5','6','7','8','9','0',
                '+','-','*','/','^','(',')',' '
            };

            char[] onlyNumbers = 
            {
                '1','2','3','4','5','6','7','8','9','0',
            };

            char[] onlyOperatrors =
            {
                '+','-','*','/','^','(',')'
            };

            input = input.Replace(" ", "");

            if (checkCharacters(input, allowedCharacters))
            {
                try
                {
                    return Convert.ToDouble(input);
                }
                catch (Exception)
                {
                    if (input.Contains("("))
                    {
                        int openBracketIndex = input.LastIndexOf("(");
                        if (input.Contains(")"))
                        {
                            int closedBracketIndex = input.IndexOf(")");
                            

                        }
                        else
                        {
                            Console.Write("Error: no closing bracket. ");
                            return -1;
                        }

                    }

                    if (input.Contains("+") || input.Contains("-"))
                    {
                        int plusIndex;
                        if (input.Contains("+")) plusIndex = input.LastIndexOf("+");
                        else plusIndex = -1;

                        int minusIndex;
                        if (input.Contains("-")) minusIndex = input.LastIndexOf("-");
                        else minusIndex = -1;

                        if (plusIndex > minusIndex)
                        {
                            string arg1 = input.Substring(0, plusIndex);
                            string arg2 = input.Substring(plusIndex + 1);
                            return calculate(arg1) + calculate(arg2);
                        }
                        else
                        {
                            string arg1 = input.Substring(0, minusIndex);
                            string arg2 = input.Substring(minusIndex + 1);
                            return calculate(arg1) - calculate(arg2);
                        }
                    }

                    else if (input.Contains("*") || input.Contains("/"))
                    {
                        int multiIndex;
                        if (input.Contains("*")) multiIndex = input.LastIndexOf("*");
                        else multiIndex = -1;

                        int divIndex;
                        if (input.Contains("/")) divIndex = input.LastIndexOf("/");
                        else divIndex = -1;

                        if (multiIndex > divIndex)
                        {
                            string arg1 = input.Substring(0, multiIndex);
                            string arg2 = input.Substring(multiIndex + 1);
                            return calculate(arg1) * calculate(arg2);
                        }
                        else
                        {
                            string arg1 = input.Substring(0, divIndex);
                            string arg2 = input.Substring(divIndex + 1);
                            return calculate(arg1) / calculate(arg2);
                        }
                    }

                    else if (input.Contains("^"))
                    {
                        int powIndex = input.IndexOf("^");
                        if (powIndex > 0)
                        {
                            string arg1 = input.Substring(0, powIndex);
                            string arg2 = input.Substring(powIndex + 1);
                            return Math.Pow(calculate(arg1), calculate(arg2));
                        }

                        Console.Write("Error: no power base entered. ");
                        return -1;
                    }

                    else
                    {
                        Console.Write("Error: unknown operator. ");
                        return -1;
                    }
                }

            }
            else
            {
                Console.Write("Error: not allowed characters detected. ");
                return -1;
            }
                /*if (checkCharacters(input, onlyNumbers))
                {
                    return Convert.ToDouble(input);
                }*/

                
        }
        static void Main(string[] args)
        {
            Console.Write("Your expression: ");
            string input = Console.ReadLine();
            Console.WriteLine(calculate(input));
        }
    }
}
