namespace Classes;

public class BankAccount
{
    // It's also static, which means it's shared by all of the BankAccount objects.
    // The value of a non-static variable is unique to each instance of the BankAccount object.
    private static int _accountNumberSeed = 1234567890;

    // readonly indicates that the value cannot be chagned after the object is constructed
    // Once a BankAccount is created, the minimumBalance can't change. 
    private readonly decimal _minimumBalance;

    public string Number { get; }
    public string Owner { get; set; }
    public decimal Balance
    {
        get
        {
            decimal balance = 0;
            foreach (var item in _allTransactions)
            {
                balance += item.Amount;
            }
            return balance;
        }
    }
    private List<Transaction> _allTransactions = new List<Transaction>();

    // the : this() expression calls the other constrctor that takes three parameters
    // used to initialize accounts that are not of the line of credit type
    public BankAccount(string name, decimal intialBalance) : this(name, intialBalance, 0)
    {

    }

    // The compiler doesn't generate a default constructor when you define a constructor yourself.
    // That means each derived class must explicitly call this constructor. 
    public BankAccount(string name, decimal initialBalance, decimal minimumBalance)
    {
        Number = _accountNumberSeed.ToString();
        _accountNumberSeed++;

        Owner = name;
        _minimumBalance = minimumBalance;
        // deposits have to be greater than 0, but credit accounts can open with a balance of 0 since you are borrowing money
        if (initialBalance > 0)
        {
            MakeDeposit(initialBalance, DateTime.Now, "initial balance");
        }
    }

    // The throw statement throws an exception.
    // Execution of the current block ends, and control transfers to the first matching catch block found in the call stack. 

    public void MakeDeposit(decimal amount, DateTime date, string note)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount of deposit must be greater than 0");
        }
        var deposit = new Transaction(amount, date, note);
        _allTransactions.Add(deposit);
    }

    public void MakeWithdrawal(decimal amount, DateTime date, string note)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount of withdrawal must be greater than 0");
        }
        Transaction? overdraftTransaction = CheckWithdrawalLimit(Balance - amount < _minimumBalance);
        Transaction? withdrawal = new(-amount, date, note);
        _allTransactions.Add(withdrawal);
        if (overdraftTransaction != null)
        {
            _allTransactions.Add(overdraftTransaction);
        }
    }

    // The added method is protected, which means that it can be called only from derived classes.
    // That declaration prevents other clients from calling the method. It's also virtual so that derived classes can change the behavior.
    protected virtual Transaction? CheckWithdrawalLimit(bool isOverdrawn)
    {
        if (isOverdrawn)
        {
            throw new InvalidOperationException("Not sufficient funds for this withdrawal");
        }
        else
        {
            return default;
        }
    }

    // The history uses the StringBuilder class to format a string that contains one line for each transaction. 
    public string GetAccountHistory()
    {
        var report = new System.Text.StringBuilder();

        decimal balance = 0;
        report.AppendLine("Date\t\tAmount\tBalance\tNote");
        foreach (var item in _allTransactions)
        {
            balance += item.Amount;
            report.AppendLine($"{item.Date.ToShortDateString()}\t{item.Amount}\t{balance}\t{item.Notes}");
        }
        return report.ToString();
    }

    // A virtual method is a method where any derived class may choose to reimplement.
    // The derived classes use the override keyword to define the new implementation.
    // "overriding the base class implementation"
    // You can also declare abstract methods where derived classes must override the behavior.
    // The base class does not provide an implementation for an abstract method.
    public virtual void PerformMonthEndTransactions()
    {

    }
}