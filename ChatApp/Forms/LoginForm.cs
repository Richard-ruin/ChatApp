using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ChatApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Forms
{
    public partial class LoginForm : Form
    {
        private ChatAppDbContext _dbContext;

        public LoginForm()
        {
            InitializeComponent();
            _dbContext = new ChatAppDbContext();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);

                if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    // Update user status
                    user.IsOnline = true;
                    user.LastLogin = DateTime.Now;
                    _dbContext.SaveChanges();

                    // Open main form based on user role
                    if (user.Role == UserRole.Admin)
                    {
                        var adminForm = new AdminDashboardForm(user.Id);
                        this.Hide();
                        adminForm.FormClosed += (s, args) => this.Close();
                        adminForm.Show();
                    }
                    else
                    {
                        var chatForm = new ChatForm(user.Id);
                        this.Hide();
                        chatForm.FormClosed += (s, args) => this.Close();
                        chatForm.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Login Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            var registerForm = new RegisterForm();
            registerForm.ShowDialog();
        }
    }
}