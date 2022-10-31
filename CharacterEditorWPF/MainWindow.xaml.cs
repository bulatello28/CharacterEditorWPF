using CharacterEditorCore;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CharacterEditorCore.Item;
using CharacterEditorCore.MongoDb;
using MongoDB.Driver;
using CharacterEditorCore.Abilities;
using CharacterEditorCore.Equips;

namespace CharacterEditorWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Character currentCharacter;
        public bool isCharacterSelected;
        public bool isClearingData;

        public MainWindow()
        {
            InitializeComponent();
            RegisterClassMaps();
        }

        private static void RegisterClassMaps()
        {
            BsonClassMap.RegisterClassMap<Character>();
            BsonClassMap.RegisterClassMap<Wizard>();
            BsonClassMap.RegisterClassMap<Rogue>();
            BsonClassMap.RegisterClassMap<Warrior>();

            BsonClassMap.RegisterClassMap<Axe>();
            BsonClassMap.RegisterClassMap<Bow>();
            BsonClassMap.RegisterClassMap<Crossbow>();
            BsonClassMap.RegisterClassMap<Knife>();
            BsonClassMap.RegisterClassMap<Wand>();
            BsonClassMap.RegisterClassMap<Hammer>();

            BsonClassMap.RegisterClassMap<Equip>();
            BsonClassMap.RegisterClassMap<Helmet>();
            BsonClassMap.RegisterClassMap<Vest>();
            BsonClassMap.RegisterClassMap<Weapon>();
        }

        private void cb_chooseCharact_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isCharacterSelected)
            {
                return;
            }


            FillListBox();

            if (cb_chooseCharact.SelectedIndex == -1)
            {
                return;
            }


            ComboBoxItem typeItem = (ComboBoxItem)cb_chooseCharact.SelectedItem;
            string? value = typeItem.Content.ToString();

            switch (value)
            {
                case "Warrior":
                    Warrior newWarrior = new Warrior();
                    currentCharacter = newWarrior;
                    FillData(newWarrior);
                    break;

                case "Rogue":
                    Rogue newRogue = new Rogue();
                    currentCharacter = newRogue;
                    FillData(newRogue);
                    break;


                case "Wizard":
                    Wizard newWizard = new Wizard();
                    currentCharacter = newWizard;
                    FillData(newWizard);
                    break;
            }
        }

        public void FillData(Character newCharacter)
        {
            tb_strength.Text = newCharacter.Strength.ToString();
            tb_dexterity.Text = newCharacter.Dexterity.ToString();
            tb_constitution.Text = newCharacter.Constitution.ToString();
            tb_intelligence.Text = newCharacter.Intelligence.ToString();

            tb_HP.Text = newCharacter.healthPoints.ToString();
            tb_MP.Text = newCharacter.manaPoints.ToString();
            tb_attack.Text = newCharacter.attack.ToString();
            tb_magicAttack.Text = newCharacter.magicAttack.ToString();
            tb_physicalDef.Text = newCharacter.physicalDefense.ToString();

            tb_eperience.Text = newCharacter.level.CurrentExperience.ToString();
            tb_level.Text = newCharacter.level.CurrentLevel.ToString();
            tb_availablePoints.Text = newCharacter.AvailablePoint.ToString();
            tb_abilityPoints.Text = newCharacter.abilitiesPoints.ToString();

            foreach (var equip in newCharacter.equips)
            {
                tb_HP.Text = (Convert.ToDouble(tb_HP.Text) + equip.BuffHealthPoints).ToString();
                tb_MP.Text = (Convert.ToDouble(tb_MP.Text) + equip.BuffManaPoints).ToString();
                tb_attack.Text = (Convert.ToDouble(tb_attack.Text) + equip.BuffAttack).ToString();
                tb_magicAttack.Text = (Convert.ToDouble(tb_magicAttack.Text) + equip.BuffMagicAttack).ToString();
                tb_physicalDef.Text = (Convert.ToDouble(tb_physicalDef.Text) + equip.BuffPhysicalDefense).ToString();
            }

            GetInventoryToListBox();
            GetPotentialAbilities();
            GetCharactersAbilities();
            GetEquipsToLB();
        }

        private void ClearData()
        {
            isClearingData = true;

            cb_createdCharacters.SelectedIndex = -1;
            cb_createdCharacters.Items.Clear();
            cb_chooseCharact.SelectedIndex = -1;
            cb_ChooseItem.SelectedIndex = -1;
            cb_Helmet.SelectedIndex = -1;
            cb_Vest.SelectedIndex = -1;
            cb_Weapon.SelectedIndex = -1;
            lb_inventory.Items.Clear();
            lb_equipmetns.Items.Clear();

            tb_name.Text = "";

            tb_strength.Text = "0";
            tb_dexterity.Text = "0";
            tb_constitution.Text = "0";
            tb_intelligence.Text = "0";

            tb_HP.Text = "0";
            tb_MP.Text = "0";
            tb_attack.Text = "0";
            tb_magicAttack.Text = "0";
            tb_physicalDef.Text = "0";

            tb_level.Text = "0";
            tb_eperience.Text = "0";
            tb_availablePoints.Text = "0";
            tb_abilityPoints.Text = "0";

            currentCharacter = null;
            isClearingData = false;
        }

        private void GetPotentialAbilities()
        {
            cb_PotentialAbilities.Items.Clear();

            foreach(var ability in currentCharacter.potentialAbilities)
            {
                cb_PotentialAbilities.Items.Add(ability);   
            }
        }

        private void GetCharactersAbilities()
        {
            cb_CharactersAbilities.Items.Clear();

            foreach (var ability in currentCharacter.abilities)
            {
                cb_CharactersAbilities.Items.Add(ability);
            }
        }

        private void btn_increaseStrength_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckCharactOnExistment())
            {
                return;
            }
            if(currentCharacter.AvailablePoint == 0)
            {
                return;
            }
            var oldValue = currentCharacter.Strength;
            currentCharacter.Strength++;
            if (oldValue != currentCharacter.Strength)
            {
                currentCharacter.AvailablePoint--;
            }
            FillData(currentCharacter);
        }

        private void btn_increaseDexterity_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckCharactOnExistment())
            {
                return;
            }
            if (currentCharacter.AvailablePoint == 0)
            {
                return;
            }
            var oldValue = currentCharacter.Dexterity;
            currentCharacter.Dexterity++;
            if (oldValue != currentCharacter.Dexterity)
            {
                currentCharacter.AvailablePoint--;
            }
            FillData(currentCharacter);
        }

        private void btn_increaseConstitution_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckCharactOnExistment())
            {
                return;
            }
            if (currentCharacter.AvailablePoint == 0)
            {
                return;
            }
            var oldValue = currentCharacter.Constitution;
            currentCharacter.Constitution++;
            if (oldValue != currentCharacter.Constitution)
            {
                currentCharacter.AvailablePoint--;
            }
            FillData(currentCharacter);
        }

        private void btn_increaseIntelligence_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckCharactOnExistment())
            {
                return;
            }
            if (currentCharacter.AvailablePoint == 0)
            {
                return;
            }
            var oldValue = currentCharacter.Intelligence;
            currentCharacter.Intelligence++;
            if (oldValue != currentCharacter.Intelligence)
            {
                currentCharacter.AvailablePoint--;
            }
            FillData(currentCharacter);
        }
        private bool CheckCharactOnExistment()
        {
            try
            {
                var temp = currentCharacter.Intelligence;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("You have to choose type of character!");
                return false;
            }
            return true;
        }

        private void btn_decreaseStrength_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckCharactOnExistment())
            {
                return;
            }
            var oldValue = currentCharacter.Strength;
            currentCharacter.Strength--;
            if (oldValue != currentCharacter.Strength)
            {
                currentCharacter.AvailablePoint++;
            }
            FillData(currentCharacter);
        }

        private void btn_decreaseDexterity_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckCharactOnExistment())
            {
                return;
            }
            var oldValue = currentCharacter.Dexterity;
            currentCharacter.Dexterity--;
            if (oldValue != currentCharacter.Dexterity)
            {
                currentCharacter.AvailablePoint++;
            }
            FillData(currentCharacter);
        }

        private void btn_decreaseConstitution_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckCharactOnExistment())
            {
                return;
            }
            var oldValue = currentCharacter.Constitution;
            currentCharacter.Constitution--;
            if (oldValue != currentCharacter.Constitution)
            {
                currentCharacter.AvailablePoint++;
            }
            FillData(currentCharacter);
        }

        private void btn_decreaseIntelligence_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckCharactOnExistment())
            {
                return;
            }
            var oldValue = currentCharacter.Intelligence;
            currentCharacter.Intelligence--;
            if (oldValue != currentCharacter.Intelligence)
            {
                currentCharacter.AvailablePoint++;
            }
            FillData(currentCharacter);
        }

        private void button_addCharacter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentCharacter.Name = tb_name.Text;
            }
            catch
            {
                MessageBox.Show("Enter the Name of character!");
            }
            try
            {
                if (currentCharacter.Name == "")
                {
                    MessageBox.Show("You have to give name to character!");
                    return;
                }

                if (MongoDb.FindById(currentCharacter._id.ToString()) is null)
                {
                    MongoDb.AddToDataBase(currentCharacter);
                }
                else
                {
                    MongoDb.ReplaceOneInDataBase(currentCharacter);
                }
                ClearData();
                FillListBox();
            }
            catch
            {
                MessageBox.Show("You have to choose type of character!");
            }

        }

        private async void FillListBox()
        {
            if (cb_createdCharacters.Items.Count != 0)
            {
                cb_createdCharacters.Items.Clear();
            }

            var collection = MongoDb.GetCollection();
            try
            {
                var filter = new BsonDocument();
                using var cursor = collection.FindSync(filter); 
                {
                    while (cursor.MoveNext())
                    {
                        var docs = cursor.Current;
                        foreach (var doc in docs)
                        {
                            cb_createdCharacters.Items.Add(doc);
                        }
                    }
                }     
            }
            catch { }
        }

        private void tb_name_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                currentCharacter.Name = tb_name.Text;
            }
            catch { }
        }

        private void form_mainForm_Loaded(object sender, RoutedEventArgs e)
        {
            FillListBox();  
        }

        private void cb_createdCharacters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isClearingData)
            {
                return;
            }

            try
            {
                Character unit = (Character)cb_createdCharacters.SelectedItem;

                if(unit is not null)
                {
                    currentCharacter = unit;

                    FillData(currentCharacter);
                    isCharacterSelected = true;
                    cb_chooseCharact.Text = currentCharacter.typeOfCharacter;
                    tb_name.Text = currentCharacter.Name;
                    isCharacterSelected = false;
                }
            }
            catch { };
        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        private void btn_AddItem_Click(object sender, RoutedEventArgs e)
        {
            if (currentCharacter is null)
            {
                return;
            }

            if (currentCharacter.inventory.Count == currentCharacter.inventory.Capacity)
            {
                return;
            }

            if (cb_ChooseItem.SelectedIndex == -1)
            {
                return;
            }

            ComboBoxItem cbi = (ComboBoxItem)cb_ChooseItem.SelectedItem;
            string selectedText = cbi.Content.ToString();

            switch (selectedText)
            {
                case "Wand":
                    Wand wand = new Wand();
                    currentCharacter.inventory.Add(wand);
                    break;

                case "Bow":
                    Bow bow = new Bow();
                    currentCharacter.inventory.Add(bow);
                    break;

                case "Crossbow":
                    Crossbow crossbow = new Crossbow();
                    currentCharacter.inventory.Add(crossbow);
                    break;

                case "Hammer":
                    Hammer hammer = new Hammer();
                    currentCharacter.inventory.Add(hammer);
                    break;

                case "Knife":
                    Knife knife = new Knife();
                    currentCharacter.inventory.Add(knife);
                    break;

                case "Axe":
                    Axe axe = new Axe();
                    currentCharacter.inventory.Add(axe);
                    break;
            }

            GetInventoryToListBox();
        }

        private void GetInventoryToListBox()
        {
            lb_inventory.Items.Clear();

            foreach (var item in currentCharacter.inventory)
            {
                lb_inventory.Items.Add(item.Name);
            }
        }

        private void Add100Exp_Click(object sender, RoutedEventArgs e)
        {
            currentCharacter.level.CurrentExperience += 100;
            FillData(currentCharacter);
        }

        private void Add500Exp_Click(object sender, RoutedEventArgs e)
        {
            currentCharacter.level.CurrentExperience += 500;
            FillData(currentCharacter);
        }

        private void Add1000Exp_Click(object sender, RoutedEventArgs e)
        {
            currentCharacter.level.CurrentExperience += 1000;
            FillData(currentCharacter);
        }

        private void btn_AddSkills_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                if(currentCharacter is null)
                {
                    return;
                }
                if(currentCharacter.abilitiesPoints == 0)
                {
                    return;
                }

                Ability ability = (Ability)cb_PotentialAbilities.SelectedItem;
                
                if(ability is null)
                {
                    return;
                }

                currentCharacter.abilities.Add(ability);
                currentCharacter.potentialAbilities.Remove(ability);
                currentCharacter.abilitiesPoints--;

                FillData(currentCharacter);
            }
            catch { }
        }
        private bool CheckStats(Equip equip)
        {
            return equip.RequiredStrength <= currentCharacter.Strength && equip.RequiredIntelligence <= currentCharacter.Intelligence &&
            equip.RequiredConstitution <= currentCharacter.Constitution && equip.RequiredDexterity <= currentCharacter.Dexterity;
        }

        private void GetEquipsToLB()
        {
            lb_equipmetns.Items.Clear();

            foreach (var equip in currentCharacter.equips)
            {
                lb_equipmetns.Items.Add(equip.GetTypeEquip());
            }
        }

        private void btn_AddHelmet_Click(object sender, RoutedEventArgs e)
        {
            if (currentCharacter.equips.Where<Equip>(x => x.GetTypeEquip() == "Helmet").FirstOrDefault() is not null)
            {
                MessageBox.Show("Такой тип предмета уже надет");
                return;
            }

            ComboBoxItem cbi = (ComboBoxItem)cb_Helmet.SelectedItem;
            string selectedText = cbi.Content.ToString();

            switch (selectedText)
            {
                case "Default Helmet":
                    Helmet defHelmet = new Helmet("Default Helmet", 1, 6, 15, 7, 5, 5, 7, 0, 8, 0);
                    if (CheckStats(defHelmet))
                        currentCharacter.equips.Add(defHelmet);
                    else
                        MessageBox.Show("Не подходят статы");
                    break;
                case "Diamond Helmet":
                    Helmet diamHelmet = new Helmet("Diamond Helmet", 5, 15, 10, 10, 15, 20, 30, 0, 30, 0);
                    if (CheckStats(diamHelmet))
                        currentCharacter.equips.Add(diamHelmet);
                    else
                        MessageBox.Show("Не подходят статы");
                    break;
                case "Vladmirs Offering":
                    Helmet vladHelmet = new Helmet("Vladmirs Offering", 4, 10, 10, 10, 20, 40, 20, 0, 15, 0);
                    if (CheckStats(vladHelmet))
                        currentCharacter.equips.Add(vladHelmet);
                    else
                        MessageBox.Show("Не подходят статы");
                    break;
                case "Dominator":
                    Helmet dominHelmet = new Helmet("Dominator", 5, 15, 15, 10, 8, 15, 30, 0, 12, 0);
                    if (CheckStats(dominHelmet))
                        currentCharacter.equips.Add(dominHelmet);
                    else
                        MessageBox.Show("Не подходят статы");
                    break;
            }
            FillData(currentCharacter);
            GetEquipsToLB();
        }

        private void btn_AddVest_Click(object sender, RoutedEventArgs e)
        {
            if (currentCharacter.equips.Where<Equip>(x => x.GetTypeEquip() == "Vest").FirstOrDefault() is not null)
            {
                MessageBox.Show("Такой тип предмета уже надет");
                return;
            }

            ComboBoxItem cbiVest = (ComboBoxItem)cb_Vest.SelectedItem;
            string selectedText = cbiVest.Content.ToString();

            switch (selectedText)
            {
                case "Assault Cuirass":
                    Vest assaultVest = new Vest("Assault Cuirass", 4, 20, 30, 25, 15, 10, 50, 0, 40, 0);
                    if (CheckStats(assaultVest))
                        currentCharacter.equips.Add(assaultVest);
                    else
                        MessageBox.Show("Не подходят статы");
                    break;
                case "Blade Mail":
                    Vest bladeVest = new Vest("Blade Mail", 1, 10, 10, 10, 10, 15, 20, 0, 20, 0);
                    if (CheckStats(bladeVest))
                        currentCharacter.equips.Add(bladeVest);
                    else
                        MessageBox.Show("Не подходят статы");
                    break;
                case "Robe":
                    Vest orbeVest = new Vest("Robe", 2, 15, 15, 10, 25, 80, 40, 0, 25, 10);
                    if (CheckStats(orbeVest))
                        currentCharacter.equips.Add(orbeVest);
                    else
                        MessageBox.Show("Не подходят статы");
                    break;
                case "Wale of Discord":
                    Vest waleVest = new Vest("Wale of Discrod", 2, 25, 15, 15, 10, 15, 50, 0, 25, 0);
                    if (CheckStats(waleVest))
                        currentCharacter.equips.Add(waleVest);
                    else
                        MessageBox.Show("Не подходят статы");
                    break;
            }
            FillData(currentCharacter);
            GetEquipsToLB();
        }

        private void btn_AddWeapon_Click(object sender, RoutedEventArgs e)
        {
            if (currentCharacter.equips.Where<Equip>(x => x.GetTypeEquip() == "Weapon").FirstOrDefault() is not null)
            {
                MessageBox.Show("Такой тип предмета уже надет");
                return;
            }

            ComboBoxItem cbiWeapon = (ComboBoxItem)cb_Weapon.SelectedItem;
            string selectedText = cbiWeapon.Content.ToString();

            switch (selectedText)
            {
                case "Sange":
                    Weapon sangeWeapon = new Weapon("Sange", 3, 30, 15, 20, 15, 20, 40, 40, 0, 15);
                    if (CheckStats(sangeWeapon))
                        currentCharacter.equips.Add(sangeWeapon);
                    else
                        MessageBox.Show("Не подходят статы");
                    break;
                case "Yasha":
                    Weapon yashaWeapon = new Weapon("Yasha", 3, 15, 35, 15, 15, 10, 30, 35, 0, 10);
                    if (CheckStats(yashaWeapon))
                        currentCharacter.equips.Add(yashaWeapon);
                    else
                        MessageBox.Show("Не подходят статы");
                    break;
                case "Kaya":
                    Weapon kayaWeapon = new Weapon("Kaya", 3, 15, 15, 10, 30, 40, 25, 20, 0, 35);
                    if (CheckStats(kayaWeapon))
                        currentCharacter.equips.Add(kayaWeapon);
                    else
                        MessageBox.Show("Не подходят статы");
                    break;
                case "Cleymore":
                    Weapon cleymoreWeapon = new Weapon("cleymoreWeapon", 3, 10, 10, 15, 10, 15, 15, 10, 0, 10);
                    if (CheckStats(cleymoreWeapon))
                        currentCharacter.equips.Add(cleymoreWeapon);
                    else
                        MessageBox.Show("Не подходят статы");
                    break;
            }
            FillData(currentCharacter);
            GetEquipsToLB();
        }

        private void btn_CreatMatch_Click(object sender, RoutedEventArgs e)
        {
            MatchWindow match = new MatchWindow();
            match.Show();
        }
    }
}
