using Scriban;
using System;
using System.Collections.Generic;
using System.IO;
using CodeGen.Domain;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeGen
{
    public static class Functions
    {
        public static string templateDirectory = "templates";

        public static List<TypeAlias> ParseTypeAliases(string moduleName, string aliasFile) 
        {
            var types = File
                .ReadAllText(aliasFile)
                .Split('\n')
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();

            return types
                .Select(type => ParseType(type))
                .Where(ta => !string.IsNullOrEmpty(ta.Modulename))
                .ToList();

            TypeAlias ParseType(string typeDef)
            {
                var regex = new Regex(@"^type (?<name>\S*) = (?<type>\S*)$");
                var match = regex.Match(typeDef);

                if (match.Success)
                {
                    var propertyName = match.Groups["name"].Value;
                    var propertyType = match.Groups["type"].Value;
                    return new TypeAlias(moduleName, propertyName, propertyType);
                }
                return new TypeAlias(string.Empty, string.Empty, string.Empty);
            }
        }

        public static CompilationUnitSyntax ParseSyntaxTree(string filename) =>
            CSharpSyntaxTree
                .ParseText(File.ReadAllText(filename))
                .GetRoot() as CompilationUnitSyntax;

        private static void WriteFile(string directory, string typename, string contents) => 
            File.WriteAllText($"{directory}{Path.DirectorySeparatorChar}{typename}.cs", contents);

        private static string GetTemplate(string templateName) => 
            File.ReadAllText($"{templateDirectory}{Path.DirectorySeparatorChar}{templateName}.liquid");

        private static string GenerateTypeAlias(string simpleTemplate, TypeAlias typeAlias) =>
            Template.ParseLiquid(simpleTemplate).Render(new { typealias = typeAlias });   

        public static void GenerateTypeAliases(List<TypeAlias> typeAliass, string directory)
        {
            var simpleTemplate = GetTemplate("typealias");
            typeAliass.ForEach(typeAlias => {
                var typeAliasCode = GenerateTypeAlias(simpleTemplate, typeAlias);
                WriteFile(directory, typeAlias.Typename, typeAliasCode);
            });
        }

        public static void GenerateTypeAliasesTests(List<TypeAlias> typeAliass, string directory)
        {
            var simpleTemplateTests = GetTemplate("typealiastests");
            typeAliass.ForEach(typeAlias => {
                var typeAliasCode = GenerateTypeAlias(simpleTemplateTests, typeAlias);
                WriteFile(directory, $"{typeAlias.Typename}Tests", typeAliasCode);
            });
        }
    }
}
