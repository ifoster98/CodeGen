using CommandLine;
using System;
using System.Linq;
using static CodeGen.Functions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;

namespace CodeGen
{
    public class AllOptions {
        [Option('s', "source", Required = true, HelpText = "Input file.")]
        public string Source { get; set; }

        [Option('o', "output", Required = false, Default = "./output", HelpText = "Output directory.")]
        public string OutputDirectory { get; set; }

        [Option('b', "business", Required = true, HelpText = "Business Identifier")]
        public string Business { get; set; }

        [Option('a', "application", Required = true, HelpText = "Application Identifier")]
        public string Application { get; set; }

        [Option('m', "module", Required = true, HelpText = "Module Identifier")]
        public string Module { get; set; }
    }

    class Program
    {
        static int Main(string[] args) {
            return Parser.Default.ParseArguments<AllOptions>(args)
                .MapResult(
                    (AllOptions opts) => RunTypesAndReturnExitCode(opts),
                    errs => 1);
        }

        private static int RunTypesAndReturnExitCode(AllOptions opts)
        {
            var typeAliases = ParseTypeAliases(opts);
            var directories = ProjectStructure.CreateDirectories(opts);
            GenerateTypeAliases(typeAliases, GetOutputDirectory(directories.DomainDirectory));
            GenerateTypeAliasesTests(typeAliases, GetOutputDirectory(directories.TestDirectory));
            return 0;

            string GetOutputDirectory(string directoryName) => $"{opts.OutputDirectory}/{directoryName}";
        }
    }
}
