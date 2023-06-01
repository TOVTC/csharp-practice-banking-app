namespace Classes;

public class LineOfCreditAccount : BankAccount
{
    // this constructor calls a different base class constructor than the other account types
    public LineOfCreditAccount(string name, decimal initialBalance, decimal creditLimit) : base(name, initialBalance, -creditLimit)
    {

    }

    // const App = () => { return true; } // with braces you've to use the return statement
    // const App = () => true; // without braces it forces the return statement automatically
    // https://stackoverflow.com/questions/60596873/arrow-function-syntax-with-parentheses-instead-of-curly-braces

    // The override returns a fee transaction when the account is overdrawn. If the withdrawal doesn't go over the limit, the method returns a null transaction
    protected override Transaction? CheckWithdrawalLimit(bool isOverdrawn) =>
        isOverdrawn ? new Transaction(-20, DateTime.Now, "apply overdraft fee") : default;

    public override void PerformMonthEndTransactions()
    {
        if (Balance < 0)
        {
            decimal interest = -Balance * 0.07m;
            MakeWithdrawal(interest, DateTime.Now, "charge monthly interest");
        }
    }
}
