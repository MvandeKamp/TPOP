using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace GameStateLib
{
    [SerializableAttribute]
    public class Player
    {
        TcpClient Sender { get; set; } //used server only
        public string Username { get; set; }
        public string Password { get; set; }
        public MembershipTypes Membership { get; set; }

        public Player(string username, string password, MembershipTypes membershipType = MembershipTypes.Player)
        {
            Username = username;
            Password = password;
            Membership = membershipType;
        }
        public void PromoteToGM()
        {
            Membership = MembershipTypes.Gamemoderator;
        }
        public enum MembershipTypes
        {
            Player,
            Gamemoderator
        }
    }
}
