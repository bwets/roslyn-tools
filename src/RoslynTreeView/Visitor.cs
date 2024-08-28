using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynTreeView
{
    public static class Visitor
    {
        public static async Task Start(TreeView treeView, string filename)
        {
            treeView.Nodes.Clear();
            if (!File.Exists(filename))
            {
                return;
            }
            var code = await File.ReadAllTextAsync(filename);
            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            IEnumerable<Diagnostic> diagnostics = tree.GetDiagnostics();
            if (diagnostics.Any())
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Error(s)");
                foreach (var diagnostic in diagnostics)
                {
                    sb.AppendLine(diagnostic.ToString());
                }

                MessageBox.Show(null,sb.ToString(),"Source file errors", MessageBoxButtons.OK);
                return;
            }

            SyntaxNode root = tree.GetRoot();
            var node = Visit(treeView,root);
            treeView.Nodes.Add(node);
            node.Expand();

        }

        private static TreeNode Visit(TreeView tree,SyntaxNode node)
        {
            TreeNode treeNode = new TreeNode(node.GetType().Name)
            {
                NodeFont = new Font(tree.Font.FontFamily, tree.Font.Size, FontStyle.Bold),
                Tag = new NodeData()
                {
                    Node = node
                }
            };

            if (node is ClassDeclarationSyntax cls)
            {
                treeNode.Text += $" ({cls.Identifier.Text})";
                
            }
            else if (node is NamespaceDeclarationSyntax ns)
            {
                treeNode.Text += $" ({ns.Name})";
            }
            else if (node is MethodDeclarationSyntax method)
            {
                treeNode.Text += $" ({method.Identifier.Text})";
            }
            else if (node is PropertyDeclarationSyntax property)
            {
                treeNode.Text += $" ({property.Identifier.Text})";
            }

            else if (node is FieldDeclarationSyntax field)
            {
                treeNode.Text += $" ({field.Declaration.Variables.First().Identifier})";
            }

            foreach (SyntaxNodeOrToken nodeOrToken in node.ChildNodesAndTokens())
            {
                if (nodeOrToken.IsNode)
                {
                    var d = nodeOrToken.AsNode();
                }else if (nodeOrToken.IsToken)
                {

                }
            }
            foreach (var d in node.ChildNodes())
            {
                treeNode.Nodes.Add(Visit(tree,d));
            }

            foreach (SyntaxToken t in node.ChildTokens())
            {
                treeNode.Nodes.Add(new TreeNode()
                {
                    Text = t.Kind().ToString(),
                    Tag = new NodeData()
                    {
                        Token = t
                    }
                });

            }
            return treeNode;
        }


    }
}
