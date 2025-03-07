using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using ChatApp.Models;

namespace ChatApp.Services
{
    public class MqttService
    {
        private static IMqttClient _mqttClient;
        private static MqttFactory _mqttFactory;
        private static string _clientId;
        private static int _currentUserId;
        private static Action<MessageDto> _messageReceivedCallback;

        public static async Task InitializeAsync(int userId, Action<MessageDto> messageReceivedCallback)
        {
            _currentUserId = userId;
            _clientId = $"ChatApp_User_{userId}_{Guid.NewGuid()}";
            _messageReceivedCallback = messageReceivedCallback;
            _mqttFactory = new MqttFactory();
            _mqttClient = _mqttFactory.CreateMqttClient();

            // Set up handlers
            _mqttClient.ApplicationMessageReceivedAsync += HandleReceivedMessage;
            _mqttClient.DisconnectedAsync += HandleDisconnected;

            await ConnectAsync();
        }

        private static async Task ConnectAsync()
        {
            try
            {
                var options = new MqttClientOptionsBuilder()
                    .WithClientId(_clientId)
                    .WithTcpServer("broker.hivemq.com", 1883) 
                    .WithCleanSession()
                    .Build();

                await _mqttClient.ConnectAsync(options, CancellationToken.None);

                // Subscribe to personal topic
                await _mqttClient.SubscribeAsync($"user/{_currentUserId}/#", MqttQualityOfServiceLevel.AtLeastOnce);

                // Subscribe to all chat rooms
                await _mqttClient.SubscribeAsync("chatroom/#", MqttQualityOfServiceLevel.AtLeastOnce);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MQTT Connection error: {ex.Message}");
                throw;
            }
        }

        public static async Task DisconnectAsync()
        {
            if (_mqttClient == null || !_mqttClient.IsConnected)
                return;

            await _mqttClient.DisconnectAsync();
        }

        public static async Task SubscribeToChatRoomAsync(int chatRoomId)
        {
            if (_mqttClient == null || !_mqttClient.IsConnected)
                await ConnectAsync();

            await _mqttClient.SubscribeAsync($"chatroom/{chatRoomId}", MqttQualityOfServiceLevel.AtLeastOnce);
        }

        public static async Task PublishMessageAsync(MessageDto message)
        {
            if (_mqttClient == null || !_mqttClient.IsConnected)
                await ConnectAsync();

            string topic;
            if (message.ChatRoomId.HasValue)
            {
                topic = $"chatroom/{message.ChatRoomId}";
            }
            else
            {
                topic = $"user/{message.RecipientId}";
            }

            string payload = JsonConvert.SerializeObject(message);
            var mqttMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .WithRetainFlag(false)
                .Build();

            await _mqttClient.PublishAsync(mqttMessage, CancellationToken.None);
        }

        private static Task HandleReceivedMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                var message = JsonConvert.DeserializeObject<MessageDto>(payload);

                // Only process messages not sent by ourselves
                if (message.SenderId != _currentUserId)
                {
                    _messageReceivedCallback?.Invoke(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing MQTT message: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        private static async Task HandleDisconnected(MqttClientDisconnectedEventArgs e)
        {
            Console.WriteLine("MQTT Client disconnected. Attempting to reconnect...");
            await Task.Delay(TimeSpan.FromSeconds(5));
            await ConnectAsync();
        }
    }

    public class MessageDto
    {
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public int? RecipientId { get; set; }
        public int? ChatRoomId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}