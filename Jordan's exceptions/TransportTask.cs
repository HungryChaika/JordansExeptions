using System;
using static System.Net.Mime.MediaTypeNames;

namespace JordansExceptions
{
    public class TransportTask
    {
        UI ui;
        private int QuantitySpaces = -1;
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
            NumberManufacturers = 3;
            Manufacturers = new int[] { 90, 70, 50 };
            NumberConsumers = 4;
            Consumers = new int[] { 80, 60, 40, 30 };
            CellsRates = new int[,] { { 2, 1, 3, 2 }, { 2, 3, 3, 1 }, { 3, 3, 2, 1 } };
            CellsContent = new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
            CellsFlagContent = new bool[,] {
                { true, true, true, true },
                { true, true, true, true },
                { true, true, true, true }
            };
            IndexMinRate = new int[] { 1, 3 };
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
                if (QuantitySpaces < ElemManufacturers.ToString().Length)
                {
                    QuantitySpaces = PotencialQuantitySpaces;
                }
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
                if (QuantitySpaces < ElemConsumers.ToString().Length)
                {
                    QuantitySpaces = PotencialQuantitySpaces;
                }
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

        public bool FindNextMinRate()
        {
            int[] OldIndex = new int[] { IndexMinRate[0], IndexMinRate[1] };
            if (Manufacturers[IndexMinRate[0]] > 0)
            {
                // проходимся по строке;
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
            {
                // проходим по столбцу;
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
            {
                // конец;
                Console.WriteLine("\n\nКОНЕЦ\n\n");
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
            Console.Write("\n\nManufacturers:\n");
            ui.MatrixWrite(PotentialsManufacturers);
            Console.Write("\n\nConsumers:\n");
            ui.MatrixWrite(PotentialsConsumers);
        }

        public void CheckIntermediateResult()
        {
            ui.MatrixWrite(Consumers);
            ui.MatrixWrite(Manufacturers);
            ui.MatrixWrite(CellsRates);
            ui.MatrixWrite(CellsContent);
            ui.MatrixWrite(CellsFlagContent);
            ui.MatrixWrite(IndexMinRate);
            //Console.WriteLine(QuantitySpaces);
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
    }
}
