using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace CS12_Project_1
{
    // PERSON
    [Serializable()]    //Person is serializable
    // Person class contains data about a person;
    public class Person : ISerializable
    {
        // FACTORY PATTERN
        // this factory must be used to instantiate new person objects
        public static class PersonFactory
        {   // DATA MEMBERS
            public enum Errors
            {   // errors list
                nullFirstName,
                nullLastName,
                nullCity,
                nullUserName,
                nullPassword,
            };
            // make a person object 
            // params:
            //  string t_firstName; person's first name
            //  string t_lastName;  person's last name
            //  string t_city;      person's city
            //  string t_userName;  person's username
            //  string t_password;  person's password
            //  uint t_id;          person's non-static id
            public static object BuildPerson(
                string t_firstName,
                string t_lastName,
                string t_city,
                string t_userName,
                string t_password,
                uint t_id)
            {   // do a bunch of null checks
                // test if any input is null or empty, return null if true
                return string.IsNullOrWhiteSpace(t_firstName) ? 
                    Errors.nullFirstName as object :    // bad first name 
                    string.IsNullOrWhiteSpace(t_lastName) ?
                    Errors.nullLastName as object :     // bad last name
                    string.IsNullOrWhiteSpace(t_city) ?
                    Errors.nullCity as object :         // bad city
                    string.IsNullOrWhiteSpace(t_userName) ?
                    Errors.nullUserName as object :     // bad username
                    string.IsNullOrWhiteSpace(t_password) ?
                    Errors.nullPassword as object :
                    ((Func<object>)(() =>
                    {   // test t_password
                        Password.ValidatePasswordStatus passwordStatus = Password.ValidatePasswordFormat(t_password);
                        return passwordStatus != Password.ValidatePasswordStatus.noError ?
                        passwordStatus as object:   // Validate t_password
                        new Person(t_firstName, t_lastName, t_city, t_userName, t_id, t_password) as object; // success
                    }))() as object;
            }
        }
        // DATA MEMBERS 
        private const int FRIENDS_LIST_INITAL_SIZE = 5; // init size of the friends list
        private const int INTERESTS_LIST_INITAL_SIZE = 5;   // init size of the interests list
        private string firstName_;  // Person's fist name
        private string lastName_;   // Person's last name
        private string city_;       // Person's city
        private string userName_;   // Person's user name
        private ulong id_;          // Person's id; maps to a person object in a collection
        private readonly ulong staticID_; // static id, should never change
        private readonly List<Person> friends_ = new List<Person>(FRIENDS_LIST_INITAL_SIZE); // pre-alloc friends list
        private List<string> interests_ = new List<string>(INTERESTS_LIST_INITAL_SIZE); // pre-alloc interests list; TODO: delete this
        private readonly List<Invitation> pendingInvitations_;   // a list of pending invitations
        private readonly List<Invitation> sentInvitations_;   // a list of sent invitations 
        private readonly Password password_;  // Person's password object
        public enum InvitationContext    // governs how invitations will be deleted
        {
            recipient,
            sender
        };
        // METHOD MEMBERS
        // GETTERS
        // get the person's static ID
        public ulong StaticID { get => staticID_; }
        // get the person's friends list
        public List<Person> Friends { get => friends_; }
        // get the perons's interests
        public List<string> Interests { get => interests_;}
        // get the perons's PendingInvitations list
        public List<Invitation> PendingInvitations { get => pendingInvitations_;}
        // get the perons's SentInvitations list
        public List<Invitation> SentInvitations { get => sentInvitations_;}
        // get the perons's password object
        public Password Password { get => password_;}  
        // get a list of friends starting at index t_start and whos size can be up to t_amt
        public List<Person> GetFriends(int t_start, int t_amt)
        {   // if friends_ is empty or unavailable, return null
            if(friends_ == null
            || friends_.Count < 1
            || t_start >= friends_.Count)
                return null;
            List<Person> output = new List<Person>(friends_.Count); // return value
            for(int i = 0; i < friends_.Count; i++)
            {
                if (i == t_amt)
                    return output;
                output.Add(friends_[i]);
            }   // fill the output list
            return output;  // return the output list
        }
        public ulong ID
        {   // get/set the user ID
            get => id_; 
            set => id_ = value;
        }
        // get a friend using an index  
        public Person GetFriend(int t_index)    // TODO: delete this
        {
            if(t_index > friends_.Count
            || t_index < 0)
                return null;    // t_index is invalid, return null
            return friends_[t_index];   // return the friend at the index
        }
        // get the user's first name
        public string FirstName { get => firstName_; }
        // get the user's last name
        public string LastName { get => lastName_; }
        // get the user's city
        public string City { get => city_; }
        // get the user's username
        public string UserName { get => userName_; }
        // SETTERS
        // These are not mutators because I want a return value where
        // True = success & False = fail
        // set first name
        public bool SetInterests(List<string> t_value)
        {   // do a null check
            if (t_value == null)
                return false;   // fail
            interests_ = t_value;
            return true;
        }
        public bool SetFirstName(string t_value)
        {   // do a null check
            if (string.IsNullOrWhiteSpace(t_value))
                return false;   // fail
            firstName_ = t_value;   // set the first name
            return true;    // success
        }
        // set the last name
        public bool SetLastName(string t_value)
        {   // do a null check
            if (string.IsNullOrWhiteSpace(t_value))
                return false;   // fail
            lastName_ = t_value;    // set the last name
            return true;    // success
        }
        // set the city
        public bool SetCity(string t_value)
        {   // do a null check
            if (string.IsNullOrWhiteSpace(t_value))
                return false;   // fail
            city_ = t_value;    // set the city
            return true;    // success
        }
        // set the username
        // TODO: delete this,
        // the username should only be set once
        public bool SetUserName(string t_value)
        {   // check if the username has been set alredy
            if(!string.IsNullOrWhiteSpace(userName_))
                return false;   // fail 
            // do a null check
            if(string.IsNullOrWhiteSpace(t_value))
                return false;   // fail 
            userName_ = t_value;    // set the username 
            return true;    // success
            // lambda test :)
            //return string.IsNullOrWhiteSpace(t_value)
            //    ? 
            //    ((Func<bool>)(() =>
            //    {
            //        userName_ = t_value;
            //        return true;
            //    }))() : ((Func<bool>)(() => 
            //    {
            //        return false;
            //    }))();
        }
        // tests if a person object is in the friends list
        public bool IsFriend(Person t_next)
        {   // do a null check
            if(t_next == null)
                return false;   // fail
            if(t_next.staticID_ == staticID_)
                return false;   // t_next is the same person as this object; fail
            return friends_.Any( x => x.staticID_ == t_next.StaticID);  // check t_next's static id against all other ids in the friends list
        }
        // CONSTRUCTOR
        // params:
        //  string t_firstName; first name
        //  string t_lastName; last name
        //  string t_city;   city
        //  string t_userName; username
        //  ulong t_id;  id
        //  string t_password; password
        private Person(string t_firstName, string t_lastName, string t_city, string t_userName, ulong t_id, string t_password)
        {   // set all data members
            ID = t_id;
            firstName_ = t_firstName;
            lastName_ = t_lastName;
            city_ = t_city;
            userName_ = t_userName;
            staticID_ = (ulong)(DateTime.UtcNow.Subtract(new DateTime(1970,1,1))).TotalSeconds + t_id;   // generate the static id
            password_ = new Password(t_password);    // assign a new password object to the Person's class
            pendingInvitations_ = new List<Invitation>();   // create new invitation lists
            sentInvitations_ = new List<Invitation>();
        }
        // used to build a new person object from a blob file
        private Person(SerializationInfo info, StreamingContext context)
        {   
            try
            {   // try to set all data members using data extracted from the blob
                password_ = (Password)info.GetValue("Password", typeof(Password));
                friends_ = (List<Person>)info.GetValue("Friends",typeof(List<Person>));
                ID = info.GetUInt64("ID");
                firstName_ = info.GetString("FirstName");
                lastName_ = info.GetString("LastName");
                city_ = info.GetString("City");
                userName_ = info.GetString("UserName");
                staticID_ = info.GetUInt64("STATIC_ID");
                interests_ = (List<string>)info.GetValue("Interests", typeof(List<string>));
                pendingInvitations_ = (List<Invitation>)info.GetValue("PendingInvitations", typeof(List<Invitation>));
                sentInvitations_ = (List<Invitation>)info.GetValue("SentInvitations", typeof(List<Invitation>));
            }
            catch
            {   // error
                ErrorHandler.FatalError(ErrorHandler.FatalErrno.PERSON_READ_FAIL);
            }
        }
        // add interest(s) to interest list
        public void AddInterest(params string[] t_next)
        {   // add interests that are not in the list already
            interests_.AddRange(t_next.Where(s => !string.IsNullOrWhiteSpace(s) && !interests_.Contains(s)));
        }
        public bool DeleteInvitation(Invitation t_next, InvitationContext t_contex)
        {   // do a null check
            if(t_next == null)
                return false;   // fail
            switch(t_contex)    // select list based on context
            {
                case InvitationContext.recipient:
                    return pendingInvitations_.Remove(t_next);  // try to delete the invitation
                case InvitationContext.sender:
                    return sentInvitations_.Remove(t_next); // try to delete the invitation
            }
            return false;   // fail
        }
        // add friend
        public bool AddFriends(params Person[] t_next)
        {
            bool output = true; // ret value
            foreach(Person x in t_next) // go through all persons in t_next
            {
                if (x == null) // skip null persons
                    output = false;   // false = error
                foreach (Person p in friends_)
                {   // if x is already in the list, skip it
                    if (p.staticID_ == x.staticID_)
                    {
                        output = false;   // false = error  
                        goto nextPerson;    // uncconditionally jump to next person
                    }
                }
                friends_.Add(x);   // add x to the list
            nextPerson:;
                continue;   // process the next person
            }
            return output;  
        }
        public bool RemoveFriends(params Person[] t_next)
        {
            bool output = true; // ret value
            int i;  // loop count
            foreach(Person x in t_next) // go through all persons in t_next
            {
                if (x == null) // if friend is null then return false
                    output = false;   // false = error
                i = 0;  // zero out loop counter
                foreach (Person p in friends_)  // i like this foreach syntax :)
                {   // if x is in the list
                    if (p.staticID_ == x.staticID_) // compare static IDs
                    {
                        friends_.RemoveAt(i);   // remove the ith friend
                        goto nextPerson;    // uncconditionally jump to next person
                    }
                    i++;    // inc the loop counter
                }
                output = false;   // false = error
            nextPerson:;
                continue;   // process the next person
            }
            return output;
        }
        // add invitations to an invitation list 
        public bool AddInvitations(InvitationContext t_context, params Invitation[] t_next)
        {   // pick the invitation list to use based on context
            switch(t_context)
            {
                case InvitationContext.recipient:
                    if (t_next.Any(x => x == null))
                        return false;   // fail; t_next contains null
                    pendingInvitations_.AddRange(t_next);   // add t_next to the pendingInvitations_ list
                    return true;    // success
                case InvitationContext.sender:
                    if (t_next.Any(x => x == null))
                        return false;   // fail; t_next contains null
                    sentInvitations_.AddRange(t_next);  // add t_next to the sentInvitations_ list
                    return true;    // success
            }
            return false;   // catch-all fail
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {   // called when saving a person object to disk
            try
            {   // try to add all data members to the serializable list
                info.AddValue("Password", password_);
                info.AddValue("ID", ID);
                info.AddValue("FirstName", firstName_);
                info.AddValue("LastName", lastName_);
                info.AddValue("City", city_);
                info.AddValue("UserName", userName_);
                info.AddValue("Friends", friends_);
                info.AddValue("STATIC_ID", staticID_);
                info.AddValue("Interests",interests_);
                info.AddValue("PendingInvitations",pendingInvitations_);
                info.AddValue("SentInvitations",sentInvitations_);
            }
            catch
            {   // error
                ErrorHandler.FatalError(ErrorHandler.FatalErrno.PERSON_SERIAL_FAIL);
            }
        }
    }
    // PASSWORD
    [Serializable()]    // password is serializable
    public struct Password
    {   // DATA MEMBERS
        private byte[] hash_;   // stores the hash of salted text
        private byte[] salt_;   // stores the salt
        private const uint HASHLEN = 32;    // length of hash in bytes
        public const int MIN_PASSWORD_LENGTH = 9;   // minimum password length
        public const int MAX_PASSWORD_REP_CHARS = 3;    // max amt of repeating chars
        public enum ValidatePasswordStatus
        {   // return values indicating any problems with testing passwords
            badRepChars,    // password contains too many repeating chars
            badPasswdLen,   // password is too short
            noError     // no errors occured
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
            for (int i = 0; i < t_clearTextPassword.Length - MAX_PASSWORD_REP_CHARS; i++)
            {   // detect repeating chars in a password; dissallow passwords with repeating chars
                char test = t_clearTextPassword[i];
                limit = i + MAX_PASSWORD_REP_CHARS;
                for (i++; i < limit; i++)
                    if (t_clearTextPassword[i] != test)
                        goto tryNextChar;   // dont invalidate the password
                return ValidatePasswordStatus.badRepChars;  // bad password; invalidate it
            tryNextChar:;   // try the next char in the string
                continue;
            }
            return ValidatePasswordStatus.noError;  // return with no errors 
        }
        // test hash_ against the salted hash of t_clearTextPassword
        public bool TestPassword(string t_clearTextPassword)
        {   // create a buffer that is the size of t_clearTextPassword + the salt
            byte[] saltedTest = new byte[t_clearTextPassword.Length + salt_.Length];
            byte[] testHash;    // used to test against the real hashed password
            int i = 0;  // loop count 
            for (; i < t_clearTextPassword.Length; i++) // memcpy the salt into the buffer 
                saltedTest[i] = (byte)t_clearTextPassword[i];
            for (int ii = 0; ii < salt_.Length; ii++)   // memcpy t_clearTextPassword into the buffer
                saltedTest[i + ii] = salt_[ii];
            using (SHA256Managed algo = new SHA256Managed())    // compute the hash of the buffer
                testHash = algo.ComputeHash(saltedTest);
            for (i = 0; i < testHash.Length; i++)   // memcmp the computed hash against the hash of the real password
                if (testHash[i] != hash_[i])
                    return false;   // fail on any difference
            return true;    // success; passwords match
        }
        // Serialization constructor
        private Password(SerializationInfo info, StreamingContext context)
        {   // TODO: delete these two lines
            hash_ = null;
            salt_ = null;
            try
            {   // try to set the hash and salt to the values stored in the de-serialized file
                hash_ = info.GetValue("Hash", typeof(byte[])) as byte[];
                salt_ = info.GetValue("Salt", typeof(byte[])) as byte[];
            }
            catch
            {   // error
                ErrorHandler.FatalError(ErrorHandler.FatalErrno.PERSON_READ_FAIL);
            }
        }
        // Serialization func
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            try
            {   // try to add hash and salt to the list of values that will be serialized
                info.AddValue("Hash", hash_);
                info.AddValue("Salt", salt_);
            }
            catch
            {   // error
                ErrorHandler.FatalError(ErrorHandler.FatalErrno.PERSON_SERIAL_FAIL);
            }
        }
    }
}
