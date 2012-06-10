using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using Roslyn.Scripting;
using Roslyn.Scripting.CSharp;

namespace Demo1
{
	class Program
	{
		static void Main(string[] args)
		{			
			var syntaxTree = SyntaxTree.ParseCompilationUnit("namespace DemoNamespace { public class Printer { public void Print() { System.Console.WriteLine(\"Yo!\"); }  } }");
			
			var references = new List<AssemblyFileReference>()
			                 	{
			                 		new AssemblyFileReference(typeof(Console).Assembly.Location)
			                 	};
			

			var compilationOptions = new CompilationOptions(assemblyKind: AssemblyKind.DynamicallyLinkedLibrary);

			var compilation = Compilation.Create("MyDemo", compilationOptions, new[] { syntaxTree }, references);

			// Generate the assembly into a memory stream
			var memStream = new MemoryStream();
			compilation.Emit(memStream);

			var assembly = Assembly.Load(memStream.GetBuffer());
			dynamic instance = Activator.CreateInstance(assembly.GetTypes().First());
			instance.Print();
		}
	}	
}
