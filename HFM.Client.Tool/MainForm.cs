/*
 * HFM.NET - Client.Tool Main Form
 * Copyright (C) 2009-2016 Ryan Harlamert (harlam357)
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; version 2
 * of the License. See the included file GPLv2.TXT.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

using HFM.Client.ObjectModel;

namespace HFM.Client.Tool
{
    public partial class MainForm : Form
    {
        private FahClientConnection _fahClient;

        private int _totalBytesSent;
        private int _totalBytesReceived;
        private string _debugBufferFileName;

        public MainForm()
        {
            //_fahClient.MessageReceived += FahClientMessageReceived;
            //_fahClient.ConnectedChanged += FahClientConnectedChanged;
            //_fahClient.DataSent += FahClientDataSent;
            //_fahClient.DataReceived += FahClientDataReceived;

            InitializeComponent();

            base.Text = $"HFM Client Tool v{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion}";
        }

        private void FahClientMessageReceived(FahClientMessage message)
        {
            AppendTextToMessageDisplayTextBox(String.Empty);
            AppendTextToMessageDisplayTextBox(FahClientMessageHelper.FormatForDisplay(message));
            var identifier = message.Identifier.ToString();
            SetStatusLabelText(identifier);
            AddTextToStatusMessageListBox(identifier);
            FahClientDataReceived(message);

            if (message.Identifier.MessageType == FahClientMessageType.SlotInfo)
            {
                var slotCollection = SlotCollection.Load(message.MessageText);
                foreach (var slot in slotCollection)
                {
                    ExecuteCommand("slot-options " + slot.ID + " client-type client-subtype cpu-usage machine-id max-packet-size core-priority next-unit-percentage max-units checkpoint pause-on-start gpu-index gpu-usage");
                    ExecuteCommand("simulation-info " + slot.ID);
                }
            }
        }

        private void AppendTextToMessageDisplayTextBox(string text)
        {
            MessageDisplayTextBox.BeginInvokeOnUIThread(t => MessageDisplayTextBox.AppendText(t), text);
        }

        private void SetStatusLabelText(string text)
        {
            this.BeginInvokeOnUIThread(t => StatusLabel.Text = t, text);
        }

        private void AddTextToStatusMessageListBox(string text)
        {
            StatusMessageListBox.BeginInvokeOnUIThread(t =>
            {
                StatusMessageListBox.Items.Add(text);
                StatusMessageListBox.SelectedIndex = StatusMessageListBox.Items.Count - 1;
            }, text);
        }

        private void LogMessagesCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (LogMessagesCheckBox.Checked)
            {
                if (String.IsNullOrEmpty(_debugBufferFileName))
                {
                    using (var dlg = new OpenFileDialog { CheckFileExists = false })
                    {
                        if (dlg.ShowDialog(this).Equals(DialogResult.OK))
                        {
                            _debugBufferFileName = dlg.FileName;
                        }
                        else
                        {
                            LogMessagesCheckBox.Checked = false;
                            return;
                        }
                    }
                }

                if (File.Exists(_debugBufferFileName))
                {
                    string message = String.Format(CultureInfo.CurrentCulture,
                       "Do you want to delete the existing {0} file?", _debugBufferFileName);
                    if (MessageBox.Show(this, message, Text, MessageBoxButtons.YesNo).Equals(DialogResult.Yes))
                    {
                        try
                        {
                            File.Delete(_debugBufferFileName);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(String.Format(CultureInfo.CurrentCulture,
                               "Could not delete {0}.  Make sure the file is not already in use.", _debugBufferFileName));
                            LogMessagesCheckBox.Checked = false;
                            return;
                        }
                    }
                }

                //_fahClient.DebugBufferFileName = _debugBufferFileName;
                //_fahClient.DebugReceiveBuffer = true;
            }
            else
            {
                //_fahClient.DebugReceiveBuffer = false;
            }
        }

        private void ConnectButtonClick(object sender, EventArgs e)
        {
            try
            {
                if (_fahClient != null)
                {
                    _fahClient.ConnectedChanged -= FahClientConnectedChanged;
                    _fahClient.Dispose();
                }
                _fahClient = new FahClientConnection(HostAddressTextBox.Text, Int32.Parse(PortTextBox.Text));
                _fahClient.ConnectedChanged += FahClientConnectedChanged;
                _fahClient.Open();
                if (!String.IsNullOrWhiteSpace(PasswordTextBox.Text))
                {
                    // TODO: send password
                }

                Task.Run(() =>
                {
                    var reader = _fahClient.CreateReader();
                    try
                    {
                        while (reader.Read())
                        {
                            FahClientMessageReceived(reader.Message);
                        }
                    }
                    catch (Exception)
                    {
                        _fahClient.Close();
                    }
                });

                _totalBytesSent = 0;
                SetDataSentValueLabelText(_totalBytesSent);
                _totalBytesReceived = 0;
                SetDataReceivedValueLabelText(_totalBytesReceived);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (TimeoutException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CloseButtonClick(object sender, EventArgs e)
        {
            _fahClient.Close();
        }

        private void SendCommandButtonClick(object sender, EventArgs e)
        {
            if (_fahClient == null || !_fahClient.Connected)
            {
                MessageBox.Show("Not connected.");
                return;
            }

            string command = CommandTextBox.Text;
            if (command == "test-commands")
            {
                ExecuteCommand("info");
                ExecuteCommand("options -a");
                ExecuteCommand("queue-info");
                ExecuteCommand("slot-info");
                ExecuteCommand("log-updates restart");
            }
            else
            {
                ExecuteCommand(CommandTextBox.Text);
            }
        }

        private void FahClientConnectedChanged(object sender, FahClientConnectedChangedEventArgs e)
        {
            SetConnectionButtonsEnabled(e.Connected);
        }

        private void SetConnectionButtonsEnabled(bool connected)
        {
            this.BeginInvokeOnUIThread(c =>
            {
                ConnectButton.Enabled = !connected;
                CloseButton.Enabled = connected;
            }, connected);
        }

        private void ClearMessagesButtonClick(object sender, EventArgs e)
        {
            StatusLabel.Text = String.Empty;
            StatusMessageListBox.Items.Clear();
            MessageDisplayTextBox.Clear();
        }

        private void FahClientDataSent(int length)
        {
            unchecked
            {
                _totalBytesSent += length;
            }
            SetDataSentValueLabelText(_totalBytesSent);
        }

        private void SetDataSentValueLabelText(int value)
        {
            DataSentValueLabel.BeginInvokeOnUIThread(v => DataSentValueLabel.Text = $"{value / 1024.0:0.0} KB", value);
        }

        private void FahClientDataReceived(FahClientMessage message)
        {
            unchecked
            {
                _totalBytesReceived += message.MessageText.Length;
            }
            SetDataReceivedValueLabelText(_totalBytesReceived);
        }

        private void SetDataReceivedValueLabelText(int value)
        {
            DataReceivedValueLabel.BeginInvokeOnUIThread(v => DataReceivedValueLabel.Text = $"{value / 1024.0:0.0} KB", value);
        }

        private void ExecuteCommand(string commandText)
        {
            FahClientDataSent(_fahClient.CreateCommand(commandText).Execute());
        }

        #region TextBox KeyPress Event Handler (to enforce digits only)

        private void DigitsOnlyKeyPress(object sender, KeyPressEventArgs e)
        {
            //Debug.WriteLine(String.Format("Keystroke: {0}", (int)e.KeyChar));

            // only allow digits & special keystrokes
            if (char.IsDigit(e.KeyChar) == false &&
                  e.KeyChar != 8 &&       // backspace 
                  e.KeyChar != 26 &&      // Ctrl+Z
                  e.KeyChar != 24 &&      // Ctrl+X
                  e.KeyChar != 3 &&       // Ctrl+C
                  e.KeyChar != 22 &&      // Ctrl+V
                  e.KeyChar != 25)        // Ctrl+Y
            {
                e.Handled = true;
            }
        }

        #endregion

        private void StatusMessageListBox_Click(object sender, EventArgs e)
        {
            string value = StatusMessageListBox.SelectedItem as string;
            if (!String.IsNullOrWhiteSpace(value))
            {
                var lines = MessageDisplayTextBox.Lines;    

                int lineToGoto;
                for (lineToGoto = 0; lineToGoto < lines.Length; lineToGoto++)
                {
                    string line = lines[lineToGoto];
                    if (line.Contains(value))
                    {
                        break;
                    }
                }
                if (lineToGoto < lines.Length)
                {
                    int position = 0;
                    for (int i = 0; i < lineToGoto; i++)
                    {
                        position += lines[i].Length + Environment.NewLine.Length;
                    }
                    MessageDisplayTextBox.SelectionStart = position;
                    MessageDisplayTextBox.ScrollToCaret();
                }
            }
        }
    }
}
