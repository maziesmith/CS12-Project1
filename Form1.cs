using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app1
{
    public static class PersonFactory
    {
        private static uint MIN_PASSWORD_LENGTH = 8;
        private static int MIN_PASSWORD_REP_CHARS = 4;
        public static Person BuildPerson(
            string t_firstName,
            string t_lastName,
            string t_city,
            string t_userName,
            string t_password,
            uint t_id,
            ISystem t_caller)
        {
            if (string.IsNullOrWhiteSpace(t_firstName)
            || string.IsNullOrWhiteSpace(t_lastName)
            || string.IsNullOrWhiteSpace(t_city)
            || string.IsNullOrWhiteSpace(t_userName)
            || string.IsNullOrWhiteSpace(t_password))
                return null;
            if (t_password.Length < MIN_PASSWORD_LENGTH)
                return null;
            if (((Func<bool>)(() =>
            {
                int limit;
                for (int i = 0; i < t_password.Length - MIN_PASSWORD_REP_CHARS; i++)
                {
                    char test = t_password[i];
                    limit = i + MIN_PASSWORD_REP_CHARS;
                    for (i++; i < limit; i++)
                        if (t_password[i] != test)
                            goto tryNextChar;
                    return true;   // bad password
                    tryNextChar:;
                    continue;
                }
                return false;
            }))())
                return null;
            return new Person(t_firstName, t_lastName, t_city, t_userName, t_id, t_password);
        }
    }
    public static class ErrorHandler
    {
        public enum FatalErrno
        {   // error codes for each error caused by the program; maps to an error message
            PERSON_CONSTRUCT_FAIL = 0,
            PERSON_SERIAL_FAIL = 1,
            PERSON_READ_FAIL = 2,
            DATABASE_READ_FAIL = 3,
            DATABASE_WRITE_FAIL = 4,
            DATABASE_NEW_FILE_FAIL = 5,
            DATABASE_WRITE_TYPE_FAIL = 6,
            GENERAL_ERROR = 7
        };
        private enum Prefixno
        {   // prefix id for error messages; maps to a string
            FATAL = 0,
            WARN = 1
        };
        private static readonly string[] fatalErrMsg =
        {   // contains error messages
            "Malformed person constructor call.",
            "Could not serialize object.",
            "Could not read data from file.",
            "Database could not read from file.",
            "Database could not write to file.",
            "Database could not create new file.",
            "Database tried to write to file but was set to wrong type",
            "Default error"
        };
        private static readonly string[] msgPrefix =
        {   // contains message prefixs
            "FATAL",
            "WARN",
        };
        private static string GetErrorMsg(Prefixno prefixno, FatalErrno errno)
        {   // return a formatted error message
            return string.Concat(msgPrefix[(uint)prefixno], ": ", fatalErrMsg[(uint)errno]);
        }
        public static void AssertFatalError(FatalErrno errno)
        {   // assert with a formatted error message
            throw new Exception(GetErrorMsg(Prefixno.FATAL, errno));
        }
        public static string AlertFatalError(FatalErrno errno)
        {   // get a formatted warning message
            return GetErrorMsg(Prefixno.WARN, errno);
        }
    }
    [Serializable()]    //Person is serializable
    public class Person : ISerializable
    {   // Person class contains data about a person; Person class implements ISerializable
        private const int FRIENDS_LIST_INITAL_SIZE = 5;
        private const int INTERESTS_LIST_INITAL_SIZE = 5;
        private string firstName_;  // Person's fist name
        private string lastName_;   // Person's last name
        private string city_;       // Person's city
        private string userName_;   // Person's user name
        private ulong id_;          // Person's id; maps to a person object in a collection
        public readonly ulong staticID;
        private readonly List<Person> friends_ = new List<Person>(FRIENDS_LIST_INITAL_SIZE); // pre-alloc friends list
        private readonly List<string> interests_ = new List<string>();
        public readonly Password password;  // Person's password object
        public ulong ID
        {   // get/set the user ID
            get { return id_; }
            set { id_ = value; }
        }
        public string FirstName
        {   // get/set the user's first name
            get { return firstName_; }
            set { firstName_ = value; }
        }
        public string LastName
        {   // get/set the user's last name
            get { return lastName_; }
            set { lastName_ = value; }
        }
        public string City
        {   // get/set the user's city
            get { return city_; }
            set { city_ = value; }
        }
        public string UserName
        {   // get/set the user's username
            get { return userName_; }
            set { userName_ = value; }
        }
        public Person(string t_firstName, string t_lastName, string t_city, string t_userName, ulong t_id, string t_password)
        {   // Person constructor;
            if (t_firstName == null
            || t_lastName == null
            || t_city == null
            || t_userName == null
            || t_password == null)  // test if any string parameter is null; if true, assert
                ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.PERSON_CONSTRUCT_FAIL);
            // set all data members
            ID = t_id;
            firstName_ = t_firstName;
            lastName_ = t_lastName;
            city_ = t_city;
            userName_ = t_userName;
            staticID = (ulong)(new TimeSpan(1970, 1, 1).TotalSeconds);
            password = new Password(t_password);    // assign a new password object to the Person's class
        }
        private Person(SerializationInfo info, StreamingContext context)
        {   // Person constructor; used to build a new person object from a blob file
            try
            {   // try to set all data members using data extracted from the blob
                password = (Password)info.GetValue("Password", typeof(Password));
                ID = info.GetUInt64("ID");
                firstName_ = info.GetString("FirstName");
                lastName_ = info.GetString("LastName");
                city_ = info.GetString("City");
                userName_ = info.GetString("UserName");
            }
            catch
            {   // assert on error
                ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.PERSON_READ_FAIL);
            }
        }
        public bool AddFriend(Person t_next)
        {
            foreach(Person p in friends_)
            {
                if (p.staticID == t_next.staticID)
                    return false;
            }
            friends_.Add(t_next);
            return true;
        }
        //public bool SendInvite(Invite t_next)
        //{
        //}
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {   // called when saving a person object to disk
            try
            {   // try to add all data members to the serializable list
                info.AddValue("Password", password);
                info.AddValue("ID", ID);
                info.AddValue("FirstName", firstName_);
                info.AddValue("LastName", lastName_);
                info.AddValue("City", city_);
                info.AddValue("UserName", userName_);
            }
            catch
            {   // assert on error
                ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.PERSON_SERIAL_FAIL);
            }
        }
    }
    [Serializable()]
    public struct Password
    {
        private byte[] hash_;   // stores the hash of salted text
        private byte[] salt_;   // stores the salt
        private const uint HASHLEN = 32;    // length of hash in bytes
        public Password(string t_clearTextPassword)
        {   // Password constructor; sets up the password hash for authentication
            hash_ = new byte[HASHLEN];  // make a new byte array to store the password hash
            salt_ = new byte[t_clearTextPassword.Length];   // make a new byte array to store the salt
            SetPassword(t_clearTextPassword);   // hash and store the password
        }
        public void SetPassword(string t_clearTextPassword)
        {   // gets the hash of the password and save it to the hash_ member
            byte[] saltedText = new byte[t_clearTextPassword.Length + salt_.Length];    // make a new array to hold the salted text
            int i = 0;
            for (; i < t_clearTextPassword.Length; i++)
                saltedText[i] = (byte)t_clearTextPassword[i];   // memcpy t_clearTextPassword to the saltedText buffer
            using (RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider())    // make a new csprng object for generating the salt
            {
                csprng.GetBytes(salt_); // get the salt; populate the salt_ member with very random bytes
            }   // destroy the csprng object after use
            for (int ii = 0; ii < salt_.Length; ii++)
                saltedText[i + ii] = salt_[ii]; // memcpy the salt to the salted text
            using (SHA256Managed algo = new SHA256Managed())
            {
                hash_ = algo.ComputeHash(saltedText);   // compute the hash of the saltedtext using the SHA256 algorithm; save the hash to the hash_ member;
            }   // destroy the algo object after use
        }
        public bool TestPassword(string t_clearTextPassword)
        {   // test hash_ against the salted hash of t_clearTextPassword
            byte[] saltedTest = new byte[t_clearTextPassword.Length + salt_.Length];
            byte[] testHash;
            int i = 0;
            for (; i < t_clearTextPassword.Length; i++)
                saltedTest[i] = (byte)t_clearTextPassword[i];
            for (int ii = 0; ii < salt_.Length; ii++)
                saltedTest[i + ii] = salt_[ii];
            using (SHA256Managed algo = new SHA256Managed())
            {
                testHash = algo.ComputeHash(saltedTest);
            }
            for (i = 0; i < testHash.Length; i++)
                if (testHash[i] != hash_[i])
                    return false;
            return true;
        }
        private Password(SerializationInfo info, StreamingContext context)
        {
            hash_ = null;
            salt_ = null;
            try
            {
                hash_ = info.GetValue("Hash", typeof(byte[])) as byte[];
                salt_ = info.GetValue("Salt", typeof(byte[])) as byte[];
            }
            catch
            {
                ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.PERSON_READ_FAIL);
            }
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            try
            {
                info.AddValue("Hash", hash_);
                info.AddValue("Salt", salt_);
            }
            catch
            {
                ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.PERSON_SERIAL_FAIL);
            }
        }
    }
    public abstract class ISystem
    {
        public readonly ISystem parent = null;
        public ISystem() { }
        public ISystem(ISystem t_parent)
        {
            parent = t_parent;
        }
        public virtual void Callback(uint t_callingCode, object t_data, object t_sender) {}
    }
    public abstract class IDatabase<T> : ISystem where T : class, new()
    {
        public enum Encoding //
        {   // encoding id for files; maps to a file extention
            BLOB = 0,
            TEXT = 1
        };
        private static readonly string[] FILE_EXT =
        {   // name for file extentions used by databases
            ".bin",  //EXT_BLOB
            ".txt"   //EXT_TEXT
        };
        public readonly Encoding encoding; // governs how files should be saved; 
        public readonly string rootDirPath = null;  // contains the path where files are looked up and saved to
        protected T data = null;    // contains the data of type T; data is obtained from file reads
        public IDatabase(string t_rootDirPath, Encoding t_encoding, ISystem t_parent) : base(t_parent) //
        {
            rootDirPath = t_rootDirPath;
            encoding = t_encoding;
        }
        public virtual bool Initalize()
        {
            data = new T();
            return true;
        }   // initialize the state of the database; can be overriden
        public virtual bool WriteFile(string t_fileName, T t_object)
        {
            return writeFile_[(uint)encoding](GetFullPath(t_fileName), t_object);
        }
        public virtual bool NewFile(string t_fileName)
        {
            return newFile_[(uint)encoding](GetFullPath(t_fileName));
        }
        public virtual bool LoadFile(string t_fileName)
        {
            if ((data = loadFile_[(uint)encoding](GetFullPath(t_fileName)) as T) == null)
                return false;
            return true;
        }
        private readonly Func<string, T, bool>[] writeFile_ =
        {
            (string t_path, T t_object) =>
            {
                if (!File.Exists(t_path))
                    return false;
                try
                {
                    using(Stream fileOut = File.Open(t_path,FileMode.Open))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(fileOut,t_object);
                    }
                }
                catch
                {
                    ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_WRITE_FAIL);
                    return false;
                }
                return true;
            },  //Write blob to file
            (string t_path, T t_object) =>
            {
                if(typeof(T) != typeof(string))
                    ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_WRITE_TYPE_FAIL);
                if (!File.Exists(t_path))
                    return false;
                try
                {
                    using(Stream fileOut = File.Open(t_path,FileMode.Open))
                    {
                        StreamWriter sw = new StreamWriter(fileOut);
                        sw.WriteLine(t_object as string);
                        sw.Close();
                    }
                }
                catch
                {
                    ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_WRITE_FAIL);
                    return false;
                }
                return true;
            }   //Write text to file
        };  // write data to disk; file encoding is governed by the encoding member
        private readonly Func<string, object>[] loadFile_ =
        {
            (string t_path) =>
            {
                if (!File.Exists(t_path))
                    return null;
                try
                {
                    using (Stream fileIn = File.Open(t_path,FileMode.Open))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        return bf.Deserialize(fileIn) as T;
                    }
                }
                catch
                {   //TODO: assert on error
                    //ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_WRITE_FAIL);
                    return null;
                }
            },  //load from blob
            (string t_path) =>
            {
                if(typeof(T) != typeof(string))
                    ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_WRITE_TYPE_FAIL);
                if (!File.Exists(t_path))
                    return null;
                try
                {
                    using (StreamReader sr = new StreamReader(File.Open(t_path,FileMode.Open)))
                    {
                        return sr.ReadToEnd();
                    }
                }
                catch
                {   //TODO: assert on error
                    //ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_WRITE_FAIL);
                    return null;
                }
            }   //load from text
        };  // load a file from disk;  file encoding is governed by the encoding member
        private readonly Func<string, bool>[] newFile_ =
        {
            (string t_path) =>
            {
                if (File.Exists(t_path))
                    return false;
                try
                {
                    using (Stream fs = File.Create(t_path));
                }
                catch
                {
                    ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_NEW_FILE_FAIL);
                    return false;
                }
                return true;
            },  //make new blob file
            (string t_path) =>
            {
                if (File.Exists(t_path))
                    return false;
                try
                {
                    //using (Stream fs = File.Create(string.Concat(t_path,FILE_EXT[(uint)Encoding.TEXT])));
                    using (Stream fs = File.Create(t_path));
                }
                catch
                {
                    ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_NEW_FILE_FAIL);
                    return false;
                }
                return true;
            }   //make new text file
        };  // creates a new file; the file type is governed by the encoding member
        private string GetFullPath(string t_fileName) //
        {
            return string.Concat(rootDirPath, t_fileName, FILE_EXT[(uint)encoding]);
        }
    }
    public class PersonsDatabase : IDatabase<List<Person>>
    {
        private uint nextPersonId_ = 0;
        private const int INITAL_SIZE = 10;
        public bool AddPerson(string t_firstName, string t_lastName, string t_city, string t_userName, string t_password)
        {
            //TODO: restrict to unique usernames
            Person next;
            if ((next = PersonFactory.BuildPerson(t_firstName, t_lastName, t_city, t_userName, t_password, (nextPersonId_++) - 1, this)) == null)
                return false;
            data.Add(next);
            return true;
        }
        public Person GetPerson(string t_userName)
        {
            foreach(Person p in data)
            {
                if (p.UserName == t_userName)
                    return p;
            }
            return null;
        }
        private bool RemovePerson(int t_id)
        {
            if (t_id >= data.Count)
                return false;
            for (int i = t_id + 1; i < data.Count; i++)
                data[i].ID--;
            data.RemoveAt(t_id);
            return true;
        }
        public PersonsDatabase(string t_databaseRoot, ISystem t_parent) : base(t_databaseRoot, Encoding.BLOB, t_parent)
        {
            NewFile("base");
            Initalize();
        }
    }
    public class User
    {   //TODO: doc this
        private readonly Person currentUser_;
        public User(Person t_currentUser)
        {
            currentUser_ = t_currentUser;
        }
        public void AddFriend()
        {
        }
        public void RemoveFriend()
        {
        }
        public void SearchInterests()
        {
        }
        public void NewInvitation()
        {
        }
        public void RemoveInvitation()
        {
        }
        public void Logout()
        {
        }
    }
    public class LoginSystem : ISystem
    {
        Person cachedPerson_ = null;
        Person retPerson_ = null;
        PersonsDatabase pd_;
        public LoginSystem(PersonsDatabase t_pd)
        {
            pd_ = t_pd;
        }
        public Person Login(string t_userName, string t_password)
        {
            if(cachedPerson_ != null)
            {
                if(cachedPerson_.password.TestPassword(t_password))
                {
                    retPerson_ = cachedPerson_;
                    cachedPerson_ = null;
                    return retPerson_;
                }
                return null;
            }
            if((cachedPerson_ = pd_.GetPerson(t_userName)) != null
            && (cachedPerson_.password.TestPassword(t_password)))
            {
                retPerson_ = cachedPerson_;
                cachedPerson_ = null;
                return retPerson_;
            }
            return null;
        }
    }
    public class Network : ISystem
    {
        private PersonsDatabase pd_;
        private LoginDialogue ld_;
        private UserDialogue ud_;
        private LoginSystem ls_;
        private User currentUser_ = null;
        public Network()
        {
            ud_ = new UserDialogue();
            pd_ = new PersonsDatabase(@"C:\Users\random\Documents\",this);
            pd_.AddPerson("admin","admin","none","admin","asdf2345");
            ls_ = new LoginSystem(pd_);
            new LoginDialogue(this,ls_,ref ld_);
        }
        public override void Callback(uint t_callingCode, object t_data, object t_sender)
        {
            if(t_sender is LoginDialogue)
            {
                switch (t_callingCode)
                {
                    case (uint)LoginDialogue.CallingCodes.LoginSuccess:
                        currentUser_ = new User((Person)t_data);
                        ld_.Hide();
                        ud_.AssignUser(currentUser_);
                        return;
                }
            }
            /*
            switch(t_sender)
            {
                case LoginDialogue a:
                    switch (t_callingCode)
                    {
                        case (uint)LoginDialogue.CallingCodes.LoginSuccess:
                            currentUser_ = new User((Person)t_data);
                            ld_.Hide();
                            ud_.AssignUser(currentUser_);
                            return;
                    }
                    return;
            }
            */
        }
    }
    public partial class Form1 : Form
    {
        Network net;
        public Form1()
        {
            net = new Network();
            //InitializeComponent();
        }
    }
}
