using OpenNETCF.Desktop.Communication;
using System;
using System.IO;

namespace HHU303.HHU
{
    class HHUManager
    {
        public RAPI m_rapi;
        public HHUManager()
        {
            m_rapi = new RAPI();
        }
        public Boolean CheckForActiveSync()
        {
            return m_rapi.DevicePresent;    //  Indicates whether ActiveSync currently has a connected device or not
        }
        public Boolean CheckForHHUConnection()
        {
            try
            {
                m_rapi.Connect();    //  Connect to the device.
                return m_rapi.Connected;
            }
            catch
            {
                ErrorHandle.showError(301, 0);
                return false;
            }
        }
        public Boolean HHUDisConnect()
        {
            try
            {
                m_rapi.Disconnect();    //  DisConnect from the device.
            }
            catch
            {
                // ErrorHandle.showError("HHU Error 301", 301, 0);
                return false;
            }
            return true;
        }
        public Boolean CopyFileFromPCToHHU(string pcPath, string HHUPath)
        {
            HHUPath += "\\" + GetFileName(pcPath);
            return m_rapi.CopyFileToDevice(pcPath, HHUPath, true);
        }

        public Boolean CopyFileFromHHUToPC(string pcPath, string HHUPath)
        {
            try
            {
                pcPath += "\\" + GetFileName(HHUPath);
                m_rapi.CopyFileFromDevice(pcPath, HHUPath, true);
            }
            catch
            {
                ErrorHandle.showError(302, 0);
                return false;
            }
            return true;
        }
        public int CopyDirFromHHUToPC(string Src, string Dst)
        {
            try
            {

                if (Dst[Dst.Length - 1] != Path.DirectorySeparatorChar) Dst += Path.DirectorySeparatorChar;
                if (!Directory.Exists(Dst)) Directory.CreateDirectory(Dst);
                FileList FileList_list;
                FileList_list = m_rapi.EnumFiles(Src + "\\*.*");
                if (FileList_list == null || FileList_list.Count < 1)
                {
                    ErrorHandle.showError(311, 2);
                    return -5;
                }
                foreach (FileInformation file in FileList_list)
                {
                    try
                    {
                        m_rapi.CopyFileFromDevice(Dst + file.FileName, (Src + "\\" + file.FileName), true);
                    }
                    catch //(Exception e)
                    {
                        //ErrorHandle.showError(302, 2);
                        return -1;
                    }
                }
            }
            catch //(Exception e)
            {
                //ErrorHandle.showError(302, 2);
                return -1;
            }
            return 6;
        }

        public string[] GetDirectories(string Src)
        {
            try
            {               
                FileList FileList_list;
                FileList_list = m_rapi.EnumFiles(Src + "\\*.*");
                if (FileList_list == null || FileList_list.Count < 1)
                {
                    ErrorHandle.showError(311, 2);
                    return null;
                }
                string[] dir = new string[FileList_list.Count];
                int k=0;
                foreach (FileInformation file in FileList_list)
                {
                    if (file.FileAttributes == 16)
                    {
                        dir[k] = file.FileName;
                        k++;
                    }
                }

                return dir;
            }
            catch //(Exception e)
            {
                //ErrorHandle.showError(302, 2);
                return null;
            }
            return null;
        }

        public int MoveDirOnHHU(string Src, string Dst)
        {
            try
            {
                if (Dst[Dst.Length - 1] != Path.DirectorySeparatorChar) Dst += Path.DirectorySeparatorChar;
                if (!Directory.Exists(Dst)) Directory.CreateDirectory(Dst);
                FileList FileList_list;
                FileList_list = m_rapi.EnumFiles(Src + "\\*.*");
                if (FileList_list != null && FileList_list.Count > 0)
                {
                    foreach (FileInformation file in FileList_list)
                    {
                        try
                        {
                            m_rapi.MoveDeviceFile((Src + "\\" + file.FileName), Dst + file.FileName);
                        }

                        catch (Exception e)
                        {
                            //ErrorHandle.showError(e.Message, 302, 2);
                            return -1;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorHandle.showError(302, 2);
                return -1;
            }
            return 7;
        }
        private string GetFileName(string path)
        {
            return path.Substring(path.LastIndexOf('\\') + 1, path.Length - path.LastIndexOf('\\') - 1);
        }

        public bool FileExistOnPC(string filePath)
        {
            return System.IO.File.Exists(filePath);
        }

        public bool DirectoryExistOnPC(string dirPath)
        {
            return System.IO.Directory.Exists(dirPath);
        }

        public bool FileExistOnHHU(string filePath)
        {
            return m_rapi.DeviceFileExists(filePath);
        }

     

        public bool DirectoryExistOnHHU(string dirPath)
        {
            return m_rapi.DeviceFileExists(dirPath);
        }
    }
}