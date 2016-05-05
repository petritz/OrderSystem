using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Data
{
    public class Session
    {
        private static Session instance;

        private readonly int id;
        private readonly string email;

        // Init

        static Session()
        {
        }

        private Session(int id, string email)
        {
            this.id = id;
            this.email = email;
        }

        // Static Functions

        public static Session CreateSession(int id, string email)
        {
            instance = new Session(id, email);
            return instance; //to allow chaining
        }

        public static void DeleteSession()
        {
            instance = null;
            Storage.Instance.Remove("email");
            Storage.Instance.Remove("password");
            Storage.Instance.Save();
        }

        // Function

        public int CurrentUserId
        {
            get { return id; }
        }

        public string CurrentUserEmail
        {
            get { return email; }
        }

        public void Save(string password)
        {
            Storage.Instance.Set("email", email);
            Storage.Instance.Set("password", password);
            Storage.Instance.Save();
        }

        // Properties

        public bool IsValidSession
        {
            get { return instance != null; }
        }

        public static Session Instance
        {
            get { return instance; }
        }
    }
}