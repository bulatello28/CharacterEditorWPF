using CharacterEditorCore.MongoDb;
using CharacterEditorCore;
using MongoDB.Bson;
using MongoDB.Driver;
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
using System.Windows.Shapes;

namespace CharacterEditorWPF
{
    /// <summary>
    /// Логика взаимодействия для MatchWindow.xaml
    /// </summary>
    public partial class MatchWindow : Window
    {
        public List<Character> characters = new List<Character>();
        public List<ObjectId> blackTeam = new List<ObjectId>();
        public List<ObjectId> whiteTeam = new List<ObjectId>();
        public MatchWindow()
        {
            InitializeComponent();
            FillAllCharacters();
        }



        public void FillAllCharacters()
        {
            var collection = MongoDb.GetCollection();
            var filter = new BsonDocument();
            using var cursor = collection.FindSync(filter);
            {
                while (cursor.MoveNext())
                {
                    var docs = cursor.Current;
                    foreach (var doc in docs)
                    {
                        characters.Add(doc);
                    }
                }
            }
            foreach (var character in characters)
            {
                lb_allCharacters.Items.Add(character);
            }
        }

        private void lb_allCharacters_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (lb_allCharacters.SelectedItem is not null)
            {
                if (e.LeftButton == Mouse.LeftButton)
                {
                    AddCharacterInTeam(blackTeam);
                }
            }
            PrintBlackTeam();
            PrintAvgLvlOfTeam(blackTeam);
        }

        private void lb_allCharacters_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (lb_allCharacters.SelectedItem is not null)
            {
                if (e.RightButton == Mouse.RightButton)
                {
                    AddCharacterInTeam(whiteTeam);
                }
            }
            PrintWhiteTeam();
            PrintAvgLvlOfTeam(whiteTeam);
        }

        public void AddCharacterInTeam(List<ObjectId> team)
        {
            if (team.Count < 6)
            {
                Character character = (Character)lb_allCharacters.SelectedItem;
                team.Add(character._id);
                lb_allCharacters.Items.Remove(character);
            }
            else
            {
                MessageBox.Show("U can't do this");
            }
        }

        public void PrintBlackTeam()
        {
            lb_blackTeam.Items.Clear();

            foreach (var bchar in blackTeam)
            {
                lb_blackTeam.Items.Add(bchar);
            }
        }

        public void PrintWhiteTeam()
        {
            lb_whiteTeam.Items.Clear();

            foreach (var wchar in whiteTeam)
            {
                lb_whiteTeam.Items.Add(wchar);
            }
        }

        public double CalculateAvgLvl(List<ObjectId> ids)
        {
            double lvlOfTeam = characters.Where(x => ids.Contains(x._id)).Select(x => x.level.CurrentLevel).Sum(x => x);
            if (ids.Count != 0)
            {
                return Math.Round(lvlOfTeam / 6, 2);
            }
            else
            {
                return 0;
            }
            //foreach (var id in ids)
            // {
            //    //lvlOfTeam += characters.FirstOrDefault(x => x._id == id)?.level.CurrentLevel ?? 0;
            // }

        }

        private void PrintAvgLvlOfTeam(List<ObjectId> ids)
        {
            if (ids == blackTeam)
            {
                textblock_BlactAvgLvl.Text = 0.ToString();
                textblock_BlactAvgLvl.Text = CalculateAvgLvl(ids).ToString();
            }
            else
            {
                textblock_WhiteAvgLvl.Text = 0.ToString();
                textblock_WhiteAvgLvl.Text = CalculateAvgLvl(ids).ToString();
            }
        }

        private bool CheckTeamBalance()
        {
            double blackTeamAverage = CalculateAvgLvl(blackTeam);
            double whiteTeamAverage = CalculateAvgLvl(whiteTeam);

            return Math.Abs(blackTeamAverage - whiteTeamAverage) <= 2;
        }

        private void btn_StartMatch_Click(object sender, RoutedEventArgs e)
        {
            if (CheckTeamBalance() == true)
            {
                Match match = new Match();
                match.blackTeam = blackTeam;
                match.whiteTeam = whiteTeam;
                MongoDb.AddMatchToDataBase(match);

                Close();
                if (RandomWinner() == true)
                {
                    MessageBox.Show("White Team is win");
                }
                else
                {
                    MessageBox.Show("Black Team is win");   
                }


            }
            else
            {
                MessageBox.Show("Teams no balanced");
            }
        }

        public bool RandomWinner()
        {
            bool isWhiteWinner;
            Random rnd = new Random();
            int winnerNumber = rnd.Next(1, 100);
            if (winnerNumber / 4 == 0)
            {
                isWhiteWinner = true;
            }
            else
            {
                isWhiteWinner = false;
            }

            return isWhiteWinner;
        }

        private void btn_AutoGenerateTeams_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();

            if (blackTeam.Count != 0 || whiteTeam.Count != 0)
            {
                blackTeam.Clear();
                whiteTeam.Clear();
            }

            while (true)
            {
                foreach (var character in characters)
                {
                    if (rnd.Next(1, 3) == 1 && blackTeam.Count < 6)
                        blackTeam.Add(character._id);
                    else if (rnd.Next(1, 3) == 2 && whiteTeam.Count < 6)
                        whiteTeam.Add(character._id);
                }

                if (CheckTeamBalance())
                {
                    break;
                }
                else
                {
                    blackTeam.Clear();
                    whiteTeam.Clear();
                }
            }

            PrintAvgLvlOfTeam(whiteTeam);
            PrintAvgLvlOfTeam(blackTeam);
            PrintBlackTeam();
            PrintWhiteTeam();
        }
    }
}
