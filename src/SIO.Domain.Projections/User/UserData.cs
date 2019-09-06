namespace SIO.Domain.Projections.User
{
    public class UserData
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Deleted { get; set; }
        public bool Verified { get; set; }
        public long CharacterTokens { get; set; }
    }
}
