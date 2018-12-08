using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS12_Project_1
{
    public abstract class ISystem
    {
        public readonly ISystem parent = null;
        public ISystem() { }
        public ISystem(ISystem t_parent)
        {
            parent = t_parent;
        }
        public enum ExitStatus
        {
            noError,
            errors,
            userClosed
        }
        public virtual void Callback(object t_data, object t_sender) {}
    }
}
