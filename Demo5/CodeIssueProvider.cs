using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using Roslyn.Compilers;
using Roslyn.Compilers.Common;
using Roslyn.Compilers.CSharp;
using Roslyn.Services;
using Roslyn.Services.Editor;

namespace Demo5
{
	[ExportSyntaxNodeCodeIssueProvider("Demo5", LanguageNames.CSharp)]
	class CodeIssueProvider : ICodeIssueProvider
	{
		private readonly ICodeActionEditFactory editFactory;

		[ImportingConstructor]
		public CodeIssueProvider(ICodeActionEditFactory editFactory)
		{
			this.editFactory = editFactory;
		}

		public IEnumerable<CodeIssue> GetIssues(IDocument document, CommonSyntaxNode node, CancellationToken cancellationToken)
		{
			if (node.Kind == (int)SyntaxKind.VariableDeclarator)
			{
				var nodeText = node.GetText();
				if (nodeText.Contains('a'))
				{
					var issueDescription = string.Format("'{0}' contains the letter 'a'", nodeText);
					yield return
						new CodeIssue(CodeIssue.Severity.Warning, node.Span, issueDescription,
									  new MyCodeAction(editFactory, document, node));		
				}
			}
		}

		#region Unimplemented ICodeIssueProvider members

		public IEnumerable<CodeIssue> GetIssues(IDocument document, CommonSyntaxToken token, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<CodeIssue> GetIssues(IDocument document, CommonSyntaxTrivia trivia, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
