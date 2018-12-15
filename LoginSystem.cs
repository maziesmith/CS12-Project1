using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS12_Project_1
{   // an object that handle's login requests
    public class LoginSystem : ISystem
    {
        private PersonsDatabase pd_;    // a reference to the person database
        // CONSTRUCTOR
        // params:
        //  PersonsDatabase t_pd; a reference to the person database
        public LoginSystem(PersonsDatabase t_pd)
        {   // initialize all data members
            pd_ = t_pd;
        }
        // try to log a person in using a username and password
        public Person Login(string t_userName, string t_password)
        {
            Person retPerson_ = pd_.BloomFilterSearch(t_userName);  // the person object to be returned
            if (retPerson_ == null)
                return null;    // if person is not found, return null;
            return retPerson_.Password.TestPassword(t_password)
                ? retPerson_    // if retPerson_'s password is the same as t_password return retPerson_
                : null; // otherwise return null 
            /* old
            return (retPerson_ = pd_.BloomFilterSearch(t_userName)) != null
            && retPerson_.password.TestPassword(t_password)
                ? retPerson_
                : null; // return null if 
                */
        }
    }
}
