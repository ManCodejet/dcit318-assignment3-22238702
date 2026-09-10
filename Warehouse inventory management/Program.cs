using System;
using System.Collections.Generic;


// ======================================================
// 1. MARKER INTERFACE
// ======================================================

public interface IInventoryItem
{
    int Id { get; }
    string Name { get; }
    int Quantity { get; set; }
}


// ======================================================
// 2. ELECTRONIC ITEM
// ======================================================

public class ElectronicItem : IInventoryItem
{
    public int Id { get; }
    public string Name { get; }
    public int Quantity { get; set; }
    public string Brand { get; }
    public int WarrantyMonths { get; }

    public ElectronicItem(
        int id,
        string name,
        int quantity,
        string brand,
        int warrantyMonths)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        Brand = brand;
        WarrantyMonths = warrantyMonths;
    }
}


// ======================================================
// 3. GROCERY ITEM
// ======================================================

public class GroceryItem : IInventoryItem
{
    public int Id { get; }
    public string Name { get; }
    public int Quantity { get; set; }
    public DateTime ExpiryDate { get; }

    public GroceryItem(
        int id,
        string name,
        int quantity,
        DateTime expiryDate)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        ExpiryDate = expiryDate;
    }
}


// ======================================================
// 4. CUSTOM EXCEPTIONS
// ======================================================

public class DuplicateItemException : Exception
{
    public DuplicateItemException(string message)
        : base(message)
    {
    }
}


public class ItemNotFoundException : Exception
{
    public ItemNotFoundException(string message)
        : base(message)
    {
    }
}


public class InvalidQuantityException : Exception
{
    public InvalidQuantityException(string message)
        : base(message)
    {
    }
}


// ======================================================
// 5. GENERIC INVENTORY REPOSITORY
// ======================================================

public class InventoryRepository<T>
    where T : IInventoryItem
{
    private Dictionary<int, T> _items =
        new Dictionary<int, T>();


    // Add an item
    public void AddItem(T item)
    {
        if (_items.ContainsKey(item.Id))
        {
            throw new DuplicateItemException(
                $"Item with ID {item.Id} already exists.");
        }

        _items.Add(item.Id, item);
    }


    // Find an item by ID
    public T GetItemById(int id)
    {
        if (_items.TryGetValue(id, out T? item))
        {
            return item;
        }

        throw new ItemNotFoundException(
            $"Item with ID {id} was not found.");
    }


    // Remove an item
    public void RemoveItem(int id)
    {
        if (!_items.ContainsKey(id))
        {
            throw new ItemNotFoundException(
                $"Item with ID {id} was not found.");
        }

        _items.Remove(id);
    }


    // Return all items
    public List<T> GetAllItems()
    {
        return new List<T>(_items.Values);
    }


    // Update quantity
    public void UpdateQuantity(
        int id,
        int newQuantity)
    {
        if (newQuantity < 0)
        {
            throw new InvalidQuantityException(
                "Quantity cannot be negative.");
        }

        T item = GetItemById(id);

        item.Quantity = newQuantity;
    }
}


// ======================================================
// 6. WAREHOUSE MANAGER
// ======================================================

public class WareHouseManager
{
    private InventoryRepository<ElectronicItem> _electronics;
    private InventoryRepository<GroceryItem> _groceries;


    // Constructor
    public WareHouseManager()
    {
        _electronics =
            new InventoryRepository<ElectronicItem>();

        _groceries =
            new InventoryRepository<GroceryItem>();
    }


    // ==================================================
    // SEED DATA
    // ==================================================

    public void SeedData()
    {
        try
        {
            // Electronic items

            ElectronicItem laptop =
                new ElectronicItem(
                    101,
                    "Laptop",
                    10,
                    "HP",
                    24);

            ElectronicItem smartphone =
                new ElectronicItem(
                    102,
                    "Smartphone",
                    15,
                    "Samsung",
                    12);

            ElectronicItem television =
                new ElectronicItem(
                    103,
                    "Television",
                    5,
                    "LG",
                    24);


            // Grocery items

            GroceryItem rice =
                new GroceryItem(
                    201,
                    "Rice",
                    30,
                    new DateTime(2027, 6, 30));

            GroceryItem milk =
                new GroceryItem(
                    202,
                    "Milk",
                    20,
                    new DateTime(2026, 12, 15));

            GroceryItem bread =
                new GroceryItem(
                    203,
                    "Bread",
                    25,
                    new DateTime(2026, 10, 20));


            // Add electronic items

            _electronics.AddItem(laptop);
            _electronics.AddItem(smartphone);
            _electronics.AddItem(television);


            // Add grocery items

            _groceries.AddItem(rice);
            _groceries.AddItem(milk);
            _groceries.AddItem(bread);
        }
        catch (DuplicateItemException ex)
        {
            Console.WriteLine(
                $"Seed data error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Unexpected error: {ex.Message}");
        }
    }


    // ==================================================
    // PRINT ALL ITEMS
    // ==================================================

    public void PrintAllItems<T>(
        InventoryRepository<T> repo)
        where T : IInventoryItem
    {
        foreach (T item in repo.GetAllItems())
        {
            Console.WriteLine(
                $"ID: {item.Id} | " +
                $"Name: {item.Name} | " +
                $"Quantity: {item.Quantity}");
        }
    }


    // ==================================================
    // PUBLIC METHOD TO PRINT GROCERIES
    // ==================================================

    public void PrintGroceries()
    {
        Console.WriteLine(
            "========== GROCERY ITEMS ==========");

        PrintAllItems(_groceries);
    }


    // ==================================================
    // PUBLIC METHOD TO PRINT ELECTRONICS
    // ==================================================

    public void PrintElectronics()
    {
        Console.WriteLine(
            "========== ELECTRONIC ITEMS ==========");

        PrintAllItems(_electronics);
    }


    // ==================================================
    // INCREASE STOCK
    // ==================================================

    public void IncreaseStock<T>(
        InventoryRepository<T> repo,
        int id,
        int quantity)
        where T : IInventoryItem
    {
        try
        {
            if (quantity < 0)
            {
                throw new InvalidQuantityException(
                    "Quantity to increase cannot be negative.");
            }

            T item = repo.GetItemById(id);

            int newQuantity =
                item.Quantity + quantity;

            repo.UpdateQuantity(
                id,
                newQuantity);

            Console.WriteLine(
                $"Stock increased successfully. " +
                $"New quantity: {newQuantity}");
        }
        catch (ItemNotFoundException ex)
        {
            Console.WriteLine(
                $"Error: {ex.Message}");
        }
        catch (InvalidQuantityException ex)
        {
            Console.WriteLine(
                $"Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Unexpected error: {ex.Message}");
        }
    }


    // ==================================================
    // REMOVE ITEM BY ID
    // ==================================================

    public void RemoveItemById<T>(
        InventoryRepository<T> repo,
        int id)
        where T : IInventoryItem
    {
        try
        {
            repo.RemoveItem(id);

            Console.WriteLine(
                $"Item with ID {id} removed successfully.");
        }
        catch (ItemNotFoundException ex)
        {
            Console.WriteLine(
                $"Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Unexpected error: {ex.Message}");
        }
    }


    // ==================================================
    // TEST DUPLICATE ITEM
    // ==================================================

    public void TryAddDuplicateItem()
    {
        try
        {
            ElectronicItem duplicate =
                new ElectronicItem(
                    101,
                    "Another Laptop",
                    5,
                    "Dell",
                    12);

            _electronics.AddItem(duplicate);
        }
        catch (DuplicateItemException ex)
        {
            Console.WriteLine(
                $"Duplicate error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Unexpected error: {ex.Message}");
        }
    }


    // ==================================================
    // TEST INVALID QUANTITY
    // ==================================================

    public void TryInvalidQuantity()
    {
        try
        {
            _electronics.UpdateQuantity(
                101,
                -5);
        }
        catch (InvalidQuantityException ex)
        {
            Console.WriteLine(
                $"Quantity error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Unexpected error: {ex.Message}");
        }
    }


    // ==================================================
    // TEST REMOVE NON-EXISTENT ITEM
    // ==================================================

    public void TryRemoveNonExistentItem()
    {
        try
        {
            _electronics.RemoveItem(999);
        }
        catch (ItemNotFoundException ex)
        {
            Console.WriteLine(
                $"Remove error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Unexpected error: {ex.Message}");
        }
    }
}


// ======================================================
// 7. MAIN PROGRAM
// ======================================================

public class Program
{
    public static void Main(string[] args)
    {
        // Create warehouse manager
        WareHouseManager warehouse =
            new WareHouseManager();


        // Seed data
        warehouse.SeedData();


        // Print all grocery items
        Console.WriteLine();

        warehouse.PrintGroceries();


        // Print all electronic items
        Console.WriteLine();

        warehouse.PrintElectronics();


        // Test duplicate item
        Console.WriteLine();

        Console.WriteLine(
            "========== DUPLICATE ITEM TEST ==========");

        warehouse.TryAddDuplicateItem();


        // Test non-existent item
        Console.WriteLine();

        Console.WriteLine(
            "========== REMOVE NON-EXISTENT ITEM TEST ==========");

        warehouse.TryRemoveNonExistentItem();


        // Test invalid quantity
        Console.WriteLine();

        Console.WriteLine(
            "========== INVALID QUANTITY TEST ==========");

        warehouse.TryInvalidQuantity();
    }
}

