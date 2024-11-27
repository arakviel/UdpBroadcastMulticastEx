using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpBroadcastReciever;

internal class Program
{
    static void Main()
    {
        // Порт для прослуховування повідомлень
        int port = 12345;

        // Створення і запуск кількох потоків (ресіверів)
        for (int i = 0; i < 3; i++) // 3 екземпляри
        {
            int receiverNumber = i + 1;
            new Thread(() => StartReceiver(port, receiverNumber)).Start();
        }

        // Оскільки потоки працюють асинхронно, основний потік чекає на завершення
        Console.WriteLine("Натисніть клавішу для виходу...");
        Console.ReadKey();
    }

    static void StartReceiver(int port, int receiverNumber)
    {
        // Створення UDP клієнта для прослуховування на порту
        using UdpClient udpClient = new UdpClient();

        try
        {
            // Налаштовуємо сокет на повторне використання адреси
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            // Прив'язуємо сокет до порту
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, port));

            udpClient.Client.ReceiveTimeout = 50_000; // 5 секунд на отримання пакета
            Console.WriteLine($"Ресивер {receiverNumber} прослуховує порт {port}...");

            while (true)
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
                byte[] data = udpClient.Receive(ref endPoint);

                // Перетворення отриманих байтів в текст
                string message = Encoding.UTF8.GetString(data);

                // Виведення отриманого повідомлення
                Console.WriteLine($"Ресивер {receiverNumber}: Отримано повідомлення від {endPoint.Address}: {message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ресивер {receiverNumber} помилка: {ex.Message}");
        }
    }
}
