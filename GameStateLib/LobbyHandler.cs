using System;
using System.Collections.Generic;
using System.Text;

namespace GameStateLib
{
    class LobbyHandler
    {
        public List<Lobby> lobbies = new List<Lobby>();
        public bool CreateLobby(Player lobbyCreator)
        {
            lobbies.Add(new Lobby(lobbyCreator));
            return true;
        }
    }
    class Lobby
    {
        public List<Player> players = new List<Player>();
        public Lobby(Player lobbyCreator)
        {
            players.Add(lobbyCreator);
            lobbyCreator.PromoteToGM();
        }
        public bool AddPlayerToLobby(Player newPlayer)
        {
            players.Add(newPlayer);
            return true;
        }
    }
}
