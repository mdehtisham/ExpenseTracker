using System;
using System.Collections.Generic;
using System.Linq;
using ExpenseTracker.Models;
using System.Text.Json;
using System.IO;

namespace ExpenseTracker.Services
{
    public class ExpenseManager
    {
        private readonly string filePath = "expenses.json";
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

        public void SaveToFile()
        {
            try
            {
                var json = JsonSerializer.Serialize(expenses, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(filePath, json);
                Console.WriteLine("Data saved successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        public void LoadFromFile()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    expenses = JsonSerializer.Deserialize<List<Expense>>(json) ?? new List<Expense>();

                    // Update nextId to avoid conflicts
                    nextId = expenses.Any() ? expenses.Max(e => e.Id) + 1 : 1;

                    Console.WriteLine($"Loaded {expenses.Count} expenses from file.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                expenses = new List<Expense>();
                nextId = 1;
            }
        }
    }
}