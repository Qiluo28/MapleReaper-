using MapleReaper.Libraries;
using MapleReaper.Models;
using MapleReaper.Properties;
using MapleReaper.Scripts;
using MapleReaper.Services;

namespace MapleReaper
{
    internal delegate void ScriptInvoke();

    public partial class MapleReaperForm : Form
    {
        private readonly HashSet<TextBox[]> keyBindingSet;
        private Script script;

        public MapleReaperForm()
        {
            InitializeComponent();
            ProcessManager.UpdateState();
            KeyboardHook.KeyboardHookInit();
            KeyboardHook.AddCallBack(new KeyboardHook.KeyboardHookCallback(KeyPressEvent));
            keyBindingSet = new HashSet<TextBox[]>()
            {
                new TextBox[] { tbBuff1Key, tbBuff1PreDelay, tbBuff1Delay },
                new TextBox[] { tbBuff2Key, tbBuff2PreDelay, tbBuff2Delay },
                new TextBox[] { tbBuff3Key, tbBuff3PreDelay, tbBuff3Delay },
                new TextBox[] { tbBuff4Key, tbBuff4PreDelay, tbBuff4Delay },
                new TextBox[] { tbBuff5Key, tbBuff5PreDelay, tbBuff5Delay },
                new TextBox[] { tbBuff6Key, tbBuff6PreDelay, tbBuff6Delay },
                new TextBox[] { tbBuff7Key, tbBuff7PreDelay, tbBuff7Delay },
                new TextBox[] { tbBuff8Key, tbBuff8PreDelay, tbBuff8Delay },
                new TextBox[] { tbBuff9Key, tbBuff9PreDelay, tbBuff9Delay },
            };
            LoadSettings();
            PluginService.Start(this);
            RabbitMQService.Start(StopScript);
        }

        public void SetLabel(string text)
        {
            if (label1.InvokeRequired) Invoke(() => label1.Text = text);
            else label1.Text = text;
        }

        private async Task KeyPressEvent(Keys key)
        {
            if (key == Keys.F9)
            {
                if (script is null) return;
                ReaperSetting.IsPausing ^= true;
                return;
            }
            if (key != Keys.F8) return;
            SaveSettings();
            if (script is null)
            {
                script = new Script();
                return;
            }
            StopScript();
        }

        private void StopScript()
        {
            ReaperSetting.IsPausing = false;
            ReaperSetting.IsScripting = false;
            script = null;
        }

        private void SaveSettings()
        {
            // settings
            Settings.Default.SelectedVersion = cbVersion.SelectedIndex;
            Settings.Default.SelectedScript = cbScript.SelectedIndex;
            Settings.Default.IsSellingEnabled = cbSelling.Checked;
            Settings.Default.SellingDelay = tbSellingDelay.Text;
            Settings.Default.SellingCommand1 = tbSellingCommand1.Text;
            Settings.Default.SellingCommand2 = tbSellingCommand2.Text;
            Settings.Default.IsRebirthEnabled = cbRebirth.Checked;
            Settings.Default.RebirthDelay = tbRebirthDelay.Text;
            Settings.Default.RebirthCommand = tbRebirthCommand.Text;
            Settings.Default.IsMesoTransformationEnabled = cbMesoTransformation.Checked;
            Settings.Default.MesoTransformationDelay = tbMesoTransformationDelay.Text;
            Settings.Default.IsBanDetectionEnabled = cbBanDetection.Checked;
            Settings.Default.IsFreezerEnabled = cbFreezer.Checked;
            Settings.Default.Id = tbId.Text;
            Settings.Default.Account = tbAccount.Text;
            Settings.Default.Password = tbPassword.Text;
            // key bindings
            Settings.Default.AttackKey = tbAttackKey.Text;
            Settings.Default.AttackDelay = tbAttackDelay.Text;
            var keyBindingList = keyBindingSet.ToList();
            keyBindingList.ToList().ForEach(keyBinding =>
            {
                if (string.IsNullOrEmpty(keyBinding[0].Text))
                {
                    keyBindingList.Remove(keyBinding);
                }
                else
                {
                    keyBinding[1].Text = string.IsNullOrEmpty(keyBinding[1].Text) ? "0" : keyBinding[1].Text;
                    keyBinding[2].Text = string.IsNullOrEmpty(keyBinding[1].Text) ? "0" : keyBinding[2].Text;
                }
            });
            Settings.Default.BuffKeys = string.Join(',', keyBindingList.Select(m => m[0].Text));
            Settings.Default.BuffPreDelays = string.Join(',', keyBindingList.Select(m => m[1].Text));
            Settings.Default.BuffDelays = string.Join(',', keyBindingList.Select(m => m[2].Text));
            Settings.Default.Save();
        }

        private void LoadSettings()
        {
            // settings
            cbVersion.SelectedIndex = Settings.Default.SelectedVersion;
            cbScript.SelectedIndex = Settings.Default.SelectedScript;
            cbSelling.Checked = Settings.Default.IsSellingEnabled;
            tbSellingDelay.Text = Settings.Default.SellingDelay;
            tbSellingCommand1.Text = Settings.Default.SellingCommand1;
            tbSellingCommand2.Text = Settings.Default.SellingCommand2;
            cbRebirth.Checked = Settings.Default.IsRebirthEnabled;
            tbRebirthDelay.Text = Settings.Default.RebirthDelay;
            tbRebirthCommand.Text = Settings.Default.RebirthCommand;
            cbMesoTransformation.Checked = Settings.Default.IsMesoTransformationEnabled;
            tbMesoTransformationDelay.Text = Settings.Default.MesoTransformationDelay;
            cbBanDetection.Checked = Settings.Default.IsBanDetectionEnabled;
            cbFreezer.Checked = Settings.Default.IsFreezerEnabled;
            tbId.Text = Settings.Default.Id;
            tbAccount.Text = Settings.Default.Account;
            tbPassword.Text = Settings.Default.Password;
            // key bindings
            tbAttackKey.Text = Settings.Default.AttackKey;
            tbAttackDelay.Text = Settings.Default.AttackDelay;
            var buffKeys = Settings.Default.BuffKeys.Split(',');
            var buffPreDelays = Settings.Default.BuffPreDelays.Split(',');
            var buffDelays = Settings.Default.BuffDelays.Split(',');
            var keyBindingList = keyBindingSet.ToList();
            for (var i = 0; i < buffKeys.Length; ++i)
            {
                keyBindingList[i][0].Text = buffKeys[i];
                keyBindingList[i][1].Text = buffPreDelays[i];
                keyBindingList[i][2].Text = buffDelays[i];
            }
        }

        private void MapleReaperFormShown(object sender, EventArgs e)
        {
            Location = new Point(830, 50);
        }

        private void TextboxKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = (TextBox)sender;
            var keyCode = e.KeyCode.ToString();
            if (keyCode == "Delete")
            {
                textBox.Text = "DDelete";
                return;
            }
            if (keyCode == "Escape")
            {
                textBox.Text = string.Empty;
                return;
            }
            textBox.Text = keyCode;
        }

        private void TextboxKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void TextboxDelayKeyPress(object sender, KeyPressEventArgs e)
        {
            var asciiCode = (int)e.KeyChar;
            e.Handled = (asciiCode >= 48 && asciiCode <= 57 || asciiCode == 8) == false;
        }

        private void BtTestClipboard(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText("test");
                MessageBox.Show("Ok", "Ok");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Err", ex.Message);
            }
        }

        private void BtPrintClick(object sender, EventArgs e)
        {
            using var bitmap = OpenCV.CaptureGameToBitmap(State.Size);
            bitmap.Save("maplestory.png");
        }

        private void BtSendAlertClick(object sender, EventArgs e)
        {
            RabbitMQService.SendAlert();
        }

        private async void BtPreprocessClick(object sender, EventArgs e)
        {
            await Keyboard.Type("", "@gashaponmega", "@tsmega");
        }

        private void BtTestClick(object sender, EventArgs e)
        {
            MessageBox.Show(MemoryReader.GetNumberOfUI().ToString(), "GetNumberOfUI");
        }

        private async void BtReapClick(object sender, EventArgs e)
        {
            Point location;
            while (OpenCV.TemplateMatch(Resources.Inventory, out location) == false) await Keyboard.KeyPress(Keys.I, 200);
            await Mouse.Move(location.X + 110, location.Y + 30, 200);
            await Mouse.LeftClick(200);
            for (var i = 0; i < 24; i++)
            {
                await Mouse.Move(i % 4 * 37 + location.X + 13, i / 4 * 34 + location.Y + 60, 800);
                await Mouse.LeftClick(200);
                await Mouse.Move(State.X + State.Size.Width - 100, State.Y + 100, 200);
                await Mouse.LeftClick(200);
                await Keyboard.KeyPress(Keys.Enter, 200);
            }
        }

        private void BtSaveClick(object sender, EventArgs e)
        {
            SaveSettings();
        }
    }
}