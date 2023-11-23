using System.Collections.Generic;

namespace GrammarsProcGen.Graph
{
    internal class Graph<TNodeData, TEdgeData> : IGraph<TNodeData, TEdgeData>
    {
        private readonly HashSet<INode<TNodeData>> _nodes;
        private readonly HashSet<IEdge<TNodeData, TEdgeData>> _edges;

        public Graph(HashSet<INode<TNodeData>> nodes, HashSet<IEdge<TNodeData, TEdgeData>> edges)
        {
            _nodes = nodes;
            _edges = edges;
        }

        public Graph() : this(new HashSet<INode<TNodeData>>(), new HashSet<IEdge<TNodeData, TEdgeData>>()) { }

        public IReadOnlyCollection<INode<TNodeData>> Nodes { get => _nodes; }
        public IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> Edges { get => _edges; }

        public IGraph<TNodeData, TEdgeData> WithNode<TNode>(TNode node) where TNode : INode<TNodeData> =>
            new Graph<TNodeData, TEdgeData>(new HashSet<INode<TNodeData>>(_nodes) { node }, _edges);

        public IGraph<TNodeData, TEdgeData> WithEdge<TEdge>(TEdge edge) where TEdge : IEdge<TNodeData, TEdgeData> =>
            new Graph<TNodeData, TEdgeData>(_nodes, new HashSet<IEdge<TNodeData, TEdgeData>>(_edges) { edge })
            .WithNode(edge.From)
            .WithNode(edge.To);

        public IGraph<TNodeData, TEdgeData> WithoutNode<TNode>(TNode node) where TNode : INode<TNodeData>
        {
            HashSet<IEdge<TNodeData, TEdgeData>> newEdges = new HashSet<IEdge<TNodeData, TEdgeData>>(_edges);
            HashSet<INode<TNodeData>> newNodes = new HashSet<INode<TNodeData>>(_nodes);

            foreach (IEdge<TNodeData, TEdgeData> edge in _edges)
            {
                if (edge.From.Equals(node) || edge.To.Equals(node))
                    newEdges.Remove(edge);
            }

            newNodes.Remove(node);
            return new Graph<TNodeData, TEdgeData>(newNodes, newEdges);
        }

        public IGraph<TNodeData, TEdgeData> WithoutEdge<TEdge>(TEdge edge) where TEdge : IEdge<TNodeData, TEdgeData>
        {
            HashSet<IEdge<TNodeData, TEdgeData>> newEdges = new HashSet<IEdge<TNodeData, TEdgeData>>(_edges);

            newEdges.Remove(edge);
            return new Graph<TNodeData, TEdgeData>(_nodes, newEdges);
        }
    }
}
