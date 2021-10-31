using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NumberLadder
{
    public class Form1 : Form
    {
        private List<Button> BoardButtons { get; }
        private TextBox GameInfo { get; }
        private Button ResetButton { get; }
        private int Piece { get; set; } = 1;
        private int Turn { get; set; }
        private bool Locked { get; set; }

        private const int Spacer = 10;
        private const int SidePadding = 40;
        private const int ButtonSize = 75;

        private static readonly Color UnusedColor = Color.LightGray;
        private static readonly Color LegalColor = Color.PaleTurquoise;
        private static readonly Color PieceColor = Color.PaleGreen;

        private Minimax MinimaxSearcher { get; set; }

        private void UpdateInfo()
        {
            var minimaxResult = MinimaxSearcher.NegamaxRoot(Piece);
            var t = Turn == 0 ? "Player 1" : "Player 2";
            var moveTillMate = 50 - Math.Abs(minimaxResult.Score);
            var res = minimaxResult.Score > 0
                ? $"Win in {moveTillMate} move"
                : $"Loss in {moveTillMate} move" + (moveTillMate == 1 ? "" : "s");
            GameInfo.Lines = new[]
            {
                $"Position: {Piece}",
                $"Turn: {t}",
                GameInfo.Lines[2],
                minimaxResult.BestMove == -1 ? "Minimax: None" : $"Minimax: Move to {minimaxResult.BestMove}, {res}",
                GameInfo.Lines[4]
            };
        }

        private void RemoveTrace()
        {
            BoardButtons.ForEach(p => p.BackColor = UnusedColor);
        }

        private void UpdateTrace()
        {
            BoardButtons[Piece - 1].BackColor = PieceColor;

            foreach (var legalDestination in Logic.LegalDestinations(Piece))
            {
                BoardButtons[legalDestination - 1].BackColor = LegalColor;
            }
        }

        private EventHandler ButtonOnClick(int i)
        {
            return (_, _) =>
            {
                if (Locked) return;
                if (!Logic.LegalDestinations(Piece).Contains(i)) return;
                RemoveTrace();
                Piece = i;

                if (i == 35)
                {
                    Locked = true;
                    var w = Turn == 0 ? "Player 1" : "Player 2";
                    GameInfo.Lines = new[]
                    {
                        GameInfo.Lines[0],
                        GameInfo.Lines[1],
                        $"{w} Wins!",
                        GameInfo.Lines[3],
                        GameInfo.Lines[4]
                    };

                    UpdateTrace();
                    UpdateInfo();

                    return;
                }

                Turn ^= 1;

                UpdateTrace();

                UpdateInfo();
            };
        }

        private void Reset()
        {
            Piece = 1;
            Turn = 0;
            Locked = false;
            GameInfo.Lines = new[] {"", "", "", "", ""};
            MinimaxSearcher = new Minimax();

            RemoveTrace();
            UpdateTrace();
            UpdateInfo();
        }

        public Form1()
        {
            BoardButtons = Enumerable.Range(1, 35)
                .Select(i =>
                {
                    var b = new Button();
                    b.Width = ButtonSize;
                    b.Height = ButtonSize;
                    var loc = Logic.IndexToPiece[i];
                    b.Location = new Point(SidePadding + (Spacer + ButtonSize) * (loc.X - 1),
                        SidePadding + (Spacer + ButtonSize) * (loc.Y - 1));
                    b.Text = i.ToString();
                    b.Click += ButtonOnClick(i);
                    Controls.Add(b);
                    return b;
                })
                .ToList();
            RemoveTrace();

            GameInfo = new TextBox();
            GameInfo.Multiline = true;
            GameInfo.Lines = new[] {"", "", "", "", ""};
            GameInfo.Height = 300;
            GameInfo.Width = 240;
            GameInfo.Font = new Font("Arial", 16);
            GameInfo.ReadOnly = true;
            GameInfo.Location = new Point(SidePadding + (Spacer + ButtonSize) * 9 + SidePadding, 50);
            Controls.Add(GameInfo);

            ResetButton = new Button();
            var n = GameInfo.Location;
            n.Offset(15, GameInfo.Height + 30);
            ResetButton.Location = n;
            ResetButton.Text = "Reset Game";
            ResetButton.Width = GameInfo.Width - 30;
            ResetButton.Height = 50;
            ResetButton.Click += (_, _) => { Reset(); };
            Controls.Add(ResetButton);

            MinimaxSearcher = new Minimax();

            UpdateInfo();
            UpdateTrace();
        }
    }
}