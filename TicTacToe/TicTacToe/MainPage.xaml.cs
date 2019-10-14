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
            
            RestartBtn.Clicked += (sender, args) => Restart();
        }

        public void InitField()
        {
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    var curBtn = new Button
                    {
                        Text = "",
                        FontSize = 40.5
                    };
                    _buttons[i, j] = curBtn;
                    this.GameField.Children.Add(_buttons[i,j], i, j);
                    curBtn.Clicked += OnButtonClick;
                }
            }
        }

        public void OnButtonClick(object sender, EventArgs args)
        {
            var curBtn = (Button) sender;

            if (curBtn.Text.Length != 0) 
                return; // пользователь нажал на заполненную кнопку
            
            curBtn.Text = StateToString(Gamer);

            var state = CheckGameState();
            System.Diagnostics.Debug.WriteLine(state);
            if (state == GameState.StillHappening)
            {
                Gamer = NextState();
                this.StateLabel.Text = Gamer == Gamer.X ? "cross" : "null";
            }
            else
            {
                ShowWinner(state);
                Restart();
            }
            
        }

        public void ShowWinner(GameState state)
        {
            if (state == GameState.NotWiner)
                ShowMessage("Ничья", "Побеждатора нет");
            else if (state == GameState.OWin || state == GameState.XWin)
                ShowMessage("Победа", "Победил " + (state == GameState.XWin ? "крестик" : "нолик"));
        }

        public void ShowMessage(string title, string message)
        {
            DisplayAlert (title, message, "OK");
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
            /*
             * крепись, дальше простыня кода
             */
            string prev;
            
            Func<bool, string, GameState, GameState> testState = (bool winend, string str, GameState st) =>
            {
                var state = st;
                if (winend && str.Length != 0)
                {
                    switch (StringToState(str))
                    {
                        case Gamer.O:
                            state = GameState.OWin;
                            break;
                        case Gamer.X:
                            state = GameState.XWin;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                return state;
            };
            
            // горизонтальная проверка
            var HorizontalState = GameState.StillHappening;
            for (var i = 0; i < 3; i++)
            {
                prev = _buttons[i, 0].Text;
                bool win = true;
                for (var j = 1; j < 3; j++)
                {
                    if (_buttons[i, j].Text != prev)
                    {
                        win = false;
                        break;
                    }
                }

                HorizontalState = testState(win, prev, HorizontalState);
            }

            if (HorizontalState == GameState.XWin || HorizontalState == GameState.OWin) return HorizontalState;

            // вертикальная проверка
            var VerticalState = GameState.StillHappening;
            for (var j = 0; j < 3; j++)
            {
                prev = _buttons[0, j].Text;
                bool win = true;
                for (var i = 1; i < 3; i++)
                {
                    if (_buttons[i, j].Text != prev)
                    {
                        win = false;
                        break;
                    }
                }

                VerticalState = testState(win, prev, VerticalState);

            }

            if (VerticalState == GameState.XWin || VerticalState == GameState.OWin) return VerticalState;

            // диоганальная проверка
            var DiaganalState = GameState.StillHappening;
            {
                prev = _buttons[0, 0].Text;
                bool win = true;
                for (var i = 1; i < 3; i++)
                {
                    if (_buttons[i, i].Text != prev)
                    {
                        win = false;
                        break;
                    }
                }

                DiaganalState = testState(win, prev, DiaganalState);
                
                prev = _buttons[0, 2].Text;
                win = true;
                for (var i = 1; i < 3; i++)
                {
                    if (_buttons[i, 3-i-1].Text != prev)
                    {
                        win = false;
                        break;
                    }
                }

                DiaganalState = testState(win, prev, DiaganalState);
            }


            if (DiaganalState == GameState.XWin || DiaganalState == GameState.OWin) return DiaganalState;

            foreach (var button in _buttons)
            {
                if (button.Text.Length == 0)
                {
                    return GameState.StillHappening;
                }
            }

            return GameState.NotWiner;
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