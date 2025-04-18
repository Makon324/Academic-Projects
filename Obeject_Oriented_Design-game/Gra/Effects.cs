using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gra
{
    /*=======================*/
    /*    EFFECTS ON ITEMS   */
    /*=======================*/
    abstract class ItemEffect : IItem
    {
        protected IItem baseItem;

        public ItemEffect(IItem baseItem)
        {
            this.baseItem = baseItem;
        }

        public virtual bool isTwoHanded => baseItem.isTwoHanded;

        public virtual string getName() => baseItem.getName();

        public virtual int? pickUp(Player P) => baseItem.pickUp(P);

        public virtual int? pickDown(Player P) => baseItem.pickDown(P);

        public virtual int modifyPlayerAttribute(PlayerAttributes A, int i) => baseItem.modifyPlayerAttribute(A, i);

        public virtual (char, ConsoleColor) Render() => baseItem.Render();

        public override string ToString() => getName();
    }

    class EffectLuck : ItemEffect
    {
        private int additionalLuck;

        public EffectLuck(IItem baseItem, int additionalLuck) : base(baseItem)
        {
            this.additionalLuck = additionalLuck;
        }

        public override string getName() => (additionalLuck >= 0 ? "Lucky " : "Unlucky ") + baseItem.getName();

        public override int modifyPlayerAttribute(PlayerAttributes A, int i)
        {
            return A == PlayerAttributes.luck
                ? base.modifyPlayerAttribute(A, i) + additionalLuck
                : base.modifyPlayerAttribute(A, i);
        }
    }

    class EffectLuckW : EffectLuck, IWeapon
    {
        public EffectLuckW(IWeapon _baseItem, int _additionalLuck) : base(_baseItem, _additionalLuck) { }

        public int getDamage() { return ((IWeapon)baseItem).getDamage(); }

        public override string ToString()
        {
            return getName() + $" (Damage: {getDamage()})";
        }
    }

    class EffectVanish : ItemEffect
    {
        public EffectVanish(IItem baseItem) : base(baseItem) { }

        public override string getName() => "Vanish " + baseItem.getName();

        public override int? pickDown(Player P) => null;
    }

    class EffectVanishW : EffectVanish, IWeapon
    {
        public EffectVanishW(IWeapon _baseItem) : base(_baseItem) { }

        public int getDamage() { return ((IWeapon)baseItem).getDamage(); }

        public override string ToString()
        {
            return getName() + $" (Damage: {getDamage()})";
        }
    }

    /*=======================*/
    /*   EFFECTS ON WEAPONS  */
    /*=======================*/
    abstract class WeaponEffect : ItemEffect, IWeapon
    {
        protected new IWeapon baseItem;

        public WeaponEffect(IWeapon baseItem) : base(baseItem)
        {
            this.baseItem = baseItem;
        }

        public virtual int getDamage() => baseItem.getDamage();
    }

    class EffectDamage : WeaponEffect
    {
        private int additionalDamage;

        public EffectDamage(IWeapon baseWeapon, int additionalDamage) : base(baseWeapon)
        {
            this.additionalDamage = additionalDamage;
        }

        public override string getName() => (additionalDamage >= 0 ? "Strong " : "Weak ") + baseItem.getName();

        public override int getDamage() => base.getDamage() + additionalDamage;

        public override string ToString() => $"{getName()} (Damage: {getDamage()})";
    }

    class EffectHeavy : WeaponEffect
    {
        private int additionalDamage;
        private int dexterityPenalty;

        public EffectHeavy(IWeapon baseWeapon, int additionalDamage = 10, int dexterityPenalty = 20) : base(baseWeapon)
        {
            this.additionalDamage = additionalDamage;
            this.dexterityPenalty = dexterityPenalty;
        }

        public override int getDamage() => base.getDamage() + additionalDamage;

        public override int modifyPlayerAttribute(PlayerAttributes A, int i)
        {
            return A == PlayerAttributes.dexterity
                ? base.modifyPlayerAttribute(A, i) - dexterityPenalty
                : base.modifyPlayerAttribute(A, i);
        }

        public override string getName() => (additionalDamage >= 0 ? "Heavy " : "Light ") + baseItem.getName();

        public override string ToString() => $"{getName()} (Damage: {getDamage()})";
    }
}