using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace Lain.Xaml.Converters
{
    //https://rachel53461.wordpress.com/2011/08/20/the-math-converter/
    public class MathConverter : IValueConverter
    {
        private static readonly char[] AllOperators = {'+', '-', '*', '/', '%', '(', ')'};
        private static readonly List<string> Grouping = new List<string> {"(", ")"};
        private static readonly List<string> Operators = new List<string> {"+", "-", "*", "/", "%"};

        // Evaluates a mathematical string and keeps track of the results in a List<double> of numbers
        private void EvaluateMathString(ref string mathEquation, ref List<double> numbers, int index)
        {
            // Loop through each mathemtaical token in the equation
            var token = GetNextToken(mathEquation);

            while (token != string.Empty)
            {
                mathEquation = mathEquation.Remove(0, token.Length);

                if (Grouping.Contains(token))
                    switch (token)
                    {
                    case "(":
                        EvaluateMathString(ref mathEquation, ref numbers, index);
                        break;

                    case ")":
                        return;
                    }

                if (Operators.Contains(token))
                {
                    var nextToken = GetNextToken(mathEquation);
                    if (nextToken == "(")
                        EvaluateMathString(ref mathEquation, ref numbers, index + 1);

                    // Verify that enough numbers exist in the List<double> to complete the operation
                    // and that the next token is either the number expected, or it was a ( meaning
                    // that this was called recursively and that the number changed
                    double temp;
                    if (nextToken == "(" || numbers.Count > index + 1 &&
                        double.TryParse(nextToken, out temp) &&
                        (temp == numbers[index + 1]))
                    {
                        switch (token)
                        {
                        case "+":
                            numbers[index] = numbers[index] + numbers[index + 1];
                            break;
                        case "-":
                            numbers[index] = numbers[index] - numbers[index + 1];
                            break;
                        case "*":
                            numbers[index] = numbers[index] * numbers[index + 1];
                            break;
                        case "/":
                            numbers[index] = numbers[index] / numbers[index + 1];
                            break;
                        case "%":
                            numbers[index] = numbers[index] % numbers[index + 1];
                            break;
                        }
                        numbers.RemoveAt(index + 1);
                    }
                    else
                        // Handle Error - Next token is not the expected number
                        throw new FormatException("Next token is not the expected number");
                }

                token = GetNextToken(mathEquation);
            }
        }
        private string GetNextToken(string mathEquation)
        {
            if (mathEquation == string.Empty)
                return string.Empty;

            var tmp = string.Empty;
            foreach (var c in mathEquation)
            {
                if (AllOperators.Contains(c))
                    return tmp == string.Empty ? c.ToString() : tmp;
                tmp += c;
            }

            return tmp;
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Parse value into equation and remove spaces
            var mathEquation = (string)parameter;
            mathEquation = mathEquation.Replace(" ", string.Empty);
            mathEquation = mathEquation.Replace("x", value.ToString());

            // Validate values and get list of numbers in equation
            var numbers = new List<double>();

            foreach (var s in mathEquation.Split(AllOperators))
            {
                if (s == string.Empty)
                    continue;
                double tmp;
                if (double.TryParse(s, out tmp))
                    numbers.Add(tmp);
                else
                    // Handle Error - Some non-numeric, operator, or grouping character found in string
                    throw new InvalidCastException();
            }

            // Begin parsing method
            EvaluateMathString(ref mathEquation, ref numbers, 0);

            // After parsing the numbers list should only have one value - the total
            return numbers[0];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

        #endregion
    }
}