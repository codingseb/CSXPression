using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace CSXPression.Parsing
{
    public partial class Parser
    {
        protected static readonly Regex typeNameRegEx = new(@"^(?<inObject>\.)?(?<name>[\p{L}_](?>[\p{L}_0-9]*))(?>\s*)((?<isgeneric>[<](?>([\p{L}_](?>[\p{L}_0-9]*)|(?>\s+)|[,\.])+|(?<gentag>[<])|(?<-gentag>[>]))*(?(gentag)(?!))[>])?)", RegexOptions.Compiled);
        protected static readonly Regex arrayTypeDetectionRegex = new(@"^(\s*(\[(?>(?>\s+)|[,])*)\])+", RegexOptions.Compiled);
        protected static readonly Regex genericsDecodeRegex = new("(?<name>[^,<>]+)(?<isgeneric>[<](?>[^<>]+|(?<gentag>[<])|(?<-gentag>[>]))*(?(gentag)(?!))[>])?", RegexOptions.Compiled);
        protected static readonly Regex genericsEndOnlyOneTrim = new(@"(?>\s*)[>](?>\s*)$", RegexOptions.Compiled);

        protected IDictionary<string, Type> PrimaryTypesDict => new Dictionary<string, Type>(Options.IgnoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal)
        {
            { "object", typeof(object) },
            { "string", typeof(string) },
            { "bool", typeof(bool) },
            { "bool?", typeof(bool?) },
            { "byte", typeof(byte) },
            { "byte?", typeof(byte?) },
            { "char", typeof(char) },
            { "char?", typeof(char?) },
            { "decimal", typeof(decimal) },
            { "decimal?", typeof(decimal?) },
            { "double", typeof(double) },
            { "double?", typeof(double?) },
            { "short", typeof(short) },
            { "short?", typeof(short?) },
            { "int", typeof(int) },
            { "int?", typeof(int?) },
            { "long", typeof(long) },
            { "long?", typeof(long?) },
            { "sbyte", typeof(sbyte) },
            { "sbyte?", typeof(sbyte?) },
            { "float", typeof(float) },
            { "float?", typeof(float?) },
            { "ushort", typeof(ushort) },
            { "ushort?", typeof(ushort?) },
            { "uint", typeof(uint) },
            { "uint?", typeof(uint?) },
            { "ulong", typeof(ulong) },
            { "ulong?", typeof(ulong?) },
            { "void", typeof(void) }
        };

        protected virtual Type EvaluateType(string expression, ref int i, string currentName = "", string genericsTypes = "")
        {
            string typeName = currentName + ((i < expression.Length && expression.Substring(i)[0] == '?') ? "?" : "");
            Type staticType = GetTypeByFriendlyName(typeName, genericsTypes);

            // For inline namespace parsing
            if (staticType == null && Functionalities.InlineNamespaces)
            {
                int subIndex = 0;
                Match namespaceMatch = typeNameRegEx.Match(expression.Substring(i + subIndex));

                while (staticType == null
                    && namespaceMatch.Success
                    && i + subIndex < expression.Length
                    && !typeName.EndsWith("?"))
                {
                    subIndex += namespaceMatch.Length;
                    typeName += namespaceMatch.Groups["inObject"].Value + namespaceMatch.Groups["name"].Value + ((i + subIndex < expression.Length && expression.Substring(i + subIndex)[0] == '?') ? "?" : "");

                    staticType = GetTypeByFriendlyName(typeName, namespaceMatch.Groups["isgeneric"].Value);

                    if (staticType != null)
                    {
                        i += subIndex;
                        break;
                    }

                    namespaceMatch = typeNameRegEx.Match(expression.Substring(i + subIndex));
                }
            }

            if (typeName.EndsWith("?") && staticType != null)
                i++;

            // For nested type parsing
            if (staticType != null)
            {
                int subIndex = 0;
                Match nestedTypeMatch = typeNameRegEx.Match(expression.Substring(i + subIndex));
                while (nestedTypeMatch.Success)
                {
                    subIndex = nestedTypeMatch.Length;
                    typeName += $"+{nestedTypeMatch.Groups["name"].Value}{((i + subIndex < expression.Length && expression.Substring(i + subIndex)[0] == '?') ? "?" : "") }";

                    Type nestedType = GetTypeByFriendlyName(typeName, nestedTypeMatch.Groups["isgeneric"].Value);
                    if (nestedType != null)
                    {
                        i += subIndex;
                        staticType = nestedType;

                        if (typeName.EndsWith("?"))
                            i++;
                    }
                    else
                    {
                        break;
                    }

                    nestedTypeMatch = typeNameRegEx.Match(expression.Substring(i));
                }
            }

            Match arrayTypeMatch;

            if (i < expression.Length && (arrayTypeMatch = arrayTypeDetectionRegex.Match(expression.Substring(i))).Success)
            {
                Type arrayType = GetTypeByFriendlyName(staticType + arrayTypeMatch.Value);
                if (arrayType != null)
                {
                    i += arrayTypeMatch.Length;
                    staticType = arrayType;
                }
            }

            return staticType;
        }

        protected virtual Type GetTypeByFriendlyName(string typeName, string genericTypes = "", bool throwExceptionIfNotFound = false)
        {
            Type result = null;
            string formatedGenericTypes = string.Empty;
            //bool isCached = false;
            try
            {
                typeName = typeName.Replace(" ", "").Replace("\t", "").Replace("\r", "").Replace("\n", "");
                genericTypes = genericTypes.Replace(" ", "").Replace("\t", "").Replace("\r", "").Replace("\n", "");

                if (PrimaryTypesDict.ContainsKey(typeName))
                {
                    result = PrimaryTypesDict[typeName];
                }

                // TODO : Cache type resolution
                //if (CacheTypesResolutions && (TypesResolutionCaching?.ContainsKey(typeName + genericTypes) ?? false))
                //{
                //    result = TypesResolutionCaching[typeName + genericTypes];
                //    isCached = true;
                //}

                if (result == null)
                {
                    if (!genericTypes.Equals(string.Empty))
                    {
                        Type[] types = GetConcreteTypes(genericTypes);
                        formatedGenericTypes = $"`{types.Length}[{ string.Join(", ", types.Select(type => "[" + type.AssemblyQualifiedName + "]"))}]";
                    }

                    result = Type.GetType(typeName + formatedGenericTypes, false, Options.IgnoreCase);
                }

                if (result == null)
                {
                    StringComparison stringComparison = Options.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
                    result = Types.ToList().Find(type => type.Name.Equals(typeName, stringComparison) || type.FullName.StartsWith(typeName + ","));
                }

                for (int a = 0; a < Assemblies.Count && result == null; a++)
                {
                    if (typeName.Contains("."))
                    {
                        result = Type.GetType($"{typeName}{formatedGenericTypes},{Assemblies[a].FullName}", false, Options.IgnoreCase);
                    }
                    else
                    {
                        for (int i = 0; i < Namespaces.Count && result == null; i++)
                        {
                            result = Type.GetType($"{Namespaces[i]}.{typeName}{formatedGenericTypes},{Assemblies[a].FullName}", false, Options.IgnoreCase);
                        }
                    }
                }
            }
            catch (ParsingException)
            {
                throw;
            }
            catch { }

            if (result != null && TypesToBlock.Contains(result))
                result = null;

            if (result == null && throwExceptionIfNotFound)
                throw new ParsingException($"Type or class {typeName}{genericTypes} is unknown");

            // TODO : Cache type resolution
            //if (CacheTypesResolutions && (result != null) && !isCached)
            //    TypesResolutionCaching[typeName + genericTypes] = result;

            return result;
        }

        protected virtual Type[] GetConcreteTypes(string genericsTypes)
        {
            return genericsDecodeRegex
                .Matches(genericsEndOnlyOneTrim.Replace(genericsTypes.TrimStart(' ', '<'), ""))
                .Cast<Match>()
                .Select(match => GetTypeByFriendlyName(match.Groups["name"].Value, match.Groups["isgeneric"].Value, true))
                .ToArray();
        }
    }
}
