namespace BuildingBlocks.Exceptions;

public class BadRequestException(string message) : Exception(message)
{
    public string? Details { get; }
    public BadRequestException(string message, string details) : this(message)
    {
        Details = details;
    }
}
