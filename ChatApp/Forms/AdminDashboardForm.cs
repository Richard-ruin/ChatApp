using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ChatApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Forms
{
    public partial class AdminDashboardForm : Form
    {
        private int _adminId;
        private User _adminUser;
        private ChatAppDbContext _dbContext;

        public AdminDashboardForm(int adminId)
        {
            InitializeComponent();
            _adminId = adminId;
            _dbContext = new ChatAppDbContext();

            // Load admin info
            _adminUser = _dbContext.Users
                .FirstOrDefault(u => u.Id == _adminId);

            if (_adminUser == null || _adminUser.Role != UserRole.Admin)
            {
                MessageBox.Show("Unauthorized access.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            this.Text = $"Admin Dashboard - {_adminUser.Username}";
            this.Load += AdminDashboardForm_Load;
            this.FormClosing += AdminDashboardForm_FormClosing;
        }

        private void AdminDashboardForm_Load(object sender, EventArgs e)
        {
            // Update admin status
            _adminUser.IsOnline = true;
            _adminUser.LastLogin = DateTime.Now;
            _dbContext.SaveChanges();

            // Initialize tabs
            RefreshUserData();
            RefreshChatRoomData();
            RefreshMessageStats();

            // Set up timer for auto-refresh
            tmrRefresh.Start();
        }

        private void AdminDashboardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Update admin status
            _adminUser.IsOnline = false;
            _dbContext.SaveChanges();
        }

        private void RefreshUserData()
        {
            try
            {
                var users = _dbContext.Users.ToList();

                // Configure DataGridView
                dgvUsers.DataSource = null;
                dgvUsers.AutoGenerateColumns = false;

                if (dgvUsers.Columns.Count == 0)
                {
                    dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Id",
                        HeaderText = "ID",
                        Width = 50
                    });

                    dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Username",
                        HeaderText = "Username",
                        Width = 120
                    });

                    dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Email",
                        HeaderText = "Email",
                        Width = 180
                    });

                    dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Role",
                        HeaderText = "Role",
                        Width = 80
                    });

                    dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "IsOnline",
                        HeaderText = "Status",
                        Width = 80
                    });

                    dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "CreatedAt",
                        HeaderText = "Created At",
                        Width = 150
                    });

                    dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "LastLogin",
                        HeaderText = "Last Login",
                        Width = 150
                    });

                    var btnEdit = new DataGridViewButtonColumn
                    {
                        HeaderText = "Action",
                        Text = "Edit",
                        UseColumnTextForButtonValue = true,
                        Width = 70
                    };

                    dgvUsers.Columns.Add(btnEdit);
                }

                dgvUsers.DataSource = users;

                // Update user count
                lblTotalUsers.Text = $"Total Users: {users.Count}";
                lblOnlineUsers.Text = $"Online Users: {users.Count(u => u.IsOnline)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user data: {ex.Message}",
                    "Data Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshChatRoomData()
        {
            try
            {
                var chatRooms = _dbContext.ChatRooms
                    .Include(cr => cr.Creator)
                    .ToList();

                // Configure DataGridView
                dgvChatRooms.DataSource = null;
                dgvChatRooms.AutoGenerateColumns = false;

                if (dgvChatRooms.Columns.Count == 0)
                {
                    dgvChatRooms.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Id",
                        HeaderText = "ID",
                        Width = 50
                    });

                    dgvChatRooms.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Name",
                        HeaderText = "Room Name",
                        Width = 150
                    });

                    dgvChatRooms.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Description",
                        HeaderText = "Description",
                        Width = 200
                    });

                    var creatorColumn = new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Creator",
                        Width = 120
                    };

                    dgvChatRooms.Columns.Add(creatorColumn);

                    dgvChatRooms.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "CreatedAt",
                        HeaderText = "Created At",
                        Width = 150
                    });

                    var memberCountColumn = new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Members",
                        Width = 80
                    };

                    dgvChatRooms.Columns.Add(memberCountColumn);

                    var btnManage = new DataGridViewButtonColumn
                    {
                        HeaderText = "Action",
                        Text = "Manage",
                        UseColumnTextForButtonValue = true,
                        Width = 70
                    };

                    dgvChatRooms.Columns.Add(btnManage);
                }

                var roomsWithCounts = chatRooms.Select(cr => new
                {
                    cr.Id,
                    cr.Name,
                    cr.Description,
                    Creator = cr.Creator?.Username ?? "Unknown",
                    cr.CreatedAt,
                    MemberCount = _dbContext.ChatRoomMembers.Count(m => m.ChatRoomId == cr.Id)
                }).ToList();

                dgvChatRooms.DataSource = roomsWithCounts;

                // Set values for non-bound columns
                for (int i = 0; i < roomsWithCounts.Count; i++)
                {
                    dgvChatRooms.Rows[i].Cells[3].Value = roomsWithCounts[i].Creator;
                    dgvChatRooms.Rows[i].Cells[5].Value = roomsWithCounts[i].MemberCount;
                }

                // Update chat room count
                lblTotalRooms.Text = $"Total Rooms: {chatRooms.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading chat room data: {ex.Message}",
                    "Data Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshMessageStats()
        {
            try
            {
                // Get message stats
                int totalMessages = _dbContext.Messages.Count();
                int directMessages = _dbContext.Messages.Count(m => m.RecipientId != null);
                int roomMessages = _dbContext.Messages.Count(m => m.ChatRoomId != null);

                // Today's messages
                DateTime today = DateTime.Today;
                int todayMessages = _dbContext.Messages.Count(m => m.SentAt.Date == today);

                // Last 7 days
                DateTime lastWeek = DateTime.Today.AddDays(-7);
                int weekMessages = _dbContext.Messages.Count(m => m.SentAt >= lastWeek);

                // Update labels
                lblTotalMessages.Text = $"Total Messages: {totalMessages}";
                lblDirectMessages.Text = $"Direct Messages: {directMessages}";
                lblRoomMessages.Text = $"Room Messages: {roomMessages}";
                lblTodayMessages.Text = $"Today's Messages: {todayMessages}";
                lblWeekMessages.Text = $"Last 7 Days: {weekMessages}";

                // Calculate most active users
                var activeUsers = _dbContext.Messages
                    .GroupBy(m => m.SenderId)
                    .Select(g => new { UserId = g.Key, MessageCount = g.Count() })
                    .OrderByDescending(x => x.MessageCount)
                    .Take(5)
                    .ToList();

                lstActiveUsers.Items.Clear();
                foreach (var user in activeUsers)
                {
                    var username = _dbContext.Users
                        .Where(u => u.Id == user.UserId)
                        .Select(u => u.Username)
                        .FirstOrDefault() ?? "Unknown";

                    lstActiveUsers.Items.Add($"{username} - {user.MessageCount} messages");
                }

                // Calculate most active rooms
                var activeRooms = _dbContext.Messages
                    .Where(m => m.ChatRoomId != null)
                    .GroupBy(m => m.ChatRoomId)
                    .Select(g => new { RoomId = g.Key, MessageCount = g.Count() })
                    .OrderByDescending(x => x.MessageCount)
                    .Take(5)
                    .ToList();

                lstActiveRooms.Items.Clear();
                foreach (var room in activeRooms)
                {
                    var roomName = _dbContext.ChatRooms
                        .Where(r => r.Id == room.RoomId)
                        .Select(r => r.Name)
                        .FirstOrDefault() ?? "Unknown";

                    lstActiveRooms.Items.Add($"{roomName} - {room.MessageCount} messages");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading message statistics: {ex.Message}",
                    "Data Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOpenChat_Click(object sender, EventArgs e)
        {
            var chatForm = new ChatForm(_adminId);
            chatForm.Show();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshUserData();
            RefreshChatRoomData();
            RefreshMessageStats();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            using (var dialog = new RegisterForm())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    RefreshUserData();
                }
            }
        }

        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            using (var dialog = new CreateChatRoomForm(_adminId))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    RefreshChatRoomData();
                }
            }
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if clicked on the Edit button column
            if (e.RowIndex >= 0 && e.ColumnIndex == 7)
            {
                var userId = (int)dgvUsers.Rows[e.RowIndex].Cells[0].Value;

                // Don't allow editing yourself
                if (userId == _adminId)
                {
                    MessageBox.Show("You cannot edit your own account from here.",
                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var dialog = new EditUserForm(userId))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        RefreshUserData();
                    }
                }
            }
        }

        private void dgvChatRooms_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if clicked on the Manage button column
            if (e.RowIndex >= 0 && e.ColumnIndex == 6)
            {
                var roomId = (int)dgvChatRooms.Rows[e.RowIndex].Cells[0].Value;

                using (var dialog = new ManageChatRoomForm(roomId))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        RefreshChatRoomData();
                    }
                }
            }
        }

        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            // Auto-refresh data every 30 seconds
            RefreshUserData();
            RefreshChatRoomData();
            RefreshMessageStats();
        }
    }
}