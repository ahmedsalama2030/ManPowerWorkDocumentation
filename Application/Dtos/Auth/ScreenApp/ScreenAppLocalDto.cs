using System;
using Application.Dtos.Localization.LanguageKey;

namespace Application.Dtos.Auth.ScreenApp;
public class ScreenAppLocalDto
{
    public string Key { get; set; }
  public List <LanguageKeyLocalDto> screens { get; set; } 
}

