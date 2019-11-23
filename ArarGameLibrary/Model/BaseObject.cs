using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueHistoryManagement;

namespace ArarGameLibrary.Model
{
    public interface IBaseObject
    {
        Guid Id { get; set; }

        string Name { get; set; }

        string Description { get; set; }

        DateTime CreateDate { get; set; }

        DateTime UpdateDate { get; set; }

        bool IsInPerformanceMode { get; set; }
    }

    public abstract class BaseObject : IBaseObject, ICloneable
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public bool IsInPerformanceMode { get; set; }

        public ValueHistoryManager ValueHistoryManager { get; set; }

        public BaseObject()
        {
            Id = Guid.NewGuid();

            CreateDate = UpdateDate = DateTime.Now;

            ValueHistoryManager = new ValueHistoryManager();
        }

        public Object Clone()
        {
            return MemberwiseClone();
        }

        public string MemberInfoName
        {
            get
            {
                return GetType().Name;
            }
        }
    }
}
