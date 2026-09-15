namespace ForgeOps.Api.Shared;

public class BaseForgeOpsError
{
    public string ErrorMessage { get; set; } = string.Empty;
    public DateTime ErrorTimestamp { get; set; } = DateTime.UtcNow;
}
