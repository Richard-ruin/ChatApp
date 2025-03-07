using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ChatApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Forms
{
    public partial class ManageChatRoomForm : Form
    {
        private int _roomId;
        private ChatRoom _chatRoom;
        private ChatAppDbContext _dbContext;

        public ManageChatRoomForm(int roomId)
        {
            InitializeComponent();
            _roomId = roomId;
            _dbContext = new ChatAppDbContext();

            // Load chat room data
            _chatRoom = _dbContext.ChatRooms
                .Include(cr => cr.Creator)
                .Include(cr => cr.Members)
                    .ThenInclude(m => m.User)
                .FirstOrDefault(cr => cr.Id == _roomId);

            if (_chatRoom == null)
            {
                MessageBox.Show("Chat room not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            // Populate fields
            txtRoomName.Text = _chatRoom.Name;
            txtDescription.Text = _chatRoom.Description;
            lblCreator.Text = $"Created by: {_chatRoom.Creator?.Username ?? "Unknown"}";
            lblCreatedAt.Text = $"Created on: {_chatRoom.CreatedAt:g}";

            LoadMembers();
            LoadNonMembers();

            this.Text = $"Manage Chat Room - {_chatRoom.Name}";
        }

        // Custom class to store user data for list items
        private class UserListItem
        {
            public int UserId { get; set; }
            public string DisplayName { get; set; }

            public override string ToString()
            {
                return DisplayName;
            }
        }

        private void LoadMembers()
        {
            lstMembers.Items.Clear();

            foreach (var member in _chatRoom.Members.OrderBy(m => m.User.Username))
            {
                string status = member.User.IsOnline ? "🟢" : "⚪";
                lstMembers.Items.Add($"{status} {member.User.Username}");
            }

            lblMemberCount.Text = $"Members: {_chatRoom.Members.Count}";
        }

        private void LoadNonMembers()
        {
            // Get users who are not members of the chat room
            var memberIds = _chatRoom.Members.Select(m => m.UserId).ToList();
            var nonMembers = _dbContext.Users
                .Where(u => !memberIds.Contains(u.Id))
                .OrderBy(u => u.Username)
                .ToList();

            lstAvailableUsers.Items.Clear();

            foreach (var user in nonMembers)
            {
                string status = user.IsOnline ? "🟢" : "⚪";
                // Create a UserListItem instead of using the Tag property
                var item = new UserListItem
                {
                    UserId = user.Id,
                    DisplayName = $"{status} {user.Username}"
                };
                lstAvailableUsers.Items.Add(item);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string roomName = txtRoomName.Text.Trim();
            string description = txtDescription.Text.Trim();

            if (string.IsNullOrEmpty(roomName))
            {
                MessageBox.Show("Room name is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Check if room name already exists
                var existingRoom = _dbContext.ChatRooms
                    .FirstOrDefault(r => r.Name == roomName && r.Id != _roomId);

                if (existingRoom != null)
                {
                    MessageBox.Show("A chat room with this name already exists.",
                        "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Update chat room information
                _chatRoom.Name = roomName;
                _chatRoom.Description = description;

                _dbContext.SaveChanges();

                MessageBox.Show("Chat room updated successfully.", "Update Complete",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Update Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnAddMember_Click(object sender, EventArgs e)
        {
            if (lstAvailableUsers.SelectedItem == null)
            {
                MessageBox.Show("Please select a user to add.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Get userId from the UserListItem
                int userId = ((UserListItem)lstAvailableUsers.SelectedItem).UserId;

                // Add user to chat room
                var newMember = new ChatRoomMember
                {
                    ChatRoomId = _roomId,
                    UserId = userId,
                    JoinedAt = DateTime.Now
                };

                _dbContext.ChatRoomMembers.Add(newMember);
                _dbContext.SaveChanges();

                // Refresh chat room data
                _chatRoom = _dbContext.ChatRooms
                    .Include(cr => cr.Creator)
                    .Include(cr => cr.Members)
                        .ThenInclude(m => m.User)
                    .FirstOrDefault(cr => cr.Id == _roomId);

                LoadMembers();
                LoadNonMembers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemoveMember_Click(object sender, EventArgs e)
        {
            if (lstMembers.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a member to remove.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Get the username (without status indicator)
                string selectedItemText = lstMembers.SelectedItem.ToString();
                string username = selectedItemText.Substring(2).Trim();

                // Find the member
                var member = _chatRoom.Members
                    .FirstOrDefault(m => m.User.Username == username);

                if (member == null)
                {
                    MessageBox.Show("Member not found.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Check if this is the creator
                if (member.UserId == _chatRoom.CreatorId)
                {
                    MessageBox.Show("Cannot remove the chat room creator.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Remove the member
                _dbContext.ChatRoomMembers.Remove(member);
                _dbContext.SaveChanges();

                // Refresh chat room data
                _chatRoom = _dbContext.ChatRooms
                    .Include(cr => cr.Creator)
                    .Include(cr => cr.Members)
                        .ThenInclude(m => m.User)
                    .FirstOrDefault(cr => cr.Id == _roomId);

                LoadMembers();
                LoadNonMembers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                $"Are you sure you want to delete the chat room '{_chatRoom.Name}'?\nThis action cannot be undone.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Check if chat room has messages
                    bool hasMessages = _dbContext.Messages.Any(m => m.ChatRoomId == _roomId);

                    if (hasMessages)
                    {
                        var confirmResult = MessageBox.Show(
                            "This chat room has messages. " +
                            "Deleting this room will also delete all messages. " +
                            "Do you want to continue?",
                            "Warning",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button2);

                        if (confirmResult != DialogResult.Yes)
                        {
                            return;
                        }

                        // Delete chat room messages
                        var messages = _dbContext.Messages
                            .Where(m => m.ChatRoomId == _roomId)
                            .ToList();
                        _dbContext.Messages.RemoveRange(messages);
                    }

                    // Delete chat room members
                    var members = _dbContext.ChatRoomMembers
                        .Where(m => m.ChatRoomId == _roomId)
                        .ToList();
                    _dbContext.ChatRoomMembers.RemoveRange(members);

                    // Delete the chat room
                    _dbContext.ChatRooms.Remove(_chatRoom);
                    _dbContext.SaveChanges();

                    MessageBox.Show("Chat room deleted successfully.", "Chat Room Deleted",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Delete Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}