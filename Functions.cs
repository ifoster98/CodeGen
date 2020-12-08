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

        public static List<TypeAlias> ParseTypeAliases(AllOptions opts) 
        {
            var types = File
                .ReadAllText(opts.Source)
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
                    return new TypeAlias(opts.Business, opts.Application, opts.Module, propertyName, propertyType);
                }
                return new TypeAlias(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            }
        }

        public static CompilationUnitSyntax ParseSyntaxTree(string filename) =>
            CSharpSyntaxTree
                .ParseText(File.ReadAllText(filename))
                .GetRoot() as CompilationUnitSyntax;

        public static List<RecordType> ParseRecordTypes(string moduleName, string dtoFile)
        {
            var contents = File.ReadAllText(dtoFile);
            var syntaxTree = CSharpSyntaxTree.ParseText(contents);
            var root = syntaxTree.GetRoot() as CompilationUnitSyntax;
            var namespaceSyntax = root.Members.OfType<NamespaceDeclarationSyntax>().First();
            return namespaceSyntax.Members.OfType<ClassDeclarationSyntax>()
                .Select(pc => ParseClass(moduleName, pc))
                .ToList();
        }

        public static RecordType ParseClass(string moduleName, ClassDeclarationSyntax programClassSyntax) {
            var properties = programClassSyntax.Members.OfType<PropertyDeclarationSyntax>();
            var entityName = programClassSyntax.Identifier.Text;
            var props = properties
                .Select(p => new PropertyDefinition(p.Identifier.Text, p.Type.ToString(), p.Type.ToString()))
                .ToList();
            return new RecordType(moduleName, entityName, props);
        }

        private static void WriteFile(string directory, string typename, string contents) 
        { 
            var filename = $"{directory}/{Path.DirectorySeparatorChar}{typename}.cs";
            if(!File.Exists(filename))
                File.WriteAllText(filename, contents);
        }

        private static string GetTemplate(string templateName) => 
            File.ReadAllText($"{templateDirectory}{Path.DirectorySeparatorChar}{templateName}.liquid");

        private static string GenerateTypeAlias(string simpleTemplate, TypeAlias typeAlias) =>
            Template.ParseLiquid(simpleTemplate).Render(new { typealias = typeAlias });   

        private static string GenerateRecordType(string recordTemplate, RecordType recordType) =>
            Template.ParseLiquid(recordTemplate).Render(new { recordtype = recordType });   

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

        public static void GenerateRecordTypes(
            List<RecordType> recordTypes, 
            string template, 
            string filenamePattern,
            string directory)
        {
            recordTypes.ForEach(recordType => {
                GenerateRecordType(recordType, template, filenamePattern, directory);
            });
        }

        public static void GenerateRecordType(
            RecordType recordType, 
            string template, 
            string filenamePattern,
            string directory)
        {
            var recordTemplate = GetTemplate(template);
            var recordTypeCode = GenerateRecordType(recordTemplate, recordType);
            WriteFile(directory, $"{recordType.Entityname}{filenamePattern}", recordTypeCode);
        }
    }
}
