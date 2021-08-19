namespace CSXPression.Parsing
{
    /// <summary>
    /// This class describe the way a parser parse its given code
    /// </summary>
    public class ParserOptions
    {
        /// <summary>
        /// If <c>true</c> the parsing of the code is case sensitives.
        /// If <c>false</c> the parsing ignore case.
        /// By default = false
        /// </summary>
        public bool IgnoreCase { get; set; }

        /// <summary>
        /// If <c>true</c> all numbers without decimal and suffixes evaluations will be parsed as double
        /// If <c>false</c> Integers values without decimal and suffixes will be evaluate as int as in C#
        /// By default = false
        /// </summary>
        public bool ForceIntegerParsingAsDouble { get; set; }
    }
}
