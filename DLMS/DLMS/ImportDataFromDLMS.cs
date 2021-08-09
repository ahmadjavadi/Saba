using DLMS.DTOEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Ionic.Zip;
using System.Reflection;

 namespace DLMS
{
    public class DLMS
    {
        public MeterData GetDataFromDLMSClient(string Path)
        {
            try
            {
                string[] arg = { Path };
                List<ObjectDTO> lstObjectDTO = new List<ObjectDTO>();
                lstObjectDTO = DeSerialize<List<ObjectDTO>>(Path, true);
                lstObjectDTO = changeObjectDTO(lstObjectDTO);
                return CreateNewObjectList(lstObjectDTO);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
            return null;
        }

        public T DeSerialize<T>(string filePath, bool fileIsZipped)
        {
            T objectOut = default(T);
            try
            {
                string file = string.Empty;
                if (fileIsZipped)
                {
                    ZipFile zip = ZipFile.Read(filePath);
                    zip.Entries.FirstOrDefault().ExtractWithPassword(Path.GetTempPath(), ExtractExistingFileAction.OverwriteSilently, "8401181392");
                    file = Path.Combine(Path.GetTempPath(), zip.Entries.FirstOrDefault().FileName);

                    if (!Path.GetExtension(file).Equals(Path.GetExtension(filePath)))
                        throw new ApplicationException("File extension is changed. File can't be openned.");
                }

                //
                if (string.IsNullOrEmpty(filePath)) { return default(T); }


                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileIsZipped ? file : filePath);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);
                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
                //


               
                if (fileIsZipped)
                    File.Delete(file);
            }
            catch (Exception)
            {
                // Propogate the exception.

            }
            return objectOut;
        }
        public string UnZip(string filePath)
        {
            string file = string.Empty;
            try
            {
                ZipFile zip = ZipFile.Read(filePath);
                zip.Entries.FirstOrDefault().ExtractWithPassword(System.AppDomain.CurrentDomain.BaseDirectory, ExtractExistingFileAction.OverwriteSilently, "8401181392");
                file = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, zip.Entries.FirstOrDefault().FileName);

                if (!Path.GetExtension(file).Equals(Path.GetExtension(filePath)))
                    throw new ApplicationException("File extension is changed. File can't be openned.");
                zip.Dispose();

            }
            catch (Exception ex)
            {

                throw;
            }
            return  file ;
        }
        public void DeleteFile(string file)
        {
            try
            {
                File.Delete(file);
            }
            catch (Exception)
            {

                throw;
            }
        }
        static void UnZip(string[] args)
        {
            //Console.ReadKey();
            if (args.Length == 0)
            {
                //Console.WriteLine("No File Specified to unzip");
                //Console.ReadKey();
            }
            else if (args.Length == 1)
            {
                try
                {
                    string filePath = args[0];
                    ZipFile zip = ZipFile.Read(filePath);
                    string newPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\", string.Empty);
                    //Console.WriteLine(newPath);
                    zip.Entries.FirstOrDefault().ExtractWithPassword(newPath, ExtractExistingFileAction.OverwriteSilently, "8401181392");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error : " + e.Message);
                    Console.ReadKey();
                }
            }
        }
        public T DeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);

            try
            {
                string attributeXml = string.Empty;

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);
                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message.ToString()); CommonData.WriteLOG(ex);
            }

            return objectOut;
        }
        public List<ObjectDTO> changeObjectDTO(List<ObjectDTO> lstObjectDTO)
        {
            int i = 0;
            int j = 0;
            int level = 1;
            try
            {
                for (i = 0; i < lstObjectDTO.Count; i++)
                {
                    ObjectDTO dto = new ObjectDTO();
                    dto = lstObjectDTO[i];
                    AttributeDTO[] atoATT = dto.Attributes;
                    for (j = 0; j < atoATT.Length; j++)
                    {
                        atoATT[j].AttributeID = j + 1;
                        DataDTO Data = atoATT[j].Data;

                        if (Data != null)
                        {
                            Data.Level = level;
                            Data.DataID = j + 1;
                            Data.Items = changeDataID(Data.Items, level, j + 1);
                        }
                    }

                    lstObjectDTO[i].Attributes = atoATT;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message + i.ToString() + " " + j.ToString());
            }
            return lstObjectDTO;
        }
        public DataDTO[] changeDataID(DataDTO[] Data, int level, int DataID)
        {
            try
            {
                level = level + 1;
                for (int k = 0; k < Data.Length; k++)
                {
                    DataDTO dataitem = Data[k];
                    dataitem.DataID = DataID;
                    dataitem.Level = level;
                    
                    if (dataitem.Items.Length > 0)
                    {

                        dataitem.Items = changeDataID(dataitem.Items, level, DataID);
                    }
                }
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message.ToString()); CommonData.WriteLOG(ex);
            }
            return Data;
        }
        public MeterData CreateNewObjectList(List<ObjectDTO> lstObjectDTO)
        {
            MeterData md = new MeterData();
            int i = 0;
            int j = 0;
            try
            {
                List<LstOBISDTO> lst = new List<LstOBISDTO>();
                for (i = 0; i < lstObjectDTO.Count; i++)
                {
                    ObjectDTO dto = new ObjectDTO();
                    dto = lstObjectDTO[i];
                    AttributeDTO[] atoATT = dto.Attributes;
                    for (j = 0; j < atoATT.Length; j++)
                    {
                        if (atoATT[j].Data != null)
                        {                             
                            LstOBISDTO obis = new LstOBISDTO();
                            obis.LogicalName = dto.Obis.LogicalName;
                            obis.ClaseID = dto.Obis.ClassId;
                            obis.AttributeID = atoATT[j].AttributeID;
                            obis.DataId = atoATT[j].Data.DataID;
                            obis.TypeString = atoATT[j].Data.TypeString;
                            obis.ValueString = atoATT[j].Data.ValueString;
                            obis.Level = atoATT[j].Data.Level;
                            lst.Add(obis);
                            AddDataItemToList(lst, atoATT[j].Data.Items, dto.Obis.LogicalName, dto.Obis.ClassId, atoATT[j].AttributeID);
                        }
                    }


                }
                md.MeterObjects = lst;
                //SaveToXmlFile(lst);
                //string path = System.AppDomain.CurrentDomain.BaseDirectory;
               // string[] args = { path + @"\data.xml" };

            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message + i.ToString() + " " + j.ToString());
            }
            return md;
        }
        public void AddDataItemToList(List<LstOBISDTO> lst, DataDTO[] Data, string LogicalName, int ClassId, int AttributeID)
        {
            try
            {
                for (int k = 0; k < Data.Length; k++)
                {
                    DataDTO dataitem = Data[k];
                    LstOBISDTO obis = new LstOBISDTO();
                    obis.LogicalName = LogicalName;
                    obis.ClaseID = ClassId;
                    obis.AttributeID = AttributeID;
                    obis.DataId = Data[k].DataID;
                    obis.TypeString = Data[k].TypeString;
                    obis.ValueString = Data[k].ValueString;
                    obis.Level = Data[k].Level;
                    lst.Add(obis);
                    if (dataitem.Items.Length > 0)
                        AddDataItemToList(lst, dataitem.Items, LogicalName, ClassId, AttributeID);
                }
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message.ToString()); CommonData.WriteLOG(ex);
            }
        }
        public void SaveToXmlFile(List<LstOBISDTO> lst)
        {
            try
            {
                var stringwriter = new System.IO.StringWriter();
                XmlSerializer myserializer = new XmlSerializer(lst.GetType());
                myserializer.Serialize(stringwriter, lst);
                string xmlMessage = stringwriter.ToString();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlMessage);
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                // Save the document to a file and auto-indent the output.
                XmlWriter writer = XmlWriter.Create("data.xml", settings);
                doc.Save(writer);
                writer.Close();
                
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString()); CommonData.WriteLOG(ex);
            }
        }
        static void Zip(string[] args)
        {
            //if (args.Length == 0)
            //{
            //    Console.WriteLine("No File Specified to zip");
            //    Console.ReadKey();
            //}
            if (args.Length == 1)
            {
                try
                {
                    string filePath = args[0];
                    ZipFile zip = new ZipFile();
                    zip.Password = "8401181392";
                    zip.AddFile(filePath, string.Empty);
                    string newPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\", string.Empty), Path.GetFileName(filePath));
                    //Console.WriteLine(newPath);
                    zip.Save(newPath);
                }
                catch (Exception e)
                {
                    //Console.WriteLine("Error : " + e.Message);
                    //Console.ReadKey();
                }

            }
        }
       
    }
}
