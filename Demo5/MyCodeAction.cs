using System.Threading;
using System.Windows.Media;
using Roslyn.Compilers.CSharp;
using Roslyn.Compilers.Common;
using Roslyn.Services;
using Roslyn.Services.Editor;
using Roslyn.Compilers;
namespace Demo5
{
	public class MyCodeAction : ICodeAction
	{
		private readonly ICodeActionEditFactory _editFactory;
        private readonly IDocument _document;
        private readonly CommonSyntaxNode _node;

        public string Description { get; private set; }
        public ImageSource Icon { get; private set; }

		public MyCodeAction(ICodeActionEditFactory editFactory, IDocument document, CommonSyntaxNode node)
        {
            _editFactory = editFactory;
            _document = document;
            _node = node;

            Description = "Replace 'a' with 'b'";
            Icon = null;
        }
		
		public ICodeActionEdit GetEdit(CancellationToken cancellationToken = new CancellationToken())
		{
			var newVariableDeclarator = Syntax.VariableDeclarator(Syntax.Identifier(_node.GetFullText().Replace('a', 'b')));

			var currentVariableDeclarator = (VariableDeclaratorSyntax)_node;						

			var tree = (SyntaxTree)_document.GetSyntaxTree();
			var newRoot = tree.Root.ReplaceNode(currentVariableDeclarator, newVariableDeclarator);
			
			return _editFactory.CreateTreeTransformEdit(_document.Project.Solution, tree, newRoot);
		}
		
	}
}