using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gra
{
    class Enemy : Entity
    {
        string name;

        public Enemy(int _X = 0, int _Y = 0, string _name = "Enemy") : base(_X, _Y)
        {
            name = _name;
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
