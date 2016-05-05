using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Data
{
    /// <summary>
    /// Class that can create and delete user sessions. Stores data with the Storage class.
    /// </summary>
    public class Session
    {
        private static Session instance;

        private readonly int id;
        private readonly string email;

        private Session(int id, string email)
        {
            this.id = id;
            this.email = email;
        }

        /// <summary>
        /// Creates the session object (saved in Instance) for the specified user.
        /// </summary>
        /// <param name="id">The id of the user</param>
        /// <param name="email">The email of the user</param>
        /// <returns></returns>
        public static Session CreateSession(int id, string email)
        {
            instance = new Session(id, email);
            return instance; //to allow chaining
        }

        /// <summary>
        /// Deletes the current session. Also removes persistent data for the user.
        /// </summary>
        public static void DeleteSession()
        {
            instance = null;
            Storage.Instance.Remove("email");
            Storage.Instance.Remove("password");
            Storage.Instance.Save();
        }

        /// <summary>
        /// Get the current user id
        /// </summary>
        public int CurrentUserId
        {
            get { return id; }
        }

        /// <summary>
        /// Get the current user email
        /// </summary>
        public string CurrentUserEmail
        {
            get { return email; }
        }

        /// <summary>
        /// Save the data to the disk
        /// </summary>
        /// <param name="password">The password secured as hash (md5)</param>
        public void Save(string password)
        {
            Storage.Instance.Set("email", email);
            Storage.Instance.Set("password", password);
            Storage.Instance.Save();
        }

        /// <summary>
        /// Checks if the session object was created
        /// </summary>
        public bool IsValidSession
        {
            get { return instance != null; }
        }

        /// <summary>
        /// The instance to access the current session
        /// </summary>
        public static Session Instance
        {
            get { return instance; }
        }
    }
}