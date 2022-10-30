using Microsoft.VisualBasic;
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

        public int[] FindIndexesResolvingElement(double[,] Elems, double[] Answers, double[] FCoofs, double[] GCoofs)
        {
            double[] CopiedAndSortGCoofs = new double[GCoofs.Length];
            Array.Copy(GCoofs, CopiedAndSortGCoofs, GCoofs.Length);
            Array.Sort(CopiedAndSortGCoofs);
            double[] CopyGCoofs = new double[GCoofs.Length];
            Array.Copy(GCoofs, CopyGCoofs, GCoofs.Length);

            int[] IndexElem = new int[2] { -1, -1 };
            double Elem = double.NaN;
            //Console.WriteLine($"\nElem is {Elem}");
            for (int i = 0; i < CopiedAndSortGCoofs.Length; i++)
            {
                for (int j = 0; j < CopyGCoofs.Length; j++)
                {
                    if (CopiedAndSortGCoofs[i] == CopyGCoofs[j])
                    {
                        CopyGCoofs[j] = double.NaN;
                        if(GCoofs[j] < 0)
                        {
                            for (int k = 0; k < Elems.GetLength(0); k++)
                            {
                                if (Elems[k, j] > 0)
                                {
                                    if (Elem is double.NaN)
                                    {
                                        //Console.WriteLine($"row: {k}, column: {j}");
                                        Elem = Answers[k] / Elems[k, j];
                                        IndexElem[0] = k;
                                        IndexElem[1] = j;
                                        //Console.WriteLine($"Elem is {Elem}");
                                    }
                                    else
                                    {
                                        //Console.WriteLine($"row: {k}, column: {j}");
                                        double PotentialElem = Answers[k] / Elems[k, j];
                                        //Console.WriteLine($"PotentialElem is {PotentialElem}");
                                        if (PotentialElem < Elem)
                                        {
                                            Elem = PotentialElem;
                                            IndexElem[0] = k;
                                            IndexElem[1] = j;
                                        }
                                        //Console.WriteLine($"Elem is {Elem}");
                                    }
                                }
                            }
                        }
                        
                        break;
                    }
                }
            }
            return IndexElem;
        }

        public void StepJordanEx(int XCoord, int YCoord, double[,] Elems, double[] Answers, double[] FCoofs,
                                  double[] GCoofs, int[] DependentVariables, int[] IndependentElems)
        {
            double[,] FuncElems = new double[NumberEquation, MaxDegree];
            double[] FuncAnswers = new double[NumberEquation + 2];
            double[] FuncFCoofs = new double[MaxDegree];
            double[] FuncGCoofs = new double[MaxDegree];

            for (int i = 0; i < NumberEquation; i++)
            {
                for (int j = 0; j < MaxDegree; j++)
                {
                    if (i == XCoord || j == YCoord)
                    {
                        FuncElems[i, j] = Elems[i, j] / Elems[XCoord, YCoord];
                    }
                    if (i != XCoord && j != YCoord)
                    {
                        FuncElems[i, j] = (Elems[i, j] * Elems[XCoord, YCoord] - Elems[XCoord, j] * Elems[i, YCoord]) / Elems[XCoord, YCoord];
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
            }
            FuncAnswers[NumberEquation] = (Answers[NumberEquation] * Elems[XCoord, YCoord] - Answers[XCoord] * FCoofs[YCoord]) / Elems[XCoord, YCoord];
            FuncAnswers[NumberEquation + 1] = (Answers[NumberEquation + 1] * Elems[XCoord, YCoord] - Answers[XCoord] * GCoofs[YCoord]) / Elems[XCoord, YCoord];

            for (int k = 0; k < MaxDegree; k++)
            {
                if (k == YCoord)
                {
                    FuncFCoofs[k] = FCoofs[k] / Elems[XCoord, YCoord];
                    FuncGCoofs[k] = GCoofs[k] / Elems[XCoord, YCoord];
                }
                else
                {
                    FuncFCoofs[k] = (FCoofs[k] * Elems[XCoord, YCoord] - FCoofs[YCoord] * Elems[XCoord, k]) / Elems[XCoord, YCoord];
                    FuncGCoofs[k] = (GCoofs[k] * Elems[XCoord, YCoord] - GCoofs[YCoord] * Elems[XCoord, k]) / Elems[XCoord, YCoord];
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
