using GrammarsProcGen.Graph.Edge;
using System.Collections.Generic;
using System.Linq;

namespace GrammarsProcGen.Graph
{
    internal readonly struct BidirectionalGraph<TVertex, TEdge> : IGraph<TVertex, TEdge, BidirectionalGraph<TVertex, TEdge>>, IReadOnlyGraph<TVertex, TEdge>, IAccessibleGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private readonly Dictionary<TVertex, HashSet<TEdge>> _verticesEdgesPairs;
        private readonly HashSet<TEdge> _edges;

        public IReadOnlyCollection<TVertex> Vertices => _verticesEdgesPairs.Keys;
        public IReadOnlyCollection<TEdge> Edges => _edges;

        public BidirectionalGraph(IEnumerable<TVertex> vertices, IEnumerable<TEdge> edges)
        {
            _verticesEdgesPairs = new Dictionary<TVertex, HashSet<TEdge>>();
            foreach (TVertex vertex in vertices)
                _verticesEdgesPairs[vertex] = new HashSet<TEdge>();

            _edges = new HashSet<TEdge>(edges);
            foreach (TEdge edge in _edges)
            {
                _verticesEdgesPairs[edge.From].Add(edge);
                _verticesEdgesPairs[edge.To].Add(edge);
            }
        }

        public BidirectionalGraph(HashSet<TVertex> vertices, HashSet<TEdge> edges)
        {
            _verticesEdgesPairs = new Dictionary<TVertex, HashSet<TEdge>>();
            foreach (TVertex vertex in vertices)
                _verticesEdgesPairs[vertex] = new HashSet<TEdge>();

            _edges = edges;
            foreach (TEdge edge in _edges)
            {
                _verticesEdgesPairs[edge.From].Add(edge);
                _verticesEdgesPairs[edge.To].Add(edge);
            }
        }

        public BidirectionalGraph(IReadOnlyGraph<TVertex, TEdge> graph)
        {
            _verticesEdgesPairs = new Dictionary<TVertex, HashSet<TEdge>>();
            foreach (TVertex vertex in graph.Vertices)
                _verticesEdgesPairs[vertex] = new HashSet<TEdge>();

            _edges = new HashSet<TEdge>(graph.Edges);
            foreach (TEdge edge in _edges)
            {
                _verticesEdgesPairs[edge.From].Add(edge);
                _verticesEdgesPairs[edge.To].Add(edge);
            }
        }

        public BidirectionalGraph(IEnumerable<TVertex> vertices) : this(vertices, Enumerable.Empty<TEdge>()) { }

        public BidirectionalGraph(IEnumerable<TEdge> edges)
        {
            _verticesEdgesPairs = new Dictionary<TVertex, HashSet<TEdge>>();
            _edges = new HashSet<TEdge>(edges);

            foreach (TEdge edge in _edges)
            {
                _verticesEdgesPairs[edge.From] = new HashSet<TEdge>(_verticesEdgesPairs[edge.From] ?? Enumerable.Empty<TEdge>()) { edge };
                _verticesEdgesPairs[edge.To] = new HashSet<TEdge>(_verticesEdgesPairs[edge.To] ?? Enumerable.Empty<TEdge>()) { edge };
            }
        }

        public static BidirectionalGraph<TVertex, TEdge> FromEmpty() =>
            new BidirectionalGraph<TVertex, TEdge>(Enumerable.Empty<TVertex>(), Enumerable.Empty<TEdge>());

        public IReadOnlyCollection<TEdge> GetEdges(TVertex vertex) =>
            _verticesEdgesPairs.TryGetValue(vertex, out HashSet<TEdge> edges) ? edges : Enumerable.Empty<TEdge>().ToList();

        public BidirectionalGraph<TVertex, TEdge> WithVertex<UVertex>(UVertex vertex) where UVertex : TVertex =>
            new BidirectionalGraph<TVertex, TEdge>(new HashSet<TVertex>(Vertices) { vertex }, Edges);

        public BidirectionalGraph<TVertex, TEdge> WithoutVertex<UVertex>(UVertex vertex) where UVertex : TVertex
        {
            HashSet<TEdge> edges = new HashSet<TEdge>(_edges);
            foreach (TEdge edge in GetEdges(vertex))
                edges.Remove(edge);

            HashSet<TVertex> vertices = new HashSet<TVertex>(Vertices);
            vertices.Remove(vertex);
            return new BidirectionalGraph<TVertex, TEdge>(vertices, edges);
        }

        public BidirectionalGraph<TVertex, TEdge> WithEdge<UEdge>(UEdge edge) where UEdge : TEdge =>
            new BidirectionalGraph<TVertex, TEdge>(new HashSet<TVertex>(Vertices) { edge.From, edge.To }, new HashSet<TEdge>(Edges) { edge });

        public BidirectionalGraph<TVertex, TEdge> WithoutEdge<UEdge>(UEdge edge) where UEdge : TEdge
        {
            HashSet<TEdge> edges = new HashSet<TEdge>(_edges);
            edges.Remove(edge);
            return new BidirectionalGraph<TVertex, TEdge>(Vertices, edges);
        }
    }
}