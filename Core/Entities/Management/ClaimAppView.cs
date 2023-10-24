using System;
using Core.Common;

namespace Core.Entities.Management;
    public class ClaimAppView:IBaseId
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public int ScreenAppId { get; set; }
        public string ClaimValue { get; set; }
        public string ClaimType { get; set; }
        public bool IsDeleted { get; set; }
         public string ScreenAppNameEn { get; set; }
        public string ScreenAppNameAr { get; set; }
        public string ModuleAppNameEn { get; set; }
        public string ModuleAppNameAr { get; set; }
        public int ModuleAppId { get; set; }
  }

