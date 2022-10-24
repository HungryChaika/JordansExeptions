using System;
namespace JordansExceptions
{
    class JordanEX
    {
        //int MaxPossibleRankMatrix = 0;
        public static void Main(string[] args)
        {
            JordanEX jordanEX = new JordanEX();
            UI ui = new UI();
            Console.WriteLine("\nМЕТОД ЖОРДАНОВЫХ ИСКЛЮЧЕНИЙ;\n");
            Console.Write("размерность системы или max n среди всех x^n: ");
            int MaxDegree = ui.CheckAndReadCorrectNumInput();
            Console.Write("количество уравнений в системе: ");
            int NumberEquation = ui.CheckAndReadCorrectNumInput();

            // Массив параметров при x^n
            double[,] Matrix = new double[NumberEquation, MaxDegree];
            // Массив значений после равенства в системах
            double[] Answers = new double[NumberEquation];
            jordanEX.FillArrayCoefficientsAndAnswersSystemEquations(Matrix, Answers);

            bool[] CoefficientsNotTransferredYet = new bool[MaxDegree];
            Array.Fill(CoefficientsNotTransferredYet, true);
            string[] CoefficientsTransferred = new string[NumberEquation];
            Array.Fill(CoefficientsTransferred, "0 ");
            //jordanEX.MaxPossibleRankMatrix = (NumberEquation < MaxDegree) ? NumberEquation : MaxDegree;
            int[] CoordSelectedElem = new int[2];

            //Имеет единственное решение Х1 = 3; Х2 = 2; Х3 = 1
            //double[,] Matrix = { { 1, 3, -4 }, { -1, 1, 1 }, { 2, 1, 1 } };
            //double[] Answers = { 5, 0, 9 };

            //Имеет параметрическое решение Х1 = Z1; X2 = Z2; X3 = 4 - Z1 - 2 * Z2; X4 = 6 - Z1 - Z2; (Система совместна)
            //double[,] Matrix = { { 1, 2, 1, 0 }, { 1, 1, 0, 1 }, { 1, -1, -2, 3 } };
            //double[] Answers = { 4, 6, 10 };

            //Имеет параметрическое решение Х1 = 1 + Z1 - Z2; X2 = Z1; X3 = Z1 + Z2; X4 = Z2; (Количество уравнений больше чем степень)
            //double[,] Matrix = { { 1, 1, -2, 3 }, { 2, -1, -1, 3 } };
            //double[] Answers = { 1, 2 };

            string FinallyAnswer;
            while (true)
            {
                ui.DrawJordanTable(Matrix, CoefficientsNotTransferredYet, CoefficientsTransferred);
                CoordSelectedElem = ui.SelectJordanElem(Matrix, Answers, CoefficientsNotTransferredYet, CoefficientsTransferred);
                jordanEX.StepJordanEx(CoordSelectedElem[0], CoordSelectedElem[1], Matrix, Answers, CoefficientsNotTransferredYet, CoefficientsTransferred);
                FinallyAnswer = jordanEX.FindFinallyAnswer(Matrix, Answers, CoefficientsNotTransferredYet, CoefficientsTransferred);
                if (FinallyAnswer == "nothing")
                {
                    continue;
                }
                else
                {
                    Console.WriteLine("\n\nОтвет:\n" + FinallyAnswer);
                    break;
                }
            }
        }

        void FillArrayCoefficientsAndAnswersSystemEquations(double[,] Elems, double[] Answers)
        {
            Console.WriteLine("\n");
            int NumberEquation = Elems.GetLength(0);
            int MaxDegree = Elems.GetLength(1);
            Console.WriteLine("Заполните систему уравнений:");
            for(int i = 0; i < NumberEquation; i++)
            {
                for(int j = 0; j < MaxDegree; j++)
                {
                    Console.Write($"A{i + 1},{j + 1} * x{j + 1}");   
                    if(j == MaxDegree - 1)
                    {
                        Console.Write($" = A{i + 1},0\n");
                    } else
                    {
                        Console.Write(" + ");
                    }
                }
            }
            for(int i = 0; i < NumberEquation; i++)
            {
                for(int j = 0; j < MaxDegree; j++)
                {
                    Console.Write($"\nA{i + 1},{j + 1} = ");
                    Elems[i,j] = Convert.ToDouble(Console.ReadLine());
                    if (j == MaxDegree - 1)
                    {
                        Console.Write($"\nA{i + 1},0 = ");
                        Answers[i] = Convert.ToDouble(Console.ReadLine());
                    }
                }
            }
        }

        void StepJordanEx(int XCoord, int YCoord, double[,] Elems, double[] Answers, bool[] CoefficientsNotTransferredYet, string[] CoefficientsTransferred)
        {
            int NumberEquation = Elems.GetLength(0);
            int MaxDegree = Elems.GetLength(1);
            double[,] FuncElems = new double[NumberEquation, MaxDegree];
            double[] FuncAnswers = new double[NumberEquation];
            for(int i = 0; i < NumberEquation; i++)
            {
                for(int j = 0; j < MaxDegree; j++)
                {
                    if(i == XCoord - 1)
                    {
                        FuncElems[i, j] = Elems[i, j] / Elems[XCoord - 1, YCoord - 1];
                    } else
                    {
                        FuncElems[i, j] = (Elems[i, j] * Elems[XCoord - 1, YCoord - 1] - Elems[XCoord - 1, j] * Elems[i, YCoord - 1]) / Elems[XCoord - 1, YCoord - 1];
                    }
                }
                if(i == XCoord - 1)
                {
                    FuncAnswers[i] = Answers[i] / Elems[XCoord - 1, YCoord - 1];
                } else
                {
                    FuncAnswers[i] = (Answers[i] * Elems[XCoord - 1, YCoord - 1] - Answers[XCoord - 1] * Elems[i, YCoord - 1]) / Elems[XCoord - 1, YCoord - 1];
                }
            }
            CoefficientsNotTransferredYet[YCoord - 1] = false;
            CoefficientsTransferred[XCoord - 1] = $"X{YCoord}";
            Array.Copy(FuncElems, Elems, FuncElems.Length);
            Array.Copy(FuncAnswers, Answers, FuncAnswers.Length);
        }

        String FindFinallyAnswer(double[,] Elems, double[] Answers, bool[] CoefficientsNotTransferredYet, string[] CoefficientsTransferred)
        {
            int NumberEquation = Elems.GetLength(0);
            int MaxDegree = Elems.GetLength(1);
            int CounterUniqueAnswer = 0;
            for (int k = 0; k < MaxDegree; k++)
            {
                CounterUniqueAnswer += (CoefficientsNotTransferredYet[k]) ? 0 : 1;
            }
            if (CounterUniqueAnswer == MaxDegree)
            {
                string Str = "";
                for(int k = 0; k < NumberEquation; k++)
                {
                    Str += $"{CoefficientsTransferred[k]}= {Answers[k]}\n";
                }
                return Str;
            }
            int CounterParametricAnswer = 0;
            int NumAbsolutelyZeroRows = 0;
            int NumZeroRowsWithoutAnswerColumn = 0;
            for (int i = 0; i < NumberEquation; i++)
            {
                if (CoefficientsTransferred[i] == "0 ")
                {
                    CounterParametricAnswer++;
                    for (int j = 0; j < MaxDegree; j++)
                    {
                        if (Elems[i, j] != 0 && CoefficientsNotTransferredYet[j])
                        {
                            return "nothing";
                        }
                    }
                    NumZeroRowsWithoutAnswerColumn++;
                    NumAbsolutelyZeroRows += (Answers[i] == 0) ? 1 : 0;
                }
            }
            if (CounterParametricAnswer == 0 || NumAbsolutelyZeroRows == NumZeroRowsWithoutAnswerColumn)
            {
                string Str = "\n";
                int CounterElemNum = 1;
                for(int k = 0; k < MaxDegree; k++)
                {
                    if (CoefficientsNotTransferredYet[k]) 
                    {
                        Str += $"X{k + 1} = Z{CounterElemNum}\n";
                        CounterElemNum++;
                    }
                }
                for (int i = 0; i < NumberEquation; i++)
                {
                    if (CoefficientsTransferred[i] != "0 ")
                    {
                        CounterElemNum = 1;
                        Str += $"{CoefficientsTransferred[i]} = {Answers[i]}";
                        for (int j = 0; j < MaxDegree; j++)
                        {
                            if (CoefficientsNotTransferredYet[j])
                            {
                                Str += $" + ({Elems[i, j]}) * (-Z{CounterElemNum})";
                                CounterElemNum++;
                            }
                        }
                        Str += "\n";
                    }
                }
                return Str;
            }       
            return "nothing";
        }
    }
} 