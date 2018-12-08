using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CS12_Project_1
{
    public class BloomFilter<T>
    {
        const int hashCount = 4;
        private List<bool> data_;

        // functional
        public static Tuple<ushort,ushort> ExtractWords(int doubleWord)
        {
            // split hash into 4 bytes
            byte[] bytes = BitConverter.GetBytes(doubleWord);
            // merge halfwords into 2 words
            return Tuple.Create(BitConverter.ToUInt16(bytes, 0),BitConverter.ToUInt16(bytes, 2));
        }
        public static uint NthHash(uint n, uint hashA, uint hashB, uint size)
        {
            return (hashA + n * hashB) % size;
        }

        public BloomFilter(int size)
        {
            data_ = new List<bool>(new bool[size * hashCount]);
        }
        public int Size
        {
            get { return data_.Count; }
        }
        public T Add(T t_next)
        {
            Tuple<ushort, ushort> hash = ExtractWords(t_next.GetHashCode());
            for(uint i = 0; i < hashCount; i++)
            {
                data_[(int)NthHash(i,hash.Item1,hash.Item2,(uint)data_.Count())] = true;
            }
            return t_next;
        }
        public bool Test(T t_next)
        {
            Tuple<ushort, ushort> hash = ExtractWords(t_next.GetHashCode());
            for(uint i = 0; i < hashCount; i++)
            {

                if (!data_[(int)NthHash(i, hash.Item1, hash.Item2,(uint)data_.Count())])
                    return false;
            }
            return true;
        }
    }
}
