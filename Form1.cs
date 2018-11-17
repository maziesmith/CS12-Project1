using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AIDAN_BIRD___Project_1
{
    public class Signal
    {
        public readonly int id;
        public readonly int delay;
        public readonly object data;
        public readonly ISystem dest;
        public Signal(int t_id, object t_data, ISystem t_dest)
        {
            id = t_id;
            data = t_data;
            dest = t_dest;
            delay = 0;
        }
        public Signal(int t_id, object t_data, ISystem t_dest, int t_delay)
        {
            id = t_id;
            data = t_data;
            dest = t_dest;
            delay = t_delay;
        }
    }
    public sealed class Person
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
        protected Action<Signal>[] signalHandler_ = null;
        public void SendSignal(Signal t_signal)
        {
            if(t_signal.id < signalHandler_.Length)
                signalHandler_[t_signal.id](t_signal);
        }
        public ISystem()
        {
        }
    }
    public sealed class SignalDispatcher
    {
        private Queue<Signal> signalQueue_ = new Queue<Signal>();
        private Timer chrono_ = new Timer();
        private Signal nextSignal_ = null;
        public SignalDispatcher()
        {
            chrono_.Enabled = false;
            chrono_.Tick += (sneder, e) => DispatchSignal_();
        }
        private void StartChrono_()
        {
            nextSignal_ = signalQueue_.Peek();
            if(nextSignal_.delay == 0)
            {
                DispatchSignal_();
                return;
            }
            chrono_.Interval = nextSignal_.delay;
            chrono_.Enabled = true;
        }
        private void DispatchSignal_()
        {
            nextSignal_.dest.SendSignal(signalQueue_.Dequeue());
            if (signalQueue_.Count == 0)
            {
                chrono_.Enabled = false;
                return;
            }
            StartChrono_();
        }
        public void AddSignal(Signal t_next)
        {
            signalQueue_.Enqueue(t_next);
            if (!chrono_.Enabled)
                StartChrono_();
        }
    }
    public sealed class PersonsDatabase : ISystem
    {
        public enum Signals
        {
            SIGPING = 1
        };
        private const int LEN_OF_SIGNALS_ = 1;
        private void SIGPing_(Signal t_signal)
        {   //Recived SIGPING
            //Console.WriteLine("Recived SIGPING");
            //MessageBox.Show("Recived SIGPING");
        }
        public PersonsDatabase()
            : base()
        {
            signalHandler_ = new Action<Signal>[LEN_OF_SIGNALS_];
            signalHandler_[(int)Signals.SIGPING] = SIGPing_;
        }
    }
    public sealed class Network
    {
        PersonsDatabase pd = new PersonsDatabase();
        SignalDispatcher sd = new SignalDispatcher(); // DEBUG
        public Network()
        {
            //SIGPING
            //sd.AddSignal(new Signal((int)PersonsDatabase.Signals.PING,null,pd));
            //sd.AddSignal(new Signal((int)PersonsDatabase.Signals.PING,null,pd,5000));
        }
    }
    public partial class Form1 : Form
    {
        //Network net = new Network();
        public Form1()
        {
            InitializeComponent();
        }
    }
}
