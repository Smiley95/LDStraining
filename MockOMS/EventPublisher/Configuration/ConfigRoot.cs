﻿using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Messaging.Configuration;

namespace EventPublisher.Configuration
{
    public class ConfigRoot
    {
        public EventStorage EventStorage { get; set; }
        public DuplexServerEndpointSettings[] DuplexServerEndpoints { get; set; }
    }
}
