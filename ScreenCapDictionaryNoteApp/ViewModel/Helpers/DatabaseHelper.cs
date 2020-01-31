using ScreenCapDictionaryNoteApp.Interface;
using ScreenCapDictionaryNoteApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenCapDictionaryNoteApp.ViewModel.Helpers
{
    public class DatabaseHelper
    {
        public static string dbFile = Path.Combine(Environment.CurrentDirectory, "dictionaryDb.db3");


        public static int GetCurrentIdIncrement<T>(SQLiteConnection connection) where T : new()
        {
            int currentIdIncrement = int.Parse(
           connection.Query<SqliteSequence>("SELECT * FROM sqlite_sequence")
           .ToList().Where(s => s.name == typeof(T).Name)
           .FirstOrDefault()
           .seq);

            return currentIdIncrement;
        }


        public static bool Insert<T>(T item) where T : IWithIdAndName, new()
        {
            bool success = false;
            int numberOfRows;
            int currentIdIncrement;

            using (SQLiteConnection connection = new SQLiteConnection(dbFile))
            {
                connection.CreateTable<T>();
                try
                {
                    currentIdIncrement =
                      GetCurrentIdIncrement<T>(connection);


                    if (typeof(T).Name == "Vocab")
                    {
                        (item as Vocab).Word = (item as Vocab).Word + "-" + (currentIdIncrement + 1);
                    }
                    else
                    {
                        item.Name = item.Name + "-" + (currentIdIncrement + 1);
                    }
                }
                catch (Exception err)
                {
                    item.Name = item.Name + "-1";

                }
                finally
                {

                    numberOfRows = connection.Insert(item);
                }

            }
            if (numberOfRows > 0)
            {
                success = true;
            }
            return success;

        }


        public static bool Update<T>(T item)
        {
            bool success = false;
            int numberOfRows;




            using (SQLiteConnection connection = new SQLiteConnection(dbFile))
            {
                connection.CreateTable<T>();
                numberOfRows = connection.Update(item);
            }
            if (numberOfRows > 0)
            {
                success = true;
            }
            return success;
        }


        public static bool Delete<T>(T item)
        {
            bool success = false;
            int numberOfRows;
            using (SQLiteConnection connection = new SQLiteConnection(dbFile))
            {
                connection.CreateTable<T>();
                numberOfRows = connection.Delete(item);
            }
            if (numberOfRows > 0)
            {
                success = true;
            }
            return success;
        }


        public static List<T> Read<T>() where T : new()
        {
            List<T> items;
            using (SQLiteConnection connection = new SQLiteConnection(dbFile))
            {
                connection.CreateTable<T>();
                items = connection.Table<T>().ToList();

            }
            return items;
        }
    }
}
