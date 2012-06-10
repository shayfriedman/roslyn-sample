using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;

namespace Demo2
{
	class Program
	{
		static void Main(string[] args)
		{
			var tree =
				SyntaxTree.ParseCompilationUnit(@"
		public void Run()
		{
			try
			{
				throw new Exception(""Yo yo yo"");
			}
			catch {  }
		}
	");



			new JohnnyWalker().Visit(tree.Root);
		}
	}





	public class JohnnyWalker : SyntaxWalker
	{				
		protected override void VisitCatchClause(CatchClauseSyntax node)
		{
			if (node.Block.Statements.Count == 0)
			{				
				Console.WriteLine("You have an empty catch block! bad developer! bad!");
			}
			base.VisitCatchClause(node);
		}
	}
}
