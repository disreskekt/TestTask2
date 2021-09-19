using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TestTask2
{
    public class KeyValuePairComparer : IComparer<KeyValuePair<string, int>>
    {
        public int Compare(KeyValuePair<string, int> x, KeyValuePair<string, int> y)
        {
            return x.Value.CompareTo(y.Value);
        }
    }
}
