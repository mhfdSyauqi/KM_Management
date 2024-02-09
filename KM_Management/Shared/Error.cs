using System.Net;

namespace KM_Management.Shared;

public sealed record Error(HttpStatusCode StatusCode, string? Message = null)
{
    public static readonly Error None = new(HttpStatusCode.OK);
}