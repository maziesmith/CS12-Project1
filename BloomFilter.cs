using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CS12_Project_1
{
    // implementation and idea based on <https://blog.michaelschmatz.com/2016/04/11/how-to-write-a-bloom-filter-cpp/>
    // BLOOM FILTER
    // a probabilistic and space-efficient data structure 
    // that can test if an object is present in the strcuture
    // in O(1) time. This test can result in false positives
    // but never false negatives. Adding new elements to the
    // structure is O(1) time operation.
    public class BloomFilter<T>
    {
        // DATA MEMBERS
        const int HASH_COUNT = 4;   // number of hashes per entry
        private List<bool> data_;   // logical list of entries 
        public int Size             // return the number of elements in the logical array
        {
            get { return data_.Count; }
        }
        // FUNCTIONAL METHODS 
        // extract upper and lower words from an int
        // params:
        //  int t_doubleWord; the double word to operate on
        public static Tuple<ushort,ushort> ExtractWords(int t_doubleWord)
        {
            // split hash into 4 bytes
            byte[] bytes = BitConverter.GetBytes(t_doubleWord);
            // merge halfwords into 2 words
            return Tuple.Create(BitConverter.ToUInt16(bytes, 0),BitConverter.ToUInt16(bytes, 2));
        }
        // map the nth hash to an index in the logical list
        // params:
        //  uint t_n;   cardinality of the nth hash
        //  uint t_hashA;   hash A
        //  uint t_hashB;   hash B
        //  uint t_size;    number of elements in the logical array
        public static uint NthHash(
            uint t_n,
            uint t_hashA,
            uint t_hashB,
            uint t_size)
        {
            // merge the hashes and compute the array index
            return (t_hashA + t_n * t_hashB) % t_size;
        }
        // CONSTRUCTOR
        // params:
        //  int size;   the starting capacity
        public BloomFilter(int size)
        {
            data_ = new List<bool>(new bool[size * HASH_COUNT]);
        }
        // OTHER METHODS
        // add a new element to the bloom filter.
        // params:
        //  T t_next    the next element to add
        public T Add(T t_next)
        {
            // compute the hash of t_next and extract the lower and upper words
            Tuple<ushort, ushort> hash = ExtractWords(t_next.GetHashCode());
            for(uint i = 0; i < HASH_COUNT; i++)
            {   // compute the index map of the nth hash; map the results to the logical array
                data_[(int)NthHash(i,hash.Item1,hash.Item2,(uint)data_.Count())] = true;
            }
            return t_next;  // return t_next
        }
        // test if t_next is in the bloom filter;
        // true = t_next could be in the filter;
        // false = t_next definitely not in the filter.
        // params:
        //  T t_next;   the search term to test the bloom filter on
        public bool Test(T t_next)
        {
            // compute the hash of t_next and extract the lower and upper words
            Tuple<ushort, ushort> hash = ExtractWords(t_next.GetHashCode());
            for(uint i = 0; i < HASH_COUNT; i++)
            {   // compute the index map of the nth hash; map the results to the logical array
                if (!data_[(int)NthHash(i, hash.Item1, hash.Item2,(uint)data_.Count())])
                    return false;
            }
            return true;
        }
    }
}
