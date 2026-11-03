using System;
using System.Collections.Generic;
using System.IO;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile;
            string stopWordsFile;
            string xmlOutputFile;
            if (args.Length == 3) {
                 inputFile = args[0];
                 stopWordsFile = args[1];
                 xmlOutputFile = args[2];
            } else { 

                 inputFile = "input.txt";
                 stopWordsFile = "stopwords_ru.txt";
                // stopWordsFile = "stopwords_eng.txt";
                 xmlOutputFile = "textexport.xml";
             }

            if (!File.Exists(inputFile))
            {
                Console.WriteLine($"Файл '{inputFile}' не найден в папке проекта.");
                return;
            }

            Text userText;
            using (StreamReader sr = new StreamReader(inputFile))
            {
                userText = TextParcer.Parse(sr);
            }

            Console.WriteLine("Текст успешно загружен.\n");

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n--- Меню ---");
                Console.WriteLine("1. Показать весь текст");
                Console.WriteLine("2. Сортировка предложений по количеству слов");
                Console.WriteLine("3. Сортировка предложений по длине");
                Console.WriteLine("4. Поиск слов в вопросительных предложениях");
                Console.WriteLine("5. Удалить слова на согласную заданной длины");
                Console.WriteLine("6. Заменить слова заданной длины на подстроку");
                Console.WriteLine("7. Удалить стоп-слова");
                Console.WriteLine("8. Экспорт в XML");
                Console.WriteLine("9. Построить конкорданс текста");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите пункт: ");

                string? choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine(userText.ToString());
                        break;

                    case "2":
                        userText.OutForWordCount();
                        Console.WriteLine("Результат сортировки по числу слов:");
                        PrintSentences(userText);
                        break;

                    case "3":
                        userText.OutForSentenceLength();
                        Console.WriteLine("Результат сортировки по длине:");
                        PrintSentences(userText);
                        break;

                    case "4":
                        Console.Write("Введите длину искомых слов: ");
                        if (int.TryParse(Console.ReadLine(), out int lengthQ))
                        {
                            List<string> words = userText.SenchInQuestionSent(lengthQ);
                            Console.WriteLine(words.Count > 0
                                ? "Найденные слова: " + string.Join(", ", words)
                                : "Слова не найдены.");
                        }
                        break;

                    case "5":
                        Console.Write("Введите длину слов для удаления: ");
                        if (int.TryParse(Console.ReadLine(), out int lengthR))
                        {
                            userText.RemoveForLength(lengthR);
                            Console.WriteLine("Результат:");
                            Console.WriteLine(userText.ToString());
                        }
                        break;

                    case "6":
                        Console.Write("Индекс предложения (начиная с 0): ");
                        int.TryParse(Console.ReadLine(), out int sentenceIndex);
                        Console.Write("Длина заменяемых слов: ");
                        int.TryParse(Console.ReadLine(), out int targetLength);
                        Console.Write("Подстрока для замены: ");
                        string replacement = Console.ReadLine() ?? "";

                        userText.Change(replacement, sentenceIndex, targetLength);
                        Console.WriteLine("Результат:");
                        Console.WriteLine(userText.ToString());
                        break;

                    case "7":
                        if (File.Exists(stopWordsFile))
                        {
                            using (StreamReader stopSr = new StreamReader(stopWordsFile))
                            {
                                userText.RemoveStopWord(stopSr);
                            }
                            Console.WriteLine("Стоп-слова удалены:");
                            Console.WriteLine(userText.ToString());
                        }
                        else
                        {
                            Console.WriteLine($"Файл '{stopWordsFile}' не найден.");
                        }
                        break;

                    case "8":
                        userText.ExportToXml(xmlOutputFile);
                        Console.WriteLine($"Экспорт выполнен в файл '{xmlOutputFile}'.");
                        break;

                    case "9":
                        Console.WriteLine("Конкорданс текста:");
                        Console.WriteLine(userText.GetConcordance());
                        break;

                    case "0":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Неверный ввод, попробуйте снова.");
                        break;
                }
            }
        }

        private static void PrintSentences(Text text)
        {
            for (int i = 0; i < text.Sentences.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {text.Sentences[i]}");
            }
        }
    }
}