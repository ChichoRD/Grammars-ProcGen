using System;

namespace GrammarsProcGen.Graph.Vertex
{
    internal interface IVertex<out TData>
    {
        TData Data { get; }
    }
}
