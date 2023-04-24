namespace BankConsole.Contracts.DTOs.Accounts;

public record UpdateDepositBalanceRequestDTO
(
    string Id,
    double DepositAmount
);