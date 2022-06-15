namespace SwarmPortal.SQLite
{
    public class Role
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public ICollection<Link> Links { get; set; }
    }
}