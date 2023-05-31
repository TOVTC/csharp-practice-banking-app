namespace Classes;

public class LineOfCreditAccount : BankAccount
{
    // this constructor calls a different base class constructor than the other account types
    public LineOfCreditAccount(string name, decimal initialBalance, decimal creditLimit) : base(name, initialBalance, -creditLimit)
    {

    }

    public override void PerformMonthEndTransactions()
    {
        if (Balance < 0)
        {
            decimal interest = -Balance * 0.07m;
            MakeWithdrawal(interest, DateTime.Now, "charge monthly interest");
        }
    }
}
