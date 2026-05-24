using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace guessTheWord
{
    public partial class MainWindow : Window
    {
        private Player player;
        private Logic logic;

        public MainWindow()
        {
            InitializeComponent();
            ShowLogin();
        }

        // =================== LOGIN / REGISTER ===================

        private void ShowLogin()
        {
            LoginPanel.Visibility = Visibility.Visible;
            GamePanel.Visibility = Visibility.Collapsed;
            HistoryPanel.Visibility = Visibility.Collapsed;
            loginSection.Visibility = Visibility.Visible;
            registerSection.Visibility = Visibility.Collapsed;
            ClearLoginFields();
            txtLogin.Focus();
        }

        private void ShowGame()
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            GamePanel.Visibility = Visibility.Visible;
            HistoryPanel.Visibility = Visibility.Collapsed;
            Title = $"Угадай слово — {player.Login}";
        }

        private void ShowHistory()
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            GamePanel.Visibility = Visibility.Collapsed;
            HistoryPanel.Visibility = Visibility.Visible;

            int played = player.GamesPlayed;
            int won = player.GamesWon;
            double rate = played > 0 ? (double)won / played * 100 : 0;
            lblStats.Text = $"Сыграно: {played}  |  Побед: {won}  |  Процент: {rate:F1}%";

            gridHistory.ItemsSource = null;
            gridHistory.ItemsSource = player.Games.OrderByDescending(g => g.Date).ToList();
        }

        private void ClearLoginFields()
        {
            txtLogin.Text = "";
            txtPassword.Password = "";
            txtNewLogin.Text = "";
            txtNewPassword.Password = "";
            txtNewPasswordConfirm.Password = "";
            lblLoginMessage.Text = "";
        }

        private void SwitchToRegister_Click(object sender, RoutedEventArgs e)
        {
            loginSection.Visibility = Visibility.Collapsed;
            registerSection.Visibility = Visibility.Visible;
            ClearLoginFields();
            txtNewLogin.Focus();
        }

        private void SwitchToLogin_Click(object sender, RoutedEventArgs e)
        {
            loginSection.Visibility = Visibility.Visible;
            registerSection.Visibility = Visibility.Collapsed;
            ClearLoginFields();
            txtLogin.Focus();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                lblLoginMessage.Text = "Введите логин и пароль!";
                lblLoginMessage.Foreground = Brushes.Red;
                return;
            }

            if (!DataStore.Validate(login, password))
            {
                lblLoginMessage.Text = "Неверный логин или пароль!";
                lblLoginMessage.Foreground = Brushes.Red;
                return;
            }

            player = DataStore.GetPlayer(login);
            logic = new Logic();
            ShowGame();
            StartNewGame();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string login = txtNewLogin.Text.Trim();
            string password = txtNewPassword.Password;
            string confirm = txtNewPasswordConfirm.Password;

            if (string.IsNullOrEmpty(login))
            {
                lblLoginMessage.Text = "Введите логин (минимум 2 символа)!";
                lblLoginMessage.Foreground = Brushes.Red;
                return;
            }

            if (login.Length < 2)
            {
                lblLoginMessage.Text = "Логин слишком короткий (мин. 2 символа)!";
                lblLoginMessage.Foreground = Brushes.Red;
                return;
            }

            if (password.Length < 3)
            {
                lblLoginMessage.Text = "Пароль слишком короткий (мин. 3 символа)!";
                lblLoginMessage.Foreground = Brushes.Red;
                return;
            }

            if (password != confirm)
            {
                lblLoginMessage.Text = "Пароли не совпадают!";
                lblLoginMessage.Foreground = Brushes.Red;
                return;
            }

            if (DataStore.Exists(login))
            {
                lblLoginMessage.Text = "Такой логин уже занят!";
                lblLoginMessage.Foreground = Brushes.Red;
                return;
            }

            if (DataStore.Register(login, password))
            {
                lblLoginMessage.Text = "Регистрация успешна! Теперь войдите.";
                lblLoginMessage.Foreground = Brushes.Green;
                txtLogin.Text = login;
                txtPassword.Focus();
            }
            else
            {
                lblLoginMessage.Text = "Ошибка регистрации!";
                lblLoginMessage.Foreground = Brushes.Red;
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            DataStore.SavePlayer(player);
            player = null;
            logic = null;
            ShowLogin();
        }

        // =================== GAME ===================

        private void StartNewGame()
        {
            Game game = logic.StartNewGame();
            player.AddGame(game);
            UpdateDisplay();
            lblStatus.Text = "Угадайте букву или слово!";
            lblStatus.Foreground = Brushes.Black;
            txtLetter.IsEnabled = true;
            btnGuessLetter.IsEnabled = true;
            txtWordGuess.IsEnabled = true;
            btnGuessWord.IsEnabled = true;
            btnHint.IsEnabled = true;
            txtLetter.Focus();
        }

        private void UpdateDisplay()
        {
            Game g = logic.CurrentGame;
            if (g == null) return;

            lblWord.Text = string.Join(" ", g.Revealed);

            lblAttempts.Text = g.AttemptsLeft.ToString();
            lblAttempts.Foreground = g.AttemptsLeft <= 2 ? Brushes.Red :
                                      g.AttemptsLeft <= 4 ? Brushes.Orange : Brushes.Green;

            lblTried.Text = string.Join(", ", g.TriedLetters);
        }

        private void BtnGuessLetter_Click(object sender, RoutedEventArgs e)
        {
            TryGuessLetter();
        }

        private void TxtLetter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
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
                lblStatus.Foreground = Brushes.Orange;
                txtLetter.Text = "";
                txtLetter.Focus();
                return;
            }

            bool found = logic.GuessLetter(letter);

            if (found)
            {
                lblStatus.Text = $"Буква '{lowerLetter}' есть!";
                lblStatus.Foreground = Brushes.Green;
            }
            else
            {
                lblStatus.Text = $"Буквы '{lowerLetter}' нет!";
                lblStatus.Foreground = Brushes.Red;
            }

            UpdateDisplay();
            CheckGameEnd();
            txtLetter.Text = "";
            txtLetter.Focus();
        }

        private void BtnGuessWord_Click(object sender, RoutedEventArgs e)
        {
            if (logic.CurrentGame == null || logic.CurrentGame.IsOver) return;

            if (string.IsNullOrEmpty(txtWordGuess.Text.Trim()))
                return;

            bool won = logic.GuessWord(txtWordGuess.Text.Trim());

            if (won)
            {
                lblStatus.Text = "Верно! Вы угадали слово!";
                lblStatus.Foreground = Brushes.Green;
            }
            else
            {
                lblStatus.Text = "Неверно! Минус попытка.";
                lblStatus.Foreground = Brushes.Red;
            }

            UpdateDisplay();
            CheckGameEnd();
            txtWordGuess.Text = "";
            txtLetter.Focus();
        }

        private void BtnHint_Click(object sender, RoutedEventArgs e)
        {
            if (logic.CurrentGame == null || logic.CurrentGame.IsOver) return;

            char hint = logic.Hint();
            if (hint != '\0')
            {
                lblStatus.Text = $"Подсказка: буква '{hint}' (минус 1 попытка)";
                lblStatus.Foreground = Brushes.Blue;
            }

            UpdateDisplay();
            CheckGameEnd();
        }

        private void BtnNewGame_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }

        private void CheckGameEnd()
        {
            if (logic.CurrentGame == null) return;

            if (logic.CurrentGame.IsWon)
            {
                lblStatus.Text = $"Победа! Слово: {logic.CurrentGame.Word}";
                lblStatus.Foreground = Brushes.Green;
                DisableInput();
            }
            else if (logic.CurrentGame.IsOver)
            {
                lblStatus.Text = $"Проигрыш! Слово было: {logic.CurrentGame.Word}";
                lblStatus.Foreground = Brushes.Red;
                DisableInput();
            }
        }

        private void DisableInput()
        {
            txtLetter.IsEnabled = false;
            btnGuessLetter.IsEnabled = false;
            txtWordGuess.IsEnabled = false;
            btnGuessWord.IsEnabled = false;
            btnHint.IsEnabled = false;
        }

        // =================== HISTORY ===================

        private void BtnHistory_Click(object sender, RoutedEventArgs e)
        {
            ShowHistory();
        }

        private void BtnBackToGame_Click(object sender, RoutedEventArgs e)
        {
            ShowGame();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (player != null)
            {
                DataStore.SavePlayer(player);
            }
            base.OnClosed(e);
        }
    }
}
