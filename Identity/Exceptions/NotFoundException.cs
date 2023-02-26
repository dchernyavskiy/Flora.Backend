namespace Flora.Identity.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string key) : base($"\"{key}\" was not found.")
    {
    }
}