using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChatApp.Models;
using ChatApp.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ChatApp.Forms
{
    public partial class ChatForm : Form
    {
        private int _currentUserId;
        private User _currentUser;
        private ChatAppDbContext _dbContext;
        private int? _selectedChatRoomId;
        private int? _selectedUserId;
        private List<ChatRoom> _chatRooms;
        private List<User> _users;

        public ChatForm(int userId)
        {
            InitializeComponent();
            _currentUserId = userId;
            _dbContext = new ChatAppDbContext();

            // Load user info
            _currentUser = _dbContext.Users
                .FirstOrDefault(u => u.Id == _currentUserId);

            if (_currentUser == null)
            {
                MessageBox.Show("User not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            this.Text = $"Chat App - {_currentUser.Username}";
            this.Load += ChatForm_Load;
            this.FormClosing += ChatForm_FormClosing;
        }

        private async void ChatForm_Load(object sender, EventArgs e)
        {
            // Initialize MQTT service
            try
            {
                await MqttService.InitializeAsync(_currentUserId, MessageReceived);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to connect to chat server: {ex.Message}",
                    "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            LoadChatRooms();
            LoadUsers();
        }

        private async void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Update user status
            try
            {
                _currentUser.IsOnline = false;
                _dbContext.SaveChanges();

                // Disconnect from MQTT
                await MqttService.DisconnectAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during form closing: {ex.Message}");
            }
        }

        private void LoadChatRooms()
        {
            _chatRooms = _dbContext.ChatRooms
                .Include(cr => cr.Members)
                .Where(cr => cr.Members.Any(m => m.UserId == _currentUserId))
                .ToList();

            lstChatRooms.Items.Clear();
            foreach (var room in _chatRooms)
            {
                lstChatRooms.Items.Add(room.Name);
            }
        }

        private void LoadUsers()
        {
            _users = _dbContext.Users
                .Where(u => u.Id != _currentUserId)
                .ToList();

            lstUsers.Items.Clear();
            foreach (var user in _users)
            {
                string status = user.IsOnline ? "🟢" : "⚪";
                lstUsers.Items.Add($"{status} {user.Username}");
            }
        }

        private void MessageReceived(MessageDto messageDto)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => MessageReceived(messageDto)));
                return;
            }

            // Check if this message belongs to the current selected chat
            bool isRelevant = false;

            if (messageDto.ChatRoomId.HasValue && _selectedChatRoomId.HasValue)
            {
                isRelevant = messageDto.ChatRoomId.Value == _selectedChatRoomId.Value;
            }
            else if (messageDto.RecipientId.HasValue && _selectedUserId.HasValue)
            {
                isRelevant = messageDto.SenderId == _selectedUserId.Value;
            }

            if (isRelevant)
            {
                // Display the message
                AppendMessageToChat(messageDto.SenderUsername, messageDto.Content, messageDto.SentAt, false);

                // Save the message to the database
                SaveMessageToDatabase(messageDto);
            }
            else
            {
                // Notify about new message
                string source = messageDto.ChatRoomId.HasValue
                    ? _chatRooms.FirstOrDefault(r => r.Id == messageDto.ChatRoomId)?.Name
                    : _users.FirstOrDefault(u => u.Id == messageDto.SenderId)?.Username;

                if (source != null)
                {
                    lblNotification.Text = $"New message from: {source}";
                    tmrNotification.Start();
                }
            }
        }

        private void SaveMessageToDatabase(MessageDto messageDto)
        {
            try
            {
                var message = new ChatApp.Models.Message
                {
                    SenderId = messageDto.SenderId,
                    RecipientId = messageDto.RecipientId,
                    ChatRoomId = messageDto.ChatRoomId,
                    Content = messageDto.Content,
                    EncryptedContent = EncryptionService.Encrypt(messageDto.Content),
                    SentAt = messageDto.SentAt,
                    IsRead = true
                };

                _dbContext.Messages.Add(message);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving message: {ex.Message}");
            }
        }

        private void AppendMessageToChat(string username, string content, DateTime sentAt, bool isOwnMessage)
        {
            try
            {
                // Check if wbChat is null or if its Document or Body is null
                if (wbChat == null || wbChat.Document == null || wbChat.Document.Body == null)
                {
                    // Initialize with empty HTML if WebBrowser isn't ready
                    InitializeChat();

                    // If it's still not ready, delay and try again
                    if (wbChat == null || wbChat.Document == null || wbChat.Document.Body == null)
                    {
                        // Log the issue and return for now
                        Console.WriteLine("WebBrowser document not ready. Message not appended.");
                        return;
                    }
                }

                string timeStr = sentAt.ToString("HH:mm:ss");
                string alignment = isOwnMessage ? "right" : "left";
                string color = isOwnMessage ? "#DCF8C6" : "#FFFFFF";

                string messageHtml = $"<div style='text-align: {alignment}; margin: 5px;'>" +
                                     $"<div style='display: inline-block; background-color: {color}; " +
                                     $"border-radius: 10px; padding: 8px; max-width: 80%; text-align: left;'>" +
                                     $"<div style='font-weight: bold;'>{username}</div>" +
                                     $"<div>{content}</div>" +
                                     $"<div style='font-size: 0.8em; color: #888; text-align: right;'>{timeStr}</div>" +
                                     $"</div></div>";

                // Make sure the document body exists before trying to modify it
                if (wbChat.Document != null && wbChat.Document.Body != null)
                {
                    // Get the current HTML content and append the new message
                    string currentHtml = wbChat.DocumentText;

                    // Prevent null reference by checking if DocumentText is null or empty
                    if (string.IsNullOrEmpty(currentHtml) || !currentHtml.Contains("</body>"))
                    {
                        InitializeChat(); // Reinitialize chat if HTML is not valid
                        currentHtml = wbChat.DocumentText;
                    }

                    wbChat.DocumentText = currentHtml.Replace("</body>", messageHtml + "</body>");

                    // Scroll to bottom after a short delay to ensure content is loaded
                    System.Windows.Forms.Timer scrollTimer = new System.Windows.Forms.Timer
                    {
                        Interval = 100,
                        Enabled = true
                    };

                    scrollTimer.Tick += (sender, e) =>
                    {
                        try
                        {
                            if (wbChat.Document != null && wbChat.Document.Window != null &&
                                wbChat.Document.Body != null)
                            {
                                wbChat.Document.Window.ScrollTo(0, wbChat.Document.Body.ScrollRectangle.Height);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error scrolling WebBrowser: {ex.Message}");
                        }

                        scrollTimer.Enabled = false;
                        scrollTimer.Dispose();
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error appending message to chat: {ex.Message}");
            }
        }

        private void InitializeChat()
        {
            try
            {
                // Initialize WebBrowser with basic HTML structure
                string htmlTemplate =
                    "<!DOCTYPE html>" +
                    "<html><head>" +
                    "<meta charset='utf-8'>" +
                    "<style>" +
                    "body { font-family: 'Segoe UI', Arial, sans-serif; margin: 10px; }" +
                    "</style>" +
                    "</head><body></body></html>";

                wbChat.DocumentText = htmlTemplate;

                // Wait for document to load
                while (wbChat.ReadyState != WebBrowserReadyState.Complete)
                {
                    Application.DoEvents();
                    Thread.Sleep(50);
                }

                // Load chat history
                if (_selectedChatRoomId.HasValue)
                {
                    LoadChatRoomHistory(_selectedChatRoomId.Value);
                }
                else if (_selectedUserId.HasValue)
                {
                    LoadDirectMessageHistory(_selectedUserId.Value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing chat: {ex.Message}");
            }
        }

        private void LoadChatRoomHistory(int chatRoomId)
        {
            try
            {
                var chatRoom = _chatRooms.FirstOrDefault(r => r.Id == chatRoomId);
                if (chatRoom == null) return;

                lblChatTitle.Text = chatRoom.Name;

                // Initialize WebBrowser first
                string htmlTemplate =
                    "<!DOCTYPE html>" +
                    "<html><head>" +
                    "<meta charset='utf-8'>" +
                    "<style>" +
                    "body { font-family: 'Segoe UI', Arial, sans-serif; margin: 10px; }" +
                    "</style>" +
                    "</head><body></body></html>";

                wbChat.DocumentText = htmlTemplate;

                // Wait for document to load
                while (wbChat.ReadyState != WebBrowserReadyState.Complete)
                {
                    Application.DoEvents();
                    Thread.Sleep(50);
                }

                var messages = _dbContext.Messages
                    .Include(m => m.Sender)
                    .Where(m => m.ChatRoomId == chatRoomId)
                    .OrderBy(m => m.SentAt)
                    .Take(50) // Load last 50 messages for performance
                    .ToList();

                StringBuilder chatContent = new StringBuilder();
                foreach (var message in messages)
                {
                    string decryptedContent = EncryptionService.Decrypt(message.EncryptedContent);
                    bool isOwnMessage = message.SenderId == _currentUserId;
                    string timeStr = message.SentAt.ToString("HH:mm:ss");
                    string alignment = isOwnMessage ? "right" : "left";
                    string color = isOwnMessage ? "#DCF8C6" : "#FFFFFF";

                    chatContent.Append($"<div style='text-align: {alignment}; margin: 5px;'>");
                    chatContent.Append($"<div style='display: inline-block; background-color: {color}; ");
                    chatContent.Append($"border-radius: 10px; padding: 8px; max-width: 80%; text-align: left;'>");
                    chatContent.Append($"<div style='font-weight: bold;'>{message.Sender.Username}</div>");
                    chatContent.Append($"<div>{decryptedContent}</div>");
                    chatContent.Append($"<div style='font-size: 0.8em; color: #888; text-align: right;'>{timeStr}</div>");
                    chatContent.Append($"</div></div>");
                }

                // Set complete HTML at once instead of appending messages one by one
                wbChat.DocumentText =
                    "<!DOCTYPE html>" +
                    "<html><head>" +
                    "<meta charset='utf-8'>" +
                    "<style>" +
                    "body { font-family: 'Segoe UI', Arial, sans-serif; margin: 10px; }" +
                    "</style>" +
                    "</head><body>" +
                    chatContent.ToString() +
                    "</body></html>";

                // Scroll to bottom
                if (wbChat.Document != null && wbChat.Document.Window != null)
                {
                    wbChat.Document.Window.ScrollTo(0, wbChat.Document.Body.ScrollRectangle.Height);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading chat history: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDirectMessageHistory(int userId)
        {
            try
            {
                var user = _users.FirstOrDefault(u => u.Id == userId);
                if (user == null) return;

                lblChatTitle.Text = user.Username;

                var messages = _dbContext.Messages
                    .Include(m => m.Sender)
                    .Where(m =>
                        (m.SenderId == _currentUserId && m.RecipientId == userId) ||
                        (m.SenderId == userId && m.RecipientId == _currentUserId))
                    .OrderBy(m => m.SentAt)
                    .Take(50) // Load last 50 messages for performance
                    .ToList();

                foreach (var message in messages)
                {
                    string decryptedContent = EncryptionService.Decrypt(message.EncryptedContent);
                    bool isOwnMessage = message.SenderId == _currentUserId;
                    AppendMessageToChat(message.Sender.Username, decryptedContent, message.SentAt, isOwnMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading chat history: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            string content = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(content))
                return;

            try
            {
                var messageDto = new MessageDto
                {
                    SenderId = _currentUserId,
                    SenderUsername = _currentUser.Username,
                    RecipientId = _selectedUserId,
                    ChatRoomId = _selectedChatRoomId,
                    Content = content,
                    SentAt = DateTime.Now
                };

                // Send message via MQTT
                await MqttService.PublishMessageAsync(messageDto);

                // Display own message
                AppendMessageToChat(_currentUser.Username, content, messageDto.SentAt, true);

                // Save to database
                SaveMessageToDatabase(messageDto);

                // Clear message box
                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lstChatRooms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstChatRooms.SelectedIndex >= 0)
            {
                lstUsers.ClearSelected();
                _selectedUserId = null;
                _selectedChatRoomId = _chatRooms[lstChatRooms.SelectedIndex].Id;
                InitializeChat();
            }
        }

        private void lstUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstUsers.SelectedIndex >= 0)
            {
                lstChatRooms.ClearSelected();
                _selectedChatRoomId = null;
                _selectedUserId = _users[lstUsers.SelectedIndex].Id;
                InitializeChat();
            }
        }

        private void tmrNotification_Tick(object sender, EventArgs e)
        {
            lblNotification.Text = "";
            tmrNotification.Stop();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadChatRooms();
            LoadUsers();
        }

        private void btnCreateRoom_Click(object sender, EventArgs e)
        {
            using (var dialog = new CreateChatRoomForm(_currentUserId))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadChatRooms();
                }
            }
        }
    }
}