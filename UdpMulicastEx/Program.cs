using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpMulicastEx;

internal class Program
{
    static void Main()
    {
        // Мультикаст адреса і порт
        string multicastAddress = "239.0.0.1"; // Виберіть адресу з діапазону мультикасту
        int port = 12345;

        // Створення клієнта для UDP
        using UdpClient udpClient = new UdpClient();

        // Налаштовуємо на відправку в мультикаст групу
        udpClient.JoinMulticastGroup(IPAddress.Parse(multicastAddress));

        // Створюємо повідомлення
        string message = "Hello, Multicast World!";
        byte[] data = Encoding.UTF8.GetBytes(message);

        // Підготовка до циклічної відправки повідомлень
        while (true)
        {
            // Відправляємо мультикаст повідомлення
            udpClient.Send(data, data.Length, new IPEndPoint(IPAddress.Parse(multicastAddress), port));
            Console.WriteLine($"Відправлено повідомлення: {message}");

            // Затримка в 1 секунду
            Thread.Sleep(1000);
        }
    }
}