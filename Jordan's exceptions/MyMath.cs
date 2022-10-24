using System;
namespace JordansExceptions
{
    public class MyMath
    {
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


        public int CalculateRankMatrix(double[,] Matrix, int CurrentRankMatrix = 0) //Не доделанный
        {
            int Rows = Matrix.GetLength(0);
            int Colums = Matrix.GetLength(1);
            int MaxLimit = (Rows < Colums) ? Rows : Colums;

            if (CurrentRankMatrix > MaxLimit)
            {
                return MaxLimit;
            }
            if (CurrentRankMatrix == 0)
            {
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Colums; j++)
                    {
                        if (Matrix[i, j] != 0)
                        {
                            CalculateRankMatrix(Matrix, 1);
                        }
                        else
                        {
                            return CurrentRankMatrix;
                        }
                    }
                }
            }
            if (CurrentRankMatrix == 1)
            {
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Colums; j++)
                    {

                    }
                }
            }

            return CurrentRankMatrix;
        }

        public double CalculateDeterminantMatrix(double[,] Matrix) // Почти Готов (корректировки) -> В целом Вроде всё!!!
        {
            int SizeMatrix = Matrix.GetLength(0);
            if (SizeMatrix == 1)
            {
                return Matrix[0, 0];
            }
            else
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

        public double[,] ComposeMinorElemMatrix(double[,] Matrix, int RowPosition, int ColumnPosition) // Почти Готов (корректировки) -> В целом Вроде всё!!!
        {
            int SizeMatrixRow = Matrix.GetLength(0);
            int SizeMatrixColumn = Matrix.GetLength(1);
            double[,] AnswerFunc = new double[SizeMatrixRow - 1, SizeMatrixColumn - 1];
            for (int i = 0; i < SizeMatrixRow; i++)
            {
                for (int j = 0; j < SizeMatrixColumn; j++)
                {
                    if (i < RowPosition)
                    {
                        if (j < ColumnPosition)
                        {
                            AnswerFunc[i, j] = Matrix[i, j];
                        }
                        else if (j > ColumnPosition)
                        {
                            AnswerFunc[i, j - 1] = Matrix[i, j];
                        }
                    }
                    else if (i > RowPosition)
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

    }
}
