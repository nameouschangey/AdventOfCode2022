using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Extension_Object_Then
{
    public static TResult Then<TElement,TResult>(this TElement obj, Func<TElement, TResult> chained)
    {
        return chained(obj);
    }
}