using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS12_Project_1
{   // INVITATION
    [Serializable()]    // invitation is serializable
    public class Invitation : ISerializable
    {   // FACTORY Pattern
        // this factory must be used to instantiate new invitations
        public static class InvitationFactory
        {   // Makes a new invitation object
            public static Invitation MakeInvitation(
                Person t_author, 
                Person[] t_recipients,
                string t_title, 
                string t_body)
            {
                Invitation output = new Invitation(            
                    t_author, 
                    t_recipients,
                    t_title, 
                    t_body);
                // add invitation to the author's sent invitations list
                t_author.AddInvitations(Person.InvitationContext.sender, output);
                // add invitation to the recipients' pending invitations list
                foreach (Person x in t_recipients)
                    x.AddInvitations(Person.InvitationContext.recipient, output);
                return output;
            }
        }
        // DATA MEMBERS
        public const int INIT_LIFETIME = 10; // time in minutes before the invitation expiers
        private int lifetime_;  // current life time of the invitation
        public readonly Tuple<int, int, int> timestamp  = null;
        private Person author = null;
        private Person[] recipients_ = null;
        public readonly string  // text
            title,  // the invitation's title
            body;   // the invitation's body paragraph
        // CONSTRUCTORS
        // params:
        //  string t_author; the author
        //  string t_title; the title
        //  string t_body;  the body paragraph
        private Invitation(
            Person t_author, 
            Person[] t_recipients,
            string t_title, 
            string t_body)
        {
            // initalize all data members
            author = t_author;
            recipients_ = t_recipients;
            title = t_title;
            body = t_body;
            lifetime_ = INIT_LIFETIME;
            timestamp = Tuple.Create(   // create a time stamp
                DateTime.Today.Month,
                DateTime.Today.Day,
                DateTime.Today.Hour);
        }
        // Serializable constructor
        private Invitation(
            SerializationInfo info, 
            StreamingContext context)
        {
            try
            {   // try to extract and set all data members
                title = info.GetString("Title");
                body = info.GetString("Body");
                lifetime_ = info.GetInt32("Lifetime");
                timestamp = (Tuple<int,int,int>)info.GetValue("Timestamp", typeof(Tuple<int,int,int>));
                author = (Person)info.GetValue("Author",typeof(Person));
            }
            catch
            {   // error
                ErrorHandler.FatalError(ErrorHandler.FatalErrno.DATABASE_READ_FAIL);
            }
        }
        public void Remove()
        {
            if (recipients_ != null)
                foreach (Person x in recipients_)
                    x.PendingInvitations.Remove(this);
            author.SentInvitations.Remove(this);
        }
        // METHOD MEMBERS
        // returns an immutable recipients_ list
        public IReadOnlyList<Person> GetRecipients
        {   // get a readonly list of the recipients
            get { return recipients_; }
        }
        // returns the author's username
        public string AuthorUsername
        {
            get { return author.UserName; }
        }
        // returns the author's static ID
        public ulong AuthorStaticID
        {
            get { return author.StaticID; }
        }
        // count down by one minute; return true if the invitation should be deleted
        public bool UpdateTime()   
        {   
            return (lifetime_--) == 0;
        }
        // serializer
        public void GetObjectData(
            SerializationInfo info, 
            StreamingContext context)
        {
            try
            {   // try to encode how a invitation will be stored in a file
                info.AddValue("Author",author);
                info.AddValue("Title",title);
                info.AddValue("Body",body);
                info.AddValue("Lifetime",lifetime_);
                info.AddValue("Timestamp",timestamp);
            }
            catch
            {   // crash and burn on error
                ErrorHandler.FatalError(ErrorHandler.FatalErrno.DATABASE_READ_FAIL);
            }
        }
    }
    // INVITATION SYSTEM
    // an object that manages invatations
    public class InvitationSystem
    {   // DATA MEMBERS
        private Timer timer_ = null;    // a timer object to update all invitations every minute
        private PersonsDatabase pd_; // reference to the person database
        private UserDialogue ud_;
        private LinkedList<Invitation> activeInvitations_ = new LinkedList<Invitation>();   
        // a linked list to store active invitations;
        // a linked list was chosen instead of a normal list (dynamic array)
        // because it has better time complexity for insertion and deletion ( O(1) ), and
        // such operations will be preformed very offtain;
        // slow operations like traversal to access a single element ( O(n) ) are never preformed as
        // functions that access nodes operate on all nodes anyway ( O(n) )
        private LinkedListNode<Invitation> node = null; // iterator node
        private LinkedListNode<Invitation> swap = null; // swap node
        private const int TIMER_INTERVAL_TIME = 6000*10;
        // CONSTRUCTOR
        // params:
        //  ref PersonsDatabase t_pd; reference to the person database
        public InvitationSystem(
            in UserDialogue t_ud,
            in PersonsDatabase t_pd)
        {   // initialize all data members
            ud_ = t_ud;
            pd_ = t_pd;
            timer_ = new Timer();   // initialize the timer
            timer_.Tick += UpdateAllInvitations;
            timer_.Interval = TIMER_INTERVAL_TIME;  // make the timer invoke UpdateAllInvitations() every minute
            timer_.Start(); // start the time
        }
        // DESTRUCTOR
        ~InvitationSystem()
        {   // ensure that the timer is deleted
            timer_.Dispose();
        }
        // METHOD MEMBERS
        // update all invitations
        private void UpdateAllInvitations(object t_sender, EventArgs t_eventArgs)
        {
            if (activeInvitations_.Count == 0)  // if there are no invitations... return
                return;
            node = activeInvitations_.First;   // make node point to the first node in the list
            while(node != null) // iterate over all nodes
            {
                if(node.Value.UpdateTime()) // update all invitations; delete all that have a lifetime of 0
                {
                    swap = node;    // save node state
                    node.Value.Remove();
                    if(node.Value.AuthorStaticID == ud_.CurrentUser.StaticID)
                    {
                        ud_.UpdatePendingInvitations(); // tell the user dialog to update the invitations list
                        ud_.UpdateSentInvitations();    
                    }
                    activeInvitations_.Remove(node);    // delete node
                    node = swap.Next;   // goto next node 
                    continue;
                }
                node = node.Next;   // goto next node
            }
        }
        // add invitation(s) to the list
        public void AddInvitation(params Invitation[] t_next)
        {
            foreach (Invitation x in t_next)    
                if(x != null)   // add all non-null invitations
                    activeInvitations_.AddLast(x);
        }
    }
}
