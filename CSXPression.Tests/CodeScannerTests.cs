using CSXPression.Lexer;
using NUnit.Framework;
using Shouldly;

namespace CSXPression.Tests
{
    public class CodeScannerTests
    {
        [Test]
        public void CodeScannerOnlyReadTests()
        {
            CodeScanner codeScanner = new CodeScanner("x + 2");

            codeScanner.Position.ShouldBe(0);
            codeScanner.IsEndOfCode.ShouldBeFalse();
            codeScanner.Read().Value.ShouldBe('x');
            codeScanner.Position.ShouldBe(1);
            codeScanner.IsEndOfCode.ShouldBeFalse();
            codeScanner.Read().Value.ShouldBe(' ');
            codeScanner.Position.ShouldBe(2);
            codeScanner.IsEndOfCode.ShouldBeFalse();
            codeScanner.Read().Value.ShouldBe('+');
            codeScanner.Position.ShouldBe(3);
            codeScanner.IsEndOfCode.ShouldBeFalse();
            codeScanner.Read().Value.ShouldBe(' ');
            codeScanner.Position.ShouldBe(4);
            codeScanner.IsEndOfCode.ShouldBeFalse();
            codeScanner.Read().Value.ShouldBe('2');
            codeScanner.Position.ShouldBe(5);
            codeScanner.IsEndOfCode.ShouldBeTrue();
            codeScanner.Read().HasValue.ShouldBeFalse();
        }

        [Test]
        public void CodeScannerWithPeekTests()
        {
            CodeScanner codeScanner = new CodeScanner("x + 2");

            codeScanner.Position.ShouldBe(0);
            codeScanner.IsEndOfCode.ShouldBeFalse();
            codeScanner.Peek().Value.ShouldBe('x');
            codeScanner.Position.ShouldBe(0);
            codeScanner.Read().Value.ShouldBe('x');
            codeScanner.Position.ShouldBe(1);
            codeScanner.IsEndOfCode.ShouldBeFalse();
            codeScanner.Peek().Value.ShouldBe(' ');
            codeScanner.Position.ShouldBe(1);
            codeScanner.Read().Value.ShouldBe(' ');
            codeScanner.Position.ShouldBe(2);
            codeScanner.IsEndOfCode.ShouldBeFalse();
            codeScanner.Peek().Value.ShouldBe('+');
            codeScanner.Position.ShouldBe(2);
            codeScanner.Read().Value.ShouldBe('+');
            codeScanner.Position.ShouldBe(3);
            codeScanner.IsEndOfCode.ShouldBeFalse();
            codeScanner.Peek().Value.ShouldBe(' ');
            codeScanner.Position.ShouldBe(3);
            codeScanner.Read().Value.ShouldBe(' ');
            codeScanner.Position.ShouldBe(4);
            codeScanner.IsEndOfCode.ShouldBeFalse();
            codeScanner.Peek().Value.ShouldBe('2');
            codeScanner.Position.ShouldBe(4);
            codeScanner.IsEndOfCode.ShouldBeFalse();
            codeScanner.Read().Value.ShouldBe('2');
            codeScanner.Position.ShouldBe(5);
            codeScanner.IsEndOfCode.ShouldBeTrue();
            codeScanner.Read().HasValue.ShouldBeFalse();
        }
    }
}
