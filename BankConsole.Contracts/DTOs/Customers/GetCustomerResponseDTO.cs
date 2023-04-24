namespace BankConsole.Contracts.DTOs.Customers;

public record GetCustomerResponseDTO
(
    string Token,
    string Id,
    string Username,
    string FirstName,
    string LastName,
    string Email
);
