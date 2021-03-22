using System;
using System.Collections.Generic;
using System.Linq;

namespace Kalkulator2
{
    internal class Program
    {
        public static char[] onlyOperators =
        {
            '+','-','*','/','^','(',')','%'
        };

        public static char[] onlyNumbers =
        {
            '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', ','
        };

        public static char[] allowedCharacters = onlyOperators.Concat(onlyNumbers).ToArray();

        private class Expression
        {
            private string expressionString;
            private double result;
            private List<string> errorList = new List<string>();

            public Expression(string input)
            {
                expressionString = input;
            }

            private double calculate(string input)
            {
                /// Console.WriteLine($"Input: {input}");

                input = input.Replace(" ", "");

                if (input == "")
                {
                    errorList.Add("Error: Expression or part of it is empty");
                    return 0;
                }
                try
                {
                    return Convert.ToDouble(input);
                }
                catch (Exception)
                {
                    if (checkCharacters(input, allowedCharacters))
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
                            /// Console.WriteLine($"Before bracket: {before}");
                            /// Console.WriteLine($"Bracket: {bracket}");
                            /// Console.WriteLine($"After bracket: {after}");
                            if (openBracketIndex > 0 && !checkCharacters(Convert.ToString(input[openBracketIndex - 1]), onlyOperators))
                            { // If no operator before opening bracket
                                before = before + "*";
                            }
                            if (closedBracketIndex < input.Length - 1 && !checkCharacters(Convert.ToString(input[closedBracketIndex + 1]), onlyOperators))
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
                                double factor = calculate(arg2);
                                if(factor==0)
                                {
                                    errorList.Add("Error: Cannot divide by zero");
                                    return 0;
                                }
                                return calculate(arg1) / factor;
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

                            /// Console.WriteLine("Error: no power base entered. ");
                            errorList.Add("Error: no power base entered. ");
                            return 0;
                        }
                        else if (input.Contains("%"))
                        {
                            int percentIndex = input.IndexOf("%");
                            if (percentIndex == 0)
                            {
                                errorList.Add("Error with '%' operator");
                                return 0;
                            }
                            string beforePercent = input.Substring(0, percentIndex);
                            int lastOpIndex = 0;
                            foreach (char c in onlyOperators)
                            {
                                int bufor = beforePercent.LastIndexOf(c);
                                if (bufor > lastOpIndex) lastOpIndex = bufor;
                            }
                            if (lastOpIndex != 0 && percentIndex - lastOpIndex < 2)
                            {
                                errorList.Add("Error with '%' operator");
                                return 0;
                            }
                            string percent = input.Substring(lastOpIndex, percentIndex - lastOpIndex + 1);
                            string percentReplaced = percent.Replace("%", "/100");
                            input = input.Replace(percent, "(" + percentReplaced + ")");
                            return calculate(input);
                        }
                        else
                        {
                            /// Console.WriteLine("Error: Probably something is wrong with operators. ");
                            errorList.Add("Error: Probably something is wrong with operators. ");
                            return 0;
                        }
                    }
                    else
                    {
                        ///  Console.WriteLine("Error: not allowed characters detected. ");
                        errorList.Add("Error: not allowed characters detected. ");
                        return 0;
                    }
                }
            }

            public string showResult()
            {
                result = calculate(expressionString);
                if (errorList.Count > 0)
                {
                    string result = "Expression is not correct";
                    foreach (string error in errorList)
                        result += "\n" + error;
                    return result;
                }
                return Convert.ToString(Math.Round((decimal)result, 8));
            }
        }

        private static bool checkCharacters(string input, char[] allowedCharacters)
        {
            bool correct = true;
            foreach (char c in input)
            {
                correct = false;
                foreach (char d in allowedCharacters)
                {
                    if (c == d) correct = true;
                }
                if (!correct) return correct;
            }
            return correct;
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Expression calculator by Maciej Kmieciak\n");

            Console.WriteLine("Recognized operators are + - * / ^ %");
            Console.WriteLine("Standard order of operation applies");
            Console.WriteLine("You can also use brackets ( )");
            Console.WriteLine("Decimal point is , (comma)");
            Console.WriteLine("Spaces are ignored\n");

            Console.WriteLine("Example: 4,5(423 432 + 3^4) / 3");
            Console.WriteLine("Result: 635269,5\n");

            Console.Write("Your expression: ");
            string input = Console.ReadLine();

            Expression e = new Expression(input);

            Console.WriteLine($"Result: {e.showResult()}");
        }
    }
}