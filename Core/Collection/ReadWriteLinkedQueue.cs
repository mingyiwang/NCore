using System.Collections.Generic;
using Core.Concurrent;

namespace Core.Collection
{

    public class ReadWriteLinkedQueue<T> : Queue<T>
    {

         private readonly ReadWrite _lock  = new ReadWrite();
         private readonly LinkedList<T> _list;

    }

}