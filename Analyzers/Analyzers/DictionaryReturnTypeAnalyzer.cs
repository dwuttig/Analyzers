using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class DictionaryReturnTypeAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "AB0006";
    private static readonly LocalizableString Title = "Method returns a dictionary";
    private static readonly LocalizableString MessageFormat = "Method '{0}' returns a dictionary";
    private static readonly LocalizableString Description = "Public methods should not return a dictionary";
    private const string Category = "Design";

    private static readonly DiagnosticDescriptor Rule = new (
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeMethod, SyntaxKind.MethodDeclaration);
    }

    private static void AnalyzeMethod(SyntaxNodeAnalysisContext context)
    {
        var methodDeclaration = (MethodDeclarationSyntax)context.Node;

        // Check if the method is public
        if (!methodDeclaration.Modifiers.Any(SyntaxKind.PublicKeyword))
            return;

        // Check if the return type is a dictionary
        var returnType = methodDeclaration.ReturnType.ToString();
        if (returnType.StartsWith("Dictionary<") || returnType.StartsWith("IDictionary<"))
        {
            var diagnostic = Diagnostic.Create(Rule, methodDeclaration.GetLocation(), methodDeclaration.Identifier.ValueText);
            context.ReportDiagnostic(diagnostic);
        }
    }
}