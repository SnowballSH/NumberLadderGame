using System;

namespace NumberLadder
{
    public struct MinimaxStat
    {
        public int Score { get; set; }
        public int BestMove { get; set; }

        public MinimaxStat(int score, int bestMove)
        {
            Score = score;
            BestMove = bestMove;
        }
    }

    public class Minimax
    {
        private int Negamax(int pos, int alpha, int beta, int ply)
        {
            if (pos == 35) return -50 + ply;

            var stat = -1000;

            var children = Logic.LegalDestinations(pos);

            foreach (var child in children)
            {
                stat = Math.Max(stat, -Negamax(child, -beta, -alpha, ply + 1));
                alpha = Math.Max(alpha, stat);
                if (alpha >= beta) break;
            }

            return stat;
        }

        public MinimaxStat NegamaxRoot(int pos)
        {
            if (pos == 35) return new MinimaxStat(-50, -1);

            var alpha = -1000;
            const int beta = 1000;

            var stat = new MinimaxStat(-1000, -1);

            var children = Logic.LegalDestinations(pos);

            foreach (var child in children)
            {
                var newS = Negamax(child, -beta, -alpha, 1);
                if (-newS > stat.Score)
                {
                    stat.Score = -newS;
                    stat.BestMove = child;
                }

                alpha = Math.Max(alpha, stat.Score);
                if (alpha >= beta) break;
            }

            return stat;
        }
    }
}