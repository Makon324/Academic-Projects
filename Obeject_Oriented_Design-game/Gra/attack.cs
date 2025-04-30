using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Gra
{
    // Attack type
    enum AttackType
    {
        Normal,
        Stealth,
        Magic
    }

    // AttackVisitor
    abstract class AttackVisitor
    {
        protected IItem _weapon;

        protected AttackVisitor(IItem weapon)
        {
            _weapon = weapon;
        }

        public abstract int Visit(HeavyWeapon weapon);
        public abstract int Visit(LightWeapon weapon);
        public abstract int Visit(MagicWeapon weapon);
        public abstract int Visit(IItem nonWeapon);
    }

    class NormalAttackVisitor : AttackVisitor
    {
        public NormalAttackVisitor(IItem weapon) : base(weapon) { }

        public override int Visit(HeavyWeapon w) => ((IWeapon)_weapon).getDamage();
        public override int Visit(LightWeapon w) => ((IWeapon)_weapon).getDamage();
        public override int Visit(MagicWeapon w) => 1;
        public override int Visit(IItem item) => 0;
    }

    class StealthAttackVisitor : AttackVisitor
    {
        public StealthAttackVisitor(IItem weapon) : base(weapon) { }

        public override int Visit(HeavyWeapon w) => ((IWeapon)_weapon).getDamage() / 2;
        public override int Visit(LightWeapon w) => ((IWeapon)_weapon).getDamage() * 2;
        public override int Visit(MagicWeapon w) => 1;
        public override int Visit(IItem item) => 0;
    }

    class MagicAttackVisitor : AttackVisitor
    {
        public MagicAttackVisitor(IItem weapon) : base(weapon) { }

        public override int Visit(HeavyWeapon w) => 1;
        public override int Visit(LightWeapon w) => 1;
        public override int Visit(MagicWeapon w) => ((IWeapon)_weapon).getDamage();
        public override int Visit(IItem item) => 0;
    }


    // DefenseVisitor
    abstract class DefenseVisitor
    {
        protected Player _player;
        protected IItem _weapon;

        protected DefenseVisitor(Player player, IItem weapon)
        {
            _player = player;
            _weapon = weapon;
        }

        public abstract int Visit(HeavyWeapon weapon);
        public abstract int Visit(LightWeapon weapon);
        public abstract int Visit(MagicWeapon weapon);
        public abstract int Visit(IItem nonWeapon);
    }

    class NormalDefenseVisitor : DefenseVisitor
    {
        public NormalDefenseVisitor(Player player, IItem weapon) : base(player, weapon) { }

        public override int Visit(HeavyWeapon weapon) => _player.getAttribute(PlayerAttributes.strength) + _player.getAttribute(PlayerAttributes.luck);
        public override int Visit(LightWeapon weapon) => _player.getAttribute(PlayerAttributes.dexterity) + _player.getAttribute(PlayerAttributes.luck);
        public override int Visit(MagicWeapon weapon) => _player.getAttribute(PlayerAttributes.dexterity) + _player.getAttribute(PlayerAttributes.luck);
        public override int Visit(IItem item) => _player.getAttribute(PlayerAttributes.dexterity);
    }

    class StealthDefenseVisitor : DefenseVisitor
    {
        public StealthDefenseVisitor(Player player, IItem weapon) : base(player, weapon) { }

        public override int Visit(HeavyWeapon weapon) => _player.getAttribute(PlayerAttributes.strength);
        public override int Visit(LightWeapon weapon) => _player.getAttribute(PlayerAttributes.dexterity);
        public override int Visit(MagicWeapon weapon) => 0;
        public override int Visit(IItem item) => 0;
    }

    class MagicDefenseVisitor : DefenseVisitor
    {
        public MagicDefenseVisitor(Player player, IItem weapon) : base(player, weapon) { }

        public override int Visit(HeavyWeapon weapon) => _player.getAttribute(PlayerAttributes.luck);
        public override int Visit(LightWeapon weapon) => _player.getAttribute(PlayerAttributes.luck);
        public override int Visit(MagicWeapon weapon) => _player.getAttribute(PlayerAttributes.wisdom) * 2;
        public override int Visit(IItem item) => _player.getAttribute(PlayerAttributes.luck);
    }


    // Controler
    static class AttackController
    {
        public static void MakeAttack(Player P, Map M, AttackType type)
        {
            Enemy? enemy = M.GetEnemy(P.getX, P.getY);
            if (enemy == null)
            {
                Renderer.Instance.LogAction("No enemy here to attack.");
                return;
            }

            int totalAttack = 0;
            int totalDefense = 0;
            if(P.RightHand != null)
            {
                totalAttack += CalculateAttackDamage(type, P.RightHand);
                totalDefense += CalculateDefense(P, type, P.RightHand);
            }            
            if (P.LeftHand != null && P.LeftHand.isTwoHanded == false)
            {
                totalAttack += CalculateAttackDamage(type, P.LeftHand);
                totalDefense += CalculateDefense(P, type, P.LeftHand);
            }            

            // Apply damage to enemy
            enemy.TakeDamage(totalAttack);
            string logMessage = $"Attacked {enemy.getName()} for {totalAttack} damage.";
            if (enemy.LifePoints <= 0)
            {
                enemy.RemoveFromBoard(M);
                logMessage += $" Defeated {enemy.getName()}.";
            }
            else
            {
                // Calculate damage taken by player
                int playerDamage = Math.Max(0, enemy.AttackValue - totalDefense);
                P.setBaseAttribute(PlayerAttributes.health, P.getBaseAttribute(PlayerAttributes.health) - playerDamage);
                logMessage += $" Enemy counterattacked for {playerDamage}.";

                // Check if player died
                if (P.getBaseAttribute(PlayerAttributes.health) <= 0)
                {
                    Renderer.Instance.LogAction("You have died. Game Over!");
                    Environment.Exit(0);
                }
            }

            Renderer.Instance.LogAction(logMessage);
        }

        public static int CalculateAttackDamage(AttackType type, IItem weapon)
        {
            AttackVisitor? visitor = type switch
            {
                AttackType.Normal => new NormalAttackVisitor(weapon),
                AttackType.Stealth => new StealthAttackVisitor(weapon),
                AttackType.Magic => new MagicAttackVisitor(weapon),
                _ => throw new ArgumentException("Invalid attack type")
            };

            return weapon.Accept(visitor);
        }

        public static int CalculateDefense(Player player, AttackType type, IItem weapon)
        {
            DefenseVisitor visitor = type switch
            {
                AttackType.Normal => new NormalDefenseVisitor(player, weapon),
                AttackType.Stealth => new StealthDefenseVisitor(player, weapon),
                AttackType.Magic => new MagicDefenseVisitor(player, weapon),
                _ => throw new ArgumentException("Invalid attack type")
            };

            return weapon.AcceptDefense(visitor);
        }
    }

}
