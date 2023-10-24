using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Dto.Helper;
public class Node
{
   public int Value { get; set; }
    public string Label { get; set; }
    public List<Node> Items { get; set; }
    public Node()
    {
        Items = new List<Node>();
    }
}