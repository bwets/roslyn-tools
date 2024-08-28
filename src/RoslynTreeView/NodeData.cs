using Microsoft.CodeAnalysis;

namespace RoslynTreeView;

public class NodeData
{
    public SyntaxNode? Node { get; set; }
    public SyntaxToken? Token { get; set; }
}