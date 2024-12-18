using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace VendingMachineDataInjectionInterfaces
{
    public class MVVM: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    public class QueueMVVM<T>: Queue<T>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public new void Enqueue(T item)
        {
            base.Enqueue(item);
            OnQueue(item);
        }

        public new T Dequeue()
        {
            T item = base.Dequeue();
            OnDequeue(item);
            return item;
        }

        public new void Clear()
        {
            base.Clear();
            OnClear();
        }

        protected void OnQueue(T item)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T>() { item }));
        }

        protected void OnDequeue(T item)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<T>() { item }, 0));
        }

        protected void OnClear()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected void OnInitialized(List<T> items)
        {
            this.Clear();
            if (items?.Any(x => x != null) ?? false)
            {
                items.ForEach(x =>
                {
                    if (x != null)
                    {
                        this.Enqueue(x);
                    }
                });
            }
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, items));
        }
    }
}
