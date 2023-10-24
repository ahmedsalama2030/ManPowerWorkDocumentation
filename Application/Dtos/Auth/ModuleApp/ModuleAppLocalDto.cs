using System;
using Application.Dtos.Auth.ScreenApp;

namespace Application.Dtos.Auth.ModuleApp;
public class ModuleAppLocalDto
{
  public string Key { get; set; }
  public List <ScreenAppLocalDto> screens { get; set; } 
}

