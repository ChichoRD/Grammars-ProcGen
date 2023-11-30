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

        public static IEnumerable<TEdge> GetEdges<TVertex, TEdge>(this IAccessibleGraph<TVertex, TEdge> graph, TVertex from, TVertex to)
            where TEdge : IEdge<TVertex> =>
            graph.GetEdges(from).Intersect(graph.GetEdges(to));

        public static bool HasConnections<TVertex, TEdge>(this IAccessibleGraph<TVertex, TEdge> graph, TVertex from, TVertex to, out IEnumerable<TEdge> connections)
            where TEdge : IEdge<TVertex> =>
            (connections = graph.GetEdges(from, to)).Any();

        public static bool IsGraph<TVertex, TEdge>(this IReadOnlyGraph<TVertex, TEdge> graph, IReadOnlyGraph<TVertex, TEdge> other)
            where TEdge : IEdge<TVertex> =>
            !graph.Vertices.Except(other.Vertices).Any()
            && !graph.Edges.Except(other.Edges).Any();

        public static bool IsSubgraphOf<TVertex, TEdge>(this IReadOnlyGraph<TVertex, TEdge> graph, IReadOnlyGraph<TVertex, TEdge> superGraph)
            where TEdge : IEdge<TVertex> =>
            !graph.Vertices.Except(superGraph.Vertices).Any()
            && !graph.Edges.Except(superGraph.Edges).Any();

        public static bool IsSupergraphOf<TVertex, TEdge>(this IReadOnlyGraph<TVertex, TEdge> graph, IReadOnlyGraph<TVertex, TEdge> subGraph)
            where TEdge : IEdge<TVertex> =>
            subGraph.IsSubgraphOf(graph);

        public static bool IsProperSubgraphOf<TVertex, TEdge>(this IReadOnlyGraph<TVertex, TEdge> graph, IReadOnlyGraph<TVertex, TEdge> superGraph)
            where TEdge : IEdge<TVertex> =>
            graph.IsSubgraphOf(superGraph) && !graph.IsGraph(superGraph);

        public static bool IsProperSupergraphOf<TVertex, TEdge>(this IReadOnlyGraph<TVertex, TEdge> graph, IReadOnlyGraph<TVertex, TEdge> subGraph)
            where TEdge : IEdge<TVertex> =>
            graph.IsSupergraphOf(subGraph) && !graph.IsGraph(subGraph);

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

        public static TGraph ReplaceSubgraphWith<TVertex, TEdge, TGraph>(this TGraph graph, IReadOnlyGraph<TVertex, TEdge> subGraph, IReadOnlyGraph<TVertex, TEdge> replacement, out bool success)
            where TEdge : IEdge<TVertex>
            where TGraph : struct, IReadOnlyGraph<TVertex, TEdge>, IAccessibleGraph<TVertex, TEdge>, IGraph<TVertex, TEdge, TGraph> =>
            (success = graph.IsSupergraphOf(subGraph))
            ? graph.WithoutIsolatedVertices<TVertex, TEdge, TGraph>()
                 .WithoutEdges<TVertex, TEdge, TGraph>(subGraph.Edges)
                 .WithVertices<TVertex, TEdge, TGraph>(replacement.Vertices)
                 .WithEdges<TVertex, TEdge, TGraph>(replacement.Edges)
            : graph;
    }
}