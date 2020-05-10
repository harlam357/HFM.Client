﻿
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

using HFM.Client.ObjectModel;

namespace HFM.Client.Tool
{
    public partial class AsyncMainForm : Form
    {
        private FahClientConnection _fahClient;
        private readonly BlockingCollection<FahClientMessage> _messageQueue;

        public AsyncMainForm()
        {
            InitializeComponent();

            base.Text = $"HFM Client Tool v{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion}";

            _messageQueue = new BlockingCollection<FahClientMessage>();
            Task.Run(() =>
            {
                FahClientMessage message;
                while ((message = _messageQueue.Take()) != null)
                {
                    FahClientMessageReceived(message);
                }
            });
        }

        private async void ConnectButtonClick(object sender, EventArgs e)
        {
            try
            {
                _fahClient?.Dispose();
                _fahClient = CreateFahClientConnection();
                await _fahClient.OpenAsync();
                FahClientConnectedChanged(_fahClient.Connected);

                ResetFahClientDataSent();
                ResetFahClientDataReceived();

                string password = PasswordTextBox.Text;
                if (!String.IsNullOrWhiteSpace(password))
                {
                    await ExecuteFahClientCommandAsync("auth " + password);
                }

                var reader = _fahClient.CreateReader();
                try
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        _messageQueue.Add(reader.Message);
                    }
                }
                catch (Exception)
                {
                    // connection died
                }
                _fahClient.Close();
                FahClientConnectedChanged(_fahClient.Connected);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private FahClientConnection CreateFahClientConnection()
        {
            string host = HostAddressTextBox.Text;
            int port = Int32.Parse(PortTextBox.Text);

            return LogMessagesCheckBox.Checked
                ? new LoggingFahClientConnection(host, port)
                : new FahClientConnection(host, port);
        }

        private void FahClientConnectedChanged(bool connected)
        {
            SetConnectionButtonsEnabled(connected);
            SetStatusLabelText(connected ? "Connected" : "Connection Closed");
        }

        private async void FahClientMessageReceived(FahClientMessage message)
        {
            AppendTextToMessageDisplayTextBox(String.Empty);
            AppendTextToMessageDisplayTextBox(FahClientMessageHelper.FormatForDisplay(message));
            var identifier = message.Identifier.ToString();
            AddTextToStatusMessageListBox(identifier);
            SetStatusLabelText(identifier);
            FahClientDataReceived(message.MessageText.Length);

            if (message.Identifier.MessageType == FahClientMessageType.SlotInfo)
            {
                var slotCollection = SlotCollection.Load(message.MessageText);
                foreach (var slot in slotCollection)
                {
                    await ExecuteFahClientCommandAsync("slot-options " + slot.ID + " client-type client-subtype cpu-usage machine-id max-packet-size core-priority next-unit-percentage max-units checkpoint pause-on-start gpu-index gpu-usage");
                    await ExecuteFahClientCommandAsync("simulation-info " + slot.ID);
                }
            }
        }

        private void CloseButtonClick(object sender, EventArgs e)
        {
            _fahClient.Close();
        }

        private void ClearMessagesButtonClick(object sender, EventArgs e)
        {
            MessageDisplayTextBox.Clear();
            StatusMessageListBox.Items.Clear();
            StatusLabel.Text = String.Empty;
        }

        private async void SendCommandButtonClick(object sender, EventArgs e)
        {
            if (_fahClient == null || !_fahClient.Connected)
            {
                MessageBox.Show("Not connected.");
                return;
            }

            try
            {
                string command = CommandTextBox.Text;
                if (command == "test-commands")
                {
                    await ExecuteFahClientCommandAsync("info");
                    await ExecuteFahClientCommandAsync("options -a");
                    await ExecuteFahClientCommandAsync("queue-info");
                    await ExecuteFahClientCommandAsync("slot-info");
                    await ExecuteFahClientCommandAsync("log-updates restart");
                }
                else
                {
                    await ExecuteFahClientCommandAsync(CommandTextBox.Text);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DigitsOnlyKeyPress(object sender, KeyPressEventArgs e)
        {
            Debug.WriteLine($"Keystroke: {(int)e.KeyChar}");

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

        private void StatusMessageListBoxClick(object sender, EventArgs e)
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

        private async Task ExecuteFahClientCommandAsync(string commandText)
        {
            FahClientDataSent(await _fahClient.CreateCommand(commandText).ExecuteAsync());
        }

        private int _totalBytesSent;

        private void ResetFahClientDataSent()
        {
            _totalBytesSent = 0;
            SetDataSentValueLabelText(_totalBytesSent);
        }

        private void FahClientDataSent(int length)
        {
            unchecked
            {
                _totalBytesSent += length;
            }
            SetDataSentValueLabelText(_totalBytesSent);
        }

        private int _totalBytesReceived;

        private void ResetFahClientDataReceived()
        {
            _totalBytesReceived = 0;
            SetDataReceivedValueLabelText(_totalBytesReceived);
        }

        private void FahClientDataReceived(int length)
        {
            unchecked
            {
                _totalBytesReceived += length;
            }
            SetDataReceivedValueLabelText(_totalBytesReceived);
        }

        private void SetConnectionButtonsEnabled(bool connected)
        {
            this.BeginInvokeOnUIThread(c =>
            {
                ConnectButton.Enabled = !connected;
                CloseButton.Enabled = connected;
            }, connected);
        }

        private void AppendTextToMessageDisplayTextBox(string text)
        {
            MessageDisplayTextBox.BeginInvokeOnUIThread(t => MessageDisplayTextBox.AppendText(t), text);
        }

        private void AddTextToStatusMessageListBox(string text)
        {
            StatusMessageListBox.BeginInvokeOnUIThread(t =>
            {
                StatusMessageListBox.Items.Add(text);
                StatusMessageListBox.SelectedIndex = StatusMessageListBox.Items.Count - 1;
            }, text);
        }

        private void SetStatusLabelText(string text)
        {
            this.BeginInvokeOnUIThread(t => StatusLabel.Text = t, text);
        }

        private void SetDataSentValueLabelText(int value)
        {
            DataSentValueLabel.BeginInvokeOnUIThread(v => DataSentValueLabel.Text = $"{value / 1024.0:0.0} KB", value);
        }

        private void SetDataReceivedValueLabelText(int value)
        {
            DataReceivedValueLabel.BeginInvokeOnUIThread(v => DataReceivedValueLabel.Text = $"{value / 1024.0:0.0} KB", value);
        }
    }
}
