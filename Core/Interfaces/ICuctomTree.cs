using System.Collections.Generic;
using Core.Dto.Helper;

namespace Core.Interfaces;
public interface ICuctomTree 
{
    List<Node> CreateTree(List<CustomTreeList> entities);
}
