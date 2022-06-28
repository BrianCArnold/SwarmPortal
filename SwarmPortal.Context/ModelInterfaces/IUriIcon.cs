namespace SwarmPortal.Context;

public interface IUriIcon
{
    ulong Id { get; set; }
    Uri Uri { get; set; }
    Uri Icon { get; set; }
    DateTime RetrievedDate { get; set; }
}
