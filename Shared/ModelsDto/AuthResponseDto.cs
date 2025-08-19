using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ModelsDto
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
