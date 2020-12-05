using CommandLine;
using System;
using System.Linq;
using static CodeGen.Functions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;

namespace CodeGen
{
    class AllOptions {
        [Option('s', "source", Required = true, HelpText = "Input file.")]
        public string Source { get; set; }

        [Option('o', "output", Required = false, Default = "./output", HelpText = "Output directory.")]
        public string OutputDirectory { get; set; }

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
            var typeAliases = ParseTypeAliases(opts.Module, opts.Source);
            var directories = ProjectStructure.CreateDirectories(opts.OutputDirectory, opts.Module);
            GenerateTypeAliases(typeAliases, GetOutputDirectory(directories.DomainDirectory));
            GenerateTypeAliasesTests(typeAliases, GetOutputDirectory(directories.TestDirectory));
            return 0;

            string GetOutputDirectory(string directoryName) => $"{opts.OutputDirectory}/{directoryName}";
        }
    }
}
