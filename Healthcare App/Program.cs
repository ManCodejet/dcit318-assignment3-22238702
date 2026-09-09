```csharp
using System;
using System.Collections.Generic;
using System.Linq;

// ===============================================
// 1. GENERIC REPOSITORY
// ===============================================

public class Repository<T>
{
    private List<T> items = new List<T>();

    public void Add(T item)
    {
        items.Add(item);
    }

    public List<T> GetAll()
    {
        return items;
    }

    public T? GetById(Func<T, bool> predicate)
    {
        return items.FirstOrDefault(predicate);
    }

    public bool Remove(Func<T, bool> predicate)
    {
        T? item = items.FirstOrDefault(predicate);

        if (item != null)
        {
            items.Remove(item);
            return true;
        }

        return false;
    }
}


// ===============================================
// 2. PATIENT CLASS
// ===============================================

public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }

    public Patient(int id, string name, int age, string gender)
    {
        Id = id;
        Name = name;
        Age = age;
        Gender = gender;
    }
}


// ===============================================
// 3. PRESCRIPTION CLASS
// ===============================================

public class Prescription
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string MedicationName { get; set; }
    public DateTime DateIssued { get; set; }

    public Prescription(
        int id,
        int patientId,
        string medicationName,
        DateTime dateIssued)
    {
        Id = id;
        PatientId = patientId;
        MedicationName = medicationName;
        DateIssued = dateIssued;
    }
}


// ===============================================
// 4. HEALTH SYSTEM APPLICATION
// ===============================================

public class HealthSystemApp
{
    private Repository<Patient> _patientRepo;
    private Repository<Prescription> _prescriptionRepo;

    private Dictionary<int, List<Prescription>> _prescriptionMap;


    // ===========================================
    // CONSTRUCTOR
    // ===========================================

    public HealthSystemApp()
    {
        _patientRepo = new Repository<Patient>();
        _prescriptionRepo = new Repository<Prescription>();

        _prescriptionMap =
            new Dictionary<int, List<Prescription>>();
    }


    // ===========================================
    // 5. SEED DATA
    // ===========================================

    public void SeedData()
    {
        // Create patients
        Patient patient1 =
            new Patient(1, "Kwame Mensah", 35, "Male");

        Patient patient2 =
            new Patient(2, "Ama Boateng", 28, "Female");

        Patient patient3 =
            new Patient(3, "Kofi Asare", 42, "Male");


        // Add patients to repository
        _patientRepo.Add(patient1);
        _patientRepo.Add(patient2);
        _patientRepo.Add(patient3);


        // Create prescriptions
        Prescription prescription1 =
            new Prescription(
                1,
                1,
                "Paracetamol",
                DateTime.Now);

        Prescription prescription2 =
            new Prescription(
                2,
                1,
                "Amoxicillin",
                DateTime.Now);

        Prescription prescription3 =
            new Prescription(
                3,
                2,
                "Ibuprofen",
                DateTime.Now);

        Prescription prescription4 =
            new Prescription(
                4,
                2,
                "Vitamin C",
                DateTime.Now);

        Prescription prescription5 =
            new Prescription(
                5,
                3,
                "Cetirizine",
                DateTime.Now);


        // Add prescriptions to repository
        _prescriptionRepo.Add(prescription1);
        _prescriptionRepo.Add(prescription2);
        _prescriptionRepo.Add(prescription3);
        _prescriptionRepo.Add(prescription4);
        _prescriptionRepo.Add(prescription5);
    }


    // ===========================================
    // 6. BUILD PRESCRIPTION MAP
    // ===========================================

    public void BuildPrescriptionMap()
    {
        foreach (Prescription prescription
                 in _prescriptionRepo.GetAll())
        {
            if (!_prescriptionMap.ContainsKey(prescription.PatientId))
            {
                _prescriptionMap[prescription.PatientId] =
                    new List<Prescription>();
            }

            _prescriptionMap[prescription.PatientId]
                .Add(prescription);
        }
    }


    // ===========================================
    // 7. PRINT ALL PATIENTS
    // ===========================================

    public void PrintAllPatients()
    {
        Console.WriteLine("========== ALL PATIENTS ==========");

        foreach (Patient patient
                 in _patientRepo.GetAll())
        {
            Console.WriteLine(
                $"ID: {patient.Id} | " +
                $"Name: {patient.Name} | " +
                $"Age: {patient.Age} | " +
                $"Gender: {patient.Gender}");
        }
    }


    // ===========================================
    // 8. PRINT PRESCRIPTIONS FOR A PATIENT
    // ===========================================

    public void PrintPrescriptionsForPatient(int patientId)
    {
        Console.WriteLine();
        Console.WriteLine(
            $"===== PRESCRIPTIONS FOR PATIENT {patientId} =====");


        if (_prescriptionMap.TryGetValue(
                patientId,
                out List<Prescription>? prescriptions))
        {
            foreach (Prescription prescription
                     in prescriptions)
            {
                Console.WriteLine(
                    $"Prescription ID: {prescription.Id} | " +
                    $"Medication: {prescription.MedicationName} | " +
                    $"Date: {prescription.DateIssued}");
            }
        }
        else
        {
            Console.WriteLine(
                "No prescriptions found for this patient.");
        }
    }
}


// ===============================================
// 9. MAIN PROGRAM
// ===============================================

public class Program
{
    public static void Main(string[] args)
    {
        HealthSystemApp app =
            new HealthSystemApp();

        app.SeedData();

        app.BuildPrescriptionMap();

        app.PrintAllPatients();

        int selectedPatientId = 1;

        app.PrintPrescriptionsForPatient(
            selectedPatientId);
    }
}
```
