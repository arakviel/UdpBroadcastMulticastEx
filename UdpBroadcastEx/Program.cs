using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpBroadcastEx;

internal class Program
{
    static void Main()
    {
        string broadcastAddress = "255.255.255.255";
        int port = 12345; // Порт, на який будемо відправляти повідомлення

        // Створення UDP клієнта
        using (UdpClient udpClient = new UdpClient())
        {
            try
            {
                // Налаштування для бродкасту
                udpClient.EnableBroadcast = true;

                // Створення адреси для бродкасту
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, port);

                // Підготовка повідомлення для відправки
                string message = "Привіт, це повідомлення через UDP бродкаст!";
                byte[] data = Encoding.UTF8.GetBytes(message);

                // Відправка повідомлення
                udpClient.Send(data, data.Length, endPoint);
                Console.WriteLine($"Повідомлення відправлено на {broadcastAddress}:{port}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}