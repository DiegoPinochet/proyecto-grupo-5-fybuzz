﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Modelos
{
    public class Menu
    {
        protected List<string> filters;
        protected List<string> badWords = new List<string>() { "fuck", "sex", "niggas", "sexo", "ass", "nigger", "culo", "viola", "violar", "spank", "puta", "hooker", "perra", "hoe", "cocaina", "alchohol", "blunt", "weed", "marihuana", "lcd", "kush", "krippy", "penis", "dick", "cock", "shit", "percocet" };
        public List<Song> searchedSongs = new List<Song>();
        public List<Video> searchedVideos = new List<Video>();
        User user = new User();
        DataBase database = new DataBase();
        Player player = new Player();


        public User DisplayLogin(List<User> userDataBase)
        {
            Server server = new Server(database);
            bool x = false;
            while (x == false)
            {
                Console.Clear();
                Console.WriteLine("------------Welcome to FyBuZz--------------");
                Console.WriteLine("I) Log-In with a existing account.");
                Console.WriteLine("II) Register.");
                Console.WriteLine("III) Close App.");
                string dec = Console.ReadLine();
                switch (dec)
                {

                    case "I":
                        database.Load_Users();
                        Console.Write("Username: ");
                        string username = Console.ReadLine();
                        Console.Write("Password: ");
                        string password = Console.ReadLine();
                        if (database.LogIn(username, password, userDataBase) != null)
                        {
                            Console.WriteLine("Login Succesfull.");
                            Thread.Sleep(1000);
                            Console.Beep();
                            x = true;
                            user = database.LogIn(username, password, userDataBase);
                        }
                        else
                        {
                            Console.WriteLine("ERROR[!] Invalid Username or Password");
                            Thread.Sleep(2000);
                        }
                        break;


                    case "II":
                        Console.Write("Welcome! Type your information in FyBuZz\nUsername: ");
                        string usr = Console.ReadLine();
                        Console.Write("Email: ");
                        string email = Console.ReadLine();
                        Console.Write("Password: ");
                        string psswd = Console.ReadLine();
                        string premium;
                        do
                        {
                            Console.Write("Would you like to pay for the premium subscription?(premium/standard/admin):");
                            premium = Console.ReadLine();
                        } while (premium != "premium" && premium != "standard" && premium != "admin");
                        string pars;
                        bool priv;
                        do
                        {
                            Console.Write("Would you like to have a private user?(true/false):");
                            pars = Console.ReadLine();
                        } while (pars != "true" && pars != "false");
                        priv = bool.Parse(pars);
                        string gender;
                        do
                        {
                            Console.Write("Select your gender(M/F): ");
                            gender = Console.ReadLine();
                        } while (gender != "M" && gender != "F");
                        int age;
                        do
                        {
                            Console.Write("Select your age: ");
                            age = int.Parse(Console.ReadLine());
                        } while (age / age != 1);
                        string profileType;
                        do
                        {
                            Console.Write("Select your Profile Type(creator/viewer): ");
                            profileType = Console.ReadLine();
                        } while (profileType != "creator" && profileType != "viewer");
                        DateTime date = DateTime.Now;
                        server.Register(user, userDataBase, usr, email, psswd, premium, priv, gender, date, profileType);
                        database.Save_Users(userDataBase);
                        x = false;
                        break;
                    case "III":
                        return null;
                }
            }
            Console.Clear();
            return user;
        }
        //Por mientras para probar algo...
        public void CreateProfile(User user, string pname, string ppic, string ptype, string pmail, string pgender, DateTime pbirthday)
        {
            int page = 2020 - pbirthday.Year;
            user.CreateProfile(pname, ppic, ptype, pmail, pgender,page);
        }
        //Hasta aca....

        public Profile DisplayProfiles(User user, List<User> userDataBase)
        {

            //Console.WriteLine("Welcome: " + user.Username + "\n");
            bool x = true;
            int u = 0;
            foreach (User usr in userDataBase)
            {
                if (usr.Username == user.Username)
                {
                    break;
                }
                u++;
            }
            //Console.WriteLine("---------Profiles----------");
            
            while (x == true)
            {
                //Console.WriteLine("I) Choose a profile\nII) Create Profile\nIII) Close App.");
                string dec = Console.ReadLine();
                if (dec == "I")
                {
                    Console.WriteLine("List of profiles:");
                    int i;
                    for (i = 0; i < userDataBase[u].Perfiles.Count(); i++)
                    {
                        Console.WriteLine("{0}).- {1}", i + 1, userDataBase[u].Perfiles[i].ProfileName);
                    }
                    Console.WriteLine("Choose a profile (Number):");
                    int index = int.Parse(Console.ReadLine()) - 1;
                    return userDataBase[u].Perfiles[index];
                }
                else if (dec == "II")
                {
                    if (user.Accountype == "premium")
                    {
                        Console.WriteLine("Create a profile:");
                        Console.Write("Profile name: ");
                        string pname = Console.ReadLine();
                        Console.Write("Profile pic: ");
                        string ppic = Console.ReadLine();
                        Console.Write("Profile type(creator/viewer): ");
                        string ptype = Console.ReadLine();
                        string pmail = user.Email;
                        Console.Write("Profile gender (M/F): ");
                        string pgender = Console.ReadLine();
                        Console.Write("Profile age: ");
                        int page = int.Parse(Console.ReadLine());
                        userDataBase[u].CreateProfile(pname, ppic, ptype, pmail, pgender, page);
                        Console.Clear();
                    }
                    else if (user.Accountype == "standard")
                    {
                        Console.WriteLine("ERROR [!] Standard Account Types, you can only have one profile.\nTry Upgrading to Premium Account.");
                        Console.Write("Do you want to change your Account Type to premium?(y/n): ");
                        string premium = Console.ReadLine();
                        if (premium == "y") userDataBase[u].Accountype = "premium";
                        else continue;
                    }
                    else if (user.Accountype == "admin")
                    {
                        Console.WriteLine("ERROR [!] Admin Account Types, you can only have one profile.");
                        Thread.Sleep(1000);
                    }
                }
                else if (dec == "III")
                {
                    return null;
                }
            }
            
            Console.Clear();
            return null;
        }

        public int DisplayStart(Profile userProfile, User usr, List<User> listUserGlobal, List<Song> listSongsGlobal, List<Song> DownloadSongs, List<Video> listVideosGlobal, List<PlayList> listPlayListGlobal, List<PlayList> listPlaylistPriv) // solo funciona si DisplayLogIn() retorna true se ve en program.
        {

            int ret = 0;
            Server server = new Server(database);

            // mostrará todas las playlist del usuario, si es primera vez que ingresa estara la playlist general y la favorita(esta sin nada)
            // es la lista global de playlist que viene de database, pero hay que conectarla

            PlayList favSongs = new PlayList(".mp3", "FavoriteSongs", usr.Username, userProfile.ProfileName);
            Dictionary<string, List<Song>> playlistFavSongs = favSongs.DicCanciones; //Playlist de favoritos que su nombre es el de arriba.

            PlayList favVideos = new PlayList(".mp3", "FavoriteVideos", usr.Username, userProfile.ProfileName);
            Dictionary<string, List<Video>> playlistFavVideos = favVideos.DicVideos; //Playlist de favoritos que su nombre es el de arriba.

            List<PlayList> followedPL = userProfile.FollowedPlayList; //una lista de todas las playlist, discos, usuarios, etc.
            //Si seguimos la usuario seguiremos todas sus playlist (REVISAR ESTO)

            Console.Clear();
            Console.WriteLine("------------Welcome to FyBuZz--------------"); //Se inicia el menu en si.
            bool x = true;
            while (x == true)
            {

                Console.WriteLine("I) Search Songs, Videos or Users."); //Panel listo
                Console.WriteLine("II) Add/Show Songs, Videos or Playlists.");
                Console.WriteLine("III) Display all Playlists."); //Panel listo
                Console.WriteLine("IV) Account Settings/ Profile Settings."); //Panel listo
                Console.WriteLine("V) Play a Playlist."); //No seria mejor juntar esto con display all playlist....? Le ponemos un searchbar para playlists.
                Console.WriteLine("VI) LogOut from Profile.");
                Console.WriteLine("VII) CloseApp.");
                Console.WriteLine("VIII) Admin Menu."); //Panel listo
                string dec = Console.ReadLine();
                switch (dec)
                { //(REVISAR DESPUES)Mejorar el metodo de busqueda para que busque canciones que se parezca
                    case "I":
                        //Método de buscar, una vez buscada la canción y elegida.
                        Console.WriteLine("What would you like to search? (Songs/Videos/Users)");
                        string type = Console.ReadLine();
                        if (type == "Songs")
                        {
                            Console.WriteLine("Type what you want to search...{name, artist, album, discography, studio, gender}");
                            string search = Console.ReadLine();
                            List<string> searchEngine = SearchEngine(search, type, database);
                            List<int> indexglobal = new List<int>();
                            Console.WriteLine("Searched Songs: ");
                            if (searchEngine.Count() != 0)
                            {
                                for (int i = 0; i < searchEngine.Count(); i++)
                                {
                                    Console.WriteLine((i + 1) + ") " + searchEngine[i]);
                                }
                                Console.WriteLine("Searched Songs, choose the position of the song you want to hear...");
                                int indice = int.Parse(Console.ReadLine()) - 1;
                                Song song = listSongsGlobal[indice]; //La cancion a la que querria escuchar
                                int cont = 0;
                                foreach (string word in badWords)
                                {
                                    if (song.Lyrics.Contains(word) == true && userProfile.Age < 16)
                                    {
                                        Console.WriteLine("ERROR[!] This content is age restricted");
                                        Thread.Sleep(1000);
                                        cont++;
                                        break;
                                    }
                                }
                                if (cont == 0)
                                {

                                    Console.WriteLine("Do you want to: \nI)Listen.\nII)Download.\nIII)Add to Playlist.");
                                    string want = Console.ReadLine();
                                    switch (want)
                                    {
                                        case "I":
                                            if (cont == 0)
                                            {
                                                Console.Clear();
                                                player.PlaySong(song, null, database, usr, userProfile);
                                            }
                                            break;
                                        case "II":
                                            Console.WriteLine("Song is Downloading...");
                                            Thread.Sleep(1000 * 5);
                                            Console.WriteLine("Song Downloaded.");
                                            Thread.Sleep(1000);
                                            DownloadSongs.Add(song);
                                            database.Save_DSongs(DownloadSongs);
                                            break;
                                        case "III":
                                            if (userProfile.CreatedPlaylist.Count() != 0)
                                            {
                                                int i = 1;
                                                Console.WriteLine("Where do you want to add this song?");
                                                foreach (PlayList playList in userProfile.CreatedPlaylist)
                                                {
                                                    Console.WriteLine(i + ") " + playList.DisplayInfoPlayList());
                                                    i++;
                                                }
                                                Console.WriteLine("Please select de number of the Playlist...");
                                                int createdPlaylistIndex = int.Parse(Console.ReadLine()) - 1;
                                                userProfile.CreatedPlaylist[createdPlaylistIndex].Songs.Add(song);
                                            }
                                            else Console.WriteLine("You don´t have any Playlists, please create one...");
                                            break;
                                    }
                                }
                                Console.WriteLine("\n");
                            }
                        }
                        else if (type == "Videos")
                        {
                            Console.WriteLine("Type what you want to search...{ name, actors, directors, quality, category, rated, description}");

                            string search = Console.ReadLine();
                            List<string> searchEngine = SearchEngine(search, type, database);
                            List<int> indexglobal = new List<int>();
                            if (searchEngine.Count != 0)
                            {
                                Console.WriteLine("Searched Videos: ");
                                for (int i = 0; i < searchEngine.Count(); i++)
                                {
                                    Console.WriteLine((i + 1) + ") " + searchEngine[i]);
                                }
                                Console.WriteLine("Searched Songs, choose the position of the song you want to hear...");
                                int indice = int.Parse(Console.ReadLine()) - 1;
                                Video video = listVideosGlobal[indice]; //La cancion a la que querria escuchar
                                int cont = 0;
                                foreach (string word in badWords)
                                {
                                    if ((video.Subtitles.Contains(word) && userProfile.Age < 16) || (int.Parse(video.Category) >= userProfile.Age))
                                    {
                                        Console.WriteLine("ERROR[!] This content is age restricted");
                                        Thread.Sleep(1000);
                                        cont++;
                                        break;
                                    }
                                }
                                if (cont == 0)
                                {
                                    Console.WriteLine("Do you want to: \nI)Listen.\nII)Add to Playlist.");
                                    string want = Console.ReadLine();
                                    switch (want)
                                    {
                                        case "I":
                                            if (cont == 0)
                                            {
                                                Console.Clear();
                                                player.PlayVideo(video, null, database, usr, userProfile);
                                            }
                                            break;
                                        case "II":
                                            if (userProfile.CreatedPlaylist.Count() != 0)
                                            {
                                                int i = 1;
                                                Console.WriteLine("Where do you want to add this video?");
                                                foreach (PlayList playList in userProfile.CreatedPlaylist)
                                                {
                                                    Console.WriteLine(i + ") " + playList.DisplayInfoPlayList());
                                                    i++;
                                                }
                                                Console.WriteLine("Please select de number of the Playlist...");
                                                int createdPlaylistIndex = int.Parse(Console.ReadLine()) - 1;
                                                userProfile.CreatedPlaylist[createdPlaylistIndex].Videos.Add(video);
                                            }
                                            break;
                                    }
                                }
                                Console.WriteLine("\n");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Type the user you want to search...");
                            string search = Console.ReadLine();
                            List<string> searchEngine = SearchEngine(search, type, database);
                            if (searchEngine.Count != 0)
                            {
                                Console.WriteLine("Searched Users: ");
                                for (int i = 0; i < searchEngine.Count(); i++)
                                {
                                    Console.WriteLine((i + 1) + ") " + searchEngine[i]);
                                }
                                Console.WriteLine("Searched Users, choose the position of the user you want to follow...");
                                int indice = int.Parse(Console.ReadLine()) - 1;
                                User u = listUserGlobal[indice];
                                List<string> listuser = usr.FollowingList;
                                string tryingToFollow = u.Username;

                                if (listuser.Contains(tryingToFollow) == false)
                                {
                                    u.Followers = u.Followers + 1;
                                    usr.Following = usr.Following + 1;
                                    if (u.ProfilePlaylists != null)
                                    {
                                        foreach (PlayList Pls in u.ProfilePlaylists)
                                        {
                                            followedPL.Add(Pls);

                                        }
                                    }
                                    else Console.WriteLine("This user hasn´t created any PlayList");
                                    usr.FollowingList.Add(u.Username);
                                    Console.WriteLine("\nFollowed: " + u.SearchedInfoUser());
                                    Thread.Sleep(2000);
                                    database.Save_Users(listUserGlobal);

                                }
                                else Console.WriteLine("You already follow this user.");
                                Thread.Sleep(1000);

                                Console.WriteLine("\n");
                            }
                        }
                        Console.Clear();
                        //database.Save_Users(database.UserDataBase);
                        break;

                    case "II":
                        Console.Clear();
                        Console.Write("What do you wish to add? (song(0), video(1), Playlist(2))\nDo you wish to Show the list of a specified multimedia? (song(3), video(4), Playlist(5)): ");
                        int opc = int.Parse(Console.ReadLine());
                        if (opc == 0 || opc == 1 || opc == 2)
                        {
                            if (userProfile.ProfileType == "creator" || userProfile.ProfileType == "admin")
                            {
                                List<string> infoMult = AskInfoMult(opc);
                                string username = usr.Username;
                                string profileUser = userProfile.ProfileName;
                                string priv;
                                if (opc == 2)
                                {
                                    Console.WriteLine("Do you want the Playlist to be private or public?(y/n)");
                                }
                                priv = Console.ReadLine();
                                string description = database.AddMult(opc, infoMult, listSongsGlobal, listPlayListGlobal, listVideosGlobal, username, profileUser, priv, listPlaylistPriv);
                                if (description == null)
                                {
                                    Console.WriteLine("Multimedia has been registered into the system!");
                                    Thread.Sleep(3000);
                                }
                                else Console.WriteLine("ERROR[!] ~{0}", description);
                                if (opc == 0) database.Save_Songs(listSongsGlobal);
                                else if (opc == 1) database.Save_Videos(listVideosGlobal);
                                else if (opc == 2)
                                {
                                    foreach (PlayList playList in listPlayListGlobal)
                                    {
                                        if (playList.Creator == usr.Username && playList.ProfileCreator == userProfile.ProfileName)
                                        {
                                            userProfile.CreatedPlaylist.Add(playList);
                                            usr.ProfilePlaylists.Add(playList);
                                        }
                                    }
                                    foreach (PlayList playList in listPlaylistPriv)
                                    {
                                        if (playList.Creator == usr.Username && playList.ProfileCreator == userProfile.ProfileName)
                                        {
                                            userProfile.CreatedPlaylist.Add(playList);
                                            //usr.ProfilePlaylists.Add(playList); Si la PL es privada no se agrega al usuario, por lo tanto no se puede seguir.
                                        }
                                    }
                                    database.Save_PLs(listPlayListGlobal);
                                    database.Save_PLs_Priv(listPlaylistPriv);
                                }
                            }
                            else
                            {
                                Console.WriteLine("You do not have permission to add Multimedia.");
                                Thread.Sleep(1000);
                            }
                        }
                        else if (opc == 3 || opc == 4 || opc == 5)
                        {
                            DisplayGlobalMult(opc - 3, database);
                            Thread.Sleep(5000);
                        }
                        Console.Clear();
                        break;
                    case "III":
                        Console.Clear();
                        if (playlistFavSongs != null || playlistFavVideos != null)
                        {
                            Console.WriteLine("Liked Playlist:");
                            Console.WriteLine(favSongs.InfoPlayList());
                            Console.WriteLine("\n");
                            Console.WriteLine(favSongs.InfoPlayList());
                            Console.WriteLine("\n");
                        }
                        else Console.WriteLine("You don´t have a Favorite Playlist.");
                        Console.WriteLine("-------------------------------------------------------");
                        Thread.Sleep(1000);

                        if (followedPL.Count() != 0)
                        {
                            Console.WriteLine("Followed Playlist:");
                            foreach (PlayList pl in followedPL)
                            {
                                Console.WriteLine(pl.DisplayInfoPlayList());
                                if (pl.Format == ".mp3" || pl.Format == ".wav")
                                {
                                    Console.WriteLine("\tSongs in Playlist:");
                                    foreach (Song song in pl.Songs)
                                    {
                                        Console.WriteLine("\t" + song.SearchedInfoSong());
                                    }
                                }
                                if (pl.Format == ".mp4" || pl.Format == ".mov")
                                {
                                    Console.WriteLine("\tVideos in Playlist:");
                                    foreach (Video video in pl.Videos)
                                    {
                                        Console.WriteLine("\t" + video.SearchedInfoVideo());
                                    }
                                }
                            }
                        }
                        else Console.WriteLine("You don´t follow anyone Playlist.");
                        Console.WriteLine("-------------------------------------------------------");
                        Thread.Sleep(1000);
                        if (userProfile.CreatedPlaylist.Count() != 0)
                        {
                            Console.WriteLine("Personal Playlist:");
                            foreach (PlayList playList in userProfile.CreatedPlaylist)
                            {
                                Console.WriteLine(playList.DisplayInfoPlayList());
                                if (playList.Format == ".mp3" || playList.Format == ".wav")
                                {
                                    Console.WriteLine("\tSongs in Playlist:");
                                    foreach (Song song in playList.Songs)
                                    {
                                        Console.WriteLine("\t" + song.SearchedInfoSong());
                                    }
                                }
                                if (playList.Format == ".mp4" || playList.Format == ".mov")
                                {
                                    Console.WriteLine("\tVideos in Playlist:");
                                    foreach (Video video in playList.Videos)
                                    {
                                        Console.WriteLine("\t" + video.SearchedInfoVideo());
                                    }
                                }
                                Console.WriteLine("\n");
                            }
                        }
                        else Console.WriteLine("You don´t have any personal Playlist.");
                        Console.WriteLine("-------------------------------------------------------");
                        Thread.Sleep(1000);

                        Console.WriteLine("Global Playlist:");
                        DisplayGlobalMult(2, database);

                        Thread.Sleep(6000);
                        Console.WriteLine("-------------------------------------------------------");
                        Console.WriteLine("\n");
                        Console.Clear();
                        break;
                    case "IV":
                        Console.Clear();
                        Console.WriteLine("---------------------------");
                        Console.Write("Which settings do you want to acces?(Account/Profile): ");
                        string settings = Console.ReadLine();
                        if (settings == "Account")
                        {
                            AccountSettings(usr);
                            Console.WriteLine("Following list:");
                            foreach (String username in usr.FollowingList)
                            {
                                Console.WriteLine(username);
                            }
                            Console.WriteLine("Do you want to change password? (y/n)");
                            string pass = Console.ReadLine();
                            if (pass == "y") server.ChangePassword(listUserGlobal);
                            else
                            {
                                Console.Clear();
                                break;
                            }
                            //Console.Clear();
                        }
                        else if (settings == "Profile") ProfileSettings(userProfile);
                        else Console.WriteLine("ERROR[!] Invalid Command");
                        Thread.Sleep(1000);
                        Console.WriteLine("---------------------------");
                        Console.Clear();
                        break;

                    case "V":
                        Console.Clear();
                        Console.WriteLine("What playlist do you want to play?(GlobalPlayLists, FollowedPlaylists or FavoritePlayList)");
                        string play;
                        do
                        {
                            play = Console.ReadLine();
                        } while (play != "GlobalPlayLists" && play != "FollowedPlaylists" && play != "FavoritePlayList");

                        if (play == "FavoritePlayList")
                        {

                            Console.Write("Do you want to listen to a song (s) or watch a video (v)?: ");
                            string op;
                            do
                            {
                                op = Console.ReadLine();
                                if ((op != "s" && op != "v") && (op != "S" && op != "V"))
                                    Console.Write("ERROR[!] ~Select a valid command please");
                            } while ((op != "s" && op != "v") && (op != "S" && op != "V"));

                            if (op == "s")
                            {
                                if (user.Accountype == "premium")
                                {
                                    Console.WriteLine("Do you want to play a random song? or select it from the playlist? (r/s): ");
                                    string rndSel;
                                    do
                                    {
                                        rndSel = Console.ReadLine();
                                        if (rndSel != "r" && rndSel != "s")
                                            Console.Write("ERROR [!] ~Select a valid command please (r/s): ");
                                    } while (rndSel != "r" && rndSel != "s");

                                    if (rndSel == "r")
                                    {
                                        player.PlaySong(userProfile.PlaylistFavoritosSongs[player.RandomMult(userProfile, 0, "Fav")], userProfile.PlaylistFavoritosSongs, database, user, userProfile);
                                    }
                                    else
                                    {
                                        for (int i = 0; i < userProfile.PlaylistFavoritosSongs.Count(); i++)
                                        {
                                            Console.WriteLine("{0}) {1}", i + 1, userProfile.PlaylistFavoritosSongs[i].Name);
                                        }
                                        Console.Write("Escoga una cancion: ");
                                        int index = int.Parse(Console.ReadLine()) - 1;
                                        player.PlaySong(userProfile.PlaylistFavoritosSongs[index], userProfile.PlaylistFavoritosSongs, database, user, userProfile);
                                    }

                                }
                                else
                                {
                                    player.PlaySong(userProfile.PlaylistFavoritosSongs[player.RandomMult(userProfile, 0, "Fav")], userProfile.PlaylistFavoritosSongs, database, user, userProfile);
                                }

                            }
                            else if (op == "v")
                            {
                                if (user.Accountype == "premium")
                                {
                                    Console.Write("Do you want to play a random video? or select it from the playlist? (r/v): ");
                                    string rndSel;
                                    do
                                    {
                                        rndSel = Console.ReadLine();
                                        if (rndSel != "r" && rndSel != "v")
                                            Console.Write("ERROR [!] ~Select a valid command please (r/v): ");
                                    } while (rndSel != "r" && rndSel != "v");

                                    if (rndSel == "r")
                                    {
                                        player.PlayVideo(userProfile.PlaylistFavoritosVideos[player.RandomMult(userProfile, 1, "Fav")], userProfile.PlaylistFavoritosVideos, database, user, userProfile);
                                    }
                                    else
                                    {
                                        for (int i = 0; i < userProfile.PlaylistFavoritosVideos.Count(); i++)
                                        {
                                            Console.WriteLine("{0}) {1}", i + 1, userProfile.PlaylistFavoritosVideos[i].Name);
                                        }
                                        Console.Write("Escoga un video: ");
                                        int index = int.Parse(Console.ReadLine()) - 1;
                                        player.PlayVideo(userProfile.PlaylistFavoritosVideos[index], userProfile.PlaylistFavoritosVideos, database, user, userProfile);
                                    }
                                }
                                else
                                {
                                    player.PlayVideo(userProfile.PlaylistFavoritosVideos[player.RandomMult(userProfile, 1, "Fav")], userProfile.PlaylistFavoritosVideos, database, user, userProfile);
                                }
                            }
                            else
                            {
                                Console.WriteLine("ERROR[!]~How did you get here? WTF");
                            }
                        }
                        else if (play == "FollowedPlaylist")
                        {
                            Console.Write("Do you wish to play a song (s) or a video (v)?");
                            string sv;
                            do
                            {
                                sv = Console.ReadLine();
                                if (sv != "s" && sv != "v")
                                    Console.Write("ERROR[!]~ Select a Valid command please: ");
                            } while (sv != "s" && sv != "v");
                            if (sv == "s")
                            {
                                int i = 0;
                                foreach (PlayList p in userProfile.FollowedPlayList)
                                {
                                    Console.WriteLine("{0}) {1}", i + 1, p.NamePlayList);
                                    i++;
                                }
                                Console.Write("Type the playlist that you want to play: ");
                                int numPl = int.Parse(Console.ReadLine()) - 1;
                                string nPl = userProfile.FollowedPlayList[numPl].NamePlayList;
                                if (user.Accountype == "Premium")
                                {
                                    Console.Write("Do you want to play a random song? or select it? (r/s): ");
                                    string rndSel;
                                    do
                                    {
                                        rndSel = Console.ReadLine();
                                        if (rndSel != "r" && rndSel != "s")
                                            Console.Write("ERROR [!] ~Select a valid command please (r/s): ");
                                    } while (rndSel != "r" && rndSel != "s");

                                    if (rndSel == "r")
                                    {
                                        int rnd = player.RandomMult(userProfile, 0, "Foll");
                                        player.PlaySong(userProfile.FollowedPlayList[rnd].DicCanciones[nPl][rnd], userProfile.FollowedPlayList[rnd].DicCanciones[nPl], database, user, userProfile);
                                    }
                                    else
                                    {
                                        for (int j = 0; j < userProfile.FollowedPlayList[numPl].DicCanciones[nPl].Count(); j++)
                                        {
                                            Console.WriteLine("{0}) {1}", j + 1, userProfile.FollowedPlayList[numPl].DicCanciones[nPl][j].Name);
                                        }

                                        Console.Write("Choose a song: ");
                                        int index = int.Parse(Console.ReadLine()) - 1;
                                        player.PlaySong(userProfile.FollowedPlayList[numPl].DicCanciones[nPl][index], userProfile.FollowedPlayList[numPl].DicCanciones[nPl], database, user, userProfile);
                                    }
                                }
                                else
                                {
                                    int rnd = player.RandomMult(userProfile, 0, "Foll");
                                    player.PlaySong(userProfile.FollowedPlayList[rnd].DicCanciones[nPl][rnd], userProfile.FollowedPlayList[rnd].DicCanciones[nPl], database, user, userProfile);
                                }
                            }
                            else
                            {
                                int i = 0;
                                foreach (PlayList p in userProfile.FollowedPlayList)
                                {
                                    Console.WriteLine("{0}) {1}", i + 1, p.NamePlayList);
                                    i++;
                                }
                                Console.Write("Type the playlist that you want to play: ");
                                int numPl = int.Parse(Console.ReadLine()) - 1;
                                string nPl = userProfile.FollowedPlayList[numPl].NamePlayList;
                                if (user.Accountype == "Premium")
                                {
                                    Console.Write("Do you want to play a random video? or select it? (r/v): ");
                                    string rndSel;
                                    do
                                    {
                                        rndSel = Console.ReadLine();
                                        if (rndSel != "r" && rndSel != "v")
                                            Console.Write("ERROR [!] ~Select a valid command please (r/v): ");
                                    } while (rndSel != "r" && rndSel != "v");

                                    if (rndSel == "r")
                                    {
                                        int rnd = player.RandomMult(userProfile, 1, "Foll");
                                        player.PlayVideo(userProfile.FollowedPlayList[rnd].DicVideos[nPl][rnd], userProfile.FollowedPlayList[rnd].DicVideos[nPl], database, user, userProfile);
                                    }
                                    else
                                    {
                                        for (int j = 0; j < userProfile.FollowedPlayList[numPl].DicVideos[nPl].Count(); j++)
                                        {
                                            Console.WriteLine("{0}) {1}", j + 1, userProfile.FollowedPlayList[numPl].DicVideos[nPl][j].Name);
                                        }

                                        Console.Write("Choose a video: ");
                                        int index = int.Parse(Console.ReadLine()) - 1;
                                        player.PlayVideo(userProfile.FollowedPlayList[numPl].DicVideos[nPl][index], userProfile.FollowedPlayList[numPl].DicVideos[nPl], database, user, userProfile);
                                    }
                                }
                                else
                                {
                                    int rnd = player.RandomMult(userProfile, 1, "Foll");
                                    player.PlayVideo(userProfile.FollowedPlayList[rnd].DicVideos[nPl][rnd], userProfile.FollowedPlayList[rnd].DicVideos[nPl], database, user, userProfile);
                                }
                            }
                        }
                        else if (play == "GlobalPlayLists")
                        {
                            Console.Write("Do you wish to play a song (s) or a video (v)?");
                            string sv;
                            do
                            {
                                sv = Console.ReadLine();
                                if (sv != "s" && sv != "v")
                                    Console.Write("ERROR[!]~ Select a Valid command please: ");
                            } while (sv != "s" && sv != "v");
                            if (sv == "s")
                            {
                                int i = 0;
                                foreach (PlayList p in listPlayListGlobal)
                                {
                                    Console.WriteLine("{0}) {1}", i + 1, p.NamePlayList);
                                    i++;
                                }
                                Console.Write("Type the playlist that you want to play: ");
                                int numPl = int.Parse(Console.ReadLine()) - 1;
                                string nPl = listPlayListGlobal[numPl].NamePlayList;
                                if (user.Accountype == "Premium")
                                {
                                    Console.Write("Do you want to play a random song? or select it? (r/s): ");
                                    string rndSel;
                                    do
                                    {
                                        rndSel = Console.ReadLine();
                                        if (rndSel != "r" && rndSel != "s")
                                            Console.Write("ERROR [!] ~Select a valid command please (r/s): ");
                                    } while (rndSel != "r" && rndSel != "s");

                                    if (rndSel == "r")
                                    {
                                        int rnd = player.RandomMult(userProfile, 0, "Gl");
                                        player.PlaySong(listPlayListGlobal[rnd].DicCanciones[nPl][rnd], listPlayListGlobal[rnd].DicCanciones[nPl], database, user, userProfile);
                                    }
                                    else
                                    {
                                        for (int j = 0; j < listPlayListGlobal[numPl].DicCanciones[nPl].Count(); j++)
                                        {
                                            Console.WriteLine("{0}) {1}", j + 1, listPlayListGlobal[numPl].DicCanciones[nPl][j].Name);
                                        }

                                        Console.Write("Choose a song: ");
                                        int index = int.Parse(Console.ReadLine()) - 1;
                                        player.PlaySong(listPlayListGlobal[numPl].DicCanciones[nPl][index], listPlayListGlobal[numPl].DicCanciones[nPl], database, user, userProfile);
                                    }
                                }
                                else
                                {
                                    int rnd = player.RandomMult(userProfile, 0, "Gl");
                                    player.PlaySong(listPlayListGlobal[rnd].DicCanciones[nPl][rnd], userProfile.FollowedPlayList[rnd].DicCanciones[nPl], database, user, userProfile);
                                }
                            }
                            else
                            {
                                int i = 0;
                                foreach (PlayList p in listPlayListGlobal)
                                {
                                    Console.WriteLine("{0}) {1}", i + 1, p.NamePlayList);
                                    i++;
                                }
                                Console.Write("Type the playlist that you want to play: ");
                                int numPl = int.Parse(Console.ReadLine()) - 1;
                                string nPl = listPlayListGlobal[numPl].NamePlayList;
                                if (user.Accountype == "Premium")
                                {
                                    Console.Write("Do you want to play a random video? or select it? (r/v): ");
                                    string rndSel;
                                    do
                                    {
                                        rndSel = Console.ReadLine();
                                        if (rndSel != "r" && rndSel != "v")
                                            Console.Write("ERROR [!] ~Select a valid command please (r/v): ");
                                    } while (rndSel != "r" && rndSel != "v");

                                    if (rndSel == "r")
                                    {
                                        int rnd = player.RandomMult(userProfile, 1, "Gl");
                                        player.PlayVideo(listPlayListGlobal[rnd].DicVideos[nPl][rnd], listPlayListGlobal[rnd].DicVideos[nPl], database, user, userProfile);
                                    }
                                    else
                                    {
                                        for (int j = 0; j < listPlayListGlobal[numPl].DicVideos[nPl].Count(); j++)
                                        {
                                            Console.WriteLine("{0}) {1}", j + 1, listPlayListGlobal[numPl].DicVideos[nPl][j].Name);
                                        }

                                        Console.Write("Choose a video: ");
                                        int index = int.Parse(Console.ReadLine()) - 1;
                                        player.PlayVideo(listPlayListGlobal[numPl].DicVideos[nPl][index], listPlayListGlobal[numPl].DicVideos[nPl], database, user, userProfile);
                                    }
                                }
                                else
                                {
                                    int rnd = player.RandomMult(userProfile, 1, "Gl");
                                    player.PlayVideo(listPlayListGlobal[rnd].DicVideos[nPl][rnd], listPlayListGlobal[rnd].DicVideos[nPl], database, user, userProfile);
                                }
                            }
                        }
                        break;

                    case "VI":
                        Console.WriteLine("Logged Out");
                        ret = 0;
                        database.Save_Users(listUserGlobal);
                        x = false;
                        Console.Clear();
                        break;
                    case "VII":
                        ret = 1;
                        database.Save_Users(listUserGlobal);
                        x = false;
                        break;
                    case "VIII":
                        //Menu admin
                        Console.Clear();
                        if (usr.Accountype == "admin")
                        {
                            Console.Clear();
                            Console.WriteLine("------Admin Menu-------");
                            Console.WriteLine("I)Erase Users.\nII)Ban Erase.");
                            string admin = Console.ReadLine();
                            switch (admin)
                            {
                                case "I":
                                    Console.WriteLine("Type the username, the user you want to delete...");
                                    string username = Console.ReadLine();
                                    foreach (User usernombre in listUserGlobal)
                                    {
                                        if (username == usernombre.Username)
                                        {
                                            listUserGlobal.Remove(usernombre);
                                            Console.WriteLine("Delete succesfull");
                                            break;
                                        }
                                    }
                                    break;
                                case "II":
                                    int y = 0;
                                    Console.WriteLine("Type the username, the user you want to ban...");
                                    string user = Console.ReadLine();
                                    foreach (User usernombre in listUserGlobal)
                                    {
                                        if (user == usernombre.Username && usernombre.Accountype == "premium")
                                        {
                                            listUserGlobal[y].Accountype.Replace("premium", "standard");
                                            Console.WriteLine("Ban succesfull");
                                            break;
                                        }
                                        y++;
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
            return ret;
        }

        //tenemos que decidir si esta clase sera de inputs y outputs, o la que hace de reproductor.
        public void DisplayGlobalMult(int typeMult, DataBase database)
        {
            if (typeMult == 0)
            {
                List<Song> ListSongsGlobal = database.Load_Songs();
                for (int i = 0; i < ListSongsGlobal.Count(); i++)
                {
                    Console.WriteLine("Cancion {0}", i + 1);
                    Console.WriteLine(ListSongsGlobal[i].DisplayInfoSong() + "\n");
                }
            }
            else if (typeMult == 1)
            {
                List<Video> ListVideosGlobal = database.Load_Videos();
                for (int i = 0; i < ListVideosGlobal.Count(); i++)
                {
                    Console.WriteLine("Video {0}", i + 1);
                    Console.WriteLine(ListVideosGlobal[i].DisplayInfoVideo() + "\n");
                }
            }
            else if (typeMult == 2)
            {
                List<PlayList> ListPLsGlobal = database.Load_PLs();
                for (int i = 0; i < ListPLsGlobal.Count(); i++)
                {
                    Console.WriteLine("Playlist {0}", i + 1);
                    Console.WriteLine(ListPLsGlobal[i].DisplayInfoPlayList() + "\n");
                }
            }

        }

        public void DisplayPlaylists(List<PlayList> playlist)
        {
            for (int i = 0; i < playlist.Count(); i++)
            {
                Console.WriteLine(i + ") " + playlist[i].InfoPlayList());
            }
        }
        public void AccountSettings(User user)
        {
            Console.WriteLine("Username: " + user.AccountSettings()[0] + "\n");
            Console.WriteLine("Password: " + user.AccountSettings()[1] + "\n");
            Console.WriteLine("Email: " + user.AccountSettings()[2] + "\n");
            Console.WriteLine("Account type: " + user.AccountSettings()[3] + "\n");
            Console.WriteLine("Followers: " + user.AccountSettings()[4] + "\n");
            Console.WriteLine("Following: " + user.AccountSettings()[5] + "\n");

        }
        public void ProfileSettings(Profile profile)
        {
            Console.WriteLine("Name: " + profile.ProfileSettings()[0] + "\n");
            Console.WriteLine("Profile Type: " + profile.ProfileSettings()[1] + "\n");
            Console.WriteLine("Profile Pic: " + profile.ProfileSettings()[2] + "\n");
            Console.WriteLine("Gender: " + profile.ProfileSettings()[3] + "\n");
            Console.WriteLine("Age: " + profile.ProfileSettings()[4] + "\n");
        }

        //Cada vez que termine el ciclo de algun archivo multimedia consultar al perfil si se le da like o no.
        /*public void Reproduction(int inplaylist, string type, int indexofmultimedia, bool ver, DataBase dataBase) // Si viene de una playlist y se decide poner aleatorio verif sera 4, si se elige una canción sera 1.
        {
            if (inplaylist == 1)
            {
                List<Song> listSongsGlobal = dataBase.Load_Songs();
                int x = 0;
                int cont = 0;
                if (type == "Song") {
                    while (cont != -1)
                    {
                        cont = player.PlaySong(cont, listSongsGlobal[indexofmultimedia], ver); //Devuelve el tiempo en el que se para la canción
                        if (cont != -1) cont = player.Stop(cont); // devuelve el contador cuando se detiene para empezar de nuevo.
                    }
                    
                }
                else
                {
                    List<Video> listVideosGlobal = dataBase.ListVideosGlobal;
                    while (cont != -1)
                    {
                        cont = player.PlayVideo(cont, listVideosGlobal[indexofmultimedia], ver); //Devuelve el tiempo en el que se para la canción
                        if (cont != -1) cont = player.Stop(cont); // devuelve el contador cuando se detiene para empezar de nuevo.
                    }
                }
                
            }
            else
            {
                //player.Random(playlist);
                Console.WriteLine("Reproduce cancion random de la playlist"); 
            }
        }*/

        public List<string> SearchEngine(string searching, string type, DataBase dataBase)
        {

            List<string> searchEngine = new List<string>();
            int num_v = 0;


            if (type == "Songs")
            {
                List<Song> listSongsGlobal = dataBase.Load_Songs();
                for (int i = 0; i < listSongsGlobal.Count(); i++)
                {
                    for (int x = 0; x < listSongsGlobal[i].InfoSong().Count(); x++)
                    {
                        if (listSongsGlobal[i].InfoSong()[x] == searching)
                        {
                            searchEngine.Add(listSongsGlobal[i].SearchedInfoSong() + ", Position: " + (i + 1)); //num_s es un int y no me patalea, si tira error es aca. Usar remove leguer
                        }
                    }
                }
            }
            else if (type == "Videos")
            {
                List<Video> listVideosGlobal = dataBase.Load_Videos();
                for (int i = 0; i < listVideosGlobal.Count(); i++)
                {
                    for (int x = 0; x < listVideosGlobal[i].InfoVideo().Count(); x++)
                    {
                        if (listVideosGlobal[i].InfoVideo()[x] == searching)
                        {
                            searchEngine.Add(listVideosGlobal[i].SearchedInfoVideo() + ", Position: " + (i + 1)); //num_s es un int y no me patalea, si tira error es aca. Usar remove leguer
                        }
                    }
                }
            }
            else if (type == "Users")
            {
                List<User> diceUserGlobal = dataBase.Load_Users();

                for (int i = 0; i < diceUserGlobal.Count(); i++)
                {
                    for (int x = 0; x < diceUserGlobal[i].infoUser().Count(); x++)
                    {
                        if (diceUserGlobal[i].infoUser()[x] == searching)
                        {
                            if (diceUserGlobal[i].Privacy != true) searchEngine.Add(diceUserGlobal[i].SearchedInfoUser() + ", Position: " + (i + 1));
                            else if (diceUserGlobal[i].Privacy == true) searchEngine.Add(diceUserGlobal[i].SearchedInfoUser() + ", Position: " + "???");
                        }
                    }
                }
            }
            if (searchEngine.Count() == 0) Console.WriteLine("No match found...");
            return searchEngine;
        }

        public void DisplayHistory(List<Song> searchStorySongs, List<Video> searchStoryVideos)
        {
            if (searchStorySongs.Count() < 5 || searchStoryVideos.Count() < 5)
            {
                for (int i = 0; i < searchStorySongs.Count(); i++)
                {
                    Console.WriteLine(searchStorySongs[i]); //Recordar que cada eleemento de estas listas van a ser la información de cada archivo multimedia.
                }
                for (int i = 0; i < searchStoryVideos.Count(); i++)
                {
                    Console.WriteLine(searchStoryVideos[i]);
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine(searchStorySongs[i]);
                }
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine(searchStoryVideos[i]);
                }
            }
        }

        public List<string> AskInfoMult(int type)
        {
            List<string> infoMult = new List<string>();
            if (type == 0)
            {
                Console.Write("Song name: "); string n = Console.ReadLine();
                Console.Write("Song artist or artists(if this is the case separate them by '-', Bad Bunny-Ricky Martin: "); string art = Console.ReadLine();
                Console.Write("Album: "); string alb = Console.ReadLine();
                Console.Write("Discography: "); string disc = Console.ReadLine();
                Console.Write("Song Gender: "); string gen = Console.ReadLine();
                Console.Write("Publish date (dd/mm/aa): "); string date = Console.ReadLine();
                Console.Write("Studio: "); string std = Console.ReadLine();
                Console.Write("Song Duration (format: min.seg)(double): "); string dur = Console.ReadLine();
                string format;
                do
                {
                    Console.Write("Song Format(.mp3 || .wav): "); format = Console.ReadLine();
                } while (format != ".mp3" && format != ".wav");
                Console.Write("Song lyrics(write: We will, we will rock you!!): ");
                string lyr = Console.ReadLine();

                infoMult = new List<string>() { n, art, alb, disc, gen, date, std, dur, lyr, format };
            }
            else if (type == 1)
            {
                Console.Write("Video Name: "); string n = Console.ReadLine();
                Console.Write("Video actor or actors(if this is the case separate them by '-', Bad Bunny-Ricky Martin: "); string act = Console.ReadLine();
                Console.Write("Video director or directors(if this is the case separate them by '-', Bad Bunny-Ricky Martin: "); string dir = Console.ReadLine();
                Console.Write("Publish date (dd/mm/aa): "); string date = Console.ReadLine();
                Console.Write("Video Dimension (16:9): "); string dim = Console.ReadLine();
                Console.Write("Video Quality"); string cal = Console.ReadLine();
                Console.Write("Video category(number) (0 = all espectator, 16 = above 16 years, etc.): "); string cat = Console.ReadLine();
                Console.Write("Video Description: "); string des = Console.ReadLine();
                Console.Write("Video Duration(format: min.seg)(double): "); string dur = Console.ReadLine();
                string format;
                do
                {
                    Console.Write("Video Format(.mp4 || .mov): "); format = Console.ReadLine();
                } while (format != ".mp4" && format != ".mov");
                string im;
                do
                {
                    Console.Write("Video Image(y/n): "); im = Console.ReadLine();
                } while (im != "y" && im != "n");

                Console.Write("Video subtitles(write): "); string sub = Console.ReadLine();

                infoMult = new List<string>() { n, act, dir, date, dim, cal, cat, des, im, dur, sub, format };
            }
            else if (type == 2)
            {
                Console.Write("Playlist Name: "); string n = Console.ReadLine();
                string choice;
                do
                {
                    Console.Write("Do you want your playlist with songs or videos? (c/v): "); choice = Console.ReadLine();
                } while (choice != "c" && choice != "v");
                string format = null;

                if (choice == "c" || choice == "C")
                {
                    do
                    {
                        Console.Write("Song Format(.mp3 || .wav): "); format = Console.ReadLine();
                    } while (format != ".mp3" && format != ".wav");
                }
                else if (choice == "v" || choice == "V")
                {
                    do
                    {
                        Console.Write("Video Format(.mp4 || .mov): "); format = Console.ReadLine();
                    } while (format != ".mp4" && format != ".mov");
                }

                infoMult = new List<string>() { n, format };
            }
            return infoMult;

        }
    }
}
