namespace CSXPression.Parsing
{
    /// <summary>
    /// This class define which Parsing functionalities are active or not
    /// </summary>
    public class ParserFunctionalities
    {
        /// <summary>
        /// If <c>true</c> allow char parsing with <c>''</c><para/>
        /// If <c>false</c> unactive this functionality
        /// <para>Default value : <c>true</c></para>
        /// </summary>
        public bool CharParsing { get; set; } = true;

        /// <summary>
        /// if <c>true</c> allow string parsing with <c>""</c><para/>
        /// if <c>false</c> unactive this functionality.
        /// <para>Default value : <c>true</c></para>
        /// </summary>
        public bool StringParsing { get; set; } = true;
    }
}
