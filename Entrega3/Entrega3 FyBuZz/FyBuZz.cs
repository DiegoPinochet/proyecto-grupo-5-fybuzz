﻿using Entrega3_FyBuZz.CustomArgs;
using Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;
using System.Media;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;

namespace Entrega3_FyBuZz
{
    public partial class FyBuZz : Form
    {
        /*
        INDICE CODIGO: (Puedes apretar Ctrl + F y copiar los nombres que estan abajo para encontrar la sección que buscas)
                       (Puedes usar el COMMAND o el CONTENIDO si quieres buscar más especifico)
        ╔═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
        ║   | NAME: WELCOME                  | COMMAND: !WW      |  CONTENIDO: !WELCOME PANEL, !REGISTER PANEL, !LOGIN PANEL                    ║ 
        ║   | NAME: PROFILE                  | COMMAND: !PRE     |  CONTENIDO: !PROFILE PANEL, !CREATE PROFILE PANEL                            ║
        ║   | NAME: MENU                     | COMMAND: !M       |  CONTENIDO: !DISPLAY START PANEL                                             ║
        ║   | NAME: SEARCH                   | COMMAND: !SCH     |  CONTENIDO: !SEARCH PANEL, !SEARCH USER PANEL                                ║
        ║   | NAME: PLAY                     | COMMAND: !PY      |  CONTENIDO: !PLAY SONG PANEL, !PLAY VIDEO PANEL, !PLAY PLAYLIST PANEL        ║
        ║   | NAME: ADD SHOW                 | COMMAND: !AS      |  CONTENIDO: !ADD SHOW PANEL                                                  ║
        ║   | NAME: ADD                      | COMMAND: !!A      |  CONTENIDO: !CREATE SONG PANEL, !CREATE VIDEO PANEL, !CREATE PLAYLIST PANEL  ║
        ║   | NAME: SHOW                     | COMMAND: !!S      |  CONTENIDO:                                                                  ║
        ║   | NAME: DISPLAY PLAYLIST         | COMMAND: !DP      |  CONTENIDO: !DISPLAY PLAYLIST PANEL                                          ║
        ║   | NAME: ACCOUNT PROFILE SETTINGS | COMMAND: !APS     |  CONTENIDO: !ACCOUNT PROFILE SETTINGS PANEL                                  ║
        ║   | NAME: USER CHANGE              | COMMAND: !UC      |  CONTENIDO: !USER PROFILE CHANGE INFO PANEL                                  ║
        ║   | NAME: PLAY PLAYLIST            | COMMAND: !PYPL    |  CONTENIDO:                                                                  ║
        ║   | NAME: ADMIN MENU               | COMMAND: !AM      |  CONTENIDO: !ADMIN MENU PANEL                                                ║
        ╚═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝   
         */

        //PUBLIC DELEGATES
        //--------------------------------------------------------------------------------

        WindowsMediaPlayer windowsMediaPlayer = new WindowsMediaPlayer();
        WindowsMediaPlayer randomWMP = new WindowsMediaPlayer();
        SoundPlayer soundPlayer;
        private List<string> badWords = new List<string>() { "fuck", "sex", "niggas", "sexo", "ass", "nigger", "culo", "viola", "violar", "spank", "puta", "hooker", "perra", "hoe", "cocaina", "alchohol", "blunt", "weed", "marihuana", "lcd", "kush", "krippy", "penis", "dick", "cock", "shit", "percocet" };

        public delegate List<string> UserGetterListEventHandler(object source, UserEventArgs args);
        public event UserGetterListEventHandler LogInLogInButton_Clicked2;

        public delegate List<string> UserProfilesNames(object source, UserEventArgs args);
        public event UserProfilesNames GetProfileNames;

        public delegate User LogInEventHandler(object soruce, LogInEventArgs args);
        public event LogInEventHandler LogInLogInButton_Clicked;

        public delegate List<string> GetSongInfo(object source, SongEventArgs args);
        public event GetSongInfo GetSongInformation;

        public delegate List<List<string>> GetAllSongsInfo(object source, SongEventArgs args);
        public event GetAllSongsInfo GetAllSongInformation;

        public delegate List<string> GetVideoInfo(object source, VideoEventArgs args);
        public event GetVideoInfo GetVideoInformation;

        public delegate List<List<string>> GetAllVideosInfo(object source, VideoEventArgs args);
        public event GetAllVideosInfo GetAllVideosInformation;

        public delegate string ReturnChangedInfo(object source, UserEventArgs args);
        public event ReturnChangedInfo UserProfileChangeInfoConfirmButton_Clicked;

        public delegate bool RegisterEventHandler(object soruce, RegisterEventArgs args);
        public event RegisterEventHandler RegisterRegisterButton_Clicked;

        public delegate string ProfileEventHandler(object source, ProfileEventArgs args);
        public event ProfileEventHandler CreateProfileCreateProfileButton_Clicked;

        public delegate List<string> ChooseProfileEventHandlerMVC(object source, ProfileEventArgs args);
        public event ChooseProfileEventHandlerMVC ProfilesChooseProfile_Clicked2;

        public delegate Profile ChooseProfileEventHandler(object source, ProfileEventArgs args);
        public event ChooseProfileEventHandler ProfilesChooseProfile_Clicked;

        public delegate List<PlayList> ChoosePlaylistEventHandler(object source, PlaylistEventArgs args);
        public event ChoosePlaylistEventHandler DisplayPlaylistsGlobalPlaylist_Clicked;

        public delegate bool SongEventHandler(object source, SongEventArgs args);
        public event SongEventHandler CreateSongCreateSongButton_Clicked;

        public delegate List<Song> ListSongEventHandler(object source, SongEventArgs args);
        public event ListSongEventHandler SearchSongButton_Clicked;


        public delegate List<User> ListUserEventHandler(object source, RegisterEventArgs args);
        public event ListUserEventHandler SearchUserButton_Clicked;

        public delegate string SearchedUserEventHandler(object source, UserEventArgs args);
        public event SearchedUserEventHandler SearchFollowButton_Clicked;

        public delegate string PlaylistEventHandler(object source, PlaylistEventArgs args);
        public event PlaylistEventHandler CreatePlaylistCreatePlaylistButton_Clicked;

        public delegate bool CreateVideoEventHandler(object source, VideoEventArgs args);
        public event CreateVideoEventHandler CreateVideoSaveButton_Clicked;

        public delegate List<Video> ListVideoEventHandler(object source, VideoEventArgs args);
        public event ListVideoEventHandler SearchVideoButton_Clicked;

        public delegate string ChoosePLEventHanlder(object source, PlaylistEventArgs args);
        public event ChoosePLEventHanlder PlaySongChoosePlsButton_Clicked;

        public delegate string SelectPlVideoEventHandler(object source, PlaylistEventArgs args);
        public event SelectPlVideoEventHandler PlayVideoSelectPlButton_Clicked;

        public delegate string RateSongEventHandler(object source, SongEventArgs args);
        public event RateSongEventHandler PlaysSongRateButton_Clicked;

        public delegate string RateVideoEventHandler(object source, VideoEventArgs args);
        public event RateVideoEventHandler PlaysVideoRateButton_Clicked;

        public delegate Song SkipOrPreviousSongEventHandler(object source, SongEventArgs args);
        public event SkipOrPreviousSongEventHandler SkipOrPreviousSongButton_Clicked;

        public delegate Video SkipOrPreviousVideoEventHandler(object source, VideoEventArgs args);
        public event SkipOrPreviousVideoEventHandler SkipOrPreviousVideoButton_Clicked; 

        public delegate bool AddSearchedMult(object sender, UserEventArgs args);
        public event AddSearchedMult AddSearchedMult_Done;

        public delegate List<string> GetSearchedMult(object sender, UserEventArgs args);
        public event GetSearchedMult ReturnSearchedMult_Done;

        public delegate string AddPlaylistMult(object sender, PlaylistEventArgs args);
        public event AddPlaylistMult AddPlaylistMult_Done;

        public delegate string AdminMenu(object sender, UserEventArgs args);
        public event AdminMenu AdminMethods_Done;


        //ATRIBUTOS
        //--------------------------------------------------------------------------------
        private string ProfileName { get; set; }
        private List<string> queueList = new List<string>();
        private List<string> QueueList { get => queueList; set => queueList = value; }

        //--------------------------------------------------------------------------------


        //CONSTRUCTOR
        //--------------------------------------------------------------------------------
        public FyBuZz()
        {
            InitializeComponent();
           
        }
        private void FyBuZz_Load(object sender, EventArgs e)
        {
            
        }
        //--------------------------------------------------------------------------------



        /*      PINO PLS VE A DONDE VA ESTO THANKS LOV U UwU         */
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewGroup listViewUsers = new ListViewGroup("Users");


            //listView1.Items.Add();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                if (filename.Contains(".srt") == true)
                {
                    CreateSongLyricsTextBox.Text = filename;
                }
                else
                {
                    CreateSongInvalidCredentialTextBox.AppendText("ERROR[!] wrong file format");
                }
                
            }
        }
        /*      PINO PLS VE A DONDE VA ESTO THANKS LOV U UwU         */



        /*---------------------------------------------------!WW----------------------------------------------------- */
        
            
            //<<!WELCOME PANEL>>
        //-------------------------------------------------------------------------------------------

        //BUTTONS
        private void WelcomeLogInButton_Click(object sender, EventArgs e)
        {
            UserLogInTextBox.Clear();
            PasswordLogInTextBox.Clear();
            LogInInvalidCredentialsTetxbox.Clear();
            LogInPanel.BringToFront();

        }

        private void WelcomeRegisterButton_Click(object sender, EventArgs e)
        {
            RegisterPanel.BringToFront();
        }


        //GO BACK/CLOSE

        private void WelcomeCloseFyBuZz_Click(object sender, EventArgs e)
        {
            Close();
        }
        //-------------------------------------------------------------------------------------------




            //<<!REGISTER PANEL>>
        //-------------------------------------------------------------------------------------------

        private void RegisterRegisterButton_Click(object sender, EventArgs e)
        {
            string username = UsernameRegisterTextBox.Text;
            string email = EmailRegisterTextBox.Text;
            string pswd = PasswordRegisterTextBox.Text;
            string subs = SubscriptionRegisterDomainUp.Text;
            bool privacy = PrivacyRegisterCheckBox.Checked; // true = privado
            string gender = GenderRegisterDomainUp.Text;
            DateTime birthday = AgeRegisterDateTimePicker.Value;
            string profileType = ProfileTypeRegisterDomainUp.Text;
            OnRegisterRegisterButtonClicked(username, email, pswd, subs, privacy, gender, birthday, profileType);
            WelcomePanel.BringToFront();
        }

        //ONEVENT
        public void OnRegisterRegisterButtonClicked(string username, string email, string psswd, string subs, bool priv, string gender, DateTime birthday, string profileType)
        {
            if (RegisterRegisterButton_Clicked != null)
            {
                bool result = RegisterRegisterButton_Clicked(this, new RegisterEventArgs() { UsernameText = username, EmailText = email, PasswrodText = psswd, SubsText = subs, PrivacyText = priv, GenderText = gender, BirthdayText = birthday, ProfileTypeText = profileType });
                if (!result) //Resultado es falso
                {
                    RegisterInvalidCredencialsTextBox.AppendText("User already exist");
                    RegisterInvalidCredencialsTextBox.Visible = true;
                    Thread.Sleep(2000);
                    UsernameRegisterTextBox.Clear();
                    EmailRegisterTextBox.Clear();
                    PasswordRegisterTextBox.Clear();
                    RegisterInvalidCredencialsTextBox.Clear();

                }
                else
                {
                    RegisterInvalidCredencialsTextBox.AppendText("Registered Succesfull");
                    RegisterInvalidCredencialsTextBox.Visible = true;
                    UsernameRegisterTextBox.Clear();
                    EmailRegisterTextBox.Clear();
                    PasswordRegisterTextBox.Clear();
                    RegisterInvalidCredencialsTextBox.Clear();
                    Thread.Sleep(2000);
                }
            }
        }

        //GO BACK/CLOSE
        private void GobackRegisterButton_Click(object sender, EventArgs e)
        {
            WelcomePanel.BringToFront();
        }
        //-------------------------------------------------------------------------------------------




            //<<!LOGIN PANEL>>
        //-------------------------------------------------------------------------------------------

        private void LogInLogInButton_Click(object sender, EventArgs e)
        {
            LogInInvalidCredentialsTetxbox.Clear();
            List<string> userGetter = new List<string>();
            string username = UserLogInTextBox.Text;
            string pass = PasswordLogInTextBox.Text;
            userGetter = OnLogInLogInButton_Clicked2(username);
            //user = OnLoginButtonClicked(username, pass);
            if (userGetter != null && userGetter[1] == pass && userGetter[9] != "1")
            {
                ProfilesInvalidCredentialTextBox.Clear();
                ProfilePanel.BringToFront();
            }
            else
            {
                LogInInvalidCredentialsTetxbox.Clear();
                LogInInvalidCredentialsTetxbox.AppendText("Incorrect Username or Password");
                Thread.Sleep(2000);
                LogInInvalidCredentialsTetxbox.Visible = true;
            }
            LogInInvalidCredentialsTetxbox.Clear();
        }
        //ONEVENT

        public User OnLoginButtonClicked(string username, string password)
        //Este metodo ya no deberia servir, no es MVC
        {
            User user = new User();
            if (LogInLogInButton_Clicked != null)
            {
                user = LogInLogInButton_Clicked(this, new LogInEventArgs() { UsernameText = username, PasswrodText = password });
                if (user == null) //Resultado es falso
                {
                    LogInInvalidCredentialsTetxbox.AppendText("Incorrect Username or Password");
                    Thread.Sleep(2000);
                    LogInInvalidCredentialsTetxbox.Visible = true;
                }
                else
                {
                    LogInInvalidCredentialsTetxbox.AppendText("Log-In Succesfull");
                    Thread.Sleep(2000);
                    LogInInvalidCredentialsTetxbox.Visible = true;

                    ProfilesWelcomeTextBox.AppendText("Welcome to FyBuZz " + user.Username);
                    foreach (Profile profile in user.Perfiles)
                    {
                        ProfileDomainUp.Items.Add(profile.ProfileName);
                    }
                }
            }
            return user;
        }

        public List<string> OnLogInLogInButton_Clicked2(string username)
        {
            List<string> userGetterStringList = new List<string>();
            List<string> userProfileNames = new List<string>();
            if (LogInLogInButton_Clicked2 != null)
            {
                userGetterStringList = LogInLogInButton_Clicked2(this, new UserEventArgs() { UsernameText = username });
                if (userGetterStringList != null && userGetterStringList[1] == PasswordLogInTextBox.Text)
                {
                    LogInInvalidCredentialsTetxbox.AppendText("Log-In Succesfull");
                    Thread.Sleep(2000);
                    LogInInvalidCredentialsTetxbox.Visible = true;

                    ProfilesWelcomeTextBox.AppendText("Welcome to FyBuZz " + userGetterStringList[0]);
                    userProfileNames = GetProfileNames(this, new UserEventArgs() { UsernameText = username });
                    foreach (string profilename in userProfileNames)
                    {
                        ProfileDomainUp.Items.Add(profilename);
                    }
                    return userGetterStringList;
                }
                else
                {
                    LogInInvalidCredentialsTetxbox.AppendText("Incorrect Username or Password");
                    Thread.Sleep(2000);
                    LogInInvalidCredentialsTetxbox.Visible = true;
                    return null;
                }

            }
            return null;
        }

        //GO BACK/CLOSE
        private void GoBackLoginButton_Click(object sender, EventArgs e)
        {

            int cont = 0;
            foreach (object searched in ProfileDomainUp.Items)
            {
                cont++;
            }
            for (int i = 0; i < cont; cont--)
            {
                ProfileDomainUp.Items.RemoveAt(cont - 1);
            }
            LogInPanel.BringToFront();

            UserLogInTextBox.Clear();
            PasswordLogInTextBox.Clear();
            LogInInvalidCredentialsTetxbox.Clear();
            WelcomePanel.BringToFront();
        }
        //-------------------------------------------------------------------------------------------



        /*---------------------------------------------------!PRE----------------------------------------------------- */

        
            //<<!PROFILE PANEL>>
        //-------------------------------------------------------------------------------------------
        private void ProfilesChooseProfile_Click(object sender, EventArgs e)
        {
            soundPlayer = new SoundPlayer();
            string username = UserLogInTextBox.Text;

            string password = PasswordLogInTextBox.Text;
            string profileProfileName = ProfileDomainUp.Text;
            List<string> profileGetterString = OnProfilesChooseProfile_Click2(profileProfileName, username, password);

            ProfileName = profileProfileName;
            DisplayStartPanel.BringToFront();


            //Creo que cada vez que necesite el perfil debo llamar a este método con el parametro
            //que venga del "ProfileDomainUp.Text"
        }

        private void ProfileCreateProfileButton_Click(object sender, EventArgs e)
        {
            CreateProfilePanel.BringToFront();
        }

        //ON EVENT

        public Profile OnProfilesChooseProfile_Click(string pName, string usr, string pass)
        {
            if (ProfilesChooseProfile_Clicked != null)
            {
                Profile choosenProfile = ProfilesChooseProfile_Clicked(this, new ProfileEventArgs() { ProfileNameText = pName, UsernameText = usr, PasswordText = pass });
                ProfilesInvalidCredentialTextBox.ResetText();
                ProfilesInvalidCredentialTextBox.AppendText("Entering FyBuZz with... " + choosenProfile.ProfileName);

                Thread.Sleep(2000);

                return choosenProfile;
            }
            else
            {
                return null;
            }
        }
        public List<string> OnProfilesChooseProfile_Click2(string pName, string usr, string pass)
        {
            if (ProfilesChooseProfile_Clicked2 != null)
            {
                List<string> choosenProfile = ProfilesChooseProfile_Clicked2(this, new ProfileEventArgs() { ProfileNameText = pName, UsernameText = usr, PasswordText = pass });
                ProfilesInvalidCredentialTextBox.ResetText();
                ProfilesInvalidCredentialTextBox.AppendText("Entering FyBuZz with... " + choosenProfile[0]);

                Thread.Sleep(2000);

                return choosenProfile;
            }
            else
            {
                return null;
            }
        }

        //GO BACK/CLOSE
        private void ProfileGoBack_Click(object sender, EventArgs e)
        {
            int cont = 0;
            foreach(object searched in ProfileDomainUp.Items)
            {
                cont++;
            }
            for(int i = 0; i < cont; cont--)
            {
                ProfileDomainUp.Items.Remove(cont - 1);
            }
            LogInPanel.BringToFront();
            UserLogInTextBox.Clear();
            PasswordLogInTextBox.Clear();
            LogInInvalidCredentialsTetxbox.Clear();
        }
        //-------------------------------------------------------------------------------------------




       
            //<<!CREATE PROFILE PANEL>>
        //-------------------------------------------------------------------------------------------

        private void CreateProfileCreateProfileButton_Click(object sender, EventArgs e)
        {
            string pName = CreateProfileProfileNameTextBox.Text;
            string pGender = CreateProfileProfileGenderDomainUp.Text;
            DateTime pBirth = CreateProfileProfileBirthdayTimePicker.Value;
            string pType = CreateProfileProfileTypeDomainUp.Text;
            string pEmail = EmailRegisterTextBox.Text;
            string username = UserLogInTextBox.Text;
            string psswd = PasswordLogInTextBox.Text;
            Image pPic = CreateProfilePic1.Image;
            if (CreateProfilePicCheckedListBox.SelectedIndex == 0) pPic = CreateProfilePic1.Image;
            else if (CreateProfilePicCheckedListBox.SelectedIndex == 1) pPic = CreateProfilePic2.Image;
            else if (CreateProfilePicCheckedListBox.SelectedIndex == 2) pPic = CreateProfilePic3.Image;
            else if (CreateProfilePicCheckedListBox.SelectedIndex == 3) pPic = CreateProfilePic4.Image;
            OnCreateProfileCreateProfileButton_Click2(username, psswd, pName, pGender, pType, pEmail, pBirth, pPic);
        }

        //ONEVENT

        public void OnCreateProfileCreateProfileButton_Click2(string username, string pswd, string pName, string pGender, string pType, string pEmail, DateTime pBirth, Image pPic)
        {
            if (CreateProfileCreateProfileButton_Clicked != null)
            {
                string result = CreateProfileCreateProfileButton_Clicked(this, new ProfileEventArgs() { UsernameText = username, PasswordText = pswd, ProfileNameText = pName, EmailText = pEmail, GenderText = pGender, BirthdayText = pBirth, ProfileTypeText = pType, PicImage = pPic });
                if (result != null)
                {
                    ProfileDomainUp.Items.Add(result);
                    ProfilePanel.BringToFront();
                    CreateProfileProfileNameTextBox.Clear();
                }
                else
                {
                    ProfilePanel.BringToFront();
                    ProfilesInvalidCredentialTextBox.AppendText("Only premium Users can create profiles");
                }
            }
        }

        //GO BACK/CLOSE
        private void CreateProfileGoBackButton_Click(object sender, EventArgs e)
        {
            ProfilePanel.BringToFront();
        }
        //-------------------------------------------------------------------------------------------




        /*---------------------------------------------------!M----------------------------------------------------- */


        
            //<<!DISPLAY START PANEL>>
        //-------------------------------------------------------------------------------------------

        private void DisplayStartSearchButton_Click(object sender, EventArgs e)
        {
            SearchPanel.BringToFront();
        }

        private void DisplayStartSettingsButton_Click(object sender, EventArgs e)
        {
            AccountSettingsUsernameTextBox.Clear();
            AccountSettingsPasswordTextBox.Clear();
            AccountSettingsAccountTypeTextBox.Clear();
            AccountSettingsEmailTextBox.Clear();
            AccountSettingsFollowersTextBox.Clear();
            AccountSettingsFollowingTextBox.Clear();

            ProfileSettingsNameTextBox.Clear();
            ProfileSettingsProfileTypeTextBox.Clear();
            ProfileSettingsGenderTextBox.Clear();
            ProfileSettingsBirthdayTextBox.Clear();
            //ProfileSettingsProfilePicImageBox.Image.Dispose();

            ProfilesInvalidCredentialTextBox.Clear();
            string username = UserLogInTextBox.Text;
            string password = PasswordLogInTextBox.Text;
            User user = new User();
            user = OnLoginButtonClicked(username, password);
            List<string> userGetterString = new List<string>();
            userGetterString = OnLogInLogInButton_Clicked2(username);
            string profileProfileName = ProfileDomainUp.Text;
            List<string> profileGetterString = OnProfilesChooseProfile_Click2(profileProfileName, username, password);

            AccountSettingsUsernameTextBox.AppendText(userGetterString[0]);
            AccountSettingsPasswordTextBox.AppendText(userGetterString[1]);
            AccountSettingsEmailTextBox.AppendText(userGetterString[2]);
            AccountSettingsAccountTypeTextBox.AppendText(userGetterString[3]);
            AccountSettingsFollowersTextBox.AppendText(userGetterString[4]);
            AccountSettingsFollowingTextBox.AppendText(userGetterString[5]);

            foreach (string seguidor in user.FollowingList)
            {
                AccountSettingsFollowingListDomaiUp.Items.Add(seguidor);
            }


            foreach (string followers in user.FollowerList)
            {
                AccountSettingsFollowerListDomainUp.Items.Add(followers);
            }

            ProfileSettingsNameTextBox.AppendText(profileGetterString[0]);
            ProfileSettingsProfileTypeTextBox.AppendText(profileGetterString[1]);
            ProfileSettingsGenderTextBox.AppendText(profileGetterString[2]);
            ProfileSettingsBirthdayTextBox.AppendText(profileGetterString[3]);

            Image pPic = CreateProfilePic1.Image;
            if (CreateProfilePicCheckedListBox.SelectedIndex == 0) pPic = CreateProfilePic1.Image;
            else if (CreateProfilePicCheckedListBox.SelectedIndex == 1) pPic = CreateProfilePic2.Image;
            else if (CreateProfilePicCheckedListBox.SelectedIndex == 2) pPic = CreateProfilePic3.Image;
            else if (CreateProfilePicCheckedListBox.SelectedIndex == 3) pPic = CreateProfilePic4.Image;

            ProfileSettingsProfilePicImageBox.Image = pPic;

            AccountProfileSettingsPanel.BringToFront();
        }


        private void DisplayStartAdminMenuButton_Click(object sender, EventArgs e)
        {
            List<string> listUser = OnLogInLogInButton_Clicked2(UserLogInTextBox.Text);
            List<User> userDatabase = OnSearchUserButton_Click();
            if(listUser[3] == "admin")
            {
                foreach(User user in userDatabase)
                {
                    if(user.Username != null)
                    {
                        AdminMenuAllUsers.Items.Add(user.Username);
                    }
                }
                AdminMenuPanel.BringToFront();
            }
            else
            {
                DisplayStartInvalidCredentials.AppendText("Only admins can acces the Admin Menu");
                Thread.Sleep(2000);
                DisplayStartInvalidCredentials.Clear();
            }
        }

        //<<PANEL DISPLAY PLAYLISTS>>

        private void DisplayStartDisplayPlaylistButton_Click(object sender, EventArgs e)
        {
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            //<<Followed>>
            if (profile.FollowedPlayList.Count() == 1)
            {
                DisplayPlaylistsFollowedPlaylist1.Visible = true;
                DisplayPlaylistsFollowedPlaylist1.Image = CreateProfilePic1.Image;
            }
            else if (profile.FollowedPlayList.Count() == 2)
            {
                DisplayPlaylistsFollowedPlaylist1.Visible = true;
                DisplayPlaylistsFollowedPlaylist2.Visible = true;
            }
            else if (profile.FollowedPlayList.Count() == 3)
            {
                DisplayPlaylistsFollowedPlaylist1.Visible = true;
                DisplayPlaylistsFollowedPlaylist2.Visible = true;
                DisplayPlaylistsFollowedPlaylist3.Visible = true;
            }
            else if (profile.FollowedPlayList.Count() > 3)
            {
                DisplayPlaylistsFollowedPlaylist1.Visible = true;
                DisplayPlaylistsFollowedPlaylist2.Visible = true;
                DisplayPlaylistsFollowedPlaylist3.Visible = true;
                DisplayPlaylistsMoreFollowedPlaylistButton.Visible = true;
            }
            //<<Created>>
            if (profile.CreatedPlaylist.Count() == 1)
            {
                DisplayPlaylistCreatedPlaylistImage1.Visible = true;
                DisplayPlaylistCreatedPlaylistImage1.Image = CreateProfilePic1.Image;
            }
            else if (profile.FollowedPlayList.Count() == 2)
            {
                DisplayPlaylistCreatedPlaylistImage1.Visible = true;
                DisplayPlaylistCreatedPlaylistImage2.Visible = true;
            }
            else if (profile.FollowedPlayList.Count() == 3)
            {
                DisplayPlaylistCreatedPlaylistImage1.Visible = true;
                DisplayPlaylistCreatedPlaylistImage2.Visible = true;
                DisplayPlaylistCreatedPlaylistImage3.Visible = true;
            }
            else if (profile.FollowedPlayList.Count() > 3)
            {
                DisplayPlaylistCreatedPlaylistImage1.Visible = true;
                DisplayPlaylistCreatedPlaylistImage2.Visible = true;
                DisplayPlaylistCreatedPlaylistImage3.Visible = true;
                DisplayPlaylistCreatedPlaylistButton.Visible = true;
            }
            DisplayPlaylistPanel.BringToFront();
        }

        private void DisplayStartShowAddButton_Click(object sender, EventArgs e)
        {
            AddShowPanel.BringToFront();
        }

        //ONEVENT


        //GO BACK/CLOSE
        private void DisplayStartCloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DisplayStartLogOutButton_Click(object sender, EventArgs e)
        {
            LogInPanel.BringToFront();
            UserLogInTextBox.ResetText();
            PasswordLogInTextBox.ResetText();
            LogInInvalidCredentialsTetxbox.Clear();
            ProfilesWelcomeTextBox.Clear();
            ProfilesInvalidCredentialTextBox.Clear();
            ProfileDomainUp.ResetText();
            ProfilesInvalidCredentialTextBox.Clear();
        }

        private void DisplayStartLogOutProfileButton_Click(object sender, EventArgs e)
        {
            AccountSettingsUsernameTextBox.Clear();
            AccountSettingsPasswordTextBox.Clear();
            AccountSettingsAccountTypeTextBox.Clear();
            AccountSettingsEmailTextBox.Clear();
            AccountSettingsFollowersTextBox.Clear();
            AccountSettingsFollowingTextBox.Clear();

            ProfileSettingsNameTextBox.Clear();
            ProfileSettingsProfileTypeTextBox.Clear();
            ProfileSettingsGenderTextBox.Clear();
            ProfileSettingsBirthdayTextBox.Clear();
            //ProfileSettingsProfilePicImageBox.Image.Dispose();

            ProfilesInvalidCredentialTextBox.Clear();
            ProfilePanel.BringToFront();
        }

        private void DisplayPlaylistGoBackButton_Click(object sender, EventArgs e)
        {
            DisplayStartPanel.BringToFront();
        }
        //-------------------------------------------------------------------------------------------





        /*---------------------------------------------------!SCH----------------------------------------------------- */


        
            //<<!SEARCH PANEL>>
        //-------------------------------------------------------------------------------------------

        private void SearchSearchButton_Click(object sender, EventArgs e)
        {
            SearchSearchResultsDomainUp.ReadOnly = true;
            SearchSearchResultsDomainUp.Text = "Searched Results:";

            string search = SearchSearchTextBox.Text; //Bad Bunny and Trap and ... and ...

            bool filtersOn = SearchFiltersOnCheckBox.Checked;
            List<List<string>> allSongInfo = ReturnAllSongsInfo();
            List<List<string>> allVideosInfo = ReturnAllVideosInfo();
            List<Song> songDataBase = new List<Song>();
            songDataBase = OnSearchSongButton_Click();
            List<User> userDataBase = new List<User>();
            userDataBase = OnSearchUserButton_Click();
            List<Video> videoDataBase = new List<Video>();
            videoDataBase = OnSearchVideoButton_Click();
            List<PlayList> playlistDataBase = new List<PlayList>();
            playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();
            int cont = 0;
            foreach (object searched in SearchSearchResultsDomainUp.Items)
            {
                cont++;
            }
            for (int i = 0; i < cont; cont--)
            {
                SearchSearchResultsDomainUp.Items.RemoveAt(cont - 1);
            }

            if (!filtersOn)
            {
                foreach (Song song in songDataBase)
                {

                    if (song.InfoSong().Contains(search))
                    {
                        SearchSearchResultsDomainUp.Visible = true;
                        SearchSearchResultsDomainUp.Items.Add(song.SearchedInfoSong());
                    }
                }
                foreach (User user in userDataBase)
                {
                    if (user.infoUser().Contains(search))
                    {
                        SearchSearchResultsDomainUp.Visible = true;
                        SearchSearchResultsDomainUp.Items.Add("User: " + user.SearchedInfoUser());
                    }
                }
                foreach (PlayList playlist in playlistDataBase)
                {
                    if (playlist.InfoPlayList().Contains(search))
                    {
                        SearchSearchResultsDomainUp.Visible = true;
                        SearchSearchResultsDomainUp.Items.Add(playlist.DisplayInfoPlayList());
                    }
                }

                foreach (Video video in videoDataBase)
                {
                    if (video.InfoVideo().Contains(search))
                    {
                        SearchSearchResultsDomainUp.Visible = true;
                        SearchSearchResultsDomainUp.Items.Add(video.SearchedInfoVideo());
                    }
                }
            }
            else
            {
                SearchAndOrCheckBox.Visible = true;
                SearchFiltersCheBox.Visible = true;
                string logic = null;
                List<int> allChosenFilters = new List<int>();
                foreach (object andOr in SearchAndOrCheckBox.CheckedItems)
                {
                    logic = andOr.ToString();
                }
                foreach (object filter in SearchFiltersCheBox.CheckedIndices)
                {
                    allChosenFilters.Add((int)filter);
                }
                if (logic == "And")
                {
                    foreach (List<string> songInfo in allSongInfo)
                    {
                        int contS = 0;
                        for (int n = 0; n < allChosenFilters.Count(); n++)
                        {
                            if (allChosenFilters[n] <= 7)
                            {
                                int newIndex = allChosenFilters[n];
                                if (songInfo[newIndex].Contains(search) == true)
                                {
                                    contS++;
                                }

                            }

                        }
                        if (contS >= allChosenFilters.Count())
                        {
                            SearchSearchResultsDomainUp.Visible = true;
                            SearchSearchResultsDomainUp.Items.Add("Song: " + songInfo[0] + ": Artist: " + songInfo[1]);
                        }

                    }
                    foreach (List<string> videoInfo in allVideosInfo)
                    {
                        int contS = 0;
                        for (int n = 0; n < allChosenFilters.Count(); n++)
                        {
                            if (allChosenFilters[n] >= 7)
                            {
                                int newIndex = allChosenFilters[n] - 7;
                                if (videoInfo[newIndex].Contains(search) == true)
                                {
                                    contS++;
                                }
                            }

                        }
                        if (contS >= allChosenFilters.Count())
                        {
                            SearchSearchResultsDomainUp.Visible = true;
                            SearchSearchResultsDomainUp.Items.Add("Video: " + videoInfo[0] + ": Actors: " + videoInfo[1] + ": Directors:" + videoInfo[3]);
                        }

                    }
                }
                else
                {
                    foreach (List<string> songInfo in allSongInfo)
                    {
                        int contS = 0;
                        for (int n = 0; n < allChosenFilters.Count(); n++)
                        {
                            if (allChosenFilters[n] <= 7)
                            {
                                int newIndex = allChosenFilters[n];
                                if (songInfo[newIndex].Contains(search) == true)
                                {
                                    contS++;
                                }
                            }

                        }
                        if (contS != 0)
                        {
                            SearchSearchResultsDomainUp.Visible = true;
                            SearchSearchResultsDomainUp.Items.Add("Song: " + songInfo[0] + ": Artist: " + songInfo[1]);
                        }

                    }
                    foreach (List<string> videoInfo in allVideosInfo)
                    {
                        int contS = 0;
                        for (int n = 0; n < allChosenFilters.Count(); n++)
                        {
                            if (allChosenFilters[n] >= 7)
                            {
                                int newIndex = allChosenFilters[n] - 7;
                                if (videoInfo[newIndex].Contains(search) == true)
                                {
                                    contS++;
                                }
                            }

                        }
                        if (contS != 0)
                        {
                            SearchSearchResultsDomainUp.Visible = true;
                            SearchSearchResultsDomainUp.Items.Add("Video: " + videoInfo[0] + ": Actors: " + videoInfo[1] + ": Directors:" + videoInfo[3]);
                        }

                    }

                }
            }
            if (SearchSearchResultsDomainUp.Items.Count == 0)
            {
                SearchInvalidCredentialsTextBox.AppendText("ERROR[!] Nothing found.");
                Thread.Sleep(1000);
                SearchInvalidCredentialsTextBox.Clear();
            }
            PlaySongChoosePlsDomainUp.Visible = false;
            PlaySongChoosePlsDomainUp.ResetText();
            PlaySongChoosePlsDomainUp.ReadOnly = true;
            PlaySongMessageTextBox.Clear();

        }

        private void SearchSelectMultButton_Click(object sender, EventArgs e)
        {
            soundPlayer = new SoundPlayer();
            List<Song> songDataBase = new List<Song>();
            songDataBase = OnSearchSongButton_Click();
            List<PlayList> playListsDataBase = new List<PlayList>();
            playListsDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();
            List<Video> videoDataBase = OnSearchVideoButton_Click();
            List<string> infoProfile = OnProfilesChooseProfile_Click2(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);

            string multimediaType = SearchSearchResultsDomainUp.Text;

            if (multimediaType.Contains("Song:") == true && multimediaType.Contains("Artist:") == true)
            {
                string[] MultimediaType = SearchSearchResultsDomainUp.Text.Split(':');
                List<string> songMVCInfo = GetSongButton(MultimediaType[1], MultimediaType[3]);
                soundPlayer.Stop();
                windowsMediaPlayer.controls.pause();
                foreach (Song song in songDataBase)
                {
                    int badWordsCount = 0;
                    if (songMVCInfo[5] != null)
                    {
                        string songLyrics = File.ReadAllText(songMVCInfo[5]);
                        foreach (string badWord in badWords)
                        {
                            if (songLyrics.Contains(badWord) == true)
                            {
                                badWordsCount++;
                            }
                        }
                    }
                    if (int.Parse(infoProfile[3]) < 16 && badWordsCount != 0)
                    {
                        SearchInvalidCredentialsTextBox.AppendText("Song with explicit content only 16+");
                        Thread.Sleep(1000);
                        SearchInvalidCredentialsTextBox.Clear();
                    }
                    else
                    {
                        if (songMVCInfo[6].Contains(".mp3") == true)
                        {
                            string result = SearchSearchResultsDomainUp.Text;
                            string songInfo = song.SearchedInfoSong();
                            if (result == song.SearchedInfoSong())
                            {
                                AddingSearchedMult(ProfileDomainUp.Text, song.SongFile, null);
                                Thread.Sleep(2000);
                                PlayerPlayingLabel.Clear();
                                SearchPlayingLabel.Clear();
                                PlaySongProgressBar.Value = 0;
                                PlaySongTimerTextBox.ResetText();

                                windowsMediaPlayer.URL = song.SongFile;

                                DurationTimer.Interval = 1000;
                                PlaySongProgressBar.Maximum = (int)(song.Duration * 60);
                                SearchProgressBar.Maximum = (int)(song.Duration * 60);
                                PlayPlaylistProgressBarBox.Maximum = (int)(song.Duration * 60);
                                PlaySongPanel.BringToFront();
                                PlayerPlayingLabel.AppendText("Song playing: " + song.Name + song.Format);
                                SearchPlayingLabel.AppendText("Song playing: " + song.Name + song.Format);
                                DurationTimer.Start();
                                break;
                            }
                        }
                        else if (song.Format == ".wav")
                        {
                            string result = SearchSearchResultsDomainUp.Text;
                            if (result == song.SearchedInfoSong())
                            {
                                AddingSearchedMult(ProfileDomainUp.Text, song.SongFile, null);
                                Thread.Sleep(2000);
                                PlayerPlayingLabel.Clear();
                                SearchPlayingLabel.Clear();
                                PlaySongProgressBar.Value = 0;
                                PlaySongTimerTextBox.ResetText();
                                soundPlayer.SoundLocation = song.SongFile;
                                soundPlayer.Play();
                                DurationTimer.Interval = 1000;
                                PlaySongProgressBar.Maximum = (int)(song.Duration * 60);
                                SearchProgressBar.Maximum = (int)(song.Duration * 60);
                                PlayPlaylistProgressBarBox.Maximum = (int)(song.Duration * 60);
                                PlaySongPanel.BringToFront();
                                PlayerPlayingLabel.AppendText("Song playing: " + song.Name + song.Format);
                                SearchPlayingLabel.AppendText("Song playing: " + song.Name + song.Format);
                                DurationTimer.Start();
                                break;
                            }
                        }
                    }
                }
            }
            else if (multimediaType.Contains("PlayList Name:") == true)
            {

                string result = SearchSearchResultsDomainUp.Text;
                string plName = "";
                foreach (PlayList playList in playListsDataBase)
                {
                    string ex = playList.DisplayInfoPlayList();
                    if (result == ex)
                    {
                        plName = playList.NamePlayList;
                        if (playList.Format == ".mp3" || playList.Format == ".wav")
                        {
                            foreach (Song song in playList.Songs)
                            {
                                PlayPlaylistShowMultimedia.Items.Add(song.SearchedInfoSong());
                            }
                        }
                        else if(playList.Format == ".mp4" || playList.Format == ".mov" || playList.Format == ".avi")
                        {
                            foreach(Video video in playList.Videos)
                            {
                                PlayPlaylistShowMultimedia.Items.Add(video.SearchedInfoVideo());
                            }
                        }
                    }
                }
                PlayPlaylistLabel.Text = "Playlist: " + plName;
                PlayPlaylistPanel.BringToFront();
            }

            else if (multimediaType.Contains("Video:") == true)
            {
                string[] MultimediaType = SearchSearchResultsDomainUp.Text.Split(':');
                List<string> videoMVCInfo = GetVideoButton(MultimediaType[1], MultimediaType[3], MultimediaType[5]);
                if (int.Parse(videoMVCInfo[4]) > int.Parse(infoProfile[3]))
                {
                    SearchInvalidCredentialsTextBox.AppendText("Video has age restriction");
                    Thread.Sleep(1000);
                    SearchInvalidCredentialsTextBox.Clear();

                }
                else
                {
                    foreach (Video video in videoDataBase)
                    {
                        string result = SearchSearchResultsDomainUp.Text;
                        if (result == video.SearchedInfoVideo())
                        {
                            AddingSearchedMult(ProfileDomainUp.Text, null, video.FileName);
                            Thread.Sleep(2000);
                            PlayVideoPanel.BringToFront();
                            wmpVideo.URL = video.FileName;
                        }
                    }
                }
            }
            PlaySongRateNumDomainUp.Refresh();
            PlaySongRateMessageTextBox.Clear();
            SearchPlayingLabel.Clear();

        }
        

        private void SearchFiltersOnCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SearchAndOrCheckBox.ClearSelected();
            SearchFiltersCheBox.ClearSelected();
            SearchFiltersCheBox.Visible = true;
            SearchAndOrCheckBox.Visible = true;
        }

        private void SearchViewUserButton_Click(object sender, EventArgs e)
        {
            SearcUserPanel.BringToFront();
            List<string> userGetter = OnLogInLogInButton_Clicked2(SearchSearchTextBox.Text);
            if (userGetter[8] != "True")
            {
                SearUserName.Visible = true;
                SearcUserEmailTextBox.Visible = true;
                SearchUserFollowers.Visible = true;
                SearchUserFollowing.Visible = true;

                SearUserName.AppendText(userGetter[0]);
                SearcUserEmailTextBox.AppendText(userGetter[2]);
                SearchUserFollowers.AppendText(userGetter[4]);
                SearchUserFollowing.AppendText(userGetter[5]);
                SearcUserPanel.BringToFront();
            }
            else
            {
                SearUserName.Visible = true;
                SearcUserEmailTextBox.Visible = true;
                SearUserName.AppendText(userGetter[0]);
                SearcUserEmailTextBox.AppendText("This user is private");
                SearcUserPanel.BringToFront();
            }
        }

        //!SEARCH PLAY PANEL

        private void SearchPlayButton_Click(object sender, EventArgs e)
        {
            soundPlayer.Play();
            List<Song> songDataBase = new List<Song>();
            songDataBase = OnSearchSongButton_Click();
            foreach (Song song in songDataBase)
            {
                if (song.Format == ".mp3")
                {
                    windowsMediaPlayer.controls.play();
                    DurationTimer.Start();
                    break;
                }
                else if (song.Format == ".wav")
                {

                    DurationTimer.Start();
                }
            }
        }


        private void SearchPauseBotton_Click(object sender, EventArgs e)
        {
            soundPlayer.Stop();
            List<Song> songDataBase = new List<Song>();
            songDataBase = OnSearchSongButton_Click();
            foreach (Song song in songDataBase)
            {
                if (song.Format == ".mp3")
                {
                    windowsMediaPlayer.controls.pause();
                    DurationTimer.Stop();
                    break;
                }
                else if (song.Format == ".wav")
                {
                    soundPlayer.Stop();
                    DurationTimer.Stop();
                    break;
                }
            }
        }

        private void SearchPlayerToMultButton_Click(object sender, EventArgs e)
        {
            string mult = SearchPlayingLabel.Text;
            if (mult.Contains("Song") == true)
            {
                PlaySongPanel.BringToFront();
            }
            else if ((mult.Contains("Playlist") == true && mult.Contains(".mp3") == true) || (mult.Contains("Playlist") == true && mult.Contains(".wav") == true))
            {
                PlayPlaylistPanel.BringToFront();
            }
        }


        private void DurationTimer_Tick(object sender, EventArgs e)
        {
            PlaySongProgressBar.Increment(1);
            PlayPlaylistProgressBarBox.Increment(1);
            SearchProgressBar.Increment(1);
            PlaySongTimerTextBox.Text = PlaySongProgressBar.Value.ToString();
            PlayPlaylistTimerBox.Text = PlayPlaylistProgressBarBox.Value.ToString();
            SearchTimerTextBox.Text = PlaySongProgressBar.Value.ToString();
            if (SearchProgressBar.Value == SearchProgressBar.Maximum)
            {

            }

        }


        //ONEVENT

        //GO BACK/CLOSE

        private void SearchGoBackButton_Click(object sender, EventArgs e)
        {
            SearchSearchTextBox.Text = "Search Songs,Video, Playlists or Users";
            SearchSearchResultsDomainUp.Visible = false;
            //SearchSearchResultsDomainUp.Items.Clear();
            SearchSearchResultsDomainUp.Text = "Searched Results:";
            DisplayStartPanel.BringToFront();
            if (windowsMediaPlayer.URL != null)
            {
                PlayerPanel.BringToFront();
                PlayerPanel.Dock = DockStyle.Bottom;
            }
        }
        //-------------------------------------------------------------------------------------------




        //<<!SEARCH USER PANEL>>
        //-------------------------------------------------------------------------------------------

        private void SearchUserFollowButton_Click(object sender, EventArgs e)
        {
            List<User> userDataBase = new List<User>();
            userDataBase = OnSearchUserButton_Click();
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            User logInUser = OnLoginButtonClicked(UserLogInTextBox.Text, PasswordLogInTextBox.Text);

            if (SearchSearchResultsDomainUp.Text.Contains("User:"))
            {
                foreach (User searchedUser in userDataBase)
                {
                    string result = SearchSearchResultsDomainUp.Text;
                    List<string> listuser = searchedUser.FollowingList;
                    if (result == "User: " + searchedUser.SearchedInfoUser())
                    {
                        OnSearchUserFollowButton_Click(logInUser, searchedUser, profile);
                    }
                }
            }
            SearUserName.Clear();
            SearcUserEmailTextBox.Clear();
            SearchUserFollowers.Clear();
            SearchUserFollowing.Clear();
        }
        //ONEVENT

        public void OnSearchUserFollowButton_Click(User userLogIn, User userSearched, Profile profilesearched)
        {
            if (SearchFollowButton_Clicked != null)
            {
                string result = SearchFollowButton_Clicked(this, new UserEventArgs() { UserLogIn = userLogIn, UserSearched = userSearched, ProfileUserLogIn = profilesearched });
                if (result != null)
                {
                    //Un label que appende el result...
                    SearchInvalidCredentialsTextBox.AppendText(result);
                    Thread.Sleep(2000);
                    DisplayStartPanel.BringToFront();
                    SearchInvalidCredentialsTextBox.Clear();
                }
                else
                {
                    //Un label que appende un error...
                    SearchInvalidCredentialsTextBox.AppendText("Error... couldn't follow " + userSearched.Username);
                    Thread.Sleep(2000);
                    DisplayStartPanel.BringToFront();
                    SearchInvalidCredentialsTextBox.Clear();
                }
            }
            else
            {
                SearchInvalidCredentialsTextBox.AppendText("Error... couldn't follow " + userSearched.Username);
                Thread.Sleep(2000);
                DisplayStartPanel.BringToFront();
                SearchInvalidCredentialsTextBox.Clear();
            }
        }

        //GO BACK/CLOSE

        private void SearchUserGoBack_Click(object sender, EventArgs e)
        {
            SearchPanel.BringToFront();
            SearUserName.Clear();
            SearcUserEmailTextBox.Clear();
            SearchUserFollowers.Clear();
            SearchUserFollowing.Clear();
        }
        //-------------------------------------------------------------------------------------------




        /*---------------------------------------------------!PY----------------------------------------------------- */

        //<<!PLAY SONG PANEL>>
        //-------------------------------------------------------------------------------------------
        private void PlaySongStopButton_Click(object sender, EventArgs e)
        {
            soundPlayer.Stop();
            List<Song> songDataBase = new List<Song>();
            songDataBase = OnSearchSongButton_Click();
            foreach (Song song in songDataBase)
            {
                if (song.Format == ".mp3")
                {
                    windowsMediaPlayer.controls.pause();
                    DurationTimer.Stop();
                    break;
                }
                else if (song.Format == ".wav")
                {
                    soundPlayer.Stop();
                    DurationTimer.Stop();
                    break;
                }
            }
        }

        private void PlaySongPlayButton_Click_1(object sender, EventArgs e)
        {
            soundPlayer.Play();
            List<Song> songDataBase = new List<Song>();
            songDataBase = OnSearchSongButton_Click();
            foreach (Song song in songDataBase)
            {
                if (song.Format == ".mp3")
                {
                    windowsMediaPlayer.controls.play();
                    DurationTimer.Start();
                    break;
                }
                else if (song.Format == ".wav")
                {

                    DurationTimer.Start();
                }
            }
        }

        private void PlaySongAddQueueButton_Click(object sender, EventArgs e)
        {
            string[] searchedMult = SearchSearchResultsDomainUp.Text.Split(':');
            if (searchedMult[0].Contains("Song") == true)
            {
                List<string> songInfo = GetSongButton(searchedMult[1], searchedMult[3]);
                queueList.Add(songInfo[6]);
            }
            //Esto deberia ir cuando haya un panel con el botton de agregar a la cola en videos.
            else if (searchedMult[0].Contains("Video") == true)
            {
                List<string> videoInfo = GetVideoButton(searchedMult[1], searchedMult[2], searchedMult[3]);
                queueList.Add(videoInfo[8]);
            }

            //Agrega a la queue.
            //Falta ver cuando se le da play a queue, es decir, si es con un boton, con tiempo o algo. No se.
        }
        private void PlaySongAddToPlaylistButton_Click(object sender, EventArgs e)
        {
            PlaySongMessageTextBox.Clear();
            PlaySongChoosePlsDomainUp.ResetText();
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            if (profile.CreatedPlaylist.Count() != 0)
            {
                foreach (PlayList playList in profile.CreatedPlaylist)
                {
                    PlaySongChoosePlsDomainUp.Items.Add(playList.NamePlayList);
                }
                PlaySongChoosePlsButton.Visible = true;
                PlaySongChoosePlsDomainUp.Visible = true;
            }
            else
            {
                PlaySongMessageTextBox.AppendText("ERROR[!] You don´t have created Playlists");
            }
        }
        private void PlaySongChoosePlsButton_Click(object sender, EventArgs e)
        {
            PlaySongMessageTextBox.Clear();
            List<Song> songDataBase = new List<Song>();
            string result = SearchSearchResultsDomainUp.Text;
            string searchedPlaylistName = PlaySongChoosePlsDomainUp.Text;
            int choosenPl = PlaySongChoosePlsDomainUp.SelectedIndex;
            songDataBase = OnSearchSongButton_Click();
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            PlaySongChoosePlsButton_Click(songDataBase, profile, result, choosenPl, searchedPlaylistName);
            SearchSearchResultsDomainUp.ResetText();
        }


        private void PlaySongDownloadSongButton_Click(object sender, EventArgs e)
        {
            PlaySongMessageTextBox.Clear();
            List<string> listUser = OnLogInLogInButton_Clicked2(UserLogInTextBox.Text);
            if (listUser[3] != "standard")
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string destDirectory = desktopPath + "\\Downloaded-Songs-FyBuZz";
                string songFile;
                if (System.IO.Directory.Exists(destDirectory) == false)
                {
                    System.IO.Directory.CreateDirectory(destDirectory);
                    File.Create(destDirectory + "\\ FyBuZz.txt");
                    if (PlayerPlayingLabel.Text.Contains(".mp3") == true)
                    {
                        songFile = windowsMediaPlayer.URL.Split('\\')[windowsMediaPlayer.URL.Split('\\').Length - 1];
                        string destFile = destDirectory + "\\" + songFile;
                        File.Copy(windowsMediaPlayer.URL, destFile);
                    }
                    else
                    {
                        songFile = soundPlayer.SoundLocation.Split('\\')[soundPlayer.SoundLocation.Split('\\').Length - 1];
                        string destFile = destDirectory + "\\" + songFile;
                        File.Copy(soundPlayer.SoundLocation, destFile);
                    }
                }
                else
                {
                    if (PlayerPlayingLabel.Text.Contains(".mp3") == true)
                    {
                        songFile = windowsMediaPlayer.URL.Split('\\')[windowsMediaPlayer.URL.Split('\\').Length - 1];
                        string destFile = destDirectory + "\\" + songFile;
                        File.Copy(windowsMediaPlayer.URL, destFile);
                    }
                    else
                    {
                        songFile = soundPlayer.SoundLocation.Split('\\')[soundPlayer.SoundLocation.Split('\\').Length - 1];
                        string destFile = destDirectory + "\\" + songFile;
                        File.Copy(soundPlayer.SoundLocation, destFile);
                    }
                }
                PlaySongMessageTextBox.AppendText("Song downloaded succesfully.");
            }
            else
            {
                PlaySongMessageTextBox.Visible = true;
                PlaySongMessageTextBox.AppendText("Standard users can't download songs.");
            }

        }

        private void PlaySongChoosePlsButton_Click(List<Song> songDataBase, Profile profile, string result, int choosenPl, string searchedPL)
        {
            if (PlaySongChoosePlsButton_Clicked != null)
            {
                string final = PlaySongChoosePlsButton_Clicked(this, new PlaylistEventArgs() { RestultText = result, ChoosenIndex = choosenPl, SongDataBaseText = songDataBase, ProfileCreatorText = profile, SearchedPlaylistNameText = searchedPL });
                if (final == null)
                {
                    PlaySongMessageTextBox.AppendText("Song added succesfully.");
                    OnSearchUserButton_Click();

                }
                else
                {
                    PlaySongMessageTextBox.AppendText("ERROR[!] couldn't add song.");
                    Thread.Sleep(1000);
                    PlaySongMessageTextBox.Clear();

                }

            }
        }
        private void PlaySongPreviousSongButton_Click(object sender, EventArgs e)
        {
            string[] infoSong = SearchSearchResultsDomainUp.Text.Split(':');
            string nameSong = infoSong[1];
            string artistSong = infoSong[3];
            int previousSong = 1;

            Song songP = OnSkipOrPreviousSongButton_Clicked(nameSong, artistSong, previousSong, null);
            if (songP != null)
            {
                PlayerPlayingLabel.Clear();
                SearchPlayingLabel.Clear();
                if (songP.Format == ".mp3")
                {
                    windowsMediaPlayer.controls.stop();
                    soundPlayer.Stop();
                    windowsMediaPlayer.URL = songP.SongFile;
                    windowsMediaPlayer.controls.play();
                }
                else if (songP.Format == ".wav")
                {
                    windowsMediaPlayer.controls.stop();
                    soundPlayer.Stop();
                    soundPlayer.SoundLocation = songP.SongFile;
                    soundPlayer.Play();

                }
                PlayerPlayingLabel.AppendText("Song playing: " + songP.Name + songP.Format);
                SearchPlayingLabel.AppendText("Song playing: " + songP.Name + songP.Format);
                SearchSearchResultsDomainUp.UpButton();
            }
            else
            {
                PlayerPlayingLabel.Clear();
                PlayerPlayingLabel.AppendText("ERROR[!] ~Song wasn't previoused!");
            }



        }

        private void PlaySongSkipSongButton_Click(object sender, EventArgs e)
        {
            string[] infoSong = SearchSearchResultsDomainUp.Text.Split(':');
            string nameSong = infoSong[1];
            string artistSong = infoSong[3];
            int previousSong = 0;

            Song songS = OnSkipOrPreviousSongButton_Clicked(nameSong, artistSong, previousSong, null);

            if (songS != null)
            {

                PlayerPlayingLabel.Clear();
                SearchPlayingLabel.Clear();
                if (songS.Format == ".mp3")
                {
                    windowsMediaPlayer.controls.stop();
                    soundPlayer.Stop();
                    windowsMediaPlayer.URL = songS.SongFile;
                    windowsMediaPlayer.controls.play();
                }
                else if (songS.Format == ".wav")
                {
                    windowsMediaPlayer.controls.stop();
                    soundPlayer.Stop();
                    soundPlayer.SoundLocation = songS.SongFile;
                    soundPlayer.Play();
                }

                PlayerPlayingLabel.AppendText("Song playing: " + songS.Name + songS.Format);
                SearchPlayingLabel.AppendText("Song playing: " + songS.Name + songS.Format);
                SearchSearchResultsDomainUp.DownButton();
            }
            else
            {
                PlayerPlayingLabel.Clear();
                PlayerPlayingLabel.AppendText("ERROR[!] ~Song wasn't skipped!");
            }

        }


        //!SHOW SONG LYRICS
        private void PlaySongShowLyrics_Click(object sender, EventArgs e)
        {
            PlaySongDisplayLyrics.Visible = false;
            PlaySongDisplayLyrics.Clear();
            string[] searchedSong = SearchSearchResultsDomainUp.Text.Split(':');
            List<string> infoSong = GetSongButton(searchedSong[1], searchedSong[3]);

            string strRegex = @"^.*([a-zA-Z]).*$";
            Regex myRegex = new Regex(strRegex, RegexOptions.Multiline);
            if (infoSong[5].Contains(".srt") && infoSong[5] != null)
            {
                string lyricsFile = File.ReadAllText(infoSong[5]);

                foreach (Match myMatch in myRegex.Matches(lyricsFile))
                {
                    if (myMatch.Success)
                    {
                        PlaySongDisplayLyrics.Visible = true;
                        PlaySongDisplayLyrics.AppendText(myMatch.Value + "\n");
                    }
                }
            }
            else
            {
                PlaySongMessageTextBox.AppendText("ERROR[!] Couldn't find lyrics");
                Thread.Sleep(2000);
                PlaySongMessageTextBox.Clear();
            }

        }
        //Rate Song
        private void PlaysSongRateButton_Click(object sender, EventArgs e)
        {
            PlaySongRateMessageTextBox.Clear();
            PlaySongRateNumDomainUp.Visible = true;
            int userRate = (int)PlaySongRateNumDomainUp.Value;
            string[] infoSong = SearchSearchResultsDomainUp.Text.Split(':');
            PlaysSongRateButton_Click(userRate, infoSong[1], infoSong[3]);
            List<string> infoSongList = GetSongButton(infoSong[1], infoSong[3]);
            PlaySongRateMessageTextBox.AppendText(infoSongList[7]);
        }
        //ONEVENT

        public Song OnSkipOrPreviousSongButton_Clicked(string nameSong, string ArtistSong, int skipOrPreviousSong, PlayList playlist)
        {
            if (SkipOrPreviousSongButton_Clicked != null)
            {
                Song song = SkipOrPreviousSongButton_Clicked(this, new SongEventArgs() { NameText = nameSong, ArtistText = ArtistSong, SkipOrPrevious = skipOrPreviousSong, playlistSong = playlist });
                PlayerPlayingLabel.Clear();
                if (song != null)
                {
                    if (skipOrPreviousSong == 0)
                    {
                        PlayerPlayingLabel.AppendText("song has been Skipped!");
                        return song;
                    }
                    else
                    {
                        PlayerPlayingLabel.AppendText("song has benn previoused!");
                        return song;
                    }
                }
                else
                {
                    if (skipOrPreviousSong == 0)
                    {
                        PlayerPlayingLabel.AppendText("ERROR[!] ~couldn't skip song!");

                    }
                    else
                    {
                        PlayerPlayingLabel.AppendText("ERROR[!] ~couldn't previous song!");

                    }
                    return null;
                }

            }
            else
            {
                PlayerPlayingLabel.Clear();
                PlayerPlayingLabel.AppendText("ERROR[!] ~Something went horribly wrong!");
            }
            return null;
        }

        public void PlaysSongRateButton_Click(int rated, string sName, string sArtist)
        {
            if (PlaysSongRateButton_Clicked != null)
            {
                string result = PlaysSongRateButton_Clicked(this, new SongEventArgs() { RankingText = rated, NameText = sName, ArtistText = sArtist });
                if (result != null)
                {
                    PlaySongMessageTextBox.Clear();
                    PlaySongMessageTextBox.AppendText(result);
                }
                else
                {
                    PlaySongMessageTextBox.Clear();
                    PlaySongMessageTextBox.AppendText("ERROR[!] couldn't rate song");
                }
            }
        }

        //GO BACK/CLOSE

        private void PlaySongGoBackButton_Click(object sender, EventArgs e)
        {
            PlaySongRateMessageTextBox.Clear();
            SearchPlayingLabel.Text = PlayerPlayingLabel.Text;
            SearchSearchTextBox.Text = "Search Songs,Video, Playlists or Users";
            SearchSearchResultsDomainUp.Visible = false;
            SearchPlayingPanel.Visible = true;
            //SearchSearchResultsDomainUp.Items.Clear();
            SearchSearchResultsDomainUp.Text = "Searched Results:";
            SearchPanel.BringToFront();
            SearchSearchResultsDomainUp.ResetText();
            int cont = 0;
            foreach (object searched in SearchSearchResultsDomainUp.Items)
            {
                cont++;
            }
            for (int i = 0; i < cont; cont--)
            {
                SearchSearchResultsDomainUp.Items.RemoveAt(cont - 1);
            }
        }
        //-------------------------------------------------------------------------------------------




        //<<!PLAY VIDEO PANEL>>
        //-------------------------------------------------------------------------------------------
        private void PlayVideoAddToPlaylistButton_Click(object sender, EventArgs e)
        {
            PlayVideoSelectPlDomainUp.ResetText();

            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);

            if (profile.CreatedPlaylist.Count() != 0)
            {
                foreach (PlayList playlist in profile.CreatedPlaylist)
                {
                    PlayVideoSelectPlDomainUp.Items.Add(playlist.NamePlayList);
                }
                PlayVideoSelectPlDomainUp.Visible = true;
                PlayVideoSelectPlButton.Visible = true;
            }
            else
            {

            }
        }

        private void PlayVideoSelectPlButton_Click(object sender, EventArgs e)
        {
            List<Video> videoDataBase = new List<Video>();
            string result = SearchSearchResultsDomainUp.Text;
            string searchedPlaylistName = PlayVideoSelectPlDomainUp.Text;
            videoDataBase = OnSearchVideoButton_Click();
            OnPlayVideoSelectPlButton_Clicked(result, videoDataBase, searchedPlaylistName);
            SearchSearchResultsDomainUp.ResetText();

        }

        private void PlayVideoRateVideoButton_Click(object sender, EventArgs e)
        {
            PlayVideoRateDomainUp.Visible = true;
            int userRate = (int)PlayVideoRateDomainUp.Value;
            string[] infoVideo = SearchSearchResultsDomainUp.Text.Split(':');
            PlaysVideoRateButton_Click(userRate, infoVideo[1], infoVideo[3], infoVideo[5]);
        }

        private void PlayVideoGoBackButton_Click(object sender, EventArgs e)
        {
            wmpVideo.Ctlcontrols.stop();
            SearchPanel.BringToFront();
        }



        private void PlayVideoPreviousButton_Click(object sender, EventArgs e)
        {
            string[] infoVideo = SearchSearchResultsDomainUp.Text.Split(':');
            string nameVideo = infoVideo[1];
            string nameActor = infoVideo[3];
            int previous = 1;

            Video video = OnSkipOrPreviousVideoButton_Click(nameVideo, nameActor, previous, null);

            if (video != null)
            {
                PlayVideoMessageAlertTextBox.Clear();

                wmpVideo.Ctlcontrols.stop();
                wmpVideo.URL = video.FileName;
                wmpVideo.Ctlcontrols.play();

                PlayVideoMessageAlertTextBox.AppendText("Video Playing: " + video.Name + video.Format);
            }
            else
            {
                PlayVideoMessageAlertTextBox.Clear();
                PlayVideoMessageAlertTextBox.AppendText("Video wasn't previoused!");
            }
        }

        private void PlayVideoSkipButton_Click(object sender, EventArgs e)
        {
            string[] infoVideo = SearchSearchResultsDomainUp.Text.Split(':');
            string nameVideo = infoVideo[1];
            string nameActor = infoVideo[3];
            int previous = 0;

            Video video = OnSkipOrPreviousVideoButton_Click(nameVideo, nameActor, previous, null);

            if (video != null)
            {
                PlayVideoMessageAlertTextBox.Clear();

                wmpVideo.Ctlcontrols.stop();
                wmpVideo.URL = video.FileName;
                wmpVideo.Ctlcontrols.play();

                PlayVideoMessageAlertTextBox.AppendText("Video Playing: " + video.Name + video.Format);
            }
            else
            {
                PlayVideoMessageAlertTextBox.Clear();
                PlayVideoMessageAlertTextBox.AppendText("Video wasn't Skipped!");
            }
        }
        //ONEVENT

        public void OnPlayVideoSelectPlButton_Clicked(string result, List<Video> videoDataBase, string searchedPl)
        {
            if (PlayVideoSelectPlButton_Clicked != null)
            {
                string description = PlayVideoSelectPlButton_Clicked(this, new PlaylistEventArgs() { RestultText = result, videoDataBaseText = videoDataBase, SearchedPlaylistNameText = searchedPl });
                if (description == null)
                {
                    PlayVideoMessageAlertTextBox.AppendText("Video added successfully!");
                    OnSearchUserButton_Click();
                    Thread.Sleep(500);
                    PlayVideoMessageAlertTextBox.Clear();
                }
                else
                {
                    PlayVideoMessageAlertTextBox.AppendText("ERROR[!] ~ Couldn't add video");
                    Thread.Sleep(500);
                    PlayVideoMessageAlertTextBox.Clear();
                }
            }
        }


        public Video OnSkipOrPreviousVideoButton_Click(string nameVideo, string nameActor, int skipOrPrevious, PlayList playlist)
        {
            if (SkipOrPreviousVideoButton_Clicked != null)
            {
                Video video = SkipOrPreviousVideoButton_Clicked(this, new VideoEventArgs() { NameText = nameVideo, ActorsText = nameActor, previousOrSkip = skipOrPrevious, playlistVideo = playlist });
                PlayVideoMessageAlertTextBox.Clear();
                if (video != null)
                {
                    if (skipOrPrevious == 0)
                    {
                        PlayVideoMessageAlertTextBox.AppendText("Video has been skipped!");
                    }
                    else
                    {
                        PlayVideoMessageAlertTextBox.AppendText("Video has been previoused!");
                    }
                    return video;
                }
                else
                {
                    if (skipOrPrevious == 0)
                    {
                        PlayVideoMessageAlertTextBox.AppendText("ERROR[!] ~ Video couldn't be skipped!");
                    }
                    else
                    {
                        PlayVideoMessageAlertTextBox.AppendText("ERROR[!] ~ Video couldn't be previoused!");
                    }
                    return null;
                }
            }
            else
            {
                PlayVideoMessageAlertTextBox.Clear();
                PlayVideoMessageAlertTextBox.AppendText("ERROR[!] ~Something went horribly wrong!");
                return null;
            }
        }

        public void PlaysVideoRateButton_Click(int rated, string vName, string vActors, string vDirectors)
        {
            if (PlaysVideoRateButton_Clicked != null)
            {
                string result = PlaysVideoRateButton_Clicked(this, new VideoEventArgs() { RankingText = rated, NameText = vName, ActorsText = vActors, DirectorsText = vDirectors });
                if (result != null)
                {
                    PlayVideoMessageLabel.Clear();
                    PlayVideoMessageLabel.AppendText(result);
                }
                else
                {
                    PlayVideoMessageLabel.Clear();
                    PlayVideoMessageLabel.AppendText("ERROR[!] couldn't rate song");
                }
            }
        }

        //GO BACK/CLOSE
        //-------------------------------------------------------------------------------------------




        //<<!PLAY PLAYLIST PANEL>>
        //-------------------------------------------------------------------------------------------


        private void PlayPlaylistChooseMultimediaButton_Click(object sender, EventArgs e)
        {
            soundPlayer.Stop();
            windowsMediaPlayer.controls.stop();
            PlayPlaylistProgressBarBox.Value = 0;
            PlayPlaylistTimerBox.Clear();
            soundPlayer.Stop();
            windowsMediaPlayer.controls.pause();
            PlayPlaylistMessageBox.Clear();
            PlayPlaylistPlayerPanel.Visible = true;
            List<Song> songDataBase = new List<Song>();
            List<PlayList> playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();
            PlayList choosenPL = null;
            List<Video> videoDataBase = OnSearchVideoButton_Click();
            songDataBase = OnSearchSongButton_Click();
            string searched = SearchSearchResultsDomainUp.Text;
            string multimediaType = PlayPlaylistShowMultimedia.Text;
            List<string> userInfo = OnLogInLogInButton_Clicked2(UserLogInTextBox.Text);
            if (userInfo[3] != "standard")
            {
                foreach (PlayList playList in playlistDataBase)
                {
                    if (searched.Contains(playList.NamePlayList) == true)
                    {
                        choosenPL = playList;
                    }
                }

                if (multimediaType.Contains("Song:") == true && multimediaType.Contains("Artist:") == true)
                {
                    List<string> choosenPLPers = ReturnSearchedMult(ProfileDomainUp.Text, "Song", null);
                    int playlistIndex = PlayPlaylistShowMultimedia.SelectedIndex;
                    if (choosenPL != null)
                    {
                        while (playlistIndex < choosenPL.Songs.Count())
                        {
                            if (choosenPL.Songs[playlistIndex].Format == ".mp3")
                            {
                                if (multimediaType == choosenPL.Songs[playlistIndex].SearchedInfoSong())
                                {
                                    PlayPlaylistMessageBox.Clear();
                                    PlaySongProgressBar.Value = 0;
                                    PlaySongTimerTextBox.Clear();
                                    windowsMediaPlayer.URL = choosenPL.Songs[playlistIndex].SongFile;
                                    DurationTimer.Interval = 1000;
                                    PlaySongProgressBar.Maximum = (int)(choosenPL.Songs[playlistIndex].Duration * 60);
                                    SearchProgressBar.Maximum = (int)(choosenPL.Songs[playlistIndex].Duration * 60);

                                    PlayPlaylistMessageBox.AppendText("Playlist playing: " + choosenPL.Songs[playlistIndex].Name + choosenPL.Songs[playlistIndex].Format);
                                    SearchPlayingLabel.AppendText("Playlist playing: " + choosenPL.Songs[playlistIndex].Name + choosenPL.Songs[playlistIndex].Format);
                                    DurationTimer.Start();
                                    windowsMediaPlayer.controls.play();
                                }
                                if (playlistIndex == choosenPL.Songs.Count())
                                {
                                    if (PlayPlaylistLoopCheckBox.Checked == true)
                                    {
                                        playlistIndex = 0;
                                    }
                                }
                                playlistIndex++;
                            }
                            else if (choosenPL.Songs[playlistIndex].Format == ".wav")
                            {
                                if (multimediaType == choosenPL.Songs[playlistIndex].SearchedInfoSong())
                                {

                                    PlayPlaylistMessageBox.Clear();
                                    PlaySongProgressBar.Value = 0;
                                    PlaySongTimerTextBox.ResetText();
                                    soundPlayer.SoundLocation = choosenPL.Songs[playlistIndex].SongFile;
                                    soundPlayer.Play();
                                    DurationTimer.Interval = 1000;
                                    PlaySongProgressBar.Maximum = (int)(choosenPL.Songs[playlistIndex].Duration * 60);
                                    SearchProgressBar.Maximum = (int)(choosenPL.Songs[playlistIndex].Duration * 60);

                                    PlayPlaylistMessageBox.AppendText("Playlist playing: " + choosenPL.Songs[playlistIndex].Name + choosenPL.Songs[playlistIndex].Format);
                                    SearchPlayingLabel.AppendText("Playlist playing: " + choosenPL.Songs[playlistIndex].Name + choosenPL.Songs[playlistIndex].Format);
                                    DurationTimer.Start();


                                }
                                if (playlistIndex == choosenPL.Songs.Count())
                                {
                                    if (PlayPlaylistLoopCheckBox.Checked == true)
                                    {
                                        playlistIndex = 0;

                                    }
                                }
                            }
                            playlistIndex++;
                        }
                    }
                    else //Cuando entro de displayPlaylist
                    {
                        foreach (Song song in songDataBase)
                        {
                            if (choosenPLPers[playlistIndex].Contains(song.SongFile) == true)
                            {
                                if (song.SongFile.Contains(".mp3"))
                                {
                                    PlayPlaylistMessageBox.Clear();
                                    PlaySongProgressBar.Value = 0;
                                    PlaySongTimerTextBox.Clear();

                                    windowsMediaPlayer.URL = song.SongFile;
                                    DurationTimer.Interval = 1000;
                                    PlaySongProgressBar.Maximum = (int)(song.Duration * 60);
                                    SearchProgressBar.Maximum = (int)(song.Duration * 60);

                                    PlayPlaylistMessageBox.AppendText("Playlist playing: " + song.Name + song.Format);
                                    SearchPlayingLabel.AppendText("Playlist playing: " + song.Name + song.Format);
                                    DurationTimer.Start();
                                    windowsMediaPlayer.controls.play();
                                    break;
                                }
                                else if (song.SongFile.Contains(".wav"))
                                {
                                    PlayPlaylistMessageBox.Clear();
                                    PlaySongProgressBar.Value = 0;
                                    PlaySongTimerTextBox.ResetText();
                                    string file = song.SongFile;
                                    soundPlayer.SoundLocation = file;
                                    soundPlayer.Play();
                                    DurationTimer.Interval = 1000;
                                    PlaySongProgressBar.Maximum = (int)(song.Duration * 60);
                                    SearchProgressBar.Maximum = (int)(song.Duration * 60);

                                    PlayPlaylistMessageBox.AppendText("Playlist playing: " + song.Name + song.Format);
                                    SearchPlayingLabel.AppendText("Playlist playing: " + song.Name + song.Format);
                                    DurationTimer.Start();
                                    break;
                                }
                                playlistIndex++;
                            }

                        }

                    }
                }
                else if (multimediaType.Contains("Video:") == true && multimediaType.Contains("Actors:") == true)
                {
                    List<string> choosenPLPers = ReturnSearchedMult(ProfileDomainUp.Text, null, "Video");
                    int playlistIndex = PlayPlaylistShowMultimedia.SelectedIndex;
                    if (choosenPL != null)
                    {
                        while (playlistIndex < choosenPL.Videos.Count())
                        {
                            if (choosenPL.Videos[playlistIndex].Format == ".mp4")
                            {
                                if (multimediaType == choosenPL.Videos[playlistIndex].SearchedInfoVideo())
                                {
                                    wmpVideo.Ctlcontrols.stop();
                                    PlayPlaylistMessageBox.Clear();
                                    PlaySongTimerTextBox.Clear();
                                    wmpVideo.URL = choosenPL.Videos[playlistIndex].FileName;
                                    PlayVideoPanel.BringToFront();
                                    wmpVideo.Ctlcontrols.play();
                                    break;
                                }
                                if (playlistIndex == choosenPL.Videos.Count())
                                {
                                    if (PlayPlaylistLoopCheckBox.Checked == true)
                                    {
                                        playlistIndex = 0;
                                    }
                                }
                                playlistIndex++;
                            }
                            else if (choosenPL.Videos[playlistIndex].Format == ".mov")
                            {
                                if (multimediaType == choosenPL.Videos[playlistIndex].SearchedInfoVideo())
                                {

                                    wmpVideo.Ctlcontrols.stop();
                                    PlayPlaylistMessageBox.Clear();
                                    PlaySongTimerTextBox.Clear();
                                    wmpVideo.URL = choosenPL.Videos[playlistIndex].FileName;

                                    PlayVideoPanel.BringToFront();
                                    wmpVideo.Ctlcontrols.play();
                                    break;


                                }
                                if (playlistIndex == choosenPL.Videos.Count())
                                {
                                    if (PlayPlaylistLoopCheckBox.Checked == true)
                                    {
                                        playlistIndex = 0;

                                    }
                                }
                                playlistIndex++;
                            }
                            else if (choosenPL.Videos[playlistIndex].Format == ".avi")
                            {
                                if (multimediaType == choosenPL.Videos[playlistIndex].SearchedInfoVideo())
                                {
                                    wmpVideo.Ctlcontrols.stop();
                                    PlayPlaylistMessageBox.Clear();
                                    PlaySongTimerTextBox.Clear();
                                    wmpVideo.URL = choosenPL.Videos[playlistIndex].FileName;

                                    PlayVideoPanel.BringToFront();
                                    wmpVideo.Ctlcontrols.play();
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (Video video in videoDataBase)
                        {
                            if (choosenPLPers[playlistIndex].Contains(video.FileName) == true)
                            {
                                if (video.FileName.Contains(".mp4"))
                                {
                                    wmpVideo.Ctlcontrols.stop();
                                    PlayPlaylistMessageBox.Clear();
                                    PlaySongTimerTextBox.Clear();
                                    wmpVideo.URL = choosenPL.Videos[playlistIndex].FileName;

                                    PlayPlaylistMessageBox.AppendText("Playlist playing: " + choosenPL.Videos[playlistIndex].Name + choosenPL.Videos[playlistIndex].Format);
                                    SearchPlayingLabel.AppendText("Playlist playing: " + choosenPL.Videos[playlistIndex].Name + choosenPL.Videos[playlistIndex].Format);
                                    PlayVideoPanel.BringToFront();
                                    wmpVideo.Ctlcontrols.play();
                                    break;
                                }
                                else if (video.FileName.Contains(".mov"))
                                {
                                    wmpVideo.Ctlcontrols.stop();
                                    PlayPlaylistMessageBox.Clear();
                                    PlaySongTimerTextBox.Clear();
                                    wmpVideo.URL = choosenPL.Videos[playlistIndex].FileName;

                                    PlayPlaylistMessageBox.AppendText("Playlist playing: " + choosenPL.Videos[playlistIndex].Name + choosenPL.Videos[playlistIndex].Format);
                                    SearchPlayingLabel.AppendText("Playlist playing: " + choosenPL.Videos[playlistIndex].Name + choosenPL.Videos[playlistIndex].Format);
                                    PlayVideoPanel.BringToFront();
                                    wmpVideo.Ctlcontrols.play();
                                    break;
                                }
                                else if (video.FileName.Contains(".avi"))
                                {
                                    wmpVideo.Ctlcontrols.stop();
                                    PlayPlaylistMessageBox.Clear();
                                    PlaySongTimerTextBox.Clear();
                                    wmpVideo.URL = choosenPL.Videos[playlistIndex].FileName;

                                    PlayPlaylistMessageBox.AppendText("Playlist playing: " + choosenPL.Videos[playlistIndex].Name + choosenPL.Videos[playlistIndex].Format);
                                    SearchPlayingLabel.AppendText("Playlist playing: " + choosenPL.Videos[playlistIndex].Name + choosenPL.Videos[playlistIndex].Format);
                                    PlayVideoPanel.BringToFront();
                                    wmpVideo.Ctlcontrols.play();
                                    break;
                                }
                                playlistIndex++;
                            }

                        }
                    }
                }
            }
            else
            {
                PlayPlaylistMessageBox.AppendText("Standard users can't choose multimedia from a Playlist.");
            }
        }
        private void PlayPlaylistRandomButton_Click(object sender, EventArgs e)
        {
            PlayPlaylistProgressBarBox.Value = 0;
            PlayPlaylistTimerBox.Clear();
            soundPlayer.Stop();
            windowsMediaPlayer.controls.stop();
            PlayPlaylistMessageBox.Clear();
            PlayPlaylistPlayerPanel.Visible = true;
            List<Song> songDataBase = new List<Song>();
            List<PlayList> playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();
            PlayList choosenPL = null;
            songDataBase = OnSearchSongButton_Click();
            List<Video> videoDataBase = OnSearchVideoButton_Click();
            string searched = SearchSearchResultsDomainUp.Text;
            string multimediaType = PlayPlaylistShowMultimedia.Text;
            Random random = new Random();
            foreach (PlayList playList in playlistDataBase)
            {
                if (searched.Contains(playList.NamePlayList) == true)
                {
                    choosenPL = playList;
                }
            }
            if (choosenPL != null && choosenPL.Songs.Count() != 0)
            {
                int playlistIndex = random.Next(choosenPL.Songs.Count());
                if (choosenPL.Songs[playlistIndex].Format == ".mp3")
                {
                    PlayPlaylistMessageBox.Clear();
                    PlaySongProgressBar.Value = 0;
                    PlaySongTimerTextBox.Clear();
                    PlayPlaylistProgressBarBox.Value = 0;

                    windowsMediaPlayer.URL = choosenPL.Songs[playlistIndex].SongFile;
                    DurationTimer.Interval = 1000;
                    PlaySongProgressBar.Maximum = (int)(choosenPL.Songs[playlistIndex].Duration * 60);
                    SearchProgressBar.Maximum = (int)(choosenPL.Songs[playlistIndex].Duration * 60);
                    PlayPlaylistProgressBarBox.Maximum = (int)(choosenPL.Songs[playlistIndex].Duration * 60);

                    PlayPlaylistMessageBox.AppendText("Playlist playing: " + choosenPL.Songs[playlistIndex].Name + choosenPL.Songs[playlistIndex].Format);
                    SearchPlayingLabel.AppendText("Playlist playing: " + choosenPL.Songs[playlistIndex].Name + choosenPL.Songs[playlistIndex].Format);
                    DurationTimer.Start();
                    windowsMediaPlayer.controls.play();
                }
                else if (choosenPL.Songs[playlistIndex].Format == ".wav")
                {
                    PlayPlaylistMessageBox.Clear();
                    PlaySongProgressBar.Value = 0;
                    PlaySongTimerTextBox.ResetText();
                    soundPlayer.SoundLocation = choosenPL.Songs[playlistIndex].SongFile;
                    soundPlayer.Play();
                    DurationTimer.Interval = 1000;
                    PlaySongProgressBar.Maximum = (int)(choosenPL.Songs[playlistIndex].Duration * 60);
                    SearchProgressBar.Maximum = (int)(choosenPL.Songs[playlistIndex].Duration * 60);

                    PlayPlaylistMessageBox.AppendText("Playlist playing: " + choosenPL.Songs[playlistIndex].Name + choosenPL.Songs[playlistIndex].Format);
                    SearchPlayingLabel.AppendText("Playlist playing: " + choosenPL.Songs[playlistIndex].Name + choosenPL.Songs[playlistIndex].Format);
                    DurationTimer.Start();

                }
            }
            else if (choosenPL == null)
            {
                soundPlayer.Stop();
                windowsMediaPlayer.controls.stop();
                List<string> choosenPLPers = ReturnSearchedMult(ProfileDomainUp.Text, "Song", null);
                int playlistIndex = random.Next(choosenPLPers.Count());
                if (PlayPlaylistMultTypeTextBox.Text == "Song")
                {
                    foreach (Song song in songDataBase)
                    {
                        if (choosenPLPers[playlistIndex].Contains(song.SongFile) == true)
                        {
                            if (song.SongFile.Contains(".mp3"))
                            {
                                PlayPlaylistMessageBox.Clear();
                                PlaySongProgressBar.Value = 0;
                                PlaySongTimerTextBox.Clear();

                                windowsMediaPlayer.URL = song.SongFile;
                                DurationTimer.Interval = 1000;
                                PlaySongProgressBar.Maximum = (int)(song.Duration * 60);
                                SearchProgressBar.Maximum = (int)(song.Duration * 60);

                                PlayPlaylistMessageBox.AppendText("Playlist playing: " + song.Name + song.Format);
                                SearchPlayingLabel.AppendText("Playlist playing: " + song.Name + song.Format);
                                DurationTimer.Start();
                                windowsMediaPlayer.controls.play();
                                break;
                            }
                            else if (song.SongFile.Contains(".wav"))
                            {
                                PlayPlaylistMessageBox.Clear();
                                PlaySongProgressBar.Value = 0;
                                PlaySongTimerTextBox.ResetText();
                                string file = song.SongFile;
                                soundPlayer.SoundLocation = file;
                                soundPlayer.Play();
                                DurationTimer.Interval = 1000;
                                PlaySongProgressBar.Maximum = (int)(song.Duration * 60);
                                SearchProgressBar.Maximum = (int)(song.Duration * 60);

                                PlayPlaylistMessageBox.AppendText("Playlist playing: " + song.Name + song.Format);
                                SearchPlayingLabel.AppendText("Playlist playing: " + song.Name + song.Format);
                                DurationTimer.Start();
                                break;
                            }
                            playlistIndex++;
                        }

                    }
                }
                if (PlayPlaylistMultTypeTextBox.Text == "Video")
                {
                    foreach (Video video in videoDataBase)
                    {
                        if (choosenPLPers[playlistIndex].Contains(video.FileName) == true)
                        {
                            if (video.FileName.Contains(".mp4"))
                            {
                                wmpVideo.Ctlcontrols.stop();
                                PlayPlaylistMessageBox.Clear();
                                PlaySongTimerTextBox.Clear();
                                wmpVideo.URL = choosenPL.Videos[playlistIndex].FileName;

                                PlayPlaylistMessageBox.AppendText("Playlist playing: " + choosenPL.Videos[playlistIndex].Name + choosenPL.Videos[playlistIndex].Format);
                                SearchPlayingLabel.AppendText("Playlist playing: " + choosenPL.Videos[playlistIndex].Name + choosenPL.Videos[playlistIndex].Format);
                                PlayVideoPanel.BringToFront();
                                wmpVideo.Ctlcontrols.play();
                                break;
                            }
                            else if (video.FileName.Contains(".mov"))
                            {
                                wmpVideo.Ctlcontrols.stop();
                                PlayPlaylistMessageBox.Clear();
                                PlaySongTimerTextBox.Clear();
                                wmpVideo.URL = choosenPL.Videos[playlistIndex].FileName;

                                PlayPlaylistMessageBox.AppendText("Playlist playing: " + choosenPL.Videos[playlistIndex].Name + choosenPL.Videos[playlistIndex].Format);
                                SearchPlayingLabel.AppendText("Playlist playing: " + choosenPL.Videos[playlistIndex].Name + choosenPL.Videos[playlistIndex].Format);
                                PlayVideoPanel.BringToFront();
                                wmpVideo.Ctlcontrols.play();
                                break;
                            }
                            else if (video.FileName.Contains(".avi"))
                            {
                                wmpVideo.Ctlcontrols.stop();
                                PlayPlaylistMessageBox.Clear();
                                PlaySongTimerTextBox.Clear();
                                wmpVideo.URL = choosenPL.Videos[playlistIndex].FileName;

                                PlayPlaylistMessageBox.AppendText("Playlist playing: " + choosenPL.Videos[playlistIndex].Name + choosenPL.Videos[playlistIndex].Format);
                                SearchPlayingLabel.AppendText("Playlist playing: " + choosenPL.Videos[playlistIndex].Name + choosenPL.Videos[playlistIndex].Format);
                                PlayVideoPanel.BringToFront();
                                wmpVideo.Ctlcontrols.play();
                                break;
                            }
                            playlistIndex++;
                        }
                    }
                }
            }
            else if (choosenPL != null && choosenPL.Videos.Count() != 0)
            {
                int playlistIndex = random.Next(choosenPL.Videos.Count());
                if (choosenPL.Videos[playlistIndex].Format == ".mp4")
                {
                    wmpVideo.Ctlcontrols.stop();
                    wmpVideo.URL = choosenPL.Videos[playlistIndex].FileName;
                    PlayVideoPanel.BringToFront();
                    wmpVideo.Ctlcontrols.play();

                }
                else if (choosenPL.Videos[playlistIndex].Format == ".mov")
                {

                    wmpVideo.Ctlcontrols.stop();
                    wmpVideo.URL = choosenPL.Videos[playlistIndex].FileName;
                    PlayVideoPanel.BringToFront();
                    wmpVideo.Ctlcontrols.play();

                }
                else if (choosenPL.Videos[playlistIndex].Format == ".avi")
                {
                    wmpVideo.Ctlcontrols.stop();
                    wmpVideo.URL = choosenPL.Videos[playlistIndex].FileName;
                    PlayVideoPanel.BringToFront();
                    wmpVideo.Ctlcontrols.play();
                }
            }
        }
        private void PlayPlaylistPlayButton_Click(object sender, EventArgs e)
        {
            soundPlayer.Play();
            List<Song> songDataBase = new List<Song>();
            songDataBase = OnSearchSongButton_Click();
            string ex = PlayPlaylistShowMultimedia.Text;
            foreach (Song song in songDataBase)
            {
                if (PlayPlaylistShowMultimedia.Text.Contains("Song:") && song.Format == ".mp3")
                {
                    windowsMediaPlayer.controls.play();
                    DurationTimer.Start();
                    break;
                }
                else if (PlayPlaylistShowMultimedia.Text.Contains("Song:") && song.Format == ".wav")
                {
                    DurationTimer.Start();
                }
            }
        }

        private void PlayPlaylistPauseButton_Click(object sender, EventArgs e)
        {
            soundPlayer.Stop();
            List<Song> songDataBase = new List<Song>();
            songDataBase = OnSearchSongButton_Click();
            string ex = PlayPlaylistShowMultimedia.Text;
            foreach (Song song in songDataBase)
            {
                if (PlayPlaylistShowMultimedia.Text.Contains("Song:") && song.Format == ".mp3")
                {
                    windowsMediaPlayer.controls.pause();
                    DurationTimer.Stop();
                    break;
                }
                else if (PlayPlaylistShowMultimedia.Text.Contains("Song:") && song.Format == ".wav")
                {
                    DurationTimer.Stop();
                }
            }
        }

        private void PlayPlaylistPreviousButton_Click(object sender, EventArgs e)
        {
            string[] infoPlName = PlayPlaylistLabel.Text.Split(':');
            string plName = infoPlName[1];
            int indexPl = 0;
            List<PlayList> allPl = OnDisplayPlaylistsGlobalPlaylist_Click();

            for (int i = 0; i < allPl.Count(); i++)
            {
                if (plName.Contains(allPl[i].NamePlayList))
                {
                    indexPl = i;
                    break;
                }
            }

            if (allPl[indexPl].Format == ".mp3" || allPl[indexPl].Format == ".wav")
            {
                string[] infoSong = PlayPlaylistShowMultimedia.Text.Split(':');
                string nameSong = infoSong[1];
                string artistSong = infoSong[3];
                int previousSong = 1;

                Song songP = OnSkipOrPreviousSongButton_Clicked(nameSong, artistSong, previousSong, allPl[indexPl]);
                if (songP != null)
                {

                    PlayPlaylistMessageBox.Clear();
                    if (songP.Format == ".mp3")
                    {
                        windowsMediaPlayer.controls.stop();
                        soundPlayer.Stop();
                        windowsMediaPlayer.URL = songP.SongFile;
                        windowsMediaPlayer.controls.play();
                    }
                    else if (songP.Format == ".wav")
                    {
                        windowsMediaPlayer.controls.stop();
                        soundPlayer.Stop();
                        soundPlayer.SoundLocation = songP.SongFile;
                        soundPlayer.Play();

                    }

                    PlayPlaylistMessageBox.AppendText("Song playing: " + songP.Name + songP.Format);
                    PlayPlaylistShowMultimedia.UpButton();
                }
                else
                {
                    PlayPlaylistMessageBox.Clear();
                    PlayPlaylistMessageBox.AppendText("ERROR[!] ~Song wasn't previoused!");
                }
            }
            else if (allPl[indexPl].Format == ".mp4" || allPl[indexPl].Format == ".avi" || allPl[indexPl].Format == ".mov")
            {
                string[] infoVideo = SearchSearchResultsDomainUp.Text.Split(':');
                string nameVideo = infoVideo[1];
                string nameActor = infoVideo[3];
                int previous = 1;

                Video video = OnSkipOrPreviousVideoButton_Click(nameVideo, nameActor, previous, null);

                if (video != null)
                {
                    PlayVideoMessageAlertTextBox.Clear();

                    wmpVideo.Ctlcontrols.stop();
                    wmpVideo.URL = video.FileName;
                    wmpVideo.Ctlcontrols.play();

                    PlayPlaylistMessageBox.AppendText("Video Playing: " + video.Name + video.Format);
                    PlayPlaylistShowMultimedia.UpButton();
                }
                else
                {
                    PlayPlaylistMessageBox.Clear();
                    PlayPlaylistMessageBox.AppendText("Video wasn't previoused!");
                }
            }




        }

        private void PlayPlaylistSkipButton_Click(object sender, EventArgs e)
        {
            string[] infoPlName = PlayPlaylistLabel.Text.Split(':');
            string plName = infoPlName[1];
            int indexPl = 0;
            List<PlayList> allPl = OnDisplayPlaylistsGlobalPlaylist_Click();

            for (int i = 0; i < allPl.Count(); i++)
            {
                if (plName.Contains(allPl[i].NamePlayList))
                {
                    indexPl = i;
                    break;
                }
            }

            if (allPl[indexPl].Format == ".mp3" || allPl[indexPl].Format == ".wav")
            {
                string[] infoSong = PlayPlaylistShowMultimedia.Text.Split(':');
                string nameSong = infoSong[1];
                string artistSong = infoSong[3];
                int skipSong = 0;

                Song songS = OnSkipOrPreviousSongButton_Clicked(nameSong, artistSong, skipSong, allPl[indexPl]);

                if (songS != null)
                {

                    PlayPlaylistMessageBox.Clear();
                    if (songS.Format == ".mp3")
                    {
                        windowsMediaPlayer.controls.stop();
                        soundPlayer.Stop();
                        windowsMediaPlayer.URL = songS.SongFile;
                        windowsMediaPlayer.controls.play();
                    }
                    else if (songS.Format == ".wav")
                    {
                        windowsMediaPlayer.controls.stop();
                        soundPlayer.Stop();
                        soundPlayer.SoundLocation = songS.SongFile;
                        soundPlayer.Play();
                    }

                    PlayPlaylistMessageBox.AppendText("Song playing: " + songS.Name + songS.Format);

                    PlayPlaylistShowMultimedia.DownButton();
                }
                else
                {
                    PlayPlaylistMessageBox.Clear();
                    PlayPlaylistMessageBox.AppendText("ERROR[!] ~Song wasn't skipped!");
                }
            }
            else if (allPl[indexPl].Format == ".mp4" || allPl[indexPl].Format == ".avi" || allPl[indexPl].Format == ".mov")
            {
                string[] infoVideo = SearchSearchResultsDomainUp.Text.Split(':');
                string nameVideo = infoVideo[1];
                string nameActor = infoVideo[3];
                int previous = 0;

                Video video = OnSkipOrPreviousVideoButton_Click(nameVideo, nameActor, previous, allPl[indexPl]);

                if (video != null)
                {
                    PlayPlaylistMessageBox.Clear();

                    wmpVideo.Ctlcontrols.stop();
                    wmpVideo.URL = video.FileName;
                    wmpVideo.Ctlcontrols.play();

                    PlayPlaylistMessageBox.AppendText("Video Playing: " + video.Name + video.Format);
                    PlayPlaylistShowMultimedia.DownButton();
                }
                else
                {
                    PlayPlaylistMessageBox.Clear();
                    PlayPlaylistMessageBox.AppendText("Video wasn't Skipped!");
                }
            }


        }
        //ONEVENT

        //GO BACK/CLOSE
        private void PlayPlaylistGoBackButton_Click(object sender, EventArgs e)
        {

            SearchSearchTextBox.Text = "Search Songs,Video, Playlists or Users";
            SearchSearchResultsDomainUp.Visible = false;
            SearchPlayingPanel.Visible = true;
            SearchSearchResultsDomainUp.Text = "Searched Results:";
            SearchPanel.BringToFront();
            SearchSearchResultsDomainUp.ResetText();
            //Borra el domain up de play playlist
            int cont1 = 0;
            foreach (object searched in PlayPlaylistShowMultimedia.Items)
            {
                cont1++;
            }
            for (int i = 0; i < cont1; cont1--)
            {
                PlayPlaylistShowMultimedia.Items.RemoveAt(cont1 - 1);
            }
            //Borra el domain up del search
            int cont = 0;
            foreach (object searched in SearchSearchResultsDomainUp.Items)
            {
                cont++;
            }
            for (int i = 0; i < cont; cont--)
            {
                SearchSearchResultsDomainUp.Items.RemoveAt(cont - 1);
            }
        }
        //-------------------------------------------------------------------------------------------




        /*---------------------------------------------------!AS----------------------------------------------------- */


        //<<!ADD SHOW PANEL>>
        //-------------------------------------------------------------------------------------------

        private void AddShowAddSongButton_Click(object sender, EventArgs e)
        {
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            if (profile.ProfileType != "viewer")
            {
                CreateSongPanel.BringToFront();
            }
            else
            {
                AddShowInvalidCredentialsLabel.Text = "You don´t have permission to create multimedia";
            }

        }

        private void AddShowAddVideoButton_Click(object sender, EventArgs e)
        {
            CreateVideoPanel.BringToFront();
        }

        private void AddShowAddPlaylistButton_Click(object sender, EventArgs e)
        {
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            if (profile.ProfileType != "viewer")
            {
                CreatePlaylistPanel.BringToFront();
            }
            else
            {
                AddShowInvalidCredentialsLabel.Text = "You don´t have permission to create multimedia";
            }
        }
        //ONEVENT

        //GO BACK/CLOSE

        private void AddShowGoBackButton_Click(object sender, EventArgs e)
        {
            DisplayStartPanel.BringToFront();
        }
        //-------------------------------------------------------------------------------------------




        /*--------------------------------------------------!!A----------------------------------------------------- */

        //<<!CREATE SONG PANEL>>
        //-------------------------------------------------------------------------------------------
        private void CreateSongCreateSongButton_Click(object sender, EventArgs e)
        {
            Profile profile = OnProfilesChooseProfile_Click(ProfileName, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            if (profile.ProfileType != "viewer")
            {
                string songName = CreateSongNameTextBox.Text;
                string songArtist = CreateSongArtistTextBox.Text;
                string songAlbum = CreateSongAlbumTextBox.Text;
                string songDiscography = CreateSongDiscographyTextBox.Text;
                string songGender = CreateSongGenderTextBox.Text;
                DateTime songPublishDate = CreateSongPublishDateTime.Value;
                string songStudio = CreateSongStudioTextBox.Text;
                double songDuration = double.Parse(CreateSongDurationTextBox.Text);
                string songFormat = CreateSongFormatTextBox.Text;
                string songLyricsSource = CreateSongLyricsTextBox.Text;
                string songLyrics = songLyricsSource.Split('\\')[songLyricsSource.Split('\\').Length - 1];
                string songFileSource = CreateSongSongFileTextBox.Text;
                string songFile = songFileSource.Split('\\')[songFileSource.Split('\\').Length - 1];
                if (File.Exists(songFile) == false)
                {
                    OnCreateSongCreateSongButton_Click(songName, songArtist, songAlbum, songDiscography, songGender, songPublishDate, songStudio, songDuration, songFormat, songLyrics, songLyricsSource, songFileSource, songFile);
                    DisplayStartPanel.BringToFront();
                    List<Song> songDataBase = OnSearchSongButton_Click();
                    AddPlaylistMult_Did(songDataBase[songDataBase.Count - 1], null);

                }
                else
                {
                    CreateSongInvalidCredentialTextBox.AppendText("An Error has ocurred please try again");
                    Thread.Sleep(2000);
                    DisplayStartPanel.BringToFront();
                    CreateSongNameTextBox.Clear();
                    CreateSongArtistTextBox.Clear();
                    CreateSongAlbumTextBox.Clear();
                    CreateSongDiscographyTextBox.Clear();
                    CreateSongGenderTextBox.Clear();
                    CreateSongStudioTextBox.Clear();
                    CreateSongDurationTextBox.Clear();
                    CreateSongFormatTextBox.Clear();
                    CreateSongLyricsTextBox.Clear();
                    CreateSongSongFileTextBox.Clear();
                }

            }
        }
        private void CreateSongSongFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                if (filename.Contains(".mp3") == true || filename.Contains(".wav") == true)
                {
                    CreateSongSongFileTextBox.Text = filename;
                }
                else
                {
                    CreateSongInvalidCredentialTextBox.AppendText("ERROR[!] wrong file format");
                }

            }
        }

        //ONEVENT

        public void OnCreateSongCreateSongButton_Click(string sName, string sArtist, string sAlbum, string sDiscography, string sGender, DateTime sPublishDate, string sStudio, double sDuration, string sFormat, string sLyrics, string sLyricsSource, string sSource, string songFile)
        {
            if (CreateSongCreateSongButton_Clicked != null)
            {
                bool result = CreateSongCreateSongButton_Clicked(this, new SongEventArgs() { NameText = sName, AlbumText = sAlbum, ArtistText = sArtist, DateText = sPublishDate, DiscographyText = sDiscography, DurationText = sDuration, FormatText = sFormat, GenderText = sGender, LyricsText = sLyrics, StudioText = sStudio, FileDestName = sSource, FileNameText = songFile, FileLyricsSource = sLyricsSource });
                if (!result)
                {
                    CreateSongInvalidCredentialTextBox.AppendText("An Error has ocurred please try again");
                    Thread.Sleep(2000);
                    DisplayStartPanel.BringToFront();
                    CreateSongNameTextBox.Clear();
                    CreateSongArtistTextBox.Clear();
                    CreateSongAlbumTextBox.Clear();
                    CreateSongDiscographyTextBox.Clear();
                    CreateSongGenderTextBox.Clear();
                    CreateSongStudioTextBox.Clear();
                    CreateSongDurationTextBox.Clear();
                    CreateSongFormatTextBox.Clear();
                    CreateSongLyricsTextBox.Clear();
                    CreateSongSongFileTextBox.Clear();

                }
                else
                {
                    CreateSongInvalidCredentialTextBox.AppendText("Song Added to the system");
                    Thread.Sleep(2000);
                    CreateSongNameTextBox.Clear();
                    CreateSongArtistTextBox.Clear();
                    CreateSongAlbumTextBox.Clear();
                    CreateSongDiscographyTextBox.Clear();
                    CreateSongGenderTextBox.Clear();
                    CreateSongStudioTextBox.Clear();
                    CreateSongDurationTextBox.Clear();
                    CreateSongFormatTextBox.Clear();
                    CreateSongLyricsTextBox.Clear();
                    CreateSongSongFileTextBox.Clear();
                }
            }
        }

        //GO BACK/CLOSE
        private void CreateSongGoBackButton_Click(object sender, EventArgs e)
        {
            AddShowPanel.BringToFront();
        }
        //-------------------------------------------------------------------------------------------




        //<<!CREATE VIDEO PANEL>>
        //-------------------------------------------------------------------------------------------
        private void CreateVideoSaveButton_Click(object sender, EventArgs e)
        {
            string videoName = CreateVideoNameTextBox.Text;
            string actors = CreateVideoActorsTextBox.Text;
            string directors = CreateVideoDirectorsTextBox.Text;
            string releaseDate = CreateVideoReleaseDateDateTimePicker.Value.ToShortDateString();
            string videoDimension = CreateVideoDimensionTextBox.Text;
            string videoQuality = CreateVideoQualityTextBox.Text;
            string videoCategory = CreateVideoCategoryTextBox.Text;
            string videoDescription = CreateVideoDescriptionTextBox.Text;
            string videoDuration = CreateVideoDurationTextBox.Text;
            string videoFormat = CreateVideoFormatTextBox.Text;
            string videoSubtitles = CreateVideoSubtitlesTextBox.Text;
            string videoFileSource = CreateVideoLoadVideoTextBox.Text;
            string videoFileName = videoFileSource.Split('\\')[videoFileSource.Split('\\').Length - 1];


            if (File.Exists(videoFileName) == false)
            {
                OnCreateVideoSaveButton_Clicked(videoName, actors, directors, releaseDate, videoDimension, videoQuality, videoCategory, videoDescription, videoDuration, videoFormat, videoSubtitles, videoFileSource, videoFileName, "true");
                List<Video> videoDataBase = OnSearchVideoButton_Click();
                AddPlaylistMult_Did(null, videoDataBase[videoDataBase.Count - 1]);
            }
            else
            {
                CreateVideoMessageTextBox.AppendText("ERROR[!] Your file already exists");
                CreateVideoNameTextBox.Clear();
                CreateVideoActorsTextBox.Clear();
                CreateVideoDirectorsTextBox.Clear();
                CreateVideoDimensionTextBox.Clear();
                CreateVideoQualityTextBox.Clear();
                CreateVideoCategoryTextBox.Clear();
                CreateVideoDescriptionTextBox.Clear();
                CreateVideoDurationTextBox.Clear();
                CreateVideoFormatTextBox.Clear();
                CreateVideoSubtitlesTextBox.Clear();
                CreateVideoLoadVideoTextBox.Clear();
                Thread.Sleep(1500);
                CreateVideoMessageTextBox.Clear();
            }

        }

        private void CreateVideoLoadVideoButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Media Files|*.mp4;*.avi;*.mov";

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                CreateVideoLoadVideoTextBox.Text = filename;
            }
        }
        //ONEVENT

        public void OnCreateVideoSaveButton_Clicked(string name, string actors, string directors, string releaseDate, string dimension, string quality, string category, string description, string duration, string format, string subtitles, string fileDest, string fileName, string image)
        {
            if (CreateVideoSaveButton_Clicked != null)
            {
                bool createVideo = CreateVideoSaveButton_Clicked(this, new VideoEventArgs() { NameText = name, ActorsText = actors, DirectorsText = directors, ReleaseDateText = releaseDate, DimensionText = dimension, Categorytext = category, DescriptionText = description, DurationText = duration, FormatText = format, SubtitlesText = subtitles, FileDestText = fileDest, FileNameText = fileName, QualityText = quality, VideoImage = image });
                if (createVideo)
                {
                    CreateVideoMessageTextBox.AppendText("Video Created succesfully!");
                    CreateVideoNameTextBox.Clear();
                    CreateVideoActorsTextBox.Clear();
                    CreateVideoDirectorsTextBox.Clear();
                    CreateVideoDimensionTextBox.Clear();
                    CreateVideoQualityTextBox.Clear();
                    CreateVideoCategoryTextBox.Clear();
                    CreateVideoDescriptionTextBox.Clear();
                    CreateVideoDurationTextBox.Clear();
                    CreateVideoFormatTextBox.Clear();
                    CreateVideoSubtitlesTextBox.Clear();
                    CreateVideoLoadVideoTextBox.Clear();
                    Thread.Sleep(2000);
                    CreateVideoMessageTextBox.Clear();
                    DisplayStartPanel.BringToFront();
                }
                else
                {
                    CreateVideoMessageTextBox.AppendText("ERROR[!] could not create video!");
                    CreateVideoNameTextBox.Clear();
                    CreateVideoActorsTextBox.Clear();
                    CreateVideoDirectorsTextBox.Clear();
                    CreateVideoDimensionTextBox.Clear();
                    CreateVideoQualityTextBox.Clear();
                    CreateVideoCategoryTextBox.Clear();
                    CreateVideoDescriptionTextBox.Clear();
                    CreateVideoDurationTextBox.Clear();
                    CreateVideoFormatTextBox.Clear();
                    CreateVideoSubtitlesTextBox.Clear();
                    CreateVideoLoadVideoTextBox.Clear();
                    Thread.Sleep(1500);
                    CreateVideoMessageTextBox.Clear();
                }
            }
        }

        //GO BACK/CLOSE
        //-------------------------------------------------------------------------------------------




        //<<!CREATE PLAYLIST PANEL>>
        //-------------------------------------------------------------------------------------------


        private void CreatePlaylistCreatePlaylistButton_Click(object sender, EventArgs e)
        {
            string playlistName = CreatePlaylistNameTextBox.Text;
            string playlistFormat = CreatePlaylistFormatDomainUp.Text;
            User playlistCreator = OnLoginButtonClicked(UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            Profile playlistProfileCreator = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            bool playlistPrivacy = CreatePlaylistPrivacyCheckBox.Checked; //True si esta checked
            OnCreatePlaylistCreatePlaylistButton_Click(playlistName, playlistFormat, playlistPrivacy, playlistCreator, playlistProfileCreator);
            OnSearchUserButton_Click();

        }
        //ONEVENT

        public void OnCreatePlaylistCreatePlaylistButton_Click(string plName, string plFormat, bool plPrivacy, User plCreator, Profile plProfileCreator)
        {
            if (CreatePlaylistCreatePlaylistButton_Clicked != null)
            {
                string result = CreatePlaylistCreatePlaylistButton_Clicked(this, new PlaylistEventArgs() { NameText = plName, CreatorText = plCreator, FormatText = plFormat, PrivacyText = plPrivacy, ProfileCreatorText = plProfileCreator });

                if (result == null)
                {
                    CreatePlaylistInvalidCredentialstextBox.AppendText("Playlist created succesfully!!");
                    Thread.Sleep(2000);
                    DisplayStartPanel.BringToFront();
                }
                else
                {
                    CreatePlaylistInvalidCredentialstextBox.AppendText("Error[!] " + result);
                    Thread.Sleep(2000);
                    DisplayStartPanel.BringToFront();
                }
            }
        }

        //GO BACK/CLOSE

        private void CreatePlaylistGoBack_Click(object sender, EventArgs e)
        {
            DisplayStartPanel.BringToFront();
            SearchSearchResultsDomainUp.ResetText();
        }
        //-------------------------------------------------------------------------------------------



        /*--------------------------------------------------!!S----------------------------------------------------- */
        /*
          
          
         */



        /*---------------------------------------------------!DP----------------------------------------------------- */
        
            
            //<<!DISPLAY PLAYLIST PANEL>>
        //-------------------------------------------------------------------------------------------
        private void DisplayPlaylistsGlobalPlaylist1_Click(object sender, EventArgs e)
        {
            PlayPlaylistMultTypeTextBox.Clear();
            soundPlayer = new SoundPlayer();
            List<PlayList> playlistDataBase = new List<PlayList>();
            playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();

            string result = playlistDataBase[0].DisplayInfoPlayList();
            foreach (PlayList playList in playlistDataBase)
            {
                string ex = playList.DisplayInfoPlayList();

                if (result == ex)
                {
                    SearchSearchResultsDomainUp.Text = ex;
                    if (playList.Format == ".mp3" || playList.Format == ".wav")
                    {
                        foreach (Song song in playList.Songs)
                        {
                            PlayPlaylistShowMultimedia.Items.Add(song.SearchedInfoSong());
                        }
                    }
                }
            }
            PlayPlaylistMultTypeTextBox.AppendText("Song");
            PlayPlaylistPanel.BringToFront();

        }

        private void DisplayPlaylistsGlobalPlaylist2_Click(object sender, EventArgs e)
        {
            PlayPlaylistMultTypeTextBox.Clear();
            soundPlayer = new SoundPlayer();
            List<PlayList> playlistDataBase = new List<PlayList>();
            playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();

            string result = playlistDataBase[1].DisplayInfoPlayList();
            foreach (PlayList playList in playlistDataBase)
            {
                string ex = playList.DisplayInfoPlayList();

                if (result == ex)
                {
                    SearchSearchResultsDomainUp.Text = ex;
                    if (playList.Format == ".mp3" || playList.Format == ".wav")
                    {
                        foreach (Song song in playList.Songs)
                        {
                            PlayPlaylistShowMultimedia.Items.Add(song.SearchedInfoSong());
                        }
                    }
                }
            }
            PlayPlaylistMultTypeTextBox.AppendText("Song");
            PlayPlaylistPanel.BringToFront();
        }

        private void DisplayPlaylistsGlobalPlaylist3_Click(object sender, EventArgs e)
        {
            PlayPlaylistMultTypeTextBox.Clear();
            soundPlayer = new SoundPlayer();
            List<PlayList> playlistDataBase = new List<PlayList>();
            playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();

            string result = playlistDataBase[2].DisplayInfoPlayList();
            foreach (PlayList playList in playlistDataBase)
            {
                string ex = playList.DisplayInfoPlayList();

                if (result == ex)
                {
                    SearchSearchResultsDomainUp.Text = ex;
                    if (playList.Format == ".mp4" || playList.Format == ".mov" || playList.Format == ".avi")
                    {
                        foreach (Video video in playList.Videos)
                        {
                            PlayPlaylistShowMultimedia.Items.Add(video.SearchedInfoVideo());
                        }
                    }
                }
            }
            PlayPlaylistMultTypeTextBox.AppendText("Video");
            PlayPlaylistPanel.BringToFront();
        }
        private void DisplayPlaylistsMoreGlobalPlaylistButton_Click(object sender, EventArgs e)
        {
            List<PlayList> playlistDataBase = new List<PlayList>();
            playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();
            if (playlistDataBase.Count > 3)
            {
                //Traer un panel que muestre las otras Pls Globales...
            }
        }
        private void DisplayPlaylistsFollowedPlaylist1_Click(object sender, EventArgs e)
        {
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            soundPlayer = new SoundPlayer();
            List<PlayList> playlistDataBase = new List<PlayList>();
            playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();

            string result = profile.FollowedPlayList[0].DisplayInfoPlayList();
            foreach (PlayList playList in playlistDataBase)
            {
                string ex = playList.DisplayInfoPlayList();
                if (result == ex)
                {
                    if (playList.Format == ".mp3" || playList.Format == ".wav")
                    {
                        foreach (Song song in playList.Songs)
                        {
                            PlayPlaylistShowMultimedia.Items.Add(song.SearchedInfoSong());
                        }
                    }
                }
            }
            PlayPlaylistPanel.BringToFront();
        }

        private void DisplayPlaylistsFollowedPlaylist2_Click(object sender, EventArgs e)
        {
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            soundPlayer = new SoundPlayer();
            List<PlayList> playlistDataBase = new List<PlayList>();
            playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();

            string result = profile.FollowedPlayList[1].DisplayInfoPlayList();
            foreach (PlayList playList in playlistDataBase)
            {
                string ex = playList.DisplayInfoPlayList();
                if (result == ex)
                {
                    if (playList.Format == ".mp3" || playList.Format == ".wav")
                    {
                        foreach (Song song in playList.Songs)
                        {
                            PlayPlaylistShowMultimedia.Items.Add(song.SearchedInfoSong());
                        }
                    }
                }
            }
            PlayPlaylistPanel.BringToFront();
        }

        private void DisplayPlaylistsFollowedPlaylist3_Click(object sender, EventArgs e)
        {
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            soundPlayer = new SoundPlayer();
            List<PlayList> playlistDataBase = new List<PlayList>();
            playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();

            string result = profile.FollowedPlayList[2].DisplayInfoPlayList();
            foreach (PlayList playList in playlistDataBase)
            {
                string ex = playList.DisplayInfoPlayList();
                if (result == ex)
                {
                    if (playList.Format == ".mp3" || playList.Format == ".wav")
                    {
                        foreach (Song song in playList.Songs)
                        {
                            PlayPlaylistShowMultimedia.Items.Add(song.SearchedInfoSong());
                        }
                    }
                }
            }
            PlayPlaylistPanel.BringToFront();
        }

        private void DisplayPlaylistsMoreFollowedPlaylistButton_Click(object sender, EventArgs e)
        {

        }
        private void DisplayPlaylistCreatedPlaylistImage1_Click(object sender, EventArgs e)
        {
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            soundPlayer = new SoundPlayer();
            List<PlayList> playlistDataBase = new List<PlayList>();
            playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();

            string result = profile.CreatedPlaylist[0].DisplayInfoPlayList();
            foreach (PlayList playList in playlistDataBase)
            {
                string ex = playList.DisplayInfoPlayList();
                if (result == ex)
                {
                    if (playList.Format == ".mp3" || playList.Format == ".wav")
                    {
                        foreach (Song song in playList.Songs)
                        {
                            PlayPlaylistShowMultimedia.Items.Add(song.SearchedInfoSong());
                        }
                    }
                }
            }
            PlayPlaylistPanel.BringToFront();
        }

        private void DisplayPlaylistCreatedPlaylistImage2_Click(object sender, EventArgs e)
        {
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            soundPlayer = new SoundPlayer();
            List<PlayList> playlistDataBase = new List<PlayList>();
            playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();

            string result = profile.CreatedPlaylist[1].DisplayInfoPlayList();
            foreach (PlayList playList in playlistDataBase)
            {
                string ex = playList.DisplayInfoPlayList();
                if (result == ex)
                {
                    if (playList.Format == ".mp3" || playList.Format == ".wav")
                    {
                        foreach (Song song in playList.Songs)
                        {
                            PlayPlaylistShowMultimedia.Items.Add(song.SearchedInfoSong());
                        }
                    }
                }
            }
            PlayPlaylistPanel.BringToFront();
        }

        private void DisplayPlaylistCreatedPlaylistImage3_Click(object sender, EventArgs e)
        {
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            soundPlayer = new SoundPlayer();
            List<PlayList> playlistDataBase = new List<PlayList>();
            playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();

            string result = profile.CreatedPlaylist[2].DisplayInfoPlayList();
            foreach (PlayList playList in playlistDataBase)
            {
                string ex = playList.DisplayInfoPlayList();
                if (result == ex)
                {
                    if (playList.Format == ".mp3" || playList.Format == ".wav")
                    {
                        foreach (Song song in playList.Songs)
                        {
                            PlayPlaylistShowMultimedia.Items.Add(song.SearchedInfoSong());
                        }
                    }
                }
            }
            PlayPlaylistPanel.BringToFront();
        }
        private void DisplayPlaylistsFavoritePlaylist1_Click(object sender, EventArgs e)
        {
            PlayPlaylistMultTypeTextBox.Clear();
            List<string> persSongList = new List<string>();
            persSongList = ReturnSearchedMult(ProfileDomainUp.Text, "Song", null);
            List<Song> songDataBase = new List<Song>();
            songDataBase = OnSearchSongButton_Click();
            foreach (Song song in songDataBase)
            {
                if (persSongList.Contains(song.SongFile) == true)
                {
                    PlayPlaylistShowMultimedia.Items.Add(song.SearchedInfoSong());
                }
            }
            PlayPlaylistMultTypeTextBox.AppendText("Song");
            PlayPlaylistPanel.BringToFront();
        }

        private void DisplayPlaylistsFavoritePlaylist2_Click(object sender, EventArgs e)
        {
            PlayPlaylistMultTypeTextBox.Clear();
            List<string> persVideoList = new List<string>();
            persVideoList = ReturnSearchedMult(ProfileDomainUp.Text, null, "Video");
            List<Video> videoDataBase = new List<Video>();
            videoDataBase = OnSearchVideoButton_Click();
            foreach (Video video in videoDataBase)
            {
                if (persVideoList.Contains(video.FileName) == true)
                {
                    PlayPlaylistShowMultimedia.Items.Add(video.SearchedInfoVideo());
                }
            }
            PlayPlaylistMultTypeTextBox.AppendText("Video");
            PlayPlaylistPanel.BringToFront();
        }

        private void DisplayPlaylistCreatedPlaylistButton_Click(object sender, EventArgs e)
        {

        }

        //ON EVENT
        public List<PlayList> OnDisplayPlaylistsGlobalPlaylist_Click()
        {
            if (DisplayPlaylistsGlobalPlaylist_Clicked != null)
            {
                List<PlayList> listPlaylist = DisplayPlaylistsGlobalPlaylist_Clicked(this, new PlaylistEventArgs()); //Nose si es necesario darle parametros
                return listPlaylist;
            }
            return null;
        }
        //-------------------------------------------------------------------------------------------


        /*---------------------------------------------------!APS----------------------------------------------------- */


        //<<!ACCOUNT PROFILE SETTINGS PANEL>>
        //-------------------------------------------------------------------------------------------
        private void UserSettinChangeUsernameButton_Click(object sender, EventArgs e)
        {
            UserProfilChangeInfoMessageBox.Clear();
            UserProfileChangeInfoInvalidBox.Clear();
            UserProfileChangeInfoPanel.BringToFront();
            UserProfilChangeInfoMessageBox.AppendText("Change Username.");
            UserProfileChangeInfoNewUsernameTextBox.Visible = true;
            label12.Visible = true;
        }

        private void UserSettinChangePasswordButton_Click(object sender, EventArgs e)
        {
            UserProfilChangeInfoMessageBox.Clear();
            UserProfileChangeInfoInvalidBox.Clear();
            UserProfileChangeInfoPanel.BringToFront();
            UserProfilChangeInfoMessageBox.AppendText("Change Password.");
            UserProfileChangeInfoNewPasswordTextBox.Visible = true;
            label11.Visible = true;
        }

        private void ProfileSettingsChangeProfileNameButton_Click(object sender, EventArgs e)
        {
            UserProfilChangeInfoMessageBox.Clear();
            UserProfileChangeInfoInvalidBox.Clear();
            UserProfileChangeInfoPanel.BringToFront();
            UserProfilChangeInfoMessageBox.AppendText("Change Profilename.");
            UserProfileChangeInfoNewProfilenameTextBox.Visible = true;
            label13.Visible = true;
        }

        //ONEVENT

        //GO BACK/CLOSE

        private void AccountProfileSettingsGoBackButton_Click(object sender, EventArgs e)
        {
            DisplayStartPanel.BringToFront();
        }
        //-------------------------------------------------------------------------------------------

        /*---------------------------------------------------!UC----------------------------------------------------- */

        //<<!USER PROFILE INFO CHANGE PANEL>>
        //-------------------------------------------------------------------------------------------


        private void UserProfileChangeInfoConfirmButton_Click(object sender, EventArgs e)
        {
            UserProfilChangeInfoMessageBox.Clear();
            UserProfileChangeInfoInvalidBox.Clear();
            int wantToChange = 0;
            string changed = null;
            if (UserProfileChangeInfoNewUsernameTextBox.Visible == true)
            {
                wantToChange = 1;
                changed = UserProfileChangeInfoNewUsernameTextBox.Text;
            }
            else if (UserProfileChangeInfoNewPasswordTextBox.Visible == true)
            {
                wantToChange = 2;
                changed = UserProfileChangeInfoNewPasswordTextBox.Text;
            }
            else if (UserProfileChangeInfoNewProfilenameTextBox.Visible == true)
            {
                wantToChange = 3;
                changed = UserProfileChangeInfoNewProfilenameTextBox.Text;
            }
            if (UserProfileChangeInfoUsernameTextBox.Text == UserLogInTextBox.Text)
            {
                UserProfileChangeInfoConfirmButton_Click(UserProfileChangeInfoUsernameTextBox.Text, UserProfileChangeInfoPasswordTextBox.Text, UserProfileChangeInfoProfileNameTextBox.Text, changed, wantToChange);
            }
            else
            {
                UserProfileChangeInfoInvalidBox.Clear();
                UserProfileChangeInfoInvalidBox.AppendText("ERRROR[!] Not your username.");
            }

            Thread.Sleep(2000);

            WelcomePanel.BringToFront();
            UserLogInTextBox.Clear();
            PasswordLogInTextBox.Clear();

            UserProfileChangeInfoPasswordTextBox.Clear();
            UserProfileChangeInfoUsernameTextBox.Clear();
            UserProfileChangeInfoProfileNameTextBox.Clear();

            UserProfileChangeInfoNewPasswordTextBox.Clear();
            UserProfileChangeInfoNewUsernameTextBox.Clear();
            UserProfileChangeInfoNewProfilenameTextBox.Clear();

            UserProfileChangeInfoNewUsernameTextBox.Visible = false;
            UserProfileChangeInfoNewPasswordTextBox.Visible = false;
            UserProfileChangeInfoNewProfilenameTextBox.Visible = false;

            label11.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
        }

        //ON EVENT

        public void UserProfileChangeInfoConfirmButton_Click(string username, string password, string profile, string changed, int want)
        {
            if (UserProfileChangeInfoConfirmButton_Clicked != null)
            {
                string result = UserProfileChangeInfoConfirmButton_Clicked(this, new UserEventArgs() { UsernameText = username, PasswordText = password, ProfilenameText = profile, WantToChangeText = want, ChangedText = changed });
                if (result == null)
                {
                    UserProfileChangeInfoInvalidBox.AppendText("ERROR[!] couldn't change settings");
                    Thread.Sleep(2000);

                }
                else
                {
                    UserProfileChangeInfoInvalidBox.AppendText(result);
                    Thread.Sleep(2000);
                }
            }
        }

        //GO BACK/CLOSE

        private void UserProfileChangeInfoGoBackButton_Click(object sender, EventArgs e)
        {
            DisplayStartPanel.BringToFront();
            UserProfileChangeInfoPasswordTextBox.Clear();
            UserProfileChangeInfoUsernameTextBox.Clear();
            UserProfileChangeInfoProfileNameTextBox.Clear();
            UserProfilChangeInfoMessageBox.Clear();
            UserProfileChangeInfoInvalidBox.Clear();
        }
        //-------------------------------------------------------------------------------------------


        /*---------------------------------------------------!PYPL----------------------------------------------------- */
        //
        //-------------------------------------------------------------------------------------------

        //ONEVENT

        //GO BACK/CLOSE
        //-------------------------------------------------------------------------------------------



        /*---------------------------------------------------!AM----------------------------------------------------- */
        //<<!ADMIN MENU PANEL>>
        private void AdminMenuEraseUserButton_Click(object sender, EventArgs e)
        {
            AdminMenuMessageBox.Clear();
            string username = AdminMenuAllUsers.Text;
            AdminMethods(username, 0);
        }

        private void AdminMenuBanUserButton_Click(object sender, EventArgs e)
        {
            AdminMenuMessageBox.Clear();
            string username = AdminMenuAllUsers.Text;
            AdminMethods(username, 1);
        }

        private void AdminMenuBanUser_Click(object sender, EventArgs e) //Unbanea
        {
            AdminMenuMessageBox.Clear();
            string username = AdminMenuAllUsers.Text;
            AdminMethods(username, 2);
        }
        //-------------------------------------------------------------------------------------------

        //ONEVENT
        public void AdminMethods(string username, int choice)
        {
            if (AdminMethods_Done != null)
            {
                string result = AdminMethods_Done(this, new UserEventArgs() { UsernameText = username, WantToChangeText = choice });
                if (result != null)
                {
                    AdminMenuMessageBox.AppendText(result);
                }
                else
                {
                    AdminMenuMessageBox.AppendText("ERROR[!]");
                }
            }

        }

        //GO BACK/CLOSE

        private void AdminMenuGoBackButton_Click(object sender, EventArgs e)
        {
            AdminMenuMessageBox.Clear();
            int cont = 0;
            foreach (object searched in AdminMenuAllUsers.Items)
            {
                cont++;
            }
            for (int i = 0; i < cont; cont--)
            {
                AdminMenuAllUsers.Items.RemoveAt(cont - 1);
            }

            DisplayStartPanel.BringToFront();
        }
        //-------------------------------------------------------------------------------------------





        /*---------------------------------------------------!ON EVENT GENERALES----------------------------------------------------- */

        public List<string> GetSongButton(string sName, string sArtist)
        {
            if (GetSongInformation != null)
            {
                List<string> songInfo = GetSongInformation(this, new SongEventArgs() { NameText = sName, ArtistText = sArtist });
                return songInfo;
            }
            return null;
        }
        public List<List<string>> ReturnAllSongsInfo()
        {
            if (GetAllSongInformation != null)
            {
                List<List<string>> allSongsInfo = GetAllSongInformation(this, new SongEventArgs());
                return allSongsInfo;
            }
            else return null;
        }
        public List<string> GetVideoButton(string vName, string vActors, string vDirectors)
        {
            if (GetVideoInformation != null)
            {
                List<string> videoInfo = GetVideoInformation(this, new VideoEventArgs() { NameText = vName, ActorsText = vActors, DirectorsText = vDirectors });
                return videoInfo;
            }
            return null;
        }
        public List<List<string>> ReturnAllVideosInfo()
        {
            if (GetAllVideosInformation != null)
            {
                List<List<string>> allVideosInfo = GetAllVideosInformation(this, new VideoEventArgs());
                return allVideosInfo;
            }
            else return null;
        }

        public void AddingSearchedMult(string pName, string sFile, string vFile)
        {
            if (AddSearchedMult_Done != null)
            {
                bool result = AddSearchedMult_Done(this, new UserEventArgs() { ProfilenameText = pName, SongFileText = sFile, VideoFileText = vFile });
                if (result)
                {
                    SearchOkMultAddedLabel.ResetText();
                    SearchOkMultAddedLabel.Text = "OK";
                }
            }
        }
        public List<string> ReturnSearchedMult(string pName, string sFile, string vFile)
        {
            List<string> persMultList = new List<string>();
            if (ReturnSearchedMult_Done != null)
            {
                persMultList = ReturnSearchedMult_Done(this, new UserEventArgs() { ProfilenameText = pName, SongFileText = sFile, VideoFileText = vFile });
            }
            return persMultList;
        }

        public void AddPlaylistMult_Did(Song song, Video video)
        {
            string result = null;

            if (AddPlaylistMult_Done != null)
            {
                result = AddPlaylistMult_Done(this, new PlaylistEventArgs() { SongText = song, VideoText = video });
            }
        }

        public List<Song> OnSearchSongButton_Click()
        {
            if (SearchSongButton_Clicked != null)
            {
                List<Song> songDataBase = SearchSongButton_Clicked(this, new SongEventArgs());
                return songDataBase;
            }
            return null;
        }

        public List<Video> OnSearchVideoButton_Click()
        {
            if (SearchVideoButton_Clicked != null)
            {
                List<Video> videoDataBase = SearchVideoButton_Clicked(this, new VideoEventArgs());
                return videoDataBase;
            }
            return null;
        }

        public List<User> OnSearchUserButton_Click()
        {
            if (SearchUserButton_Clicked != null)
            {
                List<User> userDataBase = SearchUserButton_Clicked(this, new RegisterEventArgs());
                return userDataBase;
            }
            return null;

        }

        private void PlayVideoMessageLabel_TextChanged(object sender, EventArgs e)
        {

        }

        private void PlayVideoMessageAlertTextBox_TextChanged(object sender, EventArgs e)
        {

        }
        
        
        
    }
}
