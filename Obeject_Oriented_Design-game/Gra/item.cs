using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Gra
{
    /*============*/
    /*    ITEM    */
    /*============*/

    interface IItem
    {
        public bool isTwoHanded { get; }

        public string getName();

        public int? pickUp(Player P);

        public int? pickDown(Player P);

        public int modifyPlayerAttribute(PlayerAttributes A, int i);

        public (char, ConsoleColor) Render();
    }

    class Item : IItem
    {
        private string name;

        private bool twoHanded;

        public bool isTwoHanded { get => twoHanded; }

        public string getName() { return name; }

        public Item(string _name, bool _isTwoHanded = false)
        {
            name = _name;
            twoHanded = _isTwoHanded;
        }

        public virtual int? pickUp(Player P)
        {
            return 1;
        }

        public virtual int? pickDown(Player P)
        {
            return 1;
        }

        public int modifyPlayerAttribute(PlayerAttributes A, int i) { return i; }

        public virtual (char, ConsoleColor) Render()
        {
            return ('I', ConsoleColor.Cyan);
        }

        public override string ToString()
        {
            return getName();
        }
    }

    interface IWeapon : IItem
    {
        public int getDamage();
    }

    class Weapon : Item, IWeapon
    {
        private int damage;

        public virtual int getDamage() { return damage; }

        public Weapon(string _name, int _damage = 10, bool _isTwoHanded = false) : base(_name, _isTwoHanded)
        {
            damage = _damage;
        }

        public override (char, ConsoleColor) Render() { return ('W', ConsoleColor.Blue); }

        public override string ToString()
        {
            return getName() + $" (Damage: {getDamage()})";
        }
    }


   


}
