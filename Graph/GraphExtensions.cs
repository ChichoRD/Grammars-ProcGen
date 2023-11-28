using GrammarsProcGen.Graph.Edge;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GrammarsProcGen.Graph
{
    internal static class GraphExtensions
    {
        public static TGraph WithEdges<TVertex, TEdge, TGraph>(this TGraph graph, IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
            where TGraph : IReadOnlyGraph<TVertex, TEdge>, IAccessibleGraph<TVertex, TEdge>, IGraph<TVertex, TEdge, TGraph>
        {
            TGraph newGraph = graph;
            foreach (TEdge edge in edges)
                newGraph = newGraph.WithEdge(edge);
            return newGraph;
        }

        public static TGraph WithVertices<TVertex, TEdge, TGraph>(this TGraph graph, IEnumerable<TVertex> vertices)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
            where TGraph : IReadOnlyGraph<TVertex, TEdge>, IAccessibleGraph<TVertex, TEdge>, IGraph<TVertex, TEdge, TGraph>
        {
            TGraph newGraph = graph;
            foreach (TVertex vertex in vertices)
                newGraph = newGraph.WithVertex(vertex);
            return newGraph;
        }

        public static TGraph WithoutEdges<TVertex, TEdge, TGraph>(this TGraph graph, IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
            where TGraph : IReadOnlyGraph<TVertex, TEdge>, IAccessibleGraph<TVertex, TEdge>, IGraph<TVertex, TEdge, TGraph>
        {
            TGraph newGraph = graph;
            foreach (TEdge edge in edges)
                newGraph = newGraph.WithoutEdge(edge);
            return newGraph;
        }

        public static TGraph WithoutVertices<TVertex, TEdge, TGraph>(this TGraph graph, IEnumerable<TVertex> vertices)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
            where TGraph : IReadOnlyGraph<TVertex, TEdge>, IAccessibleGraph<TVertex, TEdge>, IGraph<TVertex, TEdge, TGraph>
        {
            TGraph newGraph = graph;
            foreach (TVertex vertex in vertices)
                newGraph = newGraph.WithoutVertex(vertex);
            return newGraph;
        }

        public static bool Isomorphic<TVertex, TEdge>(this IReadOnlyGraph<TVertex, TEdge> graph, IReadOnlyGraph<TVertex, TEdge> other)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge> =>
            graph.Edges.ToHashSet().SetEquals(other.Edges.ToHashSet());

        public static bool IsSubgraphOf<TVertex, TEdge>(this IReadOnlyGraph<TVertex, TEdge> graph, IReadOnlyGraph<TVertex, TEdge> superGraph)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge> =>
            !graph.Edges.Except(superGraph.Edges).Any();

        public static bool IsSupergraphOf<TVertex, TEdge>(this IReadOnlyGraph<TVertex, TEdge> graph, IReadOnlyGraph<TVertex, TEdge> subGraph)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge> =>
            subGraph.IsSubgraphOf(graph);

        public static bool IsProperSubgraphOf<TVertex, TEdge>(this IReadOnlyGraph<TVertex, TEdge> graph, IReadOnlyGraph<TVertex, TEdge> superGraph)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge> =>
            graph.IsSubgraphOf(superGraph) && !graph.Isomorphic(superGraph);

        public static bool IsProperSupergraphOf<TVertex, TEdge>(this IReadOnlyGraph<TVertex, TEdge> graph, IReadOnlyGraph<TVertex, TEdge> subGraph)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge> =>
            graph.IsSupergraphOf(subGraph) && !graph.Isomorphic(subGraph);

        public static IEnumerable<TVertex> GetIsolatedVertices<TVertex, TEdge, TGraph>(this TGraph graph)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
            where TGraph : IReadOnlyGraph<TVertex, TEdge>, IAccessibleGraph<TVertex, TEdge>
        {
            IList<TVertex> isolatedVertices = new List<TVertex>();
            foreach (TVertex vertex in graph.Vertices)
                if (!graph.GetEdges(vertex).Any())
                    isolatedVertices.Add(vertex);

            return isolatedVertices;
        }

        public static TGraph WithoutIsolatedVertices<TVertex, TEdge, TGraph>(this TGraph graph)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
            where TGraph : IReadOnlyGraph<TVertex, TEdge>, IAccessibleGraph<TVertex, TEdge>, IGraph<TVertex, TEdge, TGraph> =>
            graph.WithoutVertices<TVertex, TEdge, TGraph>(graph.GetIsolatedVertices<TVertex, TEdge, TGraph>());

        public static TGraph WithoutIsolatedVertices<TVertex, TEdge, TGraph>(this TGraph graph, out IEnumerable<TVertex> isolatedVertices)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
            where TGraph : IReadOnlyGraph<TVertex, TEdge>, IAccessibleGraph<TVertex, TEdge>, IGraph<TVertex, TEdge, TGraph>
        {
            isolatedVertices = graph.GetIsolatedVertices<TVertex, TEdge, TGraph>();
            return graph.WithoutVertices<TVertex, TEdge, TGraph>(isolatedVertices);
        }

        public static TGraph ReplaceSubgraphWith<TVertex, TEdge, TGraph>(this TGraph graph, IReadOnlyGraph<TVertex, TEdge> subGraph, IReadOnlyGraph<TVertex, TEdge> replacement)
            where TEdge : IEdge<TVertex>, IEquatable<TEdge>
            where TGraph : IReadOnlyGraph<TVertex, TEdge>, IAccessibleGraph<TVertex, TEdge>, IGraph<TVertex, TEdge, TGraph>
        {
            if (!graph.IsSupergraphOf(subGraph)
                || subGraph.Isomorphic(replacement))
                return graph;

            return graph.WithoutEdges<TVertex, TEdge, TGraph>(subGraph.Edges)
                .WithoutIsolatedVertices<TVertex, TEdge, TGraph>()
                .WithEdges<TVertex, TEdge, TGraph>(replacement.Edges);
        }
    }
}