using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Dto.Helper;
public class CustomTreeList
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ParentId { get; set; }
}
