namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Day1();
        }

        static void Day1()
        {
            string[] lines = File.ReadAllLines("day1input.txt");

            int calibrationTotal = 0;
            foreach (string line in lines)
            {
                Console.WriteLine($"PP: {line}");
                string processedLine = line;

                /* START: PROCESSING PART II */
                for (int i = 0; i <= processedLine.Length; i++)
                {
                    string tempLine = processedLine;
                    tempLine = processedLine.Substring(0, i);

                    tempLine = tempLine.Replace("one", "1");
                    tempLine = tempLine.Replace("two", "2");
                    tempLine = tempLine.Replace("three", "3");
                    tempLine = tempLine.Replace("four", "4");
                    tempLine = tempLine.Replace("five", "5");
                    tempLine = tempLine.Replace("six", "6");
                    tempLine = tempLine.Replace("seven", "7");
                    tempLine = tempLine.Replace("eight", "8");
                    tempLine = tempLine.Replace("nine", "9");

                    if (tempLine != processedLine.Substring(0, i))
                    {
                        //Console.WriteLine($"p:{processedLine.Substring(0, i)}\nx:{tempLine}");
                        if (processedLine.Length > i)
                        {
                            processedLine = tempLine + processedLine.Substring(i-1);
                        }
                        else
                        {
                            processedLine = tempLine;
                        }
                        //Console.WriteLine($"n: {processedLine}");
                        i = 0;
                    }
                }

                Console.WriteLine($"PX: {processedLine}");
                /* END: PROCESSING PART II */

                int? first = null;
                int? last = null;
                foreach (char c in processedLine)
                {
                    if (int.TryParse(c.ToString(), out int result))
                    {
                        //Console.WriteLine($"found digit: {result}");
                        if (first is null)
                        {
                            first = result;
                        }
                        last = result;
                    }
                }

                string lineCalibration = $"{first}{last}";
                Console.WriteLine($"lineCalibration: {lineCalibration}");
                Console.WriteLine();

                calibrationTotal += int.Parse(lineCalibration);
            }

            Console.WriteLine($"calibrationTotal: {calibrationTotal}");
        }
    }
}
