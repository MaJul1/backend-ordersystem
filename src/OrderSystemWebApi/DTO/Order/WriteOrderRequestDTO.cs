using System;
using System.ComponentModel.DataAnnotations;

namespace OrderSystemWebApi.DTO.Order;

public class OrderRequestDTO
{
    [Required]
    public Guid[] ProductIds {get; set;} = [];
}
