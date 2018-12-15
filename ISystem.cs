using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS12_Project_1
{   // an abstract class that provides signal handling
    public abstract class ISystem
    {
        private ISystem parent_;  // the parent object to send signals to
        // CONSTRUCTOR
        public ISystem() { }    // when no parent is used
        public ISystem(in ISystem t_parent)
        {   // set the parent object
            parent_ = t_parent;
        }
        // change the parent object
        public bool ChangeParent(in ISystem t_next)
        {
            if(t_next != null)
            {
                parent_ = t_next;
                return true;    // success
            }
            return false;   // fail
        }
        // get the parent object 
        public ISystem Parent { get => parent_; }
        public enum ExitStatus  // exit statuses that are used for signaling
        {
            noError,    // no errors occured
            errors,     // errors occured
            userClosed  // user closed this object (form)
        }
        // a method that child objects can call to send signals to;
        // it's up to the system to override this and handle those signals;
        // by default, this method does nothing;
        // idea based on C# event system;
        public virtual void Callback(object t_data, object t_sender) {}
    }
}
