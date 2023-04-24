namespace BankConsole.Contracts.DTOs.Accounts;

public record UpdateWithdrawBalanceRequestDTO
(
    string Id,
    double WithdawAmount
);