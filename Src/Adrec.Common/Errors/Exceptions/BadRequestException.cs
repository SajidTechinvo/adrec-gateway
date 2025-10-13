namespace Adrec.Common.Errors.Exceptions
{
    public class BadRequestException(string message) : Exception($"{message}")
    { }
}