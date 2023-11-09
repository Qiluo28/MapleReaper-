using MapleReaper.Libraries;
using MapleReaper.Models;
using MapleReaper.Properties;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MapleReaper.Services
{
    internal static class RabbitMQService
    {
        private static ScriptInvoke stopScript;
        private static IModel channel;

        public static void Start(ScriptInvoke stopScript)
        {
            RabbitMQService.stopScript = stopScript;
            var factory = new ConnectionFactory();
            factory.UserName = "jimmy";
            factory.Password = "031963";
            factory.HostName = "220.133.21.181";
            var connection = factory.CreateConnection();
            channel = connection.CreateModel();
            SetupAlertConsumer(channel);
            SetupControlSonsumer(channel);
        }

        public static void SendAlert()
        {
            channel.BasicPublish("alert", string.Empty, null, GetMessage(Settings.Default.Id));
        }

        public static void SendWarning()
        {
            channel.BasicPublish("warning", string.Empty, null, GetMessage(Settings.Default.Id));
        }

        private static void SetupAlertConsumer(IModel channel)
        {
            var queueName = channel.QueueDeclare().QueueName;
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Alert;
            channel.ExchangeDeclare("alert", ExchangeType.Fanout);
            channel.QueueBind(queueName, "alert", string.Empty);
            channel.BasicConsume(queueName, true, consumer);
        }

        private static async void Alert(object sender, BasicDeliverEventArgs args)
        {
            if (Settings.Default.Id == GetMessage(args)) return;
            if (MemoryReader.GetMapId() == 910000000) return;
            stopScript();
            await Keyboard.Type("@fm");
        }

        private static void SetupControlSonsumer(IModel channel)
        {
            var queueName = channel.QueueDeclare().QueueName;
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Control;
            channel.ExchangeDeclare("control", ExchangeType.Fanout);
            channel.QueueBind(queueName, "control", string.Empty);
            channel.BasicConsume(queueName, true, consumer);
        }

        private static async void Control(object sender, BasicDeliverEventArgs args)
        {
            var message = JsonSerializer.Deserialize<string[]>(GetMessage(args));
            var target = message[0];
            var command = message[1];
            if (target != "all" && target != Settings.Default.Id) return;
            switch (command)
            {
                case "start":
                    Keyboard.ControlKeyPress(Keys.F8);
                    break;
                case "stop":
                    stopScript();
                    break;
                case "pause":
                    ReaperSetting.IsFreezing = true;
                    break;
                case "resume":
                    ReaperSetting.IsFreezing = false;
                    break;
                case "id":
                    channel.BasicPublish("register", string.Empty, null, GetMessage(Settings.Default.Id));
                    break;
                default:
                    await Keyboard.Type(message[1]);
                    break;
            }
        }

        private static string GetMessage(BasicDeliverEventArgs ea)
        {
            return Encoding.UTF8.GetString(ea.Body.ToArray());
        }

        private static byte[] GetMessage(string message)
        {
            return Encoding.UTF8.GetBytes(message);
        }
    }
}