using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ChatApp.Models;

namespace ChatApp.Forms
{
    public partial class CreateChatRoomForm : Form
    {
        private int _creatorId;
        private ChatAppDbContext _dbContext;

        public CreateChatRoomForm(int creatorId)
        {
            InitializeComponent();
            _creatorId = creatorId;
            _dbContext = new ChatAppDbContext();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            string roomName = txtRoomName.Text.Trim();
            string description = txtDescription.Text.Trim();

            if (string.IsNullOrEmpty(roomName))
            {
                MessageBox.Show("Please enter a room name.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var chatRoom = new ChatRoom
                {
                    Name = roomName,
                    Description = description,
                    CreatorId = _creatorId,
                    CreatedAt = DateTime.Now
                };

                _dbContext.ChatRooms.Add(chatRoom);
                _dbContext.SaveChanges();

                // Add creator as a member
                var member = new ChatRoomMember
                {
                    ChatRoomId = chatRoom.Id,
                    UserId = _creatorId,
                    JoinedAt = DateTime.Now
                };

                _dbContext.ChatRoomMembers.Add(member);
                _dbContext.SaveChanges();

                MessageBox.Show("Chat room created successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create chat room: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}