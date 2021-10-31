using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NumberLadder
{
    public static class Logic
    {
        /// <summary>
        /// Map index to position
        ///                         35
        ///                   32 33 34
        ///                28 29 30 31
        ///             23 24 25 26 27
        ///          17 18 19 20 21 22
        ///       10 11 12 13 14 15 16
        /// 01 02 03 04 05 06 07 08 09
        /// </summary>
        public static readonly Point[] IndexToPiece =
        {
            new(), // 0

            new(1, 7), new(2, 7), new(3, 7), new(4, 7), new(5, 7), new(6, 7),
            new(7, 7), new(8, 7), new(9, 7),

            new(3, 6), new(4, 6), new(5, 6), new(6, 6), new(7, 6), new(8, 6),
            new(9, 6),

            new(4, 5), new(5, 5), new(6, 5), new(7, 5), new(8, 5), new(9, 5),

            new(5, 4), new(6, 4), new(7, 4), new(8, 4), new(9, 4),

            new(6, 3), new(7, 3), new(8, 3), new(9, 3),

            new(7, 2), new(8, 2), new(9, 2),

            new(9, 1)
        };

        private static readonly int?[][] PieceToIndex =
        {
            new int?[] {null, null, null, null, null, null, null, null, 35},
            new int?[] {null, null, null, null, null, null, 32, 33, 34},
            new int?[] {null, null, null, null, null, 28, 29, 30, 31},
            new int?[] {null, null, null, null, 23, 24, 25, 26, 27},
            new int?[] {null, null, null, 17, 18, 19, 20, 21, 22},
            new int?[] {null, null, 10, 11, 12, 13, 14, 15, 16},
            new int?[] {1, 2, 3, 4, 5, 6, 7, 8, 9},
        };

        // piece is 1-indexed
        public static IEnumerable<int> LegalDestinations(int piece)
        {
            var pos = IndexToPiece[piece];

            return from p in IndexToPiece
                where (p.X >= pos.X && p.Y <= pos.Y) && (p != pos) &&
                      ((p.X > pos.X && p.Y == pos.Y) || (p.X == pos.X && p.Y < pos.Y) || (p.X - pos.X == pos.Y - p.Y))
                select PieceToIndex[p.Y - 1][p.X - 1].GetValueOrDefault(0);
        }
    }
}