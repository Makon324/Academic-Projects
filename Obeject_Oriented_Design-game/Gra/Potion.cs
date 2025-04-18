using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Gra
{
    /*==============*/
    /*    Potion    */
    /*==============*/

    class PotionItem : Item
    {
        private PotionEffect effect;

        public PotionItem(string _name, PotionEffect effect) : base(_name, false)
        {
            this.effect = effect;
        }

        public override (char, ConsoleColor) Render()
        {
            return ('P', ConsoleColor.Green);
        }

        public void drink(Player P)
        {
            P.addPotionEffect(effect);
            Renderer.Instance.LogAction($"Drank {getName()}");
        }

        public override string ToString()
        {
            return base.ToString() + $" ({effect.ToString()})";
        }
    }

    public interface ITurnObserver
    {
        public bool OnTurnEnded(); // Returns true if the effect should be removed
    }

    abstract class PotionEffect : ITurnObserver
    {
        protected int turnsLeft;

        public bool OnTurnEnded()
        {
            if (turnsLeft == int.MaxValue) return false;

            turnsLeft--;
            return turnsLeft <= 0;
        }

        public abstract int modifyPlayerAttribute(PlayerAttributes A, int i);
    }


    class LuckyPotionEffect : PotionEffect
    {
        private int additionalLuck;        

        public LuckyPotionEffect(int additionalLuck, int turnsActive)
        {
            this.additionalLuck = additionalLuck;
            this.turnsLeft = turnsActive;
        }

        public override int modifyPlayerAttribute(PlayerAttributes A, int i)
        {
            return A == PlayerAttributes.luck && turnsLeft >= 0
                ? i + additionalLuck
                : i;
        }

        public override string ToString()
        {
            if (turnsLeft == int.MaxValue)
                return $"Luck +{additionalLuck} for ever";
            else
                return $"Luck +{additionalLuck} for {turnsLeft} turns";
        }
    }

    class StrengthPotionEffect : PotionEffect
    {
        private int strengthBoost;

        public StrengthPotionEffect(int boostAmount, int duration)
        {
            strengthBoost = boostAmount;
            turnsLeft = duration;
        }

        public override int modifyPlayerAttribute(PlayerAttributes A, int i)
        {
            return A == PlayerAttributes.strength && turnsLeft >= 0
                ? i + strengthBoost
                : i;
        }

        public override string ToString()
        {
            if (turnsLeft == int.MaxValue)
                return $"Strength +{strengthBoost} for ever";
            else
                return $"Strength +{strengthBoost} for {turnsLeft} turns";
        }
    }

    class SpeedPotionEffect : PotionEffect
    {
        private int speedBoost;

        public SpeedPotionEffect(int boostAmount, int duration)
        {
            speedBoost = boostAmount;
            turnsLeft = duration;
        }

        public override int modifyPlayerAttribute(PlayerAttributes A, int i)
        {
            return A == PlayerAttributes.dexterity && turnsLeft >= 0
                ? i + speedBoost
                : i;
        }

        public override string ToString()
        {
            if (turnsLeft == int.MaxValue)
                return $"Speed +{speedBoost} for ever";
            else
                return $"Speed +{speedBoost} for {turnsLeft} turns";
        }
    }


}
