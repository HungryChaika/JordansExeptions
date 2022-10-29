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


    }
}
