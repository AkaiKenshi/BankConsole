namespace BankConsole.Contracts.DTOs.Accounts;

public record CreateFixedTermInvestmentAccountRequestDTO
(
    double Balance,
    int Term
);
