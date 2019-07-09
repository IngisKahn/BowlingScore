namespace BowlingScore.Console
{
    using BowlingScore.Core;
    using System.Collections.Generic;

    class Program
    {
        private static void Main(string[] args)
        {
            var game = new Game();

            for (; ; )
            {
                DisplayScore(game);

                System.Console.WriteLine($"Enter{(game.CurrentFrame.IsComplete ? "" : " Roll Score (0-9 or -,/,X), ")} R to reset, or Q to quit");
                var rawInput = System.Console.ReadKey(true);

                if (rawInput.Key == System.ConsoleKey.Escape)
                    return;

                var input = char.ToLowerInvariant(rawInput.KeyChar);

                if (!game.CurrentFrame.IsComplete)
                {
                    if (char.IsDigit(input) && game.Roll((uint)input - '0'))
                        continue;

                    switch (input)
                    {
                        case '-':
                            if (game.Roll(0))
                                continue;
                            break;
                        case 'x':
                            if (game.Roll(10))
                                continue;
                            break;
                        case '/':
                        case '\\':
                            if (game.Roll(10 - game.LastRoll))
                                continue;
                            break;
                        case 'r':
                            game = new Game();
                            continue;
                        case 'q':
                            return;
                    }

                }
                switch (input)
                {
                    case 'r':
                        game = new Game();
                        continue;
                    case 'q':
                        return;
                }

                System.Console.WriteLine("\nInvalid input!");
                System.Console.ReadKey(true);
            }
        }

        private static void DisplayScore(Game game)
        {
            System.Console.Clear();

            DrawLine('╔', '═', '╤', '═', '╤', '╦', '╗');
            DrawRolls(game);
            DrawLine('║', ' ', '└', '─', '┴', '╢', '╢');
            DrawScores(game.Frames);
            DrawLine('╚', '═', '═', '═', '═', '╩', '╝');
        }

        private static void DrawScores(IReadOnlyList<Frame> frames)
        {
            System.Console.Write('║');

            var score = 0;
            for (var x = 0; x < 9; x++)
                if (frames[x].HasStarted)
                    System.Console.Write($"{score += frames[x].Score,3}║");
                else
                    System.Console.Write("   ║");

            if (frames[9].HasStarted)
                System.Console.WriteLine($"{score += frames[9].Score,5}║");
            else
                System.Console.WriteLine("     ║");
        }

        private static void DrawRolls(Game game)
        {
            System.Console.Write('║');

            var rollIndex = 0;
            var rolls = game.Rolls;

            for (var x = 0; x < 9; x++)
            {
                var frame = game.Frames[x];
                if (frame.RollCount == 0)
                {
                    System.Console.Write(" │ ║");
                    continue;
                }
                var first = rolls[rollIndex++];
                var firstDisplay = first > 0 ? (char)(first + '0') : '-';
                if (frame.RollCount == 1)
                {
                    if (first == 10)
                        System.Console.Write(" │X║");
                    else
                        System.Console.Write($"{firstDisplay}│ ║");
                    continue;
                }
                var second = rolls[rollIndex++];
                var secondDisplay = first + second == 10 ? '/' : second > 0 ? (char)('0' + second) : '-';
                System.Console.Write($"{firstDisplay}│{secondDisplay}║");
            }

            var lastFrame = game.Frames[9];
            var r1 = rolls[rollIndex];
            var r2 = rolls[rollIndex + 1];
            var r3 = rolls[rollIndex + 2];
            var d1 = lastFrame.RollCount == 0 ? ' ' : r1 == 0 ? '-' : r1 == 10 ? 'X' : (char)('0' + r1);
            var d2 = lastFrame.RollCount <= 1 ? ' ' : r2 == 0 ? '-' : r1 + r2 == 10 ? '/' : r2 == 10 ? 'X' : (char)('0' + r2);
            var d3 = lastFrame.RollCount <= 2 ? ' ' : r3 == 0 ? '-' : r2 + r3 == 10 ? '/' : r3 == 10 ? 'X' : (char)('0' + r3);

            System.Console.WriteLine($"{d1}│{d2}│{d3}║");
        }

        private static void DrawLine(char left, char openSpace, char separatorLeft, char closedSpace, char separatorMid, char separatorRight, char right)
        {
            System.Console.Write(left);
            var slot = $"{openSpace}{separatorLeft}{closedSpace}{separatorRight}";
            for (var x = 0; x < 9; x++)
                System.Console.Write(slot);

            System.Console.WriteLine($"{openSpace}{separatorLeft}{closedSpace}{separatorMid}{closedSpace}{right}");
        }
    }
}
