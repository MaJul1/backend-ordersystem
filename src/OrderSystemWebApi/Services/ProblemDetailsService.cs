using System;
using Microsoft.AspNetCore.Mvc;
using OrderSystemWebApi.Interfaces;

namespace OrderSystemWebApi.Services;

public class ProblemDetailsService : IProblemService
{
    public ProblemDetails CreateBadRequestProblemDetails(string detail, string path)
    {
        return CreateProblemDetails(StatusCodes.Status400BadRequest, "Bad Request", detail, path);
    }

    public ProblemDetails CreateInternalServerErrorProblemDetails(string[] details, string path)
    {
        string detail = string.Concat(details);
        return CreateProblemDetails(StatusCodes.Status500InternalServerError, "Internal Server Error", detail, path);
    }

    public ProblemDetails CreateInternalServerErrorProblemDetails(string details, string path) =>
        CreateInternalServerErrorProblemDetails([details], path);

    public ProblemDetails CreateNotFoundProblemDetails(string detail, string path)
    {
        return CreateProblemDetails(StatusCodes.Status404NotFound, "Not found", detail, path);
    }

    public ProblemDetails CreateUnauthorizeProblemDetails(string detail, string path)
    {
        return CreateProblemDetails(StatusCodes.Status401Unauthorized, "Unauthorized", detail, path);
    }

    private ProblemDetails CreateProblemDetails(int code, string title, string detail, string path)
    {
        ProblemDetails details = new()
        {
            Status = code,
            Title = title,
            Detail = detail,
            Instance = path
        };

        return details;
    }
}
