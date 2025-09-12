using System;


namespace ExpenseTracker.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public ExpenseCategory Category { get; set; }
        public DateTime Date { get; set; }

        public Expense()
        {
            Date = DateTime.Now;
        }

        public Expense(decimal amount, string description, ExpenseCategory category)
        {
            Amount = amount;
            Description = description;
            Category = category;
            Date = DateTime.Now;
        }

    }
}