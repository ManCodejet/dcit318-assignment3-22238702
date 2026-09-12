using System;
using System.Collections.Generic;
using System.IO;

// ==========================================
// STUDENT CLASS
// ==========================================
public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public int Score { get; set; }

    // Method to determine the student's grade
    public string GetGrade()
    {
        if (Score >= 80 && Score <= 100)
        {
            return "A";
        }
        else if (Score >= 70)
        {
            return "B";
        }
        else if (Score >= 60)
        {
            return "C";
        }
        else if (Score >= 50)
        {
            return "D";
        }
        else
        {
            return "F";
        }
    }
}

// ==========================================
// CUSTOM EXCEPTION FOR INVALID SCORE
// ==========================================
public class InvalidScoreFormatException : Exception
{
    public InvalidScoreFormatException(string message)
        : base(message)
    {
    }
}

// ==========================================
// CUSTOM EXCEPTION FOR MISSING DATA
// ==========================================
public class MissingFieldException : Exception
{
    public MissingFieldException(string message)
        : base(message)
    {
    }
}

// ==========================================
// STUDENT RESULT PROCESSOR CLASS
// ==========================================
public class StudentResultProcessor
{
    // ==========================================
    // READ STUDENTS FROM FILE
    // ==========================================
    public List<Student> ReadStudentsFromFile(string inputFilePath)
    {
        List<Student> students = new List<Student>();

        using (StreamReader reader = new StreamReader(inputFilePath))
        {
            string line;
            int lineNumber = 0;

            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;

                // Ignore empty lines
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                // Split the line using comma
                string[] fields = line.Split(',');

                // Check if there are exactly three fields
                if (fields.Length != 3)
                {
                    throw new MissingFieldException(
                        $"Line {lineNumber}: A student record must contain ID, Full Name and Score.");
                }

                // Remove unnecessary spaces
                string idText = fields[0].Trim();
                string fullName = fields[1].Trim();
                string scoreText = fields[2].Trim();

                // Check for missing fields
                if (string.IsNullOrWhiteSpace(idText) ||
                    string.IsNullOrWhiteSpace(fullName) ||
                    string.IsNullOrWhiteSpace(scoreText))
                {
                    throw new MissingFieldException(
                        $"Line {lineNumber}: One or more required fields are missing.");
                }

                // Convert ID to integer
                int id;

                if (!int.TryParse(idText, out id))
                {
                    throw new InvalidScoreFormatException(
                        $"Line {lineNumber}: Student ID must be a number.");
                }

                // Convert score to integer
                int score;

                if (!int.TryParse(scoreText, out score))
                {
                    throw new InvalidScoreFormatException(
                        $"Line {lineNumber}: Invalid score format for {fullName}.");
                }

                // Check that score is between 0 and 100
                if (score < 0 || score > 100)
                {
                    throw new InvalidScoreFormatException(
                        $"Line {lineNumber}: Score for {fullName} must be between 0 and 100.");
                }

                // Create a Student object
                Student student = new Student
                {
                    Id = id,
                    FullName = fullName,
                    Score = score
                };

                // Add student to the list
                students.Add(student);
            }
        }

        return students;
    }

    // ==========================================
    // WRITE REPORT TO FILE
    // ==========================================
    public void WriteReportToFile(
        List<Student> students,
        string outputFilePath)
    {
        using (StreamWriter writer = new StreamWriter(outputFilePath))
        {
            foreach (Student student in students)
            {
                writer.WriteLine(
                    $"{student.FullName} (ID: {student.Id}): " +
                    $"Score = {student.Score}, Grade = {student.GetGrade()}");
            }
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
        string inputFilePath = "students.txt";
        string outputFilePath = "student_results.txt";

        try
        {
            // Create StudentResultProcessor object
            StudentResultProcessor processor =
                new StudentResultProcessor();

            // Read students from the input file
            List<Student> students =
                processor.ReadStudentsFromFile(inputFilePath);

            // Write the results to the output file
            processor.WriteReportToFile(
                students,
                outputFilePath);

            // Display success message
            Console.WriteLine("Student results processed successfully.");
            Console.WriteLine("Number of students: " + students.Count);
            Console.WriteLine("Report saved to: " + outputFilePath);
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine(
                "Error: The input file 'students.txt' was not found.");
        }
        catch (InvalidScoreFormatException ex)
        {
            Console.WriteLine(
                "Score Format Error: " + ex.Message);
        }
        catch (MissingFieldException ex)
        {
            Console.WriteLine(
                "Missing Field Error: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                "Unexpected Error: " + ex.Message);
        }
    }
}