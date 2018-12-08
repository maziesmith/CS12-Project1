using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS12_Project_1
{
    public static class ErrorHandler
    {
        public enum FatalErrno
        {   // error codes for each error caused by the program; maps to an error message
            PERSON_CONSTRUCT_FAIL = 0,
            PERSON_SERIAL_FAIL = 1,
            PERSON_READ_FAIL = 2,
            DATABASE_READ_FAIL = 3,
            DATABASE_WRITE_FAIL = 4,
            DATABASE_NEW_FILE_FAIL = 5,
            DATABASE_WRITE_TYPE_FAIL = 6,
            GENERAL_ERROR = 7
        };
        private enum Prefixno
        {   // prefix id for error messages; maps to a string
            FATAL = 0,
            WARN = 1
        };
        private static readonly string[] fatalErrMsg =
        {   // contains error messages
            "Malformed person constructor call.",
            "Could not serialize object.",
            "Could not read data from file.",
            "Database could not read from file.",
            "Database could not write to file.",
            "Database could not create new file.",
            "Database tried to write to file but was set to wrong type",
            "Default error"
        };
        private static readonly string[] msgPrefix =
        {   // contains message prefixs
            "FATAL",
            "WARN",
        };
        private static string GetErrorMsg(Prefixno prefixno, FatalErrno errno)
        {   // return a formatted error message
            return string.Concat(msgPrefix[(uint)prefixno], ": ", fatalErrMsg[(uint)errno]);
        }
        public static void AssertFatalError(FatalErrno errno)
        {   // assert with a formatted error message
            throw new Exception(GetErrorMsg(Prefixno.FATAL, errno));
        }
        public static string AlertFatalError(FatalErrno errno)
        {   // get a formatted warning message
            return GetErrorMsg(Prefixno.WARN, errno);
        }
    }
}
