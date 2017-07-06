using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChatClient.ChatService;

namespace ChatClient
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        ChatService.ChatServiceClient chatServer;
        Timer tmrSync;
        private void MainForm_Load(object sender, EventArgs e)
        {
            chatServer = new ChatService.ChatServiceClient();
            tmrSync = new Timer();
            tmrSync.Interval = 1000;
            tmrSync.Tick += TmrSync_Tick;
        }

        private void TmrSync_Tick(object sender, EventArgs e)
        {
            tmrSync.Enabled = false;

            FillRoomList();
            GetCurrentUserList();

            var messages = chatServer.GetMessages(currentUser);
            if (messages != null)
            {
                foreach (var item in messages)
                {
                    var tab = tabControl2.TabPages.Cast<TabPage>()
                         .Where(x => (x.Tag as ChatService.User) != null)
                         .FirstOrDefault(x => (x.Tag as ChatService.User).Id == item.From.Id);

                    if (tab == null)
                    {
                        tab = new TabPage(item.From.UserName);
                        tab.Tag = item.From;
                        tab.Controls.Add(new TextBox() { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true });
                        tabControl2.TabPages.Add(tab);
                    }

                    ShowMessage((tab.Controls[0] as TextBox), item.SendDate, item.From.UserName, item.Text);
                }
            }

            tmrSync.Enabled = true;
        }

        private void GetCurrentUserList()
        {
            var room = tabRoomAndUser.TabPages[1].Tag as Room;
            if (room != null)
            {
                var users = chatServer.GetUserList(room);
                FillUserList(room, users);
            }
        }

        ChatService.User currentUser;
        private void btnLogin_Click(object sender, EventArgs e)
        {
            currentUser = chatServer.Login(new ChatService.User() { UserName = txtUserName.Text });

            FillRoomList();

            tmrSync.Enabled = true;
        }

        private void FillRoomList()
        {
            lstRooms.Items.Clear();
            var roomList = chatServer.GetRoomList();
            lstRooms.Items.AddRange(roomList.Select(x => new ListViewItem() { Text = x.Name, Tag = x }).ToArray());
        }

        private void btnCreateRoom_Click(object sender, EventArgs e)
        {
            chatServer.CreateRoom(new ChatService.Room() { Name = txtRoomName.Text });

            FillRoomList();
        }

        private void lstRooms_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var room = lstRooms.SelectedItems[0].Tag as ChatService.Room;
            var users = chatServer.JoinRoom(room, currentUser);

            FillUserList(room, users);
        }

        private void FillUserList(Room room, User[] users)
        {
            lstUserList.Items.Clear();
            lstUserList.Items.AddRange(users.Select(x => new ListViewItem() { Text = x.UserName, Tag = x }).ToArray());

            tabRoomAndUser.TabPages[1].Tag = room;
            tabRoomAndUser.TabPages[1].Text = room.Name;
            tabRoomAndUser.SelectedIndex = 1;
        }

        private void lstUserList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var user = lstUserList.SelectedItems[0].Tag as ChatService.User;
            var tab = new TabPage(user.UserName);
            tab.Tag = user;

            tabControl2.TabPages.Add(tab);

            tab.Controls.Add(new TextBox() { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true });

            tabControl2.SelectTab(tab);
            textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var user = tabControl2.SelectedTab.Tag as ChatService.User;
            chatServer.SendMessage(new ChatService.Message()
            {
                From = currentUser,
                To = user,
                Text = textBox1.Text
            });

            var textArea = tabControl2.SelectedTab.Controls[0] as TextBox;
            ShowMessage(textArea, DateTime.Now, currentUser.UserName, textBox1.Text);

            textBox1.Text = string.Empty;

            textBox1.Focus();
        }

        private void ShowMessage(TextBox textArea, DateTime date, string userName, string message)
        {
            textArea.Text += string.Format("\r\n {0} - {1} : {2}", date.ToString("MM:ss"), userName, message);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                button1.PerformClick();
                e.Handled = true;
            }

        }
    }
}
