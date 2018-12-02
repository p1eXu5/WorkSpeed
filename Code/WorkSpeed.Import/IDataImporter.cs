using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using WorkSpeed.Import.Models;

namespace WorkSpeed.Import
{
    public interface IDataImporter
    {
        /// <summary>
        /// File extansions that an instance of IDataImporter supports.
        /// </summary>
        IEnumerable<string> GetFileExtensions();

        /// <summary>
        /// Import data from file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="typeRepository"></param>
        /// <returns></returns>
        /// 
        IEnumerable<ProductivityImportModel> ImportData(string fileName, ITypeRepository typeRepository);

        ///// <summary>
        ///// Imports data from file in asynchronous manner.
        ///// </summary>
        ///// <typeparam name="TModelType">
        /////     Model type with annotated or not annotated public properties
        /////     that corresponds data table in specified file.
        ///// </typeparam>
        ///// <param name="fileName">File name.</param>
        ///// <param name="typeRepository"><see cref="ITypeRepository"/></param>
        ///// <returns><see cref="IEnumerable{T}"/></returns>
        ///// 
        //IEnumerable<TModelType> ImportDataAsync<TModelType> (string fileName, ITypeRepository typeRepository) where TModelType : new ();

        ///// <summary>
        ///// Imports data from file.
        ///// </summary>
        ///// <typeparam name="TModelType">
        /////     Model type with annotated or not annotated public properties
        /////     that corresponds data table in specified file.
        ///// </typeparam>
        ///// <param name="fileName">File name.</param>
        ///// <param name="type"><see cref="Type"/></param>
        ///// <returns><see cref="IEnumerable{T}"/></returns>
        ///// 
        //IEnumerable<TModelType> ImportData<TModelType>(string fileName, Type type)  where TModelType : new();

        ///// <summary>
        ///// Imports data from file in asynchronous manner.
        ///// </summary>
        ///// <typeparam name="TModelType">
        /////     Model type with annotated or not annotated public properties
        /////     that corresponds data table in specified file.
        ///// </typeparam>
        ///// <param name="fileName">File name.</param>
        ///// <param name="type"><see cref="Type"/></param>
        ///// <returns><see cref="IEnumerable{T}"/></returns>
        ///// 
        //IEnumerable<TModelType> ImportDataAsync<TModelType> (string fileName, Type type) where TModelType : new ();
    }
}
