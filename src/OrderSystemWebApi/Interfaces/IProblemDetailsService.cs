using System;
using Microsoft.AspNetCore.Mvc;

namespace OrderSystemWebApi.Interfaces;

public interface IProblemService
{
    ProblemDetails CreateBadRequestProblemDetails(string detail, string path);
    ProblemDetails CreateInternalServerErrorProblemDetails(string[] details, string path);
    ProblemDetails CreateNotFoundProblemDetails(string detail, string path);
    ProblemDetails CreateUnauthorizeProblemDetails(string detail, string path);
}
