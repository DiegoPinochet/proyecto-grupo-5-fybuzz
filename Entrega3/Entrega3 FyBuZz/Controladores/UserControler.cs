﻿using Entrega3_FyBuZz.CustomArgs;
using Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Entrega3_FyBuZz.Controladores
{
    public class UserControler
    {
        List<User> userDataBase = new List<User>() { new User() };
        DataBase dataBase = new DataBase();
        static DataBase data = new DataBase();
        Modelos.Menu menu = new Modelos.Menu();
        Server server = new Server(data);
        FyBuZz fyBuZz;


        public UserControler(Form fyBuZz)
        {
            Initialize();
            this.fyBuZz = fyBuZz as FyBuZz;
            this.fyBuZz.LogInLogInButton_Clicked2 += userGetter;
            this.fyBuZz.GetProfileNames += GetUserProfiles;
            this.fyBuZz.LogInLogInButton_Clicked += OnLoginButtonClicked;
            this.fyBuZz.RegisterRegisterButton_Clicked += OnRegisterRegisterButtonClicked;
            this.fyBuZz.CreateProfileCreateProfileButton_Clicked += OnCreateProfileCreateProfileButton_Clicked;
            this.fyBuZz.ProfilesChooseProfile_Clicked2 += OnProfilesChooseProfile_Click2;
            this.fyBuZz.ProfilesChooseProfile_Clicked += OnProfilesChooseProfile_Click;
            this.fyBuZz.SearchUserButton_Clicked += OnSearchUserButton_Click;
            this.fyBuZz.SearchFollowButton_Clicked += OnSearchFollowButton_Click;
            this.fyBuZz.UserProfileChangeInfoConfirmButton_Clicked += OnUserProfileChangeInfoConfirmButton_Click;
        }

        public void Initialize()
        {
            if (File.Exists("AllUsers.bin") != true) dataBase.Save_Users(userDataBase);
            userDataBase = dataBase.Load_Users();
        }

        private int UserIndex(ProfileEventArgs e)
        {
            int u = 0;
            foreach(User user in userDataBase)
            {
                if (user.Username == e.UsernameText && user.Password == e.PasswordText)
                {
                    break;
                }
                u++;
            }
            return u;
        }

        private User OnLoginButtonClicked(object sender, LogInEventArgs e)
        {
            
            return dataBase.LogIn(e.UsernameText, e.PasswrodText, userDataBase);
        }
        private bool OnRegisterRegisterButtonClicked(object sender, RegisterEventArgs e)
        {
            User user = new User();
            bool x;
            x = server.Register(user, userDataBase,e.UsernameText, e.EmailText, e.PasswrodText, e.SubsText, e.PrivacyText, e.GenderText, e.BirthdayText, e.ProfileTypeText);
            dataBase.Save_Users(userDataBase);
            return x;
        }
        private string OnCreateProfileCreateProfileButton_Clicked(object sender, ProfileEventArgs e)
        {
            int u = UserIndex(e);
            if (userDataBase[u].Accountype == "premium")
            {   
                int pAge = DateTime.Now.Year - e.BirthdayText.Year;
                string pPic = "...";
                string name = userDataBase[u].Username;
                Profile profile = userDataBase[u].CreateProfile(e.ProfileNameText, pPic, e.ProfileTypeText, e.EmailText, e.GenderText, pAge);
                userDataBase[u].Perfiles.Add(profile);
                dataBase.Save_Users(userDataBase);
                return profile.ProfileName;
            }
            else
            {
                return null;
            }
        }

        private List<string> userGetter(object sender, UserEventArgs e)
        {
            List<string> userGetterString = new List<string>();
            foreach(User user in userDataBase)
            {
                if(user.Username == e.UsernameText)
                {
                    userGetterString.Add(user.Username);
                    userGetterString.Add(user.Password);
                    userGetterString.Add(user.Email);
                    userGetterString.Add(user.Accountype);
                    userGetterString.Add(user.Followers.ToString());
                    userGetterString.Add(user.Following.ToString());
                    userGetterString.Add(user.Verified.ToString());
                    userGetterString.Add(user.AdsOn.ToString());
                    userGetterString.Add(user.Privacy.ToString());
                }
            }
            if(userGetterString.Count == 0)
            {
                return null;
            }
            return userGetterString;
        }
        private List<List<string>> userListsGetter(object sender, UserEventArgs e)
        {
            List<List<string>> userGetterList = new List<List<string>>();
            foreach (User user in userDataBase)
            {
                if (user.Username == e.UsernameText)
                {
                    userGetterList.Add(user.FollowingList);
                    userGetterList.Add(user.FollowerList);
                }
            }
            if (userGetterList.Count == 0)
            {
                return null;
            }
            return userGetterList;
        }
        private List<string> GetUserProfiles(object sender, UserEventArgs e)
        {
            List<string> userProfileList = new List<string>();
            foreach (User user in userDataBase)
            {
                string usr = user.Username;
                if (user.Username == e.UsernameText)
                {
                    foreach(Profile profile in user.Perfiles)
                    {
                        userProfileList.Add(profile.ProfileName);
                    }
                }
            }
            if (userProfileList.Count == 0)
            {
                return null;
            }
            return userProfileList;
        }

        private Profile OnProfilesChooseProfile_Click(object sender, ProfileEventArgs e)

        {
            List<string> profileGetterString = new List<string>();
            int u = UserIndex(e);
            int pAge = DateTime.Now.Year - e.BirthdayText.Year;

            Profile prof = new Profile(e.ProfileNameText,"..",e.ProfileTypeText,e.EmailText,e.GenderText, pAge);
            foreach(Profile profile in userDataBase[u].Perfiles)
            {
                if (profile.ProfileName == prof.ProfileName || profile.Username == prof.ProfileName)
                {
                    prof = profile;
                }
            }
            return prof;
        }
        private List<string> OnProfilesChooseProfile_Click2(object sender, ProfileEventArgs e)
        {
            List<string> profileGetterString = new List<string>();
            int u = UserIndex(e);
            int pAge = DateTime.Now.Year - e.BirthdayText.Year;
            Profile prof = new Profile(e.ProfileNameText, "..", e.ProfileTypeText, e.EmailText, e.GenderText, pAge);
            foreach (Profile profile in userDataBase[u].Perfiles)
            {
                if (profile.ProfileName == prof.ProfileName || profile.Username == prof.ProfileName)
                {
                    profileGetterString.Add(profile.ProfileName);
                    profileGetterString.Add(profile.ProfileType);
                    profileGetterString.Add(profile.Gender);
                    profileGetterString.Add(profile.Age.ToString());
                }
            }
            if (profileGetterString.Count() == 0)
            {
                return null;
            }
            return profileGetterString;
        }
        private List<User> OnSearchUserButton_Click(object sender, RegisterEventArgs e)
        {
            dataBase.Save_Users(userDataBase);
            return userDataBase;
        }
        private string OnSearchFollowButton_Click(object sender, UserEventArgs e)
        {
            List<string> listuser = e.UserLogIn.FollowingList;
            List<PlayList> followedPL = e.ProfileUserLogIn.FollowedPlayList;
            string tryingToFollow = e.UserSearched.Username;
            string result = null;

            if (listuser.Contains(tryingToFollow) == false)
            {
                e.UserSearched.Followers = e.UserSearched.Followers + 1;
                e.UserLogIn.Following = e.UserLogIn.Following + 1;
                if (e.UserSearched.ProfilePlaylists != null)
                {
                    foreach (PlayList Pls in e.UserSearched.ProfilePlaylists)
                    {
                        followedPL.Add(Pls);
                    }
                }
                /*else
                {
                    result = "Error, this user don't have created playlists";
                }*/

                e.UserLogIn.FollowingList.Add(e.UserSearched.Username);
                e.UserSearched.FollowerList.Add(e.UserLogIn.Username);
                result = "Followed: " + e.UserSearched.SearchedInfoUser();
                dataBase.Save_Users(userDataBase);
            }
            return result;
        }
        private string OnUserProfileChangeInfoConfirmButton_Click(object sender, UserEventArgs e)
        {
            string result = null;
            if(e.WantToChangeText == 1)
            {
                foreach(User user in userDataBase)
                {
                    if(user.Username == e.UsernameText && user.Password == e.PasswordText)
                    {
                        int cont1 = 0;
                        foreach (User usr in userDataBase)
                        {
                            if(usr.Username == e.ChangedText)
                            {
                                cont1++;
                            }
                            
                        }
                        if (cont1 == 0)
                        {
                            user.Username = e.ChangedText;
                            result = "Succesfully changed Username";
                        }

                    }
                }
            }
            else if (e.WantToChangeText == 2)
            {
                foreach (User user in userDataBase)
                {

                    if (user.Username == e.UsernameText && user.Password == e.PasswordText)
                    {
                        user.Password = e.ChangedText;
                        result = "Succesfully changed Password";
                    }
                }
            }
            else
            {
                foreach (User user in userDataBase)
                {
                    if (user.Username == e.UsernameText && user.Password == e.PasswordText)
                    {
                        int cont2 = 0;
                        foreach (Profile profile in user.Perfiles)
                        {
                            if (profile.ProfileName == e.ChangedText)
                            {
                                cont2++;
                                result = null;
                            }
                            if (cont2 == 0)
                            {
                                profile.ProfileName = e.ChangedText;
                                result = "Succesfully changed Profilename";
                            }
                        }
                        

                    }
                }
            }
            dataBase.Save_Users(userDataBase);
            return result;
        }
    }
}
