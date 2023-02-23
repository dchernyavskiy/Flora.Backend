namespace Flora.Identity.Exceptions;

public class UnsupportedContextException : Exception
{
    public UnsupportedContextException() : base("HttpContext can't be null.")
    {
    }
}