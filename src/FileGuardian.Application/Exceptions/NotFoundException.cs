namespace FileGuardian.Application.Exceptions;

public class NotFoundException(string message): ApplicationException(message)
{
}
