namespace Assignment2.Models
{
    public class Graph
    {
        // Adjacency list:
        // Node -> (Neighbour -> Distance)
        public Dictionary<string, Dictionary<string, int>> Nodes { get; set; }
            = new Dictionary<string, Dictionary<string, int>>();
    }
}
