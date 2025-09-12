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

        static void AddExpense() => Console.WriteLine("Add Expense - To be implemented");
        static void ViewExpenses() => Console.WriteLine("ViewExpense - To be implemented");
        static void DeleteExpense() => Console.WriteLine("Delete Expense - To be implemented");
        static void ShowSummary() => Console.WriteLine("Show Summary - To be implemented");
        static void SearchExpenses() => Console.WriteLine("Search Expenses - To be implemented");
    }
}