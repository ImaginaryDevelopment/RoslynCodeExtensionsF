namespace AnalyzerF

open System
open System.Collections.Generic
open System.Collections.Immutable
open System.Linq
open System.Threading

open Microsoft.CodeAnalysis
open Microsoft.CodeAnalysis.CSharp
open Microsoft.CodeAnalysis.CSharp.Syntax
open Microsoft.CodeAnalysis.Diagnostics
module AnalyzerHelpers =
    [<Literal>]
    let diagnosticId = "AnalyzerF";

    // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
    // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
    //private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
    let title = "Type name contains lowercase letters"
    let messageFormat = "Type name '{0}' contains lowercase letters"
    let description = "Type names should be all uppercase."
    let category = "Naming"

    //private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);
    //let rule = DiagnosticDescriptor(id=diagnosticId,title= title,messageFormat= messageFormat, category=category, defaultSeverity= DiagnosticSeverity.Warning, isEnabledByDefault= true, description= description)
    let rule = DiagnosticDescriptor(diagnosticId,title, messageFormat, category, DiagnosticSeverity.Warning, true, description,null,null)

[<DiagnosticAnalyzer(LanguageNames.CSharp)>]
type AnalyzerFAnalyzer()  =
    inherit DiagnosticAnalyzer()

        override _.SupportedDiagnostics : ImmutableArray<DiagnosticDescriptor> = ImmutableArray.Create(AnalyzerHelpers.rule)
        override _.Initialize(context:AnalysisContext) =
            context.ConfigureGeneratedCodeAnalysis GeneratedCodeAnalysisFlags.None
            context.EnableConcurrentExecution()

            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSymbolAction(AnalyzerFAnalyzer.AnalyzeSymbol, SymbolKind.NamedType)

        static member AnalyzeSymbol(context:SymbolAnalysisContext) =
            // TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
            let namedTypeSymbol = context.Symbol :?> INamedTypeSymbol;

            // Find just those named type symbols with names containing lowercase letters.
            if namedTypeSymbol.Name.ToCharArray() |> Seq.exists System.Char.IsLower then
                // For all such symbols, produce a diagnostic.
                let diagnostic = Diagnostic.Create(AnalyzerHelpers.rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name)

                context.ReportDiagnostic diagnostic
