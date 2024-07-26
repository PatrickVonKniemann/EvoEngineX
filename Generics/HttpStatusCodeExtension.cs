using System.Net;

namespace Generics;

public static class HttpStatusCodeExtension
{
    private const string BaseErrorUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-";

    public static string ToProblemDetailsType(this HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.Continue => BaseErrorUrl + "6.2.1",
                HttpStatusCode.SwitchingProtocols => BaseErrorUrl + "6.2.2",
                HttpStatusCode.OK => BaseErrorUrl + "6.3.1",
                HttpStatusCode.Created => BaseErrorUrl + "6.3.2",
                HttpStatusCode.Accepted => BaseErrorUrl + "6.3.3",
                HttpStatusCode.NonAuthoritativeInformation => BaseErrorUrl + "6.3.4",
                HttpStatusCode.NoContent => BaseErrorUrl + "6.3.5",
                HttpStatusCode.ResetContent => BaseErrorUrl + "6.3.6",
                HttpStatusCode.PartialContent => BaseErrorUrl + "4.1",
                HttpStatusCode.MultipleChoices => BaseErrorUrl + "6.4.1",
                HttpStatusCode.MovedPermanently => BaseErrorUrl + "6.4.2",
                HttpStatusCode.Found => BaseErrorUrl + "6.4.3",
                HttpStatusCode.SeeOther => BaseErrorUrl + "6.4.4",
                HttpStatusCode.NotModified => BaseErrorUrl + "4.1",
                HttpStatusCode.UseProxy => BaseErrorUrl + "6.4.5",
                HttpStatusCode.TemporaryRedirect => BaseErrorUrl + "6.4.7",
                HttpStatusCode.BadRequest => BaseErrorUrl + "6.5.1",
                HttpStatusCode.Unauthorized => BaseErrorUrl + "3.1",
                HttpStatusCode.PaymentRequired => BaseErrorUrl + "6.5.2",
                HttpStatusCode.Forbidden => BaseErrorUrl + "6.5.3",
                HttpStatusCode.NotFound => BaseErrorUrl + "6.5.4",
                HttpStatusCode.MethodNotAllowed => BaseErrorUrl + "6.5.5",
                HttpStatusCode.NotAcceptable => BaseErrorUrl + "6.5.6",
                HttpStatusCode.ProxyAuthenticationRequired => BaseErrorUrl + "3.2",
                HttpStatusCode.RequestTimeout => BaseErrorUrl + "6.5.7",
                HttpStatusCode.Conflict => BaseErrorUrl + "6.5.8",
                HttpStatusCode.Gone => BaseErrorUrl + "6.5.9",
                HttpStatusCode.LengthRequired => BaseErrorUrl + "6.5.10",
                HttpStatusCode.PreconditionFailed => BaseErrorUrl + "4.2",
                HttpStatusCode.RequestEntityTooLarge => BaseErrorUrl + "6.5.11",
                HttpStatusCode.RequestUriTooLong => BaseErrorUrl + "6.5.12",
                HttpStatusCode.UnsupportedMediaType => BaseErrorUrl + "6.5.13",
                HttpStatusCode.RequestedRangeNotSatisfiable => BaseErrorUrl + "4.4",
                HttpStatusCode.ExpectationFailed => BaseErrorUrl + "6.5.14",
                HttpStatusCode.UpgradeRequired => BaseErrorUrl + "6.5.15",
                HttpStatusCode.InternalServerError => BaseErrorUrl + "6.6.1",
                HttpStatusCode.NotImplemented => BaseErrorUrl + "6.6.2",
                HttpStatusCode.BadGateway => BaseErrorUrl + "6.6.3",
                HttpStatusCode.ServiceUnavailable => BaseErrorUrl + "6.6.4",
                HttpStatusCode.GatewayTimeout => BaseErrorUrl + "6.6.5",
                HttpStatusCode.HttpVersionNotSupported => BaseErrorUrl + "6.6.6",
                _ => BaseErrorUrl
            };
        }
}