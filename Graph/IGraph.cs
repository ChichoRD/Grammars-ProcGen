namespace GrammarsProcGen.Graph
{
    internal interface IGraph<TNodeData, TEdgeData> : IReadOnlyGraph<TNodeData, TEdgeData>
    {
        IGraph<TNodeData, TEdgeData> WithNode<TNode>(TNode node)
            where TNode : INode<TNodeData>;
        IGraph<TNodeData, TEdgeData> WithEdge<TEdge>(TEdge edge)
            where TEdge : IEdge<TNodeData, TEdgeData>;
        IGraph<TNodeData, TEdgeData> WithoutNode<TNode>(TNode node)
            where TNode : INode<TNodeData>;
        IGraph<TNodeData, TEdgeData> WithoutEdge<TEdge>(TEdge edge)
            where TEdge : IEdge<TNodeData, TEdgeData>;
    }
}
