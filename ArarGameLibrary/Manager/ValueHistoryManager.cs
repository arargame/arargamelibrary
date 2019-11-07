using ArarGameLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Manager
{
    public struct HistoryObjectSetting : IBaseObject
    {
        public Guid Id { get; set; }

        public object Value { get; set; }

        public DateTime RecordDate { get; set; }

        public HistoryObjectValue(object value)
        {
            Value = value;

            RecordDate = DateTime.Now;
        }
    }

    public struct HistoryObject : BaseObject
    {
        public string PropertyName { get; private set; }

        public int Amount { get; private set; }

        public List<HistoryObjectValue> ValueList { get; private set; }

        public HistoryObject(string propertyName, int amount = 1)
        {
            ValueList = new List<HistoryObjectValue>();

            PropertyName = propertyName;

            if (amount < 1)
                amount = 1;

            Amount = amount;
        }

        public void Update()
        {
            if (ValueList.Count > Amount)
            {
                ValueList.RemoveAt(0);
            }
        }

        public HistoryObjectValue AddValue(object value)
        {
            ValueList.Add(new HistoryObjectValue());

            return this;
        }
    }

    public class ValueHistoryManager
    {
        List<HistoryObject> HistoryObjects { get; set; }

        public ValueHistoryManager()
        {
            HistoryObjects = new List<HistoryObject>();
        }

        public ValueHistoryManager SetHistoryObject(string propertyName, int amount)
        {
            if (!HistoryObjects.Any(ho => ho.PropertyName == propertyName))
                HistoryObjects.Add(new HistoryObject());

            return this;
        }

        public void Update()
        {
            foreach (var historyObject in HistoryObjects)
            {
                historyObject.Update();
            }
        }

        //public List<ValueHistoryObject> GetHistoricalValues(Func<HistoryObject,bool> predicate)
        //{
        //    return HistoryObjects.Where(predicate).ToList();
        //}
    }
}
