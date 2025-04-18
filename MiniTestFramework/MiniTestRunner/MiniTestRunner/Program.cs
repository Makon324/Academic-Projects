using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace MiniTestRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) // Usage
            {
                Console.WriteLine("Usage: MiniTestRunner <path/to/assembly1.dll> <path/to/assembly2.dll> ...");
                return;
            }

            foreach (var assemblyPath in args)
            {
                // Loading assembly
                Console.WriteLine($"Loading assembly: {assemblyPath}");
                Assembly assembly = Assembly.LoadFrom(assemblyPath);

                // Running tests in assembly
                var runner = new TestRunner();
                runner.ExecuteTests(assembly);
            }
        }
    }
}
