using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CS12_Project_1
{
    // FACTORY PATTERN 
    public static class PersonFactory
    {   // make a person object
        public static Person BuildPerson(
            string t_firstName, 
            string t_lastName,
            string t_city,
            string t_userName,
            string t_password,
            uint t_id,
            ISystem t_caller)
        {   // test if any input is null or empty, return null if true
            if (string.IsNullOrWhiteSpace(t_firstName)
            || string.IsNullOrWhiteSpace(t_lastName)
            || string.IsNullOrWhiteSpace(t_city)
            || string.IsNullOrWhiteSpace(t_userName)
            || string.IsNullOrWhiteSpace(t_password))
                return null;
            // otherwise return a new person
            return new Person(t_firstName, t_lastName, t_city, t_userName, t_id, t_password);
        }
    }
    // PERSON
    [Serializable()]    //Person is serializable
    public class Person : ISerializable
    {   // Person class contains data about a person; Person class implements ISerializable
        // DATA MEMBERS 
        private const int FRIENDS_LIST_INITAL_SIZE = 5;
        private const int INTERESTS_LIST_INITAL_SIZE = 5;
        private string firstName_;  // Person's fist name
        private string lastName_;   // Person's last name
        private string city_;       // Person's city
        private string userName_;   // Person's user name
        private ulong id_;          // Person's id; maps to a person object in a collection
        public readonly ulong staticID;
        public readonly List<Person> friends_ = new List<Person>(FRIENDS_LIST_INITAL_SIZE); // pre-alloc friends list
        public readonly List<string> interests_ = new List<string>();
        public readonly List<Invitation> pendingInvitations_;
        public readonly List<Invitation> sentInvitations_;   
        public readonly Password password;  // Person's password object
        public ulong ID
        {   // get/set the user ID
            get { return id_; }
            set { id_ = value; }
        }
        // METHOD MEMBERS
        // get a list of friends starting at index t_start and whos size can be up to t_amt
        public List<Person> GetFriends(int t_start, int t_amt)
        {   // if friends_ is empty or unavailable, return null
            if(friends_ == null
            || friends_.Count < 1
            || t_start >= friends_.Count)
                return null;
            List<Person> output = new List<Person>(friends_.Count);
            for(int i = 0; i < friends_.Count; i++)
            {
                if (i == t_amt)
                    return output;
                output.Add(friends_[i]);
            }
            return output;
        }
        // get a friend using an index
        public Person GetFriend(int t_index)
        {
            if (t_index > friends_.Count
            || t_index < 0)
                return null;    // t_index is invalid, return null
            return friends_[t_index];   // return the friend at the index
        }
        public string FirstName
        {   // get/set the user's first name
            get { return firstName_; }
            set { firstName_ = value; }
        }
        public string LastName
        {   // get/set the user's last name
            get { return lastName_; }
            set { lastName_ = value; }
        }
        public string City
        {   // get/set the user's city
            get { return city_; }
            set { city_ = value; }
        }
        public string UserName
        {   // get/set the user's username
            get { return userName_; }
            set { userName_ = value; }
        }
        // CONSTRUCTOR
        // params:
        //  string t_firstName; first name
        //  string t_lastName; last name
        //  string t_city;   city
        //  string t_userName; username
        //  ulong t_id;  id
        //  string t_password; password
        public Person(string t_firstName, string t_lastName, string t_city, string t_userName, ulong t_id, string t_password)
        {
            // TODO: remove this
            if (t_firstName == null
            || t_lastName == null
            || t_city == null
            || t_userName == null
            || t_password == null)  // test if any string parameter is null; if true, error
                ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.PERSON_CONSTRUCT_FAIL);
            // set all data members
            ID = t_id;
            firstName_ = t_firstName;
            lastName_ = t_lastName;
            city_ = t_city;
            userName_ = t_userName;
            staticID = (ulong)(DateTime.UtcNow.Subtract(new DateTime(1970,1,1))).TotalSeconds + t_id;
            password = new Password(t_password);    // assign a new password object to the Person's class
            pendingInvitations_ = new List<Invitation>();
            sentInvitations_ = new List<Invitation>();
        }
        private Person(SerializationInfo info, StreamingContext context)
        {   // Person constructor; used to build a new person object from a blob file
            try
            {   // try to set all data members using data extracted from the blob
                password = (Password)info.GetValue("Password", typeof(Password));
                friends_ = (List<Person>)info.GetValue("Friends",typeof(List<Person>));
                ID = info.GetUInt64("ID");
                firstName_ = info.GetString("FirstName");
                lastName_ = info.GetString("LastName");
                city_ = info.GetString("City");
                userName_ = info.GetString("UserName");
                staticID = info.GetUInt64("STATIC_ID");
                interests_ = (List<string>)info.GetValue("Interests", typeof(List<string>));
                pendingInvitations_ = (List<Invitation>)info.GetValue("PendingInvitations", typeof(List<Invitation>));
                sentInvitations_ = (List<Invitation>)info.GetValue("SentInvitations", typeof(List<Invitation>));
            }
            catch
            {   // error
                ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.PERSON_READ_FAIL);
            }
        }
        // add interest(s) to interest list
        public void AddInterest(params string[] t_next)
        {   // add interests that are not in the list already
            interests_.AddRange(t_next.Where(s => !string.IsNullOrWhiteSpace(s) && !interests_.Contains(s)));
        }
        // add friend
        public bool AddFriend(Person t_next)
        {
            if (t_next == null) // if friend is null then return false
                return false;   // false = error
            foreach(Person p in friends_)
            {   // if t_next is already in the list, do not add it and return false
                if(p.staticID == t_next.staticID)
                    return false;
            }
            friends_.Add(t_next);   // add t_next to the list
            return true;    // true = success
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {   // called when saving a person object to disk
            try
            {   // try to add all data members to the serializable list
                info.AddValue("Password", password);
                info.AddValue("ID", ID);
                info.AddValue("FirstName", firstName_);
                info.AddValue("LastName", lastName_);
                info.AddValue("City", city_);
                info.AddValue("UserName", userName_);
                info.AddValue("Friends", friends_);
                info.AddValue("STATIC_ID", staticID);
                info.AddValue("Interests",interests_);
                info.AddValue("PendingInvitations",pendingInvitations_);
                info.AddValue("SentInvitations",sentInvitations_);
            }
            catch
            {   // error
                ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.PERSON_SERIAL_FAIL);
            }
        }
    }
    // PASSWORD
    [Serializable()]
    public struct Password
    {   // DATA MEMBERS
        private byte[] hash_;   // stores the hash of salted text
        private byte[] salt_;   // stores the salt
        private const uint HASHLEN = 32;    // length of hash in bytes
        public const int MIN_PASSWORD_LENGTH = 9;
        public const int MIN_PASSWORD_REP_CHARS = 3;
        public enum ValidatePasswordStatus
        {   // return values indicating any problems with testing passwords
            badRepChars,
            badPasswdLen,
            noError
        }
        // CONSTRUCTOR
        public Password(string t_clearTextPassword)
        {   // set up the password hash for authentication 
            hash_ = new byte[HASHLEN];  // make a new byte array to store the password hash
            salt_ = new byte[t_clearTextPassword.Length];   // make a new byte array to store the salt
            SetPassword(t_clearTextPassword);   // hash and store the password
        }
        // gets the hash of the password and save it to the hash_ member
        public void SetPassword(string t_clearTextPassword)
        {   
            byte[] saltedText = new byte[t_clearTextPassword.Length + salt_.Length];    // make a new array to hold the salted text
            int i = 0;  // loop counter
            for (; i < t_clearTextPassword.Length; i++)
                saltedText[i] = (byte)t_clearTextPassword[i];   // memcpy t_clearTextPassword to the saltedText buffer
            using (RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider())    // make a new csprng object for generating the salt
            {
                csprng.GetBytes(salt_); // get the salt; populate the salt_ member with very random bytes
            }   // destroy the csprng object after use
            for (int ii = 0; ii < salt_.Length; ii++)
                saltedText[i + ii] = salt_[ii]; // memcpy the salt to the salted text
            using (SHA256Managed algo = new SHA256Managed())
            {
                hash_ = algo.ComputeHash(saltedText);   // compute the hash of the saltedtext using the SHA256 algorithm; save the hash to the hash_ member;
            }   // destroy the algo object after use
        }
        // validate a password
        public static ValidatePasswordStatus ValidatePasswordFormat(string t_clearTextPassword)
        {
            if (t_clearTextPassword.Length < MIN_PASSWORD_LENGTH)
                return ValidatePasswordStatus.badPasswdLen; // invalidate the test password if it's length is less then the min length
            int limit;  // loop limit
            for (int i = 0; i < t_clearTextPassword.Length - MIN_PASSWORD_REP_CHARS; i++)
            {
                char test = t_clearTextPassword[i];
                limit = i + MIN_PASSWORD_REP_CHARS;
                for (i++; i < limit; i++)
                    if (t_clearTextPassword[i] != test)
                        goto tryNextChar;
                return ValidatePasswordStatus.badRepChars;  // bad password
            tryNextChar:;
                continue;
            }
            return ValidatePasswordStatus.noError;
        }
        // test hash_ against the salted hash of t_clearTextPassword
        public bool TestPassword(string t_clearTextPassword)
        {   
            byte[] saltedTest = new byte[t_clearTextPassword.Length + salt_.Length];
            byte[] testHash;
            int i = 0;
            for (; i < t_clearTextPassword.Length; i++)
                saltedTest[i] = (byte)t_clearTextPassword[i];
            for (int ii = 0; ii < salt_.Length; ii++)
                saltedTest[i + ii] = salt_[ii];
            using (SHA256Managed algo = new SHA256Managed())
            {
                testHash = algo.ComputeHash(saltedTest);
            }
            for (i = 0; i < testHash.Length; i++)
                if (testHash[i] != hash_[i])
                    return false;
            return true;
        }
        //
        private Password(SerializationInfo info, StreamingContext context)
        {
            hash_ = null;
            salt_ = null;
            try
            {
                hash_ = info.GetValue("Hash", typeof(byte[])) as byte[];
                salt_ = info.GetValue("Salt", typeof(byte[])) as byte[];
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
                info.AddValue("Hash", hash_);
                info.AddValue("Salt", salt_);
            }
            catch
            {
                ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.PERSON_SERIAL_FAIL);
            }
        }
    }
}
