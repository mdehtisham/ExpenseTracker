using System;
using System.Collections.Generic;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker
{
    class Program
    {
        private static ExpenseManager expenseManager = new ExpenseManager();
        static void Main(string[] args)
        {
            Console.WriteLine("=== Personal Expense Tracker ===");

            while (true)
            {
                ShowMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddExpense();
                        break;
                    case "2":
                        ViewExpenses();
                        break;
                    case "3":
                        DeleteExpense();
                        break;
                    case "4":
                        ShowSummary();
                        break;
                    case "5":
                        SearchExpenses();
                        break;
                    case "6":
                        Console.WriteLine("Exiting the application. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            
                Console.WriteLine("\n Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("\n--- Main Menu ---:");
            Console.WriteLine("1. Add Expense");
            Console.WriteLine("2. View Expenses");
            Console.WriteLine("3. Delete Expense");
            Console.WriteLine("4. View Summary");
            Console.WriteLine("5. Search Expenses");
            Console.WriteLine("6. Exit");
            Console.Write("Choose an option: ");
        }

        static void AddExpense()
        {
            try
            {
                Console.WriteLine("\n--- Add New Expense ---");
                
                // Get amount
                Console.Write("Enter amount: $");
                if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
                {
                    Console.WriteLine("Invalid amount. Please enter a positive number.");
                    return;
                }
                
                // Get description
                Console.Write("Enter description: ");
                string description = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrWhiteSpace(description))
                {
                    Console.WriteLine("Description cannot be empty.");
                    return;
                }
                
                // Get category
                Console.WriteLine("Select category:");
                var categories = Enum.GetValues(typeof(ExpenseCategory));
                for (int i = 0; i < categories.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {categories.GetValue(i)}");
                }
                
                Console.Write("Enter category number: ");
                if (!int.TryParse(Console.ReadLine(), out int categoryChoice) || 
                    categoryChoice < 1 || categoryChoice > categories.Length)
                {
                    Console.WriteLine("Invalid category selection.");
                    return;
                }
                
                var selectedCategory = (ExpenseCategory)categories.GetValue(categoryChoice - 1)!;
                
                // Create and add expense
                var expense = new Expense(amount, description, selectedCategory);
                expenseManager.AddExpense(expense);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding expense: {ex.Message}");
            }
        }
        static void ViewExpenses()
        {
            Console.WriteLine("\n--- All Expenses ---");
            
            var expenses = expenseManager.GetAllExpenses();
            
            if (!expenses.Any())
            {
                Console.WriteLine("No expenses recorded yet.");
                return;
            }
            
            Console.WriteLine($"{"ID",-5} {"Date",-12} {"Category",-15} {"Amount",-10} {"Description",-30}");
            Console.WriteLine(new string('-', 80));
            
            foreach (var expense in expenses.OrderByDescending(e => e.Date))
            {
                Console.WriteLine($"{expense.Id,-5} {expense.Date:yyyy-MM-dd,-12} {expense.Category,-15} ${expense.Amount,-9:F2} {expense.Description,-30}");
            }
            
            Console.WriteLine(new string('-', 80));
            Console.WriteLine($"Total: ${expenseManager.GetTotalExpenses():F2}");
        }
       static void DeleteExpense()
        {
            Console.WriteLine("\n--- Delete Expense ---");
            
            var expenses = expenseManager.GetAllExpenses();
            if (!expenses.Any())
            {
                Console.WriteLine("No expenses to delete.");
                return;
            }
            
            // Show current expenses
            ViewExpenses();
            
            Console.Write("\nEnter ID of expense to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID format.");
                return;
            }
            
            if (expenseManager.DeleteExpense(id))
            {
                Console.WriteLine("Expense deleted successfully!");
            }
            else
            {
                Console.WriteLine("Expense not found.");
            }
        }
        static void ShowSummary() => Console.WriteLine("Show Summary - To be implemented");
        static void SearchExpenses() => Console.WriteLine("Search Expenses - To be implemented");
    }
}