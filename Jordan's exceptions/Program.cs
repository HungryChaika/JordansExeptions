using System;
namespace JordansExeptions
{
    class JordanEX
    {
        int MaxPossibleRankMatrix = 0;
        public static void Main(string[] args)
        {
            JordanEX jordanEX = new JordanEX();
            Console.WriteLine("Вы попали, перед вами наследие Жордана;");
            Console.WriteLine("Введите размерность системы и количество уравнений в системе;");
            Console.Write("размерность системы или max n среди всех x^n: ");
            int MaxDegree = jordanEX.CheckAndReadCorrectNumInput();
            Console.Write("количество уравнений в системе: ");
            int NumberEquation = jordanEX.CheckAndReadCorrectNumInput();

            // Массив параметров при x^n
            double[,] Matrix = new double[NumberEquation, MaxDegree];
            // Массив значений после равенства в системах
            double[] Answers = new double[NumberEquation];
            jordanEX.FillArrayCoefficientsAndAnswersSystemEquations(Matrix, Answers);

            bool[] CoefficientsNotTransferredYet = new bool[MaxDegree];
            Array.Fill(CoefficientsNotTransferredYet, true);
            string[] CoefficientsTransferred = new string[NumberEquation];
            Array.Fill(CoefficientsTransferred, "0 ");
            jordanEX.MaxPossibleRankMatrix = (NumberEquation < MaxDegree) ? NumberEquation : MaxDegree;
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
                jordanEX.DrawJordanTable(Matrix, CoefficientsNotTransferredYet, CoefficientsTransferred);
                CoordSelectedElem = jordanEX.SelectJordanElem(Matrix, Answers, CoefficientsNotTransferredYet, CoefficientsTransferred);
                jordanEX.StepJordanEx(CoordSelectedElem[0], CoordSelectedElem[1], Matrix, Answers, CoefficientsNotTransferredYet, CoefficientsTransferred);
                FinallyAnswer = jordanEX.FindFinallyAnswer(Matrix, Answers, CoefficientsNotTransferredYet, CoefficientsTransferred);
                if (FinallyAnswer == "nothing")
                {
                    continue;
                }
                else
                {
                    Console.WriteLine("\nОтвет:\n" + FinallyAnswer);
                    break;
                }
            }
            // ПРОВЕРЬ ПОТОМ СИТУАЦИЮ, КОГДА КОЛИЧЕСТВО УРАВНЕНИЙ БОЛЬШЕ МАКСИМАЛЬНОЙ СТЕПЕНИ !


            // ---------------------------- Tests ----------------------------


            //Console.WriteLine("\n\n");

            //double[,] Matrix = { { 1.0, 2.0 }, { 3.0, 4.0 } }; // det = -2
            //double[,] Matrix = { { -1.0, 2.0 , 7.0 }, { 4.0, 8.0, 6.0 }, { 3.0, 5.0, 9.0 } }; // det = -106
            //double[,] Matrix = {
            //    { 1.0, 2.0, 3.0, 4.0 }, { 5.0, 6.0, 7.0, 8.0 }, { 9.0, 10.0, 11.0, 12.0 }, { 13.0, 14.0, 15.0, 16.0 }
            //}; // det = 0
            //double[,] Matrix = {
            //    { -3.0, 0.0, 0.0, 0.0 }, { 28.0, 1.0, 0.0, 0.0 }, { -67.0, 83.0, 2.0, 0.0 }, { 19.0, 47.0, -35.0, -4.0 }
            //}; // det = 24
            //double[,] Matrix = { { -1.0, 2.0, 7.0 / 3.2, -2.0 }, { 4.0, 8.0, 6.0, 3.0 }, { 3.0, 5.0, 9.0, 6.0 } };
            //double[,] Matrix = { { -1.0, 2.0, 7.0 }, { 4.0, 8.0, 6.0 }, { 3.0, 5.0, 9.0 }, { -4.0, 0.0, 2.0 } };

            //int RankMatrix = jordanEX.CalculateRankMatrix(Matrix);
            //Console.WriteLine(RankMatrix);

            //double DeterminantMatrix = jordanEX.CalculateDeterminantMatrix(Matrix);
            //Console.WriteLine(DeterminantMatrix);

            //double[,] Answer = jordanEX.ComposeMinorElemMatrix(Matrix, 0, 0);
            //for (int i = 0; i < Answer.GetLength(0); i++)
            //{
            //    for (int j = 0; j < Answer.GetLength(1); j++)
            //    {
            //        Console.WriteLine(Answer[i, j]);
            //    }
            //}
            //Console.WriteLine("Длина: " + Answer.Length);
        }

    
        int CalculateRankMatrix(double[,] Matrix, int CurrentRankMatrix = 0) //Не доделанный
        {
            int Rows = Matrix.GetLength(0);
            int Colums = Matrix.GetLength(1);
            int MaxLimit = (Rows < Colums) ? Rows : Colums;

            if(CurrentRankMatrix > MaxLimit)
            {
                return MaxLimit;
            }
            if(CurrentRankMatrix == 0)
            {
                for(int i = 0; i < Rows; i++)
                {
                    for(int j = 0; j < Colums; j++)
                    {
                        if (Matrix[i,j] != 0)
                        {
                            CalculateRankMatrix(Matrix, 1);
                        } else
                        {
                            return CurrentRankMatrix;
                        }
                    }
                }
            }
            if(CurrentRankMatrix == 1)
            {
                for(int i = 0; i < Rows; i++)
                {
                    for(int j = 0; j < Colums; j++)
                    {

                    }
                }
            }

            return CurrentRankMatrix;
        }

        double CalculateDeterminantMatrix(double[,] Matrix) // Почти Готов (корректировки) -> В целом Вроде всё!!!
        {
            int SizeMatrix = Matrix.GetLength(0);
            if (SizeMatrix == 1)
            {
                return Matrix[0, 0];
            } else
            {
                double AnswerFunc = 0;
                for (int j = 0; j < SizeMatrix; j++)
                {
                    AnswerFunc +=
                        (double)(Matrix[0, j] * Math.Pow(-1, 0 + j + 2) * CalculateDeterminantMatrix(ComposeMinorElemMatrix(Matrix, 0, j)));
                }
                return AnswerFunc;
            }
        }

        double[,] ComposeMinorElemMatrix(double[,] Matrix, int RowPosition, int ColumnPosition) // Почти Готов (корректировки) -> В целом Вроде всё!!!
        {
            int SizeMatrixRow = Matrix.GetLength(0);
            int SizeMatrixColumn = Matrix.GetLength(1);
            double[,] AnswerFunc = new double[SizeMatrixRow - 1, SizeMatrixColumn - 1];
            for (int i = 0; i < SizeMatrixRow; i++)
            {
                for(int j = 0; j < SizeMatrixColumn; j++)
                {
                    if(i < RowPosition)
                    {
                        if(j < ColumnPosition)
                        {
                            AnswerFunc[i, j] = Matrix[i, j];
                        } else if(j > ColumnPosition)
                        {
                            AnswerFunc[i, j - 1] = Matrix[i, j];
                        }
                    } else if(i > RowPosition)
                    {
                        if (j < ColumnPosition)
                        {
                            AnswerFunc[i - 1, j] = Matrix[i, j];
                        }
                        else if (j > ColumnPosition)
                        {
                            AnswerFunc[i - 1, j - 1] = Matrix[i, j];
                        }
                    }
                }
            }
            return AnswerFunc;
        }

        int CheckAndReadCorrectNumInput(int UpperLimit = Int32.MaxValue, int LowerLimit = 1)
        {
            int Elem = 0;
            while(true)
            {
                try
                {
                    Elem = Convert.ToInt32(Console.ReadLine());
                    if (Elem < LowerLimit)
                    {
                        Console.WriteLine($"Значение мало. Введите значение >= {LowerLimit}.");
                    } else if(Elem > UpperLimit)
                    {
                        Console.WriteLine($"Значение велико. Введите значение <= {UpperLimit}.");
                    } else
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

        void MatrixWrite(double[,] Matrix)
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

        void DrawJordanTable(double[,] Elems, bool[] CoefficientsNotTransferredYet, string[] CoefficientsTransferred)
        {
            Console.Write("\n");
            int NumberEquation = Elems.GetLength(0);
            int MaxDegree = Elems.GetLength(1);
            for (int i = -1; i < NumberEquation; i++)
            {
                Console.Write("| ");
                for (int j = -2; j < MaxDegree; j++)
                {
                    if(i == -1)
                    {
                        if(j == -2)
                        {
                            Console.Write("   ");
                        } else if(j == -1)
                        {
                            Console.Write(" 1 ");
                        } else
                        {
                            if (CoefficientsNotTransferredYet[j])
                            {
                                Console.Write($"-X{j + 1}");
                            } else
                            {
                                Console.Write(" 0 ");
                            }
                        }
                    } else
                    {
                        if(j == -2)
                        {
                            Console.Write($"{CoefficientsTransferred[i]}=");
                        } else if(j == -1)
                        {
                            Console.Write($"A{i+1}0");
                        } else
                        {
                            Console.Write($"A{i + 1}{j + 1}");
                        }
                    }
                    Console.Write(" | ");
                }
                if(i == -1)
                {
                    Console.Write("\n");
                }
                Console.Write("\n");
            }           
        }

        int[] SelectJordanElem(double[,] Elems, double[] Answers, bool[] CoefficientsNotTransferredYet, string[] CoefficientsTransferred)
        {
            int NumberEquation = Elems.GetLength(0);
            int MaxDegree = Elems.GetLength(1);
            Console.WriteLine("\n\nЗначения ответов:");
            for (int i = 0; i < NumberEquation; i++)
            {
                Console.WriteLine($"A{i + 1}0 = {Answers[i]}");
            }
            Console.WriteLine("\n\nВыберите элемент, написав его номер:");
            for (int i = 0; i < NumberEquation; i++)
            {
                for (int j = 0; j < MaxDegree; j++)
                {
                    if (CoefficientsNotTransferredYet[j])
                    {
                        Console.Write($"{i * MaxDegree + j + 1}) A{i + 1}{j + 1} = {Elems[i, j]};   ");
                    } else
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
                } else if(CoefficientsTransferred[PositionI - 1] != "0 ")
                {
                    Console.WriteLine("\nВыберите число, находящееся в строке, которую ещё не выбирали!");
                    continue;
                } else if (!CoefficientsNotTransferredYet[PositionJ - 1])
                {
                    Console.WriteLine("\nСтолбец недоступен!");
                    continue;
                }
                return new int[2] { PositionI, PositionJ };
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