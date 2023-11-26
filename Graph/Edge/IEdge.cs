namespace GrammarsProcGen.Graph.Edge
{
    internal interface IEdge<out TVertex>
    {
        TVertex From { get; }
        TVertex To { get; }
    }
}
