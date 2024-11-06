using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Infrastructure.Messaging
{
    public class RabbitMqConfig
    {
        public required string Host { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Exchange { get; set; }
    }
}