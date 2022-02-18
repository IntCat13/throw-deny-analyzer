using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ThrowDenyAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ThrowAnalyser : DiagnosticAnalyzer
    {
        private static DiagnosticDescriptor DiagnosticDescriptor =
            new DiagnosticDescriptor(
                "EL0001",
                "Unaccepted keyword",
                "Throw is unaccepted keyword, remove to solve",
                "Clear code",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> 
            SupportedDiagnostics => ImmutableArray.Create(DiagnosticDescriptor);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            
            context.RegisterSyntaxNodeAction(
                AnalyzeThrowStatement,
                SyntaxKind.ThrowStatement);

            context.RegisterSyntaxNodeAction(
                AnalyzeThrowExpression,
                SyntaxKind.ThrowExpression);
        }

        private void AnalyzeThrowStatement(SyntaxNodeAnalysisContext context)
        {
            var node = (ThrowStatementSyntax) context.Node;

            context.ReportDiagnostic(
                Diagnostic.Create(
                    DiagnosticDescriptor,
                    node.GetLocation()));
        }

        private void AnalyzeThrowExpression(SyntaxNodeAnalysisContext context)
        {
            var node = (ThrowExpressionSyntax)context.Node;

            context.ReportDiagnostic(
                Diagnostic.Create(
                    DiagnosticDescriptor,
                    node.GetLocation()));
        }
    }
}
