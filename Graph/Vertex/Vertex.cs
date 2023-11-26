using System;
namespace GrammarsProcGen.Graph.Vertex
{
    internal readonly struct Vertex<TData> : IVertex<TData>,
        IEquatable<IVertex<TData>>,
        IEquatable<Vertex<TData>>
        where TData : IEquatable<TData>
    {
        public TData Data { get; }

        public Vertex(TData data)
        {
            Data = data;
        }


        public bool Equals(IVertex<TData> other) => Data.Equals(other.Data);
        public bool Equals(Vertex<TData> other) => Data.Equals(other.Data);
    }
}
