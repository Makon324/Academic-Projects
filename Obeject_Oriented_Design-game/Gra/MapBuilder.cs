using System;
using System.Collections.Generic;
using System.Linq;
using Gra;

namespace Gra
{
    /*===================*/
    /*    MAP BUILDER    */
    /*===================*/

    interface IMapBuilder
    {
        void BuildEmptyDungeon(int width, int height);
        void BuildFilledDungeon(int width, int height);
        void AddPaths(int minN = 8, int maxN = 13);
        void AddChambers(int minN = 2, int maxN = 5);
        void AddCentralRoom();
        void AddItems(double chance = 0.1);
        void AddWeapons(double chance = 0.07);
        void AddModifiedWeapons(double chance = 0.03);
        void AddPotions(double chance = 0.06);
        void AddEnemies(double chance = 0.07);
        void Reset();
    }

    class MapDirector
    {
        public void ConstructBasicDungeon(IMapBuilder builder, int width = 40, int height = 20)
        {
            builder.Reset();
            builder.BuildEmptyDungeon(width, height);
            builder.AddCentralRoom();
            builder.AddPaths();
            builder.AddChambers();
            builder.AddItems();            
            builder.AddWeapons();
        }

        public void ConstructComplexDungeon(IMapBuilder builder, int width = 40, int height = 20)
        {
            builder.Reset();
            builder.BuildFilledDungeon(width, height);
            builder.AddCentralRoom();
            builder.AddPaths(16, 20);
            builder.AddChambers(2, 3);
            builder.AddItems();
            builder.AddWeapons();
            builder.AddModifiedWeapons();
            builder.AddPotions();
            builder.AddEnemies();
        }

        public void ConstructMazeDungeon(IMapBuilder builder, int width = 40, int height = 20)
        {
            builder.Reset();
            builder.BuildEmptyDungeon(width, height);
            builder.AddPaths(20, 25);
            builder.AddItems();
        }
    }

    class DungeonBuilder : IMapBuilder
    {
        private Map? map;
        private Random random;

        public DungeonBuilder()
        {
            random = new Random();
        }

        public void BuildEmptyDungeon(int width, int height)
        {
            map = new Map(width, height, (x, y) => new Cell(Tile.NotWall));
        }

        public void BuildFilledDungeon(int width, int height)
        {
            map = new Map(width, height, (x, y) => new Cell(Tile.Wall));
        }

        private enum Direction { None, Right, Left, Up, Down }
        private void GenerateRandomPath(int startX, int startY, int endX, int endY)
        {
            if (map == null) return;

            // Ensure start/end are accessible
            map.GetCell(startX, startY).IsWall = Tile.NotWall;
            map.GetCell(endX, endY).IsWall = Tile.NotWall;

            int currentX = startX;
            int currentY = startY;
            Direction lastDirection = Direction.None;

            while (currentX != endX || currentY != endY)
            {
                int dx = endX - currentX;
                int dy = endY - currentY;

                // Determine valid directions that move toward target
                var allowed = new List<Direction>();
                if (dx > 0) allowed.Add(Direction.Right);
                else if (dx < 0) allowed.Add(Direction.Left);
                if (dy > 0) allowed.Add(Direction.Down);
                else if (dy < 0) allowed.Add(Direction.Up);

                if (allowed.Count == 0) break;

                Direction chosen = Direction.None;

                // Single valid direction
                if (allowed.Count == 1)
                {
                    chosen = allowed[0];
                }
                else
                {
                    // Bias towards last direction if valid
                    if (lastDirection != Direction.None && allowed.Contains(lastDirection))
                    {
                        chosen = random.Next(100) < 70 ?
                            lastDirection :
                            allowed.First(d => d != lastDirection);
                    }
                    else
                    {
                        chosen = allowed[random.Next(allowed.Count)];
                    }
                }

                // Apply movement
                switch (chosen)
                {
                    case Direction.Right: currentX++; break;
                    case Direction.Left: currentX--; break;
                    case Direction.Down: currentY++; break;
                    case Direction.Up: currentY--; break;
                }

                // Update cordinates, directions and set to NotWall
                currentX = Math.Clamp(currentX, 0, map.Width - 1);
                currentY = Math.Clamp(currentY, 0, map.Height - 1);
                map.GetCell(currentX, currentY).IsWall = Tile.NotWall;
                lastDirection = chosen;
            }
        }


        public void AddPaths(int minN = 8, int maxN = 13)
        {
            if (map == null) return;

            // Generate guaranteed path from (0, 0) to center
            int centerX = map.Width / 2;
            int centerY = map.Height / 2;
            GenerateRandomPath(0, 0, centerX, centerY);

            int numPaths = random.Next(minN, maxN);

            // Additional random paths
            for (int i = 0; i < numPaths; i++) // Connect random points
            {
                int x1 = random.Next(map.Width);
                int y1 = random.Next(map.Height);
                int x2 = random.Next(map.Width);
                int y2 = random.Next(map.Height);
                GenerateRandomPath(x1, y1, x2, y2);
            }
        }

        public void AddChambers(int minN = 2, int maxN = 5)
        {
            int numChambers = random.Next(minN, maxN);
            for (int i = 0; i < numChambers; i++)
            {
                int x = random.Next(map.Width - 8);
                int y = random.Next(map.Height - 8);
                int w = random.Next(3, 8);
                int h = random.Next(3, 8);
                for (int dx = 0; dx < w; dx++)
                    for (int dy = 0; dy < h; dy++)
                        if (x + dx < map.Width && y + dy < map.Height)
                            map.GetCell(x + dx, y + dy).IsWall = Tile.NotWall;
            }
        }

        public void AddCentralRoom()
        {
            int centerX = map.Width / 2;
            int centerY = map.Height / 2;
            for (int x = centerX - 5; x < centerX + 5; x++)
                for (int y = centerY - 5; y < centerY + 5; y++)
                    if (x >= 0 && x < map.Width && y >= 0 && y < map.Height)
                        map.GetCell(x, y).IsWall = Tile.NotWall;
        }

        private IItem GenerateRandomItem()
        {
            IItem item;
            int category = random.Next(5);

            switch (category)
            {
                case 0: // 20% Currency
                    var currencyType = random.Next(2) == 0 ? Currency.Gold : Currency.Coin;
                    item = new CurrencyItem($"{currencyType} Pouch", currencyType, random.Next(1, 20));
                    break;

                case 1: // 20% Random weapon (any type)
                    item = GenerateRandomWeapon();
                    break;

                default: // 60% Other items (potions, scrolls, etc)
                    string[] regularItems = {
                "First Aid", "Bread", "Crystal", "String",
                "Crown", "Leather", "Energy Drink", "Scissors",
                "Smoke Bomb", "Antidote", "TNT", "Stick",
                "Wood", "Carrot", "Bottled Water"
            };
                    item = new Item(regularItems[random.Next(regularItems.Length)]);
                    break;
            }

            return item;
        }

        private IWeapon GenerateRandomWeapon()
        {
            string[] weapons = { "Dagger", "Sword", "Axe", "Bow", "Greatsword", "Mace", "Warhammer", "Staff", "Wand" };
            string name = weapons[random.Next(weapons.Length)];

            int damage = name switch
            {
                "Dagger" => random.Next(5, 10),
                "Sword" => random.Next(10, 15),
                "Axe" => random.Next(12, 18),
                "Greatsword" => random.Next(15, 25),
                "Bow" => random.Next(8, 12),
                "Mace" => random.Next(10, 13),
                "Warhammer" => random.Next(10, 20),
                "Staff" => random.Next(10, 18),
                "Wand" => random.Next(8, 15),
                _ => 10
            };

            bool twoHanded = random.Next(4) == 0;

            if (new[] { "Greatsword", "Warhammer", "Axe", "Mace" }.Contains(name))
                return new HeavyWeapon(name, damage, twoHanded);
            else if (new[] { "Dagger", "Sword", "Bow" }.Contains(name))
                return new LightWeapon(name, damage, twoHanded);
            else // "Staff" or "Wand"
                return new MagicWeapon(name, damage, twoHanded);
        }

        public void AddItems(double chance = 0.1)
        {
            for (int x = 0; x < map.Width; x++)
                for (int y = 0; y < map.Height; y++)
                    if (map.GetCell(x, y).IsWall == Tile.NotWall && random.NextDouble() < chance)
                        map.putItem(x, y, GenerateRandomItem());
        }

        public void AddWeapons(double chance = 0.07)
        {
            for (int x = 0; x < map.Width; x++)
                for (int y = 0; y < map.Height; y++)
                    if (map.GetCell(x, y).IsWall == Tile.NotWall && random.NextDouble() < chance)
                        map.putItem(x, y, GenerateRandomWeapon());
        }

        public void AddModifiedWeapons(double chance = 0.03)
        {
            for (int x = 0; x < map.Width; x++)
                for (int y = 0; y < map.Height; y++)
                    if (map.GetCell(x, y).IsWall == Tile.NotWall && random.NextDouble() < chance)
                    {
                        IWeapon baseWeapon = GenerateRandomWeapon();

                        switch (random.Next(3))
                        {
                            case 0:
                                map.putItem(x, y, new EffectDamage(baseWeapon, random.Next(-3, 10)));
                                break;
                            case 1:
                                map.putItem(x, y, new EffectLuckW(baseWeapon, random.Next(-5, 5)));
                                break;
                            default:
                                map.putItem(x, y, new EffectHeavy(baseWeapon, random.Next(5, 15), random.Next(10, 25)));
                                break;
                        }
                    }
        }

        public void AddPotions(double chance = 0.06)
        {
            string[] potions = {
        "Luck Elixir",
        "Strength Tonic",
        "Swiftness Potion"
    };

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    if (map.GetCell(x, y).IsWall == Tile.NotWall && random.NextDouble() < chance)
                    {
                        PotionEffect effect;
                        string potionName = potions[random.Next(potions.Length)];
                        bool isInfinite = random.NextDouble() < 0.3; // 30% chance for infinite duration

                        switch (potionName)
                        {
                            case "Luck Elixir":
                                effect = new LuckyPotionEffect(
                                    additionalLuck: random.Next(10, 21),
                                    turnsActive: isInfinite ? int.MaxValue : random.Next(3, 6)
                                );
                                break;

                            case "Strength Tonic":
                                effect = new StrengthPotionEffect(
                                    boostAmount: random.Next(15, 26),
                                    duration: isInfinite ? int.MaxValue : random.Next(2, 5)
                                );
                                break;

                            case "Swiftness Potion":
                                effect = new SpeedPotionEffect(
                                    boostAmount: random.Next(10, 21),
                                    duration: isInfinite ? int.MaxValue : random.Next(3, 6)
                                );
                                break;

                            default:
                                effect = new LuckyPotionEffect(15, isInfinite ? int.MaxValue : 4);
                                break;
                        }

                        map.putItem(x, y, new PotionItem(
                            isInfinite ? $"Eternal {potionName}" : potionName, // Rename infinite potions
                            effect
                        ));
                    }
                }
            }
        }

        public void AddEnemies(double chance = 0.07)
        {
            string[] enemies = { "Enemy", "Goblin", "Orc", "Cyclops" };
            for (int x = 0; x < map.Width; x++)
                for (int y = 0; y < map.Height; y++)
                    if (map.GetCell(x, y).IsWall == Tile.NotWall && random.NextDouble() < chance)
                        map.SetEnemy(new Enemy(x, y, enemies[random.Next(enemies.Length)]));
        }

        public Map? GetMap() => map;

        public void Reset()
        {
            map = null;
        }
    }

    class InstructionBuilder : IMapBuilder
    {
        private bool itemsOnMap = false;
        private bool weaponsOnMap = false;
        private bool enemiesOnMap = false;
        private bool usableItemsOnMap = false;

        public List<string> GetInstructions()
        {
            List<string> instructionLines = new List<string>();
            instructionLines.Add($"Move: {KeyBindings.MoveUp}/{KeyBindings.MoveDown}/{KeyBindings.MoveLeft}/{KeyBindings.MoveRight}");

            if (itemsOnMap)
            {
                instructionLines.Add($"Pick item: {KeyBindings.Pickup}; Select item: 1-9/{KeyBindings.SelectLeftHand}/{KeyBindings.SelectRightHand}");
                instructionLines.Add($"Selected item: {KeyBindings.InventoryDrop} - Drop, {KeyBindings.InventoryMoveToInventory} - Move to inventory, {KeyBindings.DropAll} - Drop all");
                if (usableItemsOnMap)
                    instructionLines[^1] += $", {KeyBindings.DrinkPotion} - Drink";
                if (weaponsOnMap)
                    instructionLines[^1] += $", {KeyBindings.Attack} - Attack";
            }

            if (enemiesOnMap)
                instructionLines.Add($"Use: {KeyBindings.NormalAttack}/{KeyBindings.StealthAttack}/{KeyBindings.MagicAttack} to attack enemy!");

            instructionLines.Add($"Exit: {KeyBindings.Exit}");
            return instructionLines;
        }

        public void BuildEmptyDungeon(int width = 40, int height = 20) { }
        public void BuildFilledDungeon(int width = 40, int height = 20) { }

        public void AddPaths(int minN = 8, int maxN = 13) { }

        public void AddChambers(int minN = 2, int maxN = 5) { }
        public void AddCentralRoom() { }

        public void AddItems(double chance = 0.1) => itemsOnMap = true;
        public void AddWeapons(double chance = 0.07) { weaponsOnMap = true; itemsOnMap = true; }
        public void AddModifiedWeapons(double chance = 0.03) { weaponsOnMap = true; itemsOnMap = true; }
        public void AddPotions(double chance = 0.06) { usableItemsOnMap = true; itemsOnMap = true; }
        public void AddEnemies(double chance = 0.07) => enemiesOnMap = true;

        public void Reset()
        {
            itemsOnMap = weaponsOnMap = enemiesOnMap = usableItemsOnMap = false;
        }
    }
}