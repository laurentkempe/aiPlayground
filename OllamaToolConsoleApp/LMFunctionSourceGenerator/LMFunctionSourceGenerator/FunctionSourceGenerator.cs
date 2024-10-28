﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace LMFunctionSourceGenerator;

[Generator]
public class FunctionSourceGenerator : IIncrementalGenerator
{
    private const string Namespace = "Generators";
    private const string AttributeName = "FunctionAttribute";

    /*lang=cs*/

    private const string AttributeSourceCode =
        $$"""
          // <auto-generated/>

          namespace {{Namespace}}
          {
              [System.AttributeUsage(System.AttributeTargets.Method)]
              public class {{AttributeName}} : System.Attribute
              {
                 public string Description { get; }
          
                 public FunctionAttribute(string description)
                 {
                     Description = description;
                 }
              }
          }
          """;

    /*lang=cs*/

    private const string FunctionDetailsSourceCode =
        $$"""
          // <auto-generated/>

          namespace {{Namespace}}
          {
               public record FunctionDetails(
                   [property: JsonPropertyName("name")] string Name,
                   [property: JsonPropertyName("parameters")] FunctionParameters FunctionParameters,
                   string Description
               );
          }
          """;

    /*lang=cs*/
    private const string FunctionParametersSourceCode =
        $$"""
          // <auto-generated/>

          namespace {{Namespace}}
          {
          public record FunctionParameters([property: JsonPropertyName("city")] string City);
          }
          """;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        AddMarkerAttributeToCompilation(context);
        AddFunctionDetailsToCompilation(context);
        AddFunctionParametersToCompilation(context);

        // 👇🏼 Filter classes annotated with the [Function] attribute. Only filtered Syntax Nodes can trigger code generation.
        var provider = context.SyntaxProvider
            .CreateSyntaxProvider(
                (s, _) => s is MethodDeclarationSyntax,
                (ctx, _) => GetMethodDeclarationForSourceGen(ctx))
            .Where(t => t.functionAttributeFound)
            .Select((t, _) => t.Item1);

        // 👇🏼 Generate the source code.
        context.RegisterSourceOutput(context.CompilationProvider.Combine(provider.Collect()),
            (ctx, t) => GenerateCode(ctx, t.Right));
    }

    private static void AddMarkerAttributeToCompilation(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "FunctionAttribute.g.cs",
            SourceText.From(AttributeSourceCode, Encoding.UTF8)));
    }

    private static void AddFunctionDetailsToCompilation(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "FunctionDetails.g.cs",
            SourceText.From(FunctionDetailsSourceCode, Encoding.UTF8)));
    }

    private static void AddFunctionParametersToCompilation(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "FunctionParameters.g.cs",
            SourceText.From(FunctionParametersSourceCode, Encoding.UTF8)));
    }

    private static (MethodDeclarationSyntax, bool functionAttributeFound) GetMethodDeclarationForSourceGen(
        GeneratorSyntaxContext context)
    {
        var methodDeclarationSyntax = (MethodDeclarationSyntax)context.Node;

        // 👇🏼 Go through all attributes of the class.
        foreach (var attributeListSyntax in methodDeclarationSyntax.AttributeLists)
        foreach (var attributeSyntax in attributeListSyntax.Attributes)
        {
            if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                continue; // if we can't get the symbol, ignore it

            var attributeName = attributeSymbol.ContainingType.ToDisplayString();

            // 👇🏼 Check the full name of the [Function] attribute.
            if (attributeName == $"{Namespace}.{AttributeName}")
                return (methodDeclarationSyntax, true);
        }

        return (methodDeclarationSyntax, false);
    }

    /// <summary>
    ///     Generate code action.
    ///     It will be executed on specific nodes (ClassDeclarationSyntax annotated with the [Function] attribute) changed by
    ///     the user.
    /// </summary>
    /// <param name="context">Source generation context used to add source files.</param>
    /// <param name="methodDeclarations">Nodes annotated with the [Function] attribute that trigger the generate action.</param>
    private static void GenerateCode(SourceProductionContext context,
                                     ImmutableArray<MethodDeclarationSyntax> methodDeclarations)
    {
        var partialClassName = "";
        if (methodDeclarations.First().Parent is ClassDeclarationSyntax classDeclarationSyntax)
            partialClassName = classDeclarationSyntax.Identifier.Text;

        var (functionNamePatternMatching, functionDetails) =
            GenerateFunctionDetails(methodDeclarations);

        /*lang=cs*/
        var partialClassCode =
            $$$"""
               // <auto-generated/>

               using System;
               using System.Collections.Generic;

               partial class {{{partialClassName}}}
               {
                   public string Execute(FunctionDetails? function)
                   {
                       return function?.Name switch 
                       {
                            {{{functionNamePatternMatching}}}
                       };
                   }
                   
                   public List<FunctionDetails> GetFunctionDetails() =>
                   [
                       {{{functionDetails}}}
                   ];
               }
               """;

        // Add the source code to the compilation.
        context.AddSource($"{partialClassName}.g.cs", SourceText.From(partialClassCode, Encoding.UTF8));
    }

    private static List<(string name, string description)> ExtractFunctionDescriptions(
        ImmutableArray<MethodDeclarationSyntax> methodDeclarations)
    {
        var functionNames = new List<(string name, string description)>();

        // Go through all filtered method declarations.
        foreach (var methodDeclarationSyntax in methodDeclarations)
        {
            // 👇🏼 Get attribute property called Function from methodDeclarationSyntax
            var attributeSyntax = methodDeclarationSyntax.AttributeLists
                .SelectMany(x => x.Attributes)
                .First(x => x.Name.ToString() == "Function");

            var descriptionArgument = attributeSyntax.ArgumentList?.Arguments.First();
            if (descriptionArgument == null) continue;
            
            // 👇🏼 Get attribute Description property
            var descriptionLiteral = (LiteralExpressionSyntax)descriptionArgument.Expression;
            var descriptionValue = descriptionLiteral.Token.ValueText;

            functionNames.Add(
                new ValueTuple<string, string>(methodDeclarationSyntax.Identifier.Text, descriptionValue));
        }

        return functionNames;
    }

    private static (StringBuilder functionNamePatternMatching, StringBuilder functionDetails) GenerateFunctionDetails(
        ImmutableArray<MethodDeclarationSyntax> methodDeclarations)
    {
        StringBuilder functionNamePatternMatching = new();
        StringBuilder functionDetails = new();

        List<(string name, string description)> functionNames = ExtractFunctionDescriptions(methodDeclarations); 
        
        for (var i = 0; i < functionNames.Count; i++)
        {
            var functionNamesIndex = functionNames.Count - 1;
            var separator = i < functionNamesIndex ? "," : "";
            var indentFunctionName = i < functionNamesIndex ? "" : "             ";
            var indentFunctionDetails = i < functionNamesIndex ? "" : "        ";
            var newLine = i < functionNamesIndex ? "\n" : "";
            functionNamePatternMatching.Append(
                $"{indentFunctionName}\"{functionNames[i].name}\" => {functionNames[i].name}(function.FunctionParameters.City){separator}{newLine}");
            functionDetails.Append(
                $"{indentFunctionDetails}new(\"{functionNames[i].name}\", new FunctionParameters(\"city\"), \"{functionNames[i].description}\"){separator}{newLine}");
        }

        return (functionNamePatternMatching, functionDetails);
    }
}