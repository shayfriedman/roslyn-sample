using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;

namespace DemoCool
{
	class Program
	{
		static void Main(string[] args)
		{
			var tree =
				SyntaxTree.ParseCompilationUnit(@"
		public void Run()
		{
			unbreakable
			{
				Console.WriteLine(""Yo"");
			}			
		}
	");

			UnbreakableRewriter rewriter = new UnbreakableRewriter();
			Console.WriteLine(rewriter.Visit(tree.Root).GetFullText());
		}
	}

	public class UnbreakableRewriter : SyntaxRewriter
	{
		private BlockSyntax _blockToProtect;

		protected override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
		{
			// Locate the block that is related to the unbreakable keyword
			// Don't do anything with it yet, just save the block and remove the unbreakable identifier
			if (node.PlainName == "unbreakable")
			{
				var block = (from child in node.Parent.Parent.ChildNodes()
				             where child.Kind == SyntaxKind.Block
				             select child).FirstOrDefault();
				
				_blockToProtect = block as BlockSyntax;

				return Syntax.IdentifierName("");				
			}
			return base.VisitIdentifierName(node);
		}

		protected override SyntaxNode VisitBlock(BlockSyntax node)
		{
			// If we have an unbreakable node, replace it with a try-catch expression (using the current node as the try block)
			if (node == _blockToProtect)
			{
				return Syntax.TryStatement(block: node, catches: Syntax.CatchClause(block: Syntax.Block()));
			}
			return base.VisitBlock(node);
		}
	}
}
