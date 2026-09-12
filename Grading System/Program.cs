using System;
using System.Collections.Generic;
using System.IO;

// Student Class
public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public int Score { get; set; }

    public string GetGrade()
    {
        if (Score >= 80 && Score <= 100)
        {
            return "A";
        }
        else if (Score >= 70 && Score <= 79)
        {
            return "B";
        }
        else if (Score >= 60 && Score <= 69)
        {
            return "C";
        }
        else if (Score >= 50 && Score <= 59)
        {
            return "D";
        }
        else
        {
            return "F";
        }
    }
}

// Custom Exception 1
public class InvalidScoreFormatException : Exception
{
    public InvalidScoreFormatException(string message)
        : base(message)
    {
    }
}

// Custom Exception 2
public class MissingFieldException : Exception
{
    public MissingFieldException(string message)
        : base(message)
    {
    }
}

// Student Result Processor
public class StudentResultProcessor
{
    // Read students from the input file
    public List<Student> ReadStudentsFromFile(string inputFilePath)
    {
        List<Student> students = new List<Student>();

        using (StreamReader reader = new StreamReader(inputFilePath))
        {
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                string[] fields = line.Split(',');

                // Check if required fields are missing
                if (fields.Length < 3)
                {
                    throw new MissingFieldException(
                        "Student record is incomplete: " + line);
                }

                int id = int.Parse(fields[0]);
                string fullName = fields[1];

                int score;

                // Try to convert score to integer
                try
                {
                    score = int.Parse(fields[2]);
                }
                catch (FormatException)
                {
                    throw new InvalidScoreFormatException(
                        "Invalid score format for student: " + fullName);
                }

                // Create Student object
                Student student = new Student
                {
                    Id = id,
                    FullName = fullName,
                    Score = score
                };

                // Add student to list
                students.Add(student);
            }
        }

        return students;
    }

    // Write report to output file
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

// Main Program
class Program
{
    static void Main(string[] args)
    {
        string inputFilePath = "students.txt";
        string outputFilePath = "student_results.txt";

        try
        {
            StudentResultProcessor processor =
                new StudentResultProcessor();

            List<Student> students =
                processor.ReadStudentsFromFile(inputFilePath);

            processor.WriteReportToFile(
                students,
                outputFilePath);

            Console.WriteLine(
                "Student results processed successfully.");

            Console.WriteLine(
                "Report saved to: " + outputFilePath);
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine(
                "Error: The input file was not found.");
        }
        catch (InvalidScoreFormatException ex)
        {
            Console.WriteLine(
                "Score Error: " + ex.Message);
        }
        catch (MissingFieldException ex)
        {
            Console.WriteLine(
                "Missing Data Error: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                "Unexpected Error: " + ex.Message);
        }
    }
}