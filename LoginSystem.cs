using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS12_Project_1
{
    public class LoginSystem : ISystem
    {
        private PersonsDatabase pd_;
        public LoginSystem(PersonsDatabase t_pd)
        {
            pd_ = t_pd;
        }
        public Person Login(string t_userName, string t_password)
        {
            Person retPerson_;
            if ((retPerson_ = pd_.BloomFilterSearch(t_userName)) != null
            && retPerson_.password.TestPassword(t_password))
                return retPerson_;
            return null;
        }
    }
}
