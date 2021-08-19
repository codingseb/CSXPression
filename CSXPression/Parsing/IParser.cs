using CSXPression.Tokens;

namespace CSXPression.Parsing
{
    /// <summary>
    /// This interface define a parser for a specific syntax to build the tokens tree
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// This method build a tokens tree from the specified code base on the syntax of the parser
        /// </summary>
        /// <param name="code">The code to parse</param>
        /// <returns>The root of parsed the tokens tree</returns>
        IToken Parse(string code);
    }
}
