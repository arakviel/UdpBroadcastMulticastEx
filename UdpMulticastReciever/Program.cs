using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpMulticastReciever;

internal class Program
{
    static void Main(string[] args)
    {
        // Мультикаст адреса і порт
        string multicastAddress = "239.0.0.1"; // Виберіть адресу з діапазону мультикасту
        int port = 12345;

        // Створення UDP клієнта
        using (UdpClient udpClient = new UdpClient())
        {
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            // Налаштовуємо на отримання з мультикаст групи
            udpClient.JoinMulticastGroup(IPAddress.Parse(multicastAddress));

            // Прив'язуємо сокет до порту
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, port));

            // Прийом повідомлень
            Console.WriteLine($"Очікуємо повідомлення в мультикаст-групі {multicastAddress}:{port}...");
            while (true)
            {
                // Отримуємо дані
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
                byte[] data = udpClient.Receive(ref endPoint);

                // Перетворюємо отримані байти в текст
                string message = Encoding.UTF8.GetString(data);
                Console.WriteLine($"Отримано повідомлення: {message} від {endPoint.Address}");
            }
        }
    }
}
