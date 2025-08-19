using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ModelsDto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
