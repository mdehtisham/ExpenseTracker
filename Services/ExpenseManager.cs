using System;
using System.Collections.Generic;
using System.Linq;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services
{
    public class ExpenseManager
    {
        // This class can be expanded to include methods for managing expenses,
        // such as adding, removing, and retrieving expenses.
        private List<Expense> expenses;
        private int nextId;
        public ExpenseManager()
        {
            expenses = new List<Expense>();
            nextId = 1;
        }
        public void AddExpense(Expense expense)
        {
            expense.Id = nextId++;
            expenses.Add(expense);
            Console.WriteLine($"Expense added successfully! (ID: {expense.Id})");
        }

        public List<Expense> GetAllExpenses()
        {
            return expenses.ToList();
        }

        public bool DeleteExpense(int id)
        {
            var expense = expenses.FirstOrDefault(e => e.Id == id);
            if (expense != null)
            {
                expenses.Remove(expense);
                return true;
            }
            return false;
        }

        public decimal GetTotalExpenses()
        {
            return expenses.Sum(e => e.Amount);
        }
    }
}