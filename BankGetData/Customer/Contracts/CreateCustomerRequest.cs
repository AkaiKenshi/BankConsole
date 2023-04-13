namespace BankGetData.Customer;

public record CreateCustomerRequest(
    string CustomerID,
    string CustomerUserName,
    string CustomerName, 
    string CustomerLastName,
    string CustomerPassword
    );

