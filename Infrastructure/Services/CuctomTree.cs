using System;
using Core.Dto.Helper;
using Core.Interfaces;

namespace Infrastructure.Services.Common;
public class CuctomTree : ICuctomTree  
{
    public List<Node> CreateTree(List<CustomTreeList> entities)
    {
        List<Node> nodes = new List<Node>();
        foreach (var item in entities)
        {
            if (item.ParentId == 0||item.ParentId == null)
                nodes.Add(new Node { Value = item.Id, Label = item.Name });
            else
                CreateNode(nodes, item);
        }
        return nodes;
    }
    private void CreateNode(List<Node> nodes, CustomTreeList parent)
    {
        foreach (var node in nodes)
        {
            if (node.Value == parent.ParentId)

                node.Items.Add(new Node { Value = parent.Id, Label = parent.Name });
            else
                CreateNode(node.Items, parent);

        }
    }
}

