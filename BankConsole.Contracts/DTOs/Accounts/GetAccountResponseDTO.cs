namespace BankConsole.Contracts.DTOs.Accounts;

public record GetAccountResponseDTO
(
    string Id,
    double Balance,
    AccountType AccountType,
    string OwnerId,
    DateOnly CraetedDate,
    int? Term
);