﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FyBuzz_E2
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            //INSTANCIAS CREADAS
            DataBase dataBase = new DataBase();
            Menu menu = new Menu();
            User LogInUser;

            //LISTA DE CANCIONES, VIDEOS Y PLAYLISTS BASE
            List<Song> baseListSong = new List<Song>(){ new Song("Safaera","Bad Bunny", "YHLQMDLM","Rimas entertainment LLC", "Trap","20/01/2020","BB Rcds.", 4.9, "Tú tiene' un culo cabrón",".mp3"),
                                                        new Song("MAS DE UNA CITA", "Zion & Lenox", "LAS QUE NO IBAN A SALIR", "Rimas entertainment LLC", "Trap", "10/05/2020", "Z&L Rcds.", 3.5, "Se necesita, ey, más de una cita, ey", ".wav") };
            
            List<Video> baseListVideo = new List<Video>() { new Video("United","Tom Holland-Cris Pratt","Disney", "", "16:9" ,"1080x1920", "0","Movie", false,120,"i love you",".mp4"), 
                                                            new Video("Create a C# App from start to finish","freecodecamp.org","freecodecamp.org","12/12/2019","16:9","1080x1920","16","C# Course",true,1440,"fuck", ".mov") };
            
            List<PlayList> baseListPLs = new List<PlayList>() { new PlayList("Programming hard", ".mp3","FyBuZz", "FyBuZz"),
                                                                new PlayList("TikToks that cured my depression", ".mp4", "FyBuZz", "FyBuZz") };
            int i = 0;
            foreach (PlayList pl in baseListPLs)
            {

                pl.Songs.Add(baseListSong[i]);
                pl.Videos.Add(baseListVideo[i]);
                i++;      
            }
            
            List<PlayList> basePrivPLs = new List<PlayList>() { new PlayList("", "", "", "") };
            
            //LISTA CON OBJETO VACIO DE CANCIONES DESCARGADAS
            List<Song> downloadSongs = new List<Song>() { new Song("", "", "", "", "", "", "", 0, "", "") };

            //LISTA CON OBJETO VACIO DE USUARIOS
            List<User> baseListUser = new List<User>() {new User()};

            //LISTAS DATABASE
            List<User> userDataBase = new List<User>();
            List<Song> songDataBase = new List<Song>();
            List<Video> videoDataBase = new List<Video>();
            List<PlayList> playlistDataBase = new List<PlayList>();
            List<PlayList> playlistPrivDataBase = new List<PlayList>();
            List<Song> downloads = new List<Song>();

            //Condiciones para ver si el archivo existe o no.
            if (File.Exists("AllSongs.bin") != true)
                dataBase.Save_Songs(baseListSong);

            if (File.Exists("AllVideos.bin") != true)
                dataBase.Save_Videos(baseListVideo);

            if (File.Exists("AllPlayLists.bin") != true)
                dataBase.Save_PLs(baseListPLs);

            if (File.Exists("PrivatePlayLists.bin") != true)
                dataBase.Save_PLs_Priv(basePrivPLs);

            if (File.Exists("DownloadSongs.bin") != true)
                dataBase.Save_DSongs(downloadSongs);

            if (File.Exists("AllUsers.bin") != true)
                dataBase.Save_Users(baseListUser);


            int ret = 0;

            //ZONA DE CARGA DE ARCHIVOS
            userDataBase = dataBase.Load_Users();
            songDataBase = dataBase.Load_Songs();
            videoDataBase = dataBase.Load_Videos();
            playlistDataBase = dataBase.Load_PLs();
            downloads = dataBase.Load_DSongs();
            playlistPrivDataBase = dataBase.Load_PLs_Priv();

            //MAIN CODE
            LogInUser = menu.DisplayLogin(userDataBase);
            if (LogInUser != null)
            {
                while (ret == 0)
                {
                    Profile profileMain = menu.DisplayProfiles(LogInUser, userDataBase);
                    if (profileMain != null) ret = menu.DisplayStart(profileMain, LogInUser, userDataBase, songDataBase, downloads, videoDataBase, playlistDataBase, playlistPrivDataBase);
                    else break;
                }
            }

            //ZONA DE GUARDADO DE ARCHIVOS
            dataBase.Save_Users(userDataBase);
            dataBase.Save_Songs(songDataBase);
            dataBase.Save_Videos(videoDataBase);
            dataBase.Save_PLs(playlistDataBase);
            dataBase.Save_PLs_Priv(playlistPrivDataBase);

        }
    }
}
