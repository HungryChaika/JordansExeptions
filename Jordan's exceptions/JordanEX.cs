using System;
using System.Xml.Linq;

namespace JordansExceptions
{
    class JordanEX
    {
        public static void Main(string[] args)
        {
            UI ui = new UI();
            Console.WriteLine("\nВыберите метод:\n");
            Console.WriteLine("1) Метод Жордана;\n2) Метод искусственного базиса;\n");
            Console.Write("Выбор: ");
            int SelectedMethod = ui.CheckAndReadCorrectNumInput(2, 1);
            if (SelectedMethod == 1)
            {
                Console.WriteLine("\nМЕТОД ЖОРДАНОВЫХ ИСКЛЮЧЕНИЙ;\n");
                Console.Write("размерность системы или max n среди всех x^n: ");
                int MaxDegree = ui.CheckAndReadCorrectNumInput();
                Console.Write("количество уравнений в системе: ");
                int NumberEquation = ui.CheckAndReadCorrectNumInput();
                JordanMethod jordanMethod = new JordanMethod();
                // Массив параметров при x^n
                double[,] Matrix = new double[NumberEquation, MaxDegree];
                // Массив значений после равенства в системах
                double[] Answers = new double[NumberEquation];
                ui.FillArrayCoefficientsAndAnswersSystemEquationsAndCoofs(Matrix, Answers);
                bool[] CoefficientsNotTransferredYet = new bool[MaxDegree];
                Array.Fill(CoefficientsNotTransferredYet, true);
                string[] CoefficientsTransferred = new string[NumberEquation];
                Array.Fill(CoefficientsTransferred, "0 ");
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
                    jordanMethod.StepJordanEx(CoordSelectedElem[0], CoordSelectedElem[1], Matrix, Answers, CoefficientsNotTransferredYet, CoefficientsTransferred);
                    FinallyAnswer = jordanMethod.FindFinallyAnswer(Matrix, Answers, CoefficientsNotTransferredYet, CoefficientsTransferred);
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

            } else if(SelectedMethod == 2)
            {
                Console.WriteLine("\nМЕТОД ИСКУССТВЕННОГО БАЗИСА;\n");
                Console.Write("размерность системы или max n среди всех x^n: ");
                //int MaxDegree = ui.CheckAndReadCorrectNumInput();
                int MaxDegree = 5;
                Console.Write("количество уравнений в системе: ");
                //int NumberEquation = ui.CheckAndReadCorrectNumInput();
                int NumberEquation = 3;
                ArtificialBasisMethod artificialBasisMethod = new ArtificialBasisMethod(MaxDegree, NumberEquation);
                // Массив параметров при x^n
                //double[,] Matrix = new double[NumberEquation, MaxDegree];
                // Массив значений после равенства в системах
                //double[] Answers = new double[NumberEquation + 2];
                //Array.Fill(Answers, 0);
                // Массив параметров при x^n функции
                //double[] FCoofs = new double[MaxDegree];
                // Массив сумм элементов столюцов * (-1)
                double[] GCoofs = new double[MaxDegree];
                Array.Fill(GCoofs, 0);

                int[] IndependentElems = new int[MaxDegree];
                for (int i = 0; i < IndependentElems.Length; i++)
                {
                    IndependentElems[i] = i + 1;
                }

                Console.Write("\n\n");
                Console.Write("Это индексы верхних иксов");
                ui.MatrixWrite(IndependentElems);

                int[] DependentVariables = new int[NumberEquation];
                for (int i = 0; i < DependentVariables.Length; i++)
                {
                    DependentVariables[i] = i + 1 + IndependentElems.Length;
                }

                Console.Write("Это индексы левых иксов");
                ui.MatrixWrite(DependentVariables);

                // MOCKS
                double[,] Matrix = {
                    { 1, -4, 2, -5, 9 },
                    { 0, 1, -3, 4, -5 },
                    { 0, 1, -1, 1, -1 }
                };
                double[] Answers = { 3, 6, 1, 0, 0};
                double[] FCoofs = { 2, 6, -5, 1, 4 };
                // *****

                //ui.FillArrayCoefficientsAndAnswersSystemEquationsAndCoofs(Matrix, Answers, FCoofs);
                artificialBasisMethod.CalculationGCoofficients(Matrix, Answers, FCoofs, GCoofs);

                Console.Write("Это элементы строки G");
                ui.MatrixWrite(GCoofs);
                Console.Write("Это элементы Answers");
                ui.MatrixWrite(Answers);

                int[] IndexesResolvingElem = artificialBasisMethod.FindIndexesResolvingElement(Matrix, Answers, FCoofs, GCoofs);

                Console.Write("Координаты разрешающего элемента");
                ui.MatrixWrite(IndexesResolvingElem);

                artificialBasisMethod.StepJordanEx( IndexesResolvingElem[0], IndexesResolvingElem[1], Matrix, Answers,
                                                    FCoofs, GCoofs, DependentVariables, IndependentElems );

                Console.Write("Это элементы Matrix");
                ui.MatrixWrite(Matrix);

                Console.Write("Это элементы Answers");
                ui.MatrixWrite(Answers);

                Console.Write("Это элементы строки F");
                ui.MatrixWrite(FCoofs);

                Console.Write("Это элементы строки G");
                ui.MatrixWrite(GCoofs);

                Console.Write("Это индексы верхних иксов");
                ui.MatrixWrite(IndependentElems);

                Console.Write("Это индексы левых иксов");
                ui.MatrixWrite(DependentVariables);

                // ****************************************************************
                // Получаю позицию разрешающего элемента, далее нужен Жорданов Шаг;
                // ****************************************************************



            }

            // СЛЕДУЮЩАЯ ЛАБА - МЕТОД ИССКУСТВЕННОГО БАЗИСА
            // функция может стремиться как к max, так и к min обратить на это внимание
        }

    }
} 