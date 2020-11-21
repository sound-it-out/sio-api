using System;
using System.Collections.Generic;
using System.Text;

namespace SIO.Domain.Projections.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public UserData Data { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? LastModifiedDate { get; set; }
        public int Version { get; set; }
    }
}
