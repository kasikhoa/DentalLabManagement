namespace DentalLabManagement.BusinessTier.Error;

using System.Text.Json;

public class ErrorResponse
{
    public int StatusCode { get; set; }

    public string Error { get; set; }

    public DateTime TimeStamp { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    public ErrorResponse()
    {
    }

    public ErrorResponse(int statusCode, string error, DateTime timeStamp)
    {
        StatusCode = statusCode;
        Error = error;
        TimeStamp = timeStamp;
    }

}