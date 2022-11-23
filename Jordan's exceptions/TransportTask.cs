using System;

namespace JordansExceptions
{
    public class TransportTask
    {
        UI ui;
        public int QuantitySpaces = -1;
        public int NumberManufacturers;
        public int[] Manufacturers;
        public int NumberConsumers;
        public int[] Consumers;
        public int[,] CellsRates;
        public int[,] CellsContent;
        public bool[,] CellsFlagContent;
        public int[] IndexMinRate = { -1, -1 };

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
            CellsRates = new int[,] { { 2, 1, 3, 2 }, { 2, 3, 3, 1 }, { 3, 2, 2, 1 } };
            CellsContent = new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
            CellsFlagContent = new bool[,] {
                { false, false, false, false },
                { false, false, false, false },
                { false, false, false, false }
            };
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
                    CellsFlagContent[i, j] = false;
                }
            }
        }

        public void Step()
        {

        }

    }
}
