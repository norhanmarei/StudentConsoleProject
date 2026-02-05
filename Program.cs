using System;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;

namespace StudentApiClient
{
    class Program
    {
        static readonly HttpClient httpClient = new HttpClient();
        static async Task Main(string[] args)
        {
            httpClient.BaseAddress = new Uri("http://localhost:5098/api/students");
            await GetAllStudents();
            await GetPassedStudents();
            await GetAverageGrade();
            await GetStudentById();
            await AddNewStudent();
            await DeleteStudent();
            await UpdateStudent();
        }
        static async Task GetAllStudents()
        {
            try
            {
                Console.WriteLine("\n----------------------------------------\n");
                Console.WriteLine("\nFetching All Students...");
                var students = await httpClient.GetFromJsonAsync<List<Student>>("students/All");
                if (students != null)
                {
                    foreach (var student in students)
                    {
                        Console.WriteLine($"Id: {student.Id} Name: {student.Name} Age: {student.Age} Grade: {student.Grade}\n");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An Error Occurred: " + e.Message);
            }
        }
        static async Task GetPassedStudents()
        {
            try
            {
                Console.WriteLine("\n----------------------------------------\n");
                Console.WriteLine("\nFetching Passed Students Only...");
                var passedStudents = await httpClient.GetFromJsonAsync<List<Student>>("students/Passed");
                if(passedStudents != null)
                {
                    foreach(var student in passedStudents)
                    {
                        Console.WriteLine($"Id: {student.Id} Name: {student.Name} Age: {student.Age} Grade: {student.Grade}\n");
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("An Error Occurred: " + e.Message);
            }
        }
        static async Task GetAverageGrade()
        {
            try
            {
                Console.WriteLine("\n----------------------------------------\n");
                Console.WriteLine("\nFetching Average Grade...");
                var averageGrade = await httpClient.GetFromJsonAsync<double>("students/Average");
                if (averageGrade != null)
                {
                    Console.WriteLine($"Average Grade:  {averageGrade}\n");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An Error Occurred: " + e.Message);
            }
        }
        static async Task GetStudentById()
        {
            try
            {
                Console.WriteLine("\n----------------------------------------\n");
                Console.WriteLine("\nFetching Student By Id...");
                Console.WriteLine("\nEnter Student Id");
                string studentIDInput = Console.ReadLine();
                if (int.TryParse(studentIDInput, out int studentID))
                {
                    var student = await httpClient.GetFromJsonAsync<Student>($"students/{studentID}");
                    if (student != null)
                    {
                        Console.WriteLine($"Id: {student.Id} Name: {student.Name} Age: {student.Age} Grade: {student.Grade}\n");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Input. Please Enter An Integer Number.");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("An Error Occurred: " + e.Message);
            }
        }
        static async Task AddNewStudent()
        {
            Student student = new Student
            {
                Name = "Ola",
                Age = 18,
                Grade = 18
            };
            try
            {
                Console.WriteLine("\n----------------------------------------\n");
                Console.WriteLine("\nAdding A New Student...");
                var response = await httpClient.PostAsJsonAsync("", student);
                if (response.IsSuccessStatusCode)
                {
                    var newStudent = await response.Content.ReadFromJsonAsync<Student>();
                    if (newStudent != null)
                        Console.WriteLine($"Id: {newStudent.Id} Name: {newStudent.Name} Age: {newStudent.Age} Grade: {newStudent.Grade}\n");
                }
                else
                {
                    Console.WriteLine($"Failure: Status Code ---> {response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An Error Occurred: " + e.Message);
            }
        }
        static async Task DeleteStudent()
        {
            int Id = 3;
            try
            {
                Console.WriteLine("\n----------------------------------------\n");
                Console.WriteLine($"\nDeleting Student With Id [{Id}]...");
                var res = await httpClient.DeleteAsync($"students/{Id}");
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Student With Id [{Id}] Has Been Deleted\n");
                }
                else
                {
                    Console.WriteLine($"Failure: Status Code ---> {res.StatusCode}\n");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An Error Occurred: " + e.Message);
            }
        }
        static async Task UpdateStudent()
        {
            Student updatedStudent = new Student
            {
                Id = 2,
                Name = "new Arwa",
                Age = 99,
                Grade = 100
            };
            try
            {
                Console.WriteLine("\n----------------------------------------\n");
                Console.WriteLine("\nUpdating Student...");
                var response = await httpClient.PutAsJsonAsync($"students/{updatedStudent.Id}", updatedStudent);
                if (response.IsSuccessStatusCode)
                {
                    var student = await response.Content.ReadFromJsonAsync<Student>();
                    if(student != null) Console.WriteLine($"Id: {student.Id} Name: {student.Name} Age: {student.Age} Grade: {student.Grade}\n");
                }
                else
                {
                    Console.WriteLine($"Failure: Status Code ---> {response.StatusCode}");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error: " + e.Message);
            }
        }
    }
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public float Grade{ get; set; }
    }
}