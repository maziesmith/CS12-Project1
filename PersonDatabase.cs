using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS12_Project_1
{
    public class PersonsDatabase : IDatabase<List<Person>>
    {
        private uint nextPersonId_ = 0; // the id of the next person; goes up by 1 everytime a new person is created
        public const int INITAL_SIZE = 10;  // inital size of Data<Person>
        public const int BLOOMFILTER_STARTING_SIZE = 10;    // inital size of the bloom filter
        public BloomFilter<string> bloomFilter = new BloomFilter<string>(BLOOMFILTER_STARTING_SIZE);    // a bloom filter object 
        private Person cached_ = null;  // person object cache
        private string cachedName_ = null;  // name cache
        private Dictionary<string, bool> cityTable_;
        private Dictionary<string, bool> interestTable_;
        /// <summary>
        /// adds person objects to the database
        /// </summary>
        /// <param name="t_firstName"></param>
        /// <param name="t_lastName"></param>
        /// <param name="t_city"></param>
        /// <param name="t_userName"></param>
        /// <param name="t_password"></param>
        /// <returns></returns>
        public int UserCount
        {
            get { return data.Count; }
        }
        public bool AddPerson(string t_firstName, string t_lastName, string t_city, string t_userName, string t_password)
        {
            if (BloomFilterSearch(t_userName) != null)
                return false;
            Person next;
            if((next = PersonFactory.BuildPerson(t_firstName, t_lastName, t_city, t_userName, t_password, nextPersonId_, this)) == null)
                return false;
            nextPersonId_++;
            data.Add(next);
            RebuildBloomFilter(t_userName);
            FullRebuildCityTable();
            ClearCache();
            return true;
        }
        // IMMUTABLE ACCESSORS
        public IReadOnlyList<Person> Data
        {   // allow readonly access to data
            get { return data.AsReadOnly(); }
        }
        public IReadOnlyList<Invitation> Invitations
        {
            get { return data.Select(x => x.sentInvitations_).SelectMany(x => x).ToArray(); }
        }
        public Dictionary<string,bool> InterestTable
        {   // allow readonly access to the interestTable
            get { return interestTable_; }
        }
        public Dictionary<string,bool> CityTable
        {   // allow readonly access to the cityTable
            get { return cityTable_; }
        }
        public void ZeroOutInterestTable()
        {
            interestTable_.Values.Select(x => false);
        }
        public void ZeroOutCityTable()
        {
            cityTable_.Values.Select(x => false);
        }

        public Person BloomFilterSearch(string t_userName)
        {
            if (!bloomFilter.Test(t_userName))
                return null;    // user is not in the database
            // user is somewhere in the database
            return CachedLinearSearch(t_userName);
        }
        private void RebuildBloomFilter(string t_userName)
        {
            if (data.Count() > bloomFilter.Size) 
                FullRebuildBloomFilter();   // full rebuild
            else
                bloomFilter.Add(t_userName);    // add new key
        }
        private void FullRebuildBloomFilter()
        {
            if (data == null)
                return;
            const int NEXT_SIZE = 10;
            bloomFilter = new BloomFilter<string>(bloomFilter.Size + NEXT_SIZE);
            foreach (Person p in data)
            {
                bloomFilter.Add(p.UserName);
            }
        }
        public void FullRebuildInterestTable()
        {
            interestTable_ = data
                .Select(x => x.interests_)
                .SelectMany(x => x)
                .Distinct()
                .ToDictionary(z => z, z => false);
        }
        private void FullRebuildCityTable()
        {
            cityTable_ = data.Select(x => x.City).Distinct().ToDictionary( y => y, y => false);
        }
        private void ClearCache()
        {
            cached_ = null;
            cachedName_ = null;
        }
        /// <summary>
        /// gets a person object by username; uses caching for faster access 
        /// </summary>
        /// <param name="t_userName"></param>
        /// <returns></returns>
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