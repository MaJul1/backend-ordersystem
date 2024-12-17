using System;
using System.ComponentModel.DataAnnotations;

namespace OrderSystemWebApi.Interfaces.QueryInterfaces;

public interface IPriceFilterOption
{
        double? MinimumPrice { get; set; }
        double? MaximumPrice { get; set; }
}
