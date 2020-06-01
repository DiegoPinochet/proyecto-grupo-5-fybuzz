﻿using Entrega3_FyBuZz.CustomArgs;
using Modelos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Entrega3_FyBuZz.Controladores
{
    public class PlaylistController
    {
        List<PlayList> playlistDataBase = new List<PlayList>() { new PlayList("Programming hard", ".wav","FyBuZz", "FyBuZz"),
                                                                 new PlayList("FyBuZz Global Songs",".mp3","FyBuZz","FyBuZz"),
                                                                 new PlayList("FyBuZz Global Videos",".mp4","FyBuZz","FyBuZz")};
        
        List<PlayList> privatePlaylistsDatabase = new List<PlayList>() { new PlayList("","","","")};
        DataBase dataBase = new DataBase();
        FyBuZz fyBuZz;


        public PlaylistController(Form fyBuZz)
        {
            Initialize();
            this.fyBuZz = fyBuZz as FyBuZz;
            this.fyBuZz.DisplayPlaylistsGlobalPlaylist_Clicked += OnDisplayPlaylistsGlobalPlaylist_Clicked;
            this.fyBuZz.CreatePlaylistCreatePlaylistButton_Clicked += CreatePlaylistButton_Clicked;
            this.fyBuZz.PlaySongChoosePlsButton_Clicked += PlaySongChoosePlsButton_Clicked;
        }
        public void Initialize()
        {
            //Agrega multimedia a las playlist que vienen con el programa
            playlistDataBase[0].Songs.Add(new Song("Aplausos durante el confinamiento", "la gente", "covid 2020", "covid disc", "Indie", "12/05/2020", "covid stdio", 4, "[Insert Aplausos]", ".wav", "aplausosduranteelconfinamiento_01.wav"));
            playlistDataBase[1].Songs.Add(new Song("Safaera", "Bad Bunny", "YHLQMDLM", "Rimas entertainment LLC", "Trap", "20/01/2020", "BB Rcds.", 4.9, "Tú tiene' un culo cabrón", ".mp3", "Bad Bunny ft Jowell & Randy ft Ñengo Flow - Safaera.mp3"));
            playlistDataBase[1].Songs.Add(new Song("MAS DE UNA CITA", "Bad Bunny Zion & Lenox", "LAS QUE NO IBAN A SALIR", "Rimas entertainment LLC", "Trap", "10/05/2020", "Z&L Rcds.", 3.5, "Se necesita, ey, más de una cita, ey", ".mp3", "02-Bad-Bunny-Zion-Lennox-MÁS-DE-UNA-CITA.mp3"));
            //playlistDataBase[2].Videos.Add(); Agregar videos.
            if (File.Exists("AllPlaylists.bin") != true) dataBase.Save_PLs(playlistDataBase);
            playlistDataBase = dataBase.Load_PLs();
            if (File.Exists("PrivatePlaylists.bin") != true) dataBase.Save_PLs_Priv(privatePlaylistsDatabase);
            privatePlaylistsDatabase = dataBase.Load_PLs_Priv();

        }

        private List<PlayList> OnDisplayPlaylistsGlobalPlaylist_Clicked(object sender, PlaylistEventArgs e)
        {
            return playlistDataBase;
        }

        private string CreatePlaylistButton_Clicked(object sender, PlaylistEventArgs e)
        {
            List<string> infoMult = new List<string> { e.NameText, e.FormatText};

            string privacy = null;
            if(e.PrivacyText == true)
            {
                privacy = "y";
            }
            else
            {
                privacy = "n";
            }

            string description = dataBase.AddMult(2, infoMult, null, playlistDataBase, null, e.CreatorText.Username, e.ProfileCreatorText.ProfileName, privacy, privatePlaylistsDatabase );
            foreach (PlayList playList in playlistDataBase)
            {
                if (playList.Creator == e.CreatorText.Username && playList.ProfileCreator == e.ProfileCreatorText.ProfileName)
                {
                    if(e.ProfileCreatorText.CreatedPlaylist.Contains(playList) == false) e.ProfileCreatorText.CreatedPlaylist.Add(playList);
                    if(e.CreatorText.ProfilePlaylists.Contains(playList) == false) e.CreatorText.ProfilePlaylists.Add(playList);
                }
            }
            foreach (PlayList playList in privatePlaylistsDatabase)
            {
                if (playList.Creator == e.CreatorText.Username && playList.ProfileCreator == e.ProfileCreatorText.ProfileName)
                {
                    if (e.ProfileCreatorText.CreatedPlaylist.Contains(playList) == false) e.ProfileCreatorText.CreatedPlaylist.Add(playList);
                    //usr.ProfilePlaylists.Add(playList); Si la PL es privada no se agrega al usuario, por lo tanto no se puede seguir.
                }
            }
            dataBase.Save_PLs(playlistDataBase);
            dataBase.Save_PLs_Priv(privatePlaylistsDatabase);
            return description;
        }
        private string PlaySongChoosePlsButton_Clicked(object sender, PlaylistEventArgs e)
        {
            string description = null;
            foreach (PlayList playlist in playlistDataBase)
            {
                if(playlist.NamePlayList == e.SearchedPlaylistNameText)
                {
                    foreach (Song song in e.SongDataBaseText)
                    {
                        string result = e.RestultText;
                        int choosenPl = e.ChoosenIndex;
                        if (result == song.SearchedInfoSong() && e.ProfileCreatorText.CreatedPlaylist[choosenPl].Songs.Contains(song) == false && playlist.Songs.Contains(song) == false)
                        {
                            e.ProfileCreatorText.CreatedPlaylist[choosenPl].Songs.Add(song);
                            playlist.Songs.Add(song);
                            dataBase.Save_PLs(playlistDataBase);
                            dataBase.Save_PLs_Priv(privatePlaylistsDatabase);
                            return description;

                        }
                        else
                        {
                            description = "ERROR[!]";
                        }
                    }
                }
                
            }

            return description;
        }

        //Aqui deberia haber un metodo para ponerle play a la mult de la playlist

    }
}