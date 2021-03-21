using System;
using System.Collections.Generic;
using System.Data;

namespace Kalkulator2
{
    class Program
    {
        class Expression
        {
            string expressionString;
            double result;
            List<string> errorList = new List<string>();

            public Expression(string input)
            {
                expressionString = input;
            }
            double calculate(string input)
            {
                if (errorList.Count > 0) return -1;

                Console.WriteLine($"Input: {input}");

                input = input.Replace(" ", "");

                if (input == "") return 0;

                if (checkCharacters(input, allowedCharacters))
                {
                    try
                    {
                        return Convert.ToDouble(input);
                    }
                    catch (Exception)
                    {
                        if (input.Contains("(") && input.Contains(")"))
                        {
                            int openBracketIndex = input.IndexOf("(");
                            int closedBracketIndex = input.IndexOf(")");
                            bool bracketReady = false;
                            do
                            {
                                bool done = false;
                                for (int i = openBracketIndex + 1; !done && i < closedBracketIndex; i++)
                                {
                                    if (input[i] == '(')
                                    {
                                        openBracketIndex = i;
                                        done = true;
                                    }
                                }
                                if (!done) bracketReady = true;
                            }
                            while (!bracketReady);

                            string before = input.Substring(0, openBracketIndex);
                            string bracket = input.Substring(openBracketIndex + 1, closedBracketIndex - openBracketIndex - 1);
                            string after = input.Substring(closedBracketIndex + 1);
                            Console.WriteLine($"Before bracket: {before}");
                            Console.WriteLine($"Bracket: {bracket}");
                            Console.WriteLine($"After bracket: {after}");
                            if (openBracketIndex > 0 && !checkCharacters(Convert.ToString(input[openBracketIndex - 1]), onlyOperatrors))
                            { // If no operator before opening bracket
                                before = before + "*";
                            }
                            if (closedBracketIndex < input.Length - 1 && !checkCharacters(Convert.ToString(input[closedBracketIndex + 1]), onlyOperatrors))
                            { // If no operator after closing bracket
                                after = "*" + after;
                            }
                            return calculate(before + calculate(bracket) + after);

                        }

                        if (input.Contains("+") || input.Contains("-"))
                        {
                            input = input.Replace("--", "+");

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

                            Console.WriteLine("Error: no power base entered. ");
                            errorList.Add("Error: no power base entered. ");
                            return -1;
                        }

                        else
                        {
                            Console.WriteLine("Error: Probably something is wrong with operators. ");
                            errorList.Add("Error: Probably something is wrong with operators. ");
                            return -1;
                        }
                    }

                }
                else
                {
                    Console.WriteLine("Error: not allowed characters detected. ");
                    errorList.Add("Error: not allowed characters detected. ");
                    return -1;
                }
                /*if (checkCharacters(input, onlyNumbers))
                {
                    return Convert.ToDouble(input);
                }*/


            }
            public string showResult()
            {
                result = calculate(expressionString);
                if(errorList.Count>0)
                {
                    string result = null;
                    foreach (string error in errorList)
                        result += error + "\n";
                    return result;
                }
                return Convert.ToString(result);
            }


        }
        public static char[] allowedCharacters =
            {
                '1','2','3','4','5','6','7','8','9','0',
                '+','-','*','/','^','(',')',' '
            };

        public static char[] onlyNumbers =
        {
                '1','2','3','4','5','6','7','8','9','0',
            };

        public static char[] onlyOperatrors =
        {
                '+','-','*','/','^','(',')'
            };
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

            input = input.Replace(" ", "");

            if (input == "") return 0;

            if (checkCharacters(input, allowedCharacters))
            {
                try
                {
                    return Convert.ToDouble(input);
                }
                catch (Exception)
                {
                    if (input.Contains("(") && input.Contains(")"))
                    {
                        int openBracketIndex = input.IndexOf("(");
                        int closedBracketIndex = input.IndexOf(")");
                        bool bracketReady = false;
                        do
                        {
                            bool done = false;
                            for (int i = openBracketIndex + 1; !done && i < closedBracketIndex; i++)
                            {
                                if (input[i] == '(')
                                {
                                    openBracketIndex = i;
                                    done = true;
                                }
                            }
                            if (!done) bracketReady = true;
                        }
                        while (!bracketReady);

                        string before = input.Substring(0, openBracketIndex);
                        string bracket = input.Substring(openBracketIndex + 1, closedBracketIndex - openBracketIndex - 1);
                        string after = input.Substring(closedBracketIndex + 1);
                        Console.WriteLine($"Before bracket: {before}");
                        Console.WriteLine($"Bracket: {bracket}");
                        Console.WriteLine($"After bracket: {after}");
                        if (openBracketIndex > 0 && !checkCharacters(Convert.ToString(input[openBracketIndex - 1]), onlyOperatrors))
                        { // If no operator before opening bracket
                            before = before + "*";
                        }
                        if (closedBracketIndex < input.Length - 1 && !checkCharacters(Convert.ToString(input[closedBracketIndex + 1]), onlyOperatrors))
                        { // If no operator after closing bracket
                            after = "*" + after;
                        }
                        return calculate(before + calculate(bracket) + after);

                    }

                    if (input.Contains("+") || input.Contains("-"))
                    {
                        input = input.Replace("--","+");

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

                        Console.WriteLine("Error: no power base entered. ");
                        return -1;
                    }

                    else
                    {
                        Console.WriteLine("Error: Probably something is wrong with operators. ");
                        return -1;
                    }
                }

            }
            else
            {
                Console.WriteLine("Error: not allowed characters detected. ");
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
            Expression e = new Expression(input);
            Console.WriteLine($"Result: {e.showResult()}");
        }
    }
}
