using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIO.API.V1.User.Responses
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Deleted { get; set; }
        public bool Verified { get; set; }
        public long CharacterTokens { get; set; }
    }
}
