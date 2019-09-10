using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateTool.Helper
{
    class EntityHelper
    {
        public static string[] libraries = {
             "System",
             "System.Collections.Generic",
             "System.ComponentModel.DataAnnotations",
             "System.Linq",
             "System.Threading.Tasks"
        };

        /// <summary>
        /// Create C - sharp file Models
        /// Author : Duong Nguyen Tan Hoa
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileDir"></param>
        /// <param name="createdFileName"></param>
        public static void EntityMakeFile(string fileName, string fileDir, out string createdFileName)
        {
            string filePath = fileDir + @"\" + fileName + ".cs";
            createdFileName = "";
            /*file existed*/
            if (File.Exists(filePath))
            {
                int count = 0;
                do
                {
                    count++;
                    filePath = fileDir + @"\" + fileName + "(" + count + ")" + ".cs";
                }
                while (File.Exists(filePath));
                createdFileName = fileName + "(" + count + ")" + ".cs";
                File.Create(filePath).Close();
            }
            else
            {
                createdFileName = fileName + ".cs";
                File.Create(filePath).Close();
            }
        }

        /// <summary>
        /// write content for C - Sharp file
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <param name="props"></param>
        /// <param name="fileName"></param>
        /// <param name="fileDir"></param>
        public static void XAFWrite(string nameSpace, string className, string fileName, string fileDir, DataTable table)
        {
            string filePath = fileDir + @"\" + fileName;

            if (File.Exists(filePath))
            {
                EntityInitFile(nameSpace, filePath, className);

                foreach (DataRow row in table.Rows)
                {
                    EntityInsertProperty(row["Property Name"].ToString(), row["Type Value"].ToString(), filePath);
                }
                /*End of file*/
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("\t}");
                    writer.WriteLine("}");
                    /*Close file*/
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// Init for the file
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <param name="filePath"></param>
        /// <param name="className"></param>
        public static void EntityInitFile(string nameSpace, string filePath, string className)
        {
            if (File.Exists(filePath))
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    foreach (string library in libraries)
                    {
                        string usingLibrary = "using " + library + ";";
                        writer.WriteLine(usingLibrary);
                    }

                    writer.WriteLine("namespace " + nameSpace);
                    writer.WriteLine("{");
                    writer.WriteLine("\t" + "public class " + className);
                    writer.WriteLine("\t{");
                    /*Close file*/
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// Write content for a property
        /// </summary>
        /// <param name="propsName"></param>
        /// <param name="type"></param>
        /// <param name="filePath"></param>
        public static void EntityInsertProperty(string propsName, string type, string filePath)
        {
            if (File.Exists(filePath))
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("\t\tpublic " + type + " " + propsName + " { get; set; }");
                    /*Close file*/
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// Insert for Collection
        /// </summary>
        /// <param name="type"></param>
        /// <param name="filePath"></param>
        public static void XAFInsertCollection(string type, string filePath)
        {
            if (File.Exists(filePath))
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.Write("\t\tpublic virtual List<" + type + "> ");
                    writer.WriteLine(type + "es { get; set; }");
                    /*Close file*/
                    writer.Close();
                }
            }
        }

    }
}
