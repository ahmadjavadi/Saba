using System;
using System.IO;
using System.Collections.Generic;
using PortableDeviceApiLib;
using PortableDeviceTypesLib;
using _tagpropertykey = PortableDeviceApiLib._tagpropertykey;
using IPortableDeviceKeyCollection = PortableDeviceApiLib.IPortableDeviceKeyCollection;
using IPortableDeviceValues = PortableDeviceApiLib.IPortableDeviceValues;

namespace PortableDevices
{
    internal class PortableDevice
    {
        #region Fields

        private bool _isConnected;
        private readonly PortableDeviceClass _device;     
        
        #endregion

        #region ctor(s)

        public PortableDevice(string deviceId)
        {
            this._device = new PortableDeviceClass();
            this.DeviceId = deviceId;
        }

        #endregion

        #region Properties

        public string DeviceId { get; set; }

        public string FriendlyName
        {
            get
            {
                if (!this._isConnected)
                {
                    throw new InvalidOperationException("Not connected to device.");
                }

                // Retrieve the properties of the device
                IPortableDeviceContent content;
                IPortableDeviceProperties properties;
                this._device.Content(out content);
                content.Properties(out properties);

                // Retrieve the values for the properties
                IPortableDeviceValues propertyValues;
                properties.GetValues("DEVICE", null, out propertyValues);

                // Identify the property to retrieve
                var property = new _tagpropertykey();
                property.fmtid = new Guid(0x26D4979A, 0xE643, 0x4626, 0x9E, 0x2B,
                                          0x73, 0x6D, 0xC0, 0xC9, 0x2F, 0xDC);
                property.pid = 12;

                // Retrieve the friendly name
                string propertyValue;
                propertyValues.GetStringValue(ref property, out propertyValue);

                return propertyValue;
            }
        }

        internal PortableDeviceClass PortableDeviceClass 
        { 
            get 
            { 
                return this._device; 
            } 
        }

        #endregion

        #region Methods

        public void Connect()
        {
            if (this._isConnected) { return; }

            var clientInfo = (IPortableDeviceValues) new PortableDeviceValuesClass();
            this._device.Open(this.DeviceId, clientInfo);
            this._isConnected = true;
        }

        public void Disconnect()
        {
            if (!this._isConnected) { return; }
            this._device.Close();
            this._isConnected = false;
        }

        public PortableDeviceFolder GetContents()
        {
            var root = new PortableDeviceFolder("DEVICE", "DEVICE");

            IPortableDeviceContent content;
            this._device.Content(out content);

            EnumerateContents(ref content, root);

            return root;
        }

        private static void EnumerateContents(ref IPortableDeviceContent content,
            PortableDeviceFolder parent)
        {

            // Get the properties of the object
            IPortableDeviceProperties properties;
            content.Properties(out properties);

            // Enumerate the items contained by the current object
            IEnumPortableDeviceObjectIDs objectIds;
            content.EnumObjects(0, parent.Id, null, out objectIds);

            uint fetched = 0;
            do
            {
                string objectId;
                objectIds.Next(1, out objectId, ref fetched);
                if (fetched > 0    //&& !objectId.Contains("Flash Disk")                 
                            && !objectId.Contains("Window") && !objectId.Contains("Temp") && !objectId.Contains(".exe")
                    && !objectId.Contains("Connections") && !objectId.Contains("other") && !objectId.Contains(".dll")
                    && !objectId.Contains(".DAT")           
                    )
                {
                    var currentObject = WrapObject(properties, objectId);
                    //if (objectId.Contains("DEVICE") || objectId.Contains("f|S|\\") || objectId.Contains("Program Files")
                    //    || objectId.Contains("HHU") || objectId.Contains("Readout") || objectId.ToLower().Contains("old") || objectId.ToLower().Contains("new"))
                        
                    {
                        parent.Files.Add(currentObject);
                        if (currentObject is PortableDeviceFolder)

                            //if (parent.Id.Contains("DEVICE") || parent.Id.Contains("f|S|\\") || parent.Id.Contains("Program Files") || parent.Id.Contains("HHU") || parent.Id.Contains("Readout"))
                                EnumerateContents(ref content, (PortableDeviceFolder)currentObject);
                    }
                }
            } while (fetched > 0);
        }

        public void DownloadFile(PortableDeviceFile file, string saveToPath)
        {
            IPortableDeviceContent content;
            this._device.Content(out content);

            IPortableDeviceResources resources;
            content.Transfer(out resources);

            PortableDeviceApiLib.IStream wpdStream;
            uint optimalTransferSize = 0;

            var property = new _tagpropertykey();
            property.fmtid = new Guid(0xE81E79BE, 0x34F0, 0x41BF, 0xB5, 0x3F, 0xF1, 0xA0, 0x6A, 0xE8, 0x78, 0x42);
            property.pid = 0;
            try
            {

                resources.GetStream(file.Id, ref property, 0, ref optimalTransferSize, out wpdStream);

                System.Runtime.InteropServices.ComTypes.IStream sourceStream = (System.Runtime.InteropServices.ComTypes.IStream)wpdStream;

                var filename = Path.GetFileName(file.Id.Substring(file.Id.LastIndexOf("\\") + 1, file.Id.Length - file.Id.LastIndexOf("\\") - 1));
               // FileStream targetStream = new FileStream(Path.Combine(saveToPath, filename), FileMode.Create, FileAccess.Write);

                StreamWriter ser;
                unsafe
                {
                    var buffer = new byte[optimalTransferSize];
                    int bytesRead;
                    string obis ="";
                    do
                    {
                        sourceStream.Read(buffer, (int)optimalTransferSize, new IntPtr(&bytesRead));
                        if (System.IO.File.Exists(Path.Combine(saveToPath, filename)))
                            return;
                        ser = System.IO.File.AppendText(Path.Combine(saveToPath, filename));
                        //string str = System.Text.Encoding.Default.GetString(buffer).TrimStart();
                        string str = System.Text.Encoding.Default.GetString(buffer);
                        try
                        {

                            string[] data = str.Split('\n');

                            foreach (var item in data)
                            {
                                obis = item;
                                if (obis.Length > 7)
                                    if (!obis.Substring(5, 1).Equals(" "))
                                        obis = obis.Insert(5, " "); 

                                obis = obis.Replace("\r", "");
                                if (obis.Length > 1)
                                    if (obis[1].Equals('.') && !obis.Contains("0.0.0"))
                                        obis = "\r\n" + obis;

                                if (!obis.StartsWith("\0"))
                                    ser.Write(obis);
                                if (obis.Contains("P.0.1"))
                                    break;
                            }
                            ser.Close();

                            if (obis.Contains("P.0.1"))
                                return;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        //targetStream.Write(buffer, 0, 65536);
                    } while (bytesRead > 0);

                  //  targetStream.Close();                   
                }
            }
            catch (Exception)
            {
            }
        }

        private static PortableDeviceObject WrapObject(IPortableDeviceProperties properties, 
            string objectId)
        {            
            IPortableDeviceKeyCollection keys;
            properties.GetSupportedProperties(objectId, out keys);

            IPortableDeviceValues values;
            properties.GetValues(objectId, keys, out values);

            // Get the name of the object
            string name;
            var property = new _tagpropertykey();
            property.fmtid = new Guid(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC,
                                      0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C);
            property.pid = 4;
            values.GetStringValue(property, out name);

            // Get the type of the object
            Guid contentType;
            property = new _tagpropertykey();
            property.fmtid = new Guid(0xEF6B490D, 0x5CD8, 0x437A, 0xAF, 0xFC,
                                      0xDA, 0x8B, 0x60, 0xEE, 0x4A, 0x3C);
            property.pid = 7;
            values.GetGuidValue(property, out contentType);

            var folderType = new Guid(0x27E2E392, 0xA111, 0x48E0, 0xAB, 0x0C,
                                      0xE1, 0x77, 0x05, 0xA0, 0x5F, 0x85);
            var functionalType = new Guid(0x99ED0160, 0x17FF, 0x4C44, 0x9D, 0x98,
                                          0x1D, 0x7A, 0x6F, 0x94, 0x19, 0x21);

            if (contentType == folderType  || contentType == functionalType)
            {
                return new PortableDeviceFolder(objectId, name);
            }          

            return new PortableDeviceFile(objectId, name);
        }

        #endregion
    }
}