using System.Data.SqlTypes;
using System.Text.Encodings.Web;
using System.Text.Json;
using WestcoastBank.Enums;
using WestcoastBank.Interfaces;
using WestcoastBank.Models.Customers;
using WestcoastBank.Models.Persistance;

namespace WestcoastBank.Models.Accounts;

public class Account : IBaseAccount
{
    private List<Transaction> _transactionList = [];
    private static readonly string _path = Environment.CurrentDirectory + "/Data/Transactions.json";
    public virtual int Balance { get; private set; } = 0;
    public string AccountNumber { get; private set;}
    public List<Transaction> Transactions { get => _transactionList; }
    
    //old fashion constructor
    public Account (string accNo)
    {
        CheckIfPathExists();
        AccountNumber = accNo;
        _transactionList = Storage<Transaction>.ReadData(_path);
        CalculateBalance();
    }
    
    public void Deposit(int amount)
    {
        Balance += amount;
        AddTransaction(amount, TransactionTypeEnum.Insättning);
    }

    public void WithDraw(int amount)
    {
        if (Balance < amount)
        {
            throw new Exception("Du har inte tillräckligt på kontot");
        }
        Balance -= amount;

        AddTransaction(0-amount, TransactionTypeEnum.Uttag);
    }

    public void AddTransaction(int amount, TransactionTypeEnum type)
    {
        Transaction tran = new()
        {
            TransactionAmount = amount,
            TransactionType = type
        };
        _transactionList.Add(tran);
        Storage<Transaction>.WriteData(_transactionList, _path);
    }

    private void CalculateBalance()
    {
        Balance = _transactionList.Sum(c => c.TransactionAmount);
        //foreach (Transaction trx in _transactionList)
        //{
        //    Balance += trx.TransactionAmount;
        //}
    }

    private void CheckIfPathExists()
    {
        // Finns katalogen?
        if (!Directory.Exists(Environment.CurrentDirectory + "/Data"))
        {
            Directory.CreateDirectory(Environment.CurrentDirectory + "/Data");
        }

        //Finns filen?
        if (!File.Exists(_path))
        {
            using StreamWriter sw = new(_path);
            sw.Close();
        }
    }
}
