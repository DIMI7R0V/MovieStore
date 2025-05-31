using Kafka_Test_Project.Models;

namespace Kafka_Test_Project
{
    public static class ConsoleHelper
    {
        public static void WriteCoordinates(int x, int y, string text)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(text);
        }

        public static void WriteMsgFromUser(ChatMessage msg)
        {
            Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
            Console.WriteLine($"[{DateTime.Now.TimeOfDay}] [{msg.User}] {msg.Message}");

        }

        public static void WriteLocalUser()
        {
            Console.Write($"[{AppSetting.LocalUsername}]");

        }
    }
}