using GrammarsProcGen.Graph.Edge;

namespace GrammarsProcGen.Graph
{
    internal interface IGraph<in TVertex, in TEdge>
        where TEdge : IEdge<TVertex>
    {
        IGraph<TVertex, TEdge> WithVertex<UVertex>(UVertex vertex)
            where UVertex : TVertex;
        IGraph<TVertex, TEdge> WithoutVertex<UVertex>(UVertex vertex)
            where UVertex : TVertex;

        IGraph<TVertex, TEdge> WithEdge<UEdge>(UEdge edge)
            where UEdge : TEdge;
        IGraph<TVertex, TEdge> WithoutEdge<UEdge>(UEdge edge)
            where UEdge : TEdge;
    }

    internal interface IGraph<in TVertex, in TEdge, out TGraph> : IGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
        where TGraph : struct, IGraph<TVertex, TEdge, TGraph>
    {
        IGraph<TVertex, TEdge> IGraph<TVertex, TEdge>.WithVertex<UVertex>(UVertex vertex) =>
            WithVertex(vertex);
        IGraph<TVertex, TEdge> IGraph<TVertex, TEdge>.WithoutVertex<UVertex>(UVertex vertex) =>
            WithoutVertex(vertex);

        IGraph<TVertex, TEdge> IGraph<TVertex, TEdge>.WithEdge<UEdge>(UEdge edge) =>
            WithEdge(edge);
        IGraph<TVertex, TEdge> IGraph<TVertex, TEdge>.WithoutEdge<UEdge>(UEdge edge) =>
            WithoutEdge(edge);

        new TGraph WithVertex<UVertex>(UVertex vertex)
            where UVertex : TVertex;
        new TGraph WithoutVertex<UVertex>(UVertex vertex)
            where UVertex : TVertex;

        new TGraph WithEdge<UEdge>(UEdge edge)
            where UEdge : TEdge;
        new TGraph WithoutEdge<UEdge>(UEdge edge)
            where UEdge : TEdge;
    }
}
