namespace spark.Models;

/// <summary>
/// View model returned to error pages and used by the UI to display a request id.
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// Optional request identifier for troubleshooting.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Indicates whether the RequestId should be shown.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}

