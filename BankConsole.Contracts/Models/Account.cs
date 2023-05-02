using System.ComponentModel.DataAnnotations;

namespace BankConsole.Contracts.Models;

public class Account
{
    public string Id { get; set; } = null!;
    public double Balance { get; set; }
    public AccountType AccountType { get; set; }
    public Customer Customer { get; set; } = null!;
    public DateOnly CraetedDate { get; set; }
    public int? Term { get; set; } 
    
}
