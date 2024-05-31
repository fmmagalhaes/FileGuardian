using AutoMapper;
using FileGuardian.Application.Exceptions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;

namespace FileGuardian.Api.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while executing {Context}", context.Request.Path.Value);

            var (status, message) = GetResponse(ex);
            context.Response.StatusCode = (int)status;
            context.Response.ContentType = "application/json";

            var messageObject = JsonConvert.SerializeObject(new SimpleErrorMessage { Message = message });
            await context.Response.WriteAsync(messageObject);
        }
    }

    private static (HttpStatusCode code, string message) GetResponse(Exception exception)
    {
        HttpStatusCode code;
        switch (exception)
        {
            case NotFoundException:
                code = HttpStatusCode.NotFound;
                break;
            case NameAlreadyInUseException:
                code = HttpStatusCode.Conflict;
                break;
            case ArgumentOutOfRangeException:
                code = HttpStatusCode.BadRequest;
                break;
            case AutoMapperMappingException:
                return (HttpStatusCode.BadRequest, exception.InnerException!.Message);
            case SqliteException:
            case DbUpdateException:
                return (HttpStatusCode.InternalServerError, "Something went wrong while updating the database");
            default:
                return (HttpStatusCode.InternalServerError, "Something went wrong");
        }

        return (code, exception.Message);
    }

    private class SimpleErrorMessage
    {
        public required string Message { get; set; }

        public SimpleErrorMessage() { }
    }
}