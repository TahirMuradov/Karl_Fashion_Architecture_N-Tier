﻿using Microsoft.Extensions.Localization;
using System.Reflection;

namespace WebUI.Services
{
    public class SharedResource
    {

    }
    public class LaunguageService
    {
        private readonly IStringLocalizer _localizer;

        public LaunguageService(IStringLocalizerFactory factory)
        {
            var type=typeof(SharedResource);
            var assemblyName= new AssemblyName (type.GetTypeInfo().Assembly.FullName); 
            _localizer = factory.Create(nameof(SharedResource),assemblyName.Name);
        }
        public LocalizedString GetKey(string key)
        {
            return _localizer[key];   
        }
    }
}
