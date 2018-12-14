using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CS12_Project_1
{   // IDATABASE<T> 
    // an imcomplete class that contains abstractions over interfacing with c# file IO functions
    public abstract class IDatabase<T> : ISystem where T : class, new()
    {   // DATA MEMBERS
        public enum Encoding
        {   // encoding id for files; maps to a file extension
            BLOB = 0,   // binary blob file; file content is not view-able in a text editor
            TEXT = 1    // text file; file  file can be viewed in a text editor
        };
        private static readonly string[] FILE_EXT =
        {   // name for file extensions used by databases
            ".bin",  //BLOB
            ".txt"   //TEXT
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
            rootDirPath = t_rootDirPath;    // set the root Dir
            encoding = t_encoding;  // set the encoding
        }
        // OTHER METHODS
        // initialize data<T> to a new T
        // initialize the state of the database; can be overridden
        public virtual bool Initalize()
        {
            data = new T();
            return true;    // TODO: replace ret value with void
        }   
        // try to write to a file; how the data will be interpreted and written to file is based on the encoding
        // params:
        //  string t_fileName; the file name of the file to write to
        //  T t_object; the data to write to file
        public virtual bool WriteFile(string t_fileName, T t_object)
        {
            return writeFile_[(uint)encoding](GetFullPath(t_fileName), t_object);
        }
        // try to create a new file; how the new file will be interpreted is based on the encoding
        // params:
        //  string t_fileName; the name of the new file
        public virtual bool NewFile(string t_fileName)
        {
            return newFile_[(uint)encoding](GetFullPath(t_fileName));
        }
        // try to load a preexisting file
        // params:
        //  string t_fileName; the file to load
        public virtual bool LoadFile(string t_fileName)
        {
            if ((data = loadFile_[(uint)encoding](GetFullPath(t_fileName)) as T) == null)
                return false;   // loading a file was a success
            return true;    // failed to load file
        }
        // implementation for writing to a file; function behavior is selected based on the encoding state
        //  blob files use BinaryFormatter
        //  text files use StreamWriter
        private readonly Func<string, T, bool>[] writeFile_ =
        {
            //Write blob to file
            (string t_path, T t_object) =>
            {
                if (!File.Exists(t_path))   // fail if the file does not exist
                    return false;   // false = fail
                try
                {
                    using(Stream fileOut = File.Open(t_path,FileMode.Open)) 
                    {   // open file stream; the opened stream is closed when the using {} block goes out of scope
                        BinaryFormatter bf = new BinaryFormatter(); // new BinaryFormatter for reading blob files
                        bf.Serialize(fileOut,t_object); // try to serialize the file into an object of type T
                    }
                }
                catch
                {   // error; STOP EVERYTHING alerting the programmer (me) that they messed up
                    ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_WRITE_FAIL);
                    return false;
                }
                return true;    // true = file written successfully
            },  
            // write text to file
            (string t_path, T t_object) =>
            {
                if(typeof(T) != typeof(string)) // error if T is not a string
                    ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_WRITE_TYPE_FAIL);
                if (!File.Exists(t_path))   // fail if the file does not exist
                    return false;   // false = fail
                try
                {
                    using(Stream fileOut = File.Open(t_path,FileMode.Open))
                    {   // open file stream; the opened stream is closed when the using {} block goes out of scope
                        StreamWriter sw = new StreamWriter(fileOut);    // new StreamWriter for writing text to file
                        sw.WriteLine(t_object as string);   // write the text to file
                        sw.Close(); // delete the StreamWriter
                    }
                }
                catch
                {   // error; STOP EVERYTHING alerting the programmer (me) that they messed up
                    ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_WRITE_FAIL);
                    return false;
                }
                return true;    // true = file written successfully
            }   //Write text to file
        };
        // implementation for reading from a file; function behavior is selected based on the encoding state
        private readonly Func<string, object>[] loadFile_ =
        {
            // load from a blob file
            (string t_path) =>
            {
                if (!File.Exists(t_path))   
                    return null;    // return null if the file does not exist
                try
                {
                    using (Stream fileIn = File.Open(t_path,FileMode.Open))
                    {   // open file stream; the opened stream is closed when the using {} block goes out of scope
                        BinaryFormatter bf = new BinaryFormatter(); // new BinaryFormatter for de-serializing a file 
                        return bf.Deserialize(fileIn) as T; // De-serialize the file and return the result as T; T can be null
                    }
                }
                catch
                {   
                    // return null if any errors occur 
                    return null;
                }
            },
            // load from a text file
            (string t_path) =>
            {
                if(typeof(T) != typeof(string)) // error if T is not a string
                    ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_WRITE_TYPE_FAIL);
                if (!File.Exists(t_path))
                    return null;    // return null if the file does not exist
                try
                {
                    using (StreamReader sr = new StreamReader(File.Open(t_path,FileMode.Open)))
                    {   // open file stream; the opened stream is closed when the using {} block goes out of scope
                        return sr.ReadToEnd();  // read the entire file and return it
                    }
                }
                catch
                {   
                    // return null if any errors occur 
                    return null;
                }
            }
        };  
        // implementation for creating a new file
        private readonly Func<string, bool>[] newFile_ =
        {
            // new blob file
            (string t_path) =>
            {
                if (File.Exists(t_path))
                    return false;   // fail if the file already exists
                try
                {
                    using (Stream fs = File.Create(t_path));    // create a new file
                }
                catch
                {   // error
                    ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_NEW_FILE_FAIL);
                    return false;
                }
                return true;    // no error
            },
            // new text file
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
                {   // error
                    ErrorHandler.AssertFatalError(ErrorHandler.FatalErrno.DATABASE_NEW_FILE_FAIL);
                    return false;
                }
                return true;    // no error
            }
        };
        // Get the full path including the root dir path + file name 
        private string GetFullPath(string t_fileName)
        {
            return string.Concat(rootDirPath,t_fileName, FILE_EXT[(uint)encoding]);
        }
    }
}
