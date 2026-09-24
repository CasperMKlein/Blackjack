namespace Blackjack
{
    public partial class Form1 : Form
    {
        BlackjackLogic game;

        public Form1()
        {
            InitializeComponent();

            game = new BlackjackLogic(DisplayMessage);
            Load += async (sender, e) =>
            {
                await game.MainMenu();
                Application.Exit();
            };
        }

        public void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            string text = textBox1.Text;
            DisplayMessage("\n> " + text);
            textBox1.Clear();
            e.Handled = true;
            e.SuppressKeyPress = true;

            game.SubmitInput(text);
        }

        private void DisplayMessage(string s)
        {
            richTextBox1.Text += s;
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }


    }
}
