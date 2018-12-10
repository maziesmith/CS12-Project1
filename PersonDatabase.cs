using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS12_Project_1
{
    public class PersonsDatabase : IDatabase<List<Person>>
    {
        private uint nextPersonId_ = 0;
        public const string databaseFileName = "AIDAN BIRD - HeadEssayData";
        public const int INITAL_SIZE = 10;
        public const int BLOOMFILTER_STARTING_SIZE = 10;
        public BloomFilter<string> bloomFilter = new BloomFilter<string>(BLOOMFILTER_STARTING_SIZE);
        private Person cached_ = null;  // person object cache
        private string cachedName_ = null;  // name cache
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
            //TODO: restrict to unique usernames
            Person next;
            if((next = PersonFactory.BuildPerson(t_firstName, t_lastName, t_city, t_userName, t_password, nextPersonId_, this)) == null)
                return false;
            nextPersonId_++;
            data.Add(next);
            RebuildBloomFilter(t_userName);
            ClearCache();
            WriteFile(databaseFileName, data);
            return true;
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
        {
            // cache gives a result very quickly;
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
                {   // update the cache
                    cached_ = p;
                    return p;
                }
            }
            // update the cache
            cached_ = null;
            cachedName_ = t_userName;
            return null;
        }
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
        public PersonsDatabase(string t_databaseRoot, ISystem t_parent) : base(t_databaseRoot, Encoding.BLOB, t_parent)
        {
            if(!NewFile(databaseFileName))
                LoadFile(databaseFileName);
            if(data == null)
                Initalize();
            FullRebuildBloomFilter();
            ClearCache();
        }
    }
}
