using Assignment2.Models;

namespace Assignment2.Services
{
    public class PathFindingService
    {
        public (List<string> path, int distance)? FindShortestPath(Graph graph, string start, string end)
        {
            var distances = new Dictionary<string, int>();
            var previous = new Dictionary<string, string?>();
            var unvisited = new HashSet<string>();

            foreach (var node in graph.Nodes.Keys)
            {
                distances[node] = int.MaxValue;
                previous[node] = null;
                unvisited.Add(node);
            }

            distances[start] = 0;

            while (unvisited.Count > 0)
            {
                var current = unvisited
                    .OrderBy(n => distances[n])
                    .First();

                unvisited.Remove(current);

                if (current == end)
                    break;

                foreach (var neighbour in graph.Nodes[current])
                {
                    var alt = distances[current] + neighbour.Value;
                    if (alt < distances[neighbour.Key])
                    {
                        distances[neighbour.Key] = alt;
                        previous[neighbour.Key] = current;
                    }
                }
            }

            if (distances[end] == int.MaxValue)
                return null;

            var path = new List<string>();
            var step = end;

            while (step != null)
            {
                path.Insert(0, step);
                step = previous[step];
            }

            return (path, distances[end]);
        }
    }
}
