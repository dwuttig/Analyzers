using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class BoolParameterAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "AB0007";
    private static readonly LocalizableString Title = "Method has multiple bool parameters";
    private static readonly LocalizableString MessageFormat = "Method '{0}' has multiple bool parameters";
    private static readonly LocalizableString Description = "Methods should not have multiple bool parameters";
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

        // Check if the method has multiple bool parameters
        var boolParameters = methodDeclaration.ParameterList.Parameters
            .Where(p => p.Type?.ToString() == "bool")
            .ToList();

        if (boolParameters.Count <= 1)
            return;

        var diagnostic = Diagnostic.Create(Rule, methodDeclaration.GetLocation(), methodDeclaration.Identifier.ValueText);
        context.ReportDiagnostic(diagnostic);
    }
}