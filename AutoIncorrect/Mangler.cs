using AutoIncorrect.Interfaces;
using System;
using System.Linq;

namespace AutoIncorrect
{
    public class Mangler : IMangler
    {
        private readonly string[] CharPool = new string[3]
            {
                @"qwertyuiop",
                @"asdfghjkl;",
                @"\zxcvbnm,.",
            };

        private Random _random = new Random();

        public string Unmangled { get; }

        public string Mangled { get; private set; }

        public Mangler(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                throw new ArgumentException("Term argument is null or empty.");
            }
            Unmangled = Mangled = term;
            Mangle();
        }

        public IMangler Mangle()
        {
            var preMangled = Mangled;
            var errorType = _random.Next(5);
            switch(errorType)
            {
                case 0:
                    InsertionError();
                    break;
                case 1:
                    RemovalError();
                    break;
                case 2:
                    SwapError();
                    break;
                case 3:
                    CapslockError();
                    break;
                default:
                    CaseError();
                    break;
            }

            if (string.IsNullOrEmpty(Mangled) ||
                string.Compare(Mangled, Unmangled, StringComparison.Ordinal) == 0 ||
                string.Compare(Mangled, preMangled, StringComparison.Ordinal) == 0)
            {
                Mangled = preMangled;
                Mangle();
            }

            return this;
        }

        public override string ToString()
        {
            return Mangled;
        }

        private void InsertionError()
        {
            var insertionPosition = _random.Next(Mangled.Length + 1);
            var insertionCharacter = CharPool[_random.Next(CharPool.Length)][_random.Next(CharPool.First().Length)];
            Mangled = Mangled.Insert(insertionPosition, insertionCharacter.ToString());
        }

        private void RemovalError()
        {
            var removalPosition = _random.Next(Mangled.Length);
            Mangled = Mangled.Remove(removalPosition, 1);
        }

        private void SwapError()
        {
            if(Mangled.Length < 2)
            {
                return;
            }
            var swapPosition = _random.Next(Mangled.Length - 1);
            var swapString = Mangled.Substring(swapPosition, 2);
            Mangled = Mangled.Remove(swapPosition, 2).Insert(swapPosition, new string(swapString.Reverse().ToArray()));
        }

        private void ReplacementError()
        {
        }

        private void CapslockError()
        {
            Mangled = Mangled.ToUpper();
        }

        private void CaseError()
        {
            var caseErrorPosition = _random.Next(Mangled.Length);
            if (char.IsUpper(Mangled[caseErrorPosition]))
            {
                Mangled = Mangled.Insert(caseErrorPosition, char.ToLower(Mangled[caseErrorPosition]).ToString());
            }
            else
            {
                Mangled = Mangled.Insert(caseErrorPosition, char.ToUpper(Mangled[caseErrorPosition]).ToString());
            }
            Mangled.Remove(caseErrorPosition + 1, 1);
        }
    }
}
