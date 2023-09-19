using System.Drawing;
using System.Windows.Forms;
using System;

namespace ChessServerTest
{
    public class ChessGame : Form
    {
        public Image chessSprites;
        public int[,] map = new int[8, 8]
        {
        {15,14,13,12,11,13,14,15 },
        {16,16,16,16,16,16,16,16 },
        {0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0 },
        {26,26,26,26,26,26,26,26 },
        {25,24,23,22,21,23,24,25 },
        };

        public Button[,] butts = new Button[8, 8];

        public int currPlayer;

        public Button prevButton;
        private bool isKingUnderCheck = false;
        public bool isMoving = false;
        /*        public bool isFirstMove = true;*/

        public ChessGame()
        {
            InitializeComponent();
            chessSprites = new Bitmap("C:\\Users\\marku\\Desktop\\Chess\\Sprites\\chess.png");
            Init();
        }

        public void Init()
        {
            map = new int[8, 8]
            {
            {15,14,13,12,11,13,14,15 },
            {16,16,16,16,16,16,16,16 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {26,26,26,26,26,26,26,26 },
            {25,24,23,22,21,23,24,25 },
            };

            currPlayer = 1;
            CreateMap();
        }


        public void CreateMap()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j] = new Button();

                    Button butt = new Button();
                    butt.Size = new Size(50, 50);
                    butt.Location = new Point(j * 50, i * 50);

                    switch (map[i, j] / 10)
                    {
                        case 1:
                            Image part = new Bitmap(50, 50);
                            Graphics g = Graphics.FromImage(part);
                            g.DrawImage(chessSprites, new Rectangle(0, 0, 50, 50), 0 + 150 * (map[i, j] % 10 - 1), 0, 150, 150, GraphicsUnit.Pixel);
                            butt.BackgroundImage = part;
                            break;
                        case 2:
                            Image part1 = new Bitmap(50, 50);
                            Graphics g1 = Graphics.FromImage(part1);
                            g1.DrawImage(chessSprites, new Rectangle(0, 0, 50, 50), 0 + 150 * (map[i, j] % 10 - 1), 150, 150, 150, GraphicsUnit.Pixel);
                            butt.BackgroundImage = part1;
                            break;
                    }
                    butt.BackColor = Color.White;
                    butt.Click += new EventHandler(OnFigurePress);
                    this.Controls.Add(butt);

                    butts[i, j] = butt;
                }
            }
        }

        public bool IsMoveSafe(int fromRow, int fromCol, int toRow, int toCol)
        {
            int temp = map[toRow, toCol];
            map[toRow, toCol] = map[fromRow, fromCol];
            map[fromRow, fromCol] = 0;


            bool isSafe = !IsKingInCheck(currPlayer);

            map[fromRow, fromCol] = map[toRow, toCol];
            map[toRow, toCol] = temp;

            return isSafe;
        }

        public void OnFigurePress(object sender, EventArgs e)
        {
            if (prevButton != null)
                prevButton.BackColor = Color.White;

            Button pressedButton = sender as Button;
            int iCurrent = pressedButton.Location.Y / 50;
            int jCurrent = pressedButton.Location.X / 50;

            if (map[iCurrent, jCurrent] != 0 && map[iCurrent, jCurrent] / 10 == currPlayer)
            {
                if (!isMoving)
                {
                    CloseSteps();
                    pressedButton.BackColor = Color.Red;
                    DeactivateAllButtons();
                    pressedButton.Enabled = true;
                    ShowSteps(iCurrent, jCurrent, map[iCurrent, jCurrent]);
                    isMoving = true;
                }
                else
                {
                    isMoving = false;
                    CloseSteps();
                    ActivateAllButtons();
                }
            }
            else
            {
                if (isMoving)
                {
                    int iPrev = prevButton.Location.Y / 50;
                    int jPrev = prevButton.Location.X / 50;
                    int prevPiece = map[iPrev, jPrev];


                    if (IsValidMove(iPrev, jPrev, iCurrent, jCurrent, prevPiece))
                    {
                        map[iCurrent, jCurrent] = prevPiece;
                        map[iPrev, jPrev] = 0;
                        pressedButton.BackgroundImage = prevButton.BackgroundImage;
                        prevButton.BackgroundImage = null;


                        bool isChecking = IsKingInCheck(currPlayer);

                        if (!isChecking)
                        {
                            SwitchPlayer();
                        }
                        //else
                        //{
                        //    map[iPrev, jPrev] = prevPiece;
                        //    map[iCurrent, jCurrent] = 0;
                        //    pressedButton.BackgroundImage = null;
                        //    prevButton.BackgroundImage = pressedButton.BackgroundImage;
                        //    MessageBox.Show("Ход недопустим, так как король находится под угрозой!");
                        //}

                        isMoving = false;
                        CloseSteps();
                        ActivateAllButtons();
                    }
                    else
                    {
                        MessageBox.Show("Ход недопустим!");
                    }
                }
            }

            prevButton = pressedButton;
            bool isCheck = IsKingInCheck(currPlayer);
            bool isCheckmate = IsCheckmate(currPlayer);

            //if (isCheckmate)
            //{
            //    MessageBox.Show("Мат!");
            //}
            //else if (isCheck)
            //{
            //    MessageBox.Show("Шах!");
            //}
        }
        private bool IsValidMove(int iFrom, int jFrom, int iTo, int jTo, int piece)
        {

            if (!InsideBorder(iFrom, jFrom) || !InsideBorder(iTo, jTo))
            {
                return false;
            }


            if (map[iFrom, jFrom] != piece)
            {
                return false;
            }


            if (map[iTo, jTo] != 0 && map[iTo, jTo] / 10 == currPlayer)
            {
                return false;
            }


            int dx = Math.Abs(jTo - jFrom);
            int dy = Math.Abs(iTo - iFrom);

            switch (piece % 10)
            {
                case 6: // Пешка
                    int dir = currPlayer == 1 ? 1 : -1;


                    if (dx == 0 && dy == 1 && map[iTo, jTo] == 0)
                    {
                        return true;
                    }


                    if (dx == 0 && dy == 2 && isFirstMove(iFrom, jFrom, piece) && map[iTo, jTo] == 0)
                    {
                        return true;
                    }


                    if (dx == 1 && dy == 1 && map[iTo, jTo] != 0 && map[iTo, jTo] / 10 != currPlayer)
                    {
                        return true;
                    }


                    return false;

                case 5: // Ладья
                    if ((dx == 0 || dy == 0) && IsPathClear(iFrom, jFrom, iTo, jTo))
                    {
                        return true;
                    }
                    return false;

                case 3: // Слон
                    if (dx == dy && IsPathClear(iFrom, jFrom, iTo, jTo))
                    {
                        return true;
                    }
                    return false;

                case 2: // Ферзь
                    if ((dx == 0 || dy == 0 || dx == dy) && IsPathClear(iFrom, jFrom, iTo, jTo))
                    {
                        return true;
                    }
                    return false;

                case 1: // Король
                    if (dx <= 1 && dy <= 1)
                    {
                        return true;
                    }
                    return false;

                case 4: // Конь
                    if ((dx == 1 && dy == 2) || (dx == 2 && dy == 1))
                    {
                        return true;
                    }
                    return false;

                default:
                    return false;
            }
        }
        private bool IsPathClear(int iFrom, int jFrom, int iTo, int jTo)
        {
            int dx = Math.Sign(jTo - jFrom);
            int dy = Math.Sign(iTo - iFrom);

            int currentRow = iFrom + dy;
            int currentCol = jFrom + dx;

            while (currentRow != iTo || currentCol != jTo)
            {
                if (map[currentRow, currentCol] != 0)
                {
                    return false;
                }

                currentRow += dy;
                currentCol += dx;
            }

            return true;
        }
        private bool isFirstMove(int IcurrFigure, int JcurrFigure, int currFigure)
        {
            int dir = currPlayer == 1 ? 1 : -1;

            if ((dir == 1 && IcurrFigure == 1 && currFigure == 16) || (dir == -1 && IcurrFigure == 6 && currFigure == 26))
            {
                return true;
            }

            return false;
        }
        public void ShowSteps(int IcurrFigure, int JcurrFigure, int currFigure)
        {
            int dir = currPlayer == 1 ? 1 : -1;
            switch (currFigure % 10)
            {
                case 6:
                    if (InsideBorder(IcurrFigure + 1 * dir, JcurrFigure))
                    {
                        if (map[IcurrFigure + 1 * dir, JcurrFigure] == 0)
                        {
                            butts[IcurrFigure + 1 * dir, JcurrFigure].BackColor = Color.Yellow;
                            butts[IcurrFigure + 1 * dir, JcurrFigure].Enabled = true;

                            if (isFirstMove(IcurrFigure, JcurrFigure, currFigure) && InsideBorder(IcurrFigure + 2 * dir, JcurrFigure) && map[IcurrFigure + 2 * dir, JcurrFigure] == 0)
                            {
                                butts[IcurrFigure + 2 * dir, JcurrFigure].BackColor = Color.Yellow;
                                butts[IcurrFigure + 2 * dir, JcurrFigure].Enabled = true;
                            }
                        }
                    }

                    if (InsideBorder(IcurrFigure + 1 * dir, JcurrFigure + 1))
                    {
                        if (map[IcurrFigure + 1 * dir, JcurrFigure + 1] != 0 && map[IcurrFigure + 1 * dir, JcurrFigure + 1] / 10 != currPlayer)
                        {
                            butts[IcurrFigure + 1 * dir, JcurrFigure + 1].BackColor = Color.Yellow;
                            butts[IcurrFigure + 1 * dir, JcurrFigure + 1].Enabled = true;
                        }
                    }
                    if (InsideBorder(IcurrFigure + 1 * dir, JcurrFigure - 1))
                    {
                        if (map[IcurrFigure + 1 * dir, JcurrFigure - 1] != 0 && map[IcurrFigure + 1 * dir, JcurrFigure - 1] / 10 != currPlayer)
                        {
                            butts[IcurrFigure + 1 * dir, JcurrFigure - 1].BackColor = Color.Yellow;
                            butts[IcurrFigure + 1 * dir, JcurrFigure - 1].Enabled = true;
                        }
                    }
                    break;
                case 5:
                    ShowVerticalHorizontal(IcurrFigure, JcurrFigure);
                    break;
                case 3:
                    ShowDiagonal(IcurrFigure, JcurrFigure);
                    break;
                case 2:
                    ShowVerticalHorizontal(IcurrFigure, JcurrFigure);
                    ShowDiagonal(IcurrFigure, JcurrFigure);
                    break;
                case 1:
                    ShowVerticalHorizontal(IcurrFigure, JcurrFigure, true);
                    ShowDiagonal(IcurrFigure, JcurrFigure, true);
                    break;
                case 4:
                    ShowHorseSteps(IcurrFigure, JcurrFigure);
                    break;
            }
        }

        public void ShowHorseSteps(int IcurrFigure, int JcurrFigure)
        {
            if (InsideBorder(IcurrFigure - 2, JcurrFigure + 1))
            {
                DeterminePath(IcurrFigure - 2, JcurrFigure + 1);
            }
            if (InsideBorder(IcurrFigure - 2, JcurrFigure - 1))
            {
                DeterminePath(IcurrFigure - 2, JcurrFigure - 1);
            }
            if (InsideBorder(IcurrFigure + 2, JcurrFigure + 1))
            {
                DeterminePath(IcurrFigure + 2, JcurrFigure + 1);
            }
            if (InsideBorder(IcurrFigure + 2, JcurrFigure - 1))
            {
                DeterminePath(IcurrFigure + 2, JcurrFigure - 1);
            }
            if (InsideBorder(IcurrFigure - 1, JcurrFigure + 2))
            {
                DeterminePath(IcurrFigure - 1, JcurrFigure + 2);
            }
            if (InsideBorder(IcurrFigure + 1, JcurrFigure + 2))
            {
                DeterminePath(IcurrFigure + 1, JcurrFigure + 2);
            }
            if (InsideBorder(IcurrFigure - 1, JcurrFigure - 2))
            {
                DeterminePath(IcurrFigure - 1, JcurrFigure - 2);
            }
            if (InsideBorder(IcurrFigure + 1, JcurrFigure - 2))
            {
                DeterminePath(IcurrFigure + 1, JcurrFigure - 2);
            }
        }

        public void DeactivateAllButtons()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j].Enabled = false;
                }
            }
        }

        public void ActivateAllButtons()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j].Enabled = true;
                }
            }
        }

        public void ShowDiagonal(int IcurrFigure, int JcurrFigure, bool isOneStep = false)
        {
            int j = JcurrFigure + 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure + 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }
        }
        public Tuple<int, int> FindKing(int player)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (map[i, j] == (player == 1 ? 15 : 25))
                    {
                        return Tuple.Create(i, j);
                    }
                }
            }
            return null;
        }
        public bool IsKingInCheck(int player)
        {
            Tuple<int, int> kingPosition = FindKing(player);
            int kingRow = kingPosition.Item1;
            int kingCol = kingPosition.Item2;


            int pawnDirection = (player == 1) ? 1 : -1;
            if (InsideBorder(kingRow + pawnDirection, kingCol - 1) && map[kingRow + pawnDirection, kingCol - 1] % 10 == 6 && map[kingRow + pawnDirection, kingCol - 1] / 10 != player)
            {
                return true;
            }
            if (InsideBorder(kingRow + pawnDirection, kingCol + 1) && map[kingRow + pawnDirection, kingCol + 1] % 10 == 6 && map[kingRow + pawnDirection, kingCol + 1] / 10 != player)
            {
                return true;
            }

            int[] horizontalRows = { -1, 1, 0, 0 };
            int[] horizontalCols = { 0, 0, -1, 1 };
            for (int d = 0; d < 4; d++)
            {
                for (int dist = 1; dist < 8; dist++)
                {
                    int newRow = kingRow + dist * horizontalRows[d];
                    int newCol = kingCol + dist * horizontalCols[d];
                    if (InsideBorder(newRow, newCol))
                    {
                        int piece = map[newRow, newCol];
                        if (piece != 0)
                        {
                            if (piece / 10 != player && (piece % 10 == 5 || piece % 10 == 1))
                            {

                                return true;
                            }
                            else
                            {

                                break;
                            }
                        }
                    }
                    else
                    {

                        break;
                    }
                }
            }


            int[] diagonalRows = { -1, -1, 1, 1 };
            int[] diagonalCols = { -1, 1, -1, 1 };
            for (int d = 0; d < 4; d++)
            {
                for (int dist = 1; dist < 8; dist++)
                {
                    int newRow = kingRow + dist * diagonalRows[d];
                    int newCol = kingCol + dist * diagonalCols[d];
                    if (InsideBorder(newRow, newCol))
                    {
                        int piece = map[newRow, newCol];
                        if (piece != 0)
                        {
                            if (piece / 10 != player && (piece % 10 == 3 || piece % 10 == 1))
                            {

                                return true;
                            }
                            else
                            {

                                break;
                            }
                        }
                    }
                    else
                    {

                        break;
                    }
                }
            }


            int[] knightRows = { -2, -1, 1, 2, 2, 1, -1, -2 };
            int[] knightCols = { 1, 2, 2, 1, -1, -2, -2, -1 };
            for (int i = 0; i < 8; i++)
            {
                int newRow = kingRow + knightRows[i];
                int newCol = kingCol + knightCols[i];
                if (InsideBorder(newRow, newCol) && map[newRow, newCol] % 10 == 4 && map[newRow, newCol] / 10 != player)
                {

                    return true;
                }
            }


            return false;
        }
        public bool IsCheckmate(int player)
        {
            Tuple<int, int> kingPosition = FindKing(player);
            int kingRow = kingPosition.Item1;
            int kingCol = kingPosition.Item2;


            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    if (dr == 0 && dc == 0)
                        continue;

                    int newRow = kingRow + dr;
                    int newCol = kingCol + dc;


                    if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8)
                    {

                        if (!IsSquareUnderAttack(newRow, newCol, player))
                        {
                            return false;
                        }
                    }
                }
            }


            return true;
        }
        public bool IsSquareUnderAttack(int row, int col, int attackingPlayer)
        {
            int[] dr = { -1, -1, 1, 1, -1, 0, 1, 0, 0, -1, 0, 1, -1, -1, 1, 1 };
            int[] dc = { -1, 1, -1, 1, 0, -1, 0, 1, -1, 0, 1, 0, -1, 1, -1, 1 };

            int[] linearDr = { -1, 1, 0, 0 };
            int[] linearDc = { 0, 0, -1, 1 };


            int pawnDirection = (attackingPlayer == 1) ? 1 : -1;
            if (InsideBorder(row + pawnDirection, col - 1) && map[row + pawnDirection, col - 1] == 16 + 10 * attackingPlayer)
            {
                return true;
            }
            if (InsideBorder(row + pawnDirection, col + 1) && map[row + pawnDirection, col + 1] == 16 + 10 * attackingPlayer)
            {
                return true;
            }


            int[] knightRowMoves = { -2, -2, -1, -1, 1, 1, 2, 2 };
            int[] knightColMoves = { -1, 1, -2, 2, -2, 2, -1, 1 };
            for (int i = 0; i < 8; i++)
            {
                int newRow = row + knightRowMoves[i];
                int newCol = col + knightColMoves[i];
                if (InsideBorder(newRow, newCol) && map[newRow, newCol] == 24 + 10 * attackingPlayer)
                {
                    return true;
                }
            }


            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < 8; j++)
                {
                    int newRow = row + j * dr[i];
                    int newCol = col + j * dc[i];
                    if (!InsideBorder(newRow, newCol))
                    {
                        break;
                    }

                    int piece = map[newRow, newCol];
                    if (piece == 0)
                    {
                        continue;
                    }
                    else if (piece / 10 == attackingPlayer)
                    {
                        break;
                    }
                    else if ((i == 0 && piece % 10 == 3) || (i == 1 && piece % 10 == 3))
                    {
                        return true;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            for (int i = 4; i < 8; i++)
            {
                for (int j = 1; j < 8; j++)
                {
                    int newRow = row + j * linearDr[i - 4];
                    int newCol = col + j * linearDc[i - 4];
                    if (!InsideBorder(newRow, newCol))
                    {
                        break;
                    }

                    int piece = map[newRow, newCol];
                    if (piece == 0)
                    {
                        continue;
                    }
                    else if (piece / 10 == attackingPlayer)
                    {
                        break;
                    }
                    else if ((i == 4 && piece % 10 == 2) || (i == 5 && piece % 10 == 2) || (i == 6 && piece % 10 == 1) || (i == 7 && piece % 10 == 1))
                    {
                        return true;
                    }
                    else
                    {
                        break;
                    }
                }
            }



            return false;
        }
        public void ShowVerticalHorizontal(int IcurrFigure, int JcurrFigure, bool isOneStep = false)
        {
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (InsideBorder(i, JcurrFigure))
                {
                    if (!DeterminePath(i, JcurrFigure))
                        break;
                }
                if (isOneStep)
                    break;
            }
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (InsideBorder(i, JcurrFigure))
                {
                    if (!DeterminePath(i, JcurrFigure))
                        break;
                }
                if (isOneStep)
                    break;
            }
            for (int j = JcurrFigure + 1; j < 8; j++)
            {
                if (InsideBorder(IcurrFigure, j))
                {
                    if (!DeterminePath(IcurrFigure, j))
                        break;
                }
                if (isOneStep)
                    break;
            }
            for (int j = JcurrFigure - 1; j >= 0; j--)
            {
                if (InsideBorder(IcurrFigure, j))
                {
                    if (!DeterminePath(IcurrFigure, j))
                        break;
                }
                if (isOneStep)
                    break;
            }
        }

        public bool DeterminePath(int IcurrFigure, int j)
        {
            if (map[IcurrFigure, j] == 0)
            {
                butts[IcurrFigure, j].BackColor = Color.Yellow;
                butts[IcurrFigure, j].Enabled = true;
            }
            else
            {
                if (map[IcurrFigure, j] / 10 != currPlayer)
                {
                    butts[IcurrFigure, j].BackColor = Color.Yellow;
                    butts[IcurrFigure, j].Enabled = true;
                }
                return false;
            }
            return true;
        }

        public bool InsideBorder(int ti, int tj)
        {
            if (ti >= 8 || tj >= 8 || ti < 0 || tj < 0)
                return false;
            return true;
        }

        public void CloseSteps()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j].BackColor = Color.White;
                }
            }
        }

        public void SwitchPlayer()
        {
            if (currPlayer == 1)
                currPlayer = 2;
            else currPlayer = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            Init();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ChessGame
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "ChessGame";
            this.ResumeLayout(false);

        }
    }
}