namespace BankGetData.Customer;

public record UpsertCustomerRequest(
    string CustomerUserName,
    string CustomerName,
    string CustomerLastName,
    string CustomerPassword
    );

