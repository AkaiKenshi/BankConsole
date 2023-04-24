namespace BankConsole.Contracts.DTOs.Customers;

public record GetCustomerLoginRequestDTO
(
    string Username,
    string Password
);
