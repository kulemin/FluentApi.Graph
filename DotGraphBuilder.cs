using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FluentApi.Graph
{
    public class DotGraphBuilder
    {
        internal Graph graph;
        internal string nodeOrEdge;
        internal GraphNode currentNode;
        internal GraphEdge currentEdge;

        public static DotGraphBuilder DirectedGraph(string graphName)
        {
            return new DotGraphBuilder() { graph = new Graph(graphName, true, true) };
        }

        public static DotGraphBuilder NondirectedGraph(string graphName)
        {
            return new DotGraphBuilder() { graph = new Graph(graphName, false, false) };
        }
    }

    public static class DotGraphBuilderExtention
    {
        public static string Build(this DotGraphBuilder graphBuilder)
        {
            return graphBuilder.graph.ToDotFormat();
        }

        public static DotGraphBuilder AddNode(this DotGraphBuilder graphBuilder, string nodeName)
        {
            graphBuilder.graph.AddNode(nodeName);
            foreach (var node in graphBuilder.graph.Nodes)
                if (node.Name == nodeName)
                    graphBuilder.currentNode = node;
            graphBuilder.nodeOrEdge = "node";
            return graphBuilder;
        }

        public static DotGraphBuilder AddEdge(this DotGraphBuilder graphBuilder, string nodeBegin, string nodeEnd)
        {
            graphBuilder.graph.AddEdge(nodeBegin, nodeEnd);
            graphBuilder.nodeOrEdge = "edge";
            foreach (var edge in graphBuilder.graph.Edges)
                if (edge.SourceNode == nodeBegin && edge.DestinationNode == nodeEnd)
                    graphBuilder.currentEdge = edge;
            return graphBuilder;
        }

        public static DotGraphBuilder With(this DotGraphBuilder graphBuilder, Action<object> action)
        {
            if (graphBuilder.nodeOrEdge == "node")
                action(graphBuilder.currentNode);
            else if (graphBuilder.nodeOrEdge == "edge")
                action(graphBuilder.currentEdge);
            return graphBuilder;
        }
    }

    public static class NodeEdgeExtention
    {
        public static object Color(this object item, string color)
        {
            if (item as GraphNode != null)
                (item as GraphNode).Attributes.Add("color", color);
            else if (item as GraphEdge != null)
                (item as GraphEdge).Attributes.Add("color", color);
            return item;
        }

        public static object FontSize(this object item, int fontSize)
        {
            if (item as GraphNode != null)
                (item as GraphNode).Attributes.Add("fontsize", fontSize.ToString());
            else if (item as GraphEdge != null)
                (item as GraphEdge).Attributes.Add("fontsize", fontSize.ToString());
            return item;
        }

        public static object Label(this object item, string label)
        {
            if (item as GraphNode != null)
                (item as GraphNode).Attributes.Add("label", label);
            else if (item as GraphEdge != null)
                (item as GraphEdge).Attributes.Add("label", label);
            return item;
        }

        public static object Weight(this object item, int weight)
        {
            if (item as GraphNode != null)
                (item as GraphNode).Attributes.Add("weight", weight.ToString());
            else if (item as GraphEdge != null)
                (item as GraphEdge).Attributes.Add("weight", weight.ToString());
            return item;
        }

        public static object Shape(this object item, string shape)
        {
            if (item as GraphNode != null)
                (item as GraphNode).Attributes.Add("shape", shape);
            return item;
        }
    }

    public class NodeShape
    {
        public static string Box = "box";
        public static string Ellipse = "ellipse";
    }
}