using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

namespace MoskoiBoi
{
    /// <summary>
    /// Логика взаимодействия для Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        public Game()
        {
            InitializeComponent();

            Playing_Field.Context().GenerationOfShips();
            Playing_Field.Context().OtrisovkaShipsPlayer(GridPole);
            Playing_Field.Context().OtrisovkaShipsBots(Gridsopernik);
            Playing_Field.Context().GameDave(GridPole);
            Playing_Field.Context().GameDave(Gridsopernik);
            GridPole.IsEnabled = false;
        }
        public int koloc;
        private void GridPole_MouseUp(object sender, MouseButtonEventArgs e)
        {       
            System.Windows.Point pt = e.GetPosition(this);
            if (Playing_Field.Context().CheckInPeresechenie(GridPole, pt))
            {
                 Playing_Field.Context().GuningPlayer(GridPole, pt);
            }
        }

        private void GridPole_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void Gridsopernik_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Gridsopernik_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point pt = e.GetPosition(this);
            
            if (Playing_Field.Context().CheckInPeresechenie(Gridsopernik, pt))
            {
                if (Playing_Field.Context().GuningPlayer(Gridsopernik, pt))
                {
                    MessageBox.Show("Вы попали! Ходите еще раз");
                }
                else
                {
                    Playing_Field.Context().Guning(GridPole);
                }
                return;
            }
            
        }
    }
}
