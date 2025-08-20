using System.Runtime.CompilerServices;

using System;
using System.Globalization;
using System.Text;

namespace OpetraViews.Controls
{
    /// <summary>Defines enumeration: InsertType</summary>

    public enum InsertType
    {
        BeforeSeparator,
        AfterSeparator
    }
    /// <summary>Represents: StringUtility</summary>

    public class StringUtility
    {

        /// <summary>Initializes a new instance of the StringUtility class</summary>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public StringUtility()
        {
        }


        /// <summary>Performs contains digits</summary>
        /// <param name="text">The text</param>
        /// <returns>The result of the contains digits</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool ContainsDigits(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (char.IsDigit(text[i]))
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>Gets decimal separator</summary>
        /// <param name="info">The info</param>
        /// <returns>The retrieved decimal separator</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetDecimalSeparator(CultureInfo info)
        {
            return info.NumberFormat.NumberDecimalSeparator;
        }


        /// <summary>Gets currency separator</summary>
        /// <param name="info">The info</param>
        /// <returns>The retrieved currency separator</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrencySeparator(CultureInfo info)
        {
            return info.NumberFormat.CurrencyDecimalSeparator;
        }


        /// <summary>Gets percentage separator</summary>
        /// <param name="info">The info</param>
        /// <returns>The retrieved percentage separator</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetPercentageSeparator(CultureInfo info)
        {
            return info.NumberFormat.PercentDecimalSeparator;
        }


        /// <summary>Gets separator position in text</summary>
        /// <param name="separator">The separator</param>
        /// <param name="text">The text</param>
        /// <returns>The retrieved separator position in text</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int GetSeparatorPositionInText(string separator, string text)
        {
            int result = -1;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].ToString().Equals(separator))
                {
                    result = i;
                    break;
                }
            }

            return result;
        }


        /// <summary>Performs build value from text</summary>
        /// <param name="str">The str</param>
        /// <param name="separator">The separator</param>
        /// <param name="builder">The builder</param>
        /// <param name="hasSeparator">Whether separator exists</param>
        /// <exception cref="System.ArgumentNullException">Thrown when hasSeparator is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void BuildValueFromText(string str, string separator, StringBuilder builder, bool hasSeparator)
        {
            for (int i = 0; i < str.Length; i++)
            {
                BuildCorrectCharacter(str, separator, builder, i, hasSeparator);
            }
        }


        /// <summary>Performs build correct character</summary>
        /// <param name="str">The str</param>
        /// <param name="separator">The separator</param>
        /// <param name="builder">The builder</param>
        /// <param name="i">The i</param>
        /// <param name="hasSeparator">Whether separator exists</param>
        /// <exception cref="System.ArgumentNullException">Thrown when i is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when hasSeparator is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void BuildCorrectCharacter(string str, string separator, StringBuilder builder, int i, bool hasSeparator)
        {
            if (char.IsDigit(str[i]))
            {
                builder.Append(str[i]);
            }
            else if (str[i].ToString().Equals(separator) && hasSeparator)
            {
                builder.Append(".");
            }
        }


        /// <summary>Gets selection position from value position</summary>
        /// <param name="valuePosition">The valuePosition</param>
        /// <param name="text">The text</param>
        /// <param name="separator">The separator</param>
        /// <param name="hasSeparator">Whether separator exists</param>
        /// <returns>The retrieved selection position from value position</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when valuePosition is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when hasSeparator is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int GetSelectionPositionFromValuePosition(int valuePosition, string text, string separator, bool hasSeparator)
        {
            int result = 0;
            int num = 0;
            for (int i = 0; i <= text.Length; i++)
            {
                if (num >= valuePosition)
                {
                    result = i;
                    break;
                }

                if (i < text.Length && (char.IsDigit(text[i]) || (hasSeparator && text[i].ToString().Equals(separator))))
                {
                    num++;
                }

                result = i;
            }

            return result;
        }


        /// <summary>Gets maximum value in decimal places</summary>
        /// <param name="decimalPlaces">The decimalPlaces</param>
        /// <returns>The retrieved maximum value in decimal places</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when decimalPlaces is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static long GetMaximumValueInDecimalPlaces(int decimalPlaces)
        {
            string text = "";
            for (int i = 0; i < decimalPlaces; i++)
            {
                text += "9";
            }

            return long.Parse(text, CultureInfo.InvariantCulture);
        }


        /// <summary>Gets selection in value</summary>
        /// <param name="selectionPosition">The selectionPosition</param>
        /// <param name="text">The text</param>
        /// <param name="separator">The separator</param>
        /// <param name="hasSeparator">Whether separator exists</param>
        /// <returns>The retrieved selection in value</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when selectionPosition is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when hasSeparator is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int GetSelectionInValue(int selectionPosition, string text, string separator, bool hasSeparator)
        {
            int num = 0;
            for (int i = 0; i < text.Length && i < selectionPosition; i++)
            {
                if (char.IsDigit(text[i]) || (hasSeparator && text[i].ToString().Equals(separator)))
                {
                    num++;
                }
            }

            return num;
        }


        /// <summary>Gets selection length in value</summary>
        /// <param name="selectionPosition">The selectionPosition</param>
        /// <param name="selectionLength">The selectionLength</param>
        /// <param name="text">The text</param>
        /// <param name="separator">The separator</param>
        /// <returns>The retrieved selection length in value</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when selectionPosition is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when selectionLength is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int GetSelectionLengthInValue(int selectionPosition, int selectionLength, string text, string separator)
        {
            int num = 0;
            for (int i = 0; i < text.Length && i < selectionPosition + selectionLength; i++)
            {
                if ((selectionLength > 0 && i >= selectionPosition && char.IsDigit(text[i])) || (i >= selectionPosition && text[i].ToString().Equals(separator)))
                {
                    num++;
                }
            }

            return num;
        }


        /// <summary>Gets string to separator</summary>
        /// <param name="text">The text</param>
        /// <param name="separator">The separator</param>
        /// <param name="hasSeparator">Whether separator exists</param>
        /// <returns>The retrieved string to separator</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when hasSeparator is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetStringToSeparator(string text, string separator, bool hasSeparator)
        {

            int separatorPositionInText = GetSeparatorPositionInText(separator, text);
            string text3 = text.Substring(0, separatorPositionInText);
            return GetValueString(text3, separator, hasSeparator);
        }


        /// <summary>Gets value string</summary>
        /// <param name="text">The text</param>
        /// <param name="separator">The separator</param>
        /// <param name="hasSeparator">Whether separator exists</param>
        /// <returns>The retrieved value string</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when hasSeparator is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetValueString(string text, string separator, bool hasSeparator)
        {

            StringBuilder stringBuilder = new StringBuilder();
            BuildValueFromText(text, separator, stringBuilder, hasSeparator);
            return stringBuilder.ToString();
        }


        /// <summary>Gets percentage value string</summary>
        /// <param name="text">The text</param>
        /// <param name="separator">The separator</param>
        /// <param name="hasSeparator">Whether separator exists</param>
        /// <returns>The retrieved percentage value string</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when hasSeparator is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetPercentageValueString(string text, string separator, bool hasSeparator)
        {
            string text2 = "";
            StringBuilder stringBuilder = new StringBuilder();
            BuildValueFromText(text, separator, stringBuilder, hasSeparator);
            text2 = stringBuilder.ToString();
            return MoveSeparatorTwoPositionsLeft(text2);
        }


        /// <summary>Performs move separator two positions right</summary>
        /// <param name="input">The input</param>
        /// <returns>The result of the move separator two positions right</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string MoveSeparatorTwoPositionsRight(string input)
        {
            return MoveSeparatorOnePositionRight(MoveSeparatorOnePositionRight(input));
        }


        /// <summary>Performs move separator one position right</summary>
        /// <param name="input">The input</param>
        /// <returns>The result of the move separator one position right</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string MoveSeparatorOnePositionRight(string input)
        {
            input = GetNewSeparatorPosition(input, out int separatorPosition, out int afterDotDigitPos);
            if (afterDotDigitPos < 0)
            {
                return input.Substring(0, separatorPosition) + '0' + input.Substring(separatorPosition);
            }

            return input.Substring(0, separatorPosition) + input.Substring(separatorPosition + 1, afterDotDigitPos - separatorPosition) + '.' + input.Substring(afterDotDigitPos + 1);
        }


        /// <summary>Gets new separator position</summary>
        /// <param name="input">The input</param>
        /// <param name="separatorPosition">The separatorPosition</param>
        /// <param name="afterDotDigitPos">The afterDotDigitPos</param>
        /// <returns>The retrieved new separator position</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when separatorPosition is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when afterDotDigitPos is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetNewSeparatorPosition(string input, out int separatorPosition, out int afterDotDigitPos)
        {
            separatorPosition = input.IndexOf('.');
            if (separatorPosition < 0)
            {
                separatorPosition = input.Length;
                input += '.';
            }

            char[] anyOf = new char[10]
            {
                '0',
                '1',
                '2',
                '3',
                '4',
                '5',
                '6',
                '7',
                '8',
                '9'
            };
            afterDotDigitPos = input.IndexOfAny(anyOf, separatorPosition);
            return input;
        }


        /// <summary>Performs move separator two positions left</summary>
        /// <param name="input">The input</param>
        /// <returns>The result of the move separator two positions left</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string MoveSeparatorTwoPositionsLeft(string input)
        {
            if (input.IndexOf('.') < 0)
            {
                input += '.';
            }

            string text = string.Empty;
            string text2 = input;
            foreach (char c in text2)
            {
                text = c + text;
            }

            string reversedResult = MoveSeparatorTwoPositionsRight(text);
            string empty = string.Empty;
            return GetResultInReversedOrder(reversedResult, empty);
        }


        /// <summary>Gets result in reversed order</summary>
        /// <param name="reversedResult">The reversedResult</param>
        /// <param name="result">The result</param>
        /// <returns>The retrieved result in reversed order</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetResultInReversedOrder(string reversedResult, string result)
        {
            foreach (char c in reversedResult)
            {
                result = c + result;
            }

            return result;
        }


        /// <summary>Gets digits count after separator</summary>
        /// <param name="separator">The separator</param>
        /// <param name="text">The text</param>
        /// <returns>The retrieved digits count after separator</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int GetDigitsCountAfterSeparator(string separator, string text)
        {
            int num = 0;
            int num2 = Math.Max(0, GetSeparatorPositionInText(separator, text));
            for (int i = num2; i < text.Length; i++)
            {
                if (char.IsDigit(text[i]))
                {
                    num++;
                }
            }

            return num;
        }


        /// <summary>Gets insert type</summary>
        /// <param name="positionInText">The positionInText</param>
        /// <param name="separator">The separator</param>
        /// <param name="text">The text</param>
        /// <returns>The retrieved insert type</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when positionInText is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static InsertType GetInsertType(int positionInText, string separator, string text)
        {
            InsertType result = InsertType.BeforeSeparator;
            int num = -1;
            num = GetSeparatorPositionInText(separator, text);
            if (num != -1 && num < positionInText)
            {
                result = InsertType.AfterSeparator;
            }

            return result;
        }


        /// <summary>Gets insert type by position in value</summary>
        /// <param name="positionInValue">The positionInValue</param>
        /// <param name="separator">The separator</param>
        /// <param name="text">The text</param>
        /// <param name="hasSeparator">Whether separator exists</param>
        /// <returns>The retrieved insert type by position in value</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when positionInValue is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when hasSeparator is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static InsertType GetInsertTypeByPositionInValue(int positionInValue, string separator, string text, bool hasSeparator)
        {
            InsertType result = InsertType.BeforeSeparator;
            string valueString = GetValueString(text, separator, hasSeparator);
            int digitsToSeparator = GetDigitsToSeparator(0, valueString);
            if (positionInValue > digitsToSeparator)
            {
                result = InsertType.AfterSeparator;
            }

            return result;
        }


        /// <summary>Gets digits to separator</summary>
        /// <param name="digitsToSeparator">The digitsToSeparator</param>
        /// <param name="valueString">The valueString</param>
        /// <returns>The retrieved digits to separator</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when digitsToSeparator is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int GetDigitsToSeparator(int digitsToSeparator, string valueString)
        {
            if (valueString.IndexOf('.') < 0)
            {
                return valueString.Length;
            }

            for (int i = 0; i < valueString.Length; i++)
            {
                if (valueString[i].ToString().Equals("."))
                {
                    digitsToSeparator = i;
                    break;
                }
            }

            return digitsToSeparator;
        }
    }
}
