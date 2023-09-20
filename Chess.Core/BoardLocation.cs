using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class BoardLocation
    {
        private const int BoardSize = 8;

        public int Row { get; set; }
        public int Column { get; set; }

        private static bool IsInRange(int pos)
        {
            return (pos >= 1) && (pos <= BoardSize);
        }

        public BoardLocation(int row, int col)
        {
            Row = row;
            Column = col;
        }

        [JsonConstructor]
        public BoardLocation() { }


        public override string ToString()
        {
            return $"{Row},{Column}";
        }
    }
}
