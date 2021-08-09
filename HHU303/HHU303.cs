using HHU303.DTOEntities;
using HHU303.HHU;
using System;

namespace HHU303
{
    public class HHU303
    {      
        static bool findHHUReadOut = false;
        static string folderName = System.Environment.CurrentDirectory + "\\PortableDevice";
        static HHUReadout hhuData = null;
        static string fileName = "";


        public static HHUReadout PortableDeviceReader()
        {
            try
            {
                ReadFromHHU303();
                if (string.IsNullOrEmpty(hhuData.ErrorMessage))
                {
                    hhuData = new HHUReadout();
                    foreach (var item in System.IO.Directory.GetDirectories(folderName +"\\ReadOut"))
                    {
                        MeterReadout mro = null;
                        if (!item.Substring(item.LastIndexOf(@"\")).Replace(@"\", "").StartsWith("19"))
                            continue;
                        foreach (var meterdata in System.IO.Directory.GetFiles(item))
                        {
                            DLMS.DLMS ds = new DLMS.DLMS();

                            DLMS.DTOEntities.MeterData rr = ds.GetDataFromDLMSClient(meterdata);
                            if (rr != null)
                            {
                                if (mro == null)
                                {
                                    mro = new MeterReadout();
                                    mro.meterNo = item.Substring(item.LastIndexOf(@"\")).Replace(@"\", "");

                                }
                                var x = meterdata.Substring(meterdata.LastIndexOf(@"\")).Replace(@"\", "").Replace(".odr", "").Split(new char[] { ' ', '-' });
                                rr.ReadDate = string.Format("{0}/{1}/{2} {3}:{4}:00", x[0], x[1], x[2], x[3], x[4]);
                                mro.readoutList.Add(rr);
                            }
                        }
                        if (mro != null)
                            hhuData.hhureadout.Add(mro);
                    }
                    if (System.IO.Directory.Exists(folderName))
                    {
                        try
                        {
                            System.IO.Directory.Delete(folderName, true);
                        }
                        catch (Exception)
                        {

                        }
                    }

                    return hhuData; 
                }
            }
            catch { }
            return hhuData;
        }
        public static HHUReadout ReadHHuFile(string fileAddrss)
        {
            hhuData = new HHUReadout();
            MeterReadout mro = null;
            foreach (var item in System.IO.Directory.GetDirectories(fileAddrss))
            {
                mro = null;
                if (item.Contains("ReadOut"))
                {
                    foreach (var item1 in System.IO.Directory.GetDirectories(item))
                    {
                        if (!item1.Contains("19"))
                            continue;
                        foreach (var meterdata in System.IO.Directory.GetFiles(item1))
                        {
                            DLMS.DLMS ds = new DLMS.DLMS();

                            DLMS.DTOEntities.MeterData rr = ds.GetDataFromDLMSClient(meterdata);
                            if (rr != null)
                            {
                                if (mro == null)
                                {
                                    mro = new MeterReadout();
                                    mro.meterNo = item.Substring(item.LastIndexOf(@"\")).Replace(@"\", "");

                                }
                                var x = meterdata.Substring(meterdata.LastIndexOf(@"\")).Replace(@"\", "").Replace(".odr", "").Split(new char[] { ' ', '-' });
                                rr.ReadDate = string.Format("{0}/{1}/{2} {3}:{4}:00", x[0], x[1], x[2], x[3], x[4]);
                                mro.readoutList.Add(rr);
                            }
                        }
                    }
                }
                if (!item.Substring(item.LastIndexOf(@"\")).Replace(@"\", "").StartsWith("19"))
                    continue;
                foreach (var meterdata in System.IO.Directory.GetFiles(item))
                {
                    DLMS.DLMS ds = new DLMS.DLMS();

                    DLMS.DTOEntities.MeterData rr = ds.GetDataFromDLMSClient(meterdata);
                    if (rr != null)
                    {
                        if (mro == null)
                        {
                            mro = new MeterReadout();
                            mro.meterNo = item.Substring(item.LastIndexOf(@"\")).Replace(@"\", "");
                            
                        }
                        var x = meterdata.Substring(meterdata.LastIndexOf(@"\")).Replace(@"\", "").Replace(".odr", "").Split(new char[] { ' ', '-' });
                        rr.ReadDate = string.Format("{0}/{1}/{2} {3}:{4}:00", x[0], x[1], x[2], x[3], x[4]);
                        mro.readoutList.Add(rr);
                    }
                }
                if (mro != null)
                    hhuData.hhureadout.Add(mro);
            }
            bool hasFile = false;
            mro = null;
            foreach (var meterdata in System.IO.Directory.GetFiles(fileAddrss))
            {
                if (!meterdata.Contains(".odr"))
                    continue;

                DLMS.DLMS ds = new DLMS.DLMS();

                DLMS.DTOEntities.MeterData rr = ds.GetDataFromDLMSClient(meterdata);
                if (rr != null)
                {
                    if (mro == null)
                    {
                        mro = new MeterReadout();
                        mro.meterNo = fileAddrss.Substring(fileAddrss.LastIndexOf(@"\")).Replace(@"\", "");

                    }
                    var x = meterdata.Substring(meterdata.LastIndexOf(@"\")).Replace(@"\", "").Replace(".odr", "").Split(new char[] { ' ', '-' });
                    rr.ReadDate = string.Format("{0}/{1}/{2} {3}:{4}:00", x[0], x[1], x[2], x[3], x[4]);
                    mro.readoutList.Add(rr);
                    hasFile = true;
                }
            }
            if(hasFile) hhuData.hhureadout.Add(mro);
            return hhuData;
        }
        private static void ReadFromHHU303()
        {
            try
            {
                Boolean end_of_proc = true;
                string pcPath = folderName;
                hhuData = new HHUReadout();
                 
                HHUManager sHHU = new HHUManager();
                int step = 1;
                while (end_of_proc)
                {
                    switch (step)
                    {
                        case 0:
                            step = sHHU.CheckForActiveSync() ? 1 : 301;
                            System.Threading.Thread.Sleep(1200);
                            break;
                        case 1:
                            step = sHHU.CheckForHHUConnection() ? 2 : 302;
                            System.Threading.Thread.Sleep(600);
                            if (step > 10)
                            {                                
                                hhuData.ErrorMessage = "-1";
                            }
                            break;
                        case 2:
                            step = sHHU.DirectoryExistOnHHU(@"\Program Files") ? 3 : 308;
                            if (step > 10)
                                hhuData.ErrorMessage = "-2";
                            break;
                        case 3:
                            step = sHHU.DirectoryExistOnHHU(@"\Program Files\RSA-HHUBasic\") ? 4 : 303;
                            if (step > 10)
                                hhuData.ErrorMessage = "-2";
                            break;

                        #region Read Text File
                        case 4:
                            step = sHHU.DirectoryExistOnHHU(@"\Program Files\RSA-HHUBasic\Meter Readings") ? 5 : 304;
                            if (step > 10)
                                hhuData.ErrorMessage = "-2";
                            break;
                        case 5:
                          
                            string[] directories = sHHU.GetDirectories(@"\Program Files\RSA-HHUBasic\Meter Readings");

                            if (directories == null || directories.Length<1)
                                hhuData.ErrorMessage = "-2";
                            foreach (var item in directories)
                            {
                                if (item != null)
                                    if (item != "")
                                        if (sHHU.CopyDirFromHHUToPC(@"\Program Files\RSA-HHUBasic\Meter Readings\" + item, folderName + "\\ReadOut\\" + item) == 6)
                                            step = 6;
                            }
                            
                            //step = sHHU.CopyDirFromHHUToPC(@"\Program Files\RSA-HHUBasic\Meter Readings", folderName + "\\ReadOut\\");
                            //if (step != 6)
                            //    hhureadout.ErrorCode = step.ToString();
                            break;
                        case 6:
                            findHHUReadOut = true;
                            end_of_proc = false;
                            sHHU.HHUDisConnect();
                            
                           // step = sHHU.MoveDirOnHHU(@"\Program Files\HHU\ReadOut\New", @"\Program Files\HHU\ReadOut\Old");
                            //if (step != 7)
                            //    hhureadout.ErrorCode = step.ToString();
                            break;

                        #endregion

                        case 7:
                            end_of_proc = false;
                            sHHU.HHUDisConnect();
                            hhuData.ErrorMessage = ErrorHandle.showError(901, 2);
                            break;
                        default:
                            end_of_proc = false;
                            sHHU.HHUDisConnect();
                            //hhuData.errorMessage = ErrorHandle.showError(step, 2);
                            break;
                    }
                }
            }
            catch
            {
                hhuData.ErrorMessage = "-2";

            }
        }
    }
}
