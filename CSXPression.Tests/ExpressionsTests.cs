using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;

namespace CSXPression.Tests
{
    public class Tests
    {
        #region Type testing

        #region Test cases for TypeTesting

        #region IntFormats
        [TestCase("45", typeof(int), Category = "IntFormats")]
        [TestCase("0", typeof(int), Category = "IntFormats")]
        [TestCase("-72346", typeof(int), Category = "IntFormats")]
        #endregion

        #region DoubleFormats
        [TestCase("123.54", typeof(double), Category = "DoubleFormats")]
        [TestCase("0.0", typeof(double), Category = "DoubleFormats")]
        [TestCase("0d", typeof(double), Category = "DoubleFormats")]
        [TestCase("-146.678", typeof(double), Category = "DoubleFormats")]
        [TestCase("123.54e-12", typeof(double), Category = "DoubleFormats")]
        [TestCase("-146.678e-12", typeof(double), Category = "DoubleFormats")]
        [TestCase("45d", typeof(double), Category = "DoubleFormats")]
        [TestCase("123.54e-12d", typeof(double), Category = "DoubleFormats")]
        [TestCase("-146.678e-12d", typeof(double), Category = "DoubleFormats")]
        #endregion

        #region FloatFormats
        [TestCase("45f", typeof(float), Category = "FloatFormats")]
        [TestCase("0f", typeof(float), Category = "FloatFormats")]
        [TestCase("-63f", typeof(float), Category = "FloatFormats")]
        [TestCase("123.54f", typeof(float), Category = "FloatFormats")]
        [TestCase("-146.678f", typeof(float), Category = "FloatFormats")]
        [TestCase("123.54e-12f", typeof(float), Category = "FloatFormats")]
        [TestCase("-146.678e-12f", typeof(float), Category = "FloatFormats")]
        #endregion

        #region UIntFormats
        [TestCase("45u", typeof(uint), Category = "UIntFormats")]
        [TestCase("0u", typeof(uint), Category = "UIntFormats")]
        [TestCase("123.54e6u", typeof(uint), Category = "UIntFormats")]
        #endregion

        #region LongFormats
        [TestCase("45l", typeof(long), Category = "LongFormats")]
        [TestCase("0l", typeof(long), Category = "LongFormats")]
        [TestCase("-63l", typeof(long), Category = "LongFormats")]
        [TestCase("123.54e6l", typeof(long), Category = "LongFormats")]
        #endregion

        #region ULongFormats
        [TestCase("45ul", typeof(ulong), Category = "ULongFormats")]
        [TestCase("0ul", typeof(ulong), Category = "ULongFormats")]
        [TestCase("123.54e6ul", typeof(ulong), Category = "ULongFormats")]
        #endregion

        #region DecimalFormats
        [TestCase("45m", typeof(decimal), Category = "DecimalFormats")]
        [TestCase("0m", typeof(decimal), Category = "DecimalFormats")]
        [TestCase("-63m", typeof(decimal), Category = "DecimalFormats")]
        [TestCase("123.54m", typeof(decimal), Category = "DecimalFormats")]
        [TestCase("-146.678m", typeof(decimal), Category = "DecimalFormats")]
        [TestCase("123.54e-12m", typeof(decimal), Category = "DecimalFormats")]
        [TestCase("-146.678e-12m", typeof(decimal), Category = "DecimalFormats")]
        #endregion

        // TODO List & Arrays
        //#region Lists & Arrays
        //[TestCase("Array(14, \"A text for test\", 2.5, true)", typeof(object[]), Category = "Standard Functions,Array Function")]
        //[TestCase("List(14, \"A text for test\", 2.5, true)", typeof(List<object>), Category = "Standard Functions,List Function")]
        //#endregion

        #endregion
        public void TypeTesting(string expression, Type type)
        {
            ExpressionEvaluator evaluator = new ExpressionEvaluator();

            evaluator.Evaluate(expression)
                .ShouldBeOfType(type);
        }

        #endregion

        #region Direct Expression Evaluation

        #region Test cases for DirectExpressionEvaluation

        #region Other bases numbers

        [TestCase("0xab", ExpectedResult = 0xab, Category = "HexNumber")]
        [TestCase("0xAB", ExpectedResult = 0xab, Category = "HexNumber")]
        [TestCase("0x1", ExpectedResult = 0x1, Category = "HexNumber")]
        [TestCase("0xf", ExpectedResult = 0xf, Category = "HexNumber")]
        [TestCase("-0xf", ExpectedResult = -0xf, Category = "HexNumber")]
        [TestCase("0xff_2a", ExpectedResult = 0xff_2a, Category = "HexNumber")]

        [TestCase("0b01100111", ExpectedResult = 0b01100111, Category = "BinaryNumber")]
        [TestCase("0b0100", ExpectedResult = 0b0100, Category = "BinaryNumber")]
        [TestCase("0b1010", ExpectedResult = 0b1010, Category = "BinaryNumber")]
        [TestCase("0b10_10", ExpectedResult = 0b10_10, Category = "BinaryNumber")]
        [TestCase("-0b10_10", ExpectedResult = -0b10_10, Category = "BinaryNumber")]

        #endregion

        // TODO null, bool

        //#region Null Expression

        //[TestCase("null", ExpectedResult = null, Category = "Null Expression")]

        //#endregion

        //#region Booleans Constants

        //[TestCase("true", ExpectedResult = true, Category = "Boolean Constants")]
        //[TestCase("false", ExpectedResult = false, Category = "Boolean Constants")]

        //#endregion

        #region String Operations

        [TestCase("\"Hello World\"", ExpectedResult = "Hello World", Category = "SimpleString")]
        [TestCase("\"Hello\" + \"World\"", ExpectedResult = "HelloWorld", Category = "SimpleString")]

        [TestCase("\"\\\"\"", ExpectedResult = "\"", Category = "StringEscape")]
        [TestCase("\"\\n\"", ExpectedResult = "\n", Category = "StringEscape")]
        [TestCase("\"\\r\"", ExpectedResult = "\r", Category = "StringEscape")]
        [TestCase("\"\\t\"", ExpectedResult = "\t", Category = "StringEscape")]
        [TestCase("\"" + @"\\" + "\"", ExpectedResult = @"\", Category = "StringEscape")]
        [TestCase("\"" + @"\\\n" + "\"", ExpectedResult = "\\\n", Category = "StringEscape")]
        [TestCase("@\"" + @"\\n" + "\"", ExpectedResult = @"\\n", Category = "StringEscape")]

        [TestCase("$\"Hello {1 + 2}\"", ExpectedResult = "Hello 3", Category = "StringInterpolation")]
        [TestCase("$\"{'\"'}\"", ExpectedResult = "\"", Category = "StringInterpolation")]
        [TestCase("$\"{ '\"' }\"", ExpectedResult = "\"", Category = "StringInterpolation")]
        [TestCase("$\"{{\"", ExpectedResult = "{", Category = "StringInterpolation")]
        [TestCase("$\"{ \"{\" }\"", ExpectedResult = "{", Category = "StringInterpolation")]
        [TestCase("$\"Test { 5+5 } Test\"", ExpectedResult = "Test 10 Test", Category = "StringInterpolation")]
        [TestCase("$\"Test { 5+5 + \" Test\" } Test\"", ExpectedResult = "Test 10 Test Test", Category = "StringInterpolation")]
        [TestCase("$\"Test { 5+5 + \" Test{\" } Test\"", ExpectedResult = "Test 10 Test{ Test", Category = "StringInterpolation")]
        [TestCase("$\"Test { 5+5 + \" Test{{ }\" } Test\"", ExpectedResult = "Test 10 Test{{ } Test", Category = "StringInterpolation")]

        [TestCase("$\"Hello { $\"TS\"}\"", ExpectedResult = "Hello TS", Category = "StringInterpolationInCascade")]
        [TestCase("$\"Hello { $\"T{{S\"}\"", ExpectedResult = "Hello T{S", Category = "StringInterpolationInCascade")]
        [TestCase("$\"Hello { $\"T}}S\"}\"", ExpectedResult = "Hello T}S", Category = "StringInterpolationInCascade")]
        [TestCase("$\"Hello { $\"T{1 + 2}\"}\"", ExpectedResult = "Hello T3", Category = "StringInterpolationInCascade")]
        [TestCase("$\"Hello { $\"T{1 + 2 + \"S\"}\"}\"", ExpectedResult = "Hello T3S", Category = "StringInterpolationInCascade")]
        [TestCase("$\"Hello { $\"T{1 + 2 + $\"S\"}\"}\"", ExpectedResult = "Hello T3S", Category = "StringInterpolationInCascade")]
        [TestCase("$\"Hello { $\"T{1 + 2 + $\"S{ 2 + 2 }\"}\"}\"", ExpectedResult = "Hello T3S4", Category = "StringInterpolationInCascade")]
        [TestCase("$\"Hello { $\"T{1 + 2 + $\"S{ 2 + 2 } Test\"}\"}\"", ExpectedResult = "Hello T3S4 Test", Category = "StringInterpolationInCascade")]
        [TestCase("$\"Hello { $\"T{1 + 2 + $\"S{ 2 + \" Test\" }\"}\"}\"", ExpectedResult = "Hello T3S2 Test", Category = "StringInterpolationInCascade")]
        [TestCase("$\"Hello { $\"T{1 + 2 + $\"S{ 2 + $\" Test\" }\"}\"}\"", ExpectedResult = "Hello T3S2 Test", Category = "StringInterpolationInCascade")]
        [TestCase("$\"Hello { $\"T{1 + 2 + $\"S{ 2 + $\" Test{ 2 + 2 }\" }\"}\"}\"", ExpectedResult = "Hello T3S2 Test4", Category = "StringInterpolationInCascade")]

        [TestCase("\"Hello\" + (\"Test\" + \"(\")", ExpectedResult = "HelloTest(", Category = "StringBetweenParenthis")]
        [TestCase("\"Hello\" + (\"Test\" + \")\")", ExpectedResult = "HelloTest)", Category = "StringBetweenParenthis")]

        // TODO : Func and method in string tests

        //[TestCase("\"Hello\" + (\"Test\" + $\"{ Abs(int.Parse(\"-4\"))}\")", ExpectedResult = "HelloTest4", Category = "StringWithParenthisOrComaInFunctionsArgs")]
        //[TestCase("\"Text()\".Replace(\"(\", \"x\")", ExpectedResult = "Textx)", Category = "StringWithParenthisOrComaInFunctionsArgs")]
        //[TestCase("\"Text()\".Replace(\"x\", \",\")", ExpectedResult = "Te,t()", Category = "StringWithParenthisOrComaInFunctionsArgs")]
        //[TestCase("\"Text()\".Replace(\"(\", \",\")", ExpectedResult = "Text,)", Category = "StringWithParenthisOrComaInFunctionsArgs")]

        //[TestCase("\"Hello,Test,What\".Split(ArrayOfType(typeof(char), ',')).Length", ExpectedResult = 3, Category = "StringSplit,ArrayOfType")]
        //[TestCase("\"Hello,Test,What\".Split(ArrayOfType(typeof(char), ',')).Json", ExpectedResult = "[\"Hello\",\"Test\",\"What\"]", Category = "StringSplit,ArrayOfType")]
        //[TestCase("\"Hello,Test,What\".Split(new char[]{','}).Length", ExpectedResult = 3, Category = "StringSplit,Array instanciation")]
        //[TestCase("\"Hello,Test,What\".Split(new char[]{','}).Json", ExpectedResult = "[\"Hello\",\"Test\",\"What\"]", Category = "StringSplit,Array instanciation")]

        #endregion

        #region Chars Operations

        [TestCase("'a'", ExpectedResult = 'a', Category = "char")]
        [TestCase("'g'", ExpectedResult = 'g', Category = "char")]
        [TestCase("'z'", ExpectedResult = 'z', Category = "char")]
        [TestCase("'A'", ExpectedResult = 'A', Category = "char")]
        [TestCase("'Q'", ExpectedResult = 'Q', Category = "char")]
        [TestCase("'Z'", ExpectedResult = 'Z', Category = "char")]
        [TestCase("'é'", ExpectedResult = 'é', Category = "char")]
        [TestCase("'è'", ExpectedResult = 'è', Category = "char")]
        [TestCase("'ô'", ExpectedResult = 'ô', Category = "char")]
        [TestCase("'ç'", ExpectedResult = 'ç', Category = "char")]
        [TestCase("'%'", ExpectedResult = '%', Category = "char")]
        [TestCase("'('", ExpectedResult = '(', Category = "char")]
        [TestCase("'\"'", ExpectedResult = '"', Category = "char")]
        [TestCase(@"'\\'", ExpectedResult = '\\', Category = "char")]
        [TestCase(@"'\''", ExpectedResult = '\'', Category = "char")]
        [TestCase(@"'\0'", ExpectedResult = '\0', Category = "char")]
        [TestCase(@"'\a'", ExpectedResult = '\a', Category = "char")]
        [TestCase(@"'\b'", ExpectedResult = '\b', Category = "char")]
        [TestCase(@"'\f'", ExpectedResult = '\f', Category = "char")]
        [TestCase(@"'\n'", ExpectedResult = '\n', Category = "char")]
        [TestCase(@"'\r'", ExpectedResult = '\r', Category = "char")]
        [TestCase(@"'\t'", ExpectedResult = '\t', Category = "char")]
        [TestCase(@"'\v'", ExpectedResult = '\v', Category = "char")]
        [TestCase("'\"'", ExpectedResult = '"', Category = "char")]

        // TODO string, Methods for char tests
        //[TestCase("\"hello\" + ' ' + '!'", ExpectedResult = "hello !", Category = "char")]
        //[TestCase("(int)'a'", ExpectedResult = 97, Category = "char")]
        //[TestCase("'a'.CompareTo('b')", ExpectedResult = -1, Category = "char")]
        //[TestCase("'a'.Equals('b')", ExpectedResult = false, Category = "char")]
        //[TestCase("'b'.Equals('b')", ExpectedResult = true, Category = "char")]
        //[TestCase("char.GetNumericValue('1')", ExpectedResult = 1, Category = "char")]
        //[TestCase("char.IsControl('\t')", ExpectedResult = true, Category = "char")]
        //[TestCase("char.IsDigit('f')", ExpectedResult = false, Category = "char")]
        //[TestCase("char.IsDigit('3')", ExpectedResult = true, Category = "char")]
        //[TestCase("char.IsLetter(',')", ExpectedResult = false, Category = "char")]
        //[TestCase("char.IsLetter('R')", ExpectedResult = true, Category = "char")]
        //[TestCase("char.IsLetter('h')", ExpectedResult = true, Category = "char")]
        //[TestCase("char.IsLower('u')", ExpectedResult = true, Category = "char")]
        //[TestCase("char.IsLower('U')", ExpectedResult = false, Category = "char")]
        //[TestCase("char.IsPunctuation('.')", ExpectedResult = true, Category = "char")]
        //[TestCase("char.IsSeparator(\"test string\", 4)", ExpectedResult = true, Category = "char")]
        //[TestCase("char.IsSymbol('+')", ExpectedResult = true, Category = "char")]
        //[TestCase("char.IsWhiteSpace(\"test string\", 4)", ExpectedResult = true, Category = "char")]
        //[TestCase("char.Parse(\"S\")", ExpectedResult = 'S', Category = "char")]
        //[TestCase("char.ToLower('M')", ExpectedResult = 'm', Category = "char")]
        //[TestCase("'x'.ToString()", ExpectedResult = "x", Category = "char")]

        #endregion

        #region SimpleAddition

        [TestCase("-60 + -10", ExpectedResult = -70, Category = "SimpleAddition")]
        [TestCase("-1 + -10", ExpectedResult = -11, Category = "SimpleAddition")]
        [TestCase("1 + -10", ExpectedResult = -9, Category = "SimpleAddition")]
        [TestCase("0 + -10", ExpectedResult = -10, Category = "SimpleAddition")]
        [TestCase("10 + -10", ExpectedResult = 0, Category = "SimpleAddition")]
        [TestCase("467 + -10", ExpectedResult = 457, Category = "SimpleAddition")]

        [TestCase("-60 + -1", ExpectedResult = -61, Category = "SimpleAddition")]
        [TestCase("-1 + -1", ExpectedResult = -2, Category = "SimpleAddition")]
        [TestCase("1 + -1", ExpectedResult = 0, Category = "SimpleAddition")]
        [TestCase("0 + -1", ExpectedResult = -1, Category = "SimpleAddition")]
        [TestCase("467 + -1", ExpectedResult = 466, Category = "SimpleAddition")]

        [TestCase("-1232 + 0", ExpectedResult = -1232, Category = "SimpleAddition")]
        [TestCase("-1 + 0", ExpectedResult = -1, Category = "SimpleAddition")]
        [TestCase("1 + 0", ExpectedResult = 1, Category = "SimpleAddition")]
        [TestCase("0 + 0", ExpectedResult = 0, Category = "SimpleAddition")]
        [TestCase("467 + 0", ExpectedResult = 467, Category = "SimpleAddition")]

        [TestCase("-60 + 1", ExpectedResult = -59, Category = "SimpleAddition")]
        [TestCase("-1 + 1", ExpectedResult = 0, Category = "SimpleAddition")]
        [TestCase("1 + 1", ExpectedResult = 2, Category = "SimpleAddition")]
        [TestCase("0 + 1", ExpectedResult = 1, Category = "SimpleAddition")]
        [TestCase("467 + 1", ExpectedResult = 468, Category = "SimpleAddition")]

        [TestCase("-60 + 10", ExpectedResult = -50, Category = "SimpleAddition")]
        [TestCase("-1 + 10", ExpectedResult = 9, Category = "SimpleAddition")]
        [TestCase("1 + 10", ExpectedResult = 11, Category = "SimpleAddition")]
        [TestCase("0 + 10", ExpectedResult = 10, Category = "SimpleAddition")]
        [TestCase("10 + 10", ExpectedResult = 20, Category = "SimpleAddition")]
        [TestCase("467 + 10", ExpectedResult = 477, Category = "SimpleAddition")]

        [TestCase("-60+-10", ExpectedResult = -70, Category = "SimpleAddition")]
        [TestCase("-1+-10", ExpectedResult = -11, Category = "SimpleAddition")]
        [TestCase("1+-10", ExpectedResult = -9, Category = "SimpleAddition")]
        [TestCase("0+-10", ExpectedResult = -10, Category = "SimpleAddition")]
        [TestCase("10+-10", ExpectedResult = 0, Category = "SimpleAddition")]
        [TestCase("467+-10", ExpectedResult = 457, Category = "SimpleAddition")]

        [TestCase("-60+-1", ExpectedResult = -61, Category = "SimpleAddition")]
        [TestCase("-1+-1", ExpectedResult = -2, Category = "SimpleAddition")]
        [TestCase("1+-1", ExpectedResult = 0, Category = "SimpleAddition")]
        [TestCase("0+-1", ExpectedResult = -1, Category = "SimpleAddition")]
        [TestCase("467+-1", ExpectedResult = 466, Category = "SimpleAddition")]

        [TestCase("-1232++0", ExpectedResult = -1232, Category = "SimpleAddition")]
        [TestCase("-1++0", ExpectedResult = -1, Category = "SimpleAddition")]
        [TestCase("1 ++0", ExpectedResult = 1, Category = "SimpleAddition")]
        [TestCase("0 + +0", ExpectedResult = 0, Category = "SimpleAddition")]
        [TestCase("467 + +0", ExpectedResult = 467, Category = "SimpleAddition")]

        [TestCase("-60 + +1", ExpectedResult = -59, Category = "SimpleAddition")]
        [TestCase("-1 + +1", ExpectedResult = 0, Category = "SimpleAddition")]
        [TestCase("1 ++1", ExpectedResult = 2, Category = "SimpleAddition")]
        [TestCase("0 ++1", ExpectedResult = 1, Category = "SimpleAddition")]
        [TestCase("467 ++1", ExpectedResult = 468, Category = "SimpleAddition")]

        [TestCase("-60 ++10", ExpectedResult = -50, Category = "SimpleAddition")]
        [TestCase("-1 ++10", ExpectedResult = 9, Category = "SimpleAddition")]
        [TestCase("1 ++10", ExpectedResult = 11, Category = "SimpleAddition")]
        [TestCase("0 + +10", ExpectedResult = 10, Category = "SimpleAddition")]
        [TestCase("10 + +10", ExpectedResult = 20, Category = "SimpleAddition")]
        [TestCase("467 + +10", ExpectedResult = 477, Category = "SimpleAddition")]

        #endregion

        #region SimpleSubstraction

        [TestCase("-60 - -10", ExpectedResult = -50, Category = "SimpleSubstraction")]
        [TestCase("-1 - -10", ExpectedResult = 9, Category = "SimpleSubstraction")]
        [TestCase("1 - -10", ExpectedResult = 11, Category = "SimpleSubstraction")]
        [TestCase("0 - -10", ExpectedResult = 10, Category = "SimpleSubstraction")]
        [TestCase("10 - -10", ExpectedResult = 20, Category = "SimpleSubstraction")]
        [TestCase("467 - -10", ExpectedResult = 477, Category = "SimpleSubstraction")]

        [TestCase("-60 - -1", ExpectedResult = -59, Category = "SimpleSubstraction")]
        [TestCase("-1 - -1", ExpectedResult = 0, Category = "SimpleSubstraction")]
        [TestCase("1 - -1", ExpectedResult = 2, Category = "SimpleSubstraction")]
        [TestCase("0 - -1", ExpectedResult = 1, Category = "SimpleSubstraction")]
        [TestCase("467 - -1", ExpectedResult = 468, Category = "SimpleSubstraction")]

        [TestCase("-1232 - 0", ExpectedResult = -1232, Category = "SimpleSubstraction")]
        [TestCase("-1 - 0", ExpectedResult = -1, Category = "SimpleSubstraction")]
        [TestCase("1 - 0", ExpectedResult = 1, Category = "SimpleSubstraction")]
        [TestCase("0 - 0", ExpectedResult = 0, Category = "SimpleSubstraction")]
        [TestCase("467 - 0", ExpectedResult = 467, Category = "SimpleSubstraction")]

        [TestCase("-60 - 1", ExpectedResult = -61, Category = "SimpleSubstraction")]
        [TestCase("-1 - 1", ExpectedResult = -2, Category = "SimpleSubstraction")]
        [TestCase("1 - 1", ExpectedResult = 0, Category = "SimpleSubstraction")]
        [TestCase("0 - 1", ExpectedResult = -1, Category = "SimpleSubstraction")]
        [TestCase("467 - 1", ExpectedResult = 466, Category = "SimpleSubstraction")]

        [TestCase("-60 - 10", ExpectedResult = -70, Category = "SimpleSubstraction")]
        [TestCase("-1 - 10", ExpectedResult = -11, Category = "SimpleSubstraction")]
        [TestCase("1 - 10", ExpectedResult = -9, Category = "SimpleSubstraction")]
        [TestCase("0 - 10", ExpectedResult = -10, Category = "SimpleSubstraction")]
        [TestCase("10 - 10", ExpectedResult = 0, Category = "SimpleSubstraction")]
        [TestCase("467 - 10", ExpectedResult = 457, Category = "SimpleSubstraction")]

        [TestCase("-60--10", ExpectedResult = -50, Category = "SimpleSubstraction")]
        [TestCase("-1--10", ExpectedResult = 9, Category = "SimpleSubstraction")]
        [TestCase("1--10", ExpectedResult = 11, Category = "SimpleSubstraction")]
        [TestCase("+0- -10", ExpectedResult = 10, Category = "SimpleSubstraction")]
        [TestCase("+10 --10", ExpectedResult = 20, Category = "SimpleSubstraction")]
        [TestCase("+467--10", ExpectedResult = 477, Category = "SimpleSubstraction")]

        [TestCase("-60 --1", ExpectedResult = -59, Category = "SimpleSubstraction")]
        [TestCase("-1 --1", ExpectedResult = 0, Category = "SimpleSubstraction")]
        [TestCase("+1 --1", ExpectedResult = 2, Category = "SimpleSubstraction")]
        [TestCase("+0 --1", ExpectedResult = 1, Category = "SimpleSubstraction")]
        [TestCase("467 --1", ExpectedResult = 468, Category = "SimpleSubstraction")]

        [TestCase("-1232 -0", ExpectedResult = -1232, Category = "SimpleSubstraction")]
        [TestCase("-1 - -0", ExpectedResult = -1, Category = "SimpleSubstraction")]
        [TestCase("+1 -+0", ExpectedResult = 1, Category = "SimpleSubstraction")]
        [TestCase("+0 --0", ExpectedResult = 0, Category = "SimpleSubstraction")]
        [TestCase("467 -+0", ExpectedResult = 467, Category = "SimpleSubstraction")]

        [TestCase("-60 -+1", ExpectedResult = -61, Category = "SimpleSubstraction")]
        [TestCase("-1 -+1", ExpectedResult = -2, Category = "SimpleSubstraction")]
        [TestCase("1 -+1", ExpectedResult = 0, Category = "SimpleSubstraction")]
        [TestCase("0 - +1", ExpectedResult = -1, Category = "SimpleSubstraction")]
        [TestCase("467 - +1", ExpectedResult = 466, Category = "SimpleSubstraction")]

        [TestCase("-60 - +10", ExpectedResult = -70, Category = "SimpleSubstraction")]
        [TestCase("-1 - +10", ExpectedResult = -11, Category = "SimpleSubstraction")]
        [TestCase("1 -+10", ExpectedResult = -9, Category = "SimpleSubstraction")]
        [TestCase("0 -+10", ExpectedResult = -10, Category = "SimpleSubstraction")]
        [TestCase("+10 -+10", ExpectedResult = 0, Category = "SimpleSubstraction")]
        [TestCase("+467-+10", ExpectedResult = 457, Category = "SimpleSubstraction")]

        #endregion

        #region SimpleMultiplication

        [TestCase("-12 * 5", ExpectedResult = -60, Category = "SimpleMultiplication")]
        [TestCase("-1 * 5", ExpectedResult = -5, Category = "SimpleMultiplication")]
        [TestCase("-1 * -23", ExpectedResult = 23, Category = "SimpleMultiplication")]
        [TestCase("-1 * 1", ExpectedResult = -1, Category = "SimpleMultiplication")]
        [TestCase("-1 * -1", ExpectedResult = 1, Category = "SimpleMultiplication")]
        [TestCase("0 * 440", ExpectedResult = 0, Category = "SimpleMultiplication")]
        [TestCase("0 * 0", ExpectedResult = 0, Category = "SimpleMultiplication")]
        [TestCase("67326 * 0", ExpectedResult = 0, Category = "SimpleMultiplication")]
        [TestCase("1 * 0", ExpectedResult = 0, Category = "SimpleMultiplication")]
        [TestCase("1 * 1", ExpectedResult = 1, Category = "SimpleMultiplication")]
        [TestCase("1 * 672", ExpectedResult = 672, Category = "SimpleMultiplication")]
        [TestCase("3 * 4", ExpectedResult = 12, Category = "SimpleMultiplication")]
        [TestCase("5.5 * 10", ExpectedResult = 55, Category = "SimpleMultiplication")]
        [TestCase("-6.12 * 10",  ExpectedResult = -61.2, Category = "SimpleMultiplication")]

        #endregion

        #region SimpleDivision

        [TestCase("-10 / 5", ExpectedResult = -2, Category = "SimpleMultiplication")]
        [TestCase("-6 / 6", ExpectedResult = -1, Category = "SimpleMultiplication")]
        [TestCase("-1 / 2", ExpectedResult = 0, Category = "SimpleMultiplication")]
        [TestCase("-1 / 2d",  ExpectedResult = -0.5, Category = "SimpleMultiplication")]
        [TestCase("0 / 2", ExpectedResult = 0, Category = "SimpleMultiplication")]
        [TestCase("2 / 1", ExpectedResult = 2, Category = "SimpleMultiplication")]
        [TestCase("1 / 1", ExpectedResult = 1, Category = "SimpleMultiplication")]
        [TestCase("500 / 10", ExpectedResult = 50, Category = "SimpleMultiplication")]
        [TestCase("6 / 4", ExpectedResult = 1, Category = "SimpleMultiplication")]
        [TestCase("6d / 4d",  ExpectedResult = 1.5, Category = "SimpleMultiplication")]
        [TestCase("5.5 / 2",  ExpectedResult = 2.75, Category = "SimpleMultiplication")]

        #endregion

        #region DivAndMultiplyPriorityOverSubAndAdd

        [TestCase("5 - 10 / 2", ExpectedResult = 0, Category = "DivAndMultiplyPriorityOverSubAndAdd")]
        [TestCase("5 + 10 / 2", ExpectedResult = 10, Category = "DivAndMultiplyPriorityOverSubAndAdd")]
        [TestCase("5 - 10 * 2", ExpectedResult = -15, Category = "DivAndMultiplyPriorityOverSubAndAdd")]
        [TestCase("5 + 10 * 2", ExpectedResult = 25, Category = "DivAndMultiplyPriorityOverSubAndAdd")]

        #endregion

        #region Parentheses Priority

        // TODO boolean values and default functions
        [TestCase("(5d - 10) / 2",  ExpectedResult = -2.5, Category = "ParenthesisPriority")]
        [TestCase("(5d + 10) / 2",  ExpectedResult = 7.5, Category = "ParenthesisPriority")]
        [TestCase("(5m - 10) / 2",  ExpectedResult = -2.5, Category = "ParenthesisPriority")]
        [TestCase("(5m + 10) / 2",  ExpectedResult = 7.5, Category = "ParenthesisPriority")]
        [TestCase("(5 - 10) * 2",  ExpectedResult = -10, Category = "ParenthesisPriority")]
        [TestCase("(5 + 10) * 2",  ExpectedResult = 30, Category = "ParenthesisPriority")]
        [TestCase("5 - (10 / 2)",  ExpectedResult = 0, Category = "ParenthesisPriority")]
        [TestCase("5 + (10 / 2)",  ExpectedResult = 10, Category = "ParenthesisPriority")]
        [TestCase("5 - (10 * 2)",  ExpectedResult = -15, Category = "ParenthesisPriority")]
        [TestCase("5 + (10 * 2)",  ExpectedResult = 25, Category = "ParenthesisPriority")]
        //[TestCase("3 + (2 * (5 - 3 - (Abs(-5) - 6)))",  ExpectedResult = 9, Category = "ParenthesisPriority")]
        //[TestCase("!(!false || false)",  ExpectedResult = false, Category = "ParenthesisPriority")]
        //[TestCase("!(!false || false) || !(!true && true)",  ExpectedResult = true, Category = "ParenthesisPriority")]

        [TestCase("+(5d - 10) / 2",  ExpectedResult = -2.5, Category = "ParenthesisPriority")]
        [TestCase("+(5d + 10) / 2",  ExpectedResult = 7.5, Category = "ParenthesisPriority")]
        [TestCase("-(5 - 10) * -2",  ExpectedResult = -10, Category = "ParenthesisPriority")]
        [TestCase("-(5 + 10) * 2",  ExpectedResult = -30, Category = "ParenthesisPriority")]
        [TestCase("5 - +(10 / 2)",  ExpectedResult = 0, Category = "ParenthesisPriority")]
        [TestCase("5 + +(10 / 2)",  ExpectedResult = 10, Category = "ParenthesisPriority")]
        [TestCase("5 -+(10 * 2)",  ExpectedResult = -15, Category = "ParenthesisPriority")]
        [TestCase("5 - -(10 * 2)",  ExpectedResult = 25, Category = "ParenthesisPriority")]
        //[TestCase("3 --(2 *+(5 - 3 - +(-Abs(-5) - 6)))",  ExpectedResult = 29, Category = "ParenthesisPriority")]

        #endregion

        #region BitwiseComplement

        [TestCase("~-10", ExpectedResult = 9, Category = "BitwiseComplement")]
        [TestCase("~-2", ExpectedResult = 1, Category = "BitwiseComplement")]
        [TestCase("~-1", ExpectedResult = 0, Category = "BitwiseComplement")]
        [TestCase("~0", ExpectedResult = -1, Category = "BitwiseComplement")]
        [TestCase("~1", ExpectedResult = -2, Category = "BitwiseComplement")]
        [TestCase("~2", ExpectedResult = -3, Category = "BitwiseComplement")]
        [TestCase("~10", ExpectedResult = -11, Category = "BitwiseComplement")]

        #endregion

        #region SimpleModulo

        [TestCase("-4 % 2", ExpectedResult = 0, Category = "SimpleModulo")]
        [TestCase("-3 % 2", ExpectedResult = -1, Category = "SimpleModulo")]
        [TestCase("-2 % 2", ExpectedResult = 0, Category = "SimpleModulo")]
        [TestCase("-1 % 2", ExpectedResult = -1, Category = "SimpleModulo")]
        [TestCase("0 % 2", ExpectedResult = 0, Category = "SimpleModulo")]
        [TestCase("1 % 2", ExpectedResult = 1, Category = "SimpleModulo")]
        [TestCase("2 % 2", ExpectedResult = 0, Category = "SimpleModulo")]
        [TestCase("3 % 2", ExpectedResult = 1, Category = "SimpleModulo")]
        [TestCase("4 % 2", ExpectedResult = 0, Category = "SimpleModulo")]
        [TestCase("5 % 2", ExpectedResult = 1, Category = "SimpleModulo")]
        [TestCase("6 % 2", ExpectedResult = 0, Category = "SimpleModulo")]
        [TestCase("7 % 2", ExpectedResult = 1, Category = "SimpleModulo")]
        [TestCase("8 % 2", ExpectedResult = 0, Category = "SimpleModulo")]

        [TestCase("-6 % 3", ExpectedResult = 0, Category = "SimpleModulo")]
        [TestCase("-5 % 3", ExpectedResult = -2, Category = "SimpleModulo")]
        [TestCase("-4 % 3", ExpectedResult = -1, Category = "SimpleModulo")]
        [TestCase("-3 % 3", ExpectedResult = 0, Category = "SimpleModulo")]
        [TestCase("-2 % 3", ExpectedResult = -2, Category = "SimpleModulo")]
        [TestCase("-1 % 3", ExpectedResult = -1, Category = "SimpleModulo")]
        [TestCase("0 % 3", ExpectedResult = 0, Category = "SimpleModulo")]
        [TestCase("1 % 3", ExpectedResult = 1, Category = "SimpleModulo")]
        [TestCase("2 % 3", ExpectedResult = 2, Category = "SimpleModulo")]
        [TestCase("3 % 3", ExpectedResult = 0, Category = "SimpleModulo")]
        [TestCase("4 % 3", ExpectedResult = 1, Category = "SimpleModulo")]
        [TestCase("5 % 3", ExpectedResult = 2, Category = "SimpleModulo")]
        [TestCase("6 % 3", ExpectedResult = 0, Category = "SimpleModulo")]

        #endregion

        #region Boolean Tests Operators

        [TestCase("1 < 5", ExpectedResult = true, Category = "LowerThanBooleanOperator")]
        [TestCase("5 < 5", ExpectedResult = false, Category = "LowerThanBooleanOperator")]
        [TestCase("7 < 5", ExpectedResult = false, Category = "LowerThanBooleanOperator")]

        [TestCase("1 > 5", ExpectedResult = false, Category = "GreaterThanBooleanOperator")]
        [TestCase("5 > 5", ExpectedResult = false, Category = "GreaterThanBooleanOperator")]
        [TestCase("7 > 5", ExpectedResult = true, Category = "GreaterThanBooleanOperator")]

        [TestCase("1 <= 5", ExpectedResult = true, Category = "LowerThanOrEqualBooleanOperator")]
        [TestCase("5 <= 5", ExpectedResult = true, Category = "LowerThanOrEqualBooleanOperator")]
        [TestCase("7 <= 5", ExpectedResult = false, Category = "LowerThanOrEqualBooleanOperator")]

        [TestCase("1 >= 5", ExpectedResult = false, Category = "GreaterThanOrEqualBooleanOperator")]
        [TestCase("5 >= 5", ExpectedResult = true, Category = "GreaterThanOrEqualBooleanOperator")]
        [TestCase("7 >= 5", ExpectedResult = true, Category = "GreaterThanOrEqualBooleanOperator")]

        [TestCase("1 == 5", ExpectedResult = false, Category = "EqualBooleanOperator")]
        [TestCase("5 == 5", ExpectedResult = true, Category = "EqualBooleanOperator")]
        [TestCase("7 == 5", ExpectedResult = false, Category = "EqualBooleanOperator")]

        [TestCase("1 != 5", ExpectedResult = true, Category = "NotEqualBooleanOperator")]
        [TestCase("5 != 5", ExpectedResult = false, Category = "NotEqualBooleanOperator")]
        [TestCase("7 != 5", ExpectedResult = true, Category = "NotEqualBooleanOperator")]

        // TODO more complex boolean operations
        //[TestCase("1 is string", ExpectedResult = false, Category = "IsOperatorBooleanOperator")]
        //[TestCase("\"Test\" is string", ExpectedResult = true, Category = "IsOperatorBooleanOperator")]
        //[TestCase("true is string", ExpectedResult = false, Category = "IsOperatorBooleanOperator")]
        //[TestCase("null is string", ExpectedResult = false, Category = "IsOperatorBooleanOperator")]

        //[TestCase("true && true", ExpectedResult = true, Category = "ConditionalAndBooleanOperator")]
        //[TestCase("false && true", ExpectedResult = false, Category = "ConditionalAndBooleanOperator")]
        //[TestCase("true && false", ExpectedResult = false, Category = "ConditionalAndBooleanOperator")]
        //[TestCase("false && false", ExpectedResult = false, Category = "ConditionalAndBooleanOperator")]

        //[TestCase("true || true", ExpectedResult = true, Category = "ConditionalOrBooleanOperator")]
        //[TestCase("false || true", ExpectedResult = true, Category = "ConditionalOrBooleanOperator")]
        //[TestCase("true || false", ExpectedResult = true, Category = "ConditionalOrBooleanOperator")]
        //[TestCase("false || false", ExpectedResult = false, Category = "ConditionalOrBooleanOperator")]

        //[TestCase("!true", ExpectedResult = false, Category = "NegationBooleanOperator")]
        //[TestCase("!false", ExpectedResult = true, Category = "ConditionalOrBooleanOperator")]
        //[TestCase("!(5 > 2)", ExpectedResult = false, Category = "ConditionalOrBooleanOperator")]
        //[TestCase("!(5 < 2)", ExpectedResult = true, Category = "ConditionalOrBooleanOperator")]

        #endregion

        // TODO Null coalescing and conditional, default, typeof, sizeof, new keyword

        //#region Null Coalescing Operator

        //[TestCase("\"Option1\" ?? \"Option2\"", ExpectedResult = "Option1", Category = "Null Coalescing Operator")]
        //[TestCase("null ?? \"Option2\"", ExpectedResult = "Option2", Category = "Null Coalescing Operator")]

        //#endregion

        //#region Null conditional Operator

        //[TestCase("null?.Trim()", ExpectedResult = null, Category = "Null conditional Operator")]

        //#endregion

        //#region default values

        //[TestCase("default(int)", ExpectedResult = 0, Category = "default values")]
        //[TestCase("default(bool)", ExpectedResult = false, Category = "default values")]
        //[TestCase("default(System.Boolean)", ExpectedResult = false, Category = "default values, Inline namespaces")]

        //#endregion

        //#region typeof keyword

        //[TestCase("typeof(int)", ExpectedResult = typeof(int), Category = "typeof keyword")]
        //[TestCase("typeof(float)", ExpectedResult = typeof(float), Category = "typeof keyword")]
        //[TestCase("typeof(string)", ExpectedResult = typeof(string), Category = "typeof keyword")]
        //[TestCase("typeof(Regex)", ExpectedResult = typeof(Regex), Category = "typeof keyword")]
        //[TestCase("typeof(System.Text.RegularExpressions.Regex)", ExpectedResult = typeof(Regex), Category = "typeof keyword,inline namespace")]
        //[TestCase("typeof(string) == \"Hello\".GetType()", ExpectedResult = true, Category = "typeof keyword")]
        //[TestCase("typeof(int) == 12.GetType()", ExpectedResult = true, Category = "typeof keyword")]
        //[TestCase("typeof(string) == 12.GetType()", ExpectedResult = false, Category = "typeof keyword")]

        //#endregion

        //#region sizeof keyword

        //[TestCase("sizeof(sbyte)", ExpectedResult = sizeof(sbyte), Category = "sizeof keyword")]
        //[TestCase("sizeof(byte)", ExpectedResult = sizeof(byte), Category = "sizeof keyword")]
        //[TestCase("sizeof(short)", ExpectedResult = sizeof(short), Category = "sizeof keyword")]
        //[TestCase("sizeof(ushort)", ExpectedResult = sizeof(ushort), Category = "sizeof keyword")]
        //[TestCase("sizeof(int)", ExpectedResult = sizeof(int), Category = "sizeof keyword")]
        //[TestCase("sizeof(uint)", ExpectedResult = sizeof(uint), Category = "sizeof keyword")]
        //[TestCase("sizeof(long)", ExpectedResult = sizeof(long), Category = "sizeof keyword")]
        //[TestCase("sizeof(ulong)", ExpectedResult = sizeof(ulong), Category = "sizeof keyword")]
        //[TestCase("sizeof(char)", ExpectedResult = sizeof(char), Category = "sizeof keyword")]
        //[TestCase("sizeof(float)", ExpectedResult = sizeof(float), Category = "sizeof keyword")]
        //[TestCase("sizeof(double)", ExpectedResult = sizeof(double), Category = "sizeof keyword")]
        //[TestCase("sizeof(decimal)", ExpectedResult = sizeof(decimal), Category = "sizeof keyword")]
        //[TestCase("sizeof(bool)", ExpectedResult = sizeof(bool), Category = "sizeof keyword")]

        //#endregion

        //#region Create instance with new Keyword

        //[TestCase("new ClassForTest1().GetType()", ExpectedResult = typeof(ClassForTest1), Category = "Create instance with new Keyword")]
        //[TestCase("new ClassForTest2(15).GetType()", ExpectedResult = typeof(ClassForTest2), Category = "Create instance with new Keyword")]
        //[TestCase("new ClassForTest2(15).Value1", ExpectedResult = 15, Category = "Create instance with new Keyword")]
        //[TestCase("new CodingSeb.ExpressionEvaluator.Tests.OtherNamespace.ClassInOtherNameSpace1().Value1", ExpectedResult = 26, Category = "Create instance with new Keyword,Inline namespace")]
        //[TestCase("new Regex(@\"\\w*[n]\\w*\").Match(\"Which word contains the desired letter ?\").Value", ExpectedResult = "contains", Category = "Create instance with new Keyword")]
        //[TestCase("new List<string>(){ \"Hello\", \"Test\" }.GetType()", ExpectedResult = typeof(List<string>), Category = "Create instance with new Keyword, Collection Initializer")]
        //[TestCase("new List<string>(){ \"Hello\", \"Test\" }.Count", ExpectedResult = 2, Category = "Create instance with new Keyword, Collection Initializer")]
        //[TestCase("new List<string>(){ \"Hello\", \"Test\" }[0]", ExpectedResult = "Hello", Category = "Create instance with new Keyword, Collection Initializer")]
        //[TestCase("new List<string>(){ \"Hello\", \"Test\" }[1]", ExpectedResult = "Test", Category = "Create instance with new Keyword, Collection Initializer")]
        //[TestCase("new List<string>{ \"Hello\", \"Test\" }.GetType()", ExpectedResult = typeof(List<string>), Category = "Create instance with new Keyword, Collection Initializer")]
        //[TestCase("new List<string>{ \"Hello\", \"Test\" }.Count", ExpectedResult = 2, Category = "Create instance with new Keyword, Collection Initializer")]
        //[TestCase("new List<string>{ \"Hello\", \"Test\" }[0]", ExpectedResult = "Hello", Category = "Create instance with new Keyword, Collection Initializer")]
        //[TestCase("new List<string>{ \"Hello\", \"Test\" }[1]", ExpectedResult = "Test", Category = "Create instance with new Keyword, Collection Initializer")]
        //[TestCase("new ClassForTest1(){ IntProperty = 100, StringProperty = \"A Text\" }.GetType()", ExpectedResult = typeof(ClassForTest1), Category = "Create instance with new Keyword, Object Initializer")]
        //[TestCase("new ClassForTest1(){ IntProperty = 100, StringProperty = \"A Text\" }.IntProperty", ExpectedResult = 100, Category = "Create instance with new Keyword, Object Initializer")]
        //[TestCase("new ClassForTest1(){ IntProperty = 100, StringProperty = \"A Text\" }.StringProperty", ExpectedResult = "A Text", Category = "Create instance with new Keyword, Object Initializer")]
        //[TestCase("new ClassForTest1{ IntProperty = 100, StringProperty = \"A Text\" }.GetType()", ExpectedResult = typeof(ClassForTest1), Category = "Create instance with new Keyword, Object Initializer")]
        //[TestCase("new ClassForTest1{ IntProperty = 100, StringProperty = \"A Text\" }.IntProperty", ExpectedResult = 100, Category = "Create instance with new Keyword, Object Initializer")]
        //[TestCase("new ClassForTest1{ IntProperty = 100, StringProperty = \"A Text\" }.StringProperty", ExpectedResult = "A Text", Category = "Create instance with new Keyword, Object Initializer")]
        //[TestCase("new ClassForTest2(10){ Value2 = 100 }.GetType()", ExpectedResult = typeof(ClassForTest2), Category = "Create instance with new Keyword, Object Initializer")]
        //[TestCase("new ClassForTest2(10){ Value2 = 100 }.Value1", ExpectedResult = 10, Category = "Create instance with new Keyword, Object Initializer")]
        //[TestCase("new ClassForTest2(10){ Value2 = 100 }.Value2", ExpectedResult = 100, Category = "Create instance with new Keyword, Object Initializer")]
        //[TestCase("new Dictionary<int, string>(){ [7] = \"seven\", [7+2] = \"nine\" }.GetType()", ExpectedResult = typeof(Dictionary<int, string>), Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<int, string>(){ [7] = \"seven\", [7+2] = \"nine\" }[7]", ExpectedResult = "seven", Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<int, string>(){ [7] = \"seven\", [7+2] = \"nine\" }[9]", ExpectedResult = "nine", Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<int, string>{ [7] = \"seven\", [7+2] = \"nine\" }.GetType()", ExpectedResult = typeof(Dictionary<int, string>), Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<int, string>{ [7] = \"seven\", [7+2] = \"nine\" }[7]", ExpectedResult = "seven", Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<int, string>{ [7] = \"seven\", [7+2] = \"nine\" }[9]", ExpectedResult = "nine", Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<string, int>(){ [\"seven\"] = 7, [\"nine\"] = 9 }.GetType()", ExpectedResult = typeof(Dictionary<string, int>), Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<string, int>(){ [\"seven\"] = 7, [\"nine\"] = 9 }[\"seven\"]", ExpectedResult = 7, Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<string, int>(){ [\"seven\"] = 7, [\"nine\"] = 9 }[\"nine\"]", ExpectedResult = 9, Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<string, int>{ [\"seven\"] = 7, [\"nine\"] = 9 }.GetType()", ExpectedResult = typeof(Dictionary<string, int>), Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<string, int>{ [\"seven\"] = 7, [\"nine\"] = 9 }[\"seven\"]", ExpectedResult = 7, Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<string, int>{ [\"seven\"] = 7, [\"nine\"] = 9 }[\"nine\"]", ExpectedResult = 9, Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<int, string>(){ {7 ,\"seven\"}, {7+2, \"nine\"} }.GetType()", ExpectedResult = typeof(Dictionary<int, string>), Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<int, string>(){ {7 ,\"seven\"}, {7+2, \"nine\"} }[7]", ExpectedResult = "seven", Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<int, string>(){ {7 ,\"seven\"}, {7+2, \"nine\"}  }[9]", ExpectedResult = "nine", Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<int, string>{ {7 ,\"seven\"}, {7+2, \"nine\"}  }.GetType()", ExpectedResult = typeof(Dictionary<int, string>), Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<int, string>{ {7 ,\"seven\"}, {7+2, \"nine\"}  }[7]", ExpectedResult = "seven", Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<int, string>{ {7 ,\"seven\"}, {7+2, \"nine\"} }[9]", ExpectedResult = "nine", Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<string, int>(){ {\"seven\", 7} , {\"nine\", 9 } }.GetType()", ExpectedResult = typeof(Dictionary<string, int>), Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<string, int>(){ {\"seven\", 7} , {\"nine\", 9 } }[\"seven\"]", ExpectedResult = 7, Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<string, int>(){ {\"seven\", 7} , {\"nine\", 9 } }[\"nine\"]", ExpectedResult = 9, Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<string, int>{ {\"seven\", 7} , {\"nine\", 9 }  }.GetType()", ExpectedResult = typeof(Dictionary<string, int>), Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<string, int>{ {\"seven\", 7} , {\"nine\", 9 }  }[\"seven\"]", ExpectedResult = 7, Category = "Create instance with new Keyword, Dictionary Initializer")]
        //[TestCase("new Dictionary<string, int>{ {\"seven\", 7} , {\"nine\", 9 }  }[\"nine\"]", ExpectedResult = 9, Category = "Create instance with new Keyword, Dictionary Initializer")]

        //#endregion

        #region Logical And Shift Operators

        [TestCase("2 & 8", ExpectedResult = 0, Category = "LogicalAndOperator")]
        [TestCase("10 & 8", ExpectedResult = 8, Category = "LogicalAndOperator")]

        [TestCase("2 ^ 8", ExpectedResult = 10, Category = "LogicalXorOperator")]
        [TestCase("10 ^ 8", ExpectedResult = 2, Category = "LogicalXorOperator")]

        [TestCase("2 | 8", ExpectedResult = 10, Category = "LogicalOrOperator")]
        [TestCase("10 | 8", ExpectedResult = 10, Category = "LogicalOrOperator")]

        [TestCase("1 << 2", ExpectedResult = 4, Category = "ShiftLeftOperator")]
        [TestCase("2 << 2", ExpectedResult = 8, Category = "ShiftLeftOperator")]

        [TestCase("4 >> 2", ExpectedResult = 1, Category = "ShiftRightOperator")]
        [TestCase("8 >> 2", ExpectedResult = 2, Category = "ShiftRightOperator")]

        #endregion

        // TODO ternary conditional, Math const, lambda, standard func, generic types, complex expressions and bugs corrections

        //#region Conditional Operator t ? x : y
        //[TestCase("true ? \"Test gives yes\" : \"Test gives no\"", ExpectedResult = "Test gives yes", Category = "Conditional Operator t ? x : y")]
        //[TestCase("false ? \"Test gives yes\" : \"Test gives no\"", ExpectedResult = "Test gives no", Category = "Conditional Operator t ? x : y")]
        //[TestCase("4 < 5 ? \"Test gives yes\" : \"Test gives no\"", ExpectedResult = "Test gives yes", Category = "Conditional Operator t ? x : y")]
        //[TestCase("4 > 5 ? \"Test gives yes\" : \"Test gives no\"", ExpectedResult = "Test gives no", Category = "Conditional Operator t ? x : y")]
        //[TestCase("Abs(-4) < 10 / 2 ? \"Test gives yes\" : \"Test gives no\"", ExpectedResult = "Test gives yes", Category = "Conditional Operator t ? x : y")]
        //[TestCase("Abs(-4) > 10 / 2 ? \"Test gives yes\" : \"Test gives no\"", ExpectedResult = "Test gives no", Category = "Conditional Operator t ? x : y")]
        //[TestCase("Abs(-4) < 10 / 2 ? Abs(-3) : (Abs(-4) + 4) / 2", ExpectedResult = 3, Category = "Conditional Operator t ? x : y")]
        //[TestCase("Abs(-4) > 10 / 2 ? Abs(-3) : (Abs(-4) + 4) / 2", ExpectedResult = 4, Category = "Conditional Operator t ? x : y")]
        //[TestCase("Abs(-4) < 10 / 2 ? (true ? 6 : 3+2) : (false ? Abs(-18) : 100 / 2)", ExpectedResult = 6, Category = "Conditional Operator t ? x : y")]
        //[TestCase("Abs(-4) > 10 / 2 ? (true ? 6 : 3+2) : (false ? Abs(-18) : 100 / 2)", ExpectedResult = 50, Category = "Conditional Operator t ? x : y")]
        //[TestCase("Abs(-4) > 10 / 2?(true ? 6 : 3+2):(false?Abs(-18):100 / 2)", ExpectedResult = 50, Category = "Conditional Operator t ? x : y")]
        //[TestCase("1==1?true:false", ExpectedResult = true, Category = "Conditional Operator t ? x : y")]
        //#endregion

        //#region Math Constants
        //[TestCase("Pi",  ExpectedResult = Math.PI, Category = "Math Constants")]
        //[TestCase("E",  ExpectedResult = Math.E, Category = "Math Constants")]
        //[TestCase("+Pi",  ExpectedResult = +Math.PI, Category = "Math Constants,Unary +")]
        //[TestCase("+E",  ExpectedResult = +Math.E, Category = "Math Constants,Unary +")]
        //[TestCase("-Pi",  ExpectedResult = -Math.PI, Category = "Math Constants,Unary -")]
        //[TestCase("-E",  ExpectedResult = -Math.E, Category = "Math Constants,Unary -")]
        //[TestCase("-Pi + +Pi",  ExpectedResult = 0, Category = "Math Constants,Unary -")]
        //[TestCase("-E - -E",  ExpectedResult = 0, Category = "Math Constants,Unary -")]
        //#endregion

        //#region Lambda functions
        //[TestCase("((x, y) => x * y)(4, 2)", ExpectedResult = 8, Category = "Lambda Functions")]
        //#endregion

        //#region Standard Functions

        //#region Abs Function
        //[TestCase("Abs(-50)", ExpectedResult = 50, Category = "Standard Functions,Abs Function")]
        //[TestCase("Abs(-19)", ExpectedResult = 19, Category = "Standard Functions,Abs Function")]
        //[TestCase("Abs(-3.5)", ExpectedResult = 3.5, Category = "Standard Functions,Abs Function")]
        //[TestCase("Abs(0)", ExpectedResult = 0, Category = "Standard Functions,Abs Function")]
        //[TestCase("Abs(1)", ExpectedResult = 1, Category = "Standard Functions,Abs Function")]
        //[TestCase("Abs(4.2)", ExpectedResult = 4.2, Category = "Standard Functions,Abs Function")]
        //[TestCase("Abs(10)", ExpectedResult = 10, Category = "Standard Functions,Abs Function")]
        //[TestCase("Abs(60)", ExpectedResult = 60, Category = "Standard Functions,Abs Function")]

        //[TestCase("-30 + Abs(-30)", ExpectedResult = 0, Category = "Standard Functions,Abs Function")]
        //[TestCase("-5.5 + Abs(-5.5)", ExpectedResult = 0, Category = "Standard Functions,Abs Function")]
        //[TestCase("-1 + Abs(-1)", ExpectedResult = 0, Category = "Standard Functions,Abs Function")]
        //[TestCase("0 + Abs(0)", ExpectedResult = 0, Category = "Standard Functions,Abs Function")]
        //[TestCase("1 + Abs(1)", ExpectedResult = 2, Category = "Standard Functions,Abs Function")]
        //[TestCase("5 + Abs(5)", ExpectedResult = 10, Category = "Standard Functions,Abs Function")]
        //[TestCase("2.5 + Abs(2.5)", ExpectedResult = 5, Category = "Standard Functions,Abs Function")]

        //[TestCase("Abs(-10 - 5)", ExpectedResult = 15, Category = "Standard Functions,Abs Function")]
        //#endregion

        //#region Acos Function
        //[TestCase("Acos(-1)", ExpectedResult = Math.PI, Category = "Standard Functions,Acos Function")]
        //[TestCase("Acos(0)", ExpectedResult = 1.5707963267948966d, Category = "Standard Functions,Acos Function")]
        //[TestCase("Acos(0.5)", ExpectedResult = 1.0471975511965979d, Category = "Standard Functions,Acos Function")]
        //[TestCase("Acos(1)", ExpectedResult = 0, Category = "Standard Functions,Acos Function")]
        //[TestCase("Acos(2)", ExpectedResult = Double.NaN, Category = "Standard Functions,Acos Function")]
        //#endregion

        //#region Array Function
        //[TestCase("Array(14, \"A text for test\", 2.5, true).Length", ExpectedResult = 4, Category = "Standard Functions,Array Function,Instance Property")]
        //[TestCase("Array(14, \"A text for test\", 2.5, true)[0]", ExpectedResult = 14, Category = "Standard Functions,Array Function,Indexing")]
        //[TestCase("Array(14, \"A text for test\", 2.5, true)[1]", ExpectedResult = "A text for test", Category = "Standard Functions,Array Function,Indexing")]
        //[TestCase("Array(14, \"A text for test\", 2.5, true)[2]", ExpectedResult = 2.5, Category = "Standard Functions,Array Function,Indexing")]
        //[TestCase("Array(14, \"A text for test\", 2.5, true)[3]", ExpectedResult = true, Category = "Standard Functions,Array Function,Indexing")]
        //#endregion

        //#region Asin Function
        //[TestCase("Asin(-1)", ExpectedResult = -1.5707963267948966d, Category = "Standard Functions,Asin Function")]
        //[TestCase("Asin(0)", ExpectedResult = 0, Category = "Standard Functions,Asin Function")]
        //[TestCase("Asin(0.5)", ExpectedResult = 0.52359877559829893d, Category = "Standard Functions,Asin Function")]
        //[TestCase("Asin(1)", ExpectedResult = 1.5707963267948966d, Category = "Standard Functions,Asin Function")]
        //[TestCase("Asin(2)", ExpectedResult = Double.NaN, Category = "Standard Functions,Asin Function")]
        //#endregion

        //#region Atan Function
        //[TestCase("Atan(-Pi)", ExpectedResult = -1.2626272556789118d, Category = "Standard Functions,Atan Function")]
        //[TestCase("Atan(-1)", ExpectedResult = -0.78539816339744828d, Category = "Standard Functions,Atan Function")]
        //[TestCase("Atan(0)", ExpectedResult = 0, Category = "Standard Functions,Atan Function")]
        //[TestCase("Atan(0.5)", ExpectedResult = 0.46364760900080609d, Category = "Standard Functions,Atan Function")]
        //[TestCase("Atan(1)", ExpectedResult = 0.78539816339744828d, Category = "Standard Functions,Atan Function")]
        //[TestCase("Atan(2)", ExpectedResult = 1.1071487177940904d, Category = "Standard Functions,Atan Function")]
        //[TestCase("Atan(Pi)", ExpectedResult = 1.2626272556789118d, Category = "Standard Functions,Atan Function")]
        //#endregion

        //#region Atan2 Function
        //[TestCase("Atan2(2d, 3d)", ExpectedResult = 0.5880026035475675d, Category = "Standard Functions,Atan2 Function")]
        //[TestCase("Atan2(-1d, 2d)", ExpectedResult = -0.46364760900080609d, Category = "Standard Functions,Atan2 Function")]
        //[TestCase("Atan2(0d, 0.5)", ExpectedResult = 0, Category = "Standard Functions,Atan2 Function")]
        //[TestCase("Atan2(0.5, 2d)", ExpectedResult = 0.24497866312686414d, Category = "Standard Functions,Atan2 Function")]
        //[TestCase("Atan2(1, 1)", ExpectedResult = 0.78539816339744828d, Category = "Standard Functions,Atan2 Function")]
        //[TestCase("Atan2(Pi, 1d)", ExpectedResult = 1.2626272556789118d, Category = "Standard Functions,Atan2 Function")]
        //#endregion

        //#region Avg Function
        //[TestCase("Avg(2d)", ExpectedResult = 2d, Category = "Standard Functions,Avg Function")]
        //[TestCase("Avg(2d,3d)", ExpectedResult = 2.5, Category = "Standard Functions,Avg Function")]
        //[TestCase("Avg(2d,3d, 6.5)", ExpectedResult = 3.8333333333333335d, Category = "Standard Functions,Avg Function")]
        //[TestCase("Avg(10d,-10d)", ExpectedResult = 0d, Category = "Standard Functions,Avg Function")]
        //[TestCase("Avg(10d,-10d, 10d, -10d, 10d)", ExpectedResult = 2d, Category = "Standard Functions,Avg Function")]
        //#endregion

        //#region Ceiling Function
        //[TestCase("Ceiling(2d)", ExpectedResult = 2d, Category = "Standard Functions,Ceiling Function")]
        //[TestCase("Ceiling(2.5d)", ExpectedResult = 3d, Category = "Standard Functions,Ceiling Function")]
        //[TestCase("Ceiling(35.432638d)", ExpectedResult = 36d, Category = "Standard Functions,Ceiling Function")]
        //[TestCase("Ceiling(-2d)", ExpectedResult = -2d, Category = "Standard Functions,Ceiling Function")]
        //[TestCase("Ceiling(-2.5d)", ExpectedResult = -2d, Category = "Standard Functions,Ceiling Function")]
        //[TestCase("Ceiling(-35.432638d)", ExpectedResult = -35d, Category = "Standard Functions,Ceiling Function")]
        //#endregion

        //#region Cos Function
        //[TestCase("Cos(0d)", ExpectedResult = 1d, Category = "Standard Functions,Cos Function")]
        //[TestCase("Cos(Pi)", ExpectedResult = -1d, Category = "Standard Functions,Cos Function")]
        //[TestCase("Cos(2 * Pi)", ExpectedResult = 1d, Category = "Standard Functions,Cos Function")]
        //[TestCase("Cos(3 * Pi)", ExpectedResult = -1d, Category = "Standard Functions,Cos Function")]
        //[TestCase("Round(Cos(Pi / 3d), 1)", ExpectedResult = 0.5d, Category = "Standard Functions,Cos Function,Round Function")]
        //[TestCase("Round(Cos(Pi / 2d), 2)", ExpectedResult = 0d, Category = "Standard Functions,Cos Function,Round Function")]
        //[TestCase("Cos(4.8)", ExpectedResult = 0.087498983439446398d, Category = "Standard Functions,Cos Function")]
        //#endregion

        //#region Cosh Function
        //[TestCase("Cosh(0d)", ExpectedResult = 1d, Category = "Standard Functions,Cosh Function")]
        //[TestCase("Cosh(1d)", ExpectedResult = 1.5430806348152437d, Category = "Standard Functions,Cosh Function")]
        //[TestCase("Cosh(Pi)", ExpectedResult = 11.591953275521519d, Category = "Standard Functions,Cosh Function")]
        //#endregion

        //#region Exp Function
        //[TestCase("Exp(-10d)", ExpectedResult = 4.5399929762484854E-05d, Category = "Standard Functions,Exp Function")]
        //[TestCase("Exp(0d)", ExpectedResult = 1d, Category = "Standard Functions,Exp Function")]
        //[TestCase("Exp(1d)", ExpectedResult = 2.7182818284590451d, Category = "Standard Functions,Exp Function")]
        //[TestCase("Exp(20d)", ExpectedResult = 485165195.40979028d, Category = "Standard Functions,Exp Function")]
        //#endregion

        //#region Floor Function
        //[TestCase("Floor(2d)", ExpectedResult = 2d, Category = "Standard Functions,Floor Function")]
        //[TestCase("Floor(2.5d)", ExpectedResult = 2d, Category = "Standard Functions,Floor Function")]
        //[TestCase("Floor(35.432638d)", ExpectedResult = 35d, Category = "Standard Functions,Floor Function")]
        //[TestCase("Floor(-2d)", ExpectedResult = -2d, Category = "Standard Functions,Floor Function")]
        //[TestCase("Floor(-2.5d)", ExpectedResult = -3d, Category = "Standard Functions,Floor Function")]
        //[TestCase("Floor(-35.432638d)", ExpectedResult = -36d, Category = "Standard Functions,Floor Function")]
        //#endregion

        //#region IEEERemainder Function
        //[TestCase("IEEERemainder(-4, 2)", ExpectedResult = 0, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(-3, 2)", ExpectedResult = 1, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(-2, 2)", ExpectedResult = 0, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(-1, 2)", ExpectedResult = -1, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(0, 2)", ExpectedResult = 0, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(1, 2)", ExpectedResult = 1, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(2, 2)", ExpectedResult = 0, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(3, 2)", ExpectedResult = -1, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(4, 2)", ExpectedResult = 0, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(5, 2)", ExpectedResult = 1, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(6, 2)", ExpectedResult = 0, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(7, 2)", ExpectedResult = -1, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(8, 2)", ExpectedResult = 0, Category = "Standard Functions,IEEERemainder Function")]

        //[TestCase("IEEERemainder(-6, 3)", ExpectedResult = 0, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(-5, 3)", ExpectedResult = 1, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(-4, 3)", ExpectedResult = -1, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(-3, 3)", ExpectedResult = 0, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(-2, 3)", ExpectedResult = 1, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(-1, 3)", ExpectedResult = -1, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(0, 3)", ExpectedResult = 0, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(1, 3)", ExpectedResult = 1, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(2, 3)", ExpectedResult = -1, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(3, 3)", ExpectedResult = 0, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(4, 3)", ExpectedResult = 1, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(5, 3)", ExpectedResult = -1, Category = "Standard Functions,IEEERemainder Function")]
        //[TestCase("IEEERemainder(6, 3)", ExpectedResult = 0, Category = "Standard Functions,IEEERemainder Function")]
        //#endregion

        //#region in Function
        //[TestCase("in(8, 4, 2, 8)", ExpectedResult = true, Category = "Standard Functions,in Function")]
        //[TestCase("in(20, 4, 2, 8)", ExpectedResult = false, Category = "Standard Functions,in Function")]
        //#endregion

        //#region List Function
        //[TestCase("List(14, \"A text for test\", 2.5, true).Count", ExpectedResult = 4, Category = "Standard Functions,List Function,Instance Property")]
        //[TestCase("List(14, \"A text for test\", 2.5, true)[0]", ExpectedResult = 14, Category = "Standard Functions,List Function,Indexing")]
        //[TestCase("List(14, \"A text for test\", 2.5, true)[1]", ExpectedResult = "A text for test", Category = "Standard Functions,List Function,Indexing")]
        //[TestCase("List(14, \"A text for test\", 2.5, true)[2]", ExpectedResult = 2.5, Category = "Standard Functions,List Function,Indexing")]
        //[TestCase("List(14, \"A text for test\", 2.5, true)[3]", ExpectedResult = true, Category = "Standard Functions,List Function,Indexing")]
        //#endregion

        //#region ListOfType Function
        //[TestCase("ListOfType(typeof(int), 1,2,3 ).GetType()", ExpectedResult = typeof(List<int>), Category = "Standard Functions,ListOfType Function,Instance Property")]
        //[TestCase("ListOfType(typeof(int), 1,2,3 ).Count", ExpectedResult = 3, Category = "Standard Functions,ListOfType Function,Instance Property")]
        //[TestCase("ListOfType(typeof(int), 1,2,3 )[0]", ExpectedResult = 1, Category = "Standard Functions,ListOfType Function,Indexing")]
        //[TestCase("ListOfType(typeof(int), 1,2,3 )[1]", ExpectedResult = 2, Category = "Standard Functions,ListOfType Function,Indexing")]
        //[TestCase("ListOfType(typeof(int), 1,2,3 )[2]", ExpectedResult = 3, Category = "Standard Functions,ListOfType Function,Indexing")]
        //[TestCase("ListOfType(typeof(string), \"hello\",\"Test\").GetType()", ExpectedResult = typeof(List<string>), Category = "Standard Functions,ListOfType Function,Instance Property")]
        //[TestCase("ListOfType(typeof(string), \"hello\",\"Test\").Count", ExpectedResult = 2, Category = "Standard Functions,ListOfType Function,Instance Property")]
        //[TestCase("ListOfType(typeof(string), \"hello\",\"Test\")[0]", ExpectedResult = "hello", Category = "Standard Functions,ListOfType Function,Indexing")]
        //[TestCase("ListOfType(typeof(string), \"hello\",\"Test\")[1]", ExpectedResult = "Test", Category = "Standard Functions,ListOfType Function,Indexing")]
        //#endregion

        //#region Log Function
        //[TestCase("Log(64d, 2d)", ExpectedResult = 6, Category = "Standard Functions,Log Function")]
        //[TestCase("Log(100d, 10d)", ExpectedResult = 2, Category = "Standard Functions,Log Function")]
        //#endregion

        //#region Log10 Function
        //[TestCase("Log10(64d)", ExpectedResult = 1.8061799739838871d, Category = "Standard Functions,Log10 Function")]
        //[TestCase("Log10(100d)", ExpectedResult = 2, Category = "Standard Functions,Log10 Function")]
        //[TestCase("Log10(1000d)", ExpectedResult = 3, Category = "Standard Functions,Log10 Function")]
        //#endregion

        //#region Max Function
        //[TestCase("Max(-2)", ExpectedResult = -2, Category = "Standard Functions,Max Function")]
        //[TestCase("Max(0)", ExpectedResult = 0, Category = "Standard Functions,Max Function")]
        //[TestCase("Max(4)", ExpectedResult = 4, Category = "Standard Functions,Max Function")]
        //[TestCase("Max(5.5)", ExpectedResult = 5.5, Category = "Standard Functions,Max Function")]

        //[TestCase("Max(-2, 2)", ExpectedResult = 2, Category = "Standard Functions,Max Function")]
        //[TestCase("Max(0, 2)", ExpectedResult = 2, Category = "Standard Functions,Max Function")]
        //[TestCase("Max(1, 2)", ExpectedResult = 2, Category = "Standard Functions,Max Function")]
        //[TestCase("Max(2, 2)", ExpectedResult = 2, Category = "Standard Functions,Max Function")]
        //[TestCase("Max(3, 2)", ExpectedResult = 3, Category = "Standard Functions,Max Function")]

        //[TestCase("Max(-7, 2, 4, 6)", ExpectedResult = 6, Category = "Standard Functions,Max Function")]
        //[TestCase("Max(-6, 2, 4, 6)", ExpectedResult = 6, Category = "Standard Functions,Max Function")]
        //[TestCase("Max(-0, 2, 4, 6)", ExpectedResult = 6, Category = "Standard Functions,Max Function")]
        //[TestCase("Max(4, 2, 8, 6)", ExpectedResult = 8, Category = "Standard Functions,Max Function")]
        //[TestCase("Max(6.2, 10.6, 4.1, 6)", ExpectedResult = 10.6, Category = "Standard Functions,Max Function")]
        //#endregion

        //#region Min Function
        //[TestCase("Min(-2)", ExpectedResult = -2, Category = "Standard Functions,Min Function")]
        //[TestCase("Min(0)", ExpectedResult = 0, Category = "Standard Functions,Min Function")]
        //[TestCase("Min(4)", ExpectedResult = 4, Category = "Standard Functions,Min Function")]
        //[TestCase("Min(5.5)", ExpectedResult = 5.5, Category = "Standard Functions,Min Function")]

        //[TestCase("Min(-2, 2)", ExpectedResult = -2, Category = "Standard Functions,Min Function")]
        //[TestCase("Min(0, 2)", ExpectedResult = 0, Category = "Standard Functions,Min Function")]
        //[TestCase("Min(1, 2)", ExpectedResult = 1, Category = "Standard Functions,Min Function")]
        //[TestCase("Min(2, 2)", ExpectedResult = 2, Category = "Standard Functions,Min Function")]
        //[TestCase("Min(3, 2)", ExpectedResult = 2, Category = "Standard Functions,Min Function")]

        //[TestCase("Min(-7, 2, 4, 6)", ExpectedResult = -7, Category = "Standard Functions,Min Function")]
        //[TestCase("Min(-6, 2, 4, 6)", ExpectedResult = -6, Category = "Standard Functions,Min Function")]
        //[TestCase("Min(0, 2, 4, 6)", ExpectedResult = 0, Category = "Standard Functions,Min Function")]
        //[TestCase("Min(4, 2, 8, 6)", ExpectedResult = 2, Category = "Standard Functions,Min Function")]
        //[TestCase("Min(6.2, 10.6, 4.1, 6)", ExpectedResult = 4.1, Category = "Standard Functions,Min Function")]
        //#endregion

        //#region new Function
        //[TestCase("new(ClassForTest1).GetType()", ExpectedResult = typeof(ClassForTest1), Category = "Standard Functions,new Function")]
        //[TestCase("new(ClassForTest2, 15).GetType()", ExpectedResult = typeof(ClassForTest2), Category = "Standard Functions,new Function")]
        //[TestCase("new(ClassForTest2, 15).Value1", ExpectedResult = 15, Category = "Standard Functions,new Function")]
        //#endregion

        //#region Pow Function
        //[TestCase("Pow(2, 4)", ExpectedResult = 16, Category = "Standard Functions,Pow Function")]
        //[TestCase("Pow(10, 2)", ExpectedResult = 100, Category = "Standard Functions,Pow Function")]
        //[TestCase("Pow(2, -2)", ExpectedResult = 0.25, Category = "Standard Functions,Pow Function")]
        //[TestCase("Pow(-2, 2)", ExpectedResult = 4, Category = "Standard Functions,Pow Function")]
        //[TestCase("Pow(-2, 3)", ExpectedResult = -8, Category = "Standard Functions,Pow Function")]
        //[TestCase("Pow(0, 3)", ExpectedResult = 0, Category = "Standard Functions,Pow Function")]
        //[TestCase("Pow(3, 0)", ExpectedResult = 1, Category = "Standard Functions,Pow Function")]
        //[TestCase("Pow(4, 0.5)", ExpectedResult = 2, Category = "Standard Functions,Pow Function")]
        //[TestCase("Pow(1.5, 4)", ExpectedResult = 5.0625d, Category = "Standard Functions,Pow Function")]
        //#endregion

        //#region Round Function
        //[TestCase("Round(1.5)", ExpectedResult = 2, Category = "Standard Functions,Round Function")]
        //[TestCase("Round(1.6)", ExpectedResult = 2, Category = "Standard Functions,Round Function")]
        //[TestCase("Round(1.4)", ExpectedResult = 1, Category = "Standard Functions,Round Function")]
        //[TestCase("Round(-0.3)", ExpectedResult = 0, Category = "Standard Functions,Round Function")]
        //[TestCase("Round(-1.5)", ExpectedResult = -2, Category = "Standard Functions,Round Function")]
        //[TestCase("Round(4)", ExpectedResult = 4, Category = "Standard Functions,Round Function")]
        //[TestCase("Round(Pi, 2)", ExpectedResult = 3.14, Category = "Standard Functions,Round Function")]
        //[TestCase("Round(2.5, MidpointRounding.AwayFromZero)", ExpectedResult = 3, Category = "Standard Functions,Round Function")]
        //[TestCase("Round(2.5, MidpointRounding.ToEven)", ExpectedResult = 2, Category = "Standard Functions,Round Function")]
        //[TestCase("Round(2.25, 1, MidpointRounding.AwayFromZero)", ExpectedResult = 2.3, Category = "Standard Functions,Round Function")]
        //[TestCase("Round(2.25, 1, MidpointRounding.ToEven)", ExpectedResult = 2.2, Category = "Standard Functions,Round Function")]
        //#endregion

        //#region Sign Function
        //[TestCase("Sign(-12)", ExpectedResult = -1, Category = "Standard Functions,Sign Function")]
        //[TestCase("Sign(-3.7)", ExpectedResult = -1, Category = "Standard Functions,Sign Function")]
        //[TestCase("Sign(0)", ExpectedResult = 0, Category = "Standard Functions,Sign Function")]
        //[TestCase("Sign(2.7)", ExpectedResult = 1, Category = "Standard Functions,Sign Function")]
        //[TestCase("Sign(60)", ExpectedResult = 1, Category = "Standard Functions,Sign Function")]
        //#endregion

        //#region Sin Function
        //[TestCase("Sin(0d)", ExpectedResult = 0, Category = "Standard Functions,Sin Function")]
        //[TestCase("Round(Sin(Pi), 2)", ExpectedResult = 0, Category = "Standard Functions,Sin Function,Round Function")]
        //[TestCase("Round(Sin(Pi / 2),2)", ExpectedResult = 1, Category = "Standard Functions,Sin Function,Round Function")]
        //[TestCase("Round(Sin(Pi / 6),2)", ExpectedResult = 0.5, Category = "Standard Functions,Sin Function,Round Function")]
        //[TestCase("Sin(2)", ExpectedResult = 0.90929742682568171d, Category = "Standard Functions,Sin Function")]
        //#endregion

        //#region Sinh Function
        //[TestCase("Sinh(0d)", ExpectedResult = 0, Category = "Standard Functions,Sinh Function")]
        //[TestCase("Round(Sinh(Pi), 2)", ExpectedResult = 11.55, Category = "Standard Functions,Sinh Function,Round Function")]
        //[TestCase("Round(Sinh(Pi / 2),2)", ExpectedResult = 2.3, Category = "Standard Functions,Sinh Function,Round Function")]
        //[TestCase("Round(Sinh(Pi / 6),2)", ExpectedResult = 0.55, Category = "Standard Functions,Sinh Function,Round Function")]
        //[TestCase("Sinh(2)", ExpectedResult = 3.6268604078470186d, Category = "Standard Functions,Sinh Function")]
        //#endregion

        //#region Sqrt Function
        //[TestCase("Sqrt(0d)", ExpectedResult = 0, Category = "Standard Functions,Sqrt Function")]
        //[TestCase("Sqrt(-2)", ExpectedResult = Double.NaN, Category = "Standard Functions,Sqrt Function")]
        //[TestCase("Sqrt(4)", ExpectedResult = 2, Category = "Standard Functions,Sqrt Function")]
        //[TestCase("Sqrt(9)", ExpectedResult = 3, Category = "Standard Functions,Sqrt Function")]
        //[TestCase("Sqrt(18)", ExpectedResult = 4.2426406871192848d, Category = "Standard Functions,Sqrt Function")]
        //[TestCase("Sqrt(0.25)", ExpectedResult = 0.5, Category = "Standard Functions,Sqrt Function")]
        //[TestCase("Sqrt(100)", ExpectedResult = 10, Category = "Standard Functions,Sqrt Function")]
        //#endregion

        //#region Tan Function
        //[TestCase("Tan(0d)", ExpectedResult = 0, Category = "Standard Functions,Tan Function")]
        //[TestCase("Round(Tan(Pi / 4), 2)", ExpectedResult = 1, Category = "Standard Functions,Tan Function")]
        //[TestCase("Round(Tan(Pi / 3) ,2)", ExpectedResult = 1.73, Category = "Standard Functions,Tan Function,Round Function")]
        //[TestCase("Round(Tan(Pi / 6) ,2)", ExpectedResult = 0.58, Category = "Standard Functions,Tan Function,Round Function")]
        //[TestCase("Round(Tan(2),2)", ExpectedResult = -2.19, Category = "Standard Functions,Tan Function,Round Function")]
        //#endregion

        //#region Tanh Function
        //[TestCase("Tanh(0d)", ExpectedResult = 0, Category = "Standard Functions,Tanh Function")]
        //[TestCase("Round(Tanh(Pi / 4), 2)", ExpectedResult = 0.66, Category = "Standard Functions,Tanh Function")]
        //[TestCase("Round(Tanh(Pi / 3) ,2)", ExpectedResult = 0.78, Category = "Standard Functions,Tanh Function,Round Function")]
        //[TestCase("Round(Tanh(Pi / 6) ,2)", ExpectedResult = 0.48, Category = "Standard Functions,Tanh Function,Round Function")]
        //[TestCase("Round(Tanh(2),2)", ExpectedResult = 0.96, Category = "Standard Functions,Tanh Function,Round Function")]
        //#endregion

        //#region Truncate Function
        //[TestCase("Truncate(0d)", ExpectedResult = 0, Category = "Standard Functions,Truncate Function")]
        //[TestCase("Truncate(-1)", ExpectedResult = -1, Category = "Standard Functions,Truncate Function")]
        //[TestCase("Truncate(-2)", ExpectedResult = -2, Category = "Standard Functions,Truncate Function")]
        //[TestCase("Truncate(-23)", ExpectedResult = -23, Category = "Standard Functions,Truncate Function")]
        //[TestCase("Truncate(-0.5)", ExpectedResult = 0, Category = "Standard Functions,Truncate Function")]
        //[TestCase("Truncate(-0.6)", ExpectedResult = 0, Category = "Standard Functions,Truncate Function")]
        //[TestCase("Truncate(-0.4)", ExpectedResult = 0, Category = "Standard Functions,Truncate Function")]
        //[TestCase("Truncate(0.5)", ExpectedResult = 0, Category = "Standard Functions,Truncate Function")]
        //[TestCase("Truncate(0.6)", ExpectedResult = 0, Category = "Standard Functions,Truncate Function")]
        //[TestCase("Truncate(0.4)", ExpectedResult = 0, Category = "Standard Functions,Truncate Function")]
        //[TestCase("Truncate(1)", ExpectedResult = 1, Category = "Standard Functions,Truncate Function")]
        //[TestCase("Truncate(2)", ExpectedResult = 2, Category = "Standard Functions,Truncate Function")]
        //[TestCase("Truncate(23)", ExpectedResult = 23, Category = "Standard Functions,Truncate Function")]
        //[TestCase("Truncate(213.4719468971)", ExpectedResult = 213, Category = "Standard Functions,Truncate Function")]

        //#endregion

        //#endregion

        //#region Generic types Management

        //[TestCase("List(\"Hello\", \"Test\").Cast<string>().ToList<string>().GetType()", ExpectedResult = typeof(List<string>), Category = "List function, Generics")]
        //[TestCase("new List<string>().GetType()", ExpectedResult = typeof(List<string>), Category = "new Keyword, Generics")]
        //[TestCase("new Dictionary<string,List<int>>().GetType()", ExpectedResult = typeof(Dictionary<string, List<int>>), Category = "new Keyword, Generics")]

        //// Linq and Types inference
        //[TestCase("new List<int>() { 1, 2, 3, 4 }.Where<int>(x => x > 2).Json", ExpectedResult = "[3,4]", Category = "Linq, Lambda, new Keyword, Generics")]
        //[TestCase("new List<int>() { 1, 2, 3, 4 }.Where(x => x > 2).Json", ExpectedResult = "[3,4]", Category = "Linq, Type inference, Lambda, new Keyword, Generics")]

        //#endregion

        //#region Complex expressions
        //[TestCase("Enumerable.Range(1,4).Cast().Sum(x =>(int)x)", ExpectedResult = 10, Category = "Complex expression,Static method,Instance method,Lambda function,Cast")]
        //[TestCase("System.Linq.Enumerable.Range(1,4).Cast().Sum(x =>(int)x)", ExpectedResult = 10, Category = "Complex expression,Static method,Instance method,Lambda function,Cast")]
        //[TestCase("List(1,2,3,4,5,6).ConvertAll(x => (float)x)[2].GetType()", ExpectedResult = typeof(float), Category = "Complex expression,Type Manage,Instance method,Lambda function,Cast,Indexing")]
        //[TestCase("List(\"hello\", \"bye\").Select(x => x.ToUpper()).ToList().FluidAdd(\"test\").Count", ExpectedResult = 3, Category = "Complex expression,Fluid Functions")]
        //[TestCase("List(\"hello\", \"bye\").Select(x => x.ToUpper()).ToList().FluidAdd(\"test\")[0]", ExpectedResult = "HELLO", Category = "Complex expression,Fluid Functions")]
        //[TestCase("List(\"hello\", \"bye\").Select(x => x.ToUpper()).ToList().FluidAdd(\"test\")[1]", ExpectedResult = "BYE", Category = "Complex expression,Fluid Functions")]
        //[TestCase("List(\"hello\", \"bye\").Select(x => x.ToUpper()).ToList().FluidAdd(\"test\")[2]", ExpectedResult = "test", Category = "Complex expression,Fluid Functions")]
        //[TestCase("List(\"hello\", \"bye\").Select(x => x.ToUpper()).ToList().FluidAdd(\"test\")[2]", ExpectedResult = "test", Category = "Complex expression,Fluid Functions")]
        //[TestCase("$\"https://www.google.com/search?q={System.Net.WebUtility.UrlEncode(\"test of request with url encode() ?\")}\"", ExpectedResult = "https://www.google.com/search?q=test+of+request+with+url+encode()+%3F", Category = "Complex expression,Inline namespace")]
        //[TestCase("new System.Xml.XmlDocument().FluidLoadXml(\"<root><element id='MyElement'>Xml Content</element></root>\").SelectSingleNode(\"//element[@id='MyElement']\").InnerXml", ExpectedResult = "Xml Content", Category = "Complex expression,Inline namespace,Fluid")]
        //[TestCase("new System.Xml.XmlDocument().FluidLoadXml(\"<root><element id='MyElement'>Xml Content</element></root>\").ChildNodes[0].Name", ExpectedResult = "root", Category = "Complex expression,Inline namespace,Fluid,Custom Indexer")]
        //[TestCase("string.Join(\" - \", new string[]{\"Hello\", \"Bye\", \"Other\"})", ExpectedResult = "Hello - Bye - Other", Category = "Complex expression, Different brackets imbrication")]

        //#endregion

        //#region Bugs correction

        //[TestCase("new DateTime(1985,9,11).ToString(\"dd.MM.yyyy\")", ExpectedResult = "11.09.1985", Category = "Complex expression,Static method,Instance method,Lambda function,Cast")]

        //#endregion

        #endregion

        public object DirectExpressionEvaluation(string expression)
        {
            ExpressionEvaluator evaluator = new ExpressionEvaluator();

            // TODO events and Namespaces
            //evaluator.EvaluateVariable += Evaluator_EvaluateVariable;

            //evaluator.Namespaces.Add("CodingSeb.ExpressionEvaluator.Tests");

            object result = evaluator.Evaluate(expression);

            //evaluator.EvaluateVariable -= Evaluator_EvaluateVariable;

            return result;
        }

        #endregion
    }
}