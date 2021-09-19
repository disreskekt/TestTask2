using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TestTask2
{
    class Program
    {
        static object locker = new object();

        static void Main(string[] args)
        {
            var time = new Stopwatch();
            time.Start();

            string text;

            var triplets = new Dictionary<string, int>();

            using (var fileStream = new FileStream(args[0], FileMode.OpenOrCreate))
            {
                byte[] byteArray = new byte[fileStream.Length];

                fileStream.Read(byteArray, 0, byteArray.Length);

                text = Encoding.Default.GetString(byteArray).ToLower();
            }

            var result = Parallel.For(0, text.Length, (int i) =>
            {
                if (Char.IsLetter(text[i]) && Char.IsLetter(text[i + 1]) && Char.IsLetter(text[i + 2]))
                {
                    var triplet = new StringBuilder(3).Append(text[i]).Append(text[i + 1]).Append(text[i + 2]).ToString();

                    lock (locker)
                    {
                        if (triplets.ContainsKey(triplet))
                        {
                            triplets[triplet]++;
                        }
                        else
                        {
                            triplets.Add(triplet, 1);
                        }
                    }
                }
            });

            KeyValuePair<string, int>[] tripletsArray = new KeyValuePair<string, int>[triplets.Count];

            int j = 0;
            foreach (var triplet in triplets)
            {
                tripletsArray[j] = triplet;
                j++;
            }

            Array.Sort(tripletsArray, new KeyValuePairComparer());

            for (int i = tripletsArray.Length - 1; i >= tripletsArray.Length - 10; i--)
            {
                if (i == tripletsArray.Length - 10)
                {
                    Console.WriteLine(tripletsArray[i].Key);
                }
                else
                {
                    Console.Write(tripletsArray[i].Key + ", ");
                }
            }

            time.Stop();

            Console.WriteLine(time.ElapsedMilliseconds);

            Console.ReadLine();
        }
    }
}
