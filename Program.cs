using FormSorter;

namespace MyNamespace;

class Program
{
    public static void Main()
    {
        Console.WriteLine(Consts.DESC);
        while (true)
        {
            Console.WriteLine("Укажите путь к папке inbound/hotel/ на флэшке:");

            var inboundFlashFolder = Console.ReadLine();
            if (!Directory.Exists(inboundFlashFolder))
            {
                Console.WriteLine($"Путь не существует: {inboundFlashFolder}");
                continue;
            }

            Console.WriteLine("Укажите путь к папке бэкапа:");

            var backupFolder = Console.ReadLine();
            if (!Directory.Exists(backupFolder))
            {
                Console.WriteLine($"Путь не существует: {backupFolder}");
                continue;
            }

            Console.WriteLine("Забираю формы с флэшки, создаю бэкап.");
            Utils.MoveFiles(inboundFlashFolder, backupFolder);

            var resutlFolder = Path.Combine(backupFolder, "sorted");
            Utils.CreateFolders(resutlFolder);

            Console.WriteLine("Сортирую.");
            Utils.CopyFiles(backupFolder, resutlFolder);

            Console.WriteLine($"Работа программы завершена. Результат: {resutlFolder}");
            Console.WriteLine();
        }
    }
}