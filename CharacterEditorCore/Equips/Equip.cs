using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CharacterEditorCore.Equips
{
    public class Equip
    {
        public string Name { get; protected set; }

        public string TypeOfEquip { get; protected set; }
        public int Lvl { get; protected set; }
        public int RequiredStrength { get; protected set; }
        public int RequiredDexterity { get; protected set; }
        public int RequiredConstitution { get; protected set; }
        public int RequiredIntelligence { get; protected set; }

        public double BuffManaPoints { get; protected set; }
        public double BuffHealthPoints { get; protected set; }
        public double BuffAttack { get; protected set; }
        public double BuffPhysicalDefense { get; protected set; }
        public double BuffMagicAttack { get; protected set; }

        public Equip(string name, string typeOfEquip, int lvl, int strength,
            int dexterity, int constitution, int intelligence, double manaPoints,
            double healthPoints, double attack, double physDef, double magicAttack)
        {
            Name = name;
            TypeOfEquip = typeOfEquip;
            Lvl = lvl;
            RequiredStrength = strength;
            RequiredDexterity = dexterity;
            RequiredConstitution = constitution;
            RequiredIntelligence = intelligence;

            BuffManaPoints = manaPoints;
            BuffHealthPoints = healthPoints;
            BuffAttack = attack;
            BuffPhysicalDefense = physDef;
            BuffMagicAttack = magicAttack;

        }

        public string GetTypeEquip()
        {
            return TypeOfEquip;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
