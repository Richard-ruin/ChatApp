namespace ChatApp.Forms
{
    partial class AdminDashboardForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabUsers = new System.Windows.Forms.TabPage();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.lblOnlineUsers = new System.Windows.Forms.Label();
            this.lblTotalUsers = new System.Windows.Forms.Label();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.tabChatRooms = new System.Windows.Forms.TabPage();
            this.btnAddRoom = new System.Windows.Forms.Button();
            this.lblTotalRooms = new System.Windows.Forms.Label();
            this.dgvChatRooms = new System.Windows.Forms.DataGridView();
            this.tabStatistics = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstActiveRooms = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstActiveUsers = new System.Windows.Forms.ListBox();
            this.lblWeekMessages = new System.Windows.Forms.Label();
            this.lblTodayMessages = new System.Windows.Forms.Label();
            this.lblRoomMessages = new System.Windows.Forms.Label();
            this.lblDirectMessages = new System.Windows.Forms.Label();
            this.lblTotalMessages = new System.Windows.Forms.Label();
            this.btnOpenChat = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.tabControl.SuspendLayout();
            this.tabUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.tabChatRooms.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChatRooms)).BeginInit();
            this.tabStatistics.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();

            // tabControl
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabUsers);
            this.tabControl.Controls.Add(this.tabChatRooms);
            this.tabControl.Controls.Add(this.tabStatistics);
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(960, 506);
            this.tabControl.TabIndex = 0;

            // tabUsers
            this.tabUsers.Controls.Add(this.btnAddUser);
            this.tabUsers.Controls.Add(this.lblOnlineUsers);
            this.tabUsers.Controls.Add(this.lblTotalUsers);
            this.tabUsers.Controls.Add(this.dgvUsers);
            this.tabUsers.Location = new System.Drawing.Point(4, 26);
            this.tabUsers.Name = "tabUsers";
            this.tabUsers.Padding = new System.Windows.Forms.Padding(3);
            this.tabUsers.Size = new System.Drawing.Size(952, 476);
            this.tabUsers.TabIndex = 0;
            this.tabUsers.Text = "Users";
            this.tabUsers.UseVisualStyleBackColor = true;

            // btnAddUser
            this.btnAddUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(188)))));
            this.btnAddUser.FlatAppearance.BorderSize = 0;
            this.btnAddUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddUser.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddUser.ForeColor = System.Drawing.Color.White;
            this.btnAddUser.Location = new System.Drawing.Point(826, 16);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(110, 30);
            this.btnAddUser.TabIndex = 3;
            this.btnAddUser.Text = "Add User";
            this.btnAddUser.UseVisualStyleBackColor = false;
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);

            // lblOnlineUsers
            this.lblOnlineUsers.AutoSize = true;
            this.lblOnlineUsers.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblOnlineUsers.Location = new System.Drawing.Point(175, 16);
            this.lblOnlineUsers.Name = "lblOnlineUsers";
            this.lblOnlineUsers.Size = new System.Drawing.Size(107, 19);
            this.lblOnlineUsers.TabIndex = 2;
            this.lblOnlineUsers.Text = "Online Users: 0";

            // lblTotalUsers
            this.lblTotalUsers.AutoSize = true;
            this.lblTotalUsers.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTotalUsers.Location = new System.Drawing.Point(20, 16);
            this.lblTotalUsers.Name = "lblTotalUsers";
            this.lblTotalUsers.Size = new System.Drawing.Size(90, 19);
            this.lblTotalUsers.TabIndex = 1;
            this.lblTotalUsers.Text = "Total Users: 0";

            // dgvUsers
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvUsers.BackgroundColor = System.Drawing.Color.White;
            this.dgvUsers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvUsers.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Location = new System.Drawing.Point(15, 50);
            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.RowTemplate.Height = 25;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(921, 411);
            this.dgvUsers.TabIndex = 0;
            this.dgvUsers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsers_CellClick);

            // tabChatRooms
            this.tabChatRooms.Controls.Add(this.btnAddRoom);
            this.tabChatRooms.Controls.Add(this.lblTotalRooms);
            this.tabChatRooms.Controls.Add(this.dgvChatRooms);
            this.tabChatRooms.Location = new System.Drawing.Point(4, 26);
            this.tabChatRooms.Name = "tabChatRooms";
            this.tabChatRooms.Padding = new System.Windows.Forms.Padding(3);
            this.tabChatRooms.Size = new System.Drawing.Size(952, 476);
            this.tabChatRooms.TabIndex = 1;
            this.tabChatRooms.Text = "Chat Rooms";
            this.tabChatRooms.UseVisualStyleBackColor = true;

            // btnAddRoom
            this.btnAddRoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddRoom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(188)))));
            this.btnAddRoom.FlatAppearance.BorderSize = 0;
            this.btnAddRoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddRoom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddRoom.ForeColor = System.Drawing.Color.White;
            this.btnAddRoom.Location = new System.Drawing.Point(826, 16);
            this.btnAddRoom.Name = "btnAddRoom";
            this.btnAddRoom.Size = new System.Drawing.Size(110, 30);
            this.btnAddRoom.TabIndex = 4;
            this.btnAddRoom.Text = "Add Room";
            this.btnAddRoom.UseVisualStyleBackColor = false;
            this.btnAddRoom.Click += new System.EventHandler(this.btnAddRoom_Click);

            // lblTotalRooms
            this.lblTotalRooms.AutoSize = true;
            this.lblTotalRooms.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTotalRooms.Location = new System.Drawing.Point(20, 16);
            this.lblTotalRooms.Name = "lblTotalRooms";
            this.lblTotalRooms.Size = new System.Drawing.Size(97, 19);
            this.lblTotalRooms.TabIndex = 2;
            this.lblTotalRooms.Text = "Total Rooms: 0";

            // dgvChatRooms
            this.dgvChatRooms.AllowUserToAddRows = false;
            this.dgvChatRooms.AllowUserToDeleteRows = false;
            this.dgvChatRooms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvChatRooms.BackgroundColor = System.Drawing.Color.White;
            this.dgvChatRooms.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvChatRooms.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvChatRooms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChatRooms.Location = new System.Drawing.Point(15, 50);
            this.dgvChatRooms.MultiSelect = false;
            this.dgvChatRooms.Name = "dgvChatRooms";
            this.dgvChatRooms.ReadOnly = true;
            this.dgvChatRooms.RowHeadersVisible = false;
            this.dgvChatRooms.RowTemplate.Height = 25;
            this.dgvChatRooms.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChatRooms.Size = new System.Drawing.Size(921, 411);
            this.dgvChatRooms.TabIndex = 0;
            this.dgvChatRooms.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChatRooms_CellClick);

            // tabStatistics
            this.tabStatistics.Controls.Add(this.groupBox2);
            this.tabStatistics.Controls.Add(this.groupBox1);
            this.tabStatistics.Controls.Add(this.lblWeekMessages);
            this.tabStatistics.Controls.Add(this.lblTodayMessages);
            this.tabStatistics.Controls.Add(this.lblRoomMessages);
            this.tabStatistics.Controls.Add(this.lblDirectMessages);
            this.tabStatistics.Controls.Add(this.lblTotalMessages);
            this.tabStatistics.Location = new System.Drawing.Point(4, 26);
            this.tabStatistics.Name = "tabStatistics";
            this.tabStatistics.Padding = new System.Windows.Forms.Padding(3);
            this.tabStatistics.Size = new System.Drawing.Size(952, 476);
            this.tabStatistics.TabIndex = 2;
            this.tabStatistics.Text = "Statistics";
            this.tabStatistics.UseVisualStyleBackColor = true;

            // groupBox2
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Location = new System.Drawing.Point(486, 180);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(450, 281);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Most Active Chat Rooms";

            // lstActiveRooms
            this.lstActiveRooms.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstActiveRooms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstActiveRooms.FormattingEnabled = true;
            this.lstActiveRooms.ItemHeight = 17;
            this.lstActiveRooms.Location = new System.Drawing.Point(3, 21);
            this.lstActiveRooms.Name = "lstActiveRooms";
            this.lstActiveRooms.Size = new System.Drawing.Size(444, 257);
            this.lstActiveRooms.TabIndex = 0;

            // groupBox1
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(20, 180);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(450, 281);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Most Active Users";

            // lstActiveUsers
            this.lstActiveUsers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstActiveUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstActiveUsers.FormattingEnabled = true;
            this.lstActiveUsers.ItemHeight = 17;
            this.lstActiveUsers.Location = new System.Drawing.Point(3, 21);
            this.lstActiveUsers.Name = "lstActiveUsers";
            this.lstActiveUsers.Size = new System.Drawing.Size(444, 257);
            this.lstActiveUsers.TabIndex = 0;

            // lblWeekMessages
            this.lblWeekMessages.AutoSize = true;
            this.lblWeekMessages.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblWeekMessages.Location = new System.Drawing.Point(20, 140);
            this.lblWeekMessages.Name = "lblWeekMessages";
            this.lblWeekMessages.Size = new System.Drawing.Size(87, 19);
            this.lblWeekMessages.TabIndex = 4;
            this.lblWeekMessages.Text = "Last 7 Days: 0";

            // lblTodayMessages
            this.lblTodayMessages.AutoSize = true;
            this.lblTodayMessages.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTodayMessages.Location = new System.Drawing.Point(20, 110);
            this.lblTodayMessages.Name = "lblTodayMessages";
            this.lblTodayMessages.Size = new System.Drawing.Size(133, 19);
            this.lblTodayMessages.TabIndex = 3;
            this.lblTodayMessages.Text = "Today's Messages: 0";

            // lblRoomMessages
            this.lblRoomMessages.AutoSize = true;
            this.lblRoomMessages.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblRoomMessages.Location = new System.Drawing.Point(20, 80);
            this.lblRoomMessages.Name = "lblRoomMessages";
            this.lblRoomMessages.Size = new System.Drawing.Size(125, 19);
            this.lblRoomMessages.TabIndex = 2;
            this.lblRoomMessages.Text = "Room Messages: 0";

            // lblDirectMessages
            this.lblDirectMessages.AutoSize = true;
            this.lblDirectMessages.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDirectMessages.Location = new System.Drawing.Point(20, 50);
            this.lblDirectMessages.Name = "lblDirectMessages";
            this.lblDirectMessages.Size = new System.Drawing.Size(127, 19);
            this.lblDirectMessages.TabIndex = 1;
            this.lblDirectMessages.Text = "Direct Messages: 0";

            // lblTotalMessages
            this.lblTotalMessages.AutoSize = true;
            this.lblTotalMessages.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTotalMessages.Location = new System.Drawing.Point(20, 20);
            this.lblTotalMessages.Name = "lblTotalMessages";
            this.lblTotalMessages.Size = new System.Drawing.Size(123, 19);
            this.lblTotalMessages.TabIndex = 0;
            this.lblTotalMessages.Text = "Total Messages: 0";

            // btnOpenChat
            this.btnOpenChat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenChat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(188)))));
            this.btnOpenChat.FlatAppearance.BorderSize = 0;
            this.btnOpenChat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenChat.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnOpenChat.ForeColor = System.Drawing.Color.White;
            this.btnOpenChat.Location = new System.Drawing.Point(842, 524);
            this.btnOpenChat.Name = "btnOpenChat";
            this.btnOpenChat.Size = new System.Drawing.Size(130, 35);
            this.btnOpenChat.TabIndex = 1;
            this.btnOpenChat.Text = "Open Chat";
            this.btnOpenChat.UseVisualStyleBackColor = false;
            this.btnOpenChat.Click += new System.EventHandler(this.btnOpenChat_Click);

            // btnRefresh
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(188)))));
            this.btnRefresh.FlatAppearance.BorderSize = 1;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnRefresh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(188)))));
            this.btnRefresh.Location = new System.Drawing.Point(706, 524);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(130, 35);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Refresh Data";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // tmrRefresh
            this.tmrRefresh.Interval = 30000;
            this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);

            // AdminDashboardForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(984, 571);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnOpenChat);
            this.Controls.Add(this.tabControl);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "AdminDashboardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin Dashboard";
            this.tabControl.ResumeLayout(false);
            this.tabUsers.ResumeLayout(false);
            this.tabUsers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.tabChatRooms.ResumeLayout(false);
            this.tabChatRooms.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChatRooms)).EndInit();
            this.tabStatistics.ResumeLayout(false);
            this.tabStatistics.PerformLayout();
            this.groupBox2.Controls.Add(this.lstActiveRooms);
            this.groupBox1.Controls.Add(this.lstActiveUsers);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabUsers;
        private System.Windows.Forms.TabPage tabChatRooms;
        private System.Windows.Forms.TabPage tabStatistics;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Label lblTotalUsers;
        private System.Windows.Forms.Label lblOnlineUsers;
        private System.Windows.Forms.DataGridView dgvChatRooms;
        private System.Windows.Forms.Label lblTotalRooms;
        private System.Windows.Forms.Label lblTotalMessages;
        private System.Windows.Forms.Label lblDirectMessages;
        private System.Windows.Forms.Label lblRoomMessages;
        private System.Windows.Forms.Label lblTodayMessages;
        private System.Windows.Forms.Label lblWeekMessages;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstActiveUsers;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lstActiveRooms;
        private System.Windows.Forms.Button btnOpenChat;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.Button btnAddRoom;
        private System.Windows.Forms.Timer tmrRefresh;
    }
}