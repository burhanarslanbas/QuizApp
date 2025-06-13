namespace QuizApp.Application.Exceptions.Handlers;

public abstract class ExceptionHandler
{
    public Task HandleExceptionAsync(Exception exception) =>
        exception switch
        {
            BusinessException businessException => HandleException(businessException),
            FluentValidation.ValidationException validationException => HandleException(validationException),
            NotFoundException notFoundException => HandleException(notFoundException),
            UnauthorizedAccessException unauthorizedException => HandleException(unauthorizedException),
            ForbiddenAccessException forbiddenException => HandleException(forbiddenException),
            _ => HandleException(exception)
        };

    protected abstract Task HandleException(BusinessException businessException);
    protected abstract Task HandleException(FluentValidation.ValidationException validationException);
    protected abstract Task HandleException(NotFoundException notFoundException);
    protected abstract Task HandleException(UnauthorizedAccessException unauthorizedException);
    protected abstract Task HandleException(ForbiddenAccessException forbiddenException);
    protected abstract Task HandleException(Exception exception);
}