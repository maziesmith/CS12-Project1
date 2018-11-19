using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AIDAN_BIRD___Project_1
{
    public sealed class Signal
    {
        private uint delay_;
        public readonly uint id;
        public readonly object data;
        public readonly ISystem dest;
        public Signal(uint t_id, object t_data, ISystem t_dest)
        {
            id = t_id;
            data = t_data;
            dest = t_dest;
            delay_ = 0;
        }
        public Signal(uint t_id, object t_data, ISystem t_dest, uint t_delay)
        {
            id = t_id;
            data = t_data;
            dest = t_dest;
            delay_ = t_delay;
        }
        public bool IsReady()
        {
            if ((delay_--) < 1)
                return true;
            return false;
        }
        public uint Delay
        {
            get { return delay_; }
        }
    }
    [Serializable()]
    public sealed class Person : ISerializable
    {
        private string firstName_;
        private string lastName_;
        private string city_;
        private string userName_;
        public readonly ulong id;
        public string FirstName
        {
            get { return firstName_; }
            set { firstName_ = value; }
        }
        public string LastName
        {
            get { return lastName_; }
            set { lastName_ = value; }
        }
        public string City
        {
            get { return city_; }
            set { city_ = value; }
        }
        public string UserName
        {
            get { return userName_; }
            set { userName_ = value; }
        }
        public Person(string t_firstName, string t_lastName, string t_city, string t_userName, ulong t_id)
        {
            if (t_firstName == null
            || t_lastName == null
            || t_city == null
            || t_userName == null)
                ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.PERSON_CONSTRUCT_FAIL);
            id = t_id;
            firstName_ = t_firstName;
            lastName_ = t_lastName;
            city_ = t_city;
            userName_ = t_userName;
        }
        private Person(SerializationInfo info, StreamingContext context)
        {
            try
            {
                id = info.GetUInt64("ID");
                firstName_ = info.GetString("FirstName");
                lastName_ = info.GetString("LastName");
                city_ = info.GetString("City");
                userName_ = info.GetString("UserName");
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
                info.AddValue("ID",id);
                info.AddValue("FirstName",firstName_);
                info.AddValue("LastName",lastName_);
                info.AddValue("City",city_);
                info.AddValue("UserName",userName_);
            }
            catch
            {
                ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.PERSON_SERIAL_FAIL);
            }
        }
    }
    public sealed class User
    {
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
    {
        public enum BaseSignals
        {
            SIGPING = 0
        };
        protected const uint SPECIAL_SIGNALS_OFFSET = 1;
        protected readonly uint specialSignalsLength;
        protected Action<Signal>[] signalHandler_ = null;
        protected virtual void SigPing(Signal t_signal)
        {
            if(t_signal.data != null)
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
        {
            if(t_signal.id < signalHandler_.Length)
                signalHandler_[t_signal.id](t_signal);
        }
        public uint ResolveSpecialSignalID(uint t_id)
        {
            return SPECIAL_SIGNALS_OFFSET + t_id;
        }
        public ISystem(uint t_specialSignalsLength)
        {
            specialSignalsLength = t_specialSignalsLength;
            signalHandler_ = new Action<Signal>[SPECIAL_SIGNALS_OFFSET + t_specialSignalsLength];
            signalHandler_[(uint)BaseSignals.SIGPING] = SigPing;
        }
    }
    public static class ErrorHandler
    {
        public enum FatalErrno //
        {
            PERSON_CONSTRUCT_FAIL = 0,
            PERSON_SERIAL_FAIL = 1,
            PERSON_READ_FAIL = 2,
            DATABASE_READ_FAIL = 3,
            DATABASE_WRITE_FAIL = 4,
            DATABASE_NEW_FILE_FAIL = 5,
            DATABASE_WRITE_TYPE_FAIL = 6
        };
        private enum Prefixno
        {
            FATAL = 0,
            WARN = 1
        };
        private static readonly string[] fatalErrMsg =
        {
            "Malformed person constructor call.",
            "Could not serialize object.",
            "Could not read data from file.",
            "Database could not read from file.",
            "Database could not write to file.",
            "Database could not create new file.",
            "Database tried to write to file but was set to wrong type"
        };
        private static readonly string[] msgPrefix =
        {
            "FATAL",
            "WARN",
        };
        private static string GetErrorMsg(Prefixno prefixno, FatalErrno errno)
        {
            return string.Concat(msgPrefix[(uint)prefixno],": ",fatalErrMsg[(uint)errno]);
        }
        public static void AssertFatalError(FatalErrno errno)
        {
            throw new Exception(GetErrorMsg(Prefixno.FATAL,errno));
        }
        public static string AlertFatalError(FatalErrno errno)
        {
            return GetErrorMsg(Prefixno.WARN,errno);
        }
    }
    public abstract class IDatabase<T> : ISystem where T : class 
    {
        public enum Encoding //
        {
            BLOB = 0,
            TEXT = 1
        };
        private static readonly string[] FILE_EXT = //
        {
            ".bin",  //EXT_BLOB
            ".txt"   //EXT_TEXT
        };
        public readonly Encoding encoding; //
        public readonly string rootDirPath = null;
        private T data = null;
        public IDatabase(string t_rootDirPath, uint t_specialSignalsLength, Encoding t_encoding) : base(t_specialSignalsLength) //
        {
            rootDirPath = t_rootDirPath;
            encoding = t_encoding;
        }
        public bool WriteFile(string t_fileName, T t_object)
        {
            return writeFile_[(uint)encoding](GetFullPath(t_fileName),t_object);
        }
        public bool NewFile(string t_fileName)
        {
            return newFile_[(uint)encoding](GetFullPath(t_fileName));
        }
        public bool LoadFile(string t_fileName)
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
        }; //
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
        }; //
        private readonly Func<string,bool>[] newFile_ =
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
        };  //
        private string GetFullPath(string t_fileName) //
        {
            return string.Concat(rootDirPath, t_fileName, FILE_EXT[(uint)encoding]);
        }
    }
    public sealed class SignalDispatcher : ISystem
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
            }catch{}
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
            for(int i = 0; i < nextSignals_.Count; i++)
                if((test = nextSignals_[i]).IsReady())
                {
                    test.dest.SendSignal(test);
                    nextSignals_.RemoveAt(i);
                }
            if (nextSignals_.Count == 0)
                chrono_.Enabled = false;
        }
        public void AddSignal(Signal t_next)
        {
            if(t_next.Delay == 0)
            {   //send the signal to the appropriate system immediately
                t_next.dest.SendSignal(t_next);
                return;
            }
            nextSignals_.Add(t_next);
            chrono_.Enabled = true;
        }
    }
    public sealed class PersonsDatabase : IDatabase<Person>
    {
        public enum Signals
        {
        };
        private const int LEN_OF_SIGNALS_ = 0;
        private ulong nextPersonId_ = 0;
        public Person BuildPerson(string t_firstName, string t_lastName, string t_city, string t_userName)
        {
            return new Person(t_firstName, t_lastName, t_city, t_userName, (nextPersonId_++));
        }
        public PersonsDatabase(string t_databaseRoot) : base(t_databaseRoot, LEN_OF_SIGNALS_, IDatabase<Person>.Encoding.BLOB)
        {
            //lazy debugging && testing
            //NewFile("target");
            //WriteFile("target", BuildPerson("asdf","asdf","asdf","asdf"));
            LoadFile("target");
            //throw new Exception("lazy debugging finished"); //debug assert
        }
    }
    public sealed class Network
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
    public sealed class Password
    {
        public string hash = null;
        public string salt = null;
        public string id = null;
    }

    /*
    public sealed class PasswordDatabase : IDatabase<Password>
    {
        private const int LEN_OF_SIGNALS_ = 0;
        public PasswordDatabase(string t_databaseRoot) : base(t_databaseRoot, LEN_OF_SIGNALS_)
        {

        }
    }
    */

    public partial class Form1 : Form
    {
        PersonsDatabase pd = new PersonsDatabase(@"C:\Users\random\Documents\");
        
        public Form1()
        {
            InitializeComponent();
        }
    }
}
