using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace guessTheWord
{
    public partial class Form1 : Form
    {
        private Player player;
        private Logic logic;

        private Label lblTitle;
        private Label lblWord;
        private Label lblAttempts;
        private Label lblTried;
        private Label lblStatus;
        private TextBox txtLetter;
        private TextBox txtWordGuess;
        private Button btnGuessLetter;
        private Button btnGuessWord;
        private Button btnHint;
        private Button btnNewGame;
        private Button btnLogin;
        private TextBox txtLogin;
        private TextBox txtPassword;
        private Label lblLogin;
        private Label lblPassword;
        private Panel loginPanel;
        private Panel gamePanel;

        public Form1()
        {
            SetupForm();
            ShowLogin();
        }

        private void SetupForm()
        {
            this.Text = "Угадай слово";
            this.ClientSize = new Size(500, 400);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            // === Панель логина ===
            loginPanel = new Panel { Dock = DockStyle.Fill };

            lblLogin = new Label { Text = "Логин:", Location = new Point(150, 80), Size = new Size(80, 20) };
            txtLogin = new TextBox { Location = new Point(230, 78), Size = new Size(150, 22) };

            lblPassword = new Label { Text = "Пароль:", Location = new Point(150, 120), Size = new Size(80, 20) };
            txtPassword = new TextBox { Location = new Point(230, 118), Size = new Size(150, 22), PasswordChar = '*' };

            btnLogin = new Button { Text = "Войти", Location = new Point(230, 160), Size = new Size(100, 30) };
            btnLogin.Click += BtnLogin_Click;

            loginPanel.Controls.AddRange(new Control[] { lblLogin, txtLogin, lblPassword, txtPassword, btnLogin });

            // === Игровая панель ===
            gamePanel = new Panel { Dock = DockStyle.Fill };

            lblTitle = new Label
            {
                Text = "Угадай слово!",
                Font = new Font("Arial", 16, FontStyle.Bold),
                Location = new Point(130, 10),
                Size = new Size(250, 30)
            };

            lblWord = new Label
            {
                Text = "",
                Font = new Font("Consolas", 24, FontStyle.Bold),
                Location = new Point(50, 60),
                Size = new Size(400, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblAttempts = new Label
            {
                Text = "Попыток: 6",
                Font = new Font("Arial", 12),
                Location = new Point(20, 110),
                Size = new Size(200, 25),
                ForeColor = Color.Green
            };

            lblTried = new Label
            {
                Text = "Буквы: ",
                Font = new Font("Arial", 10),
                Location = new Point(20, 140),
                Size = new Size(460, 25)
            };

            lblStatus = new Label
            {
                Text = "",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(50, 200),
                Size = new Size(400, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            txtLetter = new TextBox
            {
                Location = new Point(20, 250),
                Size = new Size(40, 22),
                MaxLength = 1,
                CharacterCasing = CharacterCasing.Lower
            };
            txtLetter.KeyPress += TxtLetter_KeyPress;

            btnGuessLetter = new Button { Text = "Буква", Location = new Point(70, 248), Size = new Size(80, 28) };
            btnGuessLetter.Click += BtnGuessLetter_Click;

            txtWordGuess = new TextBox
            {
                Location = new Point(170, 250),
                Size = new Size(150, 22),
                CharacterCasing = CharacterCasing.Lower
            };

            btnGuessWord = new Button { Text = "Слово", Location = new Point(330, 248), Size = new Size(80, 28) };
            btnGuessWord.Click += BtnGuessWord_Click;

            btnHint = new Button { Text = "Подсказка (-1)", Location = new Point(20, 290), Size = new Size(120, 28) };
            btnHint.Click += BtnHint_Click;

            btnNewGame = new Button { Text = "Новая игра", Location = new Point(160, 290), Size = new Size(120, 28) };
            btnNewGame.Click += BtnNewGame_Click;

            gamePanel.Controls.AddRange(new Control[] {
                lblTitle, lblWord, lblAttempts, lblTried, lblStatus,
                txtLetter, btnGuessLetter, txtWordGuess, btnGuessWord,
                btnHint, btnNewGame
            });

            this.Controls.Add(loginPanel);
            this.Controls.Add(gamePanel);
        }

        private void ShowLogin()
        {
            loginPanel.Visible = true;
            gamePanel.Visible = false;
            txtLogin.Focus();
        }

        private void ShowGame()
        {
            loginPanel.Visible = false;
            gamePanel.Visible = true;
            this.Text = $"Угадай слово — {player.Login}";
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Введите логин!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            player = new Player(login, password);
            logic = new Logic();

            ShowGame();
            StartNewGame();
        }

        private void StartNewGame()
        {
            Game game = logic.StartNewGame();
            player.AddGame(game);
            UpdateDisplay();
            lblStatus.Text = "Угадайте букву или слово!";
            lblStatus.ForeColor = Color.Black;
            txtLetter.Enabled = true;
            btnGuessLetter.Enabled = true;
            txtWordGuess.Enabled = true;
            btnGuessWord.Enabled = true;
            btnHint.Enabled = true;
            txtLetter.Focus();
        }

        private void UpdateDisplay()
        {
            Game g = logic.CurrentGame;
            if (g == null) return;

            // Показываем слово с пробелами между буквами
            string display = "";
            foreach (char c in g.Revealed)
                display += c + " ";
            lblWord.Text = display.Trim();

            lblAttempts.Text = $"Попыток: {g.AttemptsLeft}";
            lblAttempts.ForeColor = g.AttemptsLeft <= 2 ? Color.Red :
                                    g.AttemptsLeft <= 4 ? Color.Orange : Color.Green;

            lblTried.Text = $"Буквы: {string.Join(", ", g.TriedLetters)}";
        }

        private void BtnGuessLetter_Click(object sender, EventArgs e)
        {
            TryGuessLetter();
        }

        private void TxtLetter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                TryGuessLetter();
                e.Handled = true;
            }
        }

        private void TryGuessLetter()
        {
            if (logic.CurrentGame == null || logic.CurrentGame.IsOver) return;

            if (string.IsNullOrEmpty(txtLetter.Text))
            {
                MessageBox.Show("Введите букву!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            char letter = txtLetter.Text[0];
            bool found = logic.GuessLetter(letter);

            if (logic.CurrentGame.TriedLetters.Count > 0 &&
                logic.CurrentGame.TriedLetters[logic.CurrentGame.TriedLetters.Count - 1] != letter)
            {
                // Буква уже была — не считаем ошибкой
                lblStatus.Text = $"Буква '{letter}' уже была!";
                lblStatus.ForeColor = Color.Orange;
            }
            else if (found)
            {
                lblStatus.Text = $"Буква '{letter}' есть!";
                lblStatus.ForeColor = Color.Green;
            }
            else
            {
                lblStatus.Text = $"Буквы '{letter}' нет!";
                lblStatus.ForeColor = Color.Red;
            }

            UpdateDisplay();
            CheckGameEnd();
            txtLetter.Text = "";
            txtLetter.Focus();
        }

        private void BtnGuessWord_Click(object sender, EventArgs e)
        {
            if (logic.CurrentGame == null || logic.CurrentGame.IsOver) return;

            if (string.IsNullOrEmpty(txtWordGuess.Text.Trim()))
            {
                MessageBox.Show("Введите слово!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool won = logic.GuessWord(txtWordGuess.Text.Trim());

            if (won)
            {
                lblStatus.Text = "Верно! Вы угадали слово!";
                lblStatus.ForeColor = Color.Green;
            }
            else
            {
                lblStatus.Text = "Неверно! Минус попытка.";
                lblStatus.ForeColor = Color.Red;
            }

            UpdateDisplay();
            CheckGameEnd();
            txtWordGuess.Text = "";
            txtLetter.Focus();
        }

        private void BtnHint_Click(object sender, EventArgs e)
        {
            if (logic.CurrentGame == null || logic.CurrentGame.IsOver) return;

            char hint = logic.Hint();
            if (hint != '\0')
            {
                lblStatus.Text = $"Подсказка: буква '{hint}' (минус 1 попытка)";
                lblStatus.ForeColor = Color.Blue;
            }

            UpdateDisplay();
            CheckGameEnd();
        }

        private void BtnNewGame_Click(object sender, EventArgs e)
        {
            StartNewGame();
        }

        private void CheckGameEnd()
        {
            if (logic.CurrentGame == null) return;

            if (logic.CurrentGame.IsWon)
            {
                lblStatus.Text = $"Победа! Слово: {logic.CurrentGame.Word}";
                lblStatus.ForeColor = Color.Green;
                DisableInput();
            }
            else if (logic.CurrentGame.IsOver)
            {
                lblStatus.Text = $"Проигрыш! Слово было: {logic.CurrentGame.Word}";
                lblStatus.ForeColor = Color.Red;
                DisableInput();
            }
        }

        private void DisableInput()
        {
            txtLetter.Enabled = false;
            btnGuessLetter.Enabled = false;
            txtWordGuess.Enabled = false;
            btnGuessWord.Enabled = false;
            btnHint.Enabled = false;
        }
    }
}
