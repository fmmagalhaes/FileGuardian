namespace FileGuardian.Application.Exceptions;

public class NameAlreadyInUseException(string message): ApplicationException(message)
{
}
