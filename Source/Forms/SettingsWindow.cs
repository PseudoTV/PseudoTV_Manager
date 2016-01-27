using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using PseudoTV_Manager.Enum;
using PseudoTV_Manager.Properties;

namespace PseudoTV_Manager.Forms
{

    public partial class SettingsWindow : Form
    {
        public SettingsWindow(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            _addonDbFileDialog = new OpenFileDialog();
            _videoDbFileDialog = new OpenFileDialog();
            _settingsFileDialog = new OpenFileDialog();
            InitializeComponent();
        }

        private readonly MainWindow _mainWindow;

        public string User = Environment.UserName;
        private string _kodiVersionString;

        public KodiVersion KodiVersion = KodiVersion.Helix;

        private readonly OpenFileDialog _videoDbFileDialog;
        private readonly OpenFileDialog _settingsFileDialog;
        private readonly OpenFileDialog _addonDbFileDialog
            ;
        private void BtnVideoDbLocationBrowse_Click(object sender, EventArgs e)
        {
            _kodiVersionString = XbmcVersion.SelectedIndex == 0 ? "XBMC" : "Kodi";

            _videoDbFileDialog.InitialDirectory = "C:\\Users\\" + User + "\\AppData\\Roaming\\" + _kodiVersionString + "\\userdata\\Database";

            _videoDbFileDialog.DefaultExt = "";
            _videoDbFileDialog.Filter = "SqliteDB files (*.db)|*MyVideos*.db";

            if ((_videoDbFileDialog.ShowDialog() == DialogResult.OK))
            {
                TxtVideoDbLocation.Text = _videoDbFileDialog.FileName;
            }
        }

        private void BtnPseudoTvSettingsLocationBrowse_Click(object sender, EventArgs e)
        {
            _settingsFileDialog.InitialDirectory = "C:\\Users\\" + User + "\\AppData\\Roaming\\" + _kodiVersionString + "\\userdata\\addon_data\\script.pseudotv.live";

            _settingsFileDialog.DefaultExt = "";
            _settingsFileDialog.Filter = "Settings2 files (*.xml)|**.xml";

            if ((_settingsFileDialog.ShowDialog() == DialogResult.OK))
            {
                TxtPseudoTvSettingsLocation.Text = _settingsFileDialog.FileName;
            }

        }

        private void BtnAddonDbLocationBrowse_Click(object sender, EventArgs e)
        {
            _addonDbFileDialog.InitialDirectory = "C:\\Users\\" + User + "\\AppData\\Roaming\\" + _kodiVersionString + "\\userdata\\Database";

            _addonDbFileDialog.DefaultExt = "";
            _addonDbFileDialog.Filter = "SqliteDB files (*.db)|*Addons*.db";

            if ((_addonDbFileDialog.ShowDialog() == DialogResult.OK))
            {
                TxtAddonDatabaseLocation.Text = _addonDbFileDialog.FileName;
            }

        }


        private void SaveSettings_Click(object sender, EventArgs e)
        {
            if (File.Exists(TxtVideoDbLocation.Text) & File.Exists(TxtPseudoTvSettingsLocation.Text) & File.Exists(TxtAddonDatabaseLocation.Text))
            {
                KodiVersion = PseudoTvUtils.GetKodiVersion(TxtVideoDbLocation.Text);

                if (PseudoTvUtils.TestMySqlLite(TxtVideoDbLocation.Text) != true) return;

                //Now, update all settings
                Settings.Default.DatabaseType = 0;
                Settings.Default.VideoDatabaseLocation = TxtVideoDbLocation.Text;
                Settings.Default.PseudoTvSettingsLocation = TxtPseudoTvSettingsLocation.Text;
                Settings.Default.AddonDatabaseLocation = TxtAddonDatabaseLocation.Text;
                Settings.Default.KodiVersion = (int)KodiVersion;
                Settings.Default.Save();

                //Refresh everything
                _mainWindow.RefreshAll();
                _mainWindow.RefreshTvGuide();

                Visible = false;
                _mainWindow.Focus();
            }
            else if (!string.IsNullOrEmpty(TxtMySqlServer.Text) & !string.IsNullOrEmpty(TxtMySqlUserId.Text) & !string.IsNullOrEmpty(TxtMySqlDatabase.Text) & File.Exists(TxtPseudoTvSettingsLocation.Text) & File.Exists(TxtAddonDatabaseLocation.Text))
            {
                var connectionString = "server=" + TxtMySqlServer.Text + "; user id=" + TxtMySqlUserId.Text + "; password=" + TxtMySqlPassword.Text + "; database=" + TxtMySqlDatabase.Text + "; port=" + TxtMySqlPort.Text;

                if (!PseudoTvUtils.TestMySql(connectionString)) return;
                
                //Now, update all settings
                Settings.Default.DatabaseType = 1;
                Settings.Default.MySqlConnectionString = connectionString;
                Settings.Default.PseudoTvSettingsLocation = TxtPseudoTvSettingsLocation.Text;
                Settings.Default.AddonDatabaseLocation = TxtAddonDatabaseLocation.Text;
                Settings.Default.KodiVersion = (int)KodiVersion;
                Settings.Default.Save();

                //Refresh everything
                _mainWindow.RefreshAll();
                _mainWindow.RefreshTvGuide();

                Visible = false;
                _mainWindow.Focus();
            }
        }


        private void SettingsWindow_Load(object sender, EventArgs e)
        {
            switch (KodiVersion)
            {
                case KodiVersion.Gotham:
                    XbmcVersion.SelectedIndex = 0;
                    break;
                case KodiVersion.Helix:
                    XbmcVersion.SelectedIndex = 1;
                    break;
                case KodiVersion.Isengard:
                    XbmcVersion.SelectedIndex = 2;
                    break;
                case KodiVersion.Jarvis:
                    XbmcVersion.SelectedIndex = 3;
                    break;
            }

            if (!string.IsNullOrEmpty(Settings.Default.VideoDatabaseLocation))
            {
                TxtVideoDbLocation.Text = Settings.Default.VideoDatabaseLocation;
                TxtPseudoTvSettingsLocation.Text = Settings.Default.PseudoTvSettingsLocation;
                TxtAddonDatabaseLocation.Text = Settings.Default.AddonDatabaseLocation;
            }
            else
            {
                FindKodiSettings();
            }

            if (string.IsNullOrEmpty(Settings.Default.MySqlConnectionString)) return;
            var splitString = Settings.Default.MySqlConnectionString.Split(';');

            TxtPseudoTvSettingsLocation.Text = Settings.Default.PseudoTvSettingsLocation;
            TxtMySqlServer.Text = Regex.Split(splitString[0], "server=")[1];
            TxtMySqlUserId.Text = Regex.Split(splitString[1], "user id=")[1];
            TxtMySqlPassword.Text = Regex.Split(splitString[2], "password=")[1];
            TxtMySqlDatabase.Text = Regex.Split(splitString[3], "database=")[1];
            TxtMySqlPort.Text = Regex.Split(splitString[4], "port=")[1];
        }

        private void FindKodiSettings()
        {
            var folderKodi = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\kodi\\userdata";
            var folderXbmc = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.xbmc\\userdata";
            string databaseFolder = null;
            string addonDataFolder = null;

            if ((Directory.Exists(folderKodi)))
            {
                databaseFolder = folderKodi + "\\Database";
                addonDataFolder = folderKodi + "\\addon_data";
            }
            else if ((Directory.Exists(folderXbmc)))
            {
                databaseFolder = folderXbmc + "\\Database";
                addonDataFolder = folderXbmc + "\\addon_data";
            }
            else
            {
                return;
            }

            var regex = new Regex("(Addons|MyVideos)(\\d+).db");
            var databaseDir = new DirectoryInfo(databaseFolder);
            var filelist = databaseDir.GetFiles();

            foreach (var file in filelist)
            {
                var match = regex.Match(file.Name);
                if (!match.Success) continue;
                switch (match.Groups[1].Value)
                {
                    case "MyVideos":
                        KodiVersion = PseudoTvUtils.GetKodiVersion(file.Name);
                        TxtVideoDbLocation.Text = file.FullName;
                        break;
                    case "Addons":
                        TxtAddonDatabaseLocation.Text = file.FullName;
                        break;
                }
            }

            //C:\Users\%CurrentUser%\AppData\Roaming\Kodi\userdata\addon_data\script.pseudotv.live\settings2.xml
            if ((File.Exists(addonDataFolder + "\\script.pseudotv.live\\settings2.xml")))
            {
                TxtPseudoTvSettingsLocation.Text = addonDataFolder + "\\script.pseudotv.live\\settings2.xml";
            }

            switch (KodiVersion)
            {
                case KodiVersion.Gotham:
                    XbmcVersion.SelectedIndex = 0;
                    break;
                case KodiVersion.Helix:
                    XbmcVersion.SelectedIndex = 1;
                    break;
                case KodiVersion.Isengard:
                    XbmcVersion.SelectedIndex = 2;
                    break;
                case KodiVersion.Jarvis:
                    XbmcVersion.SelectedIndex = 3;
                    break;
            }
        }

        //This looks up the Genre based on the name and returns the proper Genre ID
        public int ReadVersion(string genreName)
        {
            var genreId = 0;

            var selectArray = new[] { 0 };

            //Shoot it over to the ReadRecord sub
            var returnArray = PseudoTvUtils.DbReadRecord(TxtVideoDbLocation.Text, "SELECT idVersion FROM KodiVersion", selectArray);

            //The ID # is all we need.
            //Just make sure it's not a null reference.
            if (returnArray == null)
                PseudoTvUtils.ShowInputDialog("Could not find version of Kodi installed! KodiVersion table didn't return an idVersion");
            else
                genreId = Convert.ToInt32(returnArray[0]);

            return genreId;
        }
    }
}
