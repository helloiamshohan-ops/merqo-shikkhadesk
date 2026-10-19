namespace ShikkhaDesk.LanTransport;

/// <summary>
/// Every state-changing LAN request carries an operation id (idempotency key) so a
/// retry after a timeout is deduplicated by the host and never double-posts
/// (LAN-SETUP.md section 5).
/// </summary>
public abstract record LanRequest(string OperationId);

/// <summary>Connection status surfaced in the UI top bar.</summary>
public enum ConnectionStatus { Disconnected, Connecting, Connected, Reconnecting }

/// <summary>Generic host response: success flag + message (Bengali) + optional payload.</summary>
public sealed record LanResponse<T>(bool Ok, string Message, T? Data);

/// <summary>Host connection settings entered by the user on a client.</summary>
public sealed record HostSettings(string HostAddress, int Port)
{
    public string BaseUrl => $"http://{HostAddress}:{Port}";
}
