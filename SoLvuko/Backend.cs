using System;
using System.Collections.Generic;
using System.Text;

namespace SoLvuko
{
    class SodukuCell
    {
        public int Value
        { get; set; }
        public bool isEmpty
        { get => Value == 0; }
        public IList<int> Possibilities
        { get => possibilities; }
        private static Random rng = new Random();
        public static void Shuffle(IList<int> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                int value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        List<int> possibilities = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        public SodukuCell(int val)
        {
            Value = val; possibilities.Remove(val);
            Shuffle(this.possibilities);
        }
    }
    class SodukuApi
    {
        public List<List<SodukuCell>> sodukoBoard
        { get; }
        int size;
        public SodukuApi(int size)
        {
            this.size = size;
            this.sodukoBoard = new List<List<SodukuCell>>(size);
            for (int i = 0; i < size; i++)
            {
                sodukoBoard.Add(new List<SodukuCell>(size));
                for (int j = 0; j < size; j++)
                {
                    sodukoBoard[i].Add(new SodukuCell(0));
                }
            }
        }
        public SodukuApi(List<List<int>> list)
        {
            this.size = list.Count;
            this.sodukoBoard = new List<List<SodukuCell>>(size);
            for (int i = 0; i < size; i++)
            {
                sodukoBoard.Add(new List<SodukuCell>(size));
                for (int j = 0; j < size; j++)
                {
                    sodukoBoard[i].Add(new SodukuCell(0));
                }
            }
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    sodukoBoard[i][j].Value = list[i][j];
                }
            }
        }
        public bool solve()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (sodukoBoard[i][j].isEmpty)
                    {
                        foreach (int value in sodukoBoard[i][j].Possibilities)
                        {
                            if (isCorrect(i, j, value))
                            {
                                sodukoBoard[i][j].Value = value;
                                if (solve())
                                {
                                    return true;
                                }
                                else
                                {
                                    sodukoBoard[i][j].Value = 0;
                                }
                                Console.WriteLine(this.ToString());
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        bool isCorrect(int i, int j, int val)
        {
            int regionSize = (int)Math.Sqrt(size);
            for (int k = 0; k < size; k++)
            {
                if (sodukoBoard[i][k].Value == val) return false;
            }
            for (int k = 0; k < size; k++)
            {
                if (sodukoBoard[k][j].Value == val) return false;
            }
            int regionStarty = i - i % regionSize;
            int regionStartx = j - j % regionSize;
            for (int k = regionStarty; k < regionStarty + regionSize; k++)
            {
                for (int n = regionStartx; n < regionStartx + regionSize; n++)
                {
                    if (k != i && n != j && sodukoBoard[k][n].Value == val) return false;
                }
            }
            return true;
        }
        public override string ToString()
        {
            string s = "===========================\n";
            for (int i = 0; i < size; i++)
            {
                s += "|";
                for (int j = 0; j < size; j++)
                {
                    s += sodukoBoard[i][j].Value.ToString() + "|";
                }
                s += "\n";
                s += "---------------------------\n";
            }
            s += "===========================\n";
            return s;
        }
    }
}
