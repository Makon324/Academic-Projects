using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Gra
{
    /*================*/
    /*    CURRENCY    */
    /*================*/

    enum Currency
    {
        Gold,
        Coin
    }

    class CurrencyItem : Item
    {
        private Currency currencyType;
        
        private int value;

        public Currency getCurrencyType() { return currencyType; }

        public int getValue() { return value; }

        public CurrencyItem(string _name, Currency _currencyType, int _value = 1) : base(_name, false)
        {
            currencyType = _currencyType;
            value = _value;
        }

        public override int? pickUp(Player P)
        {
            P.changeCurrency(currencyType, value);
            return null;
        }

        public override int? pickDown(Player P)
        {
            P.changeCurrency(currencyType, -1*value);
            return null;
        }

        public override string ToString()
        {
            return base.ToString() + $" ({getCurrencyType()}: {getValue()})";
        }

        public override (char, ConsoleColor) Render()
        {
            return ('$', ConsoleColor.Yellow);
        }
    }
}
