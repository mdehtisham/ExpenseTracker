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
            expenseManager.LoadFromFile();

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
                        expenseManager.SaveToFile();
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
                // Auto-save after each operation
                expenseManager.SaveToFile();
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


        static void ShowSummary()
        {
            Console.WriteLine("\n--- Expense Summary ---");

            var expenses = expenseManager.GetAllExpenses();
            if (!expenses.Any())
            {
                Console.WriteLine("No expenses to summarize.");
                return;
            }

            // Summary by category
            Console.WriteLine("\nBy Category:");
            var categoryTotals = expenses
                .GroupBy(e => e.Category)
                .OrderByDescending(g => g.Sum(e => e.Amount))
                .ToList();

            foreach (var group in categoryTotals)
            {
                Console.WriteLine($"{group.Key,-15}: ${group.Sum(e => e.Amount):F2} ({group.Count()} expenses)");
            }

            // Monthly summary
            Console.WriteLine("\nBy Month:");
            var monthlyTotals = expenses
                .GroupBy(e => e.Date.ToString("yyyy-MM"))
                .OrderByDescending(g => g.Key)
                .Take(6) // Last 6 months
                .ToList();

            foreach (var group in monthlyTotals)
            {
                Console.WriteLine($"{group.Key}: ${group.Sum(e => e.Amount):F2} ({group.Count()} expenses)");
            }

            Console.WriteLine($"\nOverall Total: ${expenses.Sum(e => e.Amount):F2}");
            Console.WriteLine($"Average per expense: ${expenses.Average(e => e.Amount):F2}");
        }
        
        
        static void SearchExpenses()
        {
            Console.WriteLine("\n--- Search Expenses ---");
            Console.WriteLine("1. Search by category");
            Console.WriteLine("2. Search by description");
            Console.WriteLine("3. Search by date range");
            Console.Write("Choose search type: ");
            
            var choice = Console.ReadLine();
            var expenses = expenseManager.GetAllExpenses();
            List<Expense> results = new List<Expense>();
            
            switch (choice)
            {
                case "1":
                    results = SearchByCategory(expenses);
                    break;
                case "2":
                    results = SearchByDescription(expenses);
                    break;
                case "3":
                    results = SearchByDateRange(expenses);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    return;
            }
            
            DisplaySearchResults(results);
        }

        static List<Expense> SearchByCategory(List<Expense> expenses)
        {
            Console.WriteLine("Select category to search:");
            var categories = Enum.GetValues(typeof(ExpenseCategory));
            for (int i = 0; i < categories.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {categories.GetValue(i)}");
            }
            
            Console.Write("Enter category number: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= categories.Length)
            {
                var selectedCategory = (ExpenseCategory)categories.GetValue(choice - 1)!;
                return expenses.Where(e => e.Category == selectedCategory).ToList();
            }
            
            return new List<Expense>();
        }

        static List<Expense> SearchByDescription(List<Expense> expenses)
        {
            Console.Write("Enter search term: ");
            var searchTerm = Console.ReadLine()?.Trim().ToLower() ?? "";
            
            return expenses
                .Where(e => e.Description.ToLower().Contains(searchTerm))
                .ToList();
        }

        static List<Expense> SearchByDateRange(List<Expense> expenses)
        {
            Console.Write("Enter start date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
            {
                Console.WriteLine("Invalid start date format.");
                return new List<Expense>();
            }
            
            Console.Write("Enter end date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
            {
                Console.WriteLine("Invalid end date format.");
                return new List<Expense>();
            }
            
            return expenses
                .Where(e => e.Date.Date >= startDate.Date && e.Date.Date <= endDate.Date)
                .ToList();
        }

        static void DisplaySearchResults(List<Expense> results)
        {
            if (!results.Any())
            {
                Console.WriteLine("No expenses found matching your criteria.");
                return;
            }
            
            Console.WriteLine($"\nFound {results.Count} matching expenses:");
            Console.WriteLine($"{"ID",-5} {"Date",-12} {"Category",-15} {"Amount",-10} {"Description",-30}");
            Console.WriteLine(new string('-', 80));
            
            foreach (var expense in results.OrderByDescending(e => e.Date))
            {
                Console.WriteLine($"{expense.Id,-5} {expense.Date:yyyy-MM-dd,-12} {expense.Category,-15} ${expense.Amount,-9:F2} {expense.Description,-30}");
            }
            
            Console.WriteLine($"Total for search results: ${results.Sum(e => e.Amount):F2}");
        }
    }
}