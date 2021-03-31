using System;
using System.Collections.Generic;
using System.Text;

namespace EventPublisher.Configuration
{
    public class EventStorage
    {
        public string ConnectionName { get; set; }
        public string ConnectionString { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
