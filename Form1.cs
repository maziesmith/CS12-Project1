// CS12 OOP Project 1 <https://github.com/aidan-bird/CS12-Project1>
// August 29, 2018.
//
// Copyright 2018, Aidan Bird.
// This work is licensed under the terms of the MIT license.
// For a copy, see <https://opensource.org/licenses/MIT>.*
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; // for windows forms
using System.IO;            // for file access
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;   // for saving objects as blobs
using System.Security.Cryptography; // for hashing passwords (SHA256)

namespace cs12_project1
{
    public sealed class Signal
    {   // Signal class is an object that is used to communicate data between systems
        private uint delay_;    // time in seconds before the signal can be activated
        public readonly uint id;    // id of the signal; maps to a function in the signal handler for each system object
        public readonly object data;    // contains metadata about the signal; used as a parameter in the signal handler
        public readonly ISystem dest;   // contains a reference to the destination
        public Signal(uint t_id, object t_data, ISystem t_dest)
        {   // Constructor for a signal object with no delay; immediately invoked by the signal dispatcher
            // set all data members; delay_ defaults to 0
            id = t_id;
            data = t_data;
            dest = t_dest;
            delay_ = 0;
        }
        public Signal(uint t_id, object t_data, ISystem t_dest, uint t_delay)
        {   // Constructor for a signal object; invoked by the signal dispatcher after delay_ seconds
            // set all data members
            id = t_id;
            data = t_data;
            dest = t_dest;
            delay_ = t_delay;
        }
        public bool IsReady()
        {   // true when delay_ is zero; otherwise, decrement delay_ and return false
            if ((delay_--) < 1)
                return true;
            return false;
        }
        public uint Delay
        {   // Get the delay
            get { return delay_; }
        }
    }
    [Serializable()]    //Person is serializable
    public class Person : ISerializable
    {   // Person class contains data about a person; Person class implements ISerializable
        private string firstName_;  // Person's fist name
        private string lastName_;   // Person's last name
        private string city_;       // Person's city
        private string userName_;   // Person's user name
        private ulong id_;          // Person's id; maps to a person object in a collection
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
    public class User
    {   //TODO: doc this
        private Person currentUser_ = null;
        public User()
        {
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
    public abstract class ISystem
    {   // system abstract class; connects objects to the signal dispatcher
        public enum BaseSignals
        {   // contains a list of signals that all systems must have
            SIGPING = 0
        };
        protected const uint SPECIAL_SIGNALS_OFFSET = 1;    // adding derrived system specific signals start at this offset
        protected readonly uint specialSignalsLength;   // amount of system specific signals
        protected Action<Signal>[] signalHandler_ = null;   // signals are mapped to functions in this array
        protected virtual void SigPing(Signal t_signal)
        {   // SIGPING; called when a system pings another system; can be overriden
            if (t_signal.data != null)
            {
                try
                {
                    //Console.WriteLine((string)t_signal.data);
                    MessageBox.Show((string)t_signal.data);
                }
                catch { }
            }
            Console.WriteLine("Recived SIGPING");
        }
        public void SendSignal(Signal t_signal)
        {   // called when a signal is sent to the system
            if (t_signal.id < signalHandler_.Length) // test if the signal is in range of the signal handler
                signalHandler_[t_signal.id](t_signal);  // invoke the function that handles this signal
        }
        public uint ResolveSpecialSignalID(uint t_id)
        {   // resolve the signal to the correct mapping if the signal is system specific
            return SPECIAL_SIGNALS_OFFSET + t_id;   // return the offset + id
        }
        public ISystem(uint t_specialSignalsLength)
        {   // ISystem constructor; sets up the signal handler
            specialSignalsLength = t_specialSignalsLength;  // save the amount of system specific signals
            signalHandler_ = new Action<Signal>[SPECIAL_SIGNALS_OFFSET + t_specialSignalsLength];   // make an array of actions to contain the signal handler
            signalHandler_[(uint)BaseSignals.SIGPING] = SigPing;    // setup the default state of the signal handler (populate with default functions)
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
            DATABASE_WRITE_TYPE_FAIL = 6
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
            "Database tried to write to file but was set to wrong type"
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
        public IDatabase(string t_rootDirPath, uint t_specialSignalsLength, Encoding t_encoding) : base(t_specialSignalsLength) //
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
    public class SignalDispatcher : ISystem
    {
        public enum Signals
        {
            SIGNEW = 0
        };
        private void SigNew(Signal t_signal)
        {
            Console.WriteLine("Recived SIGNEW");
            try
            {
                AddSignal((Signal)t_signal.data);
            }
            catch { }
        }
        private const int LEN_OF_SIGNALS_ = 1;
        private const int CHRONO_UPDATE_DELAY_ = 1000; // = 1000ms
        private List<Signal> nextSignals_ = new List<Signal>();
        private Timer chrono_ = new Timer();
        public SignalDispatcher() : base(LEN_OF_SIGNALS_)
        {
            signalHandler_[ResolveSpecialSignalID((uint)Signals.SIGNEW)] = SigNew;
            chrono_.Enabled = false;
            //chrono_.Interval = CHRONO_UPDATE_DELAY;
            chrono_.Interval = CHRONO_UPDATE_DELAY_;
            chrono_.Tick += (sneder, e) => DispatchSignal_();
        }
        private void DispatchSignal_()
        {
            Signal test = null;
            for (int i = 0; i < nextSignals_.Count; i++)
                if ((test = nextSignals_[i]).IsReady())
                {
                    test.dest.SendSignal(test);
                    nextSignals_.RemoveAt(i);
                }
            if (nextSignals_.Count == 0)
                chrono_.Enabled = false;
        }
        public void AddSignal(Signal t_next)
        {
            if (t_next.Delay == 0)
            {   //send the signal to the appropriate system immediately
                t_next.dest.SendSignal(t_next);
                return;
            }
            nextSignals_.Add(t_next);
            chrono_.Enabled = true;
        }
    }
    public class PersonsDatabase : IDatabase<List<Person>>
    {
        public enum Signals
        {
        };
        private const int LEN_OF_SIGNALS_ = 0;
        private ulong nextPersonId_ = 0;
        private const int INITAL_SIZE = 10;
        //private Dictionary<string, ulong> nameDict = new Dictionary<string, ulong>(INITAL_SIZE);
        public void AddPerson(string t_firstName, string t_lastName, string t_city, string t_userName, string t_password)
        {
            //TODO: restrict to unique usernames
            data.Add(BuildPerson(t_firstName, t_lastName, t_city, t_userName, t_password));
        }
        private Person BuildPerson(string t_firstName, string t_lastName, string t_city, string t_userName, string t_password)
        {
            return new Person(t_firstName, t_lastName, t_city, t_userName, (nextPersonId_++) - 1, t_password);
        }
        public bool RemovePerson(int t_id)
        {
            if (t_id >= data.Count)
                return false;
            for (int i = t_id + 1; i < data.Count; i++)
                data[i].ID--;
            data.RemoveAt(t_id);
            return true;
        }
        //public ulong GetIDFromUserName(string t_userName)
        //{
        //    return nameDict[t_userName];
        //}
        public PersonsDatabase(string t_databaseRoot) : base(t_databaseRoot, LEN_OF_SIGNALS_, Encoding.BLOB)
        {
            //lazy debugging && testing
            Initalize();
            NewFile("target");
            AddPerson("asdf", "asdf", "asdf", "asdf", "mysuperstrongpassword");
            WriteFile("target", data);
            Initalize();
            LoadFile("target");
            throw new Exception("lazy debugging finished"); //debug assert
        }
    }
    public class Network
    {
        //PersonsDatabase pd = new PersonsDatabase();
        //SignalDispatcher sd = new SignalDispatcher();
        public Network()
        {
            //SIGPING test
            //sd.AddSignal(new Signal((int)PersonsDatabase.Signals.SIGPING,null,pd));
            //sd.AddSignal(new Signal((int)PersonsDatabase.Signals.SIGPING,null,pd,5));
            //SIGNEW test
            //sd.AddSignal(new Signal(sd.ResolveSpecialSignalID((uint)SignalDispatcher.Signals.SIGNEW),new Signal((uint)ISystem.BaseSignals.SIGPING,"ffff",sd,5),pd,5));
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
    public partial class Form1 : Form
    {
        PersonsDatabase pd = new PersonsDatabase(@"D:\cs12-project1\");
        public Form1()
        {
            InitializeComponent();
        }
    }
}