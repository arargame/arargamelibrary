using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.Manager
{
    public class General
    {
        public static List<T> PopulateListRandomlyFromAnother<T>(List<T> source,int amount)
        {
            var array = new T[amount];

            for (int i = 0; i < amount; i++)
            {
                var index = Global.Random.Next(0, source.Count());

                array[i] = source[index];

                source.RemoveAt(index);
            }

            return array.ToList();
        }

        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();

                formatter.Serialize(ms, obj);

                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
