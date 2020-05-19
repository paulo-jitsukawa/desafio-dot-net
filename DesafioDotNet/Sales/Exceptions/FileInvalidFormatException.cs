using System;

namespace Sales.Exceptions
{
    public class FileInvalidFormatException : Exception
    {
        public int Line { get; private set; }
        public string Name { get; private set; }

        public FileInvalidFormatException(string message, int line, string name, Exception inner = null) : base(message, inner)
        {
            Line = line;
            Name = name;
        }
    }
}
