namespace GrammarsProcGen.Graph
{
    internal static class GraphExtensions
    {
        public static IGraph<TNodeData, TEdgeData> WithConnection<TNodeData, TEdgeData>(this IGraph<TNodeData, TEdgeData> graph, in INode<TNodeData> from, in INode<TNodeData> to) =>
            graph.WithNode(from).WithNode(to).WithEdge(new Edge<TNodeData, TEdgeData>(in from, in to, default));

        public static IGraph<TNodeData, TEdgeData> WithConnection<TNodeData, TEdgeData>(this IGraph<TNodeData, TEdgeData> graph, in INode<TNodeData> from, in INode<TNodeData> to, in TEdgeData connectionData) =>
            graph.WithNode(from).WithNode(to).WithEdge(new Edge<TNodeData, TEdgeData>(in from, in to, in connectionData));
    }
}
