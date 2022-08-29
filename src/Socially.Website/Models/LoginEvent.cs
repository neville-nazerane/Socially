using System;

namespace Socially.Website.Models
{
    public class LoginEvent : EventArgs
    {

        public LoginEvent(UserData data)
        {
            Data = data;
        }

        public UserData Data { get; }
    }
}
