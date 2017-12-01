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
            var size = 1000;
            bool write = false;

            int[,] arr = new int[size, size]; /*{
                    { 2 , 3 , 4 , 6, 0 },
                    { 1 , 3 , 4 , 8, 0 },
                    { 2 , 17 , 16 , 9, 10 },
                    { 2 , 18 , 15 , 14, 11 },
                    { 2 , 19 , 20 , 13, 12 },
                };*/

            var f = File.ReadAllText(@"C:\Users\vladimir.mandaric\Downloads\map.txt");

            var ii = 0;
            foreach (var line in f.Split(new[] { '\n' }))
            {
                var jj = 0;
                if (ii == 0)
                {
                    ii++;
                    continue;
                }
                if (!string.IsNullOrEmpty(line.Trim()))
                    foreach (var val in line.Split(new[] { ' ' }))
                        arr[ii - 1, jj++] = Convert.ToInt32(val);
                ii++;
            }

            Random rnd = new Random();

            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    //  arr[i, j] = rnd.Next(0, size*3);
                    if (write)
                        Console.Write(arr[i, j].ToString().PadRight(6));
                }
                if (write)
                    Console.WriteLine();
            }

            Console.WriteLine("----------------------------------------------------");

            var s = new SkiPathCalculator(arr, size, size);

            var start = DateTime.UtcNow;
            var max = s.GetMax();
            var duration = (DateTime.UtcNow-start).TotalMilliseconds;

            if (write)
                for (var i = 0; i < size; i++)
                {
                    for (var j = 0; j < size; j++)
                    {
                        var pp = max;
                        var ok = false;
                        while (pp.Parent != null)
                        {
                            if (pp.Position.X == i && pp.Position.Y == j)
                                ok = true;
                            pp = pp.Parent;
                        }
                        if (pp.Position.X == i && pp.Position.Y == j)
                            ok = true;
                        if (ok)
                            Console.Write(arr[i, j].ToString().PadRight(6));
                        else
                            Console.Write(" ".PadRight(6));
                    }
                    Console.WriteLine();
                }

            Console.WriteLine("FIN");
            Console.ReadKey();
        }
    }
}
