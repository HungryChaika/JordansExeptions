using System;

namespace JordansExceptions
{
    public class TransportTask
    {
        UI ui;
        private int NumberManufacturers;
        private int[] Manufacturers;
        private int NumberConsumers;
        private int[] Consumers;
        private int[,] CellsRates;
        private int[,] CellsContent;
        private bool[,] CellsFlagContent;
        private int[] IndexMinRate = { -1, -1 };
        private double[] PotentialsManufacturers;
        private double[] PotentialsConsumers;

        public TransportTask(UI ui)
        {
            this.ui = ui;
        }

        public void TaskInitTest()
        {
            NumberManufacturers = 4;
            Manufacturers = new int[] { 4, 6, 10, 10 };
            NumberConsumers = 5;
            Consumers = new int[] { 7, 7, 7, 7, 2 };
            CellsRates = new int[,] { { 16, 30, 17, 10, 4 }, { 30, 27, 26, 9, 23 }, { 13, 4, 22, 3, 1 }, { 3, 1, 5, 4, 24 } };
            CellsContent = new int[,] { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };
            CellsFlagContent = new bool[,] {
                { true, true, true, true, true },
                { true, true, true, true, true },
                { true, true, true, true, true },
                { true, true, true, true, true }
            };
            IndexMinRate = new int[] { 2, 4 };
            //******************************************************************************
            //NumberManufacturers = 3;
            //Manufacturers = new int[] { 90, 70, 50 };
            //NumberConsumers = 4;
            //Consumers = new int[] { 80, 60, 40, 30 };
            //CellsRates = new int[,] { { 2, 1, 3, 2 }, { 2, 3, 3, 1 }, { 3, 3, 2, 1 } };
            //CellsContent = new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
            //CellsFlagContent = new bool[,] {
            //    { true, true, true, true },
            //    { true, true, true, true },
            //    { true, true, true, true }
            //};
            //IndexMinRate = new int[] { 1, 3 };
            //******************************************************************************
            //NumberManufacturers = 4;
            //Manufacturers = new int[] { 85, 112, 72, 120 };
            //NumberConsumers = 5;
            //Consumers = new int[] { 75, 125, 64, 65, 60 };
            //CellsRates = new int[,] { { 7, 1, 4, 5, 2 }, { 13, 4, 7, 6, 3 }, { 3, 8, 0, 18, 12 }, { 9, 5, 3, 4, 7 } };
            //CellsContent = new int[,] { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };
            //CellsFlagContent = new bool[,] {
            //    { true, true, true, true, true },
            //    { true, true, true, true, true },
            //    { true, true, true, true, true },
            //    { true, true, true, true, true }
            //};
            //IndexMinRate = new int[] { 2, 2 };
        }

        public void TaskInit()
        {
            Console.Write("\n\nВведите количество Производителей: ");
            NumberManufacturers = ui.CheckAndReadCorrectNumInput();
            Manufacturers = new int[NumberManufacturers];
            for (int i = 0; i < NumberManufacturers; i++)
            {
                Console.Write($"\nПроизведено товара на A{i + 1} = ");
                int ElemManufacturers = ui.CheckAndReadCorrectNumInput(int.MaxValue, 0);
                Manufacturers[i] = ElemManufacturers;
                int PotencialQuantitySpaces = ElemManufacturers.ToString().Length;
            }
            Console.Write("\n\nВведите количество Потребителей: ");
            NumberConsumers = ui.CheckAndReadCorrectNumInput();
            Consumers = new int[NumberConsumers];
            for (int i = 0; i < NumberConsumers; i++)
            {
                Console.Write($"\nПроизведено товара на alpha{i + 1} = ");
                int ElemConsumers = ui.CheckAndReadCorrectNumInput(int.MaxValue, 0);
                Consumers[i] = ElemConsumers;
                int PotencialQuantitySpaces = ElemConsumers.ToString().Length;
            }
            Console.Write("\n\nВведите тарифы:\n\n");
            CellsRates = new int[NumberManufacturers, NumberConsumers];
            int MinRate = int.MaxValue;
            for (int i = 0; i < NumberManufacturers; i++)
            {
                for (int j = 0; j < NumberConsumers; j++)
                {
                    Console.Write($"\nТариф на пересечении A{i + 1} и alpha{j + 1}: ");
                    int Rate = ui.CheckAndReadCorrectNumInput(int.MaxValue, 0);
                    CellsRates[i, j] = Rate;
                    if (Rate < MinRate)
                    {
                        MinRate = Rate;
                        IndexMinRate[0] = i;
                        IndexMinRate[1] = j;
                    }
                }
            }
            CellsContent = new int[NumberManufacturers, NumberConsumers];
            CellsFlagContent = new bool[NumberManufacturers, NumberConsumers];
            for (int i = 0; i < NumberManufacturers; i++)
            {
                for (int j = 0; j < NumberConsumers; j++)
                {
                    CellsFlagContent[i, j] = true;
                }
            }
        }

        public void Step()
        {
            int Content = (Manufacturers[IndexMinRate[0]] < Consumers[IndexMinRate[1]]) ? Manufacturers[IndexMinRate[0]] : Consumers[IndexMinRate[1]];
            CellsFlagContent[IndexMinRate[0], IndexMinRate[1]] = false;
            Manufacturers[IndexMinRate[0]] -= Content;
            Consumers[IndexMinRate[1]] -= Content;
            CellsContent[IndexMinRate[0], IndexMinRate[1]] = Content;
        }

        public bool FindNextMinRateInternetVersion()
        {
            int[] OldIndex = new int[IndexMinRate.Length];//new int[] { IndexMinRate[0], IndexMinRate[1] };
            IndexMinRate.CopyTo(OldIndex, 0);
            bool FlagFirstAssign = true;
            for (int i = 0; i < NumberManufacturers; i++)
            {
                for (int j = 0; j < NumberConsumers; j++)
                {
                    if (CellsFlagContent[i,j] && Manufacturers[i] > 0 && Consumers[j] > 0)
                    {
                        if (FlagFirstAssign)
                        {
                            IndexMinRate = new int[2] { i, j };
                            FlagFirstAssign = false;
                        }
                        else if (CellsRates[i, j] <= CellsRates[IndexMinRate[0], IndexMinRate[1]])
                        {
                            IndexMinRate = new int[2] { i, j };
                        }
                    }
                }
            }
            if (OldIndex[0] == IndexMinRate[0] && OldIndex[1] == IndexMinRate[1])
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool FindNextMinRate()
        {
            int[] OldIndex = new int[] { IndexMinRate[0], IndexMinRate[1] };
            if (Manufacturers[IndexMinRate[0]] > 0)
            {// проходимся по строке;
                for (int i = 0; i < NumberConsumers; i++)
                {
                    if (CellsFlagContent[IndexMinRate[0], i] && Consumers[i] != 0)
                    {
                        if (IndexMinRate[1] == OldIndex[1] || CellsRates[IndexMinRate[0], i] < CellsRates[IndexMinRate[0], IndexMinRate[1]])
                        {
                            IndexMinRate[1] = i;
                        }
                    }
                }
            }
            else if (Consumers[IndexMinRate[1]] > 0)
            {// проходим по столбцу;
                for (int j = 0; j < NumberManufacturers; j++)
                {

                    if (CellsFlagContent[j, IndexMinRate[1]] && Manufacturers[j] != 0)
                    {
                        if (IndexMinRate[0] == OldIndex[0] || CellsRates[j, IndexMinRate[1]] < CellsRates[IndexMinRate[0], IndexMinRate[1]])
                        {
                            IndexMinRate[0] = j;
                        }
                    }
                }
            }
            else
            {// конец;
                return true;
            }
            return false;
        }

        public void FindPotentials()
        {
            PotentialsManufacturers = new double[NumberManufacturers];
            Array.Fill(PotentialsManufacturers, double.NaN);
            bool[] FlagsPotentialsManufacturers = new bool[NumberManufacturers];
            Array.Fill(FlagsPotentialsManufacturers, true);

            PotentialsConsumers = new double[NumberConsumers];
            Array.Fill(PotentialsConsumers, double.NaN);
            bool[] FlagsPotentialsConsumers = new bool[NumberConsumers];
            Array.Fill(FlagsPotentialsConsumers, true);

            PotentialsManufacturers[0] = 0;
            FlagsPotentialsManufacturers[0] = false;

            while (true)
            {
                for (int i = 0; i < NumberManufacturers; i++)
                {
                    for (int j = 0; j < NumberConsumers; j++)
                    {
                        if (FlagsPotentialsConsumers[j] && !CellsFlagContent[i,j] && PotentialsManufacturers[i] is not double.NaN)
                        {
                            PotentialsConsumers[j] = CellsRates[i, j] - PotentialsManufacturers[i];
                            FlagsPotentialsConsumers[j] = false;
                        }
                    }
                }
                for (int j = 0; j < NumberConsumers; j++)
                {
                    for (int i = 0; i < NumberManufacturers; i++)
                    {
                        if (FlagsPotentialsManufacturers[i] && !CellsFlagContent[i, j] && PotentialsConsumers[j] is not double.NaN)
                        {
                            PotentialsManufacturers[i] = CellsRates[i, j] - PotentialsConsumers[j];
                            FlagsPotentialsManufacturers[i] = false;
                        }
                    }
                }
                if (Array.IndexOf(PotentialsConsumers, double.NaN) == -1 && Array.IndexOf(PotentialsManufacturers, double.NaN) == -1)
                {
                    break;
                }
            }
            Console.Write("\n\nПотенциалы Manufacturers:\n");
            ui.MatrixWrite(PotentialsManufacturers);
            Console.Write("\n\nПотенциалы Consumers:\n");
            ui.MatrixWrite(PotentialsConsumers);
        }

        public int[] CheckPotentials()
        {
            int MinDifference = int.MaxValue;
            int[] IndexCellsMinDifference = { -1, -1 };
            for (int i = 0; i < NumberManufacturers; i++)
            {
                for (int j = 0; j < NumberConsumers; j++)
                {
                    if (CellsFlagContent[i,j])
                    {
                        int Difference = (int)(CellsRates[i, j] - PotentialsManufacturers[i] - PotentialsConsumers[j]);
                        if (Difference < 0 && Difference < MinDifference)
                        {
                            MinDifference = Difference;
                            IndexCellsMinDifference[0] = i;
                            IndexCellsMinDifference[1] = j;
                        }
                    }
                }
            }
            return IndexCellsMinDifference;
        }

        public void CheckIntermediateResult()
        {
            Console.Write("\nПотребители:\n");
            ui.MatrixWrite(Consumers);
            Console.Write("\nПроизводители:\n");
            ui.MatrixWrite(Manufacturers);
            Console.Write("\nТарифы:\n");
            ui.MatrixWrite(CellsRates);
            Console.Write("\nЗначения таблицы:\n");
            ui.MatrixWrite(CellsContent);
            Console.Write("\nЗначения флагов таблицы:\n");
            ui.MatrixWrite(CellsFlagContent);
            Console.Write("\nПозиция минимального тарифа:\n");
            ui.MatrixWrite(IndexMinRate);
        }

        public void DrawFinalAnswer()
        {
            Console.Write("\nЗначения таблицы:\n");
            ui.MatrixWrite(CellsContent);
        }

        public int CalculateF()
        {
            int F = 0;
            for (int i = 0; i < NumberManufacturers; i++)
            {
                for (int j = 0; j < NumberConsumers; j++)
                {
                    F += CellsContent[i,j] * CellsRates[i,j];
                }
            }
            return F;
        }

        public void FindAndUseContour(int[] StartCell)
        {
            int[] ContourPath = new int[(NumberConsumers + NumberManufacturers) * 2];
            Array.Fill(ContourPath, -1);
            ContourPath[0] = StartCell[0];
            ContourPath[1] = StartCell[1];
            bool Answer = StepContour(StartCell, ContourPath , "horizontal");
            Console.Write("\n\nКонтур:");
            ui.MatrixWrite(ContourPath);
            int MinElem = FindMinElemContour(ContourPath);
            Console.Write($"\n\nМинимальный элемент:\n\n{MinElem}\n");
            for (int i = 0; i < ContourPath.Length; i += 2)
            {
                if (ContourPath[i] != -1 && ContourPath[i + 1] != -1)
                {
                    CellsContent[ContourPath[i], ContourPath[i + 1]] += MinElem * ((i / 2) % 2 == 0 ? 1 : (-1));
                    CellsFlagContent[ContourPath[i], ContourPath[i + 1]] =
                        (CellsContent[ContourPath[i], ContourPath[i + 1]] > 0 ? false : true);
                }
            }
            Console.Write("\n\nОбновлённый план:");
            ui.MatrixWrite(CellsContent);
        }

        private bool StepContour(int[] PreviousCell, int[] ContourPath, string mode)
        {
            if (Array.IndexOf(ContourPath, -1) > 5 &&
                (PreviousCell[0] == ContourPath[0] || PreviousCell[1] == ContourPath[1]))
            {
                return true;
            }

            int[] IndexCell = {-1, -1};
            int FirstFreeIndex = Array.IndexOf(ContourPath, -1);
            if (mode == "horizontal")
            {
                bool FlagFindAnswer = false;
                for (int i = PreviousCell[1] + 1; i < NumberConsumers; i++)
                {
                    if (!CellsFlagContent[PreviousCell[0], i])
                    {
                        if (FirstFreeIndex > -1 && FirstFreeIndex + 1 < ContourPath.Length)
                        {
                            IndexCell[0] = PreviousCell[0];
                            IndexCell[1] = i;
                            ContourPath[FirstFreeIndex] = IndexCell[0];
                            ContourPath[FirstFreeIndex + 1] = IndexCell[1];
                            FlagFindAnswer = StepContour(IndexCell, ContourPath, "vertical");
                            if (!FlagFindAnswer)
                            {
                                ContourPath[FirstFreeIndex] = -1;
                                ContourPath[FirstFreeIndex + 1] = -1;
                                IndexCell[0] = -1;
                                IndexCell[1] = -1;
                            }
                        }
                    }
                    if (IndexCell[0] != -1 && IndexCell[1] != -1)
                    {
                        return true;
                    }
                }
                if (!FlagFindAnswer)
                {
                    for (int i = PreviousCell[1] - 1; i >= 0; i--)
                    {
                        if (!CellsFlagContent[PreviousCell[0], i])
                        {
                            if (FirstFreeIndex > -1 && FirstFreeIndex + 1 < ContourPath.Length)
                            {
                                IndexCell[0] = PreviousCell[0];
                                IndexCell[1] = i;
                                ContourPath[FirstFreeIndex] = IndexCell[0];
                                ContourPath[FirstFreeIndex + 1] = IndexCell[1];
                                FlagFindAnswer = StepContour(IndexCell, ContourPath, "vertical");
                                if (!FlagFindAnswer)
                                {
                                    ContourPath[FirstFreeIndex] = -1;
                                    ContourPath[FirstFreeIndex + 1] = -1;
                                    IndexCell[0] = -1;
                                    IndexCell[1] = -1;
                                }
                            }
                        }
                        if (IndexCell[0] != -1 && IndexCell[1] != -1)
                        {
                            return true;
                        }
                    }
                }
            }
            if (mode == "vertical")
            {
                bool FlagFindAnswer = false;
                for (int j = PreviousCell[0] + 1; j < NumberManufacturers; j++)
                {
                    if (!CellsFlagContent[j, PreviousCell[1]])
                    {
                        if (FirstFreeIndex > -1 && FirstFreeIndex + 1 < ContourPath.Length)
                        {
                            IndexCell[0] = j;
                            IndexCell[1] = PreviousCell[1];
                            ContourPath[FirstFreeIndex] = IndexCell[0];
                            ContourPath[FirstFreeIndex + 1] = IndexCell[1];
                            FlagFindAnswer = StepContour(IndexCell, ContourPath, "horizontal");
                            if (!FlagFindAnswer)
                            {
                                ContourPath[FirstFreeIndex] = -1;
                                ContourPath[FirstFreeIndex + 1] = -1;
                                IndexCell[0] = -1;
                                IndexCell[1] = -1;
                            }
                        }
                    }
                    if (IndexCell[0] != -1 && IndexCell[1] != -1)
                    {
                        return true;
                    }
                }
                if (!FlagFindAnswer)
                {
                    for (int j = PreviousCell[0] - 1; j >= 0; j--)
                    {
                        if (!CellsFlagContent[j, PreviousCell[1]])
                        {
                            if (FirstFreeIndex > -1 && FirstFreeIndex + 1 < ContourPath.Length)
                            {
                                IndexCell[0] = j;
                                IndexCell[1] = PreviousCell[1];
                                ContourPath[FirstFreeIndex] = IndexCell[0];
                                ContourPath[FirstFreeIndex + 1] = IndexCell[1];
                                FlagFindAnswer = StepContour(IndexCell, ContourPath, "horizontal");
                                if (!FlagFindAnswer)
                                {
                                    ContourPath[FirstFreeIndex] = -1;
                                    ContourPath[FirstFreeIndex + 1] = -1;
                                    IndexCell[0] = -1;
                                    IndexCell[1] = -1;
                                }
                            }
                        }
                        if (IndexCell[0] != -1 && IndexCell[1] != -1)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private int FindMinElemContour(int[] ContourPath)
        {
            int MinElem = int.MaxValue;
            for (int i = 0; i < ContourPath.Length; i += 2)
            {
                int Elem = int.MaxValue;
                if (ContourPath[i] != -1 && ContourPath[i + 1] != -1)
                {
                    if ((i / 2) % 2 != 0)
                    {
                        Elem = CellsContent[ContourPath[i], ContourPath[i + 1]];
                    }
                }
                else
                {
                    return MinElem;
                }
                if (!CellsFlagContent[ContourPath[i], ContourPath[i + 1]] && Elem < MinElem)
                {
                    MinElem = CellsContent[ContourPath[i], ContourPath[i + 1]];
                }
            }
            return MinElem;
        }

        //private bool CheckContourPath(int[] Cell, int[] ContourPath)
        //{
        //    for (int i = 0; i < ContourPath.Length; i += 2)
        //    {
        //        if (Cell[0] == ContourPath[i] && Cell[1] == ContourPath[i + 1])
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
    }
}
