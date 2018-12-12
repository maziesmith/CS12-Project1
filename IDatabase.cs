using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CS12_Project_1
{
    // an imcomplete class that contains abstractions over interfacing with c# file IO functions
    public abstract class IDatabase<T> : ISystem where T : class, new()
    {
        // DATA MEMBERS
        public enum Encoding
        {   // encoding id for files; maps to a file extention
            BLOB = 0,
            TEXT = 1
        };
        private static readonly string[] FILE_EXT =
        {   // name for file extentions used by databases
            ".bin",  //EXT_BLOB
            ".txt"   //EXT_TEXT
        };
        public readonly Encoding encoding; // governs how files should be saved; 
        public readonly string rootDirPath = null;  // contains the path where files are looked up and saved to
        protected T data = null;    // contains the data of type T; data is obtained from file reads

        // CONSTRUCTOR
        // params:
        //  string t_rootDirPath;   path to the root directory to stage file IO on
        //  Encoding t_encoding;    the encoding of the files
        //  ISystem t_parent;   parent object to callback
        public IDatabase(
            string t_rootDirPath,
            Encoding t_encoding,
            ISystem t_parent) : base(t_parent)
        {
            rootDirPath = t_rootDirPath;    // set the root dir
            encoding = t_encoding;  // set the encoding
        }

        // OTHER METHODS
        // initalize data<T>
        public virtual bool Initalize()
        {
            data = new T();
            return true;
        }   // initialize the state of the database; can be overriden
        public virtual bool WriteFile(string t_fileName, T t_object)
        {
            return writeFile_[(uint)encoding](GetFullPath(t_fileName), t_object);
        }
        public virtual bool NewFile(string t_fileName)
        {
            return newFile_[(uint)encoding](GetFullPath(t_fileName));
        }
        public virtual bool LoadFile(string t_fileName)
        {
            if ((data = loadFile_[(uint)encoding](GetFullPath(t_fileName)) as T) == null)
                return false;
            return true;
        }
        private readonly Func<string, T, bool>[] writeFile_ =
        {
            (string t_path, T t_object) =>
            {
                if (!File.Exists(t_path))
                    return false;
                try
                {
                    using(Stream fileOut = File.Open(t_path,FileMode.Open))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(fileOut,t_object);
                    }
                }
                catch
                {
                    ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_WRITE_FAIL);
                    return false;
                }
                return true;
            },  //Write blob to file
            (string t_path, T t_object) =>
            {
                if(typeof(T) != typeof(string))
                    ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_WRITE_TYPE_FAIL);
                if (!File.Exists(t_path))
                    return false;
                try
                {
                    using(Stream fileOut = File.Open(t_path,FileMode.Open))
                    {
                        StreamWriter sw = new StreamWriter(fileOut);
                        sw.WriteLine(t_object as string);
                        sw.Close();
                    }
                }
                catch
                {
                    ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_WRITE_FAIL);
                    return false;
                }
                return true;
            }   //Write text to file
        };  // write data to disk; file encoding is governed by the encoding member
        private readonly Func<string, object>[] loadFile_ =
        {
            (string t_path) =>
            {
                if (!File.Exists(t_path))
                    return null;
                try
                {
                    using (Stream fileIn = File.Open(t_path,FileMode.Open))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        return bf.Deserialize(fileIn) as T;
                    }
                }
                catch
                {   //TODO: assert on error
                    //ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_WRITE_FAIL);
                    return null;
                }
            },  //load from blob
            (string t_path) =>
            {
                if(typeof(T) != typeof(string))
                    ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_WRITE_TYPE_FAIL);
                if (!File.Exists(t_path))
                    return null;
                try
                {
                    using (StreamReader sr = new StreamReader(File.Open(t_path,FileMode.Open)))
                    {
                        return sr.ReadToEnd();
                    }
                }
                catch
                {   //TODO: assert on error
                    //ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_WRITE_FAIL);
                    return null;
                }
            }   //load from text
        };  // load a file from disk;  file encoding is governed by the encoding member
        private readonly Func<string, bool>[] newFile_ =
        {
            (string t_path) =>
            {
                if (File.Exists(t_path))
                    return false;
                try
                {
                    using (Stream fs = File.Create(t_path));
                }
                catch
                {
                    ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_NEW_FILE_FAIL);
                    return false;
                }
                return true;
            },  //make new blob file
            (string t_path) =>
            {
                if (File.Exists(t_path))
                    return false;
                try
                {
                    //using (Stream fs = File.Create(string.Concat(t_path,FILE_EXT[(uint)Encoding.TEXT])));
                    using (Stream fs = File.Create(t_path));
                }
                catch
                {
                    ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_NEW_FILE_FAIL);
                    return false;
                }
                return true;
            }   //make new text file
        };  // creates a new file; the file type is governed by the encoding member
        private string GetFullPath(string t_fileName) //
        {
            return string.Concat(rootDirPath,t_fileName, FILE_EXT[(uint)encoding]);
        }
    }
}
