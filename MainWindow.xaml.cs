using Microsoft.Win32;
using System;
using System.CodeDom;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Sokoban.MainWindow;

namespace Sokoban
{
    enum AppState { MainMenu, Controls, LevelSelect, Game, Victory };
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _rows;
        private int _columns;
        public Random rnd = new Random();
        private AppState _state = AppState.MainMenu;
        private string _filePath;

        private string[] _textKeys =
        {
            "W",
            "P",
            "G",
            "B"
        };

        //[in clumn num,in row num] [x,y]

        Tile[] _tiles;

        public MainWindow()
        {
            InitializeComponent();
            ChangeState(AppState.MainMenu);
        }

        public void LoadGame()
        {
            string[][] loadedFile = File.ReadAllLines(_filePath).Select(a => a.Split(",")).ToArray();
            if (loadedFile.Length == 0)
            {
                {
                    ShowError("File is empty.");
                    return;
                }
            }
            _tiles = new Tile[loadedFile.Length - 2];

            for (int i = 0; i < loadedFile.Length; i++)
            {
                if (i == 0)
                {
                    if (!int.TryParse(loadedFile[i][0], out _columns))
                    {
                        ShowError("Wrong file format. Wrong column size format.");
                        return;
                    }
                }
                else if (i == 1)
                {
                    if (!int.TryParse(loadedFile[i][0], out _rows))
                    {
                        ShowError("Wrong file format. Wrong row size format.");
                        return;
                    }
                    GenerateMap();
                }
                else
                {

                    if (!_textKeys.Contains(loadedFile[i][0]))
                    {
                        ShowError("Wrong file format. Wrong tile type format.");
                        return;
                    }

                    if (!int.TryParse(loadedFile[i][1], out int x))
                    {
                        ShowError("Wrong file format. Wrong tile x-coordinate format.");
                        return;
                    }
                    if (!int.TryParse(loadedFile[i][2], out int y))
                    {
                        ShowError("Wrong file format. Wrong tile y-coordinate format.");
                        return;
                    }

                    //end of error checking

                    switch (_textKeys.First(a => a == loadedFile[i][0]))
                    {
                        case "W":
                            _tiles[i - 2] = new Wall(new(x, y), ref _tiles, ref TileGrid, TryFindResource($"Wall{rnd.Next(1, 4)}") as Style);
                            break;
                        case "G":
                            _tiles[i - 2] = new Goal(new(x, y), ref _tiles, ref TileGrid, TryFindResource("Goal") as Style);
                            break;
                        case "B":
                            _tiles[i - 2] = new Box(new(x, y), ref _tiles, ref TileGrid, TryFindResource("Box") as Style);
                            break;
                        case "P":
                            _tiles[i - 2] = new Player(new(x, y), ref _tiles, ref TileGrid, TryFindResource("Player") as Style);
                            break;
                        default:
                            break;
                    }
                }
            }
            ChangeState(AppState.Game);
        }

        private static void ShowError(string message = null)
        {
            MessageBox.Show("Could not load level. " + message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void GenerateMap()
        {
            GrassGrid.ColumnDefinitions.Clear();
            GrassGrid.RowDefinitions.Clear();
            GrassGrid.Children.Clear();

            for (int i = 0; i < _columns; i++)
            {
                GrassGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < _rows; i++)
            {
                GrassGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int x = 0; x < _columns; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    Image item = new Image();
                    item.Style = TryFindResource($"Grass{rnd.Next(1, 4)}") as Style;
                    Grid.SetZIndex(item, 10000);
                    Grid.SetRow(item, y);
                    Grid.SetColumn(item, x);
                    GrassGrid.Children.Add(item);
                }
            }

            TileGrid.ColumnDefinitions.Clear();
            TileGrid.RowDefinitions.Clear();
            TileGrid.Children.Clear();

            for (int i = 0; i < _columns; i++)
            {
                TileGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < _rows; i++)
            {
                TileGrid.RowDefinitions.Add(new RowDefinition());
            }
        }

        private void KeyboardControls(object sender, KeyEventArgs e)
        {
            if (_state == AppState.Victory)
                ChangeState(AppState.MainMenu);
            else if (e.Key == Key.Escape)
                ChangeState(AppState.MainMenu);
            else if (_state == AppState.Game)
            {
                if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right)
                {
                    Vector direction;
                    switch (e.Key)
                    {
                        case Key.Up:
                            direction = new(0, -1);
                            break;
                        case Key.Down:
                            direction = new(0, 1);
                            break;
                        case Key.Left:
                            direction = new(-1, 0);
                            break;
                        case Key.Right:
                            direction = new(1, 0);
                            break;
                        default:
                            break;
                    }

                    foreach (Player player in _tiles.Where(a => a.GetType() == typeof(Player)))
                    {
                        player.MovedThisTurn = false;
                    }
                    foreach (Player player in _tiles.Where(a => a.GetType() == typeof(Player)))
                    {
                        player.PlayerMove(direction);
                    }
                    CheckGoals();
                }

                else if (e.Key == Key.R)
                    LoadGame();
            }
        }

        private void CheckGoals()
        {
            if ((_tiles.Where(a => a.GetType() == typeof(Goal))).ToArray()
                 .Select(a => ((Goal)a).IsDone()).ToArray()
                 .All(a => a)
               )
            {
                ChangeState(AppState.Victory);
            }
        }

        private void ChangeState(AppState state)
        {

            switch (state)
            {
                case AppState.MainMenu:
                    _state = AppState.MainMenu;

                    Menus.Visibility = Visibility.Visible;
                    for (int i = 0; i < Menus.Children.Count; i++)
                    {
                        if (Menus.Children[i].GetType() == typeof(Grid))
                        Menus.Children[i].Visibility = Visibility.Collapsed;
                    }
                    Game.Visibility = Visibility.Collapsed;
                    MainMenu.Visibility = Visibility.Visible;

                    break;

                case AppState.Controls:
                    _state = AppState.Controls;

                    Menus.Visibility = Visibility.Visible;
                    for (int i = 0; i < Menus.Children.Count; i++)
                    {
                        if (Menus.Children[i].GetType() == typeof(Grid))
                            Menus.Children[i].Visibility = Visibility.Collapsed;
                    }
                    Game.Visibility = Visibility.Collapsed;
                    ControlsMenu.Visibility = Visibility.Visible;

                    break;

                case AppState.LevelSelect:
                    _state = AppState.LevelSelect;

                    Menus.Visibility = Visibility.Visible;
                    for (int i = 0; i < Menus.Children.Count; i++)
                    {
                        if (Menus.Children[i].GetType() == typeof(Grid))
                            Menus.Children[i].Visibility = Visibility.Collapsed;
                    }
                    Game.Visibility = Visibility.Collapsed;
                    LoadLevelMenu.Visibility = Visibility.Visible;

                    break;

                case AppState.Game:
                    _state = AppState.Game;

                    Game.Visibility = Visibility.Visible;
                    Menus.Visibility = Visibility.Collapsed;
                    VictoryScreen.Visibility = Visibility.Collapsed;

                    break;

                case AppState.Victory:
                    _state = AppState.Victory;

                    VictoryScreen.Visibility = Visibility.Visible;
                    Game.Visibility = Visibility.Visible;
                    Menus.Visibility = Visibility.Collapsed;
                    break;

                default:
                    break;
            }
        }

        private void ChangeMenuControls(object sender, RoutedEventArgs e)
        {
            ChangeState(AppState.Controls);
        }

        private void ChangeMenuLevel(object sender, RoutedEventArgs e)
        {
            ChangeState(AppState.LevelSelect);
        }

        private void LoadFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new();
            fileDialog.Filter = "Sokoban files (*.soko ; *.sokoban)|*.soko;*.sokoban";
            fileDialog.Title = "Load level";

            if (fileDialog.ShowDialog() == false)
            {
                ShowError("Could not find file.");
                return;
            }
            _filePath = fileDialog.FileName;
            LoadGame();
        }

        private void LoadLevelButton(object sender, RoutedEventArgs e)
        {
            _filePath = "Resources\\Levels\\" + ((Button)sender).Tag.ToString();
            LoadGame();
        }
    }

    public abstract class Tile
    {
        public Point Coordinates;
        protected Tile[] _listReference;
        public Image Item;

        protected Tile(Point coordinates, ref Tile[] listReference, ref Grid objectGrid, Style style)
        {
            Coordinates = coordinates;
            _listReference = listReference;
            Item = new();
            Item.Style = style;
            objectGrid.Children.Add(Item);
            ChangePosition(new());
        }

        public bool Move(Vector direction, bool wasPushed = false)
        {
            if ((direction + Coordinates).X < 0 || (direction + Coordinates).Y < 0 ||
                (direction + Coordinates).X >= ((Grid)Item.Parent).ColumnDefinitions.Count || (direction + Coordinates).Y >= ((Grid)Item.Parent).RowDefinitions.Count)
                return false;
            if (CanMove(direction, wasPushed))
            {
                ChangePosition(direction);
                return true;
            }
            return false;
        }

        protected abstract bool CanMove(Vector direction, bool wasPushed = false);

        protected void ChangePosition(Vector direction)
        {
            Coordinates += direction;
            Grid.SetRow(Item, (int)Coordinates.Y);
            Grid.SetColumn(Item, (int)Coordinates.X);
        }
    }

    public class Wall : Tile
    {
        public Wall(Point coordinates, ref Tile[] listReference, ref Grid objectGrid, Style style) : base(coordinates, ref listReference, ref objectGrid, style)
        {
        }

        protected override bool CanMove(Vector direction, bool wasPushed = false)
        {
            return false;
        }
    }

    public class Goal : Tile
    {
        public Goal(Point coordinates, ref Tile[] listReference, ref Grid objectGrid, Style style) : base(coordinates, ref listReference, ref objectGrid, style)
        {
        }

        protected override bool CanMove(Vector direction, bool wasPushed = false)
        {
            return true;
        }

        public bool IsDone()
        {
            foreach (Box box in _listReference.Where(a => a.GetType() == typeof(Box)))
            {
                if (box.Coordinates == Coordinates)
                    return true;
            }
            return false;
        }
    }

    public class Player : Tile
    {
        public bool MovedThisTurn;
        public Player(Point coordinates, ref Tile[] listReference, ref Grid objectGrid, Style style) : base(coordinates, ref listReference, ref objectGrid, style)
        {
        }

        public void PlayerMove(Vector direction)
        {
            if (MovedThisTurn)
                return;
            Move(direction);
        }

        protected override bool CanMove(Vector direction, bool wasPushed = false)
        {
            wasPushed = false;
            Tile tile = _listReference.Where(a => a.GetType() != typeof(Goal)).FirstOrDefault(a => a.Coordinates == this.Coordinates + direction);

            if (tile != null)
                if (tile.Move(direction, wasPushed))
                {
                    MovedThisTurn = true;
                    return true;
                }
                else
                    return false;
            else
            {
                MovedThisTurn = true;
                return true;
            }

        }
    }

    public class Box : Tile
    {
        public Box(Point coordinates, ref Tile[] listReference, ref Grid objectGrid, Style style) : base(coordinates, ref listReference, ref objectGrid, style)
        {
        }

        protected override bool CanMove(Vector direction, bool wasPushed = false)
        {
            if (wasPushed)
                return false;
            wasPushed = true;

            Tile tile = _listReference.Where(a => a.GetType() != typeof(Goal)).FirstOrDefault(a => a.Coordinates == this.Coordinates + direction);

            if (tile != null)
                return tile.Move(direction, wasPushed);
            else
                return true;
        }
    }

}