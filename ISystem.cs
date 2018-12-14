using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS12_Project_1
{   // an abstract class that provides signal handling
    public abstract class ISystem
    {
        public readonly ISystem parent = null;  // the parent object to send signals to
        // CONSTRUCTOR
        public ISystem() { }    // when no parent is used
        public ISystem(ISystem t_parent)
        {   // set the parent object
            parent = t_parent;
        }
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
