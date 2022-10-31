using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterEditorCore.Equips
{
    public class Weapon : Equip
    {
        public Weapon(string name, int lvl, int strength,
            int dexterity, int constitution, int intelligence, double manaPoints,
            double healthPoints, double attack, double physDef, double magicAttack)
            : base(name, typeOfEquip: "Weapon", lvl, strength, dexterity, constitution,
                  intelligence, manaPoints, healthPoints, attack, physDef, magicAttack)
        {
            TypeOfEquip = "Weapon";
        }
    }
}
