namespace Adrec.Common.Errors.Exceptions
{
    public class NotFoundException(string message) : Exception($"{message} not found")
    { }
}