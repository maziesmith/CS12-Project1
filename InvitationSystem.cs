using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS12_Project_1
{
    // A data oriented Invitation object 
    [Serializable()]
    public class Invitation : ISerializable
    {
        // DATA MEMBERS
        public const int INIT_LIFETIME = 10; // time in minutes before the invitation expiers
        private int lifetime_;  // current life time of the invitation
        public readonly Tuple<int, int, int> timestamp  = null;
        public readonly string  // text
            author, // the invitation's author
            title,  // the invitation's title
            body;   // the invitation's body paragraph
        // CONSTRUCTORS
        // params:
        //  string t_author; the author
        //  string t_title; the title
        //  string t_body;  the body paragraph
        public Invitation(
            string t_author, 
            string t_title, 
            string t_body)
        {
            // initalize all data members
            author = t_author;
            title = t_title;
            body = t_body;
            lifetime_ = INIT_LIFETIME;
            timestamp = Tuple.Create(
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
                author = info.GetString("Author");
                title = info.GetString("Title");
                body = info.GetString("Body");
                lifetime_ = info.GetInt32("Lifetime");
                timestamp = (Tuple<int,int,int>)info.GetValue("Timestamp", typeof(Tuple<int,int,int>));
            }
            catch
            {
                ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_READ_FAIL);
            }
        }
        // METHOD MEMBERS
        public bool UpdateTime()   
        {   // count down by one minute; return true if the invatation should be deleted
            return lifetime_-- == 0;
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
            {
                ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_READ_FAIL);
            }
        }
    }
    // an object that manages invatations
    public class InvitationSystem
    {   // DATA MEMBERS
        private Timer timer_ = null;    // a timer object to update all invitations every minute
        private PersonsDatabase pd_ = null; // reference to the person database
        private ISystem parent_ = null; // reference to the parent object to send signals to
        private LinkedList<Invitation> activeInvitations_ = new LinkedList<Invitation>();   
        // a linked list to store active invitations;
        // a linked list was chosen instead of a normal list (dynamic array)
        // because it has better time complexity for insertion and deletion ( O(1) ), and
        // such operations will be preformed very offtain;
        // slow operations like traversal to access a single element ( O(n) ) are never preformed as
        // functions that access nodes operate on all nodes anyway ( O(n) )
        private LinkedListNode<Invitation> node = null; // iterator node
        private LinkedListNode<Invitation> swap = null; // swap node
        // CONSTRUCTOR
        // params:
        //  ISystem t_parent;   the parent object to send signals to 
        //  ref PersonsDatabase t_pd; reference to the person database
        public InvitationSystem(
            ISystem t_parent,
            ref PersonsDatabase t_pd)
        {   // initialize all data members
            parent_ = t_parent;
            t_pd = pd_;
            timer_ = new Timer();   // initialize the timer
            timer_.Tick += UpdateAllInvitations;
            timer_.Interval = 6000*10;  // make the timer invoke UpdateAllInvitations() every minute
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
            foreach (Invitation x in t_next)    // add all non-null invitations
                if(x != null)
                    activeInvitations_.AddLast(x);
        }
    }
}
