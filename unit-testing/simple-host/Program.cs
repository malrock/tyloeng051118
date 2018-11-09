using calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simple_host
{
    class Program
    {
        static void Main(string[] args)
        {
            float param1 = 1.0f;
            float param2 = 1.0f;
            var res = BasicCalculator.Add(param1,param2);
            Console.WriteLine($"Basic calculator, addition: {param1} + {param2} = {res}");
            
            
            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }
    }
}
