using GrammarsProcGen.Graph.Edge;
using GrammarsProcGen.Graph.Vertex;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GrammarsProcGen.Graph
{
    internal class GraphTest : MonoBehaviour
    {
        [SerializeField]
        private Bounds _bounds;

        [SerializeField]
        private int _verticesCount;

        [SerializeField]
        private int _connectionsCount;

        private BidirectionalGraph<IVertex<VertexData>, IDataEdge<IVertex<VertexData>, EdgeData>> _graph;

        private void Awake()
        {
            Generate();
            DebugGraph();
        }

        [ContextMenu(nameof(Generate))]
        private void Generate()
        {
            if (_graph.Edges != null)
                foreach (var vertex in _graph.Vertices)
                    Destroy(vertex.Data.GameObject);

            _graph = BidirectionalGraph<IVertex<VertexData>, IDataEdge<IVertex<VertexData>, EdgeData>>.FromEmpty();
            Transform transform = this.transform;
            for (int i = 0; i < _verticesCount; i++)
            {
                Vector3 position = new Vector3(Random.Range(_bounds.min.x, _bounds.max.x),
                                               Random.Range(_bounds.min.y, _bounds.max.y),
                                               Random.Range(_bounds.min.z, _bounds.max.z));

                int primitiveTypeIndex = Random.Range(0, Enum.GetValues(typeof(PrimitiveType)).Length);
                PrimitiveType primitiveType = (PrimitiveType)primitiveTypeIndex;
                Transform primitiveTransform = GameObject.CreatePrimitive(primitiveType).transform;
                primitiveTransform.position = position;
                primitiveTransform.SetParent(transform);

                _graph = _graph.WithVertex(new Vertex<VertexData>(new VertexData(primitiveTransform.gameObject, primitiveType)));
            }

            int connectionsMade = 0;
            while (connectionsMade < _connectionsCount)
            {
                IVertex<VertexData> from = _graph.Vertices.ElementAt(Random.Range(0, _graph.Vertices.Count));
                IVertex<VertexData> to = _graph.Vertices.ElementAt(Random.Range(0, _graph.Vertices.Count));

                if (from.Equals(to)
                    || _graph.HasConnections(from, to, out _))
                    continue;

                //float weight = Vector3.Distance(from.Data.GameObject.transform.position, to.Data.GameObject.transform.position);

                _graph = _graph.WithEdge(new DataEdge<IVertex<VertexData>, EdgeData>(from, to, new EdgeData(from.Data.GameObject.transform, to.Data.GameObject.transform)));
                ++connectionsMade;
            }
        }

        [ContextMenu(nameof(MutateGraph))]
        private void MutateGraph()
        {
            IVertex<VertexData> ruleVertex0 = new Vertex<VertexData>(new VertexData(null, PrimitiveType.Sphere));
            BidirectionalGraph<IVertex<VertexData>, IDataEdge<IVertex<VertexData>, EdgeData>> ruleGraph =
                BidirectionalGraph<IVertex<VertexData>, IDataEdge<IVertex<VertexData>, EdgeData>>.FromEmpty()
                .WithVertex(ruleVertex0);

            Vector3 displacement = Vector3.forward * 10.0f;
            GameObject primitiveCubeGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            primitiveCubeGameObject.transform.position = displacement;
            primitiveCubeGameObject.SetActive(false);

            IVertex<VertexData> additionVertex = new Vertex<VertexData>(new VertexData(primitiveCubeGameObject, PrimitiveType.Cube));
            IDataEdge<IVertex<VertexData>, EdgeData> additionEdge = new DataEdge<IVertex<VertexData>, EdgeData>(
                ruleVertex0,
                additionVertex,
                new EdgeData(0.0f));
            BidirectionalGraph<IVertex<VertexData>, IDataEdge<IVertex<VertexData>, EdgeData>> additionGraph =
                BidirectionalGraph<IVertex<VertexData>, IDataEdge<IVertex<VertexData>, EdgeData>>.FromEmpty()
                .WithVertex(ruleVertex0)
                .WithVertex(additionVertex)
                .WithEdge(additionEdge);

            IEqualityComparer<IVertex<VertexData>> vertexEqualityComparer = new VertexPrimitiveTypeEqualityComparer();
            IEqualityComparer<IDataEdge<IVertex<VertexData>, EdgeData>> edgeEqualityComparer = new EdgeEqualityComparer(vertexEqualityComparer);

            _graph = _graph.ReplaceSubgraphWith(
                ruleGraph,
                additionGraph,
                vertexEqualityComparer,
                edgeEqualityComparer,
                out bool success);

            if (success)
            {
                primitiveCubeGameObject.SetActive(true);
                DebugGraph();
                return;
            }

            _graph = _graph.WithoutVertex(additionVertex);
            Destroy(additionVertex.Data.GameObject);
        }

        [ContextMenu(nameof(DebugGraph))]
        private void DebugGraph()
        {
            const float DURATION = 10.0f;
            foreach (IVertex<VertexData> vertex in _graph.Vertices)
                if (vertex.Data.GameObject != null)
                    DrawCross3D(vertex.Data.GameObject.transform.position, 1.0f, Color.blue, DURATION);

            foreach (IDataEdge<IVertex<VertexData>, EdgeData> edge in _graph.Edges)
            {
                if (edge.From.Data.GameObject == null
                    || edge.To.Data.GameObject == null)
                    continue;

                Color closeColor = Color.green;
                Color farColor = Color.red;
                float maxWeight = _bounds.size.magnitude;

                float lerpFactor = edge.Data.Weight / maxWeight;
                Color color = Contrast(Color.Lerp(closeColor, farColor, lerpFactor), 4.0f);

                Debug.DrawLine(edge.From.Data.GameObject.transform.position,
                               edge.To.Data.GameObject.transform.position,
                               color,
                               DURATION);

                static Color Contrast(Color color, float slope) => new Color(
                    (color.r - 0.5f) * slope + 0.5f,
                    (color.g - 0.5f) * slope + 0.5f,
                    (color.b - 0.5f) * slope + 0.5f,
                    (color.a - 0.5f) * slope + 0.5f);
            }

            static void DrawCross3D(Vector3 position, float size, Color color = default, float duration = 0.0f)
            {
                Debug.DrawLine(position + Vector3.up * size, position + Vector3.down * size, color, duration);
                Debug.DrawLine(position + Vector3.left * size, position + Vector3.right * size, color, duration);
                Debug.DrawLine(position + Vector3.forward * size, position + Vector3.back * size, color, duration);
            }
        }

        private class EdgeEqualityComparer : EqualityComparer<IDataEdge<IVertex<VertexData>, EdgeData>>
        {
            private IEqualityComparer<IVertex<VertexData>> _vertexEqualityComparer;
            public EdgeEqualityComparer(IEqualityComparer<IVertex<VertexData>> vertexEqualityComparer)
            {
                _vertexEqualityComparer = vertexEqualityComparer;
            }

            public override bool Equals(IDataEdge<IVertex<VertexData>, EdgeData> x, IDataEdge<IVertex<VertexData>, EdgeData> y) =>
                _vertexEqualityComparer.Equals(x.From, y.From)
                && _vertexEqualityComparer.Equals(x.To, y.To);

            public override int GetHashCode(IDataEdge<IVertex<VertexData>, EdgeData> obj) =>
                HashCode.Combine(_vertexEqualityComparer.GetHashCode(obj.From), _vertexEqualityComparer.GetHashCode(obj.To));
        }

        private class VertexPrimitiveTypeEqualityComparer : EqualityComparer<IVertex<VertexData>>
        {
            public override bool Equals(IVertex<VertexData> x, IVertex<VertexData> y) =>
                x.Data.PrimitiveType.Equals(y.Data.PrimitiveType);

            public override int GetHashCode(IVertex<VertexData> obj) => obj.Data.PrimitiveType.GetHashCode();
        }

        private class VertexReferenceEqualityComparer : EqualityComparer<IVertex<VertexData>>
        {
            public override bool Equals(IVertex<VertexData> x, IVertex<VertexData> y) =>
                x.Data.GameObject.Equals(y.Data.GameObject)
                && x.Data.PrimitiveType.Equals(y.Data.PrimitiveType);

            public override int GetHashCode(IVertex<VertexData> obj) =>
                HashCode.Combine(obj.Data.GameObject, obj.Data.PrimitiveType);
        }

        private readonly struct VertexData : IEquatable<VertexData>
        {
            public readonly GameObject GameObject { get; }
            public readonly PrimitiveType PrimitiveType { get; }
            public VertexData(GameObject gameObject, PrimitiveType primitiveType)
            {
                GameObject = gameObject;
                PrimitiveType = primitiveType;
            }

            public bool Equals(VertexData other) => GameObject.Equals(other.GameObject);
        }

        private readonly struct EdgeData : IEquatable<EdgeData>
        {
            private readonly float _weight;
            public readonly float Weight { get => HasFromAndTo() ? Vector3.Distance(_from.position, _to.position) : _weight; }

            private readonly Transform _from;
            private readonly Transform _to;

            public EdgeData(Transform from, Transform to)
            {
                _weight = Vector3.Distance(from.position, to.position);
                _from = from;
                _to = to;
            }

            public EdgeData(float weight)
            {
                _weight = weight;
                _from = null;
                _to = null;
            }

            public bool Equals(EdgeData other) => Weight.Equals(other.Weight);

            private bool HasFromAndTo() => _from != null && _to != null;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(_bounds.center, _bounds.size);
        }
    }
}