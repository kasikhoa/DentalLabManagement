namespace DentalLabManagement.BusinessTier.Error;

using System.Text.Json;

public class ErrorResponse
{
	public int StatusCode { get; set; }

	public string Message { get; set; }


	public override string ToString()
	{
		return JsonSerializer.Serialize(this);
	}

    public ErrorResponse(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }
}