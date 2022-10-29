using System;
namespace JordansExceptions
{
    public class JordanMethod
    {
        //int MaxPossibleRankMatrix = 0;
        //this.MaxPossibleRankMatrix = (NumberEquation<MaxDegree) ? NumberEquation : MaxDegree;

        public void StepJordanEx(int XCoord, int YCoord, double[,] Elems, double[] Answers, bool[] CoefficientsNotTransferredYet, string[] CoefficientsTransferred)
        {
            int NumberEquation = Elems.GetLength(0);
            int MaxDegree = Elems.GetLength(1);
            double[,] FuncElems = new double[NumberEquation, MaxDegree];
            double[] FuncAnswers = new double[NumberEquation];
            for (int i = 0; i < NumberEquation; i++)
            {
                for (int j = 0; j < MaxDegree; j++)
                {
                    if (i == XCoord - 1)
                    {
                        FuncElems[i, j] = Elems[i, j] / Elems[XCoord - 1, YCoord - 1];
                    }
                    else
                    {
                        FuncElems[i, j] = (Elems[i, j] * Elems[XCoord - 1, YCoord - 1] - Elems[XCoord - 1, j] * Elems[i, YCoord - 1]) / Elems[XCoord - 1, YCoord - 1];
                    }
                }
                if (i == XCoord - 1)
                {
                    FuncAnswers[i] = Answers[i] / Elems[XCoord - 1, YCoord - 1];
                }
                else
                {
                    FuncAnswers[i] = (Answers[i] * Elems[XCoord - 1, YCoord - 1] - Answers[XCoord - 1] * Elems[i, YCoord - 1]) / Elems[XCoord - 1, YCoord - 1];
                }
            }
            CoefficientsNotTransferredYet[YCoord - 1] = false;
            CoefficientsTransferred[XCoord - 1] = $"X{YCoord}";
            Array.Copy(FuncElems, Elems, FuncElems.Length);
            Array.Copy(FuncAnswers, Answers, FuncAnswers.Length);
        }

        public String FindFinallyAnswer(double[,] Elems, double[] Answers, bool[] CoefficientsNotTransferredYet, string[] CoefficientsTransferred)
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
                for (int k = 0; k < NumberEquation; k++)
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
                for (int k = 0; k < MaxDegree; k++)
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
