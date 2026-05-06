using System;

namespace FASTSocietiesSystem.BLL
{
    /// <summary>
    /// Custom exceptions for business logic layer
    /// </summary>

    /// <summary>
    /// Thrown when authentication fails (invalid credentials)
    /// </summary>
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException(string message = "Invalid email or password") : base(message) { }
    }

    /// <summary>
    /// Thrown when trying to create duplicate resources
    /// </summary>
    public class DuplicateResourceException : Exception
    {
        public DuplicateResourceException(string message = "This resource already exists") : base(message) { }
    }

    /// <summary>
    /// Thrown when a requested resource is not found
    /// </summary>
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string message = "The requested resource was not found") : base(message) { }
    }

    /// <summary>
    /// Thrown when operation is not allowed
    /// </summary>
    public class UnauthorizedOperationException : Exception
    {
        public UnauthorizedOperationException(string message = "You are not authorized to perform this operation") : base(message) { }
    }

    /// <summary>
    /// Thrown when business rule validation fails
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }

    /// <summary>
    /// Thrown when event is full or capacity exceeded
    /// </summary>
    public class EventCapacityExceededException : Exception
    {
        public EventCapacityExceededException(string message = "This event has reached its maximum capacity") : base(message) { }
    }

    /// <summary>
    /// Thrown when registration deadline has passed
    /// </summary>
    public class RegistrationDeadlinePassedException : Exception
    {
        public RegistrationDeadlinePassedException(string message = "Registration deadline has passed") : base(message) { }
    }
}
