﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NJsonSchema.Generation;

namespace SigSpec.Core
{
    public class SigSpecGeneratorSettings : JsonSchemaGeneratorSettings
    {
        public List<Type> Hubs { get; set; } = new List<Type>();
        public SigSpecGeneratorSettings()
        {
            SerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }
    }
}
