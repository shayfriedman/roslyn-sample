using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;

namespace Demo3
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




			JohnnyRewriter rewriter = new JohnnyRewriter();
			Console.WriteLine(rewriter.Visit(tree.Root).GetFullText());
		}
	}





	public class JohnnyRewriter : SyntaxRewriter
	{		
		protected override SyntaxNode VisitCatchClause(CatchClauseSyntax node)
		{
			if (node.Block.Statements.Count == 0)
			{				
				return Syntax.CatchClause(
				  block: Syntax.Block(statements:Syntax.ThrowStatement()));
			}
			return base.VisitCatchClause(node);
		}
	}
}
