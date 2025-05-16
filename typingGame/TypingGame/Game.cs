namespace TypingGame
{
    public partial class Game : Form
    {
        private int charIndex = 0;
        private string currentGame = "";
        private List<string> words = [];

        private GameType gameType = new LoremGame();

        public Game()
        {
            InitializeComponent();
            FillList();
            LineFinished();
        }

        private void FillList()
        {
            string line;
            while((line = gameType.Reader.ReadLine()) != null)
                words.Add(line);
        }

        public void LineFinished()
        {
            inputTextBox.Clear();
            charIndex = 0;
            string newText = gameType.NextLine().Trim();
            toCopyTextBox.Text = newText;
            currentGame = newText;
        }

        private void InputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char pressed = e.KeyChar;
            if (pressed == (char)8)
            {
                string input = inputTextBox.Text;
                inputTextBox.Text = input.Remove(input.Length - 1);
                charIndex--;
            }
            if (pressed == currentGame[charIndex])
            {
                if (charIndex == currentGame.Length - 1)
                {
                    LineFinished();
                    return;
                }
                inputTextBox.Text += pressed;
                charIndex++;
            }
        }

        private void GameSelectorComboBox_ValueChanged(object sender, EventArgs e)
        {
            SelectedGame selection;
            Enum.TryParse<SelectedGame>(gameSelectorComboBox.SelectedValue.ToString(), out selection);
            switch (selection)
            {
                case SelectedGame.Lorem:
                    gameType = new LoremGame();
                    LineFinished();
                    break;
                case SelectedGame.Common:
                    gameType = new CommonGame();
                    LineFinished();
                    break;
                default:
                    break;
            }
        }

        enum SelectedGame
        {
            Lorem,
            Common,
        }
    }
}
