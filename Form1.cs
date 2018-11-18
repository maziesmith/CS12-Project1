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
    public class Signal
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
        public Person(string t_firstName, string t_lastName, string t_city, string t_userName)
        {
             firstName_ = t_firstName;
             lastName_ = t_lastName;
             city_ = t_city;
             userName_ = t_userName;
        }
        private Person(SerializationInfo info, StreamingContext context)
        {
            firstName_ = info.GetString("FirstName");
            lastName_ = info.GetString("LastName");
            city_ = info.GetString("City");
            userName_ = info.GetString("UserName");
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("FirstName",firstName_);
            info.AddValue("LastName",lastName_);
            info.AddValue("City",city_);
            info.AddValue("UserName",userName_);
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
    public abstract class IDatabase<T> : ISystem where T : class 
    {
        public readonly string rootDirPath = null;
        private T data = null;
        private bool isSecure;
        private const string ext = ".bin";
        public IDatabase(string t_rootDirPath, uint t_specialSignalsLength, bool t_isSecure = false) : base(t_specialSignalsLength)
        {
            rootDirPath = t_rootDirPath;
            isSecure = t_isSecure;
        }
        protected bool NewFile(string t_fileName)
        {
            string fullPath = string.Concat(rootDirPath, t_fileName,ext);
            if (File.Exists(fullPath))
                return false;
            try
            {
                using (Stream fs = File.Create(fullPath));
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }
        protected bool WriteFile(string t_fileName, T t_object)
        {
            string fullPath = string.Concat(rootDirPath, t_fileName,ext);
            if (!File.Exists(fullPath))
                return false;
            try
            {
                using(Stream fileOut = File.Open(fullPath,FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fileOut,t_object);
                }
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }
        protected bool LoadFile(string t_fileName)
        {
            string fullPath = string.Concat(rootDirPath, t_fileName,ext);
            if (!File.Exists(fullPath))
                return false;
            try
            {
                using (Stream fileIn = File.Open(fullPath,FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    data = bf.Deserialize(fileIn) as T;
                }
            }
            catch(Exception e)
            {
                data = null; 
                return false;
            }
            return true;
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
        public PersonsDatabase(string t_databaseRoot) : base(t_databaseRoot, LEN_OF_SIGNALS_)
        {
            //lazy debugging && testing
            NewFile("target");
            WriteFile("target", new Person("asdf","asdf","asdf","asdf"));
            LoadFile("target");
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

    public partial class Form1 : Form
    {
        PersonsDatabase pd = new PersonsDatabase(@"C:\Users\random\Documents\");
        
        public Form1()
        {
            InitializeComponent();
        }
    }
}
