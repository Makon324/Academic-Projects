using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Gra
{
    /*================*/
    /*    RENDERER    */
    /*================*/

    sealed class Renderer
    {
        private static readonly Lazy<Renderer> lazyInstance = new Lazy<Renderer>(() => new Renderer());

        private List<string> actionLog = new List<string>();
        private List<string> instructions = new List<string>();

        private List<string> previousFrame = new List<string>(); // Cached previous frame

        private Renderer() // Prevents instantiation from other classes
        {
            Console.CursorVisible = false; // Sets coursor to not visible
        }

        public static Renderer Instance => lazyInstance.Value;

        public void Render(Map map, Player player)
        {
            List<string> infoLines = BuildInfoLines(map, player);
            List<string> currentFrame = BuildCurrentFrame(map, player, infoLines);

            // Changing only changed lines
            for (int y = 0; y < currentFrame.Count; y++)
            {
                if (y >= previousFrame.Count || currentFrame[y] != previousFrame[y])
                {
                    Console.SetCursorPosition(0, y);
                    Console.Write(currentFrame[y]);

                    // Clearing leftover characters if current line is shorter
                    if (y < previousFrame.Count && currentFrame[y].Length < previousFrame[y].Length)
                    {
                        Console.Write(new string(' ', previousFrame[y].Length - currentFrame[y].Length));
                    }
                }
            }

            // Clearing leftover lines from previous frame
            if (previousFrame.Count > currentFrame.Count)
            {
                for (int y = currentFrame.Count; y < previousFrame.Count; y++)
                {
                    Console.SetCursorPosition(0, y);
                    Console.Write(new string(' ', previousFrame[y].Length));
                }
            }

            previousFrame = new List<string>(currentFrame);
        }

        private List<string> BuildInfoLines(Map map, Player player)
        {
            List<string> infoLines = new List<string>();

            // Player Stats
            infoLines.Add("Player Stats:");
            infoLines.Add($"Strength: {player.getAttribute(PlayerAttributes.strength)}  Dexterity: {player.getAttribute(PlayerAttributes.dexterity)}");
            infoLines.Add($"Health: {player.getAttribute(PlayerAttributes.health)}    Luck: {player.getAttribute(PlayerAttributes.luck)}");
            infoLines.Add($"Aggression: {player.getAttribute(PlayerAttributes.aggression)}  Wisdom: {player.getAttribute(PlayerAttributes.wisdom)}");
            infoLines.Add("");

            // Active Effects
            infoLines.Add("Active Effects:");
            var effects = player.GetPotionEffects();
            if (effects.Count > 0)
            {
                foreach (var effect in effects)
                {
                    infoLines.Add(effect.ToString());
                }
            }
            else
            {
                infoLines.Add("");
            }
            infoLines.Add("");

            // Currency
            infoLines.Add("Currency:");
            foreach (Currency currency in Enum.GetValues(typeof(Currency)))
            {
                infoLines.Add($"{currency}: {player.GetCurrencyAmount(currency)}");
            }
            infoLines.Add("");

            // Equipped Items
            infoLines.Add("Equipped Items:");
            infoLines.Add($"Left Hand: {player.LeftHand?.ToString() ?? "Empty"}");
            infoLines.Add($"Right Hand: {player.RightHand?.ToString() ?? "Empty"}");

            // Current Tile Item
            IItem? currentItem = map.peekItem(player.getX, player.getY);
            infoLines.Add("");
            infoLines.Add($"Standing On: {currentItem?.ToString() ?? ""}");

            // Action Log
            infoLines.Add("");
            infoLines.Add($"Recent Action: {actionLog.LastOrDefault() ?? ""}");

            // Inventory
            infoLines.Add("");
            infoLines.Add("Inventory:");
            int c = 1;
            foreach (IItem item in player.Inventory)
            {
                infoLines.Add($"{c}. {item}");
                c++;
            }

            return infoLines;
        }

        private List<string> BuildCurrentFrame(Map map, Player player, List<string> infoLines)
        {
            List<string> currentFrame = new List<string>();

            for (int y = 0; y < infoLines.Count || y < map.Height; y++)
            {
                StringBuilder line = new StringBuilder();
                if (y < map.Height)
                {
                    for (int x = 0; x < map.Width; x++)
                    {
                        var (symbol, color) = map.GetRenderAt(x, y);
                        if (player.getX == x && player.getY == y)
                        {
                            (symbol, color) = player.Render();
                        }
                        line.Append(GetAnsiColor(color) + symbol + "\x1b[0m");
                    }
                }
                else
                {
                    line.Append("".PadRight(map.Width));
                }

                if (y < infoLines.Count)
                {
                    line.Append("  ").Append(infoLines[y]);
                }
                currentFrame.Add(line.ToString());
            }

            currentFrame.AddRange(instructions);
            return currentFrame;
        }

        public void LogAction(string action)
        {
            actionLog.Add(action);
        }

        public void SetInstructions(List<string> _instructions)
        {
            instructions = _instructions;
        }

        private string GetAnsiColor(ConsoleColor color)
        {
            return color switch
            {
                ConsoleColor.Black => "\x1b[30m",
                ConsoleColor.DarkBlue => "\x1b[34m",
                ConsoleColor.DarkGreen => "\x1b[32m",
                ConsoleColor.DarkCyan => "\x1b[36m",
                ConsoleColor.DarkRed => "\x1b[31m",
                ConsoleColor.DarkMagenta => "\x1b[35m",
                ConsoleColor.DarkYellow => "\x1b[33m",
                ConsoleColor.Gray => "\x1b[37m",
                ConsoleColor.DarkGray => "\x1b[90m",
                ConsoleColor.Blue => "\x1b[94m",
                ConsoleColor.Green => "\x1b[92m",
                ConsoleColor.Cyan => "\x1b[96m",
                ConsoleColor.Red => "\x1b[91m",
                ConsoleColor.Magenta => "\x1b[95m",
                ConsoleColor.Yellow => "\x1b[93m",
                ConsoleColor.White => "\x1b[97m",
                _ => "\x1b[0m",
            };
        }
    }
}