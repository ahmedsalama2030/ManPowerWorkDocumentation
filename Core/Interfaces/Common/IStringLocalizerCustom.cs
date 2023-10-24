using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Core.Interfaces.Common;
public interface IStringLocalizerCustom : IStringLocalizer
{
    LocalizedString getValue(string Key, string Culture, params object[] arguments);
}
