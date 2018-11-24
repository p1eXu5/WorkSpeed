using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Import
{
    public interface IDataImporter
    {
        /// <summary>
        /// Imports data from file.
        /// </summary>
        /// <typeparam name="TModelType">
        ///     Model type with annotated or not annotated public properties
        ///     that corresponds data table in specified file.
        /// </typeparam>
        /// <param name="fileName">File name.</param>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        IEnumerable<TModelType> ImportData<TModelType>(string fileName)  where TModelType : new();

        /// <summary>
        /// Imports data from file in asynchronous manner.
        /// </summary>
        /// <typeparam name="TModelType">
        ///     Model type with annotated or not annotated public properties
        ///     that corresponds data table in specified file.
        /// </typeparam>
        /// <param name="fileName">File name.</param>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        /// 
        IEnumerable<TModelType> ImportDataAsync<TModelType> (string fileName) where TModelType : new ();
    }
}
