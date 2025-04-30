using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gra
{
    class Enemy : Entity
    {
        private string name;
        public int LifePoints { get; private set; }
        public int AttackValue { get; }
        public int Armor { get; }

        public Enemy(int _X = 0, int _Y = 0, string _name = "Enemy", int lifePoints = 50, int attackValue = 30, int armor = 5) : base(_X, _Y)
        {
            name = _name;
            LifePoints = lifePoints;
            AttackValue = attackValue;
            Armor = armor;
        }

        public void RemoveFromBoard(Map map)
        {
            map.RemoveEnemy(getX, getY);
        }

        public void TakeDamage(int damage)
        {
            int effective = Math.Max(0, damage - Armor);
            LifePoints -= effective;
        }

        public string getName()
        {
            return name;
        }

        public override string ToString()
        {
            return getName();
        }

        public override (char, ConsoleColor) Render()
        {
            return ('E', ConsoleColor.DarkYellow);
        }
    }
}
