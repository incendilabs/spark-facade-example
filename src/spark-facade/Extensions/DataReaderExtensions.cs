using System;
using System.Collections.Generic;
using System.Data;

namespace Spark.Facade.Extensions
{
    public static class DataReaderExtensions
    {
        /// <summary>
        /// Transforms a IDataReader to a IEnumerable<T>.
        /// </summary>
        /// <typeparam name="T">
        /// Generic representing the class we want our records to be transformed to.
        /// </typeparam>
        /// <param name="reader">
        /// The data reader holding the data to be transformed.
        /// </param>
        /// <returns>
        /// A IEnumerable<T> containing the data from the data reader.
        /// </returns>
        /// <remarks>
        /// The property names of type T must conform to the fields of the IDataReader.
        /// </remarks>
        public static IEnumerable<T> TransformTo<T>(this IDataReader reader)
            where T : class
        {
            Type transformToType = typeof(T);

            while (reader.Read())
            {
                T transformTo = Activator.CreateInstance<T>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    //
                    // TODO: Add insanity checks to see if the property type matches, if not then throw a proper excpetion.
                    //
                    var propInfo = transformToType.GetProperty(reader.GetName(i));
                    if (propInfo == null) continue;
                    
                    var value = reader.GetValue(i);
                    if(value == DBNull.Value) continue;

                    propInfo.SetValue(transformTo, value);
                }
                //
                yield return transformTo;
            }
        }
    }
}
