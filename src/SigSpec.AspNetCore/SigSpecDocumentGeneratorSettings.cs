﻿using System;
using System.Collections.Generic;
using SigSpec.Core;

namespace SigSpec.AspNetCore.Middlewares
{
    public class SigSpecDocumentGeneratorSettings : SigSpecGeneratorSettings
    {
        public string DocumentName { get; set; } = "v1";

        public Dictionary<string, Type> Hubs { get; set; } = new Dictionary<string, Type>();

        public SigSpecDocument Template { get; set; } = new SigSpecDocument();
    }
}
