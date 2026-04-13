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

        // === Логин-панель ===
        private Panel loginPanel;
        private Label lblLoginTitle;
        private Label lblLogin;
        private TextBox txtLogin;
        private Label lblPassword;
        private TextBox txtPassword;
        private Label lblNewLogin;
        private TextBox txtNewLogin;
        private Label lblNewPassword;
        private TextBox txtNewPassword;
        private Label lblNewPasswordConfirm;
        private TextBox txtNewPasswordConfirm;
        private Button btnLogin;
        private Button btnRegister;
        private Label lblLoginMessage;

        // === Игровая панель ===
        private Panel gamePanel;
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
        private Button btnLogout;

        public Form1()
        {
            SetupForm();
            ShowLogin();
        }

        private void SetupForm()
        {
            this.Text = "Угадай слово";
            this.ClientSize = new Size(500, 480);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            // === ПАНЕЛЬ ЛОГИНА ===
            loginPanel = new Panel { Dock = DockStyle.Fill };

            lblLoginTitle = new Label
            {
                Text = "🔑 Вход / Регистрация",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(120, 10),
                Size = new Size(260, 30)
            };

            // --- Вход ---
            var lblSectionLogin = new Label
            {
                Text = "── Вход ──",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(170, 50),
                Size = new Size(160, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblLogin = new Label { Text = "Логин:", Location = new Point(120, 80), Size = new Size(80, 20) };
            txtLogin = new TextBox { Location = new Point(210, 78), Size = new Size(170, 22) };

            lblPassword = new Label { Text = "Пароль:", Location = new Point(120, 110), Size = new Size(80, 20) };
            txtPassword = new TextBox { Location = new Point(210, 108), Size = new Size(170, 22), PasswordChar = '*' };

            btnLogin = new Button { Text = "Войти", Location = new Point(210, 140), Size = new Size(170, 30), BackColor = Color.LightGreen };
            btnLogin.Click += BtnLogin_Click;

            // --- Регистрация ---
            var lblSectionRegister = new Label
            {
                Text = "── Регистрация ──",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(150, 190),
                Size = new Size(200, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblNewLogin = new Label { Text = "Логин:", Location = new Point(120, 220), Size = new Size(80, 20) };
            txtNewLogin = new TextBox { Location = new Point(210, 218), Size = new Size(170, 22) };

            lblNewPassword = new Label { Text = "Пароль:", Location = new Point(120, 250), Size = new Size(80, 20) };
            txtNewPassword = new TextBox { Location = new Point(210, 248), Size = new Size(170, 22), PasswordChar = '*' };

            lblNewPasswordConfirm = new Label { Text = "Повтор:", Location = new Point(120, 280), Size = new Size(80, 20) };
            txtNewPasswordConfirm = new TextBox { Location = new Point(210, 278), Size = new Size(170, 22), PasswordChar = '*' };

            btnRegister = new Button { Text = "Зарегистрироваться", Location = new Point(210, 310), Size = new Size(170, 30), BackColor = Color.LightSkyBlue };
            btnRegister.Click += BtnRegister_Click;

            lblLoginMessage = new Label
            {
                Text = "",
                Font = new Font("Arial", 9),
                Location = new Point(50, 360),
                Size = new Size(400, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Red
            };

            loginPanel.Controls.AddRange(new Control[] {
                lblLoginTitle, lblSectionLogin,
                lblLogin, txtLogin, lblPassword, txtPassword, btnLogin,
                lblSectionRegister,
                lblNewLogin, txtNewLogin, lblNewPassword, txtNewPassword,
                lblNewPasswordConfirm, txtNewPasswordConfirm, btnRegister,
                lblLoginMessage
            });

            // === ИГРОВАЯ ПАНЕЛЬ ===
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

            btnLogout = new Button { Text = "Выход", Location = new Point(300, 290), Size = new Size(80, 28), BackColor = Color.LightPink };
            btnLogout.Click += BtnLogout_Click;

            gamePanel.Controls.AddRange(new Control[] {
                lblTitle, lblWord, lblAttempts, lblTried, lblStatus,
                txtLetter, btnGuessLetter, txtWordGuess, btnGuessWord,
                btnHint, btnNewGame, btnLogout
            });

            this.Controls.Add(loginPanel);
            this.Controls.Add(gamePanel);
        }

        // =================== ЛОГИН / РЕГИСТРАЦИЯ ===================

        private void ShowLogin()
        {
            loginPanel.Visible = true;
            gamePanel.Visible = false;
            ClearLoginFields();
            txtLogin.Focus();
        }

        private void ShowGame()
        {
            loginPanel.Visible = false;
            gamePanel.Visible = true;
            this.Text = $"Угадай слово — {player.Login}";
        }

        private void ClearLoginFields()
        {
            txtLogin.Text = "";
            txtPassword.Text = "";
            txtNewLogin.Text = "";
            txtNewPassword.Text = "";
            txtNewPasswordConfirm.Text = "";
            lblLoginMessage.Text = "";
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                lblLoginMessage.Text = "Введите логин и пароль!";
                lblLoginMessage.ForeColor = Color.Red;
                return;
            }

            if (!PlayerDatabase.Validate(login, password))
            {
                lblLoginMessage.Text = "Неверный логин или пароль!";
                lblLoginMessage.ForeColor = Color.Red;
                return;
            }

            player = new Player(login, password);
            logic = new Logic();
            ShowGame();
            StartNewGame();
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string login = txtNewLogin.Text.Trim();
            string password = txtNewPassword.Text.Trim();
            string confirm = txtNewPasswordConfirm.Text.Trim();

            if (string.IsNullOrEmpty(login))
            {
                lblLoginMessage.Text = "Введите логин (минимум 2 символа)!";
                lblLoginMessage.ForeColor = Color.Red;
                return;
            }

            if (login.Length < 2)
            {
                lblLoginMessage.Text = "Логин слишком короткий (мин. 2 символа)!";
                lblLoginMessage.ForeColor = Color.Red;
                return;
            }

            if (password.Length < 3)
            {
                lblLoginMessage.Text = "Пароль слишком короткий (мин. 3 символа)!";
                lblLoginMessage.ForeColor = Color.Red;
                return;
            }

            if (password != confirm)
            {
                lblLoginMessage.Text = "Пароли не совпадают!";
                lblLoginMessage.ForeColor = Color.Red;
                return;
            }

            if (PlayerDatabase.Exists(login))
            {
                lblLoginMessage.Text = "Такой логин уже занят!";
                lblLoginMessage.ForeColor = Color.Red;
                return;
            }

            if (PlayerDatabase.Register(login, password))
            {
                lblLoginMessage.Text = "Регистрация успешна! Теперь войдите.";
                lblLoginMessage.ForeColor = Color.Green;
                txtLogin.Text = login;
                txtPassword.Focus();
            }
            else
            {
                lblLoginMessage.Text = "Ошибка регистрации!";
                lblLoginMessage.ForeColor = Color.Red;
            }
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            player = null;
            logic = null;
            ShowLogin();
        }

        // =================== ИГРА ===================

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
                return;

            char letter = txtLetter.Text[0];
            char lowerLetter = char.ToLower(letter);

            if (logic.CurrentGame.TriedLetters.Contains(lowerLetter))
            {
                lblStatus.Text = $"Буква '{lowerLetter}' уже была!";
                lblStatus.ForeColor = Color.Orange;
                txtLetter.Text = "";
                txtLetter.Focus();
                return;
            }

            bool found = logic.GuessLetter(letter);

            if (found)
            {
                lblStatus.Text = $"Буква '{lowerLetter}' есть!";
                lblStatus.ForeColor = Color.Green;
            }
            else
            {
                lblStatus.Text = $"Буквы '{lowerLetter}' нет!";
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
                return;

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
