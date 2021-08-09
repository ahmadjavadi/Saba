using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HHU303.HHU
{
    class ErrorHandle
    {
        public static string 
            showError(int s2, int DisType)
        {
            string errDesc = "";
            switch (s2)
            {
                // HHU Errors
                case 0:
                    errDesc = "";
                    break;
                case 301:
                    errDesc = " .را دوباره نصب نماييد HHUنرم افزار ارتباط بين كامپيوتر و ، HHU مشكل در اتصال به ";
                    break;
                case 302:
                    errDesc = ".را بررسي نماييدHHU اتصال فيزيكي و روشن بودن ، HHU مشكل در اتصال به  ";
                    break;
                case 303:
                    errDesc = "  .نرم افزار قرائتگر وجود ندارد ، HHU مشكل در دريافت اطلاعات از ";
                    break;
                case 304:
                    errDesc = " . فايل اطلاعات مربوط به قرائتها در قرائتگر وجود ندارد  ، HHU مشكل در دريافت اطلاعات از ";
                    break;
                case 305:
                    errDesc = "HHU مشكل در دريافت اطلاعات از ";
                    break;
                case 306:
                    errDesc = "HHU مشكل در بروز رساني اطلاعات ";
                    break;
                case 307:
                    errDesc = "HHU مشكل در كپي اطلاعات در ";
                    break;
                case 308:
                    errDesc = "HHU مشكل در دسترسي به فايل اطلاعات در ";
                    break;
                case 309:
                    errDesc = "HHU مشكل در كپي فايل تنظيمات در ";
                    break;
                case 310:
                    errDesc = "حتما مي بايست حداقل يك گزينه را انتخاب كنيد ";
                    break;
                case 311:
                    errDesc = "فايلي براي قرائت وجود ندارد";
                    break;
                // End of HHU Errors
            }
            return errDesc;

        }
    }
}
