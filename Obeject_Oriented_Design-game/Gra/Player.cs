using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gra
{
    class Entity
    {
        private int X;
        private int Y;

        public int getX => X;
        public int getY => Y;

        public Entity(int _X, int _Y)
        {
            X = _X;
            Y = _Y;
        }

        public bool move(int newX, int newY, Map M)
        {
            if (M.tileAviable(newX, newY))
            {
                X = newX;
                Y = newY;
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual (char, ConsoleColor) Render()
        {
            return ('E', ConsoleColor.White);
        }
    }

    enum PlayerAttributes
    {
        strength,
        dexterity,
        health,
        luck,
        aggression,
        wisdom
    }


    class Player : Entity
    {
        // Attributes
        private Dictionary<PlayerAttributes, int> attributes;

        public int getAttribute(PlayerAttributes A)
        {
            int i = attributes[A];

            // Items modification
            if(RightHand != null)
            {
                i = RightHand.modifyPlayerAttribute(A, i);
            }
            if(LeftHand != null && !LeftHand.isTwoHanded)
            {
                i = LeftHand.modifyPlayerAttribute(A, i);
            }

            // Potions modification
            foreach (var effect in activeObs.OfType<PotionEffect>())
            {
                i = effect.modifyPlayerAttribute(A, i);
            }

            return i;
        }

        public int getBaseAttribute(PlayerAttributes A)
        {
            return attributes[A];
        }

        public void setBaseAttribute(PlayerAttributes A, int i)
        {
            attributes[A] = i;
        }


        // Items
        private IItem? leftHand;
        private IItem? rightHand;
        private List<IItem> inventory;

        public IItem? LeftHand => leftHand;
        public IItem? RightHand => rightHand;
        public List<IItem> Inventory => inventory;

        // Currency
        private Dictionary<Currency, int> playerCurrency;

        public int GetCurrencyAmount(Currency C)
        {
            return playerCurrency[C];
        }        

        public bool changeCurrency(Currency C, int v) // returns false if change cant happen
        {
            if (playerCurrency[C] + v >= 0)
            {
                playerCurrency[C] += v;
                return true;
            }
            else
            {
                return false;
            }
        }

        // Potion Effects
        private List<ITurnObserver> activeObs = new List<ITurnObserver>();

        public void addPotionEffect(PotionEffect effect)
        {
            activeObs.Add(effect);
        }

        public void countTurn()
        {
            // Notify all observers
            int i = 0;
            while (i < activeObs.Count)
            {
                if (activeObs[i].OnTurnEnded())
                    activeObs.RemoveAt(i);
                else
                    i++;
            }
        }
        public List<PotionEffect> GetPotionEffects()
        {
            return activeObs
                .OfType<PotionEffect>()
                .ToList();
        }


        // Inventory
        public void pickItem(IItem I)
        {
            Renderer.Instance.LogAction($"Picked {I.getName()}");
            if(I.pickUp(this) != null)
            {
                inventory.Add(I);
            }         
        }

        public void EquipItem(int n, bool isLeftHand)
        {
            if((n == -1 && !isLeftHand) || (n == -2 && isLeftHand))
            {
                (rightHand, leftHand) = (leftHand, rightHand);
                Renderer.Instance.LogAction($"Swapped items in hands");
                return;
            }
            if (n < 0 || n >= inventory.Count) return;

            IItem item = inventory[n];

            if (item.isTwoHanded == true)
            {
                UnequipItem(true);
                UnequipItem(false);

                leftHand = item;
                rightHand = item;                
            }
            else
            {
                if(isLeftHand)
                {
                    UnequipItem(true);
                    leftHand = item;
                }
                else
                {
                    UnequipItem(false);
                    rightHand = item;
                }
            }

            Renderer.Instance.LogAction($"Equipped {item.getName()}");
            inventory.RemoveAt(n);
        }

        public void UnequipItem(bool isLeftHand, bool addToInv = true)
        {
            IItem? item = isLeftHand ? LeftHand : RightHand;
            if (item == null) return;

            if (addToInv) inventory.Add(item);

            if (item.isTwoHanded)
            {
                leftHand = null;
                rightHand = null;
            }
            else
            {
                if (isLeftHand)
                    leftHand = null;
                else
                    rightHand = null;
            }
        }

        public void DropItem(int n, Map M)
        {
            if(n == -2 && rightHand != null)
            {
                Renderer.Instance.LogAction($"Dropped {rightHand.getName()}");
                if (rightHand.pickDown(this) != null) M.putItem(getX, getY, rightHand);
                UnequipItem(false, false);
            }
            else if (n == -1 && leftHand != null)
            {
                Renderer.Instance.LogAction($"Dropped {leftHand.getName()}");
                if (leftHand.pickDown(this) != null) M.putItem(getX, getY, leftHand);
                UnequipItem(true, false);
            }
            else if (n >= 0 && n < inventory.Count)
            {
                Renderer.Instance.LogAction($"Dropped {inventory[n].getName()}");
                if (inventory[n].pickDown(this) != null) M.putItem(getX, getY, inventory[n]);
                inventory.RemoveAt(n);                
            }
        }

        public void DropAllItems(Map M)
        {
            // Drop equipped items
            if (LeftHand != null)
            {
                DropItem(-1, M); // -1 represents left hand
            }
            if (RightHand != null)
            {
                DropItem(-2, M); // -2 represents right hand
            }

            // Drop inventory items (iterate backwards to avoid index issues)
            for (int i = Inventory.Count - 1; i >= 0; i--)
            {
                DropItem(i, M);
            }

            Renderer.Instance.LogAction("Dropped all items!");
        }

        // Constructor
        public Player(int _X = 0 , int _Y = 0, Dictionary<PlayerAttributes, int>? _attributes = null) : base(_X, _Y)
        {
            // Attributes
            if(_attributes == null)
            {
                attributes = new Dictionary<PlayerAttributes, int> {
                    { PlayerAttributes.strength, 100},
                    { PlayerAttributes.dexterity, 100},
                    { PlayerAttributes.health, 100},
                    { PlayerAttributes.luck, 100},
                    { PlayerAttributes.aggression, 100},
                    { PlayerAttributes.wisdom, 100}
                };
            }
            else
            {
                attributes = _attributes;
            }

            // Inventory
            leftHand = null;
            rightHand = null;

            inventory = new List<IItem>();

            // Currency
            playerCurrency = new Dictionary<Currency, int>();
            foreach (Currency currency in Enum.GetValues(typeof(Currency)))
            {
                playerCurrency[currency] = 0;
            }

            // Potion Effects
            activeObs = new List<ITurnObserver>();
        }

        public override (char, ConsoleColor) Render()
        {
            return ('¶', ConsoleColor.Red);
        }
    }
}
