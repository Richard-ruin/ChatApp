using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ChatApp.Models;

namespace ChatApp.Forms
{
    public partial class EditUserForm : Form
    {
        private int _userId;
        private User _user;
        private ChatAppDbContext _dbContext;

        public EditUserForm(int userId)
        {
            InitializeComponent();
            _userId = userId;
            _dbContext = new ChatAppDbContext();

            // Load user data
            _user = _dbContext.Users.FirstOrDefault(u => u.Id == _userId);

            if (_user == null)
            {
                MessageBox.Show("User not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            // Populate fields
            txtUsername.Text = _user.Username;
            txtEmail.Text = _user.Email;
            cmbRole.SelectedIndex = _user.Role == UserRole.Admin ? 0 : 1;

            this.Text = $"Edit User - {_user.Username}";
        }

        private void chkResetPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.Enabled = chkResetPassword.Checked;
            if (!chkResetPassword.Checked)
            {
                txtPassword.Clear();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            UserRole role = cmbRole.SelectedIndex == 0 ? UserRole.Admin : UserRole.User;

            // Validate input
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Username and email are required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if password needs to be reset
            if (chkResetPassword.Checked)
            {
                string password = txtPassword.Text;

                if (string.IsNullOrEmpty(password) || password.Length < 6)
                {
                    MessageBox.Show("Password must be at least 6 characters long.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            try
            {
                // Check if username or email already used by another user
                var existingUser = _dbContext.Users.FirstOrDefault(u =>
                    (u.Username == username || u.Email == email) && u.Id != _userId);

                if (existingUser != null)
                {
                    if (existingUser.Username == username)
                    {
                        MessageBox.Show("Username already exists. Please choose a different username.",
                            "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Email already registered. Please use a different email address.",
                            "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    return;
                }

                // Update user information
                _user.Username = username;
                _user.Email = email;
                _user.Role = role;

                if (chkResetPassword.Checked)
                {
                    _user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text);
                }

                _dbContext.SaveChanges();

                MessageBox.Show("User information updated successfully.", "Update Complete",
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                $"Are you sure you want to delete the user '{_user.Username}'?\nThis action cannot be undone.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Check if user has any dependencies
                    bool hasDependencies = false;

                    // Check for messages
                    if (_dbContext.Messages.Any(m => m.SenderId == _userId || m.RecipientId == _userId))
                    {
                        hasDependencies = true;
                    }

                    // Check for chat room creator
                    if (_dbContext.ChatRooms.Any(r => r.CreatorId == _userId))
                    {
                        hasDependencies = true;
                    }

                    // Check for chat room membership
                    if (_dbContext.ChatRoomMembers.Any(m => m.UserId == _userId))
                    {
                        hasDependencies = true;
                    }

                    if (hasDependencies)
                    {
                        var confirmResult = MessageBox.Show(
                            "This user has messages or is a member of chat rooms. " +
                            "Deleting this user will also delete all related data. " +
                            "Do you want to continue?",
                            "Warning",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button2);

                        if (confirmResult != DialogResult.Yes)
                        {
                            return;
                        }

                        // Remove chat room memberships
                        var memberships = _dbContext.ChatRoomMembers.Where(m => m.UserId == _userId).ToList();
                        _dbContext.ChatRoomMembers.RemoveRange(memberships);

                        // Remove messages
                        var messages = _dbContext.Messages
                            .Where(m => m.SenderId == _userId || m.RecipientId == _userId)
                            .ToList();
                        _dbContext.Messages.RemoveRange(messages);

                        // For rooms created by this user, either delete them or transfer ownership
                        var rooms = _dbContext.ChatRooms.Where(r => r.CreatorId == _userId).ToList();
                        foreach (var room in rooms)
                        {
                            // Find another member to transfer ownership to
                            var newOwner = _dbContext.ChatRoomMembers
                                .Where(m => m.ChatRoomId == room.Id && m.UserId != _userId)
                                .FirstOrDefault();

                            if (newOwner != null)
                            {
                                room.CreatorId = newOwner.UserId;
                            }
                            else
                            {
                                _dbContext.ChatRooms.Remove(room);
                            }
                        }
                    }

                    // Delete the user
                    _dbContext.Users.Remove(_user);
                    _dbContext.SaveChanges();

                    MessageBox.Show("User deleted successfully.", "User Deleted",
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

        private bool IsValidEmail(string email)
        {
            // Basic email validation
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}