using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment_Genric_Console
{
    internal class Program
    {
        static void Main()
        {

            string filePath = @"C:\Users\spadmin\Downloads\Fibonacci.txt"; //file path
            GeneralRequirement2(filePath);
            GeneralRequirement1();
            Console.ReadKey();
        }

        private static void GeneralRequirement1()
        {
            try
            {
                // Prompt the user to enter an integer
                Console.Write("Enter an integer: ");
                int limit = int.Parse(Console.ReadLine());
                int sum = 0;
                string numbers = "";
                for (int i = 1; i <= limit; i++)
                {
                    // Check if the number is divisible by 3 or 5
                    if (i % 3 == 0 || i % 5 == 0)
                    {
                        // Add the number to the sum
                        sum += i;
                        // Add the number to the list of numbers to display
                        if (numbers == "")
                            numbers = i.ToString();  // For the first number
                        else
                            numbers += "+" + i.ToString();  // For subsequent numbers
                    }
                }

                // Display the results
                Console.WriteLine($"{numbers} = {sum}");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
        }

        private static void GeneralRequirement2(string filePath)
        {

            int[] fibonacciNumbers = new int[15];

            fibonacciNumbers[0] = 0;
            fibonacciNumbers[1] = 1;

            for (int i = 2; i < 15; i++)
            {
                fibonacciNumbers[i] = fibonacciNumbers[i - 1] + fibonacciNumbers[i - 2];
            }


            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (int number in fibonacciNumbers)
                    {
                        writer.WriteLine(number);
                    }
                }
                Console.WriteLine("Fibonacci sequence saved to  " + filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: {ex.Message}");
            }
        }
    }
}
