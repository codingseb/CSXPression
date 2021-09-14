using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace TryCScharpStuffs
{
    class Program
    {
        private static Regex regex = new Regex(@"(?<name>[\p{L}_](?>[\p{L}_0-9]*))", RegexOptions.Compiled);

        static void Main(string[] args)
        {

            Console.ReadLine();
        }
    }
}
