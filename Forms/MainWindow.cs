using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using NLog;
using PseudoTV_Manager.Enum;
using PseudoTV_Manager.Properties;

namespace PseudoTV_Manager.Forms
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //For sorting columns in listviews
        private ColumnHeader _mSortingColumn;
        private ColumnHeader _mSortingColumn2;

        public string PluginNotInclude;
        public string YouTubeMulti;
        private int _resetHours;

        protected Logger Logger = LogManager.GetCurrentClassLogger();

        public int LookUpGenre(string genreName)
        {
            //This looks up the Genre based on the name and returns the proper Genre ID
            var genreId = 0;

            var selectArray = new[] { 0 };

            var genrePar = "name";
            if ((Settings.Default.KodiVersion < (int)KodiVersion.Isengard))
            {
                genrePar = "strGenre";
            }

            //Shoot it over to the ReadRecord sub
            var returnArray = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                "SELECT * FROM genre where " + genrePar + "='" + genreName + "'", selectArray);

            //The ID # is all we need.
            //Just make sure it's not a null reference.
            if (returnArray == null) MessageBox.Show("nothing!");
            else genreId = Convert.ToInt32(returnArray[0]);

            return genreId;
        }

        public string LookUpNetwork(string network)
        {
            //This looks up the Network name and returns the proper Network ID

            string networkId = null;

            var selectArray = new[] { 0 };

            var studioPar = "name";
            if ((Settings.Default.KodiVersion < (int)KodiVersion.Isengard))
            {
                studioPar = "strStudio";
            }

            //Shoot it over to the ReadRecord sub
            var returnArray = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                "SELECT * FROM studio where " + studioPar + "='" + network + "'", selectArray);

            //The ID # is all we need.
            //Just make sure it's not a null reference.
            if (returnArray == null)
            {
            }
            else
            {
                networkId = returnArray[0];
            }
            return networkId;

        }

        #region Refresh Methods
        public void RefreshTvGuide()
        {
            //Clear the TV name and the List items
            TVGuideShowName.Text = "";
            TVGuideList.Items.Clear();

            var totalChannels = 0;

            //This will hold an array of our channel #s
            string[] channelArray = null;
            var channelNum = 0;

            var fileLocation = Settings.Default.PseudoTvSettingsLocation;

            if (System.IO.File.Exists(Settings.Default.PseudoTvSettingsLocation) == true)
            {
                //Load everything into the FullFile string
                var fullFile = PseudoTvUtils.ReadFile(Settings.Default.PseudoTvSettingsLocation);

                using (var objReader = new System.IO.StreamReader(fileLocation))
                {
                    while (objReader.Peek() != -1)
                    {
                        var singleLine = objReader.ReadLine();

                        if (singleLine.Contains("_type" + (char)34 + " value="))
                        {
                            var part1 = Regex.Split(singleLine, "_type")[0];
                            var part2 = part1.Split('_')[1];

                            Array.Resize(ref channelArray, channelNum + 1);
                            channelNum = channelNum + 1;
                            channelArray[channelArray.Length - 1] = part2;

                        }

                    }
                }

                for (var x = 0; x <= channelArray.Length - 1; x++)
                {
                    string[] channelInfo = null;

                    var channelRules = "";
                    var channelRulesAdvanced = "";
                    var channelRulesCount = "";
                    var channelType = "";
                    var channelTypeDetail = "";
                    var channelTime = "";
                    var channelTypeDetail2 = "";
                    var channelTypeDetail3 = "";
                    var channelTypeDetail4 = "";

                    //Grab everything that says setting id = Channel #
                    channelInfo = Regex.Split(fullFile, "<setting id=" + (char)34 + "Channel_" + channelArray[x] + "_");

                    //Now loop through everything it returned.
                    for (var y = 1; y <= channelInfo.Length - 1; y++)
                    {
                        string ruleType = null;
                        string ruleValue = null;

                        ruleType = channelInfo[y].Split((char)34)[0];

                        ruleValue = Regex.Split(channelInfo[y], "value=" + (char)34)[1];
                        ruleValue = ruleValue.Split((char)34)[0];


                        switch (ruleType)
                        {
                            case "changed":
                                break;
                            case "rulecount":
                                channelRulesCount = ruleValue;
                                break;
                            case "time":
                                channelTime = ruleValue;
                                break;
                            case "type":
                                //Update the Channel type to the value of that.
                                channelType = ruleValue;
                                break;
                            case "1":
                                //Gets more information on what type the channel is, playlist location/genre/zap2it/etc.
                                channelTypeDetail = ruleValue;
                                break;
                            case "2":
                                //Gets (LiveTV-8)stream source/(IPTV-9)iptv source/(Youtube-10)youtube channel type/
                                //(Rss-11)reserved/(LastFM-13)LastFM User/(BTP/Cinema Experience14)filter types or smart playlist/
                                //(Direct/SF-15, Direct Playon-16)exclude list/
                                channelTypeDetail2 = ruleValue;
                                break;
                            case "3":
                                //Gets (LiveTV-8)xmltv filename/(IPTV-9)show titles/(Youtube-10, Rss-11, LastFM-13)media limits/
                                //(BTP/Cinema Experience-14)parsing resolution/(Direct/SF-15, Direct Playon-16)file limit
                                channelTypeDetail3 = ruleValue;
                                break;
                            case "4":
                                //Gets (IPTV-9)show description/(Youtube-10, Rss-11, LastFM-13)sort ordering/
                                //(BTP/Cinema Experience-14)years to parse by or unused/(Direct/SF-15, Direct Playon-16)sort ordering
                                channelTypeDetail4 = ruleValue;
                                break;
                            default:
                                if (ruleType.Contains("rule"))
                                {
                                    //Okay, It's rule information.

                                    //Get the rule number.
                                    string ruleNumber = null;
                                    ruleNumber = Regex.Split(ruleType, "rule_")[1];
                                    ruleNumber = ruleNumber.Split('_')[0];

                                    if (ruleType.Contains("opt"))
                                    {
                                        //Okay, it's an actual option tied to another rule.

                                        var optNumber = Regex.Split(ruleType, "_opt_")[1];
                                        ruleNumber = Regex.Split(ruleType, "_opt_")[0];
                                        ruleNumber = Regex.Split(ruleNumber, "rule_")[1];

                                        //MsgBox("Opt : " & RuleNumber & " | " & OptNumber & " | " & RuleValue)
                                        //ChannelRulesAdvanced = ChannelRulesAdvanced & "~" & RuleNumber & "|" & OptNumber & "|" & RuleValue
                                        //MsgBox(RuleNumber & " | " & OptNumber & " | " & RuleValue)

                                        //Add this to the previous rule, remove the ending 
                                        //Then add this rule as Rule#:RuleValue
                                        channelRules = channelRules + "|" + optNumber + "^" + ruleValue;
                                    }
                                    else
                                    {
                                        channelRules = channelRules + "~" + ruleNumber + "|" + ruleValue;
                                    }

                                }
                                break;
                        }

                        //End result for a basic option:  ~RuleNumber|RuleValue 
                        //End result for an advanced option:  ~RuleNumber|RuleValue|Rule1^Rule1Value|Rule2^Rule2Value

                    }

                    var str = new string[11];

                    str[0] = channelArray[x];
                    //Channel #.  
                    str[1] = channelType;
                    str[2] = channelTypeDetail;
                    str[3] = channelTime;
                    str[4] = channelRules;
                    str[5] = channelRulesCount;
                    str[6] = channelTypeDetail2;
                    str[7] = channelTypeDetail3;
                    str[8] = channelTypeDetail4;

                    var itm = new ListViewItem(str);
                    //Add to list
                    TVGuideList.Items.Add(itm);

                }

            }

            //Sort List
            TVGuideList.ListViewItemSorter = new ListViewSorter(0, SortOrder.Ascending);
            // Sort. 
            TVGuideList.Sort();
        }

        public void RefreshGenres()
        {
            GenresList.Items.Clear();
            var selectArrayMain = new[] { 0, 1 };


            //Shoot it over to the ReadRecord sub
            var returnArrayMain = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation, "SELECT * FROM genre",
                selectArrayMain);

            //Loop through and read the name


            for (var x = 0; x <= returnArrayMain.Length - 1; x++)
            {
                //Sort them into an array
                var splitItem = returnArrayMain[x].Split('~');
                //Position 0 = genre ID
                //Position 1 = genre name

                //Push array into ListViewItem

                var selectArray = new[] { 1 };

                //Now, grab a list of all the shows that match the GenreID
                string[] returnArray = null;
                if ((Settings.Default.KodiVersion >= (int)KodiVersion.Isengard))
                {
                    returnArray = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                        "SELECT * FROM genre_link WHERE genre_id='" + splitItem[0] + "' AND media_type = 'tvshow'",
                        selectArray);
                }
                else
                {
                    returnArray = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                        "SELECT * FROM genrelinktvshow WHERE idGenre='" + splitItem[0] + "'", selectArray);
                }

                //This will grab the number of movies.
                string[] returnArray2 = null;
                if ((Settings.Default.KodiVersion >= (int)KodiVersion.Isengard))
                {
                    returnArray2 = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                        "SELECT * FROM genre_link WHERE genre_id='" + splitItem[0] + "' AND media_type = 'movie'",
                        selectArray);
                }
                else
                {
                    returnArray2 = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                        "SELECT * FROM genrelinkmovie WHERE idGenre='" + splitItem[0] + "'", selectArray);
                }

                int showNum;
                int movieNum;

                //This is the total number of shows that match this genre.
                //Also, verify the returning array is something, not null before proceeding.
                if (returnArray == null)
                {
                    showNum = 0;
                }
                else
                {
                    showNum = returnArray.Length;
                }

                if (returnArray2 == null)
                {
                    movieNum = 0;
                }
                else
                {
                    movieNum = returnArray2.Length;
                }

                var str = new string[5];
                //Genre Name
                //# of shows in genre
                //# of movies in genre
                //Total of both /\
                //Genre ID

                str[0] = splitItem[1];
                str[1] = showNum.ToString();
                str[2] = movieNum.ToString();
                str[3] = (showNum + movieNum).ToString();
                str[4] = splitItem[0];


                var itm = new ListViewItem(str);
                //Add to list
                GenresList.Items.Add(itm);

            }

            GenresList.Sort();
        }

        public void RefreshTvShows()
        {
            TVShowList.Items.Clear();

            //Set an array with the columns you want returned
            var selectArray = new[] { 1, 15, 0 };

            //Shoot it over to the ReadRecord sub, 
            var returnArray = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation, "SELECT * FROM tvshow ORDER BY c00",
                selectArray);

            //Now, read the output of the array.
            //Loop through each of the Array items.

            for (var x = 0; x <= returnArray.Length - 1; x++)
            {
                //Split them by ~'s.  This is how we seperate the rows in the single-element.
                var str = returnArray[x].Split('~');

                //Now take that split string and make it an item.
                var itm = default(ListViewItem);
                itm = new ListViewItem(str);

                //Add the item to the TV show list.
                TVShowList.Items.Add(itm);
            }
        }

        private void RefreshPlugins()
        {
            PluginType.Items.Clear();

            var addonLike = "plugin.video";
            var selectArray = new[] { 1 };

            //Grab the Plugin List
            var returnArray = PseudoTvUtils.ReadPluginRecord(Settings.Default.AddonDatabaseLocation,
                "SELECT DISTINCT addon.addonID, addon.name FROM addon, package WHERE addon.addonID = package.addonID and addon.addonID LIKE '" +
                addonLike + "%'", selectArray);

            for (var x = 0; x <= returnArray.Length - 1; x++)
            {
                //Split them by ~'s.  This is how we seperate the rows in the single-element.
                var str = returnArray[x].Split('~');

                //Add the item to the Plugins List
                PluginType.Items.Add(str[0]);
            }

        }

        public void RefreshMovieList()
        {
            MovieList.Items.Clear();

            //Set an array with the columns you want returned
            var selectArray = new[] { 2, 0 };

            //Shoot it over to the ReadRecord sub, 
            var returnArray = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation, "SELECT * FROM movie ORDER BY c00",
                selectArray);

            //Now, read the output of the array.
            //Loop through each of the Array items.

            for (var x = 0; x <= returnArray.Length - 1; x++)
            {
                //Split them by ~'s.  This is how we seperate the rows in the single-element.
                var str = returnArray[x].Split('~');

                //Now take that split string and make it an item.
                var itm = default(ListViewItem);
                itm = new ListViewItem(str);

                //Add the item to the TV show list.
                MovieList.Items.Add(itm);
            }
        }

        public void RefreshSettings()
        {
            Settings.Default.Reload();


            RefreshAll();
            RefreshTvGuide();

            //TODO:
            //    //System.IO.File.Create(SettingsFile)
            //    MessageBox.Show(
            //        "Unable to locate the location of XBMC video library and PseudoTV's setting location.  Please enter them and save the changes.");
            //    Form6 mySettings = new Form6();
            //    //mySettings.Version = Me.Version
            //    mySettings.Show();
            //}
        }

        public void RefreshNetworkListMovies()
        {
            MovieNetworkList.Items.Clear();

            var selectArray = new[] { 2, 20 };

            var returnArray = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                "SELECT * FROM movie ORDER BY c18 ASC",
                selectArray);

            //Loop through each returned Movie
            for (var x = 0; x <= returnArray.Length - 1; x++)
            {
                if ((string.IsNullOrEmpty(returnArray[x])))
                {
                    continue;
                }

                string[] returnArraySplit = null;

                string showName = null;
                string showNetwork = null;

                returnArraySplit = returnArray[x].Split('~');

                showName = returnArraySplit[0];
                //Updated ReturnArraySplit for ShowNetwork to reflect MyVideos78.db schema
                showNetwork = returnArraySplit[1];

                var networkListed = false;


                for (var y = 0; y <= MovieNetworkList.Items.Count - 1; y++)
                {
                    if (MovieNetworkList.Items[y].SubItems[0].Text == showNetwork)
                    {
                        networkListed = true;
                        MovieNetworkList.Items[y].SubItems[1].Text = MovieNetworkList.Items[y].SubItems[1].Text + 1;
                    }

                }

                if (networkListed == false)
                {
                    var itm = default(ListViewItem);
                    var str = new string[2];

                    str[0] = showNetwork;
                    str[1] = "1";

                    itm = new ListViewItem(str);

                    //Add the item to the TV show list.
                    MovieNetworkList.Items.Add(itm);

                }

            }

        }

        public void RefreshNetworkList()
        {
            NetworkList.Items.Clear();

            var selectArray = new[] { 1, 15 };

            var returnArray = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                "SELECT * FROM tvshow ORDER BY c14 ASC",
                selectArray);

            //Loop through each returned TV show.

            for (var x = 0; x <= returnArray.Length - 1; x++)
            {

                string[] returnArraySplit = null;

                string showName = null;
                string showNetwork = null;

                returnArraySplit = returnArray[x].Split('~');

                showName = returnArraySplit[0];
                showNetwork = returnArraySplit[1];

                var networkListed = false;

                for (var y = 0; y <= NetworkList.Items.Count - 1; y++)
                {
                    if (NetworkList.Items[y].SubItems[0].Text == showNetwork)
                    {
                        networkListed = true;
                        NetworkList.Items[y].SubItems[1].Text = NetworkList.Items[y].SubItems[1].Text + 1;
                    }

                }

                if (networkListed == false)
                {
                    var itm = default(ListViewItem);
                    var str = new string[2];

                    str[0] = showNetwork;
                    str[1] = "1";

                    itm = new ListViewItem(str);

                    //Add the item to the TV show list.
                    NetworkList.Items.Add(itm);

                }

            }

        }

        public void RefreshAllGenres()
        {
            var savedText = Option2.Text;
            Option2.Items.Clear();
            //Set an array with the columns you want returned
            var selectArray = new[] { 1 };

            //Shoot it over to the ReadRecord sub, 
            var returnArray = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation, "SELECT * FROM genre", selectArray);

            //Now, read the output of the array.

            //Loop through each of the Array items.
            for (var x = 0; x <= returnArray.Length - 1; x++)
            {
                Option2.Items.Add(returnArray[x]);
            }
            Option2.Sorted = true;
            Option2.Text = savedText;
        }

        public void RefreshAllStudios()
        {
            var savedText = Option2.Text;

            //Clear all
            Option2.Items.Clear();
            //Form3.ListBox1.Items.Clear()
            TxtShowNetwork.Items.Clear();
            txtMovieNetwork.Items.Clear();
            //TODO: Form8.ListBox1.Items.Clear();

            //Set an array with the columns you want returned
            var selectArray = new[] { 1 };

            //Shoot it over to the ReadRecord sub, 
            var returnArray = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation, "SELECT * FROM studio", selectArray);

            //Now, read the output of the array.

            //Loop through each of the Array items.
            for (var x = 0; x <= returnArray.Length - 1; x++)
            {
                Option2.Items.Add(returnArray[x]);
                //Form3.ListBox1.Items.Add(ReturnArray[x])
                TxtShowNetwork.Items.Add(returnArray[x]);
                txtMovieNetwork.Items.Add(returnArray[x]);
                //TODO: Form8.ListBox1.Items.Add(returnArray[x]);
            }

            //Sort them all.
            Option2.Sorted = true;
            //Form3.ListBox1.Sorted = True
            //TODO: Form8.ListBox1.Sorted = true;
            TxtShowNetwork.Sorted = true;
            txtMovieNetwork.Sorted = true;
            Option2.Text = savedText;

        }

        public void RefreshAll()
        {
            if (!string.IsNullOrEmpty(Settings.Default.VideoDatabaseLocation) |
                !string.IsNullOrEmpty(Settings.Default.MySqlConnectionString) & !string.IsNullOrEmpty(Settings.Default.PseudoTvSettingsLocation))
            {
                RefreshMovieList();
                RefreshTvShows();
                RefreshPlugins();
                RefreshAllStudios();
                RefreshNetworkList();
                RefreshNetworkListMovies();
                RefreshGenres();
                TxtShowName.Text = "";
                txtShowLocation.Text = "";
                TVPosterPictureBox.ImageLocation = "";
                MovieLocation.Text = "";
                MoviePicture.ImageLocation = "";
            }
        }

        #endregion

        private void MainWindow_Load(object sender, System.EventArgs e)
        {
            HelpList.SelectedIndex = 0;

            RefreshSettings();

            TVShowList.Columns.Add("Show", 100, HorizontalAlignment.Left);
            TVShowList.Columns.Add("Network", 100, HorizontalAlignment.Left);
            TVShowList.Columns.Add("ID", 0, HorizontalAlignment.Left);

            MovieList.Columns.Add("Movie", 300, HorizontalAlignment.Left);
            MovieList.Columns.Add("ID", 0, HorizontalAlignment.Left);

            NetworkList.Columns.Add("Network", 140, HorizontalAlignment.Left);
            NetworkList.Columns.Add("# Shows", 60, HorizontalAlignment.Left);

            MovieNetworkList.Columns.Add("Studio", 170, HorizontalAlignment.Left);
            MovieNetworkList.Columns.Add("# Movies", 60, HorizontalAlignment.Left);

            GenresList.Columns.Add("Genre", 100, HorizontalAlignment.Left);
            GenresList.Columns.Add("# Shows", 60, HorizontalAlignment.Center);
            GenresList.Columns.Add("# Movies", 60, HorizontalAlignment.Center);
            GenresList.Columns.Add("# Total", 80, HorizontalAlignment.Center);
            GenresList.Columns.Add("Genre ID", 0, HorizontalAlignment.Left);

            TVGuideList.Columns.Add("Channel", 200, HorizontalAlignment.Left);
            TVGuideList.Columns.Add("Type", 0, HorizontalAlignment.Left);
            TVGuideList.Columns.Add("TypeDetail", 0, HorizontalAlignment.Left);
            TVGuideList.Columns.Add("Time", 0, HorizontalAlignment.Left);
            TVGuideList.Columns.Add("Rules", 0, HorizontalAlignment.Left);
            TVGuideList.Columns.Add("RuleCount", 0, HorizontalAlignment.Left);

            InterleavedList.Columns.Add("Chan", 50, HorizontalAlignment.Left);
            InterleavedList.Columns.Add("Min", 45, HorizontalAlignment.Left);
            InterleavedList.Columns.Add("Max", 45, HorizontalAlignment.Left);
            InterleavedList.Columns.Add("Epi", 45, HorizontalAlignment.Left);
            InterleavedList.Columns.Add("Epis", 45, HorizontalAlignment.Left);

            SchedulingList.Columns.Add("Chan", 53, HorizontalAlignment.Left);
            SchedulingList.Columns.Add("Days", 45, HorizontalAlignment.Left);
            SchedulingList.Columns.Add("Time", 45, HorizontalAlignment.Left);
            SchedulingList.Columns.Add("Epi", 45, HorizontalAlignment.Left);

            TVGuideSubMenu.Columns.Add("Shows / Movies", 300, HorizontalAlignment.Left);

        }

        #region Events
        private void ListTVBanners_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var x = ListTVBanners.SelectedIndex;
            if (x < 0)
            {
                return;
            }
            if (ListTVBanners.Items.Count <= 0)
            {
                TVBannerPictureBox.ImageLocation = Application.StartupPath + "\\Images\\banner.png";
            }
            else
            {
                TVBannerPictureBox.ImageLocation = ListTVBanners.Items[x].ToString();
                TVBannerPictureBox.Refresh();
            }
        }

        private void TVBannerSelect_Click(System.Object sender, System.EventArgs e)
        {
            var x = ListTVBanners.SelectedIndex;
            var type = "tvshow";
            var mediaType = "banner";

            if (txtShowLocation.TextLength >= 6)
            {
                if (txtShowLocation.Text.Substring(0, 6) == "smb://")
                {
                    txtShowLocation.Text = txtShowLocation.Text.Replace("/", "\\");
                    txtShowLocation.Text = "\\\\" + txtShowLocation.Text.Substring(6);
                }
            }

            // Displays a SaveFileDialog so the user can save the Image
            // assigned to TVBannerSelect.
            var saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = txtShowLocation.Text;
            saveFileDialog1.Filter = "JPeg Image|*.jpg";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.FileName = "banner.jpg";
            saveFileDialog1.ShowDialog();

            var fileToSaveAs = System.IO.Path.Combine(txtShowLocation.Text, saveFileDialog1.FileName);
            TVBannerPictureBox.Image.Save(fileToSaveAs, System.Drawing.Imaging.ImageFormat.Jpeg);

            PseudoTvUtils.DbExecute("UPDATE art SET url = '" + ListTVBanners.Items[x].ToString() +
                                    "' WHERE media_id = '" +
                                    TVShowLabel.Text + "' and type = '" + type + "' and type = '" + mediaType + "'");
            //TODO: VisualStyleElement.Status.Text = "Updated " + TxtShowName.Text + " Successfully with " +
            //                                 ListTVBanners.Items[x].ToString() + "";
        }

        private void ListTVPosters_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var x = ListTVPosters.SelectedIndex;
            if (x < 0)
            {
                return;
            }
            if (ListTVPosters.Items.Count == 0)
            {
                TVPosterPictureBox.ImageLocation = Application.StartupPath + "\\Images\\poster.png";
            }
            else
            {
                TVPosterPictureBox.ImageLocation = ListTVPosters.Items[x].ToString();
                TVPosterPictureBox.Refresh();
            }

        }

        private void TVPosterSelect_Click(System.Object sender, System.EventArgs e)
        {
            var x = ListTVPosters.SelectedIndex;
            var mediaType = "poster";


            if (txtShowLocation.TextLength >= 6)
            {
                if (txtShowLocation.Text.Substring(0, 6) == "smb://")
                {
                    txtShowLocation.Text = txtShowLocation.Text.Replace("/", "\\");
                    txtShowLocation.Text = @"\\" + txtShowLocation.Text.Substring(6);
                }
            }

            // Displays a SaveFileDialog so the user can save the Image
            // assigned to TVPosterSelect.
            var saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = txtShowLocation.Text;
            saveFileDialog1.Filter = "JPeg Image|*.jpg";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.FileName = "poster.jpg";
            saveFileDialog1.ShowDialog();

            var fileToSaveAs = System.IO.Path.Combine(Path.GetTempPath(), saveFileDialog1.FileName);
            TVPosterPictureBox.Image.Save(fileToSaveAs, System.Drawing.Imaging.ImageFormat.Jpeg);

            PseudoTvUtils.DbExecute("UPDATE art SET url = '" + ListTVPosters.Items[x].ToString() +
                                    "' WHERE media_id = '" +
                                    TVShowLabel.Text + "' and type = '" + mediaType + "'");
            //VisualStyleElement.Status.Text = "Updated " + TxtShowName.Text + " Successfully with " +
            //                                 ListTVPosters.Items[x].ToString() + "";

        }

        private void TVShowList_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {

            if (TVShowList.SelectedItems.Count > 0)
            {
                var listItem = default(ListViewItem);
                listItem = TVShowList.SelectedItems[0];

                string tvShowName = null;
                string tvShowId = null;

                tvShowId = listItem.SubItems[2].Text;
                tvShowName = listItem.SubItems[0].Text;

                TVShowLabel.Text = tvShowId;

                var tvShowArray = new int[5];
                tvShowArray[0] = 1;
                tvShowArray[1] = 9;
                tvShowArray[2] = 15;
                tvShowArray[3] = 17;
                tvShowArray[4] = 7;

                //Shoot it over to the ReadRecord sub, 
                var tvShowReturnArray = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                    "SELECT * FROM tvshow WHERE idShow='" + tvShowId + "'", tvShowArray);

                string[] tvShowReturnArraySplit = null;

                //We only have 1 response, since it searches by ID. So, just break it into parts. 
                tvShowReturnArraySplit = tvShowReturnArray[0].Split('~');

                var tvPathArray = new[] { 0, 1 };


                //Shoot it over to the ReadRecord sub, 
                var vidPathArray = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                    "SELECT * FROM tvshowlinkpath WHERE idShow='" + tvShowId + "'", tvPathArray);

                string[] vidPathArraySplit = null;

                vidPathArraySplit = vidPathArray[0].Split('~');

                var tvShowLocationArray = new[] { 0, 1 };

                var tvShowLocationArrayReturn = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                    "SELECT * FROM path WHERE idPath='" + vidPathArraySplit[1] + "'", tvShowLocationArray);

                string[] tvShowLocationSplit = null;

                tvShowLocationSplit = tvShowLocationArrayReturn[0].Split('~');

                TxtShowName.Text = tvShowReturnArraySplit[0];

                var tvGenres = tvShowReturnArraySplit[1];
                if (string.IsNullOrEmpty(tvShowReturnArraySplit[2]))
                {
                    TxtShowNetwork.SelectedIndex = -1;
                }
                else
                {
                    TxtShowNetwork.Text = tvShowReturnArraySplit[2];
                }

                txtShowLocation.Text = tvShowLocationSplit[1];

                var tvPoster = tvShowReturnArraySplit[4];
                ListTVPosters.Items.Clear();

                var tvBanner = tvShowReturnArraySplit[4];
                ListTVBanners.Items.Clear();

                if (tvPoster.Contains("<thumb aspect=\"poster\">"))
                {
                    var tvPosterSplit = Regex.Split(tvPoster, "<thumb aspect=\"poster\">");

                    for (var x = 1; x <= tvPosterSplit.Length - 1; x++)
                    {
                        var i = tvPosterSplit[x].IndexOf("<thumb aspect=\"poster\">");
                        tvPosterSplit[x] = tvPosterSplit[x].Substring(i + 1, tvPosterSplit[x].IndexOf("</thumb>"));
                        ListTVPosters.Items.Add(tvPosterSplit[x]);
                    }
                }
                else
                {
                    ListTVPosters.Items.Add("Nothing Found");
                }

                if (tvBanner.Contains("<thumb aspect=\"banner\">"))
                {
                    var tvBannerSplit = Regex.Split(tvBanner, "<thumb aspect=\"banner\">");

                    for (var x = 1; x <= tvBannerSplit.Length - 1; x++)
                    {
                        var i = tvBannerSplit[x].IndexOf("<thumb aspect=\"banner\">");
                        tvBannerSplit[x] = tvBannerSplit[x].Substring(i + 1, tvBannerSplit[x].IndexOf("</thumb>"));
                        ListTVBanners.Items.Add(tvBannerSplit[x]);
                    }
                }
                else
                {
                    ListTVBanners.Items.Add("Nothing Found");
                }

                //Loop through each TV Genre, if there more than one.
                ListTVGenres.Items.Clear();
                if (tvGenres.Contains(" / "))
                {
                    var tvGenresSplit = Regex.Split(tvGenres, " / ");

                    for (var x = 0; x <= tvGenresSplit.Length - 1; x++)
                    {
                        ListTVGenres.Items.Add(tvGenresSplit[x]);
                    }
                }
                else if (!string.IsNullOrEmpty(tvGenres))
                {
                    ListTVGenres.Items.Add(tvGenres);
                }

                if (txtShowLocation.TextLength >= 6)
                {
                    if (txtShowLocation.Text.Substring(0, 6) == "smb://")
                    {
                        txtShowLocation.Text = txtShowLocation.Text.Replace("/", "\\");
                        txtShowLocation.Text = "\\\\" + txtShowLocation.Text.Substring(6);
                    }
                }

                if (System.IO.File.Exists(txtShowLocation.Text + "poster.jpg"))
                {
                    TVPosterPictureBox.ImageLocation = txtShowLocation.Text + "poster.jpg";
                }
                else
                {
                    TVPosterPictureBox.ImageLocation =
                        "https://github.com/Lunatixz/script.pseudotv.live/raw/development/resources/images/poster.png";
                }

                if (System.IO.File.Exists(txtShowLocation.Text + "banner.jpg"))
                {
                    TVBannerPictureBox.ImageLocation = txtShowLocation.Text + "banner.jpg";
                }
                else
                {
                    TVBannerPictureBox.ImageLocation =
                        "https://github.com/Lunatixz/script.pseudotv.live/raw/development/resources/images/banner.png";
                }
            }

        }

        private void TVShowList_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            // Get the new sorting column. 
            var newSortingColumn = TVShowList.Columns[e.Column];
            // Figure out the new sorting order. 
            SortOrder sortOrder;
            if (_mSortingColumn == null)
            {
                // New column. Sort ascending. 
                sortOrder = SortOrder.Ascending;
                // See if this is the same column. 
            }
            else
            {
                if (newSortingColumn.Equals(_mSortingColumn))
                {
                    // Same column. Switch the sort order. 
                    if (_mSortingColumn.Text.StartsWith("> "))
                    {
                        sortOrder = SortOrder.Descending;
                    }
                    else
                    {
                        sortOrder = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New column. Sort ascending. 
                    sortOrder = SortOrder.Ascending;
                }
                // Remove the old sort indicator. 
                _mSortingColumn.Text = _mSortingColumn.Text.Substring(2);
            }
            // Display the new sort order. 
            _mSortingColumn = newSortingColumn;
            if (sortOrder == SortOrder.Ascending)
            {
                _mSortingColumn.Text = "> " + _mSortingColumn.Text;
            }
            else
            {
                _mSortingColumn.Text = "< " + _mSortingColumn.Text;
            }
            // Create a comparer. 
            TVShowList.ListViewItemSorter = new ListViewSorter(e.Column, sortOrder);
            // Sort. 
            TVShowList.Sort();
        }

        private void NetworkList_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            // Get the new sorting column. 
            var newSortingColumn = NetworkList.Columns[e.Column];
            // Figure out the new sorting order. 
            var sortOrder = default(System.Windows.Forms.SortOrder);
            if (_mSortingColumn == null)
            {
                // New column. Sort ascending. 
                sortOrder = SortOrder.Ascending;
                // See if this is the same column. 
            }
            else
            {
                if (newSortingColumn.Equals(_mSortingColumn))
                {
                    // Same column. Switch the sort order. 
                    if (_mSortingColumn.Text.StartsWith("> "))
                    {
                        sortOrder = SortOrder.Descending;
                    }
                    else
                    {
                        sortOrder = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New column. Sort ascending. 
                    sortOrder = SortOrder.Ascending;
                }
                // Remove the old sort indicator. 
                _mSortingColumn.Text = _mSortingColumn.Text.Substring(2);
            }
            // Display the new sort order. 
            _mSortingColumn = newSortingColumn;
            if (sortOrder == SortOrder.Ascending)
            {
                _mSortingColumn.Text = "> " + _mSortingColumn.Text;
            }
            else
            {
                _mSortingColumn.Text = "< " + _mSortingColumn.Text;
            }
            // Create a comparer. 
            NetworkList.ListViewItemSorter = new ListViewSorter(e.Column, sortOrder);
            // Sort. 
            NetworkList.Sort();
        }

        private void NetworkList_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            NetworkListSubList.Items.Clear();


            if (NetworkList.SelectedIndices.Count > 0)
            {
                var selectArray = new[] { 1 };

                var returnArray = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                    "SELECT * FROM tvshow WHERE c14='" +
                    NetworkList.Items[NetworkList.SelectedIndices[0]].SubItems[0].Text + "'", selectArray);

                for (var x = 0; x <= returnArray.Length - 1; x++)
                {
                    NetworkListSubList.Items.Add(returnArray[x]);
                }

            }
        }

        private void BtnAddGenre_Click(System.Object sender, System.EventArgs e)
        {
            if (TVShowList.SelectedIndices.Count > 0)
            {
                //TODO:
                //Form2.Visible = true;
                //Form2.Focus();
            }
        }

        private void BtnDeleteGenre_Click(System.Object sender, System.EventArgs e)
        {

            if (ListTVGenres.SelectedIndex >= 0)
            {
                //Grab the 3rd column from the TVShowList, which is the TVShowID
                var genreId = LookUpGenre(ListTVGenres.Items[ListTVGenres.SelectedIndex].ToString());

                //Now, remove the link in the database.
                //PseudoTvUtils.DbExecute("DELETE FROM genrelinktvshow WHERE idGenre = '" & GenreID & "' AND idShow ='" & TVShowList.Items(TVShowList.SelectedIndices[0]).SubItems[2].Text & "'")


                ListTVGenres.Items.RemoveAt(ListTVGenres.SelectedIndex);
                // SaveTVShow_Click(Nothing, Nothing)
                RefreshGenres();
            }
        }

        private void BtnRefresh_Click(System.Object sender, System.EventArgs e)
        {
            RefreshAll();
        }

        private void GenresList_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            // Get the new sorting column. 
            var newSortingColumn = GenresList.Columns[e.Column];
            // Figure out the new sorting order. 
            var sortOrder = default(System.Windows.Forms.SortOrder);
            if (_mSortingColumn2 == null)
            {
                // New column. Sort ascending. 
                sortOrder = SortOrder.Ascending;
                // See if this is the same column. 
            }
            else
            {
                if (newSortingColumn.Equals(_mSortingColumn2))
                {
                    // Same column. Switch the sort order. 
                    if (_mSortingColumn2.Text.StartsWith("> "))
                    {
                        sortOrder = SortOrder.Descending;
                    }
                    else
                    {
                        sortOrder = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New column. Sort ascending. 
                    sortOrder = SortOrder.Ascending;
                }
                // Remove the old sort indicator. 
                _mSortingColumn2.Text = _mSortingColumn2.Text.Substring(2);
            }
            // Display the new sort order. 
            _mSortingColumn2 = newSortingColumn;
            if (sortOrder == SortOrder.Ascending)
            {
                _mSortingColumn2.Text = "> " + _mSortingColumn2.Text;
            }
            else
            {
                _mSortingColumn2.Text = "< " + _mSortingColumn2.Text;
            }
            // Create a comparer. 
            GenresList.ListViewItemSorter = new ListViewSorter(e.Column, sortOrder);
            // Sort. 
            GenresList.Sort();
        }

        private void GenresList_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            GenresListSubList.Items.Clear();
            GenresListSubListMovies.Items.Clear();

            if (GenresList.SelectedIndices.Count > 0)
            {
                var selectArray = new[] { 1 };

                //Now, gather a list of all the show IDs that match the genreID
                string[] returnArray = null;
                if ((Settings.Default.KodiVersion >= (int)KodiVersion.Isengard))
                {
                    returnArray = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                        "SELECT * FROM genre_link WHERE genre_id='" +
                        GenresList.Items[GenresList.SelectedIndices[0]].SubItems[4].Text + "' AND media_type = 'tvshow'",
                        selectArray);
                }
                else
                {
                    returnArray = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                        "SELECT * FROM genrelinktvshow WHERE idGenre='" +
                        GenresList.Items[GenresList.SelectedIndices[0]].SubItems[4].Text + "'", selectArray);
                }

                //Now loop through each one individually.

                if (returnArray == null)
                {
                }
                else
                {
                    for (var x = 0; x <= returnArray.Length - 1; x++)
                    {
                        var showNameArray = new string[1];
                        selectArray[0] = 1;

                        var returnArray2 = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                            "SELECT * FROM tvshow WHERE idShow='" + returnArray[x] + "'", selectArray);

                        //Now add that name to the list.
                        GenresListSubList.Items.Add(returnArray2[0]);
                    }
                }

                //MOVIES REPEAT THIS PROCESS.
                string[] returnArrayMovies = null;
                if ((Settings.Default.KodiVersion >= (int)KodiVersion.Isengard))
                {
                    returnArrayMovies = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                        "SELECT * FROM genre_link WHERE genre_id='" +
                        GenresList.Items[GenresList.SelectedIndices[0]].SubItems[4].Text + "' AND media_type = 'movie'",
                        selectArray);
                }
                else
                {
                    returnArrayMovies = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                        "SELECT * FROM genrelinkmovie WHERE idGenre='" +
                        GenresList.Items[GenresList.SelectedIndices[0]].SubItems[4].Text + "'", selectArray);
                }

                //Now loop through each one individually 
                if (returnArrayMovies == null)
                {
                }
                else
                {
                    for (var x = 0; x <= returnArrayMovies.Length - 1; x++)
                    {
                        var showNameArray = new string[1];
                        selectArray[0] = 1;

                        var returnArray2 = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                            "SELECT * FROM movie WHERE idMovie='" + returnArrayMovies[x] + "'", selectArray);

                        //Now add that name to the list.
                        GenresListSubListMovies.Items.Add(returnArray2[0]);
                    }
                }
            }

        }

        private void SaveTVShow_Click(System.Object sender, System.EventArgs e)
        {

            if (TVShowList.SelectedItems.Count > 0)
            {
                // Fix any issues with shows and 's.
                var tvShowName = TxtShowName.Text;
                //Convert show genres into the format ex:  genre1 / genre2 / etc.
                var showGenres = ConvertGenres(ListTVGenres);
                tvShowName = tvShowName.Replace("'", "''");
                //Grab the Network ID based on the name
                var networkId = LookUpNetwork(TxtShowNetwork.Text);

                if ((Settings.Default.KodiVersion >= (int)KodiVersion.Isengard))
                {
                    PseudoTvUtils.DbExecute("DELETE FROM studio_link WHERE media_id = '" + TVShowLabel.Text + "'");
                    PseudoTvUtils.DbExecute("INSERT INTO studio_link(studio_id, media_id, media_type) VALUES ('" +
                                            networkId + "', '" +
                                            TVShowLabel.Text + "', 'tvshow')");
                }
                else
                {
                    PseudoTvUtils.DbExecute("DELETE FROM studiolinktvshow WHERE idShow = '" + TVShowLabel.Text + "'");
                    PseudoTvUtils.DbExecute("INSERT INTO studiolinktvshow (idStudio, idShow) VALUES ('" + networkId +
                                            "', '" +
                                            TVShowLabel.Text + "')");
                }

                PseudoTvUtils.DbExecute("UPDATE tvshow SET c00 = '" + tvShowName + "', c08 = '" + showGenres +
                                        "', c14 ='" +
                                        TxtShowNetwork.Text + "' WHERE idShow = '" + TVShowLabel.Text + "'");
                //TODO: VisualStyleElement.Status.Text = "Updated " + TxtShowName.Text + " Successfully";

                if ((Settings.Default.KodiVersion >= (int)KodiVersion.Isengard))
                {
                    //Remove all genres from tv show
                    PseudoTvUtils.DbExecute("DELETE FROM genre_link WHERE media_id = '" + TVShowLabel.Text + "'");
                    //add each one.  one by one.
                    for (var x = 0; x <= ListTVGenres.Items.Count - 1; x++)
                    {
                        var genreId = LookUpGenre(ListTVGenres.Items[x].ToString());
                        PseudoTvUtils.DbExecute("INSERT INTO genre_link (genre_id, media_id, media_type) VALUES ('" +
                                                genreId + "', '" +
                                                TVShowLabel.Text + "', 'tvshow')");
                    }
                }
                else
                {
                    //Remove all genres from tv show
                    PseudoTvUtils.DbExecute("DELETE FROM genrelinktvshow  WHERE idShow = '" + TVShowLabel.Text + "'");

                    //add each one.  one by one.
                    for (var x = 0; x <= ListTVGenres.Items.Count - 1; x++)
                    {
                        var genreId = LookUpGenre(ListTVGenres.Items[x].ToString());
                        PseudoTvUtils.DbExecute("INSERT INTO genrelinktvshow (idGenre, idShow) VALUES ('" + genreId +
                                                "', '" +
                                                TVShowLabel.Text + "')");
                    }
                }

                //Now update the tv show table

                var savedName = TxtShowNetwork.Text;

                //Refresh Things
                RefreshNetworkList();
                RefreshGenres();

                //Reset the text
                //txtShowNetwork.Text = SavedName

                var returnindex = TVShowList.SelectedIndices[0];
                RefreshAll();
                TVShowList.Items[returnindex].Selected = true;
            }
        }

        public void RefreshTvGuideSublist(int listType, string listValue)
        {
            TVGuideSubMenu.Items.Clear();

            var tvChannelTypeValue = listValue;

            if (listType == (int)TVGuideListType.Playlist)
            {
                //Playlist

                //Add Info for PlayList editing/loading.

            }
            else if (listType == (int)TVGuideListType.TvNetwork)
            {
                //This is a TV Network.

                //Make sure there's a value in this box.
                if (!string.IsNullOrEmpty(tvChannelTypeValue))
                {
                    var channelPreview = new[] { 1 };

                    var returnArray = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                        "SELECT * FROM tvshow WHERE c14='" + tvChannelTypeValue + "'", channelPreview);

                    //Make sure the Array is not null.
                    if (returnArray != null)
                    {
                        for (var x = 0; x <= returnArray.Length - 1; x++)
                        {
                            //Add each item it returns to the list.
                            TVGuideSubMenu.Items.Add(returnArray[x]);
                        }
                    }
                }

            }
            else if (listType == (int)TVGuideListType.MovieStudio)
            {
                //Movie Studio

                //Make sure there's a value in this box.
                if (!string.IsNullOrEmpty(tvChannelTypeValue))
                {
                    var selectArray = new[] { 2 };


                    //Now, gather a list of all the show IDs that match the genreID
                    var returnArray = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                        "SELECT * FROM movie WHERE c18='" + tvChannelTypeValue + "'", selectArray);

                    //Now loop through each one individually.
                    if (returnArray != null)
                    {
                        for (var x = 0; x <= returnArray.Length - 1; x++)
                        {
                            //Now add that name to the list.
                            TVGuideSubMenu.Items.Add(returnArray[x]);
                        }
                    }
                }

            }
            else if (listType == (int)TVGuideListType.TvGenre)
            {
                //TV Genre

                //Make sure there's a value in this box.
                if (!string.IsNullOrEmpty(tvChannelTypeValue))
                {
                    var selectArray = new[] { 1 };


                    //Now, gather a list of all the show IDs that match the genreID
                    string[] returnArray = null;
                    if ((Settings.Default.KodiVersion >= (int)KodiVersion.Isengard))
                    {
                        returnArray = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                            "SELECT * FROM genre_link WHERE genre_id='" + LookUpGenre(tvChannelTypeValue) +
                            "' AND media_type = 'tvshow'", selectArray);
                    }
                    else
                    {
                        returnArray = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                            "SELECT * FROM genrelinktvshow WHERE idGenre='" + LookUpGenre(tvChannelTypeValue) + "'",
                            selectArray);
                    }


                    //Now loop through each one individually.
                    for (var x = 0; x <= returnArray.Length - 1; x++)
                    {
                        var showNameArray = new[] { 1 };

                        var returnArray2 = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                            "SELECT * FROM tvshow WHERE idShow='" + returnArray[x] + "'", showNameArray);

                        //Now add that name to the list.
                        TVGuideSubMenu.Items.Add(returnArray2[0]);
                    }
                }
            }
            else if (listType == (int)TVGuideListType.MovieGenre)
            {
                //Movie Genre

                var selectArrayMovies = new[] { 1 };

                string[] returnArrayMovies = null;
                if ((Settings.Default.KodiVersion >= (int)KodiVersion.Isengard))
                {
                    returnArrayMovies = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                        "SELECT * FROM genre_link WHERE genre_id='" + LookUpGenre(tvChannelTypeValue) +
                        "' AND media_type = 'movie'", selectArrayMovies);
                }
                else
                {
                    returnArrayMovies = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                        "SELECT * FROM genrelinkmovie WHERE idGenre='" + LookUpGenre(tvChannelTypeValue) + "'",
                        selectArrayMovies);
                }

                //Now loop through each one individually.
                if (returnArrayMovies == null)
                {
                }
                else
                {

                    for (var x = 0; x <= returnArrayMovies.Length - 1; x++)
                    {
                        var showArray = new[] { 2 };

                        var returnArray2 = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                            "SELECT * FROM movie WHERE idMovie='" + returnArrayMovies[x] + "'", showArray);

                        //Now add that name to the list.
                        TVGuideSubMenu.Items.Add(returnArray2[0]);
                    }
                }
            }
            else if (listType == (int)TVGuideListType.MixedGenre)
            {
                //Mixed Genre

                //Make sure there's a value in this box.
                if (!string.IsNullOrEmpty(tvChannelTypeValue))
                {
                    var selectArray = new[] { 1 };

                    //Now, gather a list of all the show IDs that match the genreID
                    string[] returnArray = null;
                    if ((Settings.Default.KodiVersion >= (int)KodiVersion.Isengard))
                    {
                        returnArray = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                            "SELECT * FROM genre_link WHERE genre_id='" + LookUpGenre(tvChannelTypeValue) +
                            "' AND media_type = 'tvshow'", selectArray);
                    }
                    else
                    {
                        returnArray = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                            "SELECT * FROM genrelinktvshow WHERE idGenre='" + LookUpGenre(tvChannelTypeValue) + "'",
                            selectArray);
                    }

                    //Now loop through each one individually.
                    if (returnArray == null)
                    {
                    }
                    else
                    {
                        for (var x = 0; x <= returnArray.Length - 1; x++)
                        {
                            var showArray = new[] { 1 };

                            var returnArray2 = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                                "SELECT * FROM tvshow WHERE idShow='" + returnArray[x] + "'", showArray);

                            //Now add that name to the list.
                            TVGuideSubMenu.Items.Add(returnArray2[0]);
                        }
                    }
                    //------------------------------------
                    //Repeat this step for the Movies now.

                    var selectArrayMovies = new[] { 1 };

                    string[] returnArrayMovies = null;
                    if ((Settings.Default.KodiVersion >= (int)KodiVersion.Isengard))
                    {
                        returnArrayMovies = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                            "SELECT * FROM genre_link WHERE genre_id='" + LookUpGenre(tvChannelTypeValue) +
                            "' AND media_type = 'movie'", selectArrayMovies);
                    }
                    else
                    {
                        returnArrayMovies = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                            "SELECT * FROM genrelinkmovie WHERE idGenre='" + LookUpGenre(tvChannelTypeValue) + "'",
                            selectArrayMovies);
                    }


                    //Now loop through each one individually.
                    //Verify it's not NULL.
                    if (returnArrayMovies == null)
                    {
                    }
                    else
                    {
                        for (var x = 0; x <= returnArrayMovies.Length - 1; x++)
                        {
                            var showArray = new[] { 2 };

                            var returnArray2 = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                                "SELECT * FROM movie WHERE idMovie='" + returnArrayMovies[x] + "'", showArray);

                            //Now add that name to the list.
                            TVGuideSubMenu.Items.Add(returnArray2[0]);
                        }
                    }


                }

            }
            else if (listType == (int)TVGuideListType.TvShow)
            {
                //TV Show
            }
            else if (listType == (int)TVGuideListType.Directory)
            {
                //Directory

            }

            //Now loop through all the shows listed to NOT show, compare them to the list of shows and make any of them have a red background if they match.
            for (var x = 0; x <= NotShows.Items.Count - 1; x++)
            {
                var notShow = NotShows.Items[x].ToString();

                for (var y = 0; y <= TVGuideSubMenu.Items.Count - 1; y++)
                {
                    if (!notShow.Equals(TVGuideSubMenu.Items[y].SubItems[0].Text))
                    {
                        TVGuideSubMenu.Items[y].BackColor = Color.Red;
                    }
                }

            }
            TVGuideSubMenu.Sort();
        }

        private void TVGuideList_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            // Get the new sorting column. 
            var newSortingColumn = TVGuideList.Columns[e.Column];
            // Figure out the new sorting order. 
            var sortOrder = default(System.Windows.Forms.SortOrder);
            if (_mSortingColumn == null)
            {
                // New column. Sort ascending. 
                sortOrder = SortOrder.Ascending;
                // See if this is the same column. 
            }
            else
            {
                if (newSortingColumn.Equals(_mSortingColumn))
                {
                    // Same column. Switch the sort order. 
                    if (_mSortingColumn.Text.StartsWith("> "))
                    {
                        sortOrder = SortOrder.Descending;
                    }
                    else
                    {
                        sortOrder = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New column. Sort ascending. 
                    sortOrder = SortOrder.Ascending;
                }
                // Remove the old sort indicator. 
                _mSortingColumn.Text = _mSortingColumn.Text.Substring(2);
            }
            // Display the new sort order. 
            _mSortingColumn = newSortingColumn;
            if (sortOrder == SortOrder.Ascending)
            {
                _mSortingColumn.Text = "> " + _mSortingColumn.Text;
            }
            else
            {
                _mSortingColumn.Text = "< " + _mSortingColumn.Text;
            }
            // Create a comparer. 
            TVGuideList.ListViewItemSorter = new ListViewSorter(e.Column, sortOrder);
            // Sort. 
            TVGuideList.Sort();
        }

        private void TVGuideList_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {

            if (TVGuideList.SelectedIndices.Count > 0)
            {
                //Reset the checked options.
                ChkLogo.Checked = false;
                chkDontPlayChannel.Checked = false;
                ChkRandom.Checked = false;
                ChkRealTime.Checked = false;
                ChkResume.Checked = false;
                ChkIceLibrary.Checked = false;
                ChkExcludeBCT.Checked = false;
                ChkPopup.Checked = false;
                ChkUnwatched.Checked = false;
                ChkWatched.Checked = false;
                ChkPause.Checked = false;
                ChkPlayInOrder.Checked = false;
                ResetDays.Clear();
                ChannelName.Clear();

                //Clear other form items.
                TVGuideSubMenu.Items.Clear();
                var playListNumber = Convert.ToInt32(TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[1].Text);
                Option2.Items.Clear();
                InterleavedList.Items.Clear();
                SchedulingList.Items.Clear();
                NotShows.Items.Clear();

                //Display the Channel Number.
                TVGuideShowName.Text = "Channel " + TVGuideList.SelectedItems[0].SubItems[0].Text;


                if (playListNumber != 9999)
                {
                    PlayListType.SelectedIndex = playListNumber;

                    var option1 = 0;

                    var tvChannelTypeValue = TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[2].Text;

                    if (playListNumber == 0)
                    {
                        //Playlist
                        option1 = 1;

                        //Add Info for PlayList editing/loading.

                    }
                    else if (playListNumber == 1)
                    {
                        //This is a TV Network.

                        for (var x = 0; x <= NetworkList.Items.Count - 1; x++)
                        {
                            Option2.Items.Add(NetworkList.Items[x].SubItems[0].Text);
                        }

                        //Make sure there's a value in this box.
                        if (!string.IsNullOrEmpty(tvChannelTypeValue))
                        {
                            RefreshTvGuideSublist(playListNumber, tvChannelTypeValue);
                        }

                    }
                    else if (playListNumber == 2)
                    {
                        //Movie Studio
                        RefreshAllStudios();

                    }
                    else if (playListNumber == 3)
                    {
                        //TV Genre
                        for (var x = 0; x <= GenresList.Items.Count - 1; x++)
                        {
                            Option2.Items.Add(GenresList.Items[x].SubItems[0].Text);
                        }

                    }
                    else if (playListNumber == 4)
                    {
                        //Movie Genre
                        RefreshAllGenres();

                    }
                    else if (playListNumber == 5)
                    {
                        //Mixed Genre
                        RefreshAllGenres();

                    }
                    else if (playListNumber == 6)
                    {
                        //TV Show
                        for (var x = 0; x <= TVShowList.Items.Count - 1; x++)
                        {
                            Option2.Items.Add(TVShowList.Items[x].SubItems[0].Text);
                        }
                    }
                    else if (playListNumber == 7)
                    {
                        //Directory
                        option1 = 1;
                    }
                    else if (playListNumber == 8)
                    {
                        //LiveTV
                        option1 = 2;
                    }
                    else if (playListNumber == 9)
                    {
                        //InternetTV
                        option1 = 3;
                    }
                    else if (playListNumber == 10 | playListNumber == 11)
                    {
                        //YoutubeTV or RSS
                        option1 = 4;
                    }
                    else if (playListNumber == 13)
                    {
                        //Music Videos
                        option1 = 5;
                    }
                    else if (playListNumber == 14)
                    {
                        //Extras
                        option1 = 6;
                    }
                    else if (playListNumber == 15)
                    {
                        //Direct Plugin Type
                        option1 = 7;
                    }

                    //Now, we loop through the advanced rules to populate those properly.

                    //break this array into all the rules for this channel.
                    var allRules = TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[4].Text.Split('~');

                    //Loop through all of them.
                    //But, only the ones it "says" it has.

                    var ruleCount = 0;


                    if (!string.IsNullOrEmpty(TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[5].Text))
                    {
                        ruleCount = Convert.ToInt32(TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[5].Text);
                    }


                    for (var y = 1; y <= ruleCount; y++)
                    {
                        //For y = 1 To UBound(AllRules)
                        var ruleSettings = allRules[y].Split('|');

                        //Rule #1, #2, etc.
                        var ruleNum = ruleSettings[0];
                        //Value, most of the time this is the only thing we need.
                        var ruleValue = ruleSettings[1];


                        switch (ruleValue)
                        {
                            case "5":
                                chkDontPlayChannel.Checked = true;
                                break;
                            case "10":
                                ChkRandom.Checked = true;
                                break;
                            case "7":
                                ChkRealTime.Checked = true;
                                break;
                            case "9":
                                ChkResume.Checked = true;
                                break;
                            case "11":
                                ChkUnwatched.Checked = true;
                                break;
                            case "4":
                                ChkWatched.Checked = true;
                                break;
                            case "8":
                                ChkPause.Checked = true;
                                break;
                            case "12":
                                ChkPlayInOrder.Checked = true;
                                break;
                            default:
                                //Okay, so it's not something requiring a single option.
                                //Now loop through all the sub-options of each rule.
                                var subOptions = new string[5];

                                for (var z = 2; z <= ruleSettings.Length; z++)
                                {
                                    var optionNum = Convert.ToInt32(ruleSettings[z].Split('^')[0]);
                                    var optionValue = ruleSettings[z].Split('^')[1];


                                    switch (ruleValue)
                                    {
                                        case "13":
                                            //TODO: Optimize
                                            ResetDays.Text = (Convert.ToInt32(optionValue)/60).ToString();
                                            break;
                                        case "1":
                                            ChannelName.Text = optionValue;
                                            break;
                                        case "15":
                                            ChkLogo.Checked = optionValue == "Yes";
                                            break;
                                        case "14":
                                            ChkIceLibrary.Checked = optionValue == "Yes";
                                            break;
                                        case "17":
                                            ChkExcludeBCT.Checked = optionValue == "Yes";
                                            break;
                                        case "18":
                                            ChkPopup.Checked = optionValue == "Yes";
                                            break;
                                        case "2":
                                            NotShows.Items.Add(optionValue);
                                            break;
                                        case "6":
                                            //Add this option to a sub-item array to add later to the
                                            //Object at the end
                                            subOptions[optionNum - 1] = optionValue;

                                            if (optionNum != 5) continue;
                                        
                                            //last option.
                                            //create + insert object
                                            var interleavedItm = new ListViewItem(subOptions);
                                            //Add to list
                                            InterleavedList.Items.Add(interleavedItm);
                                            break;
                                        case "3":
                                            //Add this option to a sub-item array to add later to the
                                            //Object at the end
                                            subOptions[optionNum - 1] = optionValue;
                                            if (optionNum == 4)
                                            {
                                                //last option.
                                                //create + insert object
                                                var schedulingItm = new ListViewItem(subOptions);
                                                //Add to list

                                                SchedulingList.Items.Add(schedulingItm);
                                            }
                                            break;
                                    }
                                }
                                break;
                        }
                    }

                    RefreshTvGuideSublist(playListNumber, tvChannelTypeValue);

                    var index = 0;
                    switch (option1)
                    {
                        case 1:
                            PlayListLocation.Text = TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[2].Text;
                            //SortTypeBox.SelectedIndex = TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[8].Text
                            index =
                                MediaLimitBox.FindString(TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[7].Text);
                            MediaLimitBox.SelectedIndex = index;
                            break;
                        case 2:
                            PlayListLocation.Text = TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[2].Text;
                            StrmUrlBox.Text = TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[6].Text;
                            ShowTitleBox.Text = TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[7].Text;
                            break;
                        case 3:
                            PlayListLocation.Text = TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[2].Text;
                            StrmUrlBox.Text = TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[6].Text;
                            ShowTitleBox.Text = TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[7].Text;
                            ShowDescBox.Text = TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[8].Text;
                            break;
                        case 4:
                            PlayListLocation.Text = TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[2].Text;
                            YouTubeType.SelectedIndex =
                                Convert.ToInt32(TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[6].Text) - 1;
                            index =
                                MediaLimitBox.FindString(TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[7].Text);
                            MediaLimitBox.SelectedIndex = index;
                            SortTypeBox.SelectedIndex =
                                Convert.ToInt32(TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[8].Text);
                            if (YouTubeType.SelectedIndex == 6 | YouTubeType.SelectedIndex == 7)
                            {
                                var returnMulti = "";

                                returnMulti = TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[2].Text;

                                if (returnMulti.Contains("|"))
                                {
                                    var returnMultiSplit = returnMulti.Split('|');
                                    for (var x = 0; x <= returnMultiSplit.Length - 1; x++)
                                        NotShows.Items.Add(returnMultiSplit[x]);
                                }
                                else
                                {
                                    ShowDescBox.Visible = true;
                                    ShowDescBox.Text = returnMulti;
                                }

                                YouTubeMulti = TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[2].Text;
                            }

                            break;
                        case 5:
                            PlayListLocation.Text = TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[6].Text;
                            SubChannelType.SelectedIndex =
                                Convert.ToInt32(TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[6].Text) - 1;
                            index =
                                MediaLimitBox.FindString(TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[7].Text);
                            MediaLimitBox.SelectedIndex = index;
                            SortTypeBox.SelectedIndex =
                                Convert.ToInt32(TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[8].Text);
                            break;
                        case 7:
                            PlayListLocation.Text = TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[2].Text;
                            SortTypeBox.SelectedIndex =
                                Convert.ToInt32(TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[8].Text);
                            index =
                                MediaLimitBox.FindString(TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[7].Text);
                            MediaLimitBox.SelectedIndex = index;

                            var returnPlugin = "";

                            var selectArray = new[] { 0 };

                            returnPlugin = TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[2].Text;

                            var returnProperPlugin = returnPlugin.Remove(0, 9);

                            var returnArray = PseudoTvUtils.ReadPluginRecord(Settings.Default.AddonDatabaseLocation,
                                "SELECT name FROM addon WHERE addonID = '" + returnProperPlugin + "'", selectArray);

                            var index2 = 0;
                            index2 = PluginType.FindString(returnArray[0]);
                            PluginType.SelectedIndex = index2;

                            PluginNotInclude = TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[6].Text;

                            if (PluginNotInclude.Contains(","))
                            {
                                var pluginNotIncludeSplit = PluginNotInclude.Split(',');

                                for (var x = 0; x <= pluginNotIncludeSplit.Length; x++)
                                {
                                    NotShows.Items.Add(pluginNotIncludeSplit[x]);
                                }
                            }
                            break;
                        default:
                            Option2.Text = TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[2].Text;
                            break;
                    }
                    TVGuideSubMenu.Sort();
                }
            }
        }

        private void MnuSettings_Click(System.Object sender, System.EventArgs e)
        {
            SettingsWindow settings = new SettingsWindow(this);
            settings.Show();
        }

        private void DontShowToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            if (TVGuideSubMenu.SelectedItems.Count > 0)
            {
                NotShows.Items.Add(TVGuideSubMenu.Items[TVGuideSubMenu.SelectedIndices[0]].SubItems[0].Text);
                TVGuideSubMenu.Items[TVGuideSubMenu.SelectedIndices[0]].BackColor = Color.Red;
            }
        }

        private void ListMoviePosters_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var x = ListMoviePosters.SelectedIndex;

            MoviePicture.ImageLocation = ListMoviePosters.Items[x].ToString();
            MoviePicture.Refresh();

        }

        private void MoviePosterSelect_Click(System.Object sender, System.EventArgs e)
        {
            var x = ListMoviePosters.SelectedIndex;
            var type = "poster";
            var mediaType = "movie";


            if (MovieLocation.TextLength >= 6)
            {
                if (MovieLocation.Text.Substring(0, 6) == "smb://")
                {
                    MovieLocation.Text = MovieLocation.Text.Replace("/", "\\");
                    MovieLocation.Text = "\\\\" + MovieLocation.Text.Substring(6);
                }
            }

            var directoryName = "";
            directoryName = Path.GetDirectoryName(MovieLocation.Text);

            // Displays a SaveFileDialog so the user can save the Image
            // assigned to TVPosterSelect.
            var saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = directoryName;
            saveFileDialog1.Filter = "JPeg Image|*.jpg";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.FileName = "poster.jpg";
            saveFileDialog1.ShowDialog();

            var fileToSaveAs = System.IO.Path.Combine(saveFileDialog1.InitialDirectory, saveFileDialog1.FileName);
            MoviePicture.Image.Save(fileToSaveAs, System.Drawing.Imaging.ImageFormat.Jpeg);

            PseudoTvUtils.DbExecute("UPDATE art SET url = '" + ListMoviePosters.Items[x].ToString() +
                                    "' WHERE media_id = '" +
                                    MovieIDLabel.Text + "' and media_type = '" + mediaType + "' and type = '" + type +
                                    "'");
            //TODO: VisualStyleElement.Status.Text = "Updated " + MovieLabel.Text + " " + MovieIDLabel.Text +
            //                                " Successfully with " + ListMoviePosters.Items[x].ToString() + "";

        }

        private void MovieList_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {

        }

        private void MovieNetworkList_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            // Get the new sorting column. 
            var newSortingColumn = MovieNetworkList.Columns[e.Column];
            // Figure out the new sorting order. 
            var sortOrder = default(System.Windows.Forms.SortOrder);
            if (_mSortingColumn == null)
            {
                // New column. Sort ascending. 
                sortOrder = SortOrder.Ascending;
                // See if this is the same column. 
            }
            else
            {
                if (newSortingColumn.Equals(_mSortingColumn))
                {
                    // Same column. Switch the sort order. 
                    if (_mSortingColumn.Text.StartsWith("> "))
                    {
                        sortOrder = SortOrder.Descending;
                    }
                    else
                    {
                        sortOrder = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New column. Sort ascending. 
                    sortOrder = SortOrder.Ascending;
                }
                // Remove the old sort indicator. 
                _mSortingColumn.Text = _mSortingColumn.Text.Substring(2);
            }
            // Display the new sort order. 
            _mSortingColumn = newSortingColumn;
            if (sortOrder == SortOrder.Ascending)
            {
                _mSortingColumn.Text = "> " + _mSortingColumn.Text;
            }
            else
            {
                _mSortingColumn.Text = "< " + _mSortingColumn.Text;
            }
            // Create a comparer. 
            MovieNetworkList.ListViewItemSorter = new ListViewSorter(e.Column, sortOrder);
            // Sort. 
            MovieNetworkList.Sort();
        }

        private void MovieNetworkList_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            MovieNetworkListSubList.Items.Clear();


            if (MovieNetworkList.SelectedIndices.Count > 0)
            {
                var selectArray = new[] { 2 };


                var returnArray = PseudoTvUtils.DbReadRecord(Settings.Default.VideoDatabaseLocation,
                    "SELECT * FROM movie WHERE c18='" +
                    MovieNetworkList.Items[MovieNetworkList.SelectedIndices[0]].SubItems[0].Text + "'", selectArray);

                for (var x = 0; x <= returnArray.Length - 1; x++)
                {
                    MovieNetworkListSubList.Items.Add(returnArray[x]);
                }

            }
        }

        private void BtnRefreshGenres_Click(System.Object sender, System.EventArgs e)
        {
            LookUpGenre("aaccc");
        }

        private void YouTubeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (YouTubeType.SelectedIndex == 0)
            {
                Label6.Text = "Channel/User:";
                var with32 = PlayListLocation;
                with32.Location = new System.Drawing.Point(270, 120);
                PlayListLocation.Visible = true;
                NotShows.Visible = false;
                AddExcludeBtn.Visible = false;
                DelExcludeBtn.Visible = false;
                Label12.Visible = false;
            }
            else if (YouTubeType.SelectedIndex == 1)
            {
                Label6.Text = "Playlist:";
                var with33 = PlayListLocation;
                with33.Location = new System.Drawing.Point(220, 120);
                PlayListLocation.Visible = true;
                NotShows.Visible = false;
                AddExcludeBtn.Visible = false;
                DelExcludeBtn.Visible = false;
                Label12.Visible = false;
            }
            else if (YouTubeType.SelectedIndex == 2 | YouTubeType.SelectedIndex == 3)
            {
                Label6.Text = "Username:";
                var with34 = PlayListLocation;
                with34.Location = new System.Drawing.Point(245, 120);
                PlayListLocation.Visible = true;
                NotShows.Visible = false;
                AddExcludeBtn.Visible = false;
                DelExcludeBtn.Visible = false;
                Label12.Visible = false;
            }
            else if (YouTubeType.SelectedIndex == 4)
            {
                Label6.Text = "Search String:";
                var with35 = PlayListLocation;
                with35.Location = new System.Drawing.Point(270, 120);
                PlayListLocation.Visible = true;
                NotShows.Visible = false;
                AddExcludeBtn.Visible = false;
                DelExcludeBtn.Visible = false;
                Label12.Visible = false;
            }
            else if (YouTubeType.SelectedIndex == 6)
            {
                Label6.Text = "";
                var with36 = PlayListLocation;
                with36.Location = new System.Drawing.Point(295, 120);
                PlayListLocation.Visible = false;
                NotShows.Visible = true;
                AddExcludeBtn.Visible = true;
                DelExcludeBtn.Visible = true;
                Label12.Text = "Add/Remove Playlists";
                Label12.Visible = true;
            }
            else if (YouTubeType.SelectedIndex == 7)
            {
                Label6.Text = "";
                var with37 = PlayListLocation;
                with37.Location = new System.Drawing.Point(295, 120);
                PlayListLocation.Visible = false;
                NotShows.Visible = true;
                AddExcludeBtn.Visible = true;
                DelExcludeBtn.Visible = true;
                Label12.Text = "Add/Remove Channels";
                Label12.Visible = true;
            }
            else if (YouTubeType.SelectedIndex == 8)
            {
                Label6.Text = "GData Url:";
                var with38 = PlayListLocation;
                with38.Location = new System.Drawing.Point(245, 120);
                PlayListLocation.Visible = true;
                NotShows.Visible = false;
                AddExcludeBtn.Visible = false;
                DelExcludeBtn.Visible = false;
                Label12.Visible = false;
                GDataDemoLink.Links.Remove(GDataDemoLink.Links[0]);
                //TODO: Check this logic... it wants an int as the second param but in vb they were giving a bool?
                GDataDemoLink.Links.Add(0, GDataDemoLink.Text == "GDataDemo" ? 1 : 0,
                    "http://gdata.youtube.com/demo/index.html");
                GDataDemoLink.Visible = true;
            }
            else
            {
                Label6.Text = "Nothing Here";
                PlayListLocation.Visible = false;
                NotShows.Visible = false;
                AddExcludeBtn.Visible = false;
                DelExcludeBtn.Visible = false;
                Label12.Visible = false;
            }

        }

        private void SubChannelType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PlayListType.SelectedIndex == 13 & SubChannelType.SelectedIndex == 0)
            {
                Label6.Text = "LastFM User:";
                var with39 = PlayListLocation;
                with39.Location = new System.Drawing.Point(270, 120);
            }
            else if (PlayListType.SelectedIndex == 13 & SubChannelType.SelectedIndex == 1)
            {
                Label6.Text = "Channel_##:";
                var with40 = PlayListLocation;
                with40.Location = new System.Drawing.Point(295, 120);
            }
        }

        private void AddBanner_Click(object sender, EventArgs e)
        {
            //TODO:
            //Form9.Visible = true;
            //Form9.Focus();
            //Form9.AddBannerPictureBox.ImageLocation =
            //    "http://github.com/Lunatixz/script.pseudotv.live/raw/development/resources/images/banner.png";
        }

        private void AddPoster_Click(object sender, EventArgs e)
        {
            //TODO:
            //Form10.Visible = true;
            //Form10.Focus();
            //Form10.AddPosterPictureBox.ImageLocation =
            //    "http://github.com/Lunatixz/script.pseudotv.live/raw/development/resources/images/poster.png";
        }

        private void AddMoviePosterButton_Click(object sender, EventArgs e)
        {
            //TODO:
            //Form11.Visible = true;
            //Form11.Focus();
            //Form11.AddMoviePosterPictureBox.ImageLocation =
            //    "http://github.com/Lunatixz/script.pseudotv.live/raw/development/resources/images/poster.png";
        }

        private void GDataDemoLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var sInfo = new ProcessStartInfo(e.Link.LinkData.ToString());
            Process.Start(sInfo);
        }

        private void AddExcludeBtn_Click(object sender, EventArgs e)
        {

            if (PlayListType.SelectedIndex == 10)
            {
                var response = PseudoTvUtils.ShowInputDialog("Enter Playlist or User String").Input;

                if (!string.IsNullOrEmpty(response))
                {
                    NotShows.Items.Add(response);
                }

                if (string.IsNullOrEmpty(YouTubeMulti))
                {
                    YouTubeMulti = response;
                }
                else
                {
                    YouTubeMulti = YouTubeMulti + "|" + response;
                }
            }
            else
            {
                var response = PseudoTvUtils.ShowInputDialog("Enter Exclude String").Input;

                if (!string.IsNullOrEmpty(response))
                {
                    NotShows.Items.Add(response);
                }

                if (string.IsNullOrEmpty(PluginNotInclude))
                {
                    PluginNotInclude = response;
                }
                else
                {
                    PluginNotInclude = PluginNotInclude + "," + response;
                }
            }

        }

        private void DelExclutn_Click(object sender, EventArgs e)
        {
            if (PlayListType.SelectedIndex == 10)
            {
                if (NotShows.SelectedItems.Count > 0)
                {
                    NotShows.Items.RemoveAt(NotShows.SelectedIndex);
                }

                var items = NotShows.Items.OfType<object>().Select(item => item.ToString()).ToArray();
                var result = string.Join("|", items);

                YouTubeMulti = result;
            }
            else
            {
                if (NotShows.SelectedItems.Count > 0)
                {
                    NotShows.Items.RemoveAt(NotShows.SelectedIndex);
                }

                var items = NotShows.Items.OfType<object>().Select(item => item.ToString()).ToArray();
                var result = string.Join(",", items);

                PluginNotInclude = result;
            }
        }

        private void PlayListType_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            //Clear the Sub-menu
            TVGuideSubMenu.Items.Clear();
            var with1 = PlayListLocation;
            with1.Location = new System.Drawing.Point(267, 120);
            PlayListLocation.Text = "";
            Label6.Visible = false;
            Label6.Text = "";
            Button5.Visible = false;
            PlayListLocation.Visible = false;
            PluginType.Visible = false;
            StrmUrl.Visible = false;
            StrmUrlBox.Visible = false;
            Option2.Visible = false;
            YouTubeType.Visible = false;
            ShowTitle.Visible = false;
            ShowTitleBox.Visible = false;
            ShowDesc.Visible = false;
            ShowDescBox.Visible = false;
            MediaLimit.Visible = false;
            MediaLimitBox.Visible = false;
            SortType.Visible = false;
            SortTypeBox.Visible = false;
            SubChannelType.Visible = false;
            TVGuideSubMenu.Visible = false;
            InterleavedList.Visible = false;
            Label7.Visible = false;
            SchedulingList.Visible = false;
            Label11.Visible = false;
            NotShows.Visible = false;
            Label12.Visible = false;
            AddExcludeBtn.Visible = false;
            DelExcludeBtn.Visible = false;
            PluginNotInclude = "";

            if (PlayListType.SelectedIndex == 0)
            {
                Button5.Visible = true;
                Label6.Text = "Location:";
                Label6.Visible = true;
                PlayListLocation.Visible = true;
            }
            else if (PlayListType.SelectedIndex == 1 | PlayListType.SelectedIndex == 2 | PlayListType.SelectedIndex == 3 |
                     PlayListType.SelectedIndex == 4 | PlayListType.SelectedIndex == 5 | PlayListType.SelectedIndex == 6)
            {
                Option2.Visible = true;
                TVGuideSubMenu.Visible = true;
                InterleavedList.Visible = true;
                Label7.Visible = true;
                SchedulingList.Visible = true;
                Label11.Visible = true;
                NotShows.Visible = true;
                Label12.Visible = true;
                Button8.Visible = true;
                Button9.Visible = true;
                Button10.Visible = true;
                Button11.Visible = true;
                Button12.Visible = true;
            }
            else if (PlayListType.SelectedIndex == 7)
            {
                Label6.Text = "Location:";
                Label6.Visible = true;
                PlayListLocation.Visible = true;
                var with2 = PlayListLocation;
                with2.Location = new System.Drawing.Point(270, 120);
                PlayListLocation.Visible = true;
                var with3 = MediaLimit;
                with3.Location = new System.Drawing.Point(160, 160);
                MediaLimit.Visible = true;
                var with4 = MediaLimitBox;
                with4.Location = new System.Drawing.Point(162, 180);
                MediaLimitBox.SelectedIndex = 0;
                MediaLimitBox.Visible = true;
                var with5 = SortType;
                with5.Location = new System.Drawing.Point(225, 160);
                SortType.Visible = true;
                var with6 = SortTypeBox;
                with6.Location = new System.Drawing.Point(227, 180);
                SortTypeBox.SelectedIndex = 0;
                SortTypeBox.Visible = true;
                NotShows.Visible = true;
                NotShows.Visible = false;
                AddExcludeBtn.Visible = false;
                DelExcludeBtn.Visible = false;
                Label12.Visible = false;
                Button5.Visible = true;
            }
            else if (PlayListType.SelectedIndex == 8)
            {
                Label6.Text = "Channel id:";
                Label6.Visible = true;
                PlayListLocation.Visible = true;
                StrmUrl.Visible = true;
                StrmUrlBox.Visible = true;
                ShowTitle.Text = "XMLTV Filename:";
                ShowTitle.Visible = true;
                ShowTitleBox.Visible = true;
            }
            else if (PlayListType.SelectedIndex == 9)
            {
                Label6.Text = "Duration:";
                Label6.Visible = true;
                PlayListLocation.Visible = true;
                StrmUrl.Text = "Source path:";
                StrmUrl.Visible = true;
                StrmUrlBox.Visible = true;
            }
            else if (PlayListType.SelectedIndex == 10)
            {
                NotShows.Items.Clear();
                Button5.Visible = false;
                YouTubeType.SelectedIndex = 0;
                YouTubeType.Visible = true;
                Label6.Text = "Channel/User:";
                Label6.Visible = true;
                var with7 = PlayListLocation;
                with7.Location = new System.Drawing.Point(270, 120);
                PlayListLocation.Visible = true;
                var with8 = MediaLimit;
                with8.Location = new System.Drawing.Point(160, 160);
                MediaLimit.Visible = true;
                var with9 = MediaLimitBox;
                with9.Location = new System.Drawing.Point(162, 180);
                MediaLimitBox.SelectedIndex = 0;
                MediaLimitBox.Visible = true;
                var with10 = SortType;
                with10.Location = new System.Drawing.Point(225, 160);
                SortType.Visible = true;
                var with11 = SortTypeBox;
                with11.Location = new System.Drawing.Point(227, 180);
                SortTypeBox.SelectedIndex = 0;
                SortTypeBox.Visible = true;
                NotShows.Visible = true;
                NotShows.Visible = false;
                AddExcludeBtn.Visible = false;
                DelExcludeBtn.Visible = false;
                Label12.Visible = false;
            }
            else if (PlayListType.SelectedIndex == 11)
            {
                Label6.Text = "Source path:";
                Label6.Visible = true;
                PlayListLocation.Visible = true;
                var with12 = MediaLimit;
                with12.Location = new System.Drawing.Point(160, 160);
                MediaLimit.Visible = true;
                var with13 = MediaLimitBox;
                with13.Location = new System.Drawing.Point(162, 180);
                MediaLimitBox.SelectedIndex = 0;
                MediaLimitBox.Visible = true;
                var with14 = SortType;
                with14.Location = new System.Drawing.Point(225, 160);
                SortType.Visible = true;
                var with15 = SortTypeBox;
                with15.Location = new System.Drawing.Point(227, 180);
                SortTypeBox.SelectedIndex = 0;
                SortTypeBox.Visible = true;
                Button8.Visible = false;
                Button9.Visible = false;
                Button10.Visible = false;
                Button11.Visible = false;
                Button12.Visible = false;
            }
            else if (PlayListType.SelectedIndex == 13)
            {
                Label6.Text = "LastFM Username:";
                Label6.Visible = true;
                PlayListLocation.Visible = true;
                var with16 = MediaLimit;
                with16.Location = new System.Drawing.Point(160, 160);
                MediaLimit.Visible = true;
                var with17 = MediaLimitBox;
                with17.Location = new System.Drawing.Point(162, 180);
                MediaLimitBox.SelectedIndex = 0;
                MediaLimitBox.Visible = true;
                var with18 = SortType;
                with18.Location = new System.Drawing.Point(225, 160);
                SortType.Visible = true;
                var with19 = SortTypeBox;
                with19.Location = new System.Drawing.Point(227, 180);
                SortTypeBox.SelectedIndex = 0;
                SortTypeBox.Visible = true;
                SubChannelType.Items.Clear();
                var with20 = SubChannelType.Items;
                with20.Add("LastFM");
                with20.Add("MyMusicTV");
                SubChannelType.Visible = true;
                SubChannelType.SelectedIndex = 0;
            }
            else if (PlayListType.SelectedIndex == 14)
            {
                Label6.Visible = false;
                var with21 = MediaLimit;
                with21.Location = new System.Drawing.Point(160, 160);
                MediaLimit.Visible = true;
                var with22 = MediaLimitBox;
                with22.Location = new System.Drawing.Point(162, 180);
                MediaLimitBox.SelectedIndex = 0;
                MediaLimitBox.Visible = true;
                var with23 = SortType;
                with23.Location = new System.Drawing.Point(225, 160);
                SortType.Visible = true;
                var with24 = SortTypeBox;
                with24.Location = new System.Drawing.Point(227, 180);
                SortTypeBox.SelectedIndex = 0;
                SortTypeBox.Visible = true;
                SubChannelType.Items.Clear();
                var with25 = SubChannelType.Items;
                with25.Add("popcorn");
                with25.Add("cinema");
                SubChannelType.Visible = true;
                SubChannelType.SelectedIndex = 0;
            }
            else if (PlayListType.SelectedIndex == 15)
            {
                NotShows.Items.Clear();
                Label6.Visible = false;
                var with26 = PlayListLocation;
                with26.Location = new System.Drawing.Point(220, 120);
                PlayListLocation.Visible = true;
                var with27 = PluginType;
                with27.Location = new System.Drawing.Point(227, 86);
                PluginType.Visible = true;
                var with28 = MediaLimit;
                with28.Location = new System.Drawing.Point(160, 160);
                MediaLimit.Visible = true;
                var with29 = MediaLimitBox;
                with29.Location = new System.Drawing.Point(162, 180);
                MediaLimitBox.SelectedIndex = 0;
                MediaLimitBox.Visible = true;
                var with30 = SortType;
                with30.Location = new System.Drawing.Point(225, 160);
                SortType.Visible = true;
                var with31 = SortTypeBox;
                with31.Location = new System.Drawing.Point(227, 180);
                SortTypeBox.SelectedIndex = 0;
                SortTypeBox.Visible = true;
                NotShows.Visible = true;
                Label12.Text = "Do not include these items";
                Label12.Visible = true;
                AddExcludeBtn.Visible = true;
                DelExcludeBtn.Visible = true;
                Button8.Visible = false;
                Button9.Visible = false;
                Button10.Visible = false;
                Button11.Visible = false;
                Button12.Visible = false;

            }
            else
            {
                Option2.Visible = true;
                PlayListLocation.Visible = false;

            }

            Option2.Items.Clear();
            Option2.Text = "";


            if (PlayListType.SelectedIndex == 0)
            {
                for (var x = 0; x <= NetworkList.Items.Count - 1; x++)
                {
                    Option2.Items.Add(NetworkList.Items[x].SubItems[0].Text);
                }
            }
            else if (PlayListType.SelectedIndex == 1)
            {
                for (var x = 0; x <= NetworkList.Items.Count - 1; x++)
                {
                    Option2.Items.Add(NetworkList.Items[x].SubItems[0].Text);
                }
            }
            else if (PlayListType.SelectedIndex == 2)
            {
                RefreshAllStudios();
            }
            else if (PlayListType.SelectedIndex == 3)
            {
                for (var x = 0; x <= GenresList.Items.Count - 1; x++)
                {
                    Option2.Items.Add(GenresList.Items[x].SubItems[0].Text);
                }
            }
            else if (PlayListType.SelectedIndex == 4)
            {
                RefreshAllGenres();
            }
            else if (PlayListType.SelectedIndex == 5)
            {
                RefreshAllGenres();
            }
            else if (PlayListType.SelectedIndex == 6)
            {
                for (var x = 0; x <= TVShowList.Items.Count - 1; x++)
                {
                    Option2.Items.Add(TVShowList.Items[x].SubItems[0].Text);
                }
            }

        }

        private void RefreshButton_Click(System.Object sender, System.EventArgs e)
        {
            //Loop through config file.
            //Grab all comments MINUS the ones for selected #
            //Append this & our new content to the file.


            if (TVGuideList.SelectedItems.Count > 0)
            {
                var fileName = Settings.Default.PseudoTvSettingsLocation;
                var textFile = "";

                var channelNum = TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[0].Text;


                //Loop through config file.
                //Grab all comments MINUS the ones for selected #

                if (System.IO.File.Exists(fileName) == true)
                {
                    var objReader = new System.IO.StreamReader(fileName);


                    while (objReader.Peek() != -1)
                    {
                        var singleLine = objReader.ReadLine();

                        if (!singleLine.Contains("<setting id=" + (char)34 + "Channel_" + channelNum + "_") &&
                            !singleLine.Contains("</settings"))
                        {
                            textFile = textFile + singleLine + Environment.NewLine;
                        }

                    }

                    objReader.Close();

                }
                else
                {
                    MessageBox.Show("File Does Not Exist");

                }

                var returnindex = TVGuideList.SelectedIndices[0];
                RefreshTvGuide();
                TVGuideList.Items[returnindex].Selected = true;

            }
        }

        private void BtnTvShowLocationBrowse_Click(System.Object sender, System.EventArgs e)
        {
            if (TVShowList.SelectedItems.Count > 0)
            {
                //RefreshAllStudios()
                //TODO:
                //Form3.Visible = true;
                //Form3.Focus();
            }
        }
        #endregion
        public object ConvertGenres(ListBox genrelist)
        {
            //Converts the existing ListTVGenre's contents to the proper format.

            var tvGenresString = "";
            for (var x = 0; x <= genrelist.Items.Count - 1; x++)
            {
                if (x == 0)
                {
                    tvGenresString = genrelist.Items[x].ToString();
                }
                else
                {
                    tvGenresString = tvGenresString + " / " + genrelist.Items[x].ToString();
                }
            }

            return tvGenresString;
        }

        private void Option2_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            if (PlayListType.SelectedIndex >= 0 & !string.IsNullOrEmpty(Option2.Text))
            {
                RefreshTvGuideSublist(PlayListType.SelectedIndex, Option2.Text);
            }

        }

        private void Button5_Click(System.Object sender, System.EventArgs e)
        {

            if (PlayListType.Text == "Directory")
            {
                FolderBrowserDialog1.ShowDialog();
                PlayListLocation.Text = FolderBrowserDialog1.SelectedPath;
            }
            else if (PlayListType.Text == "Playlist")
            {
                OpenFileDialog1.ShowDialog();

                var filename = OpenFileDialog1.FileName;
                var filenameSplit = filename.Split('\\');

                PlayListLocation.Text = "special://profile/playlists/video/" +
                                        filenameSplit[filenameSplit.Length];
            }
        }

        private void BtnTvGuideSave_Click(System.Object sender, System.EventArgs e)
        {
            //Loop through config file.
            //Grab all comments MINUS the ones for selected #
            //Append this & our new content to the file.

            SaveExcludeBtn.Visible = false;


            if (TVGuideList.SelectedItems.Count > 0)
            {
                var fileName = Settings.Default.PseudoTvSettingsLocation;
                var textFile = "";

                var channelNum = TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[0].Text;


                //Loop through config file.
                //Grab all comments MINUS the ones for selected #

                if (System.IO.File.Exists(fileName) == true)
                {
                    var objReader = new System.IO.StreamReader(fileName);


                    while (objReader.Peek() != -1)
                    {
                        var singleLine = objReader.ReadLine();

                        if (!singleLine.Contains("<setting id=" + (char)34 + "Channel_" + channelNum + "_") &&
                            !singleLine.Contains("</settings"))
                        {
                            textFile = textFile + singleLine + System.Environment.NewLine;
                        }

                    }

                    objReader.Close();

                }
                else MessageBox.Show("File Does Not Exist");

                //Now, append info for this channel we're editing.

                var appendInfo = "";
                var rulecount = 0;

                //Show the Logo is checked.
                //<setting id="Channel_1_rule_1_id" value="15" />
                if (ChkLogo.Checked == true)
                {
                    rulecount = rulecount + 1;
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_id" + (char)34 + " value=" +
                                 (char)34 + "15" + (char)34 + " />";
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_opt_1" + (char)34 + " value=" +
                                 (char)34 + "Yes" + (char)34 + " />";
                }

                //Don't show this channel is checked
                //<setting id="Channel_1_rule_1_id" value="5" />
                if (chkDontPlayChannel.Checked == true)
                {
                    rulecount = rulecount + 1;
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_id" + (char)34 + " value=" +
                                 (char)34 + "5" + (char)34 + " />";
                }

                //Play Random Mode
                //<setting id="Channel_1_rule_1_id" value="10" />
                if (ChkRandom.Checked == true)
                {
                    rulecount = rulecount + 1;
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_id" + (char)34 + " value=" +
                                 (char)34 + "10" + (char)34 + " />";
                }

                //Play Real-Time Mode
                //<setting id="Channel_1_rule_1_id" value="7" />
                if (ChkRealTime.Checked == true)
                {
                    rulecount = rulecount + 1;
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_id" + (char)34 + " value=" +
                                 (char)34 + "7" + (char)34 + " />";
                }

                //Play Resume Mode
                //<setting id="Channel_1_rule_1_id" value="9" />
                if (ChkResume.Checked == true)
                {
                    rulecount = rulecount + 1;
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_id" + (char)34 + " value=" +
                                 (char)34 + "9" + (char)34 + " />";
                }

                //Play Only Unwatched Films
                //<setting id="Channel_1_rule_1_id" value="11" />
                if (ChkUnwatched.Checked == true)
                {
                    rulecount = rulecount + 1;
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_id" + (char)34 + " value=" +
                                 (char)34 + "11" + (char)34 + " />";
                }

                //Only play Watched
                //<setting id="Channel_1_rule_1_id" value="4" />
                if (ChkWatched.Checked == true)
                {
                    rulecount = rulecount + 1;
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_id" + (char)34 + " value=" +
                                 (char)34 + "4" + (char)34 + " />";
                }

                //Exclude Strms?
                //<setting id="Channel_1_rule_1_id" value="14" />
                if (ChkIceLibrary.Checked == true)
                {
                    rulecount = rulecount + 1;
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_id" + (char)34 + " value=" +
                                 (char)34 + "14" + (char)34 + " />";
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_opt_1" + (char)34 + " value=" +
                                 (char)34 + "No" + (char)34 + " />";
                }

                //Exclude BCT?
                //<setting id="Channel_1_rule_1_id" value="17" />
                if (ChkExcludeBCT.Checked == true)
                {
                    rulecount = rulecount + 1;
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_id" + (char)34 + " value=" +
                                 (char)34 + "17" + (char)34 + " />";
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_opt_1" + (char)34 + " value=" +
                                 (char)34 + "No" + (char)34 + " />";
                }

                //Disable Popup?
                //<setting id="Channel_1_rule_1_id" value="18" />
                if (ChkPopup.Checked == true)
                {
                    rulecount = rulecount + 1;
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_id" + (char)34 + " value=" +
                                 (char)34 + "18" + (char)34 + " />";
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_opt_1" + (char)34 + " value=" +
                                 (char)34 + "No" + (char)34 + " />";
                }

                //Pause when not watching
                //<setting id="Channel_1_rule_1_id" value="8" />
                if (ChkPause.Checked == true)
                {
                    rulecount = rulecount + 1;
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_id" + (char)34 + " value=" +
                                 (char)34 + "8" + (char)34 + " />";
                }

                //Play shows in order
                //<setting id="Channel_1_rule_1_id" value="12" />
                if (ChkPlayInOrder.Checked == true)
                {
                    rulecount = rulecount + 1;
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_id" + (char)34 + " value=" +
                                 (char)34 + "12" + (char)34 + " />";
                }

                //Theres a # in the reset day amount
                //<setting id="Channel_1_rule_1_id" value="13" />
                //<setting id="Channel_1_rule_1_opt_1" value=ResetDays />

                if (PseudoTvUtils.IsNumeric(ResetDays.Text))
                {
                    _resetHours = (Convert.ToInt32(ResetDays.Text) * 60);
                    rulecount = rulecount + 1;
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_id" + (char)34 + " value=" +
                                 (char)34 + "13" + (char)34 + " />";
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_opt_1" + (char)34 + " value=" +
                                 (char)34 + _resetHours + (char)34 + " />";
                }

                //Theres a channel name
                //<setting id="Channel_1_rule_1_id" value="1" />
                //<setting id="Channel_1_rule_1_opt_1" value=ChannelName />
                if (!string.IsNullOrEmpty(ChannelName.Text))
                {
                    rulecount = rulecount + 1;
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_id" + (char)34 + " value=" +
                                 (char)34 + "1" + (char)34 + " />";
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_opt_1" + (char)34 + " value=" +
                                 (char)34 + ChannelName.Text + (char)34 + " />";
                }

                //Loop through shows not to play
                //<setting id="Channel_1_rule_1_id" value="2" />
                //<setting id="Channel_1_rule_1_opt_1" value=ShowName />
                if (PlayListType.SelectedIndex == 15 | PlayListType.SelectedIndex == 10)
                {
                }
                else
                {
                    for (var x = 0; x <= NotShows.Items.Count - 1; x++)
                    {
                        rulecount = rulecount + 1;
                        appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                     "Channel_" + channelNum + "_rule_" + rulecount + "_id" + (char)34 +
                                     " value=" + (char)34 + "2" + (char)34 + " />";
                        appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                     "Channel_" + channelNum + "_rule_" + rulecount + "_opt_1" + (char)34 +
                                     " value=" + (char)34 + NotShows.Items[x] + (char)34 + " />";
                    }
                }
                //Interleaved loop
                for (var x = 0; x <= InterleavedList.Items.Count - 1; x++)
                {
                    rulecount = rulecount + 1;
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_id" + (char)34 + " value=" +
                                 (char)34 + "6" + (char)34 + " />";
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_opt_1" + (char)34 + " value=" +
                                 (char)34 + InterleavedList.Items[x].SubItems[0].Text + (char)34 + " />";
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_opt_2" + (char)34 + " value=" +
                                 (char)34 + InterleavedList.Items[x].SubItems[1].Text + (char)34 + " />";
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_opt_3" + (char)34 + " value=" +
                                 (char)34 + InterleavedList.Items[x].SubItems[2].Text + (char)34 + " />";
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_opt_4" + (char)34 + " value=" +
                                 (char)34 + InterleavedList.Items[x].SubItems[3].Text + (char)34 + " />";
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_opt_5" + (char)34 + " value=" +
                                 (char)34 + InterleavedList.Items[x].SubItems[4].Text + (char)34 + " />";
                }

                for (var x = 0; x <= SchedulingList.Items.Count - 1; x++)
                {
                    rulecount = rulecount + 1;
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_id" + (char)34 + " value=" +
                                 (char)34 + "3" + (char)34 + " />";
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_opt_1" + (char)34 + " value=" +
                                 (char)34 + SchedulingList.Items[x].SubItems[0].Text + (char)34 + " />";
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_opt_2" + (char)34 + " value=" +
                                 (char)34 + SchedulingList.Items[x].SubItems[1].Text + (char)34 + " />";
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_opt_3" + (char)34 + " value=" +
                                 (char)34 + SchedulingList.Items[x].SubItems[2].Text + (char)34 + " />";
                    appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                 "Channel_" + channelNum + "_rule_" + rulecount + "_opt_4" + (char)34 + " value=" +
                                 (char)34 + SchedulingList.Items[x].SubItems[3].Text + (char)34 + " />";
                }

                //Update it has been changed to flag it?
                //<setting id="Channel_1_changed" value="True" />
                appendInfo = appendInfo + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                             "Channel_" + channelNum + "_changed" + (char)34 + " value=" + (char)34 +
                             "True" + (char)34 + " />";

                //Add type of channel to the top.
                var topAppend = "\t" + "<setting id=" + (char)34 + "Channel_" + channelNum + "_type" +
                                   (char)34 + " value=" + (char)34 + PlayListType.SelectedIndex + (char)34 +
                                   " />";

                if (PlayListType.SelectedIndex == 0)
                {
                    topAppend += Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                "Channel_" + channelNum + "_1" + (char)34 + " value=" + (char)34 +
                                PlayListLocation.Text + (char)34 + " />";
                }
                else if (PlayListType.SelectedIndex == 7)
                {
                    topAppend += Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                "Channel_" + channelNum + "_1" + (char)34 + " value=" + (char)34 +
                                PlayListLocation.Text + (char)34 + " />" + System.Environment.NewLine + "\t" +
                                "<setting id=" + (char)34 + "Channel_" + channelNum + "_3" + (char)34 +
                                " value=" + (char)34 + MediaLimitBox.Text + (char)34 + " />" +
                                System.Environment.NewLine + "\t" + "<setting id=" + (char)34 + "Channel_" +
                                channelNum + "_4" + (char)34 + " value=" + (char)34 +
                                SortTypeBox.SelectedIndex + (char)34 + " />";
                }
                else if (PlayListType.SelectedIndex == 8 | PlayListType.SelectedIndex == 9)
                {
                    topAppend += System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                "Channel_" + channelNum + "_1" + (char)34 + " value=" + (char)34 +
                                PlayListLocation.Text + (char)34 + " />" + System.Environment.NewLine + "\t" +
                                "<setting id=" + (char)34 + "Channel_" + channelNum + "_2" + (char)34 +
                                " value=" + (char)34 + StrmUrlBox.Text + (char)34 + " />" +
                                System.Environment.NewLine + "\t" + "<setting id=" + (char)34 + "Channel_" +
                                channelNum + "_3" + (char)34 + " value=" + (char)34 + ShowTitleBox.Text +
                                (char)34 + " />" + System.Environment.NewLine + "\t" + "<setting id=" +
                                (char)34 + "Channel_" + channelNum + "_4" + (char)34 + " value=" +
                                (char)34 + ShowDescBox.Text + (char)34 + " />";
                }
                else if (PlayListType.SelectedIndex == 10)
                {
                    if (YouTubeType.SelectedIndex == 6 | YouTubeType.SelectedIndex == 7)
                    {
                        PlayListLocation.Text = YouTubeMulti;
                    }

                    topAppend += Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                "Channel_" + channelNum + "_1" + (char)34 + " value=" + (char)34 +
                                PlayListLocation.Text + (char)34 + " />" + System.Environment.NewLine + "\t" +
                                "<setting id=" + (char)34 + "Channel_" + channelNum + "_2" + (char)34 +
                                " value=" + (char)34 + YouTubeType.SelectedIndex + 1 + (char)34 + " />" +
                                System.Environment.NewLine + "\t" + "<setting id=" + (char)34 + "Channel_" +
                                channelNum + "_3" + (char)34 + " value=" + (char)34 + MediaLimitBox.Text +
                                (char)34 + " />" + System.Environment.NewLine + "\t" + "<setting id=" +
                                (char)34 + "Channel_" + channelNum + "_4" + (char)34 + " value=" +
                                (char)34 + SortTypeBox.SelectedIndex + (char)34 + " />";
                }
                else if (PlayListType.SelectedIndex == 11)
                {
                    topAppend += System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                "Channel_" + channelNum + "_1" + (char)34 + " value=" + (char)34 +
                                PlayListLocation.Text + (char)34 + " />" + System.Environment.NewLine + "\t" +
                                "<setting id=" + (char)34 + "Channel_" + channelNum + "_2" + (char)34 +
                                " value=" + (char)34 + "1" + (char)34 + " />" + System.Environment.NewLine +
                                "\t" + "<setting id=" + (char)34 + "Channel_" + channelNum + "_3" +
                                (char)34 + " value=" + (char)34 + MediaLimitBox.Text + (char)34 +
                                " />" + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                "Channel_" + channelNum + "_4" + (char)34 + " value=" + (char)34 +
                                SortTypeBox.SelectedIndex + (char)34 + " />";
                }
                else if (PlayListType.SelectedIndex == 13)
                {
                    topAppend += Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                "Channel_" + channelNum + "_1" + (char)34 + " value=" + (char)34 +
                                SubChannelType.SelectedIndex + 1 + (char)34 + " />" + System.Environment.NewLine +
                                "\t" + "<setting id=" + (char)34 + "Channel_" + channelNum + "_2" +
                                (char)34 + " value=" + (char)34 + PlayListLocation.Text + (char)34 +
                                " />" + System.Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                "Channel_" + channelNum + "_3" + (char)34 + " value=" + (char)34 +
                                MediaLimitBox.Text + (char)34 + " />" + System.Environment.NewLine + "\t" +
                                "<setting id=" + (char)34 + "Channel_" + channelNum + "_4" + (char)34 +
                                " value=" + (char)34 + SortTypeBox.SelectedIndex + (char)34 + " />";
                }
                else if (PlayListType.SelectedIndex == 15)
                {
                    topAppend += Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                "Channel_" + channelNum + "_1" + (char)34 + " value=" + (char)34 +
                                PlayListLocation.Text + (char)34 + " />" + System.Environment.NewLine + "\t" +
                                "<setting id=" + (char)34 + "Channel_" + channelNum + "_2" + (char)34 +
                                " value=" + (char)34 + PluginNotInclude + (char)34 + " />" +
                                System.Environment.NewLine + "\t" + "<setting id=" + (char)34 + "Channel_" +
                                channelNum + "_3" + (char)34 + " value=" + (char)34 + MediaLimitBox.Text +
                                (char)34 + " />" + System.Environment.NewLine + "\t" + "<setting id=" +
                                (char)34 + "Channel_" + channelNum + "_4" + (char)34 + " value=" +
                                (char)34 + SortTypeBox.SelectedIndex + (char)34 + " />";
                }
                else
                {
                    topAppend += Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                                "Channel_" + channelNum + "_1" + (char)34 + " value=" + (char)34 +
                                Option2.Text + (char)34 + " />";
                }

                //Also append the Rulecount to the top, just underneath the channel type & 2nd value
                topAppend += Environment.NewLine + "\t" + "<setting id=" + (char)34 +
                            "Channel_" + channelNum + "_rulecount" + (char)34 + " value=" + (char)34 +
                            rulecount + (char)34 + " />";

                appendInfo = topAppend + appendInfo;

                //Combine the original text, plus the edited channel at the bottom, followed by ending the settings.
                textFile += appendInfo + System.Environment.NewLine + "</settings>";

                PseudoTvUtils.SaveFile(Settings.Default.PseudoTvSettingsLocation, textFile);

                //RefreshALL()
                var returnindex = TVGuideList.SelectedIndices[0];
                RefreshTvGuide();
                TVGuideList.Items[returnindex].Selected = true;
            }
        }

        private void Button8_Click(System.Object sender, System.EventArgs e)
        {
            if (InterleavedList.SelectedItems.Count > 0)
            {
                InterleavedList.Items[InterleavedList.SelectedIndices[0]].Remove();
            }
        }

        private void Button9_Click(System.Object sender, System.EventArgs e)
        {
            if (SchedulingList.SelectedItems.Count > 0)
            {
                SchedulingList.Items[SchedulingList.SelectedIndices[0]].Remove();
            }
        }

        private void Button7_Click(System.Object sender, System.EventArgs e)
        {
            //TODO:
            //Form4.Visible = true;
        }

        private void Button12_Click(System.Object sender, System.EventArgs e)
        {
            var response = PseudoTvUtils.ShowInputDialog("Enter TV Show's Name", "TV Show Name");

            if (!string.IsNullOrEmpty(response.Input))
            {
                NotShows.Items.Add(response);
            }

        }

        private void Button11_Click(System.Object sender, System.EventArgs e)
        {
            if (NotShows.SelectedItems.Count > 0)
            {
                NotShows.Items.RemoveAt(NotShows.SelectedIndex);
            }
        }

        private void Button14_Click(System.Object sender, System.EventArgs e)
        {
            var newItem = new string[6];

            newItem[0] = PseudoTvUtils.ShowInputDialog("Enter Channel Number", "Enter Channel Number").Input;
            newItem[1] = "1";
            newItem[2] = null;
            newItem[3] = null;
            newItem[4] = null;
            newItem[5] = null;

            var itm = default(ListViewItem);
            itm = new ListViewItem(newItem);

            var inList = false;

            if (PseudoTvUtils.IsNumeric(newItem[0]))
            {
                var firstItemValue = Convert.ToInt32(newItem[0]);
                if (firstItemValue > 0 & firstItemValue <= 999)
                {
                    for (var x = 0; x <= TVGuideList.Items.Count - 1; x++)
                    {
                        if (TVGuideList.Items[x].SubItems[0].Text == newItem[0])
                        {
                            MessageBox.Show("You already have a channel " + newItem[0]);
                            inList = true;
                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Sorry, the channel has to be 1 - 999)");
                    inList = true;
                }
            }

            if (inList == false & PseudoTvUtils.IsNumeric(newItem[0]))
            {
                TVGuideList.Items.Add(itm);

                //Now make that the selected item.
                for (var x = 0; x <= TVGuideList.Items.Count - 1; x++)
                {

                    if (TVGuideList.Items[x].SubItems[0].Text == newItem[0])
                    {
                        TVGuideList.Items[x].Selected = true;
                    }
                    else if (TVGuideList.Items[x].Selected == true)
                    {
                        TVGuideList.Items[x].Selected = false;
                    }
                }
            }
        }

        private void Button13_Click(System.Object sender, System.EventArgs e)
        {

            if (TVGuideList.Items.Count != 1)
            {
                //Loop through config file.
                //Grab all comments MINUS the ones for selected #
                //Append this & our new content to the file.

                var fileName = Settings.Default.PseudoTvSettingsLocation;
                var textFile = "";

                var channelNum = TVGuideList.Items[TVGuideList.SelectedIndices[0]].SubItems[0].Text;

                //Loop through config file.
                //Grab all comments MINUS the ones for selected #

                if (System.IO.File.Exists(fileName) == true)
                {
                    var objReader = new System.IO.StreamReader(fileName);


                    while (objReader.Peek() != -1)
                    {
                        var singleLine = objReader.ReadLine();

                        if (
                            singleLine.Contains("<setting id=" + (char)34 + "Channel_" + channelNum + "_") ||
                            singleLine.Contains("</settings"))
                        {
                        }
                        else
                        {
                            textFile = textFile + singleLine + System.Environment.NewLine;
                        }

                    }

                    objReader.Close();

                }
                else
                {
                    MessageBox.Show("File Does Not Exist");

                }

                PseudoTvUtils.SaveFile(Settings.Default.PseudoTvSettingsLocation, textFile);

                RefreshTvGuide();

                TVGuideList.SelectedItems.Clear();

                //Clear everything on the form.

                //Reset the checked options.
                ChkLogo.Checked = false;
                chkDontPlayChannel.Checked = false;
                ChkRandom.Checked = false;
                ChkRealTime.Checked = false;
                ChkResume.Checked = false;
                ChkIceLibrary.Checked = false;
                ChkExcludeBCT.Checked = false;
                ChkPopup.Checked = false;
                ChkUnwatched.Checked = false;
                ChkWatched.Checked = false;
                ChkPause.Checked = false;
                ChkPlayInOrder.Checked = false;
                ResetDays.Clear();
                ChannelName.Clear();

                //Clear other form items.
                TVGuideSubMenu.Items.Clear();
                Option2.Items.Clear();
                InterleavedList.Items.Clear();
                SchedulingList.Items.Clear();
                NotShows.Items.Clear();

            }
            else
            {
                MessageBox.Show("You must have at least one channel");
            }
        }

        private void Button10_Click(System.Object sender, System.EventArgs e)
        {
            //TODO: Form5.Visible = true;
        }

        private void Button16_Click(System.Object sender, System.EventArgs e)
        {
            //TODO:
            //Form7.Visible = true;
            //Form7.Focus();
        }

        private void Button15_Click(System.Object sender, System.EventArgs e)
        {

            if (MovieGenresList.SelectedIndex >= 0)
            {
                //Grab the 3rd column from the TVShowList, which is the TVShowID
                var genreId = LookUpGenre(MovieGenresList.Items[MovieGenresList.SelectedIndex].ToString());

                //Now, remove the link in the database.
                //PseudoTvUtils.DbExecute("DELETE FROM genrelinktvshow WHERE idGenre = '" & GenreID & "' AND idShow ='" & TVShowList.Items(TVShowList.SelectedIndices[0]).SubItems[2].Text & "'")


                MovieGenresList.Items.RemoveAt(MovieGenresList.SelectedIndex);
                // SaveTVShow_Click(Nothing, Nothing)
                RefreshGenres();
            }
        }

        private void Button17_Click(System.Object sender, System.EventArgs e)
        {

            if (MovieList.SelectedItems.Count > 0)
            {
                // Fix any issues with shows and 's.
                var movieName = MovieLabel.Text;
                //Convert show genres into the format ex:  genre1 / genre2 / etc.
                var movieGenres = ConvertGenres(MovieGenresList);
                movieName = movieName.Replace("'", "''");
                //Grab the Network ID based on the name
                var networkId = LookUpNetwork(txtMovieNetwork.Text);
                var movieId = MovieList.SelectedItems[0].SubItems[1].Text;

                if ((Settings.Default.KodiVersion >= (int)KodiVersion.Isengard))
                {
                    PseudoTvUtils.DbExecute("DELETE FROM studio_link WHERE media_id = '" + movieId + "'");
                    PseudoTvUtils.DbExecute("INSERT INTO studio_link (studio_id, media_id, media_type) VALUES ('" +
                                            networkId + "', '" +
                                            movieId + "', 'movie')");
                }
                else
                {
                    PseudoTvUtils.DbExecute("DELETE FROM studiolinkmovie WHERE idMovie = '" + movieId + "'");
                    PseudoTvUtils.DbExecute("INSERT INTO studiolinkmovie (idStudio, idMovie) VALUES ('" + networkId +
                                            "', '" + movieId +
                                            "')");
                }

                PseudoTvUtils.DbExecute("UPDATE movie SET c14 = '" + movieGenres + "', c18 ='" + txtMovieNetwork.Text +
                                        "' WHERE idMovie = '" + movieId + "'");
                //TODO: VisualStyleElement.Status.Text = "Updated " + MovieLabel.Text + " Successfully";

                if ((Settings.Default.KodiVersion >= (int)KodiVersion.Isengard))
                {
                    //Remove all genres from tv show
                    PseudoTvUtils.DbExecute("DELETE FROM genre_link  WHERE media_id = '" + movieId + "'");

                    //add each one.  one by one.
                    for (var x = 0; x <= MovieGenresList.Items.Count - 1; x++)
                    {
                        var genreId = LookUpGenre(MovieGenresList.Items[x].ToString());
                        PseudoTvUtils.DbExecute("INSERT INTO genre_link  (genre_id, media_id, media_type) VALUES ('" +
                                                genreId +
                                                "', '" + movieId + "', 'movie')");
                    }
                }
                else
                {
                    //Remove all genres from tv show
                    PseudoTvUtils.DbExecute("DELETE FROM genrelinkmovie  WHERE idMovie = '" + movieId + "'");

                    //add each one.  one by one.
                    for (var x = 0; x <= MovieGenresList.Items.Count - 1; x++)
                    {
                        var genreId = LookUpGenre(MovieGenresList.Items[x].ToString());
                        PseudoTvUtils.DbExecute("INSERT INTO genrelinkmovie (idGenre, idMovie) VALUES ('" + genreId +
                                                "', '" + movieId +
                                                "')");
                    }
                }

                //Save our spot on the list.
                var savedName = txtMovieNetwork.Text;

                //Refresh Things
                RefreshNetworkListMovies();
                RefreshGenres();

                var returnindex = MovieList.SelectedIndices[0];
                RefreshMovieList();
                MovieList.Items[returnindex].Selected = true;

            }
        }

        private void Button18_Click(System.Object sender, System.EventArgs e)
        {
            if (MovieList.SelectedItems.Count > 0)
            {
                RefreshAllStudios();
                //TODO:
                //Form8.Visible = true;
                //Form8.Focus();
            }
        }
    }
}