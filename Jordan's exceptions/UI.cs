using System;
namespace JordansExceptions
{
    public class UI
    {
        public int CheckAndReadCorrectNumInput(int UpperLimit = Int32.MaxValue, int LowerLimit = 1)
        {
            int Elem = 0;
            while (true)
            {
                try
                {
                    Elem = Convert.ToInt32(Console.ReadLine());
                    if (Elem < LowerLimit)
                    {
                        Console.WriteLine($"Значение мало. Введите значение >= {LowerLimit}.");
                    }
                    else if (Elem > UpperLimit)
                    {
                        Console.WriteLine($"Значение велико. Введите значение <= {UpperLimit}.");
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Значение некоректно Введите значение заново.");
                }
            }
            return Elem;
        }

        public void DrawJordanTable(double[,] Elems, bool[] CoefficientsNotTransferredYet, string[] CoefficientsTransferred)
        {
            Console.Write("\n\n");
            int NumberEquation = Elems.GetLength(0);
            int MaxDegree = Elems.GetLength(1);
            for (int i = -1; i < NumberEquation; i++)
            {
                Console.Write("| ");
                for (int j = -2; j < MaxDegree; j++)
                {
                    if (i == -1)
                    {
                        if (j == -2)
                        {
                            Console.Write("   ");
                        }
                        else if (j == -1)
                        {
                            Console.Write(" 1 ");
                        }
                        else
                        {
                            if (CoefficientsNotTransferredYet[j])
                            {
                                Console.Write($"-X{j + 1}");
                            }
                            else
                            {
                                Console.Write(" 0 ");
                            }
                        }
                    }
                    else
                    {
                        if (j == -2)
                        {
                            Console.Write($"{CoefficientsTransferred[i]}=");
                        }
                        else if (j == -1)
                        {
                            Console.Write($"A{i + 1}0");
                        }
                        else
                        {
                            Console.Write($"A{i + 1}{j + 1}");
                        }
                    }
                    Console.Write(" | ");
                }
                if (i == -1)
                {
                    Console.Write("\n");
                }
                Console.Write("\n");
            }
        }

        public int[] SelectJordanElem(double[,] Elems, double[] Answers, bool[] CoefficientsNotTransferredYet, string[] CoefficientsTransferred)
        {
            int NumberEquation = Elems.GetLength(0);
            int MaxDegree = Elems.GetLength(1);
            Console.WriteLine("\n\nЗначения ответов:");
            for (int i = 0; i < NumberEquation; i++)
            {
                Console.WriteLine($"A{i + 1}0 = {Answers[i]}");
            }
            Console.WriteLine("\nВыберите элемент, написав его номер:");
            for (int i = 0; i < NumberEquation; i++)
            {
                for (int j = 0; j < MaxDegree; j++)
                {
                    if (CoefficientsNotTransferredYet[j])
                    {
                        Console.Write($"{i * MaxDegree + j + 1}) A{i + 1}{j + 1} = {Elems[i, j]};   ");
                    }
                    else
                    {
                        Console.Write($"{i * MaxDegree + j + 1}) A{i + 1}{j + 1} = Столбец недоступен;   ");
                    }
                }
                Console.Write("\n");
            }
            while (true)
            {
                Console.Write("\nВаш выбор: ");
                double AnswerNum = (double)CheckAndReadCorrectNumInput(NumberEquation * MaxDegree);
                int PositionI = (int)(AnswerNum / MaxDegree - (AnswerNum / MaxDegree) % 1 + (((AnswerNum / MaxDegree) % 1 == 0) ? 0 : 1));
                int PositionJ = (int)(AnswerNum - MaxDegree * (PositionI - 1));
                if (Elems[PositionI - 1, PositionJ - 1] == 0)
                {
                    Console.WriteLine("\nВыберите число не равное нолю!");
                    continue;
                }
                else if (CoefficientsTransferred[PositionI - 1] != "0 ")
                {
                    Console.WriteLine("\nВыберите число, находящееся в строке, которую ещё не выбирали!");
                    continue;
                }
                else if (!CoefficientsNotTransferredYet[PositionJ - 1])
                {
                    Console.WriteLine("\nСтолбец недоступен!");
                    continue;
                }
                return new int[2] { PositionI, PositionJ };
            }
        }

        public void MatrixWrite(double[,] Matrix)
        {
            Console.WriteLine("\n\n");
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    Console.Write(Matrix[i, j] + "   ");
                }
                Console.WriteLine("\n");
            }
            Console.WriteLine("\nДлина: " + Matrix.Length + "\n");
        }
    }
}
