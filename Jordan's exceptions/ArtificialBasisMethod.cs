using System;

namespace JordansExceptions
{
    public class ArtificialBasisMethod
    {
        public double[] LastPlan;
        private int MaxDegree;
        private int NumberEquation;

        public ArtificialBasisMethod(int maxDegree, int numberEquation)
        {
            this.LastPlan = new double[maxDegree + numberEquation];
            MaxDegree = maxDegree;
            NumberEquation = numberEquation;

        }

        public void CalculationGCoofficients(double[,] Elems, double[] Answers, double[] FCoofs, double[] GCoofs)
        {
            for (int k = 0; k < NumberEquation; k++)
            {
                Answers[Answers.Length - 1] += Answers[k];
            }
            Answers[Answers.Length - 1] = Answers[Answers.Length - 1] * (-1);
            for (int i = 0; i < MaxDegree; i++)
            {
                for (int j = 0; j < NumberEquation; j++)
                {
                    GCoofs[i] += Elems[j, i];
                }
                GCoofs[i] = GCoofs[i] * (-1);
            }
        }

        public int[] FindIndexesResolvingElement(double[,] Elems, double[] Answers, double[] FCoofs, double[] GCoofs/*, bool flashok = false*/)
        {
            //if (flashok)
            //{
            //    return new int[2] { 0, 0 };
            //}
            bool FlagGCoofsHaveNotNegativeNumbers = true;
            bool FlagGCoofsHaveZeroNumbers = false;
            bool[] IndexsZeroNumbersInGCoofs = new bool[GCoofs.Length];
            Array.Fill(IndexsZeroNumbersInGCoofs, false);

            for (int k = 0; k < GCoofs.Length; k++)
            {
                if (GCoofs[k] < 0)
                {
                    FlagGCoofsHaveNotNegativeNumbers = false;
                    break;
                }
                else if(GCoofs[k] == 0)
                {
                    IndexsZeroNumbersInGCoofs[k] = true;
                    FlagGCoofsHaveZeroNumbers = true;
                }
            }

            if (FlagGCoofsHaveNotNegativeNumbers)
            {
                if (FlagGCoofsHaveZeroNumbers)
                {
                    double SmallestElemFCoof = double.NaN;
                    int IndexSmallerElemFCoof = -1;

                    for (int k = 0; k < IndexsZeroNumbersInGCoofs.Length; k++)
                    {
                        if (IndexsZeroNumbersInGCoofs[k])
                        {
                            if (SmallestElemFCoof is double.NaN && FCoofs[k] < 0)
                            {
                                SmallestElemFCoof = FCoofs[k];
                                IndexSmallerElemFCoof = k;
                            }
                            else if (FCoofs[k] < SmallestElemFCoof)
                            {
                                SmallestElemFCoof = FCoofs[k];
                                IndexSmallerElemFCoof = k;
                            }
                        }
                    }

                    if (SmallestElemFCoof is double.NaN)
                    {

                        return new int[2] { -1, -1 };// Решение - план оптимален!

                    }
                    else
                    {
                        double AnswerNumber = double.NaN;
                        int[] IndexsAnswerNumber = new int[2] { -2, -2 };

                        for (int k = 0; k < Elems.GetLength(0); k++)
                        {
                            if (Elems[k, IndexSmallerElemFCoof] > 0)
                            {
                                if (AnswerNumber is double.NaN)
                                {
                                    AnswerNumber = Answers[k] / Elems[k, IndexSmallerElemFCoof];
                                    IndexsAnswerNumber[0] = k;
                                    IndexsAnswerNumber[1] = IndexSmallerElemFCoof;
                                }
                                else
                                {
                                    double PotentialAnswerNumber = Answers[k] / Elems[k, IndexSmallerElemFCoof];
                                    if (PotentialAnswerNumber < AnswerNumber)
                                    {
                                        AnswerNumber = PotentialAnswerNumber;
                                        IndexsAnswerNumber[0] = k;
                                        IndexsAnswerNumber[1] = IndexSmallerElemFCoof;
                                    }
                                }
                            }
                        }

                        return IndexsAnswerNumber; // вернёт либо координаты, либо значения - значащие, что элемент не найден и решения нет

                    }
                }
                else
                {
                    return new int[2] { -5, -5 };// Этого быть не должно (сказал Виталий Иванович)
                }
            }
            else
            {
                double SmallestElemGCoof = double.NaN;
                int IndexSmallerElemGCoof = -1;

                for (int k = 0; k < GCoofs.Length; k++)
                {
                    if (SmallestElemGCoof is double.NaN && GCoofs[k] < 0)
                    {
                        SmallestElemGCoof = GCoofs[k];
                        IndexSmallerElemGCoof = k;
                    }
                    else if (GCoofs[k] < SmallestElemGCoof)
                    {
                        SmallestElemGCoof = GCoofs[k];
                        IndexSmallerElemGCoof = k;
                    }
                }

                double AnswerNumber = double.NaN;
                int[] IndexsAnswerNumber = new int[2] { -2, -2 }; // Можно придумать др. значения

                for (int k = 0; k < Elems.GetLength(0); k++)
                {
                    if (Elems[k, IndexSmallerElemGCoof] > 0)
                    {
                        if (AnswerNumber is double.NaN)
                        {
                            AnswerNumber = Answers[k] / Elems[k, IndexSmallerElemGCoof];
                            IndexsAnswerNumber[0] = k;
                            IndexsAnswerNumber[1] = IndexSmallerElemGCoof;
                        }
                        else
                        {
                            double PotentialAnswerNumber = Answers[k] / Elems[k, IndexSmallerElemGCoof];
                            if (PotentialAnswerNumber < AnswerNumber)
                            {
                                AnswerNumber = PotentialAnswerNumber;
                                IndexsAnswerNumber[0] = k;
                                IndexsAnswerNumber[1] = IndexSmallerElemGCoof;
                            }
                        }
                    }
                }

                return IndexsAnswerNumber; // Вернёт либо координаты, либо значения - значащие, что элемент не найден и решения нет
            }

            return new int[2] { -10, -10 }; // Ничего не произошло
        }

        public void StepJordanEx(int XCoord, int YCoord, double[,] Elems, double[] Answers, double[] FCoofs,
                                  double[] GCoofs, int[] DependentVariables, int[] IndependentElems)
        {
            double[,] FuncElems = new double[NumberEquation, MaxDegree];
            double[] FuncAnswers = new double[NumberEquation + 2];
            double[] FuncFCoofs = new double[MaxDegree];
            double[] FuncGCoofs = new double[MaxDegree];

            double Limit = 0.000001;

            for (int i = 0; i < NumberEquation; i++)
            {
                for (int j = 0; j < MaxDegree; j++)
                {
                    if (i != XCoord && j != YCoord)
                    {
                        FuncElems[i, j] = (Elems[i, j] * Elems[XCoord, YCoord] - Elems[XCoord, j] * Elems[i, YCoord]) / Elems[XCoord, YCoord];
                    }
                    else if (i == XCoord && j != YCoord)
                    {
                        FuncElems[i, j] = Elems[i, j] / Elems[XCoord, YCoord];
                    }
                    else if (i != XCoord && j == YCoord)
                    {
                        FuncElems[i, j] = Elems[i, j] / Elems[XCoord, YCoord] * (-1);
                    } else if (i == XCoord && j == YCoord)
                    {
                        FuncElems[i, j] = 1 / Elems[XCoord, YCoord];
                    }
                    //FuncElems[i, j] = Convert.ToDouble(FuncElems[i, j].ToString("0.##"));
                    if(FuncElems[i, j] < Limit && FuncElems[i, j] > -Limit)
                    {
                        FuncElems[i, j] = 0;
                    }
                }
                if (i == XCoord)
                {
                    FuncAnswers[i] = Answers[i] / Elems[XCoord, YCoord];
                }
                else
                {
                    FuncAnswers[i] = (Answers[i] * Elems[XCoord, YCoord] - Answers[XCoord] * Elems[i, YCoord]) / Elems[XCoord, YCoord];
                }
                //FuncAnswers[i] = Convert.ToDouble(FuncAnswers[i].ToString("0.##"));
                if (FuncAnswers[i] < Limit && FuncAnswers[i] > -Limit)
                {
                    FuncAnswers[i] = 0;
                }
            }
            FuncAnswers[NumberEquation] = (Answers[NumberEquation] * Elems[XCoord, YCoord] - Answers[XCoord] * FCoofs[YCoord]) / Elems[XCoord, YCoord];
            //FuncAnswers[NumberEquation] = Convert.ToDouble(FuncAnswers[NumberEquation].ToString("0.##"));
            if (FuncAnswers[NumberEquation] < Limit && FuncAnswers[NumberEquation] > -Limit)
            {
                FuncAnswers[NumberEquation] = 0;
            }

            FuncAnswers[NumberEquation + 1] = (Answers[NumberEquation + 1] * Elems[XCoord, YCoord] - Answers[XCoord] * GCoofs[YCoord]) / Elems[XCoord, YCoord];
            //FuncAnswers[NumberEquation + 1] = Convert.ToDouble(FuncAnswers[NumberEquation + 1].ToString("0.##"));
            if (FuncAnswers[NumberEquation + 1] < Limit && FuncAnswers[NumberEquation + 1] > -Limit)
            {
                FuncAnswers[NumberEquation + 1] = 0;
            }

            for (int k = 0; k < MaxDegree; k++)
            {
                if (k == YCoord)
                {
                    FuncFCoofs[k] = FCoofs[k] / Elems[XCoord, YCoord] * (-1);
                    FuncGCoofs[k] = GCoofs[k] / Elems[XCoord, YCoord] * (-1);
                }
                else
                {
                    FuncFCoofs[k] = (FCoofs[k] * Elems[XCoord, YCoord] - FCoofs[YCoord] * Elems[XCoord, k]) / Elems[XCoord, YCoord];
                    FuncGCoofs[k] = (GCoofs[k] * Elems[XCoord, YCoord] - GCoofs[YCoord] * Elems[XCoord, k]) / Elems[XCoord, YCoord];
                }
                //FuncFCoofs[k] = Convert.ToDouble(FuncFCoofs[k].ToString("0.##"));
                if (FuncFCoofs[k] < Limit && FuncFCoofs[k] > -Limit)
                {
                    FuncFCoofs[k] = 0;
                }
                //FuncGCoofs[k] = Convert.ToDouble(FuncGCoofs[k].ToString("0.##"));
                if (FuncGCoofs[k] < Limit && FuncGCoofs[k] > -Limit)
                {
                    FuncGCoofs[k] = 0;
                }
            }

            int Item = IndependentElems[YCoord];
            IndependentElems[YCoord] = DependentVariables[XCoord];
            DependentVariables[XCoord] = Item;
            Array.Copy(FuncElems, Elems, FuncElems.Length);
            Array.Copy(FuncAnswers, Answers, FuncAnswers.Length);
            Array.Copy(FuncFCoofs, FCoofs, FuncFCoofs.Length);
            Array.Copy(FuncGCoofs, GCoofs, FuncGCoofs.Length);
        }


    }
}
