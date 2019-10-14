using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TicTacToe
{

    public partial class MainPage : ContentPage
    {
        private readonly Button[,] _buttons;
        public Gamer Gamer = Gamer.X;
        public MainPage()
        {
            InitializeComponent();
            _buttons = new Button[3,3];
            InitField();
        }

        public void InitField()
        {
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    var curBtn = new Button();
                    _buttons[i, j] = curBtn;
                    this.GameField.Children.Add(_buttons[i,j], i, j);
                    curBtn.Clicked += (object sender, EventArgs args) =>
                    {
                        if (curBtn == null)
                        {
                            System.out
                        }
                        if (curBtn.Text.Length == 0) 
                            return; // пользователь нажал на заполненную кнопку

                        curBtn.Text = StateToString(Gamer);
                        Gamer = NextState();
                    };
                }
            }
        }

        public Gamer StringToState(string str)
        {
            switch (str)
            {
                case "X":
                    return Gamer.X;
                case "O":
                    return Gamer.O;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public static string StateToString(Gamer st)
        {
            switch (st)
            {
                case Gamer.X:
                    return "X";
                case Gamer.O:
                    return "O";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public GameState CheckGameState()
        {
            // горизонтальная проверка
            var HorizontalState = GameState.StillHappening;
            for (var i = 0; i < 3; i++)
            {
                var prev = _buttons[i, 0].Text;
                bool win = true;
                for (var j = 1; j < 3; j++)
                {
                    if (_buttons[i, j].Text != prev)
                    {
                        win = false;
                        break;
                    }
                }

                if (win && prev.Length != 0)
                {
                    switch (StringToState(prev))
                    {
                        case Gamer.O:
                            HorizontalState = GameState.OWin;
                            break;
                        case Gamer.X:
                            HorizontalState = GameState.XWin;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            
            // вертикальная проверка
            var VerticalState = GameState.StillHappening;
            for (var j = 0; j < 3; j++)
            {
                var prev = _buttons[0, j].Text;
                bool win = true;
                for (var i = 1; i < 3; i++)
                {
                    if (_buttons[i, j].Text != prev)
                    {
                        win = false;
                        break;
                    }
                }

                if (win && prev.Length != 0)
                {
                    switch (StringToState(prev))
                    {
                        case Gamer.O:
                            VerticalState = GameState.OWin;
                            break;
                        case Gamer.X:
                            VerticalState = GameState.XWin;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            
            // диоганальная проверка
            /*for (var i = 0; i < 3; i++)
            {
                _buttons[i, i].Text;
            }*/

            switch (HorizontalState)
            {
                case GameState.XWin:
                case GameState.OWin:
                    return HorizontalState;
            }
            switch (VerticalState)
            {
                case GameState.XWin:
                case GameState.OWin:
                    return VerticalState;
            }

            return GameState.StillHappening;
        }

        public Gamer NextState()
        {
            switch (Gamer)
            {
                case Gamer.X:
                    return Gamer.O;
                case Gamer.O:
                    return Gamer.X;
                default:
                    throw new ArgumentOutOfRangeException($"not founded state");
            }
        }

        public void Restart()
        {
            Gamer = Gamer.X;
            
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    _buttons[i, j].Text = "";
                }
            }
        }
    }

    public enum Gamer
    {
        O, X
    }

    public enum GameState
    {
        XWin, OWin, StillHappening, NotWiner
    }
}