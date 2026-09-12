using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

// ==========================================
// MARKER INTERFACE FOR INVENTORY ENTITIES
// ==========================================
public interface IInventoryEntity
{
    int Id { get; }
}

// ==========================================
// IMMUTABLE INVENTORY RECORD
// ==========================================
public record InventoryItem(
    int Id,
    string Name,
    int Quantity,
    DateTime DateAdded
) : IInventoryEntity;

// ==========================================
// GENERIC INVENTORY LOGGER
// ==========================================
public class InventoryLogger<T> where T : IInventoryEntity
{
    private List<T> _log;
    private string _filePath;

    // Constructor
    public InventoryLogger(string filePath)
    {
        _log = new List<T>();
        _filePath = filePath;
    }

    // Add an item to the log
    public void Add(T item)
    {
        _log.Add(item);
    }

    // Return all items
    public List<T> GetAll()
    {
        return _log;
    }

    // Save items to a JSON file
    public void SaveToFile()
    {
        try
        {
            string json = JsonSerializer.Serialize(
                _log,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            using (StreamWriter writer = new StreamWriter(_filePath))
            {
                writer.Write(json);
            }

            Console.WriteLine("Inventory data saved successfully.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine(
                "Error: You do not have permission to write to the file.");
        }
        catch (IOException ex)
        {
            Console.WriteLine(
                "File Error: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                "Unexpected Error: " + ex.Message);
        }
    }

    // Load items from a JSON file
    public void LoadFromFile()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine(
                    "Error: Inventory file does not exist.");
                return;
            }

            using (StreamReader reader = new StreamReader(_filePath))
            {
                string json = reader.ReadToEnd();

                List<T> loadedItems =
                    JsonSerializer.Deserialize<List<T>>(json);

                if (loadedItems != null)
                {
                    _log = loadedItems;
                }
            }

            Console.WriteLine("Inventory data loaded successfully.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine(
                "Error: You do not have permission to read the file.");
        }
        catch (IOException ex)
        {
            Console.WriteLine(
                "File Error: " + ex.Message);
        }
        catch (JsonException)
        {
            Console.WriteLine(
                "Error: The inventory file contains invalid JSON data.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                "Unexpected Error: " + ex.Message);
        }
    }
}

// ==========================================
// INVENTORY APPLICATION
// ==========================================
public class InventoryApp
{
    private InventoryLogger<InventoryItem> _logger;

    // Constructor
    public InventoryApp()
    {
        _logger = new InventoryLogger<InventoryItem>(
            "inventory.json");
    }

    // Add sample data
    public void SeedSampleData()
    {
        _logger.Add(
            new InventoryItem(
                101,
                "Laptop",
                10,
                DateTime.Now));

        _logger.Add(
            new InventoryItem(
                102,
                "Keyboard",
                25,
                DateTime.Now));

        _logger.Add(
            new InventoryItem(
                103,
                "Mouse",
                30,
                DateTime.Now));

        _logger.Add(
            new InventoryItem(
                104,
                "Monitor",
                15,
                DateTime.Now));

        _logger.Add(
            new InventoryItem(
                105,
                "Printer",
                8,
                DateTime.Now));

        Console.WriteLine("Sample inventory data added.");
    }

    // Save inventory data
    public void SaveData()
    {
        _logger.SaveToFile();
    }

    // Load inventory data
    public void LoadData()
    {
        _logger.LoadFromFile();
    }

    // Print all inventory items
    public void PrintAllItems()
    {
        Console.WriteLine();
        Console.WriteLine("========== INVENTORY ITEMS ==========");

        List<InventoryItem> items = _logger.GetAll();

        foreach (InventoryItem item in items)
        {
            Console.WriteLine(
                $"ID: {item.Id} | " +
                $"Name: {item.Name} | " +
                $"Quantity: {item.Quantity} | " +
                $"Date Added: {item.DateAdded}");
        }
    }
}

// ==========================================
// MAIN PROGRAM
// ==========================================
class Program
{
    static void Main(string[] args)
    {
        // First session
        Console.WriteLine("========== FIRST SESSION ==========");

        InventoryApp app = new InventoryApp();

        app.SeedSampleData();

        app.SaveData();

        // Simulate a new session
        Console.WriteLine();
        Console.WriteLine("========== NEW SESSION ==========");

        InventoryApp newApp = new InventoryApp();

        // Load data from the file
        newApp.LoadData();

        // Display recovered data
        newApp.PrintAllItems();

        Console.WriteLine();
        Console.WriteLine("Inventory data recovered successfully.");
    }
}