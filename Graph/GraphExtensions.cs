using GrammarsProcGen.Graph.Edge;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GrammarsProcGen.Graph
{
    internal static class GraphExtensions
    {
        public static TGraph WithEdges<TVertex, TEdge, TGraph>(this TGraph graph, IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
            where TGraph : struct, IReadOnlyGraph<TVertex, TEdge>, IAccessibleGraph<TVertex, TEdge>, IGraph<TVertex, TEdge, TGraph>
        {
            TGraph newGraph = graph;
            foreach (TEdge edge in edges)
                newGraph = newGraph.WithEdge(edge);
            return newGraph;
        }

        public static TGraph WithVertices<TVertex, TEdge, TGraph>(this TGraph graph, IEnumerable<TVertex> vertices)
            where TEdge : IEdge<TVertex>
            where TGraph : struct, IReadOnlyGraph<TVertex, TEdge>, IAccessibleGraph<TVertex, TEdge>, IGraph<TVertex, TEdge, TGraph>
        {
            TGraph newGraph = graph;
            foreach (TVertex vertex in vertices)
                newGraph = newGraph.WithVertex(vertex);
            return newGraph;
        }

        public static TGraph WithoutEdges<TVertex, TEdge, TGraph>(this TGraph graph, IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
            where TGraph : struct, IReadOnlyGraph<TVertex, TEdge>, IAccessibleGraph<TVertex, TEdge>, IGraph<TVertex, TEdge, TGraph>
        {
            TGraph newGraph = graph;
            foreach (TEdge edge in edges)
                newGraph = newGraph.WithoutEdge(edge);
            return newGraph;
        }

        public static TGraph WithoutVertices<TVertex, TEdge, TGraph>(this TGraph graph, IEnumerable<TVertex> vertices)
            where TEdge : IEdge<TVertex>
            where TGraph : struct, IReadOnlyGraph<TVertex, TEdge>, IAccessibleGraph<TVertex, TEdge>, IGraph<TVertex, TEdge, TGraph>
        {
            TGraph newGraph = graph;
            foreach (TVertex vertex in vertices)
                newGraph = newGraph.WithoutVertex(vertex);
            return newGraph;
        }

        public static bool IsGraphOf<TVertex, TEdge, TGraph>(this IRelatableGraph<TVertex, TEdge, TGraph> graph, TGraph other)
            where TEdge : IEdge<TVertex>
            where TGraph : IReadOnlyGraph<TVertex, TEdge> =>
            graph.IsGraphOf(other, EqualityComparer<TVertex>.Default, EqualityComparer<TEdge>.Default);

        public static bool IsSubgraphOf<TVertex, TEdge, TGraph>(this IRelatableGraph<TVertex, TEdge, TGraph> graph, TGraph superGraph)
            where TEdge : IEdge<TVertex>
            where TGraph : IReadOnlyGraph<TVertex, TEdge> =>
            graph.IsSubgraphOf(superGraph, EqualityComparer<TVertex>.Default, EqualityComparer<TEdge>.Default);

        public static bool IsSupergraphOf<TVertex, TEdge, TGraph>(this IRelatableGraph<TVertex, TEdge, TGraph> graph, TGraph subGraph)
            where TEdge : IEdge<TVertex>
            where TGraph : IReadOnlyGraph<TVertex, TEdge> =>
            graph.IsSupergraphOf(subGraph, EqualityComparer<TVertex>.Default, EqualityComparer<TEdge>.Default);

        public static bool IsProperSubgraphOf<TVertex, TEdge, TGraph>(this IRelatableGraph<TVertex, TEdge, TGraph> graph, TGraph superGraph)
            where TEdge : IEdge<TVertex>
            where TGraph : IReadOnlyGraph<TVertex, TEdge> =>
            graph.IsProperSubgraphOf(superGraph, EqualityComparer<TEdge>.Default, EqualityComparer<TVertex>.Default);

        public static bool IsProperSupergraphOf<TVertex, TEdge, TGraph>(this IRelatableGraph<TVertex, TEdge, TGraph> graph, TGraph subGraph)
            where TEdge : IEdge<TVertex>
            where TGraph : IReadOnlyGraph<TVertex, TEdge> =>
            graph.IsProperSupergraphOf(subGraph, EqualityComparer<TEdge>.Default, EqualityComparer<TVertex>.Default);

        public static IEnumerable<TEdge> GetEdges<TVertex, TEdge>(this IAccessibleGraph<TVertex, TEdge> graph, TVertex from, TVertex to)
            where TEdge : IEdge<TVertex> =>
            graph.GetEdges(from).Intersect(graph.GetEdges(to));

        public static bool HasConnections<TVertex, TEdge>(this IAccessibleGraph<TVertex, TEdge> graph, TVertex from, TVertex to, out IEnumerable<TEdge> connections)
            where TEdge : IEdge<TVertex> =>
            (connections = graph.GetEdges(from, to)).Any();

        public static IEnumerable<TVertex> GetIsolatedVertices<TVertex, TEdge, TGraph>(this TGraph graph)
            where TEdge : IEdge<TVertex>
            where TGraph : IReadOnlyGraph<TVertex, TEdge>, IAccessibleGraph<TVertex, TEdge>
        {
            IList<TVertex> isolatedVertices = new List<TVertex>();
            foreach (TVertex vertex in graph.Vertices)
                if (!graph.GetEdges(vertex).Any())
                    isolatedVertices.Add(vertex);

            return isolatedVertices;
        }

        public static TGraph WithoutIsolatedVertices<TVertex, TEdge, TGraph>(this TGraph graph)
            where TEdge : IEdge<TVertex>
            where TGraph : struct, IReadOnlyGraph<TVertex, TEdge>, IAccessibleGraph<TVertex, TEdge>, IGraph<TVertex, TEdge, TGraph> =>
            graph.WithoutVertices<TVertex, TEdge, TGraph>(graph.GetIsolatedVertices<TVertex, TEdge, TGraph>());

        public static TGraph WithoutIsolatedVertices<TVertex, TEdge, TGraph>(this TGraph graph, out IEnumerable<TVertex> isolatedVertices)
            where TEdge : IEdge<TVertex>
            where TGraph : struct, IReadOnlyGraph<TVertex, TEdge>, IAccessibleGraph<TVertex, TEdge>, IGraph<TVertex, TEdge, TGraph>
        {
            isolatedVertices = graph.GetIsolatedVertices<TVertex, TEdge, TGraph>();
            return graph.WithoutVertices<TVertex, TEdge, TGraph>(isolatedVertices);
        }

        public static TGraph ReplaceSubgraphWith<TVertex, TEdge, TGraph>(this TGraph graph, IReadOnlyGraph<TVertex, TEdge> subGraph, IReadOnlyGraph<TVertex, TEdge> replacement, IEqualityComparer<TVertex> vertexEqualityComparer, IEqualityComparer<TEdge> edgeEqualityComparer, out bool success)
            where TEdge : IEdge<TVertex>
            where TGraph : struct, IReadOnlyGraph<TVertex, TEdge>, IAccessibleGraph<TVertex, TEdge>, IGraph<TVertex, TEdge, TGraph>, IRelatableGraph<TVertex, TEdge, IReadOnlyGraph<TVertex, TEdge>> =>
            (success = graph.IsSupergraphOf(subGraph, vertexEqualityComparer, edgeEqualityComparer))
            ? graph.WithoutIsolatedVertices<TVertex, TEdge, TGraph>()
                 .WithoutEdges<TVertex, TEdge, TGraph>(subGraph.Edges)
                 .WithVertices<TVertex, TEdge, TGraph>(replacement.Vertices)
                 .WithEdges<TVertex, TEdge, TGraph>(replacement.Edges)
            : graph;

        public static TGraph ReplaceSubgraphWith<TVertex, TEdge, TGraph>(this TGraph graph, IReadOnlyGraph<TVertex, TEdge> subGraph, IReadOnlyGraph<TVertex, TEdge> replacement, out bool success)
            where TEdge : IEdge<TVertex>
            where TGraph : struct, IReadOnlyGraph<TVertex, TEdge>, IAccessibleGraph<TVertex, TEdge>, IGraph<TVertex, TEdge, TGraph>, IRelatableGraph<TVertex, TEdge, IReadOnlyGraph<TVertex, TEdge>> =>
            graph.ReplaceSubgraphWith(subGraph, replacement, EqualityComparer<TVertex>.Default, EqualityComparer<TEdge>.Default, out success);
    }
}