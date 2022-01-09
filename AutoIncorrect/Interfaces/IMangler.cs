using System;
using System.Collections.Generic;
using System.Text;

namespace AutoIncorrect.Interfaces
{
    public interface IMangler
    {
        string Unmangled { get; }
        string Mangled { get; }
        IMangler Mangle();
    }
}
