using System;

namespace OrderSystemWebApi.DTO;

public class ReadProductRequestDTO
{
    public Guid Id {get; set;}
    public string Name {get; set;} = null!;
    public double Price {get; set;}
}
