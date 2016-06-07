using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {


            if (File.Exists("test.txt"))
            {
                Console.WriteLine("Please enter a new name for this file:");
                string newFilename = Console.ReadLine();
                if (newFilename != String.Empty)
                {
                    File.Move("test.txt", newFilename);
                    if (File.Exists(newFilename))
                    {
                        Console.WriteLine("The file was renamed to " + newFilename);
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
