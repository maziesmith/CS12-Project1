using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS12_Project_1
{
    // PERSONS DATABASE
    //  stores person objects
    //  provides an interface for searching and accessing person objects
    public class PersonsDatabase : IDatabase<List<Person>>
    {
        private uint nextPersonId_ = 0; // the id of the next person; goes up by 1 everytime a new person is created
        public const int INITAL_SIZE = 10;  // inital size of Data<Person>
        public const int BLOOMFILTER_STARTING_SIZE = 10;    // inital size of the bloom filter
        public BloomFilter<string> bloomFilter = new BloomFilter<string>(BLOOMFILTER_STARTING_SIZE);    // a bloom filter object 
        private Person cached_ = null;  // person object cache
        private string cachedName_ = null;  // name cache
        private Dictionary<string, bool> cityTable_;    // used for accessing logical values via string very quickly 
        private Dictionary<string, bool> interestTable_;    // used for accessing logical values via string very quickly 
        // adds person objects to the database
        public int UserCount
        {
            get { return data.Count; }
        }
        // register a new person object in the database
        public bool AddPerson(string t_firstName, string t_lastName, string t_city, string t_userName, string t_password)
        {
            if (BloomFilterSearch(t_userName) != null)  // test if the person object is already in the database
                return false;   // fail; user already exists 
            Person next = ((Func<Person>)(() =>     // a person object that stores the result of registering a new person object in the database
            {   // make the person object via factory pattern
                object ret = Person.PersonFactory.BuildPerson(t_firstName, t_lastName, t_city, t_userName, t_password, nextPersonId_);
                if (ret == null)    // do a null check
                    return null;    // fail
                if (ret.GetType() != typeof(Person))
                {
                    Parent.Callback(ret, this); // tell the parent that this operation failed
                    return null;   // fail; could not add person
                }
                return ret as Person;   // success
            }))();
            if (next == null)   // do a null check
                return false;   // fail
            nextPersonId_++;    
            data.Add(next); // add the new person object to the database
            RebuildBloomFilter(t_userName); // cleanup stuff
            FullRebuildCityTable();
            ClearCache();
            return true;    // success
        }
        // allow readonly access to data
        public IReadOnlyList<Person> Data
        {   
            get { return data.AsReadOnly(); }
        }
        // get all Invitations
        public IReadOnlyList<Invitation> Invitations
        {
            get { return data?.Select(x => x.SentInvitations).SelectMany(x => x).ToArray(); }
        }
        // allow readonly access to the interestTable
        public Dictionary<string,bool> InterestTable
        {   
            get { return interestTable_; }
        }
        // allow readonly access to the cityTable
        public Dictionary<string,bool> CityTable
        {   
            get { return cityTable_; }
        }
        // set all values in the interest table to false
        public void ZeroOutInterestTable()
        {
            interestTable_.Values.Select(x => false);
        }
        // set all values in the City Table  to false
        public void ZeroOutCityTable()
        {
            cityTable_.Values.Select(x => false);
        }
        // use the bloom filter to test if a person object exists...
        // then do a linear search
        public Person BloomFilterSearch(string t_userName)
        {
            if (!bloomFilter.Test(t_userName))
                return null;    // user is not in the database
            // user is somewhere in the database
            return CachedLinearSearch(t_userName);
        }
        // rebuild the Bloom Filter
        private void RebuildBloomFilter(string t_userName)
        {
            if (data.Count() > bloomFilter.Size) 
                FullRebuildBloomFilter();   // full rebuild
            else
                bloomFilter.Add(t_userName);    // add new key
        }
        // do a full rebuild of the Bloom Filter
        private void FullRebuildBloomFilter()
        {
            if (data == null)   // do a null check
                return; // fail
            const int NEXT_SIZE = 10;
            bloomFilter = new BloomFilter<string>(bloomFilter.Size + NEXT_SIZE);    // expand the size of the bloom filter
            foreach (Person p in data)  // populate the bloom filter 
            {
                bloomFilter.Add(p.UserName);
            }
        }
        // rebuild the interest table
        public void FullRebuildInterestTable()
        {   // get all interests from all person objects and put them in a dictionary
            interestTable_ = data
                .Select(x => x.Interests)
                .SelectMany(x => x)
                .Distinct()
                .ToDictionary(z => z, z => false);
        }
        // rebuid the City Table
        private void FullRebuildCityTable()
        {
            cityTable_ = data.Select(x => x.City).Distinct().ToDictionary( y => y, y => false);
        }
        // clear the cache used by linear search
        private void ClearCache()
        {
            cached_ = null;
            cachedName_ = null;
        }
        // gets a person object by username; uses caching for faster access 
        public Person CachedLinearSearch(string t_userName)
        {   // cache gives a result very quickly;
            // but only after doing a liner search once.
            if(cached_ != null
            && cached_.UserName == t_userName)
                return cached_;
            if (t_userName == cachedName_)
                return null;
            // linear search
            foreach(Person p in data)
            {
                if (p.UserName == t_userName)
                {   // update the cache to be p
                    cached_ = p;
                    return p;   // return p
                }
            }
            // update the cache to be null
            cached_ = null;
            cachedName_ = t_userName;
            return null;
        }
        // TODO: unused func, delete this
        /*
        private bool RemovePerson(string t_userName)
        {
            Person target;
            if ((target = BloomFilterSearch(t_userName)) == null)
                return false;
            data.RemoveAt((int)target.ID);
            FullRebuildBloomFilter();
            ClearCache();
            return true;
        }
        */
        // write the database state to file
        public bool Save(string t_fileName)
        {   // write to file
            return base.WriteFile(t_fileName, data);
        }
        // initalize the database by loading inportant files and creating new ones
        public void Initalize(string t_databaseFileName)
        {
            if(!NewFile(t_databaseFileName))    // try to create a new database file to save the database state to
                LoadFile(t_databaseFileName);   // load the file if it already exists
            if(data == null)    // if data<Person> is null, instantiate a new T where T is a List<Person>
                base.Initalize();
            FullRebuildBloomFilter();   // rebuild all tables
            FullRebuildCityTable();     
            FullRebuildInterestTable();
            ClearCache();   // clear cache
        }
        // CONSTRUCTOR
        // params:
        //  string t_databaseRoot;  database root path
        //  ISystem t_parent;   not used; TODO: Delete this
        public PersonsDatabase(string t_databaseRoot, ISystem t_parent) : base(t_databaseRoot, Encoding.BLOB, t_parent)
        {
        }
    }
}