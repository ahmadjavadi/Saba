using System;
using System.Linq;
using System.Devices;
using System.IO;
using System.Security.Permissions;
using System.Threading;
using RsaDateTime;

namespace HandHeldReader
{
    public class HandHeldReader
    {
        public static bool FindHhuReadOut = false;

        public static HHUReadOut PortableDeviceReader()
        {
            //RemoteDeviceManager r = new RemoteDeviceManager();
            //r.UnsafeThreadDeviceConnectedNotice += r_DeviceConnected;
            //r.UnsafeThreadDeviceDisconnectedNotice += r_DeviceDisconnected;
             
            //r.UnsafeThreadDeviceConnectedNotice -= r_DeviceConnected;
            //r.UnsafeThreadDeviceDisconnectedNotice -= r_DeviceDisconnected;
            return ReadFromHhu();
        }

        /// <summary>
        /// ErrorCode = 0 success
        /// ErrorCode = -1 cannot connect to HHU
        /// ErrorCode = -2 cannot find HHUReadOut folder
        /// ErrorCode = -3 catch exception
        /// ErrorCode = -4 Cannot Move files from new to old
        /// ErrorCode = -5 not find any readout data in HHU
        /// ErrorCode = -6 cannot create HHUreadout folder
        /// </summary>
        /// <returns></returns>
        private static HHUReadOut ReadFromHhu()
        {
            HHUReadOut hhureadout = new HHUReadOut { ErrorCode = "0" };
            try
            {
                RemoteDeviceManager r = new RemoteDeviceManager();
                RemoteDevice dev = r.Devices.FirstConnectedDevice;
                if (dev == null)
                {
                    hhureadout.ErrorCode = "-1";
                    return hhureadout;
                }

                RemoteFileInfo fi = new RemoteFileInfo(dev, @"\Program Files\HHU\Readout\new");
                RemoteFileInfo oldDir = new RemoteFileInfo(dev, @"\Program Files\HHU\Readout\old");
                if (fi.Exists)
                {
                    if (!oldDir.Exists)
                        RemoteDirectory.CreateDirectory(dev, @"\Program Files\HHU\Readout\old");

                    string[] readOutsDataName = RemoteDirectory.GetFiles(dev, @"\Program Files\HHU\Readout\new");

                    FindHhuReadOut = readOutsDataName.Length>0;
                    
                    if (readOutsDataName.Length > 0)
                    {
                        if (!System.IO.Directory.Exists(System.Environment.CurrentDirectory + "\\HHUReadOut"))
                        {
                            try
                            {
                                var di = System.IO.Directory.CreateDirectory(System.Environment.CurrentDirectory + "\\HHUReadOut");
                            }
                            catch 
                            {
                                hhureadout.ErrorCode = "-6";
                            }
                            
                        }

                        foreach (var item in readOutsDataName)
                        {
                            RemoteFile.CopyFileFromDevice(dev, @"\Program Files\HHU\Readout\New\" + item,
                            System.Environment.CurrentDirectory + "\\HHUReadOut" + "\\" + item, true);
                        }
                        try
                        {
                            RemoteDirectory.Move(dev, @"\Program Files\HHU\Readout\new",
                                @"\Program Files\HHU\Readout\old\" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss"));
                            RemoteDirectory.CreateDirectory(dev, @"\Program Files\HHU\Readout\new");
                        }
                        catch
                        {
                            hhureadout.ErrorCode = "-4";
                        }
                        return ConvertReadSavedFile(hhureadout);
                    }
                    else
                    {
                        hhureadout.ErrorCode = "-5";
                    }
                }
                else
                {
                    hhureadout.ErrorCode = "-2";
                }
            }
            catch(Exception ex)
            {
                hhureadout.ErrorCode = "-3";
                // ignored
            }
            return hhureadout;
        }

        public static HHUReadOut ReadFromHhuFile(string directoryAddress)
        {
            HHUReadOut hhureadout = new HHUReadOut { ErrorCode = "0" };
            try
            {

                foreach (var fileName in System.IO.Directory.GetFiles(directoryAddress))
                {
                    try
                    {
                        // int hh=0,mm=0,ss=0,year=1,month=1,day=1;
                        ReadOut ro = new ReadOut();

                        ro.OBISObjectList = new OBISObjectsList();
                        ro.OBISObjectList.OBISObjectList = new System.Collections.Generic.List<OBISObject>();
                        #region foreach
                        foreach (var item in System.IO.File.ReadAllLines(fileName))
                        {
                            OBISObject oo = new OBISObject();
                            try
                            {
                                
                                if (item.Length > 5)
                                {
                                    OBISObject oBISObject = OBISMapping.getStandardOBIS(item.Substring(0, 5), (item.Substring(5).TrimStart().Split(' '))[0]);

                                    if (oBISObject != null)
                                    {
                                        
                                        if (item.Substring(0, 5).Equals("0.0.0"))
                                        {
                                            ro.SerialNumber = oBISObject.value;
                                            ro.OBISObjectList.OBISObjectList.Add(oBISObject);
                                        }
                                        
                                        else if (item.Substring(0, 5).Equals("0.0.5"))
                                        {
                                            OBISObject OB1 = new OBISObject();
                                            OB1.code = oBISObject.code;
                                            string[] CT = oBISObject.value.Split('/');
                                            oBISObject.value = CT[0];
                                            ro.OBISObjectList.OBISObjectList.Add(oBISObject);
                                            OB1.code = "0100000405FF";
                                            OB1.value = CT[1];
                                            ro.OBISObjectList.OBISObjectList.Add(OB1);

                                        }
                                        else if (item.Substring(0, 5).Equals("0.0.6"))
                                        {
                                            if (item.Contains(":"))
                                                ro.ReadOutDateTime = oBISObject.value;

                                        }

                                        else if (item.Substring(0, 5).Equals("A.9.3"))
                                        {
                                            ro.OBISObjectList.OBISObjectList.Add(oBISObject);
                                            ro.SoftwareVersion = oBISObject.value;
                                        }

                                        else if (item.Substring(0, 5).Equals("0.0.7"))
                                        {
                                            try
                                            {
                                                int year = 1300, day = 0, month = 0;
                                                int HH = 0, MM = 0, SS = 0;

                                                int.TryParse(item.Substring(6, 2), out year);
                                                int.TryParse(item.Substring(9, 2), out month);
                                                int.TryParse(item.Substring(12, 2), out day);

                                                int.TryParse(ro.ReadOutDateTime.Substring(0, 2), out HH);
                                                int.TryParse(ro.ReadOutDateTime.Substring(3, 2), out MM);
                                                int.TryParse(ro.ReadOutDateTime.Substring(6, 2), out SS);

                                                if ((HH < 0 || HH > 23 || MM < 0 || MM > 59 || SS < 0 || SS > 59))
                                                {
                                                    HH = 0;
                                                    MM = 0;
                                                    SS = 0;
                                                }
                                                if (year > 50)
                                                    year = 1300 + year;
                                                else
                                                    year = 1400 + year;

                                                if (year > 1450 || year < 1350 || month < 1 || month > 12 || day < 1 || day > 31)
                                                {
                                                    ro.ReadOutDateTime = new DateTime(1921,3,21,0,0,0).ToString();
                                                    oBISObject.value = ro.ReadOutDateTime;
                                                }
                                                else
                                                {

                                                    PersianDate pd = new PersianDate(year, month, day, HH, MM, SS);
                                                    ro.ReadOutDateTime = pd.ConvertToGeorgianDateTime().ToString("yyyy/MM/dd HH:mm:ss");
                                                    oBISObject.value = ro.ReadOutDateTime;
                                                }
                                            }
                                            catch
                                            {
                                                ro.ReadOutDateTime = new DateTime(1921,3,21,0,0,0).ToString();
                                                oBISObject.value = ro.ReadOutDateTime;

                                            }



                                            oBISObject.value = oBISObject.value.Replace("ب.ظ", "").Replace("ق.ظ", "").Replace("AM", "").Replace("PM", "").TrimEnd();
                                            oBISObject.code = oBISObject.code.Replace("_0.0.7", "");
                                            ro.OBISObjectList.OBISObjectList.Add(oBISObject);

                                        }
                                        else
                                            ro.OBISObjectList.OBISObjectList.Add(oBISObject);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("====================== 2 22 ==============================\r\nSave Object:\r\n" + fileName.Substring(fileName.LastIndexOf("\\") + 1).Replace(".txt", "").Substring(0, 8) + "  " + item + " " + ex.Message);
                            }

                        }
                        #endregion foreach
                        if (hhureadout.ReadOutList == null)
                            hhureadout.ReadOutList = new System.Collections.Generic.List<ReadOut>();
                        if (ro.ReadOutDateTime != new DateTime(1921,3,21,0,0,0).ToString() && ro.SerialNumber.StartsWith("207"))
                            hhureadout.ReadOutList.Add(ro);
                    }
                    catch (Exception ex)
                    {
                        if (fileName.Length > fileName.LastIndexOf("\\", StringComparison.Ordinal) + 1)
                            Console.WriteLine("============================= 111  =======================\r\nSave Object:\r\n" +
                                fileName.Substring(fileName.LastIndexOf("\\", StringComparison.Ordinal) + 1).Replace(".txt", "").Substring(0, 8) + " " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                hhureadout.ErrorCode = "-3";
                // ignored
            }
            return hhureadout;
        }

        private static HHUReadOut ConvertReadSavedFile(HHUReadOut hhureadout)
        {
            if (!System.IO.Directory.Exists(System.Environment.CurrentDirectory + "\\HHUReadOut"))
            {
                hhureadout.ErrorCode = "-2";
                return hhureadout;
            }
            foreach (var fileName in System.IO.Directory.GetFiles(System.Environment.CurrentDirectory + "\\HHUReadOut", "*.txt"))
            {
                try
                {
                    // int hh=0,mm=0,ss=0,year=1,month=1,day=1;
                    ReadOut ro = new ReadOut();

                    ro.OBISObjectList = new OBISObjectsList();
                    ro.OBISObjectList.OBISObjectList = new System.Collections.Generic.List<OBISObject>();
                    #region foreach
                    foreach (var item in System.IO.File.ReadAllLines(fileName))
                    {                       
                        OBISObject oo = new OBISObject();
                        try
                        {
                            if (item.Length > 5)
                            {
                                OBISObject oBISObject = OBISMapping.getStandardOBIS(item.Substring(0, 5), (item.Substring(5).TrimStart().Split(' '))[0]);

                                if (oBISObject != null)
                                {
                                    if (item.Substring(0, 5).Equals("0.0.0"))
                                    {
                                        ro.SerialNumber = oBISObject.value;
                                        ro.OBISObjectList.OBISObjectList.Add(oBISObject);
                                    }
                                    else if (item.Substring(0, 5).Equals("0.0.5"))
                                    {
                                        OBISObject OB1 = new OBISObject();
                                        OB1.code = oBISObject.code;
                                        string[] CT = oBISObject.value.Split('/');
                                        oBISObject.value = CT[0];
                                        ro.OBISObjectList.OBISObjectList.Add(oBISObject);
                                        OB1.code = "0100000405FF";
                                        OB1.value = CT[1];
                                        ro.OBISObjectList.OBISObjectList.Add(OB1);

                                    }
                                    else if (item.Substring(0, 5).Equals("0.0.6"))
                                    {
                                        if (item.Contains(":"))
                                            ro.ReadOutDateTime = oBISObject.value;
                                    
                                    }

                                    else if (item.Substring(0, 5).Equals("A.9.3"))
                                    {
                                        ro.SoftwareVersion = oBISObject.value;
                                    }

                                    else if (item.Substring(0, 5).Equals("0.0.7"))
                                    {
                                        try
                                        {
                                            int year = 1300, day = 0, month = 0;
                                            int HH = 0, MM = 0, SS = 0;

                                            int.TryParse(item.Substring(6, 2), out year);
                                            int.TryParse(item.Substring(9, 2), out month);
                                            int.TryParse(item.Substring(12, 2), out day);

                                            int.TryParse(ro.ReadOutDateTime.Substring(0, 2), out HH);
                                            int.TryParse(ro.ReadOutDateTime.Substring(3, 2), out MM);
                                            int.TryParse(ro.ReadOutDateTime.Substring(6, 2), out SS);

                                            if ((HH < 0 || HH > 23 || MM < 0 || MM > 59 || SS < 0 || SS > 59))
                                            {
                                                HH = 0;
                                                MM = 0;
                                                SS = 0;
                                            }
                                            if (year > 50)
                                                year = 1300 + year;
                                            else
                                                year = 1400 + year;

                                            if (year > 1450 || year < 1350 || month < 1 || month > 12 || day < 1 || day > 31)
                                            {
                                                ro.ReadOutDateTime = new DateTime(1921,3,21,0,0,0).ToString();
                                                oBISObject.value = ro.ReadOutDateTime;
                                            }
                                            else
                                            {

                                                PersianDate pd = new PersianDate(year, month, day, HH, MM, SS);
                                                ro.ReadOutDateTime = pd.ConvertToGeorgianDateTime().ToString("yyyy/MM/dd HH:mm:ss");
                                                oBISObject.value = ro.ReadOutDateTime;
                                            }
                                        }
                                        catch
                                        {
                                            ro.ReadOutDateTime = new DateTime(1921,3,21,0,0,0).ToString();
                                            oBISObject.value = ro.ReadOutDateTime;

                                        }
                                       


                                        oBISObject.value = oBISObject.value.Replace("ب.ظ", "").Replace("ق.ظ", "").Replace("AM", "").Replace("PM", "").TrimEnd();
                                        oBISObject.code = oBISObject.code.Replace("_0.0.7", "");
                                        ro.OBISObjectList.OBISObjectList.Add(oBISObject);

                                        }
                                    else
                                        ro.OBISObjectList.OBISObjectList.Add(oBISObject);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("====================== 2 22 ==============================\r\nSave Object:\r\n" + fileName.Substring(fileName.LastIndexOf("\\") + 1).Replace(".txt", "").Substring(0, 8) + "  " + item + " " + ex.Message);
                        }

                    }
                    #endregion foreach
                    if (hhureadout.ReadOutList == null)
                        hhureadout.ReadOutList = new System.Collections.Generic.List<ReadOut>();
                    if (ro.ReadOutDateTime != new DateTime(1921,3,21,0,0,0).ToString() && ro.SerialNumber.StartsWith("207"))
                        hhureadout.ReadOutList.Add(ro);
                }
                catch (Exception ex)
                {
                    if (fileName.Length > fileName.LastIndexOf("\\", StringComparison.Ordinal) + 1)
                            Console.WriteLine("============================= 111  =======================\r\nSave Object:\r\n" + 
                                fileName.Substring(fileName.LastIndexOf("\\", StringComparison.Ordinal) + 1).Replace(".txt", "").Substring(0, 8) + " " + ex.Message);
                }
            }

            if (System.IO.Directory.Exists(System.Environment.CurrentDirectory + "\\HHUReadOut"))
            {
                System.IO.Directory.Delete(System.Environment.CurrentDirectory + "\\HHUReadOut", true);
            }
            if (hhureadout.ReadOutList != null)
                if (hhureadout.ReadOutList.Count > 0)
                    if (hhureadout.ReadOutList[0].SerialNumber.Length > 7)
                        hhureadout.ErrorCode = "0";
            return hhureadout;
        }
    }
}
