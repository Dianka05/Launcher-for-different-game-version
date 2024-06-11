using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Launcher_for_different_game_version
{
    public partial class MainWindow : Window
    {
        private readonly Point[] buttonPositions;
        private readonly List<string> installedVersions = new List<string>();

        private Button choosenVersion;
        private string gameZip;
        private string gameExe;
        private string rootPath;

        public MainWindow()
        {
            InitializeComponent();
            choosenVersion = VersionButton1_0;
            buttonPositions = ButtonGrid.Children.OfType<Button>().Select(button => new Point(Canvas.GetLeft(button), Canvas.GetTop(button))).ToArray();
            CheckInstalledVersionsAndUpdateUI();
        }
        private void CheckInstalledVersionsAndUpdateUI()
        {
            foreach (var button in ButtonGrid.Children.OfType<Button>())
            {
                var version = button.Content.ToString();
                rootPath = Path.Combine(Directory.GetCurrentDirectory(), "GameFiles");
                if (!Directory.Exists(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                }
                gameZip = Path.Combine(rootPath, $"TimeGDPS {version}.zip");
                gameExe = Path.Combine(Path.Combine(rootPath, $"TimeGDPS {version}"), $"TimeGDPS {version}.exe");

                if (File.Exists(gameExe) == true)
                {
                    string InstalledImagePath = Path.Combine(Directory.GetCurrentDirectory(), "Img", $"Ver {version}.png");
                    button.Background = new ImageBrush(new BitmapImage(new Uri(InstalledImagePath)));
                    installedVersions.Add(version);
                    DownloadButton.Content = "Играть";
                }
                else
                {
                    DownloadButton.Content = "Загрузить";
                    string notInstalledImagePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "Img"), "un", $"unVer {version}.png");
                    button.Background = new ImageBrush(new BitmapImage(new Uri(notInstalledImagePath)));
                }
            }
        }


        private void VersionButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var version = button.Content.ToString();
            choosenVersion = button;
            if (installedVersions.Contains(version))
            {
                DownloadButton.Content = "Играть";
            }
            else
            {
                DownloadButton.Content = "Загрузить";
            }

            var index = ButtonGrid.Children.IndexOf(button);

          
            var steps = index - (ButtonGrid.Children.Count / 2);

          
            var rotationAngle = steps * 36;
            var rotationAnimation = new DoubleAnimation(0, rotationAngle, TimeSpan.FromSeconds(0.001));

            rotationAnimation.Completed += (s, _) =>
            {
 
                MoveButtonsCircularly(index);
              
                CurentVersionText.Content = "Ver. " + button.Content.ToString();

                choosenVersion.Width = 198;
                choosenVersion.Height = 198;

                DownloadButton.Content = installedVersions.Contains(version) ? "Играть" : "Загрузить";
            };

            HiddenButton.BeginAnimation(Button.OpacityProperty, rotationAnimation);
          
        }
        private void MoveButtonsCircularly(int index)
        {
            for (int i = 0; i < ButtonGrid.Children.Count; i++)
            {
                int newIndex = (index + i) % ButtonGrid.Children.Count;
                var targetButton = (Button)ButtonGrid.Children[newIndex];
                targetButton.Width = 148;
                targetButton.Height = 148;

                var targetPosition = buttonPositions[(newIndex - index + ButtonGrid.Children.Count) % ButtonGrid.Children.Count];
                MoveButton(targetButton, targetPosition);
            }
            
        }
        private void MoveButton(Button button, Point newPosition)
        {
            var initialPositionX = Canvas.GetLeft(button);
            var initialPositionY = Canvas.GetTop(button);
      

            if (double.IsNaN(initialPositionX) || double.IsNaN(initialPositionY)){}
            else
            {
                var animationX = new DoubleAnimation(initialPositionX, newPosition.X, TimeSpan.FromSeconds(0.5));
                var animationY = new DoubleAnimation(initialPositionY, newPosition.Y, TimeSpan.FromSeconds(0.5));

                button.BeginAnimation(Canvas.LeftProperty, animationX);
                button.BeginAnimation(Canvas.TopProperty, animationY);
            }

            
        }
        private void HiddenButton_Click(object sender, RoutedEventArgs e) {}
        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            switch (choosenVersion.Content)
            {
                case "1.0":
                    CustomMessageBox.Show("Версия: " + choosenVersion.Content + " доступна только в мобильной версии");
                    break;
                case "1.1":
                    CustomMessageBox.Show("Версия: " + choosenVersion.Content + " доступна только в мобильной версии");
                    break;
                case "1.3":
                    CustomMessageBox.Show("Версия: " + choosenVersion.Content + " доступна только в мобильной версии");
                    break;
                case "1.4":
                    CustomMessageBox.Show("Версия: " + choosenVersion.Content + " доступна только в мобильной версии");
                    break;
                case "1.5":
                    CustomMessageBox.Show("Версия: " + choosenVersion.Content + " доступна только в мобильной версии");
                    break;
                case "1.6":
                    CustomMessageBox.Show("Версия: " + choosenVersion.Content + " доступна только в мобильной версии");
                    break;
                case "1.7":
                    CustomMessageBox.Show("Версия: " + choosenVersion.Content + " доступна только в мобильной версии");
                    break;
                case "1.8":
                    CustomMessageBox.Show("Версия: " + choosenVersion.Content + " доступна только в мобильной версии");
                    break;
                case "1.9":
                    CheckIfInstall();
                    break;
                case "2.0":
                    CheckIfInstall();
                    break;
                case "2.1":
                    CheckIfInstall();
                    break;
                case "2.2":
                    CheckIfInstall();
                    break;
                default:
                    break;
            }
        }
        public void CheckIfInstall()
        {

            if (installedVersions.Contains(choosenVersion.Content))
            {
                DownloadButton.Content = "Играть";
             
                gameZip = Path.Combine(rootPath, $"TimeGDPS {choosenVersion.Content}.zip");
                gameExe = Path.Combine(Path.Combine(rootPath, $"TimeGDPS {choosenVersion.Content}"), $"TimeGDPS {choosenVersion.Content}.exe");
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo(gameExe);
                    startInfo.WorkingDirectory = Path.Combine(rootPath, $"TimeGDPS {choosenVersion.Content}");
                    startInfo.UseShellExecute = false;
                    Process.Start(startInfo);
                }
                catch (Exception ex)
                {
                    CustomMessageBox.Show($"Ошибка при запуске процесса игры: {ex.Message}");
                }

            }
            else
            {
                DownloadButton.Content = "Загрузить";
                CustomMessageBox.Show("Версия: " + choosenVersion.Content + " устанавливается");
                InstallGameVersion(choosenVersion.Content.ToString());
            }
        }
  

        public void InstallGameVersion(string version)
        {
          
            if (choosenVersion != null)
            {
                if (!installedVersions.Contains(version))
                {
                    switch (version)
                    {
                        case "1.9":
                            InstalingProcees(version);
                            break;
                        case "2.0":
                            InstalingProcees(version);
                            break;
                        case "2.1":
                            InstalingProcees(version);
                            break;
                        case "2.2":
                            InstalingProcees(version);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    //CustomMessageBox.Show($"Version {version} is already installed.");
                }
            }
        }

        public void InstalingProcees(string version)
        {

            WebClient webClient = new WebClient();
            gameZip = Path.Combine(rootPath, $"TimeGDPS {version}.zip");
            gameExe = Path.Combine(Path.Combine(rootPath, $"TimeGDPS {version}"), $"TimeGDPS {version}.exe");

            LabelProgress.Visibility = Visibility.Visible;
            ProgressBar.Visibility = Visibility.Visible;

            webClient.DownloadProgressChanged += (s, e) =>
            {
                LabelProgress.Text = $"Downloading: {e.ProgressPercentage}% ({((double)e.BytesReceived / 1048576).ToString("#.#")} MB)";
                ProgressBar.Value = e.ProgressPercentage;
            };


           
            switch (version)
            {
                case "1.9":
                    webClient.DownloadFileTaskAsync(new Uri("https://www.dropbox.com/scl/fi/msx7c0s060ho9uleu2r1t/TimeGDPS-1.9.zip?rlkey=0pgokxpud03c2ch96yrejgnf5&st=ckc470rm&dl=1"), gameZip);
                    break;
                case "2.0":
                    webClient.DownloadFileTaskAsync(new Uri("https://www.dropbox.com/scl/fi/9hmqsitcchulmwy1x3w34/TimeGDPS-2.0.zip?rlkey=15tu2g94k2gtz3q7tee8upn6w&st=ubfb1bd5&dl=1"), gameZip);
                    //webClient.DownloadFileAsync(new Uri("https://www.dropbox.com/scl/fo/bqhoya3shr704wgsufamn/ACY7HWHCGAQw189aS8QCHFA?rlkey=rg3wyjeh360khlq60w8u4c4ex&st=zi12rzlj&dl=1"), gameZip);
                    break;
                case "2.1":
                    webClient.DownloadFileTaskAsync(new Uri("https://www.dropbox.com/scl/fi/4xy0el5i1kxuo401hp7ps/TimeGDPS-2.1.zip?rlkey=r1eo9mosb5nukndjzktkyvfd9&st=i4b09748&dl=1"), gameZip);
                    break;
                case "2.2":
                    webClient.DownloadFileTaskAsync(new Uri("https://www.dropbox.com/scl/fi/alew5isiympabxgtxour7/TimeGDPS-2.2.zip?rlkey=tltz029j9yogxc277byyswggb&st=fav9yasb&dl=1"), gameZip);
                    break;
                default:
                    break;
            }
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadGameCompletedCallback);
            //CheckInstalledVersionsAndUpdateUI();

        }
        private void DownloadGameCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            var version = choosenVersion.Content.ToString();
            gameZip = Path.Combine(rootPath, $"TimeGDPS {version}.zip");
            gameExe = Path.Combine(Path.Combine(rootPath, $"TimeGDPS {version}"), $"TimeGDPS {version}.exe");
            try
            {
                string targetDirectory = rootPath;
                string targetFilePath = Path.Combine(rootPath, gameExe);

                LabelProgress.Visibility = Visibility.Hidden;
                ProgressBar.Visibility = Visibility.Hidden;

                if (!File.Exists(targetFilePath))
                {
                    System.IO.Compression.ZipFile.ExtractToDirectory(gameZip, targetDirectory, Encoding.Default);
                }
                else
                {
                    System.IO.Compression.ZipFile.ExtractToDirectory(gameZip, targetDirectory, Encoding.Default);
                }

                File.Delete(gameZip);
                CheckInstalledVersionsAndUpdateUI();
                CustomMessageBox.Show("Версия: " + choosenVersion.Content + " установлена");
                DownloadButton.Content = "Играть";
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show($"Ошибка в процессе извлечения: {ex.Message}");
            }
        }


    }
}
