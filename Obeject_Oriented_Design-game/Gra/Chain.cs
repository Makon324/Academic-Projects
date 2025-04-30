using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Gra
{
    static class KeyBindings
    {
        // Movement
        public const ConsoleKey MoveUp = ConsoleKey.W;
        public const ConsoleKey MoveDown = ConsoleKey.S;
        public const ConsoleKey MoveLeft = ConsoleKey.A;
        public const ConsoleKey MoveRight = ConsoleKey.D;

        // Actions
        public const ConsoleKey Pickup = ConsoleKey.E;
        public const ConsoleKey Exit = ConsoleKey.Escape;

        // Inventory Selection
        public const ConsoleKey SelectLeftHand = ConsoleKey.L;
        public const ConsoleKey SelectRightHand = ConsoleKey.R;

        // Inventory Actions
        public const ConsoleKey InventoryDrop = ConsoleKey.D;
        public const ConsoleKey InventoryMoveToInventory = ConsoleKey.I;
        public const ConsoleKey DrinkPotion = ConsoleKey.U;
        public const ConsoleKey Attack = ConsoleKey.R;
        public const ConsoleKey DropAll = ConsoleKey.O;

        // Attack
        public const ConsoleKey NormalAttack = ConsoleKey.N;
        public const ConsoleKey StealthAttack = ConsoleKey.T;
        public const ConsoleKey MagicAttack = ConsoleKey.M;
    }

    abstract class InputHandler
    {
        protected InputHandler Next;

        public InputHandler SetNext(InputHandler next)
        {
            Next = next;
            return next;
        }

        public virtual bool Handle(ConsoleKeyInfo key, Game game)
        {
            return Next?.Handle(key, game) ?? false;
        }
    }

    class MoveHandler : InputHandler
    {
        public override bool Handle(ConsoleKeyInfo key, Game game)
        {
            int newX = game.player.getX;
            int newY = game.player.getY;
            string direction = "";

            switch (key.Key)
            {
                case KeyBindings.MoveUp:
                    newY--;
                    direction = "north";
                    break;
                case KeyBindings.MoveDown:
                    newY++;
                    direction = "south";
                    break;
                case KeyBindings.MoveLeft:
                    newX--;
                    direction = "west";
                    break;
                case KeyBindings.MoveRight:
                    newX++;
                    direction = "east";
                    break;
                default:
                    return base.Handle(key, game);
            }

            if (game.player.move(newX, newY, game.map))
            {
                Renderer.Instance.LogAction($"Moved {direction}");
                return true;
            }
            return false;
        }
    }

    class PickupHandler : InputHandler
    {
        public override bool Handle(ConsoleKeyInfo key, Game game)
        {
            if (key.Key != KeyBindings.Pickup)
                return base.Handle(key, game);

            IItem? item = game.map.extractItem(game.player.getX, game.player.getY);
            if (item != null)
            {
                game.player.pickItem(item);
                return true;
            }
            return false;
        }
    }

    class InventorySelectHandler : InputHandler
    {
        public override bool Handle(ConsoleKeyInfo key, Game game)
        {
            int index = -1;
            if (key.Key >= ConsoleKey.D1 && key.Key <= ConsoleKey.D9)
                index = key.Key - ConsoleKey.D1;
            else if (key.Key == KeyBindings.SelectLeftHand)
                index = -1;
            else if (key.Key == KeyBindings.SelectRightHand)
                index = -2;
            else
                return base.Handle(key, game);

            ConsoleKeyInfo actionKey = Console.ReadKey(true);
            return new InventoryActionHandler(index).Handle(actionKey, game);
        }
    }

    class InventoryActionHandler : InputHandler
    {
        private int selectedIndex;

        public InventoryActionHandler(int index)
        {
            selectedIndex = index;
        }

        public override bool Handle(ConsoleKeyInfo key, Game game)
        {
            switch (key.Key)
            {
                case KeyBindings.InventoryMoveToInventory:
                    HandleMoveToInventory(game);
                    return true;
                case KeyBindings.InventoryDrop:
                    game.player.DropItem(selectedIndex, game.map);
                    return true;                
                case KeyBindings.DrinkPotion:
                    DrinkPotion(game);
                    return true;
                case KeyBindings.SelectLeftHand:
                case KeyBindings.SelectRightHand:
                    game.player.EquipItem(selectedIndex, key.Key == KeyBindings.SelectLeftHand);
                    return true;
                default:
                    return false;
            }
        }

        private void HandleMoveToInventory(Game game)
        {
            if (selectedIndex == -2)
                game.player.UnequipItem(false);
            else if (selectedIndex == -1)
                game.player.UnequipItem(true);
        }

        private void DrinkPotion(Game game)
        {
            IItem? item = null;

            // Check hands first
            if (selectedIndex == -1)
                item = game.player.LeftHand;
            else if (selectedIndex == -2)
                item = game.player.RightHand;
            else if (selectedIndex >= 0 && selectedIndex < game.player.Inventory.Count)
                item = game.player.Inventory[selectedIndex];

            if (item is PotionItem potion)
            {
                potion.drink(game.player);
                Renderer.Instance.LogAction($"Drank {potion.getName()}");

                // Remove from appropriate location
                if (selectedIndex == -1)
                    game.player.UnequipItem(true, false);
                else if (selectedIndex == -2)
                    game.player.UnequipItem(false, false);
                else
                    game.player.Inventory.RemoveAt(selectedIndex);
            }
            else
            {
                Renderer.Instance.LogAction($"Can't drink {item?.ToString() ?? "nothing"}");
            }
        }
    }

    class DropAllHandler : InputHandler
    {
        public override bool Handle(ConsoleKeyInfo key, Game game)
        {
            if (key.Key == KeyBindings.DropAll)
            {
                game.player.DropAllItems(game.map);
                return true;
            }
            return base.Handle(key, game);
        }
    }

    class CombatHandler : InputHandler
    {
        public override bool Handle(ConsoleKeyInfo key, Game game)
        {
            AttackType? attackType = null;
            switch (key.Key)
            {
                case KeyBindings.NormalAttack:
                    attackType = AttackType.Normal;
                    break;
                case KeyBindings.StealthAttack:
                    attackType = AttackType.Stealth;
                    break;
                case KeyBindings.MagicAttack:
                    attackType = AttackType.Magic;
                    break;
                default:
                    return base.Handle(key, game);
            }

            if (attackType.HasValue)
            {
                AttackController.MakeAttack(game.player, game.map, attackType.Value);
                return true;
            }

            return false;
        }
    }

    class ExitHandler : InputHandler
    {
        public override bool Handle(ConsoleKeyInfo key, Game game)
        {
            if (key.Key == KeyBindings.Exit)
            {
                Environment.Exit(0);
                return true;
            }
            return base.Handle(key, game);
        }
    }

    class InvalidInputHandler : InputHandler
    {
        public override bool Handle(ConsoleKeyInfo key, Game game)
        {
            Renderer.Instance.LogAction("Invalid key pressed.");
            return true;
        }
    }
}