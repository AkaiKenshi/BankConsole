namespace BankConsole.Contracts.DTOs.Customers;

public record UpdateCustomerEmailRequestDTO
(
    string Email,
    string Password
);