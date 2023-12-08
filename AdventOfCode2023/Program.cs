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
            //Day4();
            //Day6();
            //Day7();
            Day8();
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
                    IEnumerable<int> parts = potentialGears.Where(g => g.pos.x == gear.pos.x && g.pos.y == gear.pos.y).Select(g => g.partNumber);
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
            string[] raw = File.ReadAllLines("input/day4full.txt");
            int pointsTotal = 0;
            Dictionary<int, int> scratchCards = new Dictionary<int, int>();
            int cardNum = 0;

            foreach (string card in raw)
            {
                if (!scratchCards.TryAdd(cardNum, 1))
                {
                    scratchCards[cardNum] += 1;
                }
                //int cardPoints = 0;
                string[] cardNumbers = card.Split(":")[1].Split("|");
                List<int> winningNumbers = cardNumbers[0].Trim().Replace("  ", " ").Split(" ").Select(x => int.Parse(x)).ToList();
                List<int> selectedNumbers = cardNumbers[1].Trim().Replace("  ", " ").Split(" ").Select(x => int.Parse(x)).ToList();

                int winCount = selectedNumbers.Where(x => winningNumbers.Contains(x)).Count();
                for (int i = cardNum + 1; i < raw.Length && i < winCount + cardNum + 1; i++)
                {
                    if (!scratchCards.TryAdd(i, 1 * scratchCards[cardNum]))
                    {
                        scratchCards[i] += 1 * scratchCards[cardNum];
                    }
                }

                //foreach (int selectedNumber in selectedNumbers)
                //{
                //    if (winningNumbers.Contains(selectedNumber))
                //    {
                //        if (cardPoints == 0)
                //        {
                //            cardPoints = 1;
                //        }
                //        else
                //        {
                //            cardPoints *= 2;
                //        }
                //    }
                //}
                //pointsTotal += cardPoints;
                cardNum++;
            }

            int cardsTotal = scratchCards.Sum(x => x.Value);

            Console.WriteLine($"cardstotal: {cardsTotal}");

        }

        static void Day6()
        {
            string[] raw = File.ReadAllLines("input/day6full.txt");
            List<(long, long)> timeDistanceRecords = new List<(long, long)>();

            string[] rawTimes = raw[0].Split(":")[1].Trim().Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            string[] rawDistances = raw[1].Split(":")[1].Trim().Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            for (int i = 0; i < rawTimes.Length; i++)
            {
                timeDistanceRecords.Add((long.Parse(rawTimes[i]), long.Parse(rawDistances[i])));
            }

            long winMultiplier = 1;
            foreach ((long time, long distance) record in timeDistanceRecords)
            {
                long waysToWin = 0;
                Console.WriteLine($"time: {record.time}, distance: {record.distance}");
                for (long i = 0; i <= record.time; i++)
                {
                    long distanceTraveled = i * (record.time - i);
                    if (distanceTraveled > record.distance)
                    {
                        waysToWin++;
                    }
                }
                winMultiplier *= waysToWin;
            }

            Console.WriteLine($"winMultiplier: {winMultiplier}");
        }

        static void Day7()
        {
            string[] raw = File.ReadAllLines("input/day7full.txt");

            List<(List<int> cards, int bid)> hands = new List<(List<int> cards, int bid)>();
            foreach (string line in raw)
            {
                string[] split = line.Split(" ");
                int bid = int.Parse(split[1]);
                List<int> cards = new List<int>();
                foreach (char c in split[0])
                {
                    if (int.TryParse(c.ToString(), out int result))
                    {
                        cards.Add(result);
                    }
                    else
                    {
                        switch (c)
                        {
                            case 'A':
                                cards.Add(14);
                                break;
                            case 'K':
                                cards.Add(13);
                                break;
                            case 'Q':
                                cards.Add(12);
                                break;
                            case 'J':
                                cards.Add(1); // modified for part 2
                                break;
                            case 'T':
                                cards.Add(10);
                                break;
                        }
                    }
                }

                hands.Add((cards, bid));
            }

            hands = hands.OrderBy(x => x.cards, new HandComparer()).ToList();

            int totalWinnings = 0;
            for (int i = 0; i < hands.Count; i++)
            {
                totalWinnings += hands[i].bid * (i + 1);
            }

            Console.Write(totalWinnings);
        }

        public class HandComparer : IComparer<List<int>>
        {
            public int Compare(List<int>? x, List<int>? y)
            {
                int val = HandRank(x) - HandRank(y);
                if (val != 0)
                {
                    return val;
                }
                else
                {
                    for (int i = 0; i < x.Count; i++)
                    {
                        if (x[i] > y[i])
                        {
                            return 1;
                        }
                        else if (x[i] < y[i])
                        {
                            return -1;
                        }
                        else
                        {
                            if (i == x.Count - 1)
                            {
                                return 0;
                            }
                        }
                    }
                }

                // we shouldn't get here
                throw new Exception();
                return 0;
            }

            private int HandRank(List<int> hand)
            {
                Dictionary<int, int> cardCounts = new Dictionary<int, int>();
                foreach (int card in hand)
                {
                    if (!cardCounts.TryAdd(card, 1))
                    {
                        cardCounts[card] += 1;
                    }
                }

                // for part 2
                if (cardCounts.TryGetValue(1, out int jokerCount))
                {
                    cardCounts[1] = 0;

                    int maxKey = cardCounts.MaxBy(x => x.Value).Key;
                    cardCounts[maxKey] += jokerCount;
                }

                if (cardCounts.Any(x=>x.Value == 5))
                {
                    return 6;
                }
                else if (cardCounts.Any(x=>x.Value ==4))
                {
                    return 5;
                }
                else if (cardCounts.Any(x=>x.Value == 3) && cardCounts.Any(x=>x.Value == 2))
                {
                    return 4;
                }
                else if (cardCounts.Any(x=>x.Value == 3))
                {
                    return 3;
                }
                else if (cardCounts.Where(x=> x.Value == 2).Count() == 2)
                {
                    return 2;
                }
                else if (cardCounts.Any(x=>x.Value == 2))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        static void Day8()
        {
            string[] raw = File.ReadAllLines("input/day8full.txt");
            string instructions = raw[0];

            Dictionary<string, (string left, string right)> map = new Dictionary<string, (string left, string right)>();

            string finalLocationKey = "ZZZ";
            string firstKey = "AAA";

            for (int i = 2; i < raw.Length; i++)
            {
                string[] loc = raw[i].Split("=");
                loc[0] = loc[0].Trim();
                loc[1] = loc[1].Trim().Replace("(","").Replace(")","");

                map.Add(loc[0], (loc[1].Split(", ")[0].Trim(), loc[1].Split(", ")[1].Trim()));
            }

            string currentKey = firstKey;
            int counter = 0;
            bool endOfMap = false;
            do
            {
                var location = map[currentKey];
                char direction = instructions[counter % instructions.Length];
                if (direction == 'L')
                {
                    currentKey = location.left;
                }
                else if (direction == 'R')
                {
                    currentKey = location.right;
                }
                counter++;

                if (currentKey == finalLocationKey)
                {
                    endOfMap = true;
                }
            } while (!endOfMap);

            Console.WriteLine($"counter: {counter}");
        }
    }
}
