using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SkiPath
{
    class Program
    {
        static void Main(string[] args)
        {
            bool writeToConsole = false;            

            if (args.Count() == 0)
            {
                Console.Error.WriteLine("no file path given");
                return;
            }

            int[,] arr = ReadFile(args[0]);


            Console.WriteLine("----------------------------------------------------");

            var s = new SkiPathCalculator(arr,  (current, next) => current>next);
            var max = s.GetMax();
           
            Console.WriteLine("\r\nLength {0}\r\nDrop {1}", max.Depth, max.Drop());

            // output path
            var currentNode = max;
            var path = new List<int>();
            while (currentNode != null)
            {
                path.Add(currentNode.Value);
                currentNode = currentNode.Parent;
            }
            path.Reverse();

            Console.Write("PATH: ");
            foreach (var val in path)
                Console.Write("{0} - ", val);

            Console.ReadKey();
        }


        private static int[,] ReadFile(string path)
        {
            int[,] arr=null;
            var f = File.ReadAllText(path);

            var ii = 0;
            foreach (var line in f.Split(new[] { '\n' }))
            {
                var jj = 0;
                if (ii == 0)
                {
                    var val = line.Split(new[] { ' ' });
                    arr = new int[Convert.ToInt32(val[0]), Convert.ToInt32(val[1])];                 
                }
                else
                {
                    if (!string.IsNullOrEmpty(line.Trim()))
                        foreach (var val in line.Split(new[] { ' ' }))
                            arr[ii - 1, jj++] = Convert.ToInt32(val);
                }
                ii++;
            }

            return arr;
        }
    }
}
