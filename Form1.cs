using Sidekick.WinForms;

namespace blink
{
    public partial class Form1 : Form
    {
        private const int _countdownDuration = 60;
        private const int _sleepDuration = 60 * 20;
        private int _countdown;

        public Form1()
        {
            InitializeComponent();
            ApiHelper.InitializeApiClient();
            InitializeForm();
        }

        #region Logic
        private async Task InitializeForm()
        {
            notifyIcon1.Icon = SystemIcons.Application;
            ShowIcon = true;
            WindowState = FormWindowState.Minimized;
            Bounds = Screen.PrimaryScreen.Bounds;
            LocationExtension.SnapElementToMidScreen(this, lbl_timer);
            lbl_command.Size = new Size(Size.Width, lbl_command.Size.Height);
            LocationExtension.SnapElementHorizontalMid(this, lbl_command, Size.Height - 200);
            await Arise();
            WindowState = FormWindowState.Maximized;
        }

        private void ResetTimer()
        {
            _countdown = _countdownDuration;
            lbl_timer.Text = _countdown.ToString();
        }

        public void Vanish()
        {
            timer.Stop();
            Hide();
            Thread.Sleep(1000 * _sleepDuration);
            Arise();
        }

        public async Task Arise()
        {
            ResetTimer();

            await UpdateColors();
            UpdateCommand();

            timer.Start();
            Show();
        }

        private void UpdateCommand()
        {
            string command = CommandManager.GetRandomCommand();

            lbl_command.Text = command;
        }

        private async Task UpdateColors()
        {
            string[] colors = await ColorApi.GetRandomColors();
            BackColor = ColorTranslator.FromHtml($"#{colors[0]}");
            lbl_command.ForeColor = ColorTranslator.FromHtml($"#{colors[1]}");
            lbl_timer.ForeColor = ColorTranslator.FromHtml($"#{colors[1]}");
        }
        #endregion

        private void timer_Tick(object sender, EventArgs e)
        {
            _countdown--;
            lbl_timer.Text = _countdown.ToString();

            if (_countdown <= 0)
                Vanish();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}