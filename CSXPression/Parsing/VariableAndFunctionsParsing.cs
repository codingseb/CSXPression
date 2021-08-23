//using CSXPression.Tokens;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Data;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading;
//using System.Threading.Tasks;

//namespace CSXPression.Parsing
//{
//    public partial class Parser
//    {
//        protected Regex VarOrFunctionRegex => new(@"^((?<sign>[+-])|(?<prefixOperator>[+][+]|--)|(?<varKeyword>var)\s+|(?<dynamicKeyword>dynamic)\s+|((?<nullConditional>[?])?(?<inObject>\.))?)(?<name>[\p{L}_](?>[\p{L}_0-9]*))(?>\s*)((?<assignationOperator>(?<assignmentPrefix>[+\-*/%&|^]|<<|>>|\?\?)?=(?![=>]))|(?<postfixOperator>([+][+]|--)(?![\p{L}_0-9]))|((?<isgeneric>[<](?>([\p{L}_](?>[\p{L}_0-9]*)|(?>\s+)|[,\.])+|(?<gentag>[<])|(?<-gentag>[>]))*(?(gentag)(?!))[>])?(?<isfunction>[(])?))", (Options.IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None) | RegexOptions.Compiled);

//        protected virtual bool EvaluateVarOrFunc(string expression, Stack<object> stack, ref int i)
//        {
//            Match varFuncMatch = VarOrFunctionRegex.Match(expression.Substring(i));

//            if (varFuncMatch.Groups["varKeyword"].Success
//                && !varFuncMatch.Groups["assignationOperator"].Success)
//            {
//                throw new ParsingException("Implicit variables must be initialized. [var " + varFuncMatch.Groups["name"].Value + "]");
//            }

//            string varFuncName = varFuncMatch.Groups["name"].Value;

//            if (varFuncMatch.Success
//            && (!varFuncMatch.Groups["sign"].Success
//                || stack.Count == 0
//                || stack.Peek() is IOperatorToken)
//            && ((!BinaryOperatorsDictionary.ContainsKey(varFuncName) && !UnaryOperatorsDictionary.ContainsKey(varFuncName))
//                || varFuncMatch.Groups["inObject"].Success))
//            {
//                string genericsTypes = varFuncMatch.Groups["isgeneric"].Value;
//                bool inObject = varFuncMatch.Groups["inObject"].Success;

//                i += varFuncMatch.Length;

//                if (varFuncMatch.Groups["isfunction"].Success)
//                {
//                    List<string> funcArgs = GetExpressionsBetweenParenthesesOrOtherImbricableBrackets(expression, ref i, true, OptionFunctionArgumentsSeparator);

//                    if (inObject
//                        || Context?.GetType()
//                            .GetMethods(InstanceBindingFlag)
//                            .Any(methodInfo => methodInfo.Name.Equals(varFuncName, StringComparisonForCasing)) == true)
//                    {
//                        if (inObject && (stack.Count == 0 || stack.Peek() is IOperatorToken))
//                            throw new ParsingException($"[{varFuncMatch.Value})] must follow an object.");

//                        object obj = inObject ? stack.Pop() : Context;
//                        object keepObj = obj;
//                        Type objType = null;
//                        Type[] inferedGenericsTypes = obj?.GetType().GenericTypeArguments;
//                        ValueTypeNestingTrace valueTypeNestingTrace = null;

//                        if (obj != null && TypesToBlock.Contains(obj.GetType()))
//                            throw new ParsingException($"{obj.GetType().FullName} type is blocked");
//                        else if (obj is Type staticType && TypesToBlock.Contains(staticType))
//                            throw new ParsingException($"{staticType.FullName} type is blocked");
//                        else if (obj is ClassOrEnumType classOrType && TypesToBlock.Contains(classOrType.Type))
//                            throw new ParsingException($"{classOrType.Type} type is blocked");

//                        try
//                        {
//                            if (obj is NullConditionalNullValue)
//                            {
//                                stack.Push(obj);
//                            }
//                            else if (varFuncMatch.Groups["nullConditional"].Success && obj == null)
//                            {
//                                stack.Push(new NullConditionalNullValue());
//                            }
//                            else if (obj is BubbleExceptionContainer)
//                            {
//                                stack.Push(obj);
//                                return true;
//                            }
//                            else
//                            {
//                                FunctionPreEvaluationEventArg functionPreEvaluationEventArg = new FunctionPreEvaluationEventArg(varFuncName, funcArgs, this, obj, genericsTypes, GetConcreteTypes);

//                                PreEvaluateFunction?.Invoke(this, functionPreEvaluationEventArg);

//                                if (functionPreEvaluationEventArg.CancelEvaluation)
//                                {
//                                    throw new ExpressionEvaluatorSyntaxErrorException($"[{objType}] object has no Method named \"{varFuncName}\".");
//                                }
//                                else if (functionPreEvaluationEventArg.FunctionReturnedValue)
//                                {
//                                    stack.Push(functionPreEvaluationEventArg.Value);
//                                }
//                                else
//                                {
//                                    int argIndex = 0;
//                                    List<ArgKeywordsEncaps> argsWithKeywords = new List<ArgKeywordsEncaps>();

//                                    List<object> oArgs = funcArgs.ConvertAll(arg =>
//                                    {
//                                        Match functionArgKeywordsMatch = functionArgKeywordsRegex.Match(arg);
//                                        object argValue;

//                                        if (functionArgKeywordsMatch.Success)
//                                        {
//                                            ArgKeywordsEncaps argKeywordEncaps = new ArgKeywordsEncaps()
//                                            {
//                                                Index = argIndex,
//                                                Keyword = functionArgKeywordsMatch.Groups["keyword"].Value,
//                                                VariableName = functionArgKeywordsMatch.Groups["varName"].Value
//                                            };

//                                            argsWithKeywords.Add(argKeywordEncaps);

//                                            if (functionArgKeywordsMatch.Groups["typeName"].Success)
//                                            {
//                                                Type fixedType = ((ClassOrEnumType)Evaluate(functionArgKeywordsMatch.Groups["typeName"].Value)).Type;

//                                                variables[argKeywordEncaps.VariableName] = new StronglyTypedVariable() { Type = fixedType, Value = GetDefaultValueOfType(fixedType) };
//                                            }
//                                            else if (!variables.ContainsKey(argKeywordEncaps.VariableName))
//                                            {
//                                                variables[argKeywordEncaps.VariableName] = null;
//                                            }

//                                            argValue = Evaluate(functionArgKeywordsMatch.Groups["toEval"].Value);
//                                        }
//                                        else
//                                        {
//                                            argValue = Evaluate(arg);
//                                        }

//                                        argIndex++;
//                                        return argValue;
//                                    });

//                                    BindingFlags flag = DetermineInstanceOrStatic(ref objType, ref obj, ref valueTypeNestingTrace);

//                                    if (!OptionStaticMethodsCallActive && (flag & BindingFlags.Static) != 0)
//                                        throw new ExpressionEvaluatorSyntaxErrorException($"[{objType}] object has no Method named \"{varFuncName}\".");
//                                    if (!OptionInstanceMethodsCallActive && (flag & BindingFlags.Instance) != 0)
//                                        throw new ExpressionEvaluatorSyntaxErrorException($"[{objType}] object has no Method named \"{varFuncName}\".");

//                                    // Standard Instance or public method find
//                                    MethodInfo methodInfo = GetRealMethod(ref objType, ref obj, varFuncName, flag, oArgs, genericsTypes, inferedGenericsTypes, argsWithKeywords);

//                                    // if not found check if obj is an expandoObject or similar
//                                    if (obj is IDynamicMetaObjectProvider
//                                        && obj is IDictionary<string, object> dictionaryObject
//                                        && (dictionaryObject[varFuncName] is InternalDelegate || dictionaryObject[varFuncName] is Delegate))
//                                    {
//                                        if (dictionaryObject[varFuncName] is InternalDelegate internalDelegate)
//                                            stack.Push(internalDelegate(oArgs.ToArray()));
//                                        else if (dictionaryObject[varFuncName] is Delegate del)
//                                            stack.Push(del.DynamicInvoke(oArgs.ToArray()));
//                                    }
//                                    else if (objType.GetProperty(varFuncName, InstanceBindingFlag) is PropertyInfo instancePropertyInfo
//                                        && (instancePropertyInfo.PropertyType.IsSubclassOf(typeof(Delegate)) || instancePropertyInfo.PropertyType == typeof(Delegate))
//                                        && instancePropertyInfo.GetValue(obj) is Delegate del)
//                                    {
//                                        stack.Push(del.DynamicInvoke(oArgs.ToArray()));
//                                    }
//                                    else
//                                    {
//                                        bool isExtention = false;

//                                        // if not found try to Find extension methods.
//                                        if (methodInfo == null && obj != null)
//                                        {
//                                            oArgs.Insert(0, obj);
//                                            objType = obj.GetType();

//                                            object extentionObj = null;
//                                            for (int e = 0; e < StaticTypesForExtensionsMethods.Count && methodInfo == null; e++)
//                                            {
//                                                Type type = StaticTypesForExtensionsMethods[e];
//                                                methodInfo = GetRealMethod(ref type, ref extentionObj, varFuncName, StaticBindingFlag, oArgs, genericsTypes, inferedGenericsTypes, argsWithKeywords, true);
//                                                isExtention = methodInfo != null;
//                                            }
//                                        }

//                                        if (methodInfo != null)
//                                        {
//                                            object[] argsArray = oArgs.ToArray();
//                                            stack.Push(methodInfo.Invoke(isExtention ? null : obj, argsArray));
//                                            argsWithKeywords
//                                                .FindAll(argWithKeyword => argWithKeyword.Keyword.Equals("out", StringComparisonForCasing) || argWithKeyword.Keyword.Equals("ref", StringComparisonForCasing))
//                                                .ForEach(outOrRefArg => AssignVariable(outOrRefArg.VariableName, argsArray[outOrRefArg.Index + (isExtention ? 1 : 0)]));
//                                        }
//                                        else if (objType.GetProperty(varFuncName, StaticBindingFlag) is PropertyInfo staticPropertyInfo
//                                            && (staticPropertyInfo.PropertyType.IsSubclassOf(typeof(Delegate)) || staticPropertyInfo.PropertyType == typeof(Delegate))
//                                            && staticPropertyInfo.GetValue(obj) is Delegate del2)
//                                        {
//                                            stack.Push(del2.DynamicInvoke(oArgs.ToArray()));
//                                        }
//                                        else
//                                        {
//                                            FunctionEvaluationEventArg functionEvaluationEventArg = new FunctionEvaluationEventArg(varFuncName, funcArgs, this, obj ?? keepObj, genericsTypes, GetConcreteTypes);

//                                            EvaluateFunction?.Invoke(this, functionEvaluationEventArg);

//                                            if (functionEvaluationEventArg.FunctionReturnedValue)
//                                            {
//                                                stack.Push(functionEvaluationEventArg.Value);
//                                            }
//                                            else
//                                            {
//                                                if (OptionDetectExtensionMethodsOverloadsOnExtensionMethodNotFound)
//                                                {
//                                                    IEnumerable<MethodInfo> query = from type in StaticTypesForExtensionsMethods
//                                                                                    where
//                                                                                          !type.IsGenericType &&
//                                                                                          type.IsSealed &&
//                                                                                          !type.IsNested
//                                                                                    from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
//                                                                                    where method.IsDefined(typeof(ExtensionAttribute), false)
//                                                                                    where method.GetParameters()[0].ParameterType == objType // static extMethod(this outType, ...)
//                                                                                    select method;

//                                                    if (query.Any())
//                                                    {
//                                                        string fnArgsPrint = string.Join(",", funcArgs);
//                                                        string fnOverloadsPrint = "";

//                                                        foreach (MethodInfo mi in query)
//                                                        {
//                                                            ParameterInfo[] parInfo = mi.GetParameters();
//                                                            fnOverloadsPrint += string.Join(",", parInfo.Select(x => x.ParameterType.FullName ?? x.ParameterType.Name)) + "\n";
//                                                        }

//                                                        throw new ExpressionEvaluatorSyntaxErrorException($"[{objType}] extension method \"{varFuncName}\" has no overload for arguments: {fnArgsPrint}. Candidates: {fnOverloadsPrint}");
//                                                    }
//                                                }

//                                                throw new ExpressionEvaluatorSyntaxErrorException($"[{objType}] object has no Method named \"{varFuncName}\".");
//                                            }
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                        catch (ExpressionEvaluatorSecurityException)
//                        {
//                            throw;
//                        }
//                        catch (ExpressionEvaluatorSyntaxErrorException)
//                        {
//                            throw;
//                        }
//                        catch (NullReferenceException nullException)
//                        {
//                            stack.Push(new BubbleExceptionContainer()
//                            {
//                                Exception = nullException
//                            });

//                            return true;
//                        }
//                        catch (Exception ex)
//                        {
//                            //Transport the exception in stack.
//                            stack.Push(new BubbleExceptionContainer()
//                            {
//                                Exception = new ExpressionEvaluatorSyntaxErrorException($"The call of the method \"{varFuncName}\" on type [{objType}] generate this error : {ex.InnerException?.Message ?? ex.Message}", ex)
//                            });
//                            return true;  //Signals an error to the parsing method array call                          
//                        }
//                    }
//                    else
//                    {
//                        FunctionPreEvaluationEventArg functionPreEvaluationEventArg = new FunctionPreEvaluationEventArg(varFuncName, funcArgs, this, null, genericsTypes, GetConcreteTypes);

//                        PreEvaluateFunction?.Invoke(this, functionPreEvaluationEventArg);

//                        if (functionPreEvaluationEventArg.CancelEvaluation)
//                        {
//                            throw new ExpressionEvaluatorSyntaxErrorException($"Function [{varFuncName}] unknown in expression : [{expression.Replace("\r", "").Replace("\n", "")}]");
//                        }
//                        else if (functionPreEvaluationEventArg.FunctionReturnedValue)
//                        {
//                            stack.Push(functionPreEvaluationEventArg.Value);
//                        }
//                        else if (DefaultFunctions(varFuncName, funcArgs, out object funcResult))
//                        {
//                            stack.Push(funcResult);
//                        }
//                        else if (Variables.TryGetValue(varFuncName, out object o) && o is InternalDelegate lambdaExpression)
//                        {
//                            stack.Push(lambdaExpression.Invoke(funcArgs.ConvertAll(Evaluate).ToArray()));
//                        }
//                        else if (Variables.TryGetValue(varFuncName, out o) && o is Delegate delegateVar)
//                        {
//                            stack.Push(delegateVar.DynamicInvoke(funcArgs.ConvertAll(Evaluate).ToArray()));
//                        }
//                        else if (Variables.TryGetValue(varFuncName, out o) && o is MethodsGroupEncaps methodsGroupEncaps)
//                        {
//                            List<object> args = funcArgs.ConvertAll(Evaluate);
//                            List<object> modifiedArgs = null;
//                            MethodInfo methodInfo = null;

//                            for (int m = 0; methodInfo == null && m < methodsGroupEncaps.MethodsGroup.Length; m++)
//                            {
//                                modifiedArgs = new List<object>(args);

//                                methodInfo = TryToCastMethodParametersToMakeItCallable(methodsGroupEncaps.MethodsGroup[m], modifiedArgs, genericsTypes, new Type[0], methodsGroupEncaps.ContainerObject);
//                            }

//                            if (methodInfo != null)
//                                stack.Push(methodInfo.Invoke(methodsGroupEncaps.ContainerObject, modifiedArgs?.ToArray()));
//                        }
//                        else
//                        {
//                            FunctionEvaluationEventArg functionEvaluationEventArg = new FunctionEvaluationEventArg(varFuncName, funcArgs, this, genericTypes: genericsTypes, evaluateGenericTypes: GetConcreteTypes);

//                            EvaluateFunction?.Invoke(this, functionEvaluationEventArg);

//                            if (functionEvaluationEventArg.FunctionReturnedValue)
//                            {
//                                stack.Push(functionEvaluationEventArg.Value);
//                            }
//                            else
//                            {
//                                throw new ExpressionEvaluatorSyntaxErrorException($"Function [{varFuncName}] unknown in expression : [{expression.Replace("\r", "").Replace("\n", "")}]");
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    if (inObject
//                        || Context?.GetType()
//                            .GetProperties(InstanceBindingFlag)
//                            .Any(propInfo => propInfo.Name.Equals(varFuncName, StringComparisonForCasing)) == true
//                        || Context?.GetType()
//                            .GetFields(InstanceBindingFlag)
//                            .Any(fieldInfo => fieldInfo.Name.Equals(varFuncName, StringComparisonForCasing)) == true)
//                    {
//                        if (inObject && (stack.Count == 0 || stack.Peek() is ExpressionOperator))
//                            throw new ExpressionEvaluatorSyntaxErrorException($"[{varFuncMatch.Value}] must follow an object.");

//                        object obj = inObject ? stack.Pop() : Context;
//                        object keepObj = obj;
//                        Type objType = null;
//                        ValueTypeNestingTrace valueTypeNestingTrace = null;

//                        if (obj != null && TypesToBlock.Contains(obj.GetType()))
//                            throw new ExpressionEvaluatorSecurityException($"{obj.GetType().FullName} type is blocked");
//                        else if (obj is Type staticType && TypesToBlock.Contains(staticType))
//                            throw new ExpressionEvaluatorSecurityException($"{staticType.FullName} type is blocked");
//                        else if (obj is ClassOrEnumType classOrType && TypesToBlock.Contains(classOrType.Type))
//                            throw new ExpressionEvaluatorSecurityException($"{classOrType.Type} type is blocked");

//                        try
//                        {
//                            if (obj is NullConditionalNullValue)
//                            {
//                                stack.Push(obj);
//                            }
//                            else if (varFuncMatch.Groups["nullConditional"].Success && obj == null)
//                            {
//                                stack.Push(new NullConditionalNullValue());
//                            }
//                            else if (obj is BubbleExceptionContainer)
//                            {
//                                stack.Push(obj);
//                                return true;
//                            }
//                            else
//                            {
//                                VariablePreEvaluationEventArg variablePreEvaluationEventArg = new VariablePreEvaluationEventArg(varFuncName, this, obj, genericsTypes, GetConcreteTypes);

//                                PreEvaluateVariable?.Invoke(this, variablePreEvaluationEventArg);

//                                if (variablePreEvaluationEventArg.CancelEvaluation)
//                                {
//                                    throw new ExpressionEvaluatorSyntaxErrorException($"[{objType}] object has no public Property or Member named \"{varFuncName}\".", new Exception("Variable evaluation canceled"));
//                                }
//                                else if (variablePreEvaluationEventArg.HasValue)
//                                {
//                                    stack.Push(variablePreEvaluationEventArg.Value);
//                                }
//                                else
//                                {
//                                    BindingFlags flag = DetermineInstanceOrStatic(ref objType, ref obj, ref valueTypeNestingTrace);

//                                    if (!OptionStaticPropertiesGetActive && (flag & BindingFlags.Static) != 0)
//                                        throw new ExpressionEvaluatorSyntaxErrorException($"[{objType}] object has no public Property or Field named \"{varFuncName}\".");
//                                    if (!OptionInstancePropertiesGetActive && (flag & BindingFlags.Instance) != 0)
//                                        throw new ExpressionEvaluatorSyntaxErrorException($"[{objType}] object has no public Property or Field named \"{varFuncName}\".");

//                                    bool isDynamic = (flag & BindingFlags.Instance) != 0 && obj is IDynamicMetaObjectProvider && obj is IDictionary<string, object>;
//                                    IDictionary<string, object> dictionaryObject = obj as IDictionary<string, object>;

//                                    MemberInfo member = isDynamic ? null : objType?.GetProperty(varFuncName, flag);
//                                    dynamic varValue = null;
//                                    bool assign = true;

//                                    if (member == null && !isDynamic)
//                                        member = objType.GetField(varFuncName, flag);

//                                    if (member == null && !isDynamic)
//                                    {
//                                        MethodInfo[] methodsGroup = objType.GetMember(varFuncName, flag).OfType<MethodInfo>().ToArray();

//                                        if (methodsGroup.Length > 0)
//                                        {
//                                            varValue = new MethodsGroupEncaps()
//                                            {
//                                                ContainerObject = obj,
//                                                MethodsGroup = methodsGroup
//                                            };
//                                        }
//                                    }

//                                    bool pushVarValue = true;

//                                    if (isDynamic)
//                                    {
//                                        if (!varFuncMatch.Groups["assignationOperator"].Success || varFuncMatch.Groups["assignmentPrefix"].Success)
//                                            varValue = dictionaryObject.ContainsKey(varFuncName) ? dictionaryObject[varFuncName] : null;
//                                        else
//                                            pushVarValue = false;
//                                    }

//                                    bool isVarValueSet = false;
//                                    if (member == null && pushVarValue)
//                                    {
//                                        VariableEvaluationEventArg variableEvaluationEventArg = new VariableEvaluationEventArg(varFuncName, this, obj ?? keepObj, genericsTypes, GetConcreteTypes);

//                                        EvaluateVariable?.Invoke(this, variableEvaluationEventArg);

//                                        if (variableEvaluationEventArg.HasValue)
//                                        {
//                                            varValue = variableEvaluationEventArg.Value;
//                                            isVarValueSet = true;
//                                        }
//                                    }

//                                    if (!isVarValueSet && !isDynamic && varValue == null && pushVarValue)
//                                    {
//                                        varValue = ((dynamic)member).GetValue(obj);

//                                        if (varValue is ValueType)
//                                        {
//                                            stack.Push(valueTypeNestingTrace = new ValueTypeNestingTrace
//                                            {
//                                                Container = valueTypeNestingTrace ?? obj,
//                                                Member = member,
//                                                Value = varValue
//                                            });

//                                            pushVarValue = false;
//                                        }
//                                    }

//                                    if (pushVarValue)
//                                    {
//                                        stack.Push(varValue);
//                                    }

//                                    if (OptionPropertyOrFieldSetActive)
//                                    {
//                                        if (varFuncMatch.Groups["assignationOperator"].Success)
//                                        {
//                                            varValue = ManageKindOfAssignation(expression, ref i, varFuncMatch, () => varValue, stack);
//                                        }
//                                        else if (varFuncMatch.Groups["postfixOperator"].Success)
//                                        {
//                                            varValue = varFuncMatch.Groups["postfixOperator"].Value.Equals("++") ? varValue + 1 : varValue - 1;
//                                        }
//                                        else
//                                        {
//                                            assign = false;
//                                        }

//                                        if (assign)
//                                        {
//                                            if (isDynamic)
//                                            {
//                                                dictionaryObject[varFuncName] = varValue;
//                                            }
//                                            else if (valueTypeNestingTrace != null)
//                                            {
//                                                valueTypeNestingTrace.Value = varValue;
//                                                valueTypeNestingTrace.AssignValue();
//                                            }
//                                            else
//                                            {
//                                                ((dynamic)member).SetValue(obj, varValue);
//                                            }
//                                        }
//                                    }
//                                    else if (varFuncMatch.Groups["assignationOperator"].Success)
//                                    {
//                                        i -= varFuncMatch.Groups["assignationOperator"].Length;
//                                    }
//                                    else if (varFuncMatch.Groups["postfixOperator"].Success)
//                                    {
//                                        i -= varFuncMatch.Groups["postfixOperator"].Length;
//                                    }
//                                }
//                            }
//                        }
//                        catch (ParsingException)
//                        {
//                            throw;
//                        }
//                        catch (Exception ex)
//                        {
//                            //Transport the exception in stack.
//                            stack.Push(new BubbleExceptionContainer()
//                            {
//                                Exception = new ExpressionEvaluatorSyntaxErrorException($"[{objType}] object has no public Property or Member named \"{varFuncName}\".", ex)
//                            });
//                            i--;
//                            return true;  //Signals an error to the parsing method array call
//                        }
//                    }
//                    else
//                    {
//                        VariablePreEvaluationEventArg variablePreEvaluationEventArg = new VariablePreEvaluationEventArg(varFuncName, this, genericTypes: genericsTypes, evaluateGenericTypes: GetConcreteTypes);

//                        PreEvaluateVariable?.Invoke(this, variablePreEvaluationEventArg);

//                        if (variablePreEvaluationEventArg.CancelEvaluation)
//                        {
//                            throw new ExpressionEvaluatorSyntaxErrorException($"Variable [{varFuncName}] unknown in expression : [{expression}]");
//                        }
//                        else if (variablePreEvaluationEventArg.HasValue)
//                        {
//                            stack.Push(variablePreEvaluationEventArg.Value);
//                        }
//                        else if (defaultVariables.TryGetValue(varFuncName, out object varValueToPush))
//                        {
//                            stack.Push(varValueToPush);
//                        }
//                        else if ((Variables.TryGetValue(varFuncName, out object cusVarValueToPush)
//                                || varFuncMatch.Groups["assignationOperator"].Success
//                                || (stack.Count == 1 && stack.Peek() is ClassOrEnumType && string.IsNullOrWhiteSpace(expression.Substring(i))))
//                            && (cusVarValueToPush == null || !TypesToBlock.Contains(cusVarValueToPush.GetType())))
//                        {
//                            if (stack.Count == 1 && stack.Peek() is ClassOrEnumType classOrEnum)
//                            {
//                                if (Variables.ContainsKey(varFuncName))
//                                    throw new ExpressionEvaluatorSyntaxErrorException($"Can not declare a new variable named [{varFuncName}]. A variable with this name already exists");
//                                else if (varFuncMatch.Groups["varKeyword"].Success)
//                                    throw new ExpressionEvaluatorSyntaxErrorException("Can not declare a variable with type and var keyword.");
//                                else if (varFuncMatch.Groups["dynamicKeyword"].Success)
//                                    throw new ExpressionEvaluatorSyntaxErrorException("Can not declare a variable with type and dynamic keyword.");

//                                stack.Pop();

//                                Variables[varFuncName] = new StronglyTypedVariable
//                                {
//                                    Type = classOrEnum.Type,
//                                    Value = GetDefaultValueOfType(classOrEnum.Type),
//                                };
//                            }

//                            if (cusVarValueToPush is StronglyTypedVariable typedVariable)
//                                cusVarValueToPush = typedVariable.Value;

//                            stack.Push(cusVarValueToPush);

//                            if (OptionVariableAssignationActive)
//                            {
//                                bool assign = true;

//                                if (varFuncMatch.Groups["assignationOperator"].Success)
//                                {
//                                    cusVarValueToPush = ManageKindOfAssignation(expression, ref i, varFuncMatch, () => cusVarValueToPush, stack);
//                                }
//                                else if (varFuncMatch.Groups["postfixOperator"].Success)
//                                {
//                                    cusVarValueToPush = varFuncMatch.Groups["postfixOperator"].Value.Equals("++") ? (dynamic)cusVarValueToPush + 1 : (dynamic)cusVarValueToPush - 1;
//                                }
//                                else if (varFuncMatch.Groups["prefixOperator"].Success)
//                                {
//                                    stack.Pop();
//                                    cusVarValueToPush = varFuncMatch.Groups["prefixOperator"].Value.Equals("++") ? (dynamic)cusVarValueToPush + 1 : (dynamic)cusVarValueToPush - 1;
//                                    stack.Push(cusVarValueToPush);
//                                }
//                                else
//                                {
//                                    assign = false;
//                                }

//                                if (assign)
//                                {
//                                    AssignVariable(varFuncName, cusVarValueToPush);
//                                }
//                            }
//                            else if (varFuncMatch.Groups["assignationOperator"].Success)
//                            {
//                                i -= varFuncMatch.Groups["assignationOperator"].Length;
//                            }
//                            else if (varFuncMatch.Groups["postfixOperator"].Success)
//                            {
//                                i -= varFuncMatch.Groups["postfixOperator"].Length;
//                            }
//                        }
//                        else
//                        {
//                            Type staticType = EvaluateType(expression, ref i, varFuncName, genericsTypes);

//                            if (staticType != null)
//                            {
//                                stack.Push(new ClassOrEnumType() { Type = staticType });
//                            }
//                            else
//                            {
//                                VariableEvaluationEventArg variableEvaluationEventArg = new VariableEvaluationEventArg(varFuncName, this, genericTypes: genericsTypes, evaluateGenericTypes: GetConcreteTypes);

//                                EvaluateVariable?.Invoke(this, variableEvaluationEventArg);

//                                if (variableEvaluationEventArg.HasValue)
//                                {
//                                    stack.Push(variableEvaluationEventArg.Value);
//                                }
//                                else
//                                {
//                                    throw new ExpressionEvaluatorSyntaxErrorException($"Variable [{varFuncName}] unknown in expression : [{expression}]");
//                                }
//                            }
//                        }
//                    }

//                    i--;
//                }

//                if (varFuncMatch.Groups["sign"].Success)
//                {
//                    object temp = stack.Pop();
//                    stack.Push(varFuncMatch.Groups["sign"].Value.Equals("+") ? ExpressionOperator.UnaryPlus : ExpressionOperator.UnaryMinus);
//                    stack.Push(temp);
//                }

//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }
//    }
//}
