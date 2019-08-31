using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateTool.Helper
{
    public class XAFHelper
    {

        public static string[] libraries = {"DevExpress.ExpressApp.ConditionalAppearance", "DevExpress.ExpressApp.DC",
                                     "DevExpress.ExpressApp.Model", "DevExpress.Persistent.Base",
                                     "DevExpress.Persistent.BaseImpl", "DevExpress.Xpo", "System", "System.Collections.Generic",
                                     "System.Linq", "System.Text", "System.Threading.Tasks"};
        
        /// <summary>
        /// Create C - sharp file Business Object
        /// Author : Duong Nguyen Tan Hoa
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileDir"></param>
        /// <param name="countFrom"></param>
        public static void XAFMakeFile(string fileName, string fileDir, out string createdFileName)
        {
            string filePath  = fileDir + @"\" + fileName + ".cs";
            createdFileName = "";
            /*file existed*/
            if(File.Exists(filePath))
            {
                int count = 0;
                do
                {
                    count++;
                    filePath = fileDir + @"\" + fileName + "(" + count + ")" + ".cs";
                }
                while(File.Exists(filePath));
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
        public static void XAFWrite(string nameSpace, string className, string fileName, string fileDir)
        {
            string filePath = fileDir + @"\" + fileName;
            
            if(File.Exists(filePath))
            {
                XAFInitFile(nameSpace, filePath, className);
                XAFInsertProperty("fullName", "string", filePath);
                XAFInsertCollection("TrainingClass", "Employee-TrainingClass", filePath);

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
        /// <param name="fileName"></param>
        /// <param name="fileDir"></param>
        public static void XAFInitFile(string nameSpace, string filePath, string className)
        {
            if(File.Exists(filePath))
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    foreach(string library in libraries)
                    {
                        string usingLibrary = "using " + library + ";";
                        writer.WriteLine(usingLibrary);
                    }

                    writer.WriteLine("namespace " + nameSpace);
                    writer.WriteLine("{");
                    writer.WriteLine("\t[Persistent(@\"" + className + "\")]");
                    writer.WriteLine("\t[DefaultClassOptions]");
                    writer.WriteLine("\t"+ "public class " + className + ": BaseObject");
                    writer.WriteLine("\t{");
                    writer.WriteLine("\t\tpublic " + className + "(Session session) : base(session) { }");
                    writer.WriteLine("\t\tpublic override void AfterConstruction()");
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tbase.AfterConstruction();");
                    writer.WriteLine("\t\t}");
                    /*Close file*/
                    writer.Close();
                }
            }
        }
        
        /// <summary>
        /// Write contnnt for a property
        /// </summary>
        /// <param name="propsName"></param>
        /// <param name="type"></param>
        /// <param name="filePath"></param>
        public static void XAFInsertProperty(string propsName, string type, string filePath)
        {
            if (File.Exists(filePath))
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("\t\tprivate " + type + " _" + propsName + ";");
                    writer.WriteLine("\t\tpublic " + type + " " +propsName);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget => " + "_" + propsName + ";");
                    writer.WriteLine("\t\t\tset => SetPropertyValue(nameof(" + propsName + "), ref " + "_" + propsName + ", value );");
                    writer.WriteLine("\t\t}");
                    /*Close file*/
                    writer.Close();
                }
            }
        }
        
        /// <summary>
        /// Insert for XPCollection
        /// </summary>
        /// <param name="type"></param>
        /// <param name="associationName"></param>
        /// <param name="filePath"></param>
        public static void XAFInsertCollection(string type, string associationName, string filePath)
        {
            if (File.Exists(filePath))
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("\t\t[Association(\"" + associationName + "\")");
                    writer.Write("\t\tpublic XPCollection<" + type + "> ");
                    writer.Write(type + "es { get => GetCollection<" + type +">(");
                    writer.WriteLine("nameof(" + type +"es)); }");
                    /*Close file*/
                    writer.Close();
                }
            }
        }
    }
}
