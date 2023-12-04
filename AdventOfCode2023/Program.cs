namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            //Day1();
            //Day2();
            //Day3();
            Day4();
        }

        static void Day1()
        {
            string[] lines = File.ReadAllLines("input/day1full.txt");

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

        static void Day2()
        {
            string[] raw = File.ReadAllLines("input/day2full.txt");

            Dictionary<int, bool> gamePossible = new Dictionary<int, bool>();
            Dictionary<int, int> gamePower = new Dictionary<int, int>(); // this didn't need to be a dictionary, could've summed the powers as we went, but it allowed for potential debugging of individual games
            int possibleGamesSum = 0;
            int powerSum = 0;

            (int red, int green, int blue) possible = (12, 13, 14);
            foreach (string rawGame in raw)
            {
                string[] idAndData = rawGame.Split(":");
                int gameId = int.Parse(idAndData[0].Split(" ")[1]);

                int minRed = 0;
                int minGreen = 0;
                int minBlue = 0;

                string[] gameRolls = idAndData[1].Split(";");
                foreach (string gameRoll in gameRolls)
                {
                    string[] rollData = gameRoll.Split(",");
                    // This is a bit silly and could almost certainly be done better, but it works
                    int red = int.Parse(rollData.Where(x => x.Contains("red"))?.FirstOrDefault()?.Split(" ")[1] ?? "0");
                    int green = int.Parse(rollData.Where(x => x.Contains("green"))?.FirstOrDefault()?.Split(" ")[1] ?? "0");
                    int blue = int.Parse(rollData.Where(x => x.Contains("blue"))?.FirstOrDefault()?.Split(" ")[1] ?? "0");

                    if (red <= possible.red && green <= possible.green && blue <= possible.blue)
                    {
                        // try add rather and no overwrite - it will either already be true or if it's false, we want it to stay false
                        gamePossible.TryAdd(gameId, true);
                    }
                    else
                    {
                        // I think we could probably just do the stuff in the if block, but this is probably better practice?  and I already wrote it
                        if (!gamePossible.TryAdd(gameId, false))
                        {
                            gamePossible[gameId] = false;
                        }
                    }

                    if (red > minRed)
                    {
                        minRed = red;
                    }
                    if (green > minGreen)
                    {
                        minGreen = green;
                    }
                    if (blue > minBlue)
                    {
                        minBlue = blue;
                    }
                }

                gamePower.Add(gameId, minRed * minGreen * minBlue);
            }

            foreach (KeyValuePair<int, bool> game in gamePossible)
            {
                if (game.Value)
                {
                    possibleGamesSum += game.Key;
                }
            }

            foreach (KeyValuePair<int, int> game in gamePower)
            {
                powerSum += game.Value;
            }

            Console.WriteLine($"possibleGamesSum: {possibleGamesSum}");
            Console.WriteLine($"powerSum: {powerSum}");
        }

        /// <summary>
        /// The code is super gross.  It is not optimized at all, there are almost certainly better examples to work off of.
        /// It also lack any sort of comments and it is certainly not self-documenting.
        /// Good luck if you try to use it :).
        /// </summary>
        static void Day3()
        {
            char[] ignoreChars = { '.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            string[] raw = File.ReadAllLines("input/day3full.txt");
            int gridWidth = raw[0].Length;
            int gridHeight = raw.Length;
            int partSum = 0;

            char[,] grid = new char[gridWidth, gridHeight];

            for (int i = 0; i < gridHeight; i++)
            {
                string line = raw[i];
                for (int j = 0; j < gridWidth; j++)
                {
                    grid[j, i] = line[j];
                }
            }

            // do not do this with tuples
            List<((int x, int y) pos, int partNumber)> potentialGears = new List<((int x, int y) pos, int partNumber)>();

            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    if (int.TryParse(grid[x, y].ToString(), out _) && grid[x, y] != '.')
                    {

                        bool endOfPart = false;
                        string partString = grid[x, y].ToString();
                        int xStart = x-1;
                        int yStart = y-1;
                        int yEnd = y+2;
                        x = int.Min(x+1, gridWidth-1);
                        do
                        {
                            if (int.TryParse(grid[x, y].ToString(), out _) && grid[x, y] != '.')
                            {
                                partString += grid[x, y].ToString();
                                x++;
                            }
                            else
                            {
                                endOfPart = true;
                            }
                        } while (!endOfPart && x < gridWidth);

                        x++;
                        if (x >= gridWidth)
                        {
                            x = gridWidth-1;
                        }
                        if (xStart < 0)
                        {
                            xStart = 0;
                        }
                        if (yStart < 0)
                        {
                            yStart = 0;
                        }
                        if (yEnd > gridHeight)
                        {
                            yEnd = gridHeight;
                        }


                        for (int xx = xStart; xx < x; xx++)
                        {
                            for (int yy = yStart; yy < yEnd; yy++)
                            {
                                char currentChar = grid[xx, yy];
                                if (!ignoreChars.Contains(currentChar))
                                {
                                    partSum += int.Parse(partString);
                                    if (currentChar == '*')
                                    {
                                        potentialGears.Add(((xx, yy), int.Parse(partString)));
                                    }
                                    break;
                                }
                            }
                        }

                        if (x < gridWidth - 2)
                        {
                            x = int.Max(x - 2, 0);
                        }
                    }
                }
            }

            int gearRatioSum = 0;
            HashSet<(int x, int y)> gearPositions = new HashSet<(int x, int y)>();
            foreach (((int x, int y) pos, int partNumber) gear in potentialGears)
            {
                if (!gearPositions.Contains(gear.pos))
                {
                    int multiplier = 1;
                    IEnumerable<int> parts= potentialGears.Where(g => g.pos.x == gear.pos.x && g.pos.y == gear.pos.y).Select(g=>g.partNumber);
                    if (parts.Count() <= 1) continue; // gross, don't use this syntax
                    foreach (int part in parts)
                    {
                        multiplier *= part;
                    }
                    gearRatioSum += multiplier;
                    gearPositions.Add(gear.pos);
                }
            }
            Console.ReadKey();
        }

        static void Day4()
        {

        }
    }
}
