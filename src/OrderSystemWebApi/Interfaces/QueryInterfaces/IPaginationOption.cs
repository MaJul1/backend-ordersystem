using System;
using System.ComponentModel.DataAnnotations;

namespace OrderSystemWebApi.Interfaces.QueryInterfaces;

public interface IPaginationOption
{
    int? PageNumber {get; set;}
    int? PageSize {get; set;}
}
