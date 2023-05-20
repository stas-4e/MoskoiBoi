using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Numerics;
using System.Threading;
using Accessibility;

namespace MoskoiBoi
{

    interface IPlaying
    {
        public void GameDave(Grid grid);
        public void CreateShips(Grid grid, System.Windows.Point pt, bool gorizontal, int ships);
        public bool GuningPlayer(Grid grid, System.Windows.Point pt);
        public void Guning(Grid grid);
        public void OtrisovkaShipsPlayer(Grid grid);
        public void GenerationOfShips();
        public void OtrisovkaShipsBots(Grid grid);
        public bool CheckInPeresechenie(Grid grid, System.Windows.Point pt);

    }

    class Playing_Field : IPlaying
    {
        public static IPlaying Context()
        {
            Playing_Field playing_ = new Playing_Field();
            return playing_;
        }
        public static int[,] array = new int[10, 10];
        public static int[,] arrayBot = new int[10, 10];
        public static int[,] arrayHisortBot = new int[10, 10];
        public static int[,] arrayHisortPlayer = new int[10, 10];
        public Playing_Field()
        {
        }
        public void GameDave(Grid grid)
        {
            for (byte i = 0; i <= 10; i++)

            {
                //построение горизонтальные и вертикальные линии на поле игрока

                Line y = new Line();

                y.X1 = i * 40;

                y.X2 = i * 40;

                y.Y1 = 0;

                y.Y2 = grid.Width;

                y.Stroke = Brushes.Gray;
                grid.Children.Add(y);



                Line x = new Line();

                x.X1 = 0;

                x.X2 = grid.Width;

                x.Y1 = i * 40;

                x.Y2 = i * 40;

                x.Stroke = Brushes.Gray;

                grid.Children.Add(x);



                //строит горизонтальные линии на поле компьютера

                Line y_с = new Line();

                y_с.X1 = i * 40;

                y_с.X2 = i * 40;

                y_с.Y1 = 0;

                y_с.Y2 = grid.Width;

                y_с.Stroke = Brushes.Black;

                grid.Children.Add(y_с);


                Line x_с = new Line();

                x_с.X1 = 0;

                x_с.X2 = grid.Width;

                x_с.Y1 = i * 40;

                x_с.Y2 = i * 40;

                x_с.Stroke = Brushes.Black;

                grid.Children.Add(x_с);
            }
        } // Отрисовка сетки

        private void OtrisovkaKorabl(double x, double y, Grid grid) // Отрисовка выбора корабля
        {
            System.Windows.Shapes.Rectangle cube = new System.Windows.Shapes.Rectangle();
            Thickness mrgn = new Thickness(x, y, 0, 0);
            Image img = new Image();
            img.Margin = mrgn;
            img.Source = new BitmapImage(new Uri("D.png", UriKind.Relative));
            img.Width = 38;
            img.Height = 38;
            grid.Children.Add(img);
        }
        public void OtrisovkaPopal(double x, double y, Grid grid)
        {
            Thickness mrgn = new Thickness(x, y, 0, 0);
            Image img = new Image();
            img.Margin = mrgn;
            img.Source = new BitmapImage(new Uri("krest.png", UriKind.Relative));
            img.Width = 40;
            img.Height = 40;
            grid.Children.Add(img);
        }
        public void OtrisovkaPromox(double x, double y, Grid grid) // Отрисовка промоха
        {
            Thickness mrgn = new Thickness(x, y, 0, 0);
            Image img = new Image();
            img.Margin = mrgn;
            img.Source = new BitmapImage(new Uri("tockha.png", UriKind.Relative));
            img.Width = 35;
            img.Height = 35;
            grid.Children.Add(img);
        }
        public void OtrisovkaShipsPlayer(Grid grid) // Отрисовка кораблей игрока
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (array[i, j] == 1)
                    {
                        int x_k = 0, y_k = 0;
                        x_k = -360 + i * 80;
                        y_k = -360 + j * 80;
                        OtrisovkaKorabl(x_k, y_k, grid);
                    }
                }
            }
        }

       

        public void ClearShips(double x, double y, Grid grid)
        {
            Brush color = new SolidColorBrush(Colors.Gray);

            Canvas ell = new Canvas();

            Thickness mrgn = new Thickness(x, y, 0, 0);

            ell.Margin = mrgn;

            ell.Width = 39;

            ell.Height = 39;

            ell.Background = color;
            grid.Children.Add(ell);
        }

        private bool CheckingWinPlayer()
        {
            for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                           if (arrayBot[i,j] == 1)
                           {
                                return false;
                           }
                    }
                }
            return true;
        }

        private bool CheckingWinBoot()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (array[i, j] == 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        private int game_zone(int x, int y, int a) // Заполнение массива при выборе кораблей
        {
            array[x, y] = a;
            return a;
        }


        private int game_zone_bot(int x, int y, int a = 1) // Заполнение массива для бота
        {
            arrayBot[x, y] = a;
            return a;
        }

        private bool check_massiv_bot(int x, int y, int a = 1)
        {
            if (arrayBot[x, y] == a)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool GuningPlayer(Grid grid, System.Windows.Point pt) // Ход игрока
        {
              int b = 0;
              int x = Convert.ToInt32(pt.X), y = Convert.ToInt32(pt.Y);
              int x_k = 0, y_k = 0;
              x = (x - (int)grid.Margin.Left) / 40;
              y = (y - (int)grid.Margin.Top) / 40;
              x_k = -360 + x * 80;
              y_k = -360 + y * 80;
                if (arrayBot[x, y] == 1)
                {
                    arrayBot[x, y] = 2;
                    OtrisovkaPopal(x_k, y_k, grid);
                    if (CheckingWinPlayer())
                    {
                        MessageBox.Show("Победа");
                        grid.IsEnabled = false;
                    }
                    return true;
                }
                else
                {
                    OtrisovkaPromox(x_k, y_k, grid);
                    return false;
                }
        }
        public void CreateShips(Grid grid, System.Windows.Point pt, bool gorizontal, int ships) // Создание кораблей
        {
            int x = Convert.ToInt32(pt.X), y = Convert.ToInt32(pt.Y);
            int x_k = 0, y_k = 0;
            x = (x - (int)grid.Margin.Left) / 40;
            y = (y - (int)grid.Margin.Top) / 40;
            x_k = -360 + x * 80;
            y_k = -360 + y * 80;
            if (array[x, y] != 1)
            {
                if (CheckingShips(x, y, ships, gorizontal, grid, array))
                {
                    game_zone(x, y, 1);
                    //OtrisovkaKorabl(x_k, y_k, grid);
                }
            }
            else
            {
                ClearShips(x_k, y_k, grid);
                game_zone(x, y, 0);
            }

        }
        public static int[] ships = new int[5]; 
        private bool CheckingShips(int x, int y, int paluba, bool isGorizontal, Grid grid, int[,] ArrayPlayer)
        {
            try
            {               
                switch (paluba)
                {
                    case 1:
                        if (ships[paluba] < 4)
                        {
                            bool isValid = false;
                            if (!isValid && x == 9 && y == 9 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x - 1, y - 1] != 1 && ArrayPlayer[x, y - 1] != 1)
                            {
                                isValid = true;
                            }
                            if (!isValid && x == 0 && y == 9 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y - 1] != 1 && ArrayPlayer[x, y - 1] != 1)
                            {
                                isValid = true;
                            }
                            if (!isValid && y == 0 && x == 0 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x, y + 1] != 1)
                            {
                                isValid = true;
                            }
                            if (!isValid && y == 0 && x == 9 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x - 1, y] != 1 &&
                                ArrayPlayer[x - 1, y + 1] != 1)
                            {
                                isValid = true;
                            }
                            if (!isValid && y == 9 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x + 1, y - 1] != 1 && ArrayPlayer[x - 1, y - 1] != 1 && ArrayPlayer[x, y - 1] != 1)
                            {
                                isValid = true;
                            }
                            if (!isValid && x == 9 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x - 1, y + 1] != 1 && ArrayPlayer[x - 1, y - 1] != 1 && ArrayPlayer[x, y - 1] != 1)
                            {
                                isValid = true;
                            }
                            if (!isValid && x == 0 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x + 1, y - 1] != 1 && ArrayPlayer[x, y - 1] != 1)
                            {
                                isValid = true;
                            }

                            if (!isValid && y == 0 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x - 1, y] != 1 &&
                                ArrayPlayer[x - 1, y + 1] != 1)
                            {
                                isValid = true;
                            }
                            if (!isValid && ArrayPlayer[x, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x - 1, y + 1] != 1 && ArrayPlayer[x + 1, y - 1] != 1 && ArrayPlayer[x - 1, y - 1] != 1 && ArrayPlayer[x, y - 1] != 1)
                            {
                                isValid = true;
                            }
                            if (isValid == true)
                            {
                                game_zone(x, y, 1);
                                int x_k = -360 + x * 80;
                                int y_k = -360 + y * 80;
                                OtrisovkaKorabl(x_k, y_k, grid);
                                ships[paluba]++;
                            }
                        }

                        break;
                    #region ДвухПалубные кораблики
                    case 2:
                        if (isGorizontal)
                        {
                            bool isValid = false;
                            if (ships[paluba] != 3)
                            {
                                if (x != 9)
                                {
                                    if (!isValid && y == 9 && x == 0 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x, y - 1] != 1 && ArrayPlayer[x + 2, y] != 1)
                                    {
                                        isValid = true;

                                    }
                                    if (y == 0 && x == 0 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 2, y] != 1 && ArrayPlayer[x + 1, y + 1] != 1)
                                    {
                                        isValid = true;

                                    }
                                    if (!isValid && y == 9 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x, y - 1] != 1 && ArrayPlayer[x + 1, y - 1] != 1 && ArrayPlayer[x - 1, y - 1] != 1)
                                    {
                                        isValid = true;

                                    }
                                    if (!isValid && y == 0 && x == 8 && ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x + 1, y + 1] != 1)
                                    {
                                        isValid = true;
                                    }
                                    if (!isValid && x == 8 && ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x, y - 1] != 1)
                                    {
                                        isValid = true;

                                    }
                                    if (!isValid && y == 0 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 2, y] != 1 && ArrayPlayer[x, y + 1] != 1)
                                    {
                                        isValid = true;
                                    }
                                    if (!isValid && x == 0 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x, y - 1] != 1)
                                    {
                                        isValid = true;
                                    }
                                    if (!isValid && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x - 1, y + 1] != 1 && ArrayPlayer[x, y + 1] != 1 
                                        && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x, y - 1] != 1 && ArrayPlayer[x + 1, y - 1] != 1 && ArrayPlayer[x - 1, y - 1] != 1 && ArrayPlayer[x + 2, y] != 1 && ArrayPlayer[x + 2, y + 1] != 1 && ArrayPlayer[x + 2, y - 1] != 1)
                                    {
                                        isValid = true;
                                    }
                                    if (isValid == true)
                                    {
                                        int y_k = -360 + y * 80;
                                        int x_k = -360 + x * 80;
                                        game_zone(x, y, 1);
                                        OtrisovkaKorabl(x_k, y_k, grid);
                                        x++;
                                        x_k = -360 + x * 80;
                                        game_zone(x, y, 1);
                                        OtrisovkaKorabl(x_k, y_k, grid);
                                        ships[paluba]++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (y != 9 && ships[paluba] != 3)
                            {
                                bool isValid = false;
                                if (!isValid && y == 8 && x == 0 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x + 1, y - 1] != 1 && ArrayPlayer[x, y - 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && y == 8 && x == 9 && ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x - 1, y + 1] != 1 && ArrayPlayer[x - 1, y - 1] != 1 && ArrayPlayer[x, y - 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && y == 8 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x - 1, y + 1] != 1 && ArrayPlayer[x + 1, y - 1] != 1 && ArrayPlayer[x - 1, y - 1] != 1 && ArrayPlayer[x, y - 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 9 && y == 0 && ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x - 1, y + 1] != 1 && ArrayPlayer[x, y + 2] != 1 && ArrayPlayer[x - 1, y + 2] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 0 && y == 0 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x, y + 2] != 1 && ArrayPlayer[x + 1, y + 2] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && y == 0 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x - 1, y + 1] != 1 && ArrayPlayer[x, y + 2] != 1 && ArrayPlayer[x + 1, y + 2] != 1 && ArrayPlayer[x - 1, y + 2] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 0 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x + 1, y - 1] != 1 && ArrayPlayer[x, y - 1] != 1 && ArrayPlayer[x, y + 2] != 1 && ArrayPlayer[x + 1, y + 2] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 9 && ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x - 1, y + 1] != 1 && ArrayPlayer[x - 1, y - 1] != 1 && ArrayPlayer[x, y - 1] != 1 && ArrayPlayer[x, y + 2] != 1 && ArrayPlayer[x - 1, y + 2] != 1)
                                {
                                    isValid = true;
                                }

                                if (!isValid && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x - 1, y + 1] != 1 && ArrayPlayer[x + 1, y - 1] != 1 && ArrayPlayer[x - 1, y - 1] != 1 && ArrayPlayer[x, y - 1] != 1
                                    && ArrayPlayer[x, y +2] !=1 && ArrayPlayer[x+1,y+2] !=1 && ArrayPlayer[x-1,y+2] !=1 && ArrayPlayer[x,y+2] !=1)
                                {
                                    isValid = true;
                                }

                                if (isValid == true)
                                {
                                    int x_k = -360 + x * 80;
                                    int y_k = -360 + y * 80;
                                    game_zone(x, y, 1);
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    y++;
                                    y_k = -360 + y * 80;
                                    game_zone(x, y, 1);
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    ships[paluba]++;
                                }
                            }

                        }
                        #endregion
                        break;
                    case 3:
                        if (isGorizontal)
                        {
                            bool isValid = false;
                            if (x != 8 && x != 9 && ships[paluba] != 2)
                            {
                                if (!isValid && y == 0 && x == 0 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y + 1] != 1 &&
                                    ArrayPlayer[x + 2, y] != 1 && ArrayPlayer[x + 2, y + 1] != 1 && ArrayPlayer[x + 3, y] != 1 && ArrayPlayer[x + 3, y + 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 7 && y == 9 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x - 1, y - 1] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y - 1] != 1 &&
                                    ArrayPlayer[x + 2, y] != 1 && ArrayPlayer[x + 2, y - 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 0 && y == 9 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y - 1] != 1 &&
                                    ArrayPlayer[x + 2, y] != 1 && ArrayPlayer[x + 2, y - 1] != 1 && ArrayPlayer[x + 3, y] != 1 && ArrayPlayer[x + 3, y - 1] != 1)
                                if (!isValid && x == 7 && y == 0 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x - 1, y + 1] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y + 1] != 1 &&
                                    ArrayPlayer[x + 2, y] != 1 && ArrayPlayer[x + 2, y + 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x== 7 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x - 1, y - 1] != 1 && ArrayPlayer[x - 1, y + 1] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x + 1, y - 1] != 1 &&
                                    ArrayPlayer[x + 2, y] != 1 && ArrayPlayer[x + 2, y - 1] != 1 && ArrayPlayer[x + 2, y + 1] != 1 && arrayBot[x, y - 1] != 1 && arrayBot[x, y + 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && y == 0 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x - 1, y + 1] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y + 1] != 1 &&
                                    ArrayPlayer[x + 2, y] != 1 && ArrayPlayer[x + 2, y + 1] != 1 && ArrayPlayer[x + 3, y] != 1 && ArrayPlayer[x + 3, y + 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 0 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x + 1, y - 1] != 1 &&
                                    ArrayPlayer[x + 2, y] != 1 && ArrayPlayer[x + 2, y - 1] != 1 && ArrayPlayer[x + 2, y + 1] != 1 && ArrayPlayer[x + 3, y] != 1 && ArrayPlayer[x + 3, y - 1] != 1 && ArrayPlayer[x + 3, y + 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && y == 9 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x - 1, y - 1] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y - 1] != 1 &&
                                    ArrayPlayer[x + 2, y] != 1 && ArrayPlayer[x + 2, y - 1] != 1 && ArrayPlayer[x + 3, y] != 1 && ArrayPlayer[x + 3, y - 1] != 1)
                                {
                                    isValid = true;
                                }

                                
                                if (!isValid && ArrayPlayer[x,y] !=1 && ArrayPlayer[x-1,y] !=1 && ArrayPlayer[x-1,y-1] !=1 && ArrayPlayer[x-1,y+1] !=1 && ArrayPlayer[x+1,y] !=1 && ArrayPlayer[x+1,y+1] !=1 && ArrayPlayer[x+1,y-1] !=1 &&
                                    ArrayPlayer[x+2,y] !=1 && ArrayPlayer[x+2,y-1] !=1 && ArrayPlayer[x+2,y+1] !=1 && ArrayPlayer[x+3,y] !=1 && ArrayPlayer[x+3,y-1] !=1 && ArrayPlayer[x+3,y+1] !=1 && ArrayPlayer[x, y-1] != 1 && ArrayPlayer[x,y+1] != 1)
                                {
                                    isValid = true;
                                }
                                if (isValid == true)
                                {
                                    int y_k = -360 + y * 80;
                                    int x_k = -360 + x * 80;
                                    game_zone(x, y, 1);
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    x++;
                                    x_k = -360 + x * 80;
                                    game_zone(x, y, 1);
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    x++;
                                    game_zone(x, y, 1);
                                    x_k = -360 + x * 80;
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    ships[paluba]++;
                                }
                            }
                        }
                        else
                        {
                            if (y != 9 && y != 8 && ships[paluba] != 2)
                            {
                                bool isValid = false;
                                if (!isValid && x == 0 && y == 7 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x, y - 1] != 1 && ArrayPlayer[x + 1, y - 1] != 1 && ArrayPlayer[x, y + 2] != 1 && ArrayPlayer[x + 1, y + 2] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 9 && y == 7  && ArrayPlayer[x, y] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x, y - 1] != 1 && ArrayPlayer[x - 1, y - 1] != 1 &&
                                     ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x - 1, y + 1] != 1 && ArrayPlayer[x, y + 2] != 1 && ArrayPlayer[x - 1, y + 2] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && y == 0 && x == 9 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x - 1, y] != 1 &&
                                    ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x - 1, y + 1] != 1 && ArrayPlayer[x, y + 2] != 1 && ArrayPlayer[x - 1, y + 2] != 1 &&
                                    ArrayPlayer[x, y + 3] != 1 && ArrayPlayer[x - 1, y + 3] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 0 && y == 0 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x + 1, y] != 1 &&
                                    ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x, y + 2] != 1 && ArrayPlayer[x + 1, y + 2] != 1 &&
                                    ArrayPlayer[x, y + 3] != 1 && ArrayPlayer[x + 1, y + 3] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && y == 0 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x - 1, y] != 1 &&
                                    ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x - 1, y + 1] != 1 && ArrayPlayer[x, y + 2] != 1 && ArrayPlayer[x + 1, y + 2] != 1 && ArrayPlayer[x - 1, y + 2] != 1 &&
                                    ArrayPlayer[x, y + 3] != 1 && ArrayPlayer[x + 1, y + 3] != 1 && ArrayPlayer[x - 1, y + 3] != 1)
                                {
                                    isValid = true;
                                }
                                
                                
                                if (!isValid && x == 9 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x, y - 1] != 1 &&
                                    ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x, y + 2] != 1 && ArrayPlayer[x, y + 3] != 1)
                                {
                                    isValid = true;
                                }

                                if (!isValid && x == 0 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x, y - 1] != 1 && ArrayPlayer[x + 1, y - 1] != 1 &&
                                    ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x, y + 2] != 1 && ArrayPlayer[x + 1, y + 2] != 1 &&
                                    ArrayPlayer[x, y + 3] != 1 && ArrayPlayer[x + 1, y + 3] != 1)
                                {
                                    isValid = true;
                                }

                                if (!isValid && y == 7 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x, y - 1] != 1 && ArrayPlayer[x + 1, y - 1] != 1 && ArrayPlayer[x - 1, y - 1] != 1 &&
                                    ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x - 1, y + 1] != 1 && ArrayPlayer[x, y + 2] != 1 && ArrayPlayer[x + 1, y + 2] != 1 && ArrayPlayer[x - 1, y + 2] != 1)
                                {
                                    isValid = true;
                                }

                                if (!isValid && ArrayPlayer[x,y] != 1 && ArrayPlayer[x+1,y] !=1 && ArrayPlayer[x-1,y] !=1 && ArrayPlayer[x,y-1] !=1 && ArrayPlayer[x+1,y-1] != 1 && ArrayPlayer[x-1,y-1]!=1 &&
                                    ArrayPlayer[x+1,y+1]!=1 && ArrayPlayer[x,y+1]!=1 && ArrayPlayer[x-1,y+1]!=1 && ArrayPlayer[x,y+2]!=1 && ArrayPlayer[x+1,y+2]!=1 && ArrayPlayer[x-1,y+2]!=1 &&
                                    ArrayPlayer[x,y+3]!=1 && ArrayPlayer[x+1,y+3]!=1 && ArrayPlayer[x-1,y+3] !=1)
                                {
                                    isValid = true;
                                }

                                if (isValid == true)
                                {
                                    int x_k = -360 + x * 80;
                                    int y_k = -360 + y * 80;
                                    game_zone(x, y, 1);
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    y++;
                                    y_k = -360 + y * 80;
                                    game_zone(x, y, 1);
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    y++;
                                    y_k = -360 + y * 80;
                                    game_zone(x, y, 1);
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    ships[paluba]++;
                                }
                            }
                        }
                        break;
                    case 4:
                        if (isGorizontal)
                        {
                            bool isValid = false;
                            if (x != 7 && x != 8 && x != 9 && ships[paluba] != 1)
                            {
                                if (!isValid && y == 0 && x==0 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y + 1] != 1 &&
                                    ArrayPlayer[x + 2, y] != 1 && ArrayPlayer[x + 2, y + 1] != 1 && ArrayPlayer[x + 3, y] != 1 && ArrayPlayer[x + 3, y + 1] != 1 && ArrayPlayer[x + 4, y] != 1 && ArrayPlayer[x + 4, y + 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && y == 0 && x == 6 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y + 1] != 1 &&
                                    ArrayPlayer[x + 2, y] != 1 && ArrayPlayer[x + 2, y + 1] != 1 && ArrayPlayer[x + 3, y] != 1 && ArrayPlayer[x + 3, y + 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && y == 9 && x == 6 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x, y - 1] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x - 1, y - 1] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y - 1] != 1 &&
                                    ArrayPlayer[x + 2, y] != 1 && ArrayPlayer[x + 2, y - 1] != 1 && ArrayPlayer[x + 3, y] != 1 && ArrayPlayer[x + 3, y - 1] != 1)
                                {
                                    isValid = true;
                                }
                                
                                if (!isValid && x == 6 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x, y - 1] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x - 1, y - 1] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x + 1, y - 1] != 1 &&
                                    ArrayPlayer[x + 2, y] != 1 && ArrayPlayer[x + 2, y - 1] != 1 && ArrayPlayer[x + 2, y + 1] != 1 && ArrayPlayer[x + 3, y] != 1 && ArrayPlayer[x + 3, y + 1] != 1 && ArrayPlayer[x + 3, y - 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && y == 9 && x == 0 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x, y - 1] != 1 &&
                                    ArrayPlayer[x + 2, y] != 1 && ArrayPlayer[x + 2, y - 1] != 1 && ArrayPlayer[x + 3, y] != 1 && ArrayPlayer[x + 3, y - 1] != 1 && ArrayPlayer[x + 4, y] != 1 &&
                                    ArrayPlayer[x + 4, y - 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 0 && !isValid && ArrayPlayer[x, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y + 1] != 1 &&
                                    ArrayPlayer[x + 2, y] != 1 && ArrayPlayer[x + 2, y - 1] != 1 && ArrayPlayer[x + 2, y + 1] != 1 && ArrayPlayer[x + 3, y] != 1 && ArrayPlayer[x + 3, y + 1] != 1 && ArrayPlayer[x + 3, y - 1] != 1 && ArrayPlayer[x + 4, y] != 1 &&
                                    ArrayPlayer[x + 4, y - 1] != 1 && ArrayPlayer[x + 4, y + 1] != 1)
                                {
                                    isValid = true;
                                }     
                                
                                if (!isValid && y == 9 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x, y - 1] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x - 1, y - 1] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y - 1] != 1 &&
                                    ArrayPlayer[x + 2, y] != 1 && ArrayPlayer[x + 2, y - 1] != 1 && ArrayPlayer[x + 3, y] != 1 && ArrayPlayer[x + 3, y - 1] != 1 && ArrayPlayer[x + 4, y] != 1 &&
                                    ArrayPlayer[x + 4, y - 1] != 1)
                                {
                                    isValid = true;
                                }

                                if (!isValid && y == 0 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x + 1, y + 1] != 1 &&
                                    ArrayPlayer[x + 2, y] != 1 && ArrayPlayer[x + 2, y + 1] != 1 && ArrayPlayer[x + 3, y] != 1 && ArrayPlayer[x + 3, y + 1] != 1 && ArrayPlayer[x + 4, y] != 1 && ArrayPlayer[x + 4, y + 1] != 1)
                                {
                                    isValid = true;
                                }

                                if (!isValid && ArrayPlayer[x, y] != 1 && ArrayPlayer[x, y - 1] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x - 1, y - 1] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x+1, y +1] != 1 && ArrayPlayer[x+1, y -1] != 1 &&
                                    ArrayPlayer[x+2,y] !=1 && ArrayPlayer[x+2, y -1] !=1 && ArrayPlayer[x+2, y+1] != 1 && ArrayPlayer[x+3,y] !=1 && ArrayPlayer[x+3,y+1] !=1 && ArrayPlayer[x+3,y-1] !=1 && ArrayPlayer[x+4,y] !=1 &&
                                    ArrayPlayer[x+4, y-1] !=1 && ArrayPlayer[x+4,y+1] !=1)
                                {
                                    isValid = true;
                                }

                                if (isValid == true)
                                {
                                    int y_k = -360 + y * 80;
                                    int x_k = -360 + x * 80;
                                    game_zone(x, y, 1);
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    x++;
                                    x_k = -360 + x * 80;
                                    game_zone(x, y, 1);
                                   // MessageBox.Show($"x = {x.ToString()}\n y = {x.ToString()}");
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    x++;
                                    game_zone(x, y, 1);
                                    x_k = -360 + x * 80;
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    x++;
                                    game_zone(x, y, 1);
                                    x_k = -360 + x * 80;
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    ships[paluba]++;
                                }
                            }    
                        }
                        else
                        {
                            if (y != 9 && y != 8 && y != 7 && ships[paluba] != 1)
                            {
                                bool isValid = false;
                                if (!isValid && y == 0 && x == 0 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x + 1, y] != 1 &&
                                    ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x, y + 2] != 1 && ArrayPlayer[x + 1, y + 2] != 1 &&
                                    ArrayPlayer[x, y + 3] != 1 && ArrayPlayer[x + 1, y + 3] != 1 && ArrayPlayer[x, y + 4] != 1 && ArrayPlayer[x + 1, y + 4] != 1)
                                {
                                    isValid = true;
                                } 
                                if (!isValid && x == 9 && y == 0 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x - 1, y] != 1 &&
                                    ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x - 1, y + 1] != 1 && ArrayPlayer[x, y + 2] != 1 && ArrayPlayer[x - 1, y + 2] != 1 &&
                                    ArrayPlayer[x, y + 3] != 1 && ArrayPlayer[x - 1, y + 3] != 1 && ArrayPlayer[x, y + 4] != 1 && ArrayPlayer[x - 1, y + 4] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 9 && y == 6 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x, y - 1] != 1 && ArrayPlayer[x - 1, y - 1] != 1 &&
                                    ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x - 1, y + 1] != 1 && ArrayPlayer[x, y + 2] != 1 && ArrayPlayer[x - 1, y + 2] != 1 &&
                                    ArrayPlayer[x, y + 3] != 1 && ArrayPlayer[x - 1, y + 3] != 1)
                                {
                                    isValid = true;
                                } 

                                if (!isValid && x == 0 && y == 6 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x, y - 1] != 1 && ArrayPlayer[x + 1, y - 1] != 1 &&
                                    ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x, y + 2] != 1 && ArrayPlayer[x + 1, y + 2] != 1 &&
                                    ArrayPlayer[x, y + 3] != 1 && ArrayPlayer[x + 1, y + 3] != 1)
                                {
                                    isValid = true;
                                }

                                if (!isValid && y == 0 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x - 1, y] != 1 &&
                                    ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x - 1, y + 1] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x, y + 2] != 1 && ArrayPlayer[x + 1, y + 2] != 1 && ArrayPlayer[x - 1, y + 2] != 1 &&
                                    ArrayPlayer[x, y + 3] != 1 && ArrayPlayer[x + 1, y + 3] != 1 && ArrayPlayer[x - 1, y + 3] != 1 && ArrayPlayer[x, y + 4] != 1 && ArrayPlayer[x + 1, y + 4] != 1 && ArrayPlayer[x - 1, y + 4] != 1)
                                {
                                    isValid = true;
                                }

                                if (!isValid && x == 0 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x, y - 1] != 1 && ArrayPlayer[x + 1, y - 1] != 1 &&
                                    ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x, y + 2] != 1 && ArrayPlayer[x + 1, y + 2] != 1 &&
                                    ArrayPlayer[x, y + 3] != 1 && ArrayPlayer[x + 1, y + 3] != 1 && ArrayPlayer[x, y + 4] != 1 && ArrayPlayer[x + 1, y + 4] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 9 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x, y - 1] != 1 && ArrayPlayer[x - 1, y - 1] != 1 &&
                                    ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x - 1, y + 1] != 1 && ArrayPlayer[x, y + 2] != 1  && ArrayPlayer[x - 1, y + 2] != 1 &&
                                    ArrayPlayer[x, y + 3] != 1 && ArrayPlayer[x - 1, y + 3] != 1 && ArrayPlayer[x, y + 4] != 1 && ArrayPlayer[x - 1, y + 4] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && y == 6 && ArrayPlayer[x, y] != 1 && ArrayPlayer[x + 1, y] != 1 && ArrayPlayer[x - 1, y] != 1 && ArrayPlayer[x, y - 1] != 1 && ArrayPlayer[x + 1, y - 1] != 1 && ArrayPlayer[x - 1, y - 1] != 1 &&
                                    ArrayPlayer[x, y + 1] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x - 1, y + 1] != 1 && ArrayPlayer[x + 1, y + 1] != 1 && ArrayPlayer[x, y + 2] != 1 && ArrayPlayer[x + 1, y + 2] != 1 && ArrayPlayer[x - 1, y + 2] != 1 &&
                                    ArrayPlayer[x, y + 3] != 1 && ArrayPlayer[x + 1, y + 3] != 1 && ArrayPlayer[x - 1, y + 3] != 1)
                                {
                                    isValid = true;
                                }


                                if (!isValid && ArrayPlayer[x,y]!=1 && ArrayPlayer[x+1,y] != 1 && ArrayPlayer[x-1,y] != 1 && ArrayPlayer[x,y-1]!=1 && ArrayPlayer[x+1,y-1]!=1 && ArrayPlayer[x-1,y-1]!=1 &&
                                    ArrayPlayer[x,y+1]!=1 && ArrayPlayer[x+1,y+1]!=1 && ArrayPlayer[x-1,y+1]!=1 && ArrayPlayer[x+1,y+1]!=1 && ArrayPlayer[x,y+2]!=1 && ArrayPlayer[x+1,y+2]!=1 && ArrayPlayer[x-1,y+2]!=1&&
                                    ArrayPlayer[x,y+3]!=1 && ArrayPlayer[x+1,y+3]!=1 && ArrayPlayer[x-1,y+3]!=1 && ArrayPlayer[x,y+4]!=1 && ArrayPlayer[x+1,y+4]!=1 && ArrayPlayer[x-1,y+4]!=1)
                                {
                                    isValid = true;
                                }

                                if (isValid == true)
                                {
                                    int x_k = -360 + x * 80;
                                    int y_k = -360 + y * 80;
                                    game_zone(x, y, 1);
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    y++;
                                    y_k = -360 + y * 80;
                                    game_zone(x, y, 1);
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    y++;
                                    y_k = -360 + y * 80;
                                    game_zone(x, y, 1);
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    y++;
                                    y_k = -360 + y * 80;
                                    game_zone(x, y, 1);
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    ships[paluba]++;
                                }
                            }
                        }
                        break;
                }
                return false;
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
                return false;
            }
        }
        public static int[] shipsBot = new int[5];
        private bool CheckingShipsBot(int x, int y, int paluba, bool isGorizontal, Grid grid)
        {
            try
            {
                //MessageBox.Show("x = " + x.ToString() + " y = " + y.ToString());
                switch (paluba)
                {
                    case 1:
                        if (shipsBot[paluba] < 4)
                        {
                            bool isValid = false;
                            if (!isValid && x == 9 && y == 9 && arrayBot[x, y] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x - 1, y - 1] != 1 && arrayBot[x, y - 1] != 1)
                            {
                                isValid = true;
                            }
                            if (!isValid && x == 0 && y == 9 && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y - 1] != 1 && arrayBot[x, y - 1] != 1)
                            {
                                isValid = true;
                            }
                            if (!isValid && y == 0 && x == 0 && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x, y + 1] != 1)
                            {
                                isValid = true;
                            }
                            if (!isValid && y == 0 && x == 9 && arrayBot[x, y] != 1 && arrayBot[x, y + 1] != 1 && arrayBot[x - 1, y] != 1 &&
                                arrayBot[x - 1, y + 1] != 1)
                            {
                                isValid = true;
                            }
                            if (!isValid && y == 9 && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x + 1, y - 1] != 1 && arrayBot[x - 1, y - 1] != 1 && arrayBot[x, y - 1] != 1)
                            {
                                isValid = true;
                            }
                            if (!isValid && x == 9 && arrayBot[x, y] != 1 && arrayBot[x, y + 1] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x - 1, y - 1] != 1 && arrayBot[x, y - 1] != 1)
                            {
                                isValid = true;
                            }
                            if (!isValid && x == 0 && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x, y + 1] != 1 && arrayBot[x + 1, y - 1] != 1 && arrayBot[x, y - 1] != 1)
                            {
                                isValid = true;
                            }

                            if (!isValid && y == 0 && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x, y + 1] != 1 && arrayBot[x - 1, y] != 1 &&
                                arrayBot[x - 1, y + 1] != 1)
                            {
                                isValid = true;
                            }
                            if (!isValid && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x, y + 1] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x + 1, y - 1] != 1 && arrayBot[x - 1, y - 1] != 1 && arrayBot[x, y - 1] != 1)
                            {
                                isValid = true;
                            }
                            if (isValid == true)
                            {
                                game_zone_bot(x, y, 1);
                                int x_k = -360 + x * 80;
                                int y_k = -360 + y * 80;
                               // OtrisovkaKorabl(x_k, y_k, grid);
                                shipsBot[paluba]++;
                                return true;
                            }
                        }

                        break;
                    #region ДвухПалубные кораблики
                    case 2:
                        if (isGorizontal)
                        {
                            bool isValid = false;
                            if (shipsBot[paluba] != 3)
                            {
                                if (x != 9)
                                {
                                    // Что то работает, но не совсем
                                    if (!isValid && y == 9 && x == 0 && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x, y - 1] != 1 && arrayBot[x + 2, y] != 1)
                                    {
                                        isValid = true;

                                    }
                                    if (y == 0 && x == 0 && arrayBot[x + 1, y] != 1 && arrayBot[x + 2, y] != 1 && arrayBot[x + 1, y + 1] != 1)
                                    {
                                        isValid = true;

                                    }
                                    if (!isValid && y == 9 && arrayBot[x + 1, y] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x, y - 1] != 1 && arrayBot[x + 1, y - 1] != 1 && arrayBot[x - 1, y - 1] != 1)
                                    {
                                        isValid = true;

                                    }
                                    if (!isValid && y == 0 && x == 8 && arrayBot[x, y + 1] != 1 && arrayBot[x + 1, y + 1] != 1)
                                    {
                                        isValid = true;
                                    }
                                    if (!isValid && x == 8 && arrayBot[x, y + 1] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x, y - 1] != 1)
                                    {
                                        isValid = true;

                                    }
                                    if (!isValid && y == 0 && arrayBot[x - 1, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x + 2, y] != 1 && arrayBot[x, y + 1] != 1)
                                    {
                                        isValid = true;
                                    }
                                    if (!isValid && x == 0 && arrayBot[x + 1, y] != 1 && arrayBot[x, y + 1] != 1 && arrayBot[x, y - 1] != 1)
                                    {
                                        isValid = true;
                                    }
                                    if (!isValid && arrayBot[x - 1, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x, y + 1] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x, y - 1] != 1 && arrayBot[x + 1, y - 1] != 1 && arrayBot[x - 1, y - 1] != 1 && arrayBot[x + 2, y] != 1 && arrayBot[x + 2, y + 1] != 1 && arrayBot[x + 2, y - 1] != 1)
                                    {
                                        isValid = true;
                                    }
                                    if (isValid == true)
                                    {
                                        int y_k = -360 + y * 80;
                                        int x_k = -360 + x * 80;
                                        game_zone_bot(x, y, 1);
                                       // OtrisovkaKorabl(x_k, y_k, grid);
                                        x++;
                                        x_k = -360 + x * 80;
                                        game_zone_bot(x, y, 1);
                                        //OtrisovkaKorabl(x_k, y_k, grid);
                                        shipsBot[paluba]++;
                                        return true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (y != 9 && shipsBot[paluba] != 3)
                            {
                                bool isValid = false;
                                if (!isValid && y == 8 && x == 0 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x, y + 1] != 1 && arrayBot[x + 1, y - 1] != 1 && arrayBot[x, y - 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && y == 8 && x == 9 && arrayBot[x, y + 1] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x - 1, y - 1] != 1 && arrayBot[x, y - 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && y == 8 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x, y + 1] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x + 1, y - 1] != 1 && arrayBot[x - 1, y - 1] != 1 && arrayBot[x, y - 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 9 && y == 0 && arrayBot[x, y + 1] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x - 1, y + 2] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 0 && y == 0 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x, y + 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x + 1, y + 2] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && y == 0 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x, y + 1] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x + 1, y + 2] != 1 && arrayBot[x - 1, y + 2] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 0 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x, y + 1] != 1 && arrayBot[x + 1, y - 1] != 1 && arrayBot[x, y - 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x + 1, y + 2] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 9 && arrayBot[x, y + 1] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x - 1, y - 1] != 1 && arrayBot[x, y - 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x - 1, y + 2] != 1)
                                {
                                    isValid = true;
                                }

                                if (!isValid && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x, y + 1] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x + 1, y - 1] != 1 && arrayBot[x - 1, y - 1] != 1 && arrayBot[x, y - 1] != 1
                                    && arrayBot[x, y + 2] != 1 && arrayBot[x + 1, y + 2] != 1 && arrayBot[x - 1, y + 2] != 1 && arrayBot[x, y + 2] != 1)
                                {
                                    isValid = true;
                                }
                                if (isValid == true)
                                {
                                    int x_k = -360 + x * 80;
                                    int y_k = -360 + y * 80;
                                    game_zone_bot(x, y, 1);
                                    //OtrisovkaKorabl(x_k, y_k, grid);
                                    y++;
                                    y_k = -360 + y * 80;
                                    game_zone_bot(x, y, 1);
                                    //OtrisovkaKorabl(x_k, y_k, grid);
                                    shipsBot[paluba]++;
                                    return true;
                                }
                            }
                            }
                        #endregion
                        break;
                    case 3:
                        if (isGorizontal && shipsBot[paluba] != 2)
                        {
                            bool isValid = false;
                            if (x != 8 && x != 9)
                            {
                                if (!isValid && y == 0 && x == 0 && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y + 1] != 1 &&
                                    arrayBot[x + 2, y] != 1 && arrayBot[x + 2, y + 1] != 1 && arrayBot[x + 3, y] != 1 && arrayBot[x + 3, y + 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 7 && y == 9 && arrayBot[x, y] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x - 1, y - 1] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y - 1] != 1 &&
                                    arrayBot[x + 2, y] != 1 && arrayBot[x + 2, y - 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 0 && y == 9 && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y - 1] != 1 &&
                                    arrayBot[x + 2, y] != 1 && arrayBot[x + 2, y - 1] != 1 && arrayBot[x + 3, y] != 1 && arrayBot[x + 3, y - 1] != 1)
                                    if (!isValid && x == 7 && y == 0 && arrayBot[x, y] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y + 1] != 1 &&
                                        arrayBot[x + 2, y] != 1 && arrayBot[x + 2, y + 1] != 1)
                                    {
                                        isValid = true;
                                    }
                                if (!isValid && x == 7 && arrayBot[x, y] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x - 1, y - 1] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x + 1, y - 1] != 1 &&
                                    arrayBot[x + 2, y] != 1 && arrayBot[x + 2, y - 1] != 1 && arrayBot[x + 2, y + 1] != 1 && arrayBot[x, y - 1] != 1 && arrayBot[x, y + 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && y == 0 && arrayBot[x, y] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y + 1] != 1 &&
                                    arrayBot[x + 2, y] != 1 && arrayBot[x + 2, y + 1] != 1 && arrayBot[x + 3, y] != 1 && arrayBot[x + 3, y + 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 0 && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x + 1, y - 1] != 1 &&
                                    arrayBot[x + 2, y] != 1 && arrayBot[x + 2, y - 1] != 1 && arrayBot[x + 2, y + 1] != 1 && arrayBot[x + 3, y] != 1 && arrayBot[x + 3, y - 1] != 1 && arrayBot[x + 3, y + 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && y == 9 && arrayBot[x, y] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x - 1, y - 1] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y - 1] != 1 &&
                                    arrayBot[x + 2, y] != 1 && arrayBot[x + 2, y - 1] != 1 && arrayBot[x + 3, y] != 1 && arrayBot[x + 3, y - 1] != 1)
                                {
                                    isValid = true;
                                }


                                if (!isValid && arrayBot[x, y] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x - 1, y - 1] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x + 1, y - 1] != 1 &&
                                    arrayBot[x + 2, y] != 1 && arrayBot[x + 2, y - 1] != 1 && arrayBot[x + 2, y + 1] != 1 && arrayBot[x + 3, y] != 1 && arrayBot[x + 3, y - 1] != 1 && arrayBot[x + 3, y + 1] != 1 && arrayBot[x, y-1] != 1 && arrayBot[x, y+1] !=1 )
                                {
                                    isValid = true;
                                }
                                if (isValid == true)
                                {
                                    int y_k = -360 + y * 80;
                                    int x_k = -360 + x * 80;
                                    game_zone_bot(x, y, 1);
                                    //OtrisovkaKorabl(x_k, y_k, grid);
                                    x++;
                                    x_k = -360 + x * 80;
                                    game_zone_bot(x, y, 1);
                                   // OtrisovkaKorabl(x_k, y_k, grid);
                                    x++;
                                    game_zone_bot(x, y, 1);
                                    x_k = -360 + x * 80;
                                    //OtrisovkaKorabl(x_k, y_k, grid);
                                    shipsBot[paluba]++;
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            if (y != 9 && y != 8 && shipsBot[paluba]!=2)
                            {
                                bool isValid = false;
                                if (!isValid && x == 0 && y == 7 && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x, y - 1] != 1 && arrayBot[x + 1, y - 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x + 1, y + 2] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 9 && y == 7 && arrayBot[x, y] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x, y - 1] != 1 && arrayBot[x - 1, y - 1] != 1 &&
                                     arrayBot[x, y + 1] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x - 1, y + 2] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && y == 0 && x == 9 && arrayBot[x, y] != 1 && arrayBot[x - 1, y] != 1 &&
                                    arrayBot[x, y + 1] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x - 1, y + 2] != 1 &&
                                    arrayBot[x, y + 3] != 1 && arrayBot[x - 1, y + 3] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 0 && y == 0 && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 &&
                                    arrayBot[x + 1, y + 1] != 1 && arrayBot[x, y + 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x + 1, y + 2] != 1 &&
                                    arrayBot[x, y + 3] != 1 && arrayBot[x + 1, y + 3] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && y == 0 && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x - 1, y] != 1 &&
                                    arrayBot[x + 1, y + 1] != 1 && arrayBot[x, y + 1] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x + 1, y + 2] != 1 && arrayBot[x - 1, y + 2] != 1 &&
                                    arrayBot[x, y + 3] != 1 && arrayBot[x + 1, y + 3] != 1 && arrayBot[x - 1, y + 3] != 1)
                                {
                                    isValid = true;
                                }


                                if (!isValid && x == 9 && arrayBot[x, y] != 1 && arrayBot[x, y - 1] != 1 &&
                                    arrayBot[x, y + 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x, y + 3] != 1)
                                {
                                    isValid = true;
                                }

                                if (!isValid && x == 0 && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x, y - 1] != 1 && arrayBot[x + 1, y - 1] != 1 &&
                                    arrayBot[x + 1, y + 1] != 1 && arrayBot[x, y + 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x + 1, y + 2] != 1 &&
                                    arrayBot[x, y + 3] != 1 && arrayBot[x + 1, y + 3] != 1)
                                {
                                    isValid = true;
                                }

                                if (!isValid && y == 7 && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x, y - 1] != 1 && arrayBot[x + 1, y - 1] != 1 && arrayBot[x - 1, y - 1] != 1 &&
                                    arrayBot[x + 1, y + 1] != 1 && arrayBot[x, y + 1] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x + 1, y + 2] != 1 && arrayBot[x - 1, y + 2] != 1)
                                {
                                    isValid = true;
                                }

                                if (!isValid && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x, y - 1] != 1 && arrayBot[x + 1, y - 1] != 1 && arrayBot[x - 1, y - 1] != 1 &&
                                    arrayBot[x + 1, y + 1] != 1 && arrayBot[x, y + 1] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x + 1, y + 2] != 1 && arrayBot[x - 1, y + 2] != 1 &&
                                    arrayBot[x, y + 3] != 1 && arrayBot[x + 1, y + 3] != 1 && arrayBot[x - 1, y + 3] != 1)
                                {
                                    isValid = true;
                                }

                                if (isValid == true)
                                {
                                    int x_k = -360 + x * 80;
                                    int y_k = -360 + y * 80;
                                    game_zone_bot(x, y, 1);
                                   // OtrisovkaKorabl(x_k, y_k, grid);
                                    y++;
                                    y_k = -360 + y * 80;
                                    game_zone_bot(x, y, 1);
                                   // OtrisovkaKorabl(x_k, y_k, grid);
                                    y++;
                                    y_k = -360 + y * 80;
                                    game_zone_bot(x, y, 1);
                                   // OtrisovkaKorabl(x_k, y_k, grid);
                                    shipsBot[paluba]++;
                                    return true;
                                }
                            }
                        }
                        break;
                    case 4:
                        if (isGorizontal)
                        {
                            bool isValid = false;
                            if (x != 7 && x != 8 && x != 9 && shipsBot[paluba] != 1)
                            {
                                if (!isValid && y == 0 && x == 0 && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y + 1] != 1 &&
                                    arrayBot[x + 2, y] != 1 && arrayBot[x + 2, y + 1] != 1 && arrayBot[x + 3, y] != 1 && arrayBot[x + 3, y + 1] != 1 && arrayBot[x + 4, y] != 1 && arrayBot[x + 4, y + 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && y == 0 && x == 6 && arrayBot[x, y] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y + 1] != 1 &&
                                    arrayBot[x + 2, y] != 1 && arrayBot[x + 2, y + 1] != 1 && arrayBot[x + 3, y] != 1 && arrayBot[x + 3, y + 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && y == 9 && x == 6 && arrayBot[x, y] != 1 && arrayBot[x, y - 1] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x - 1, y - 1] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y - 1] != 1 &&
                                    arrayBot[x + 2, y] != 1 && arrayBot[x + 2, y - 1] != 1 && arrayBot[x + 3, y] != 1 && arrayBot[x + 3, y - 1] != 1)
                                {
                                    isValid = true;
                                }

                                if (!isValid && x == 6 && arrayBot[x, y] != 1 && arrayBot[x, y - 1] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x - 1, y - 1] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x + 1, y - 1] != 1 &&
                                    arrayBot[x + 2, y] != 1 && arrayBot[x + 2, y - 1] != 1 && arrayBot[x + 2, y + 1] != 1 && arrayBot[x + 3, y] != 1 && arrayBot[x + 3, y + 1] != 1 && arrayBot[x + 3, y - 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && y == 9 && x == 0 && arrayBot[x, y] != 1 && arrayBot[x, y - 1] != 1 &&
                                    arrayBot[x + 2, y] != 1 && arrayBot[x + 2, y - 1] != 1 && arrayBot[x + 3, y] != 1 && arrayBot[x + 3, y - 1] != 1 && arrayBot[x + 4, y] != 1 &&
                                    arrayBot[x + 4, y - 1] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 0 && !isValid && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y + 1] != 1 &&
                                    array[x + 2, y] != 1 && arrayBot[x + 2, y - 1] != 1 && arrayBot[x + 2, y + 1] != 1 && arrayBot[x + 3, y] != 1 && arrayBot[x + 3, y + 1] != 1 && arrayBot[x + 3, y - 1] != 1 && arrayBot[x + 4, y] != 1 &&
                                    array[x + 4, y - 1] != 1 && arrayBot[x + 4, y + 1] != 1)
                                {
                                    isValid = true;
                                }

                                if (!isValid && y == 9 && arrayBot[x, y] != 1 && arrayBot[x, y - 1] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x - 1, y - 1] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y - 1] != 1 &&
                                    arrayBot[x + 2, y] != 1 && arrayBot[x + 2, y - 1] != 1 && arrayBot[x + 3, y] != 1 && arrayBot[x + 3, y - 1] != 1 && arrayBot[x + 4, y] != 1 &&
                                    arrayBot[x + 4, y - 1] != 1)
                                {
                                    isValid = true;
                                }

                                if (!isValid && y == 0 && arrayBot[x, y] != 1 && arrayBot[x - 1, y] != 1 && array[x + 1, y] != 1 && array[x + 1, y + 1] != 1 &&
                                    arrayBot[x + 2, y] != 1 && arrayBot[x + 2, y + 1] != 1 && array[x + 3, y] != 1 && array[x + 3, y + 1] != 1 && array[x + 4, y] != 1 && array[x + 4, y + 1] != 1)
                                {
                                    isValid = true;
                                }

                                if (!isValid && arrayBot[x, y] != 1 && arrayBot[x, y - 1] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x - 1, y - 1] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x + 1, y - 1] != 1 &&
                                    arrayBot[x + 2, y] != 1 && arrayBot[x + 2, y - 1] != 1 && arrayBot[x + 2, y + 1] != 1 && arrayBot[x + 3, y] != 1 && arrayBot[x + 3, y + 1] != 1 && arrayBot[x + 3, y - 1] != 1 && arrayBot[x + 4, y] != 1 &&
                                    arrayBot[x + 4, y - 1] != 1 && arrayBot[x + 4, y + 1] != 1)
                                {
                                    isValid = true;
                                }

                                if (isValid == true)
                                {
                                    int y_k = -360 + y * 80;
                                    int x_k = -360 + x * 80;
                                    game_zone_bot(x, y, 1);
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    x++;
                                    x_k = -360 + x * 80;
                                    game_zone(x, y, 1);
                                    //MessageBox.Show($"x = {x.ToString()}\n y = {x.ToString()}");
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    x++;
                                    game_zone_bot(x, y, 1);
                                    x_k = -360 + x * 80;
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    x++;
                                    game_zone_bot(x, y, 1);
                                    x_k = -360 + x * 80;
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    shipsBot[paluba]++;
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            if (y != 9 && y != 8 && y != 7 && shipsBot[paluba] !=1)
                            {
                                bool isValid = false;
                                if (!isValid && y == 0 && x == 0 && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 &&
                                    arrayBot[x, y + 1] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x + 1, y + 2] != 1 &&
                                    arrayBot[x, y + 3] != 1 && arrayBot[x + 1, y + 3] != 1 && arrayBot[x, y + 4] != 1 && arrayBot[x + 1, y + 4] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 9 && y == 0 && arrayBot[x, y] != 1 && arrayBot[x - 1, y] != 1 &&
                                    arrayBot[x, y + 1] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x - 1, y + 2] != 1 &&
                                    arrayBot[x, y + 3] != 1 && arrayBot[x - 1, y + 3] != 1 && arrayBot[x, y + 4] != 1 && arrayBot[x - 1, y + 4] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 9 && y == 6 && arrayBot[x, y] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x, y - 1] != 1 && arrayBot[x - 1, y - 1] != 1 &&
                                    arrayBot[x, y + 1] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x - 1, y + 2] != 1 &&
                                    arrayBot[x, y + 3] != 1 && arrayBot[x - 1, y + 3] != 1)
                                {
                                    isValid = true;
                                }

                                if (!isValid && x == 0 && y == 6 && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x, y - 1] != 1 && arrayBot[x + 1, y - 1] != 1 &&
                                    arrayBot[x, y + 1] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x + 1, y + 2] != 1 &&
                                    arrayBot[x, y + 3] != 1 && arrayBot[x + 1, y + 3] != 1)
                                {
                                    isValid = true;
                                }

                                if (!isValid && y == 0 && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x - 1, y] != 1 &&
                                    arrayBot[x, y + 1] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x + 1, y + 2] != 1 && arrayBot[x - 1, y + 2] != 1 &&
                                    arrayBot[x, y + 3] != 1 && arrayBot[x + 1, y + 3] != 1 && arrayBot[x - 1, y + 3] != 1 && arrayBot[x, y + 4] != 1 && arrayBot[x + 1, y + 4] != 1 && arrayBot[x - 1, y + 4] != 1)
                                {
                                    isValid = true;
                                }

                                if (!isValid && x == 0 && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x, y - 1] != 1 && arrayBot[x + 1, y - 1] != 1 &&
                                    arrayBot[x, y + 1] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x + 1, y + 2] != 1 &&
                                    arrayBot[x, y + 3] != 1 && arrayBot[x + 1, y + 3] != 1 && arrayBot[x, y + 4] != 1 && arrayBot[x + 1, y + 4] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && x == 9 && arrayBot[x, y] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x, y - 1] != 1 && arrayBot[x - 1, y - 1] != 1 &&
                                    arrayBot[x, y + 1] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x - 1, y + 2] != 1 &&
                                    arrayBot[x, y + 3] != 1 && arrayBot[x - 1, y + 3] != 1 && arrayBot[x, y + 4] != 1 && arrayBot[x - 1, y + 4] != 1)
                                {
                                    isValid = true;
                                }
                                if (!isValid && y == 6 && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x, y - 1] != 1 && arrayBot[x + 1, y - 1] != 1 && arrayBot[x - 1, y - 1] != 1 &&
                                    arrayBot[x, y + 1] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x + 1, y + 2] != 1 && arrayBot[x - 1, y + 2] != 1 &&
                                    arrayBot[x, y + 3] != 1 && arrayBot[x + 1, y + 3] != 1 && arrayBot[x - 1, y + 3] != 1)
                                {
                                    isValid = true;
                                }


                                if (!isValid && arrayBot[x, y] != 1 && arrayBot[x + 1, y] != 1 && arrayBot[x - 1, y] != 1 && arrayBot[x, y - 1] != 1 && arrayBot[x + 1, y - 1] != 1 && arrayBot[x - 1, y - 1] != 1 &&
                                    arrayBot[x, y + 1] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x - 1, y + 1] != 1 && arrayBot[x + 1, y + 1] != 1 && arrayBot[x, y + 2] != 1 && arrayBot[x + 1, y + 2] != 1 && arrayBot[x - 1, y + 2] != 1 &&
                                    arrayBot[x, y + 3] != 1 && arrayBot[x + 1, y + 3] != 1 && arrayBot[x - 1, y + 3] != 1 && arrayBot[x, y + 4] != 1 && arrayBot[x + 1, y + 4] != 1 && arrayBot[x - 1, y + 4] != 1)
                                {
                                    isValid = true;
                                }

                                if (isValid == true)
                                {
                                    int x_k = -360 + x * 80;
                                    int y_k = -360 + y * 80;
                                    game_zone_bot(x, y, 1);
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    y++;
                                    y_k = -360 + y * 80;
                                    game_zone_bot(x, y, 1);
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    y++;
                                    y_k = -360 + y * 80;
                                    game_zone_bot(x, y, 1);
                                    OtrisovkaKorabl(x_k, y_k, grid);
                                    y++;
                                    y_k = -360 + y * 80;
                                    game_zone_bot(x, y, 1);
                                   OtrisovkaKorabl(x_k, y_k, grid);
                                    shipsBot[paluba]++;
                                    return true;
                                }
                            }
                        }
                        break;
                }
                return false;
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
                return false;
            }
        }
        public void OtrisovkaShipsBots(Grid grid) // Отрисовка кораблей бота
        {
            Random random = new Random();
          //  MessageBox.Show(Convert.ToBoolean(random.Next(0,1)).ToString());
            int j = 4;
            int kolvo = 1; //АААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААА
            for (int i = 0; i <= j;)
            {
                if (j == 0)
                {
                    return;
                }
                if (i < j)
                {
                    while (!CheckingShipsBot(random.Next(0, 9), random.Next(0, 9), kolvo, Convert.ToBoolean(random.Next(0, 2)), grid))
                    {
                   
                    }
                    i++;
                }
                else
                {
                    i = 0;
                    j--;
                    kolvo++;
                }    
                
              // j++;
              // if (i == j)
              // {
              //     kolvo++;
              //     j--;
              //     i = 0;
              // }
              // i++;
            }
        }

        // Генерация кораблей для компьютера
        //  однопалуб = 4
        //  двухпалуб = 3
        //  трехпалуб = 2
        //  четырехпалуб = 1
        public void GenerationOfShips()
        {
         //   MessageBox.Show("Генерация корабликов");
         //   Random random = new Random();
         //   //Однопалубные = 4
         //   for (int i = 0; i < 4; i++)
         //   {
         //       int rand, rand2;
         //       rand = random.Next(0, 10);
         //       rand2 = random.Next(0, 10);
         //       if (check_massiv_bot(rand, rand2))
         //       {
         //           game_zone_bot(rand, rand2, 1);
         //       }
         //   }
         //   //двухпалуб = 3
         //   for (int i = 0; i < 3; i++)
         //   {
         //       int rand, rand2;
         //       int ranoms = random.Next(1, 4);
         //       rand = random.Next(0, 10);
         //       rand2 = random.Next(rand, 10);
         //       for (int j = 0; j < 2; j++)
         //       {
         //           if (check_massiv_bot(rand, rand2))
         //           {
         //               game_zone_bot(rand, rand2, 1);
         //           }
         //           if (ranoms == 1)
         //           {
         //               rand2++;
         //           }
         //           else if (ranoms == 2)
         //           {
         //               rand2 = rand2 - 2;
         //           }
         //           else if (ranoms == 3)
         //           {
         //
         //           }
         //       }
         //   }
        }
        public int prov(int x, int y, int b, int vert)
        {
            int otv = 0;
            if (vert == 0)
            {
                switch (b)

                {

                    case 1: if (array[x, y] != 0) otv = 1; break;

                    case 2: if (array[x, y] != 0 || array[x + 1, y] != 0) otv = 1; break;

                    case 3: if (array[x, y] != 0 || array[x + 1, y] != 0 || array[x + 2, y] != 0) otv = 1; break;

                    case 4: if (array[x, y] != 0 || array[x + 1, y] != 0 || array[x + 2, y] != 0 || array[x + 3, y] != 0) otv = 1; break;

                    default: otv = 0; break;

                }

            }

            else

            {

                switch (b)

                {

                    case 1: if (array[x, y] != 0) otv = 1; break;

                    case 2: if (array[x, y] != 0 || array[x, y + 1] != 0) otv = 1; break;

                    case 3: if (array[x, y] != 0 || array[x, y + 1] != 0 || array[x, y + 2] != 0) otv = 1; break;

                    case 4: if (array[x, y] != 0 || array[x, y + 1] != 0 || array[x, y + 2] != 0 || array[x, y + 3] != 0) otv = 1; break;

                    default: otv = 0; break;
                }
            }
            return otv;
        }
        public void CorrectnesShips()
        {

        }

        public void ChekingDeadShips(Grid grid)
        {
            //   int x = 2;
            //   int y = 2;
            //   int x_k = 0, y_k = 0;
            //   if (arrayHisortPlayer[x,y] == 1 && arrayBot[x,y] == 1)
            //   {
            //       x--;
            //       x_k = -360 + x * 80;
            //       y_k = -360 + y * 80;
            //       for (int i = 0; i < 8; i++)
            //       {
            //           OtrisovkaPromox(x_k,y_k, grid);
            //       }
            //   }
        }
        public static int[,] arrayhis = new int[10,10];
        public void Guning(Grid grid)
        {
            try
            {
                int x = 0;
                int y = 0;
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (arrayhis[i, j] == 1 && array[i,j] == 1)
                        {
                             x = i;
                             y = j;
                            arrayhis[i, j] = 0;
                            goto proverka;
                        }
                        else
                        {
                            arrayhis[i, j] = 0;
                        }    
                    }
                }
                Random rand = new Random();
                 x = rand.Next(0, 10);
                 y = rand.Next(0, 10);
                Thread.Sleep(600);
                int x_k = 0, y_k = 0;
                proverka:
                x_k = -360 + x * 80;
                y_k = -360 + y * 80;
                if (arrayHisortBot[x, y] == 0)
                {
                    if (array[x, y] == 1)
                    {
                        OtrisovkaPopal(x_k, y_k, grid);
                        arrayHisortBot[x, y] = 1;
                        array[x, y] = 2;                       
                        if (array[x, y + 1] == 1 || array[x + 1, y] == 1 || array[x - 1, y] == 1 || array[x, y - 1] == 1)
                        {
                            //MessageBox.Show("Корабль не убит, вы просто попали по нему!");
                            arrayhis[x,y+1] = 1;
                            arrayhis[x,y-1] = 1;
                            arrayhis[x+1,y] = 1;
                            arrayhis[x-1,y] = 1;
                        }
                        else
                        {
                           // MessageBox.Show("Убит!");
                        }

                        if (array[x, y + 1] == 2 || array[x + 1, y] == 2 || array[x - 1, y] == 2 || array[x, y - 1] == 2 && array[x + 1, y] != 1 && array[x - 1, y] != 1 && array[x, y + 1] != 1 && array[x, y - 1] != 1)
                        {
                            //MessageBox.Show("Убил!");
                        }
                        Thread.Sleep(500);
                        Guning(grid);
                        Thread.Sleep(500);
                        if (CheckingWinBoot())
                        {
                            MessageBox.Show("Победил бот!");
                        }
                        //arrayHisortBot[x, y] = 2;
                    }
                    else
                    {
                        OtrisovkaPromox(x_k, y_k, grid);
                        //arrayHisortBot[x, y] = 1;
                    }
                }
            }
            catch
            {
              //  MessageBox.Show("ERRROR");
            }
        }

        // Хранение убитых кораблей
        public void StorageOfDeadShips()
        {

        }

        // Хранение информации о ходах
        public void StoringInformationAboutMoves(int x, int y)
        {
            //  ArrayPlayer[]
        }

        // Проверка количества кораблей
        public void CheckingTheNumberOfShips()
        {

        }
        public static int[,] historyGuningPlayer = new int[10, 10];
        public bool CheckInPeresechenie(Grid grid, System.Windows.Point pt)
        {
            int x = Convert.ToInt32(pt.X), y = Convert.ToInt32(pt.Y);
            int x_k = 0, y_k = 0;
            x = (x - (int)grid.Margin.Left) / 40;
            y = (y - (int)grid.Margin.Top) / 40;
            x_k = -360 + x * 80;
            y_k = -360 + y * 80;

            if (historyGuningPlayer[x, y] == 0)
            {
                    historyGuningPlayer[x, y] = 2;
                    return true;
            }
            else
            {
                MessageBox.Show("Сюда уже били");
                return false;
            }
        }
    }
}
