using System;
using System.Collections.Generic;

// --------------------------------------------------
// 1. TRANSACTION RECORD
// --------------------------------------------------

public record Transaction(
    int Id,
    DateTime Date,
    decimal Amount,
    string Category
);


// --------------------------------------------------
// 2. TRANSACTION PROCESSOR INTERFACE
// --------------------------------------------------

public interface ITransactionProcessor
{
    void Process(Transaction transaction);
}


// --------------------------------------------------
// 3. MOBILE MONEY PROCESSOR
// --------------------------------------------------

public class MobileMoneyProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine(
            $"Mobile Money processed: GHC {transaction.Amount} for {transaction.Category}"
        );
    }
}


// --------------------------------------------------
// 4. BANK TRANSFER PROCESSOR
// --------------------------------------------------

public class BankTransferProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine(
            $"Bank Transfer processed: GHC {transaction.Amount} for {transaction.Category}"
        );
    }
}


// --------------------------------------------------
// 5. CRYPTO WALLET PROCESSOR
// --------------------------------------------------

public class CryptoWalletProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine(
            $"Crypto Wallet processed: GHC {transaction.Amount} for {transaction.Category}"
        );
    }
}


// --------------------------------------------------
// 6. GENERAL ACCOUNT CLASS
// --------------------------------------------------

public class Account
{
    public string AccountNumber { get; set; }

    public decimal Balance { get; protected set; }

    public Account(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public virtual void ApplyTransaction(Transaction transaction)
    {
        Balance -= transaction.Amount;

        Console.WriteLine(
            $"Transaction applied. New balance: GHC {Balance}"
        );
    }
}


// --------------------------------------------------
// 7. SAVINGS ACCOUNT CLASS
// --------------------------------------------------

public sealed class SavingsAccount : Account
{
    public SavingsAccount(string accountNumber, decimal initialBalance)
        : base(accountNumber, initialBalance)
    {
    }

    public override void ApplyTransaction(Transaction transaction)
    {
        if (transaction.Amount > Balance)
        {
            Console.WriteLine("Insufficient funds");
        }
        else
        {
            Balance -= transaction.Amount;

            Console.WriteLine(
                $"Transaction applied successfully. Updated balance: GHC {Balance}"
            );
        }
    }
}


// --------------------------------------------------
// 8. FINANCE APP CLASS
// --------------------------------------------------

public class FinanceApp
{
    private List<Transaction> _transactions = new List<Transaction>();

    public void Run()
    {
        SavingsAccount savingsAccount =
            new SavingsAccount("GH-ACC-001", 1000m);

        Transaction transaction1 = new Transaction(
            1,
            DateTime.Now,
            200m,
            "Groceries"
        );

        Transaction transaction2 = new Transaction(
            2,
            DateTime.Now,
            150m,
            "Utilities"
        );

        Transaction transaction3 = new Transaction(
            3,
            DateTime.Now,
            100m,
            "Entertainment"
        );

        ITransactionProcessor mobileMoneyProcessor =
            new MobileMoneyProcessor();

        ITransactionProcessor bankTransferProcessor =
            new BankTransferProcessor();

        ITransactionProcessor cryptoWalletProcessor =
            new CryptoWalletProcessor();

        mobileMoneyProcessor.Process(transaction1);

        bankTransferProcessor.Process(transaction2);

        cryptoWalletProcessor.Process(transaction3);

        savingsAccount.ApplyTransaction(transaction1);

        savingsAccount.ApplyTransaction(transaction2);

        savingsAccount.ApplyTransaction(transaction3);

        _transactions.Add(transaction1);
        _transactions.Add(transaction2);
        _transactions.Add(transaction3);

        Console.WriteLine();
        Console.WriteLine("All transactions have been recorded.");
        Console.WriteLine($"Total transactions: {_transactions.Count}");
        Console.WriteLine($"Final account balance: GHC {savingsAccount.Balance}");
    }
}


// --------------------------------------------------
// 9. MAIN APPLICATION
// --------------------------------------------------

public class Program
{
    public static void Main(string[] args)
    {
        FinanceApp app = new FinanceApp();

        app.Run();
    }
}

