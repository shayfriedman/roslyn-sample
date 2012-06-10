using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using Roslyn.Services;

namespace Demo4
{
	class Program
	{
		
		static void Main(string[] args)
		{
			var workspace = Workspace.LoadSolution(
				@"C:\Lectures\NDC 2012 - June 2012\Roslyn... hmmm.... what\Demos\TestSolution\TestSolution.sln");

			var solution = workspace.CurrentSolution;
			
			var newSolution = solution;
			foreach (var project in solution.Projects)
			{
				foreach (var document in project.Documents)
				{
					var updatedFile = new StringText(TopOfFileComment + document.GetSyntaxTree().Root.GetFullText());

					newSolution = newSolution.UpdateDocument(document.Id, updatedFile);
				}	
			}

			if (newSolution != solution)
			{
				workspace.ApplyChanges(solution, newSolution);
			}
			
		}

		private const string TopOfFileComment = @"
// ******************************
// *   Written by yours truly   *
// ******************************
";

	}
}
