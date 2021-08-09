using System;
using RsaDateTime;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Globalization;

namespace VEE.WaterData
{
    class Verify_Consumption_Data303
    {
        List<Water_Data_Reading_Class> _allReadingDdata;
        const double Time_Diff = 30 * 24 * 3600;
       DateTime previousDateTime;
        DateTime currentDateTime;
        private List<Water_Data_Reading_Class> OrginalWaterDataList;        

        public Verify_Consumption_Data303(List<Water_Data_Reading_Class> OrginalWaterDataList)
        {
            // TODO: Complete member initialization
            this.OrginalWaterDataList = OrginalWaterDataList;
        }

        private Water_Data_Reading_Class Get_Orginal_Previous_Reading(Water_Data_Reading_Class previousReading)
        {
            Water_Data_Reading_Class wdrc = new Water_Data_Reading_Class();
            foreach (var item in OrginalWaterDataList)
            {
                if (item.Reading_Date.Equals(previousReading.Reading_Date))
                {
                    wdrc = item;
                    break;
                }
            }
            return wdrc;
        }
        int _listTransversal = 0;
        public List<Water_Data_Reading_Class> Data_Correction(List<Water_Data_Reading_Class> Water_Data_Reading_List, Water_Data_Reading_Class objValidData,string softwareversion="")
        {
            _allReadingDdata = Water_Data_Reading_List;
            int k = Water_Data_Reading_List.Count;
            Water_Data_Reading_Class previousReading, currentReading, temp;
            if(Meter_Consumption_Data303.ConsumedWaterdata.Count==0)
                InsertDataIntoConsmedWater(objValidData, 22,softwareversion);
            previousReading = objValidData;

            if (Water_Data_Reading_List.Count == 1)
            {
                if (VerifyDate(Water_Data_Reading_List[0].Reading_Date))
                {

                    InsertDataIntoConsmedWater(Water_Data_Reading_List[0], 22, softwareversion);
                }
            }
            else
            {
                for (_listTransversal = Water_Data_Reading_List.Count - 1; _listTransversal > 0; _listTransversal--)
                {
                    currentReading = (Water_Data_Reading_Class)Water_Data_Reading_List[_listTransversal - 1];
                    if (_listTransversal == Water_Data_Reading_List.Count - 1)
                        previousReading = objValidData;

                    else
                    {
                        previousReading = (Water_Data_Reading_Class)Water_Data_Reading_List[_listTransversal];
                        //  if (previousReading.Error != "" && n == Water_Data_Reading_List.Count - 1)
                        // previousReading = (Water_Data_Reading_Class)Water_Data_Reading_List[n];

                    }

                    if (!VerifyDate(previousReading.Reading_Date) && (!VerifyDate(currentReading.Reading_Date)))
                    {

                        Water_Data_Reading_Class tempWaterDataObj = new Water_Data_Reading_Class();
                        tempWaterDataObj = previousReading;
                        tempWaterDataObj.Error = "تاریخ قرائت معتبر نیست";
                        for (int u = _listTransversal + 1; u < k; u++)
                            Water_Data_Reading_List[u - 1] = Water_Data_Reading_List[u];
                        Water_Data_Reading_List[k - 1] = tempWaterDataObj;
                        k--;

                        tempWaterDataObj = currentReading;

                        for (int u = _listTransversal; u < k; u++)
                            Water_Data_Reading_List[u - 1] = Water_Data_Reading_List[u];
                        tempWaterDataObj.Error = "تاریخ قرائت معتبر نیست";
                        Water_Data_Reading_List[k - 1] = tempWaterDataObj;
                        k--;
                        if (k == 0 || _listTransversal - 1 == 0)
                            return Water_Data_Reading_List;
                        continue;
                    }

                    else if (!VerifyDate(previousReading.Reading_Date))
                    {
                        if (k - 1 == _listTransversal)
                            continue;
                        Water_Data_Reading_Class tempWaterDataObj = new Water_Data_Reading_Class();
                        tempWaterDataObj = previousReading;
                        for (int u = _listTransversal + 1; u < k; u++)
                            Water_Data_Reading_List[u - 1] = Water_Data_Reading_List[u];
                        tempWaterDataObj.Error = "تاریخ قرائت معتبر نیست";
                        Water_Data_Reading_List[k - 1] = tempWaterDataObj;
                        k--;
                        if (k == 0)
                            return Water_Data_Reading_List;
                        _listTransversal++;
                        continue;
                    }

                    if (!VerifyDate(currentReading.Reading_Date))
                    {
                        Water_Data_Reading_Class tempWaterDataObj = new Water_Data_Reading_Class();
                        tempWaterDataObj = currentReading;
                        for (int u = _listTransversal; u < k; u++)
                            Water_Data_Reading_List[u - 1] = Water_Data_Reading_List[u];
                        tempWaterDataObj.Error = "تاریخ قرائت معتبر نیست";
                        Water_Data_Reading_List[k - 1] = tempWaterDataObj;
                        k--;
                        if (k == 0 || _listTransversal - 1 == 0)
                            return Water_Data_Reading_List;
                        continue;
                    }

                    try
                    {
                        if (VerifyDate(currentReading.Reading_Date))
                        {
                            currentReading.Error = previousReading.Error;
                             
                            if ((currentReading.MeterNo.StartsWith("19") && softwareversion.EndsWith("3")))
                            {
                                temp = Water_Data_verification(previousReading, currentReading, out var monthDiff);

                                if (temp != null)
                                    InsertDataIntoConsmedWater(temp, monthDiff, softwareversion);
                            }
                            else
                            {
                                InsertDataIntoConsmedWater(currentReading, 23, softwareversion);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
            for (int i = 0; i < Meter_Consumption_Data303.ConsumedWaterdata.Count; i++)
            {
                if (i == Meter_Consumption_Data303.ConsumedWaterdata.Count - 1)
                {
                    Meter_Consumption_Data303.ConsumedWaterdata[i].MonthlyConsumption = Meter_Consumption_Data303.ConsumedWaterdata[i].TotalConsumption;
                          
                }
                else
                {
                    Meter_Consumption_Data303.ConsumedWaterdata[i].MonthlyConsumption =
                        (ConvertToDecimal(Meter_Consumption_Data303.ConsumedWaterdata[i].TotalConsumption) -
                        ConvertToDecimal(Meter_Consumption_Data303.ConsumedWaterdata[i + 1].TotalConsumption)).ToString();
                }
            }

            return Water_Data_Reading_List;
        }

        private void InsertDataIntoConsmedWater(Water_Data_Reading_Class temp, int monthDiff,string softwareVersion)
        {
            try
            {
                decimal[] totalConsumption = new decimal[24];
                decimal[] monthlyConsumption = new decimal[24];
                 
                decimal totalWater = ConvertToDecimal(temp.TotalWater);
                totalConsumption[0] = ConvertToDecimal(temp.water_data_Reading_List[0].Water_Consumption.ToString());
                if (temp.water_data_Reading_List[1].Water_Consumption >= 0)
                    monthlyConsumption[0] = totalConsumption[0] - ConvertToDecimal(temp.water_data_Reading_List[1].Water_Consumption.ToString());





                if (softwareVersion.EndsWith("2") || softwareVersion.EndsWith("1"))
                {
                    if (!Meter_Consumption_Data303.ConsumedWaterdata.Any(x => x.ConsumedDate.Contains(temp.Reading_Date)))
                    {
                        Meter_Consumption_Data303.ConsumedWaterdata.Insert(0, new VeeConsumedWater()
                        {
                            CustomerId = null,
                            MeterId = Meter_Consumption_Data303.MeterId,
                            ConsumedDate = temp.Reading_Date,

                            //MonthlyConsumption = temp.water_data_Reading_List[0].Water_Consumption.ToString(),
                            Flow = temp.TotalPumpWorkingTime,
                            TotalConsumption = temp.TotalWater.ToString(),

                            IsValid = true
                        });
                    }
                    return;

                }


                for (int j = 0; j < temp.water_data_Reading_List.Count - 1; j++)
                {
                    if (j > 23)
                        break;
                    if (temp.water_data_Reading_List[j].Water_Consumption == -1)
                        continue;

                    totalConsumption[j] = ConvertToDecimal(temp.water_data_Reading_List[j].Water_Consumption.ToString());
                    if (j < temp.water_data_Reading_List.Count - 1)
                    {
                        monthlyConsumption[j] = ConvertToDecimal(temp.water_data_Reading_List[j].Water_Consumption.ToString()) - ConvertToDecimal(temp.water_data_Reading_List[j + 1].Water_Consumption.ToString());
                    }
                }
                if (monthDiff < 24)
                    for (int j = monthDiff; j >= 0; j--)
                    {
                        if (j >= (temp.water_data_Reading_List.Count)) continue;

                        try
                        {
                            var date1 = GetConsumedDate(temp.Reading_Date, j);
                            if (temp.water_data_Reading_List[j].Water_Consumption == -1) continue;

                            //totalWaater = totaltemp[j].water_data_Reading_List[j].Water_Consumption.ToString();
                            if (Meter_Consumption_Data303.ConsumedWaterdata.Count > 0 && Meter_Consumption_Data303.ConsumedWaterdata[0].ConsumedDate.CompareTo(date1) > -1)
                                continue;
                            if (!Meter_Consumption_Data303.ConsumedWaterdata.Any(x => x.ConsumedDate.Contains(date1)))
                            {
                                Meter_Consumption_Data303.ConsumedWaterdata.Insert(0, new VeeConsumedWater()
                                {
                                    CustomerId = null,
                                    MeterId = Meter_Consumption_Data303.MeterId,
                                    ConsumedDate = date1,
                                    Flow = temp.water_data_Reading_List[j].Month_Maximum_Debi.ToString(),
                                    TotalConsumption = temp.water_data_Reading_List[j].Water_Consumption.ToString(),

                                    MonthlyConsumption = monthlyConsumption[j].ToString(),
                                    IsValid = temp.water_data_Reading_List[j].IsValidData
                                });
                            }

                        }
                        catch (Exception ex)
                        {

                        }

                    }
                else
                {
                    // فاصله بین دو قرائت بیشتر از 23 ماه است
                    for (int j = monthlyConsumption.Length - 1; j > 0; j--)
                    {
                        if (j >= (temp.water_data_Reading_List.Count)) continue;

                        try
                        {
                            if (temp.water_data_Reading_List[j].Water_Consumption == -1) continue;
                            var date2 = GetConsumedDate(temp.Reading_Date, j);
                            if (Meter_Consumption_Data303.ConsumedWaterdata.Count > 0 && Meter_Consumption_Data303.ConsumedWaterdata[0].ConsumedDate.CompareTo(date2) > -1)
                                continue;
                            //totalWaater = totaltemp[j].water_data_Reading_List[j].Water_Consumption.ToString();
                            if (!Meter_Consumption_Data303.ConsumedWaterdata.Any(x => x.ConsumedDate.Contains(date2)))
                            {
                                Meter_Consumption_Data303.ConsumedWaterdata.Insert(0, new VeeConsumedWater()
                                {
                                    CustomerId = null,
                                    MeterId = Meter_Consumption_Data303.MeterId,
                                    ConsumedDate = date2,
                                    Flow = temp.water_data_Reading_List[j].Month_Maximum_Debi.ToString(),
                                    MonthlyConsumption = temp.water_data_Reading_List[j].Water_Consumption.ToString(),

                                    TotalConsumption = totalConsumption[j].ToString(),
                                    IsValid = temp.water_data_Reading_List[j].IsValidData
                                });
                            }

                        }
                        catch (Exception ex)
                        {

                        }

                    }
                }

            }
            catch (Exception ex)
            {

                //throw;
            }
            
            
        }

        decimal ConvertToDecimal(string value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch (Exception)
            {
                return Convert.ToDecimal(value.Replace(".", "/"));
            }
        }
            
            
        string GetConsumedDate(string readDate, int index)
        {
            var dt =RsaDateTime.PersianDateTime.ConvertToGeorgianDateTime(readDate);
            var firstMonth = new DateTime(dt.Year, dt.Month, 20);
            if (dt.Day < 20)
            {
                if (dt.Month == 1)
                {
                    firstMonth = new DateTime(dt.Year-1, 12, 20); 
                }
                else
                {
                    firstMonth = new DateTime(dt.Year, dt.Month - 1, 20);
                }
            }
            firstMonth = firstMonth.AddMonths((-1)*index );
            return firstMonth.ToPersianDateString();
        }
        private Water_Data_Reading_Class Water_Data_verification(Water_Data_Reading_Class previousReading, Water_Data_Reading_Class currentReading, out int Month_Diff)
        {
           var temp = currentReading.Get_Water_Data_Reading_Class();
            int newValidDataIndex;
            int oldValidDataIndex;
            int  position = 0;
            bool previousReading_IS_VEE = false;

            Water_data nearest_Valid_Water_Data = new Water_data();

            Month_Diff = DiffOfShamsiDate(previousReading.Reading_Date, temp.Reading_Date);

            #region Month_diff>23 || Month_diff<=0

            if (Month_Diff >= 23)
            {
                temp.Error = "تفاوت زمان دو قرائت متوالی بیش از 23 ماه است";
                return temp;
            }
            if (Month_Diff < 0)
            {
                temp.Error = "خطا : تاریخ کنتور تغییر کرده است";
                VEE.Vee207.hasError = true;
                return null; Water_Data_Verificaiton(temp, Month_Diff);
            }
            if (Month_Diff == 0)
            {
                for (int i = 1; i < 23; i++)
                {
                    temp.water_data_Reading_List[i] = previousReading.water_data_Reading_List[i];
                }
                temp.Error = previousReading.Error;
                return temp;
            }
            #endregion

            #region 0 < Month_diff < 23

            int Index_Of_Valid_Zero = Compute_Index_of_Valid_Zero_Consumption(previousReading);

            oldValidDataIndex = Find_Nearest_Valid_Water_Data(previousReading, Index_Of_Valid_Zero, ref nearest_Valid_Water_Data);

            int InvalidZeroDataInPrevoisReading = GetInvalidZeroDataInPrevoisReadin(previousReading,oldValidDataIndex);

            newValidDataIndex = Find_Valid_Water_Data_In_CurrentReading( nearest_Valid_Water_Data, temp);
                      

            #region Find Valid Data In previous Reading
            if (nearest_Valid_Water_Data.IsValidData ||previousReading.Error=="-3")
            {
                // jumpPosition is Position of DataValid In currentReading

                if (previousReading.Error=="-3")
                {                     
                       newValidDataIndex = Search_Nearest_Common_Data(previousReading,ref nearest_Valid_Water_Data,ref oldValidDataIndex,
                          temp, Month_Diff, ref position);
                }
                else
                {                    
                       newValidDataIndex = Search_Nearest_Common_Valid_Data(nearest_Valid_Water_Data, oldValidDataIndex,
                        temp, Month_Diff, ref position, previousReading);
                }
                #region Dont Find Valid Water Data In Curret Reading
                if (newValidDataIndex == -1)
                {
                    previousReading_IS_VEE = false;
                    for (int i = 0; i < 23; i++)
                    {
                        if (((Water_data)previousReading.water_data_Reading_List[i]).Water_Consumption < -1)
                        {
                            previousReading_IS_VEE = true;
                            break;
                        }
                    }

                    if (previousReading_IS_VEE)
                    {
                        Water_Data_Reading_Class orginal_Previous_Reading = Get_Orginal_Previous_Reading(previousReading) ;
                        temp = Water_Data_verification(orginal_Previous_Reading, temp, out var monthDiff);
                        int k = 1;
                        for (int i = Month_Diff + 1; i < 23; i++)
                        {
                            temp.water_data_Reading_List[i] = previousReading.water_data_Reading_List[k];
                            k++;
                        }
                    }
                    else
                    {
                        if (temp == null)
                            return null;
                        if (!temp.Error.Contains("تعداد پرشهای صورت گرفته خیلی زیاد اس"))
                        {
                            temp.Error = "داده معتبری برای اعتبار سنجی وجود ندارد. تعداد پرشهای صورت گرفته خیلی زیاد است";
                            // LogFile.WriteLOG(serial + " ( "+currentReading.Reading_Date+" ): " + currentReading.Error,false);

                            temp = Modified_Data_With_Large_Number_Of_Jumps(temp, previousReading);
                            int k = 1; 
                            if (temp == null)
                                return null;
                            for (int i = Month_Diff + 1; i < 23; i++)
                            {
                                temp.water_data_Reading_List[i] = previousReading.water_data_Reading_List[k];
                                k++;
                            }
                        }
                        return temp;
                    }
                }


                if (newValidDataIndex == -2)
                {
                    temp.Error = ".تاریخ و ساعت کنتور تغییر کرده است. ممکن است مصارف ماهها معتبر نباشند";
                    //  LogFile.WriteLOG(serial + " ( "+currentReading.Reading_Date+" ): " + currentReading.Error,false);
                    Insert_PreviousReading_to_currentReading(temp, previousReading, oldValidDataIndex, position);
                }

                if (newValidDataIndex == -3)
                {
                    //SaveToExcell(temp, previousReading, "");
                    //currentReading.Error = ".تاریخ و ساعت کنتور تغییر کرده است. ";
                    //  LogFile.WriteLOG(serial + " ( "+currentReading.Reading_Date+" ): " + currentReading.Error,false);
                    
                }
                #endregion

                #region Find Valid Water Data In Curret Reading
                if (newValidDataIndex > -1)
                {

                    int jumpCount = newValidDataIndex - Month_Diff - oldValidDataIndex;
                    while (jumpCount > 0)
                    {
                        temp.water_data_Reading_List.RemoveAt(1);
                        jumpCount--;
                    }

                    return temp;
                }
                #endregion

                return temp;
            }
            #endregion

            #region Don't Find Valid Data In previous Reading
            else
            {
                previousReading_IS_VEE = false;
                for (int i = 0; i < previousReading.water_data_Reading_List.Count; i++)
                {
                    if (((Water_data)previousReading.water_data_Reading_List[i]).Water_Consumption < -1)
                    {
                        previousReading_IS_VEE = true;
                        break;
                    }
                }

                if (previousReading_IS_VEE)
                {
                    Water_Data_Reading_Class orginal_Previous_Reading =Get_Orginal_Previous_Reading(previousReading);
                    temp = Water_Data_verification(orginal_Previous_Reading, temp, out var monthDiff);
                    int k = 1;
                    for (int i = Month_Diff + 1; i < 23; i++)
                    {
                        temp.water_data_Reading_List[i] = previousReading.water_data_Reading_List[k];
                        k++;
                    }
                    return temp;
                }
                else
                {
                    if (!temp.Error .Contains("تعداد پرشهای صورت گرفته خیلی زیاد اس"))
                    {
                        temp.Error = "داده معتبری برای اعتبار سنجی وجود ندارد. تعداد پرشهای صورت گرفته خیلی زیاد است";

                        temp = Modified_Data_With_Large_Number_Of_Jumps(temp, previousReading);
                        int k = 1;
                        for (int i = Month_Diff + 1; i < 23; i++)
                        {
                            temp.water_data_Reading_List[i] = previousReading.water_data_Reading_List[k];
                            k++;
                        }

                    }
                    return temp;
                }

            }
            #endregion

            #endregion
        }

        private void RemoveJump(Water_Data_Reading_Class temp, Water_data wd, int indexWd)
        {
            if (indexWd == 1)
            {

                Water_data wdnew = new Water_data((temp.water_data_Reading_List[2].Water_Consumption + wd.Water_Consumption).ToString(),
                    temp.water_data_Reading_List[2].Month_Maximum_Debi.ToString(), false);

                RemoveJumpFromAllReadingData(temp.water_data_Reading_List[1], temp.water_data_Reading_List[2], null, wdnew);

                temp.water_data_Reading_List[2].Water_Consumption += wd.Water_Consumption;
                temp.water_data_Reading_List[2].IsValidData = false;
                temp.water_data_Reading_List.RemoveAt(1);
            }
            else
            {
                if (temp.water_data_Reading_List[indexWd + 1].Water_Consumption > temp.water_data_Reading_List[indexWd - 1].Water_Consumption)
                {
                    Water_data wdnew = new Water_data((temp.water_data_Reading_List[indexWd - 1].Water_Consumption + wd.Water_Consumption).ToString(),
                       temp.water_data_Reading_List[indexWd - 1].Month_Maximum_Debi.ToString(), false);

                    RemoveJumpFromAllReadingData(temp.water_data_Reading_List[indexWd - 1], temp.water_data_Reading_List[indexWd], null, wdnew);


                    temp.water_data_Reading_List[indexWd - 1].Water_Consumption += wd.Water_Consumption;
                    temp.water_data_Reading_List[indexWd - 1].IsValidData = false;
                    temp.water_data_Reading_List.RemoveAt(indexWd);
                }
                else
                {
                    Water_data wdnew = new Water_data((temp.water_data_Reading_List[indexWd + 1].Water_Consumption + wd.Water_Consumption).ToString(),
                          temp.water_data_Reading_List[indexWd + 1].Month_Maximum_Debi.ToString(), false);

                    RemoveJumpFromAllReadingData(null, temp.water_data_Reading_List[indexWd], temp.water_data_Reading_List[indexWd - 1], wdnew);
                    temp.water_data_Reading_List[indexWd + 1].Water_Consumption += wd.Water_Consumption;
                    temp.water_data_Reading_List[indexWd + 1].IsValidData = false;
                    temp.water_data_Reading_List.RemoveAt(indexWd);

                }
            }
        }

        private Water_Data_Reading_Class RemoveJump(Water_Data_Reading_Class previousReading, Water_Data_Reading_Class currentReading, int newValidDataIndex, int oldValidDataIndex, int jumpCount)
        {
            int JumpNumber = jumpCount / 2;
            int number_of_Invalid_Data_with_Zero = 0;
            int number_of_InValid_Data_withOut_Zero = 0;

            #region Counting the number of non-zero consumption
            for (int y = newValidDataIndex - 2; y > 0; y--)
            {
                if (Compute_Rate((Water_data)currentReading.water_data_Reading_List[y], false))
                {
                    number_of_InValid_Data_withOut_Zero++;
                }
                if (Compute_Rate((Water_data)currentReading.water_data_Reading_List[y], true))
                {
                    number_of_Invalid_Data_with_Zero++;
                }
            }
            #endregion

            #region Number of jump is equal with number of invalid data with non-zero consumption
            if (JumpNumber == number_of_InValid_Data_withOut_Zero)
            {
                RemoveJump(ref currentReading, previousReading, false, newValidDataIndex, JumpNumber, oldValidDataIndex);
            }
            #endregion

            #region Number of jump is equal with number of invalid data with non-zero consumption
            else if (JumpNumber < number_of_InValid_Data_withOut_Zero)
            {
                RemoveJump(ref currentReading, previousReading, false, newValidDataIndex, JumpNumber, oldValidDataIndex, number_of_InValid_Data_withOut_Zero);
            }
            #endregion

            #region Number of jump is equal to the number of invalid data and zero consumption
            else if (JumpNumber == number_of_Invalid_Data_with_Zero)
            {
                RemoveJump(ref currentReading, previousReading, true, newValidDataIndex, JumpNumber, oldValidDataIndex);
            }
            #endregion

            #region number_of_InValid_Data_withOut_Zero <JumpNumber <number_of_Invalid_Data_with_Zero
            else if (number_of_InValid_Data_withOut_Zero < JumpNumber && JumpNumber < number_of_Invalid_Data_with_Zero)
            {
                RemoveJump(ref currentReading, previousReading, false, newValidDataIndex, JumpNumber, oldValidDataIndex);
                Remove_Zero_Consumption(ref currentReading, previousReading, newValidDataIndex, JumpNumber - number_of_InValid_Data_withOut_Zero,
                    oldValidDataIndex);

            }
            else
            {

                //  currentReading.Error = "در قرائت " + currentReading.Reading_Date + "محل پرش را نمی توان تشخیص داد";
                // در این حالت کوچکترین عدد را پیدا کن و یه پرش را با استفاده از آن حذف کن
                MergeLowerConsumption(ref currentReading, previousReading, newValidDataIndex, JumpNumber, oldValidDataIndex);
                Vee207.hasError = true;

            }
            #endregion
            return currentReading;
        }

        private void MergeLowerConsumption(ref Water_Data_Reading_Class currentReading, Water_Data_Reading_Class previousReading, int newValidDataIndex, int jumpNumber, int oldValidDataIndex)
        {
            try
            {
               
                while (jumpNumber > 0)
                {
                    
                      var lowValue = currentReading.water_data_Reading_List[2];
                    int lowValueIndex = 2;


                    var lowValue1 = currentReading.water_data_Reading_List[2];
                    int lowValueIndex1 = 2;


                    double minValue = double.MaxValue;// lowValue.Water_Consumption + currentReading.water_data_Reading_List[1].Water_Consumption;
                    if (newValidDataIndex < 3)
                    {
                        ///////////////////////// دیتای مصرف کوچیک نمیشه پیدا کرد
                    }
                    for (int i = 2; i < newValidDataIndex - 1; i++)
                    {
                        var mergeValue = currentReading.water_data_Reading_List[i-1].Water_Consumption + currentReading.water_data_Reading_List[i].Water_Consumption + currentReading.water_data_Reading_List[i+1].Water_Consumption;
                        if (mergeValue < minValue)
                        {
                            minValue = mergeValue;
                            lowValue = currentReading.water_data_Reading_List[i];
                            lowValueIndex = i;
                        }
                        if (lowValue.Water_Consumption > currentReading.water_data_Reading_List[i].Water_Consumption)
                        {
                            lowValue1 = currentReading.water_data_Reading_List[i];
                            lowValueIndex1 = i;
                        }
                    }
                    if(lowValueIndex!= lowValueIndex1) 
                    {
                    }

                    Water_data wd = new Water_data();

                    wd.Water_Consumption = ((Water_data)currentReading.water_data_Reading_List[lowValueIndex - 1]).Water_Consumption;
                    wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[lowValueIndex]).Water_Consumption;


                    wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[lowValueIndex - 1]).Month_Maximum_Debi,
                    ((Water_data)currentReading.water_data_Reading_List[lowValueIndex]).Month_Maximum_Debi);

                    wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[lowValueIndex + 1]).Month_Maximum_Debi,
                            wd.Month_Maximum_Debi);
                    wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[lowValueIndex + 1]).Water_Consumption;

                    wd.IsValidData = false;

                    if (!HighLow(wd))
                    {
                        break;
                    }
                    currentReading.water_data_Reading_List[lowValueIndex - 1] = wd;

                    currentReading.water_data_Reading_List.RemoveRange(lowValueIndex, 2);
                    newValidDataIndex -= 2;
                    jumpNumber--;
                }
            }
            catch (Exception ex)
            {
                 
            }
         
        }

        private int Find_Valid_Water_Data_In_CurrentReading(Water_data valid_Water_Data, Water_Data_Reading_Class currentReading)
        {
            for (int i = 0; i < currentReading.water_data_Reading_List.Count; i++)
            {
                if (currentReading.water_data_Reading_List[i].Water_Consumption == valid_Water_Data.Water_Consumption                     )
                    return i;
            }
            return -1;
        }

        private int GetInvalidZeroDataInPrevoisReadin(Water_Data_Reading_Class previousReading, int oldValidDataIndex)
        {
            int cnt = 0;
            for (int i = 0; i < oldValidDataIndex; i++)
            {
                if (previousReading.water_data_Reading_List[i].IsValidData == false && previousReading.water_data_Reading_List[i].Water_Consumption == 0)
                {
                    cnt++;
                }
            }
            return cnt;
        }

        private void SaveToExcell(Water_Data_Reading_Class currentReading, Water_Data_Reading_Class previousReading , string text)
        {
            Microsoft.Office.Interop.Excel.Application oXL;
            Microsoft.Office.Interop.Excel._Workbook oWB;
            Microsoft.Office.Interop.Excel._Worksheet oSheet;
            Microsoft.Office.Interop.Excel.Range oRng;
            object misvalue = System.Reflection.Missing.Value;
            try
            {
                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application();
                 
                //Get a new workbook.
                oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(""));
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;

                //Add table headers going cell by cell.
                oSheet.Cells[1, 1] = "currentReading.Reading_Date";
                oSheet.Cells[2, 1] = "currentReading.TotalWater";
                oSheet.Cells[1, 2] = currentReading.Reading_Date;
                oSheet.Cells[2, 2] = currentReading.TotalWater;
                for (int i = 0; i < 23; i++)
                {
                    oSheet.Cells[i + 3, 1] = currentReading.water_data_Reading_List[i].Water_Consumption;
                    oSheet.Cells[i + 3, 2] = currentReading.water_data_Reading_List[i].Month_Maximum_Debi;
                }
                oSheet.Cells[1, 5] = "previousReading.Reading_Date";
                oSheet.Cells[2, 5] = "previousReading.TotalWater";
                oSheet.Cells[1, 6] = previousReading.Reading_Date;
                oSheet.Cells[2, 6] = previousReading.TotalWater;
                for (int i = 0; i < 23; i++)
                {
                    oSheet.Cells[i + 3, 5] = previousReading.water_data_Reading_List[i].Water_Consumption;
                    oSheet.Cells[i + 3, 6] = previousReading.water_data_Reading_List[i].Month_Maximum_Debi;
                }

                //Format A1:D1 as bold, vertical alignment = center.
                oSheet.get_Range("A1", "G1").Font.Bold = true;
                oSheet.get_Range("A1", "G1").VerticalAlignment =
                    Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                // Create an array to multiple values at once.



                oRng = oSheet.get_Range("A1", "G1");
                oRng.NumberFormat = "$0.00";

                //AutoFit columns A:D.
                oRng = oSheet.get_Range("A1", "G1");
                oRng.EntireColumn.AutoFit();

                oXL.Visible = false;
                oXL.UserControl = false;
                if (!System.IO.Directory.Exists("D:\\VeeData\\" + currentReading.MeterNo))
                    System.IO.Directory.CreateDirectory("D:\\VeeData\\" + currentReading.MeterNo);
                oWB.SaveAs("D:\\VeeData\\" + currentReading.MeterNo+"\\" + text + currentReading.MeterNo + currentReading.Reading_Date.Replace("/","").Replace(":", "") + ".xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                    false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                oWB.Close();
                oXL.Quit();
            }
            catch(Exception ex)
            { 
            }
        }

        private Water_Data_Reading_Class Modified_Data_With_Large_Number_Of_Jumps(Water_Data_Reading_Class currentReading, Water_Data_Reading_Class previousReading)
        {            
            Water_Data_Reading_Class orginal_Previous_Reading = Get_Orginal_Previous_Reading(previousReading);
            Water_Data_Reading_Class temp_Reading = new Water_Data_Reading_Class();
            temp_Reading = verification_With_orginal_Previous_Reading(orginal_Previous_Reading, currentReading);
            if (temp_Reading == null)
            {
                VEE.Vee207.hasError = true;
                return null;
            }
            int Month_diff = DiffOfShamsiDate(previousReading.Reading_Date, currentReading.Reading_Date);

            if (temp_Reading != null)
            {
                int k = 1;
                for (int i = Month_diff + 1; i < 23; i++)
                {
                    temp_Reading.water_data_Reading_List[i] = previousReading.water_data_Reading_List[k];
                    k++;
                }
                return temp_Reading;
            }

            int first_unvalid_data_Position = 1;
            int MinJumpNumber = 0;

            double maxDebi = 0;
            double total_Register_Consumption_In_current_Reading = 0;
            double Unsaved_Consumption = 0;
            int number_Of_invalid_Data_Without_Zero = 0, number_Of_invalid_Data_With_Zero = 0;
            //========================================================

            for (int i = 1; i < currentReading.water_data_Reading_List.Count; i++)
            {
                if (Compute_Rate((Water_data)currentReading.water_data_Reading_List[i], true))
                {
                    break;
                }
                first_unvalid_data_Position = i;
            }


            #region Month_diff = 0

            if (Month_diff == 0)
            {
                for (int i = 1; i < 23; i++)
                {
                    currentReading.water_data_Reading_List[i] = previousReading.water_data_Reading_List[i];
                }
                return currentReading;
            }
            #endregion

            #region Find Maximum of Debi and Maximum of consumption
            for (int i = currentReading.water_data_Reading_List.Count - 1; i > -1; i--)
            {
                total_Register_Consumption_In_current_Reading += ((Water_data)currentReading.water_data_Reading_List[i]).Water_Consumption;
                if (maxDebi < ((Water_data)currentReading.water_data_Reading_List[i]).Month_Maximum_Debi)
                    maxDebi = ((Water_data)currentReading.water_data_Reading_List[i]).Month_Maximum_Debi;
                if (Compute_Rate(((Water_data)currentReading.water_data_Reading_List[i]), false))
                {
                    number_Of_invalid_Data_Without_Zero++;
                }
                if (Compute_Rate(((Water_data)currentReading.water_data_Reading_List[i]), true))
                {
                    number_Of_invalid_Data_With_Zero++;
                }
            }
            Unsaved_Consumption = double.Parse(currentReading.TotalWater, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo) 
                - double.Parse(previousReading.TotalWater, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo) -
            total_Register_Consumption_In_current_Reading + ((Water_data)previousReading.water_data_Reading_List[0]).Water_Consumption;
            #endregion

            MinJumpNumber = (23 - Month_diff) / 2;

            if (Unsaved_Consumption * 1000 / maxDebi < Time_Diff)
            {
                return RemoveAllUnvalidData(currentReading, previousReading, Month_diff, first_unvalid_data_Position);
            }

            if (MinJumpNumber > number_Of_invalid_Data_With_Zero)
                return RemoveAllUnvalidData(currentReading, previousReading, Month_diff, first_unvalid_data_Position);

            return RemoveJump(currentReading, previousReading, Month_diff, first_unvalid_data_Position);
        }

        private Water_Data_Reading_Class verification_With_orginal_Previous_Reading(Water_Data_Reading_Class previousReading, Water_Data_Reading_Class currentReading)
        {
            int valid_WaterDataPosition_In_Current_Reading;
            int valid_WaterDataPosition_In_Previous_Reading;
            int Month_Diff, position = 0;
            bool previousReading_IS_VEE = false;

            Water_data nearest_Valid_Water_Data = new Water_data();

            Month_Diff = DiffOfShamsiDate(previousReading.Reading_Date, currentReading.Reading_Date);

            #region 0 < Month_diff < 23

            int Index_Of_Valid_Zero = Compute_Index_of_Valid_Zero_Consumption(previousReading);

            valid_WaterDataPosition_In_Previous_Reading = Find_Nearest_Valid_Water_Data(previousReading, Index_Of_Valid_Zero, ref nearest_Valid_Water_Data);

            #region Find Valid Data In previous Reading
            if (nearest_Valid_Water_Data.IsValidData)
            {
                // jumpPosition is Position of DataValid In currentReading
                valid_WaterDataPosition_In_Current_Reading = Search_Nearest_Common_Valid_Data(nearest_Valid_Water_Data, valid_WaterDataPosition_In_Previous_Reading,
                    currentReading, Month_Diff, ref position, previousReading);

                #region Dont Find Valid Water Data In Curret Reading
                if (valid_WaterDataPosition_In_Current_Reading == -1)
                {
                    return null;
                }


                if (valid_WaterDataPosition_In_Current_Reading == -2)
                {
                    return null;
                }
                #endregion

                #region Find Valid Water Data In Curret Reading
                if (valid_WaterDataPosition_In_Current_Reading > -1)
                {
                    int jumpCount = valid_WaterDataPosition_In_Current_Reading - Month_Diff - valid_WaterDataPosition_In_Previous_Reading;

                    if (jumpCount % 2 == 1 || jumpCount < 0)
                    {
                        currentReading.Error = "خطا: احتمالا تاریخ کنتور تغییر پیدا کرده است";
                    }

                    #region Find Jump
                    if (jumpCount > 1)
                    {
                        int JumpNumber = jumpCount / 2;
                        int number_of_Invalid_Data_with_Zero = 0;
                        int number_of_InValid_Data_withOut_Zero = 0;

                        #region Counting the number of non-zero consumption
                        for (int y = valid_WaterDataPosition_In_Current_Reading - 2; y > 0; y--)
                        {
                            if (Compute_Rate((Water_data)currentReading.water_data_Reading_List[y], false))
                            {
                                number_of_InValid_Data_withOut_Zero++;
                            }
                            if (Compute_Rate((Water_data)currentReading.water_data_Reading_List[y], true))
                            {
                                number_of_Invalid_Data_with_Zero++;
                            }
                        }
                        #endregion

                        #region Number of jump is equal with number of invalid data with non-zero consumption
                        if (JumpNumber <= number_of_InValid_Data_withOut_Zero)
                        {
                            RemoveJump(ref currentReading, previousReading, false, valid_WaterDataPosition_In_Current_Reading, JumpNumber, valid_WaterDataPosition_In_Previous_Reading);
                        }
                        #endregion

                        #region Number of jump is equal to the number of invalid data and zero consumption
                        else if (JumpNumber == number_of_Invalid_Data_with_Zero)
                        {
                            RemoveJump(ref currentReading, previousReading, true, valid_WaterDataPosition_In_Current_Reading, JumpNumber, valid_WaterDataPosition_In_Previous_Reading);
                        }
                        #endregion

                        #region number_of_InValid_Data_withOut_Zero <JumpNumber <number_of_Invalid_Data_with_Zero
                        else if (number_of_InValid_Data_withOut_Zero < JumpNumber && JumpNumber < number_of_Invalid_Data_with_Zero)
                        {
                            RemoveJump(ref currentReading, previousReading, false, valid_WaterDataPosition_In_Current_Reading, JumpNumber, valid_WaterDataPosition_In_Previous_Reading);
                            Remove_Zero_Consumption(ref currentReading, previousReading, valid_WaterDataPosition_In_Current_Reading, JumpNumber - number_of_InValid_Data_withOut_Zero, valid_WaterDataPosition_In_Previous_Reading);
                        }
                        else
                        {
                            // currentReading.Error = "در قرائت " + currentReading.Reading_Date + "محل پرش را نمی توان تشخیص داد";
                            //  LogFile.WriteLOG(serial + " ( " + currentReading.Reading_Date + " ): " + currentReading.Error, false);                            
                        }
                        #endregion

                    }
                    #endregion

                    #region Dont Find Jump
                    else
                    {

                        for (int i = 1;
                            valid_WaterDataPosition_In_Current_Reading + i < 23; i++)
                        {
                            currentReading.water_data_Reading_List[valid_WaterDataPosition_In_Current_Reading + i] =
                                previousReading.water_data_Reading_List[valid_WaterDataPosition_In_Previous_Reading + i];
                        }
                    }
                    #endregion
                }
                #endregion

                return currentReading;
            }
            #endregion

            #region Don't Find Valid Data In previous Reading
            else
            {
                previousReading_IS_VEE = false;
                for (int i = 0; i < 23; i++)
                {
                    if (((Water_data)previousReading.water_data_Reading_List[i]).Water_Consumption < -1)
                    {
                        previousReading_IS_VEE = true;
                        break;
                    }
                }

                if (previousReading_IS_VEE)
                {
                    Water_Data_Reading_Class orginal_Previous_Reading = Get_Orginal_Previous_Reading(previousReading);
                    currentReading = Water_Data_verification(orginal_Previous_Reading, currentReading, out var monthDiff);
                    int k = 1;
                    for (int i = Month_Diff + 1; i < 23; i++)
                    {
                        currentReading.water_data_Reading_List[i] = previousReading.water_data_Reading_List[k];
                        k++;
                    }
                    return currentReading;
                }
                else
                {
                    if (!currentReading.Error.Contains("تعداد پرشهای صورت گرفته خیلی زیاد اس"))
                    {
                        currentReading.Error = "داده معتبری برای اعتبار سنجی وجود ندارد. تعداد پرشهای صورت گرفته خیلی زیاد است";

                        currentReading = Modified_Data_With_Large_Number_Of_Jumps(currentReading, previousReading);
                        int k = 1;
                        for (int i = Month_Diff + 1; i < 23; i++)
                        {
                            currentReading.water_data_Reading_List[i] = previousReading.water_data_Reading_List[k];
                            k++;
                        }
                    }
                    return currentReading;
                }
            }
            #endregion

            #endregion
        }

        private Water_Data_Reading_Class Water_Data_Verificaiton(Water_Data_Reading_Class currentReading, int MonthDiff)
        {
            double sum = 0;
            if (MonthDiff > 23)
                return currentReading;
            int lastNonZeroConsumption = 23;
            for (int i = 23; i >= 0; i--)
            {
                if ((((Water_data)currentReading.water_data_Reading_List[lastNonZeroConsumption]).Water_Consumption) == 0)
                    lastNonZeroConsumption--;
                sum += (((Water_data)currentReading.water_data_Reading_List[i]).Water_Consumption);
            }



            bool Overflow = false;

            sum = Math.Round(sum, MidpointRounding.ToEven);
            double total = Math.Round(double.Parse(currentReading.TotalWater, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo), MidpointRounding.ToEven);
            if (lastNonZeroConsumption <= MonthDiff && sum >= total)
            {
                return currentReading;
            }
            else
                Overflow = true;

            for (int y = 23; y > 0; y--)
            {
                if (MonthDiff < 23)
                    if (currentReading.water_data_Reading_List.Count < MonthDiff)
                        break;
                if (Compute_Rate((Water_data)currentReading.water_data_Reading_List[y], Overflow))
                {

                    if (y < currentReading.water_data_Reading_List.Count - 1)
                    {
                        Water_data wd = new Water_data();
                        // find jump
                        wd.Water_Consumption = ((Water_data)currentReading.water_data_Reading_List[y - 1]).Water_Consumption;
                        wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y]).Water_Consumption;
                        wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y + 1]).Water_Consumption;

                        wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y - 1]).Month_Maximum_Debi,
                        ((Water_data)currentReading.water_data_Reading_List[y]).Month_Maximum_Debi);

                        wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y + 1]).Month_Maximum_Debi,
                            wd.Month_Maximum_Debi);
                        wd.IsValidData = false;

                        if (!HighLow(wd))
                            continue;
                        RemoveJumpFromAllReadingData(currentReading.water_data_Reading_List[y - 1], currentReading.water_data_Reading_List[y], currentReading.water_data_Reading_List[y + 1], wd);
                        currentReading.water_data_Reading_List[y - 1] = wd;

                        currentReading.water_data_Reading_List.RemoveRange(y, 2);

                    }

                    else if (y == currentReading.water_data_Reading_List.Count - 1)
                    {
                        Water_data wd = new Water_data();
                        // find jump
                        wd.Water_Consumption = ((Water_data)currentReading.water_data_Reading_List[y - 1]).Water_Consumption;
                        wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y]).Water_Consumption;


                        wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y - 1]).Month_Maximum_Debi,
                        ((Water_data)currentReading.water_data_Reading_List[y]).Month_Maximum_Debi);

                        wd.IsValidData = false;

                        if (!HighLow(wd))
                            continue;
                        RemoveJumpFromAllReadingData(currentReading.water_data_Reading_List[y - 1], currentReading.water_data_Reading_List[y], currentReading.water_data_Reading_List[y + 1], wd);

                        currentReading.water_data_Reading_List[y - 1] = wd;

                        currentReading.water_data_Reading_List.RemoveRange(y, 1);

                    }

                }
            }


            int g = -1;

            if (sum == (double.Parse(currentReading.TotalWater, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo)))
                g = 0;
            for (int m = currentReading.water_data_Reading_List.Count; currentReading.water_data_Reading_List.Count < 23; m++)
            {
                Water_data wd = new Water_data();
                wd.IsValidData = false;
                wd.Water_Consumption = g;
                wd.Water_Consumption = g;
                currentReading.water_data_Reading_List.Add(wd);
            }

            //====================================================================================================================================
            return currentReading;
        }

        private void RemoveJump(ref Water_Data_Reading_Class currentReading, Water_Data_Reading_Class previousReading, bool considerZero,
            int valid_WaterDataPosition_In_Current_Reading, int JumpNumber, int valid_WaterDataPosition_In_Previous_Reading)
        {
            for (int y = 1; y < currentReading.water_data_Reading_List.Count; y++)
            {
                if (Compute_Rate((Water_data)currentReading.water_data_Reading_List[y], considerZero))
                {
                    if (Compute_Month_Number(currentReading.Reading_Date, y) < 8)
                    {
                        Water_data wd = new Water_data();
                        // find jump
                        wd.Water_Consumption = ((Water_data)currentReading.water_data_Reading_List[y - 1]).Water_Consumption;
                        wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y]).Water_Consumption;


                        wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y - 1]).Month_Maximum_Debi,
                        ((Water_data)currentReading.water_data_Reading_List[y]).Month_Maximum_Debi);

                        if (y < currentReading.water_data_Reading_List.Count - 1)
                        {
                            wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y + 1]).Month_Maximum_Debi,
                                wd.Month_Maximum_Debi);
                            wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y + 1]).Water_Consumption;
                        }

                        wd.IsValidData = false;

                        if (!HighLow(wd))
                            continue;
                        if(y + 1>= currentReading.water_data_Reading_List.Count)
                            RemoveJumpFromAllReadingData(currentReading.water_data_Reading_List[y - 1], currentReading.water_data_Reading_List[y], null, wd);
                        else
                            RemoveJumpFromAllReadingData(currentReading.water_data_Reading_List[y - 1], currentReading.water_data_Reading_List[y], currentReading.water_data_Reading_List[y + 1], wd);

                        currentReading.water_data_Reading_List[y - 1] = wd;
                        if (y == currentReading.water_data_Reading_List.Count - 1)
                        {
                            currentReading.water_data_Reading_List.RemoveAt(y);
                        }
                        else
                        {
                            currentReading.water_data_Reading_List.RemoveRange(y, 2);
                        }
                        y--;
                        if (y < 1)
                            y = 1;

                        JumpNumber--;
                        if (JumpNumber == 0)
                            break;
                    }
                }
                if (y > valid_WaterDataPosition_In_Current_Reading)
                    break;
            }
            if (JumpNumber > 0)
            {
                for (int y = 1; y < currentReading.water_data_Reading_List.Count; y++)
                {
                    if (y >= currentReading.water_data_Reading_List.Count)
                        break;
                    if (Compute_Rate((Water_data)currentReading.water_data_Reading_List[y], considerZero))
                    {
                        if (Compute_Month_Number(currentReading.Reading_Date, y) > 7)
                        {
                            Water_data wd = new Water_data();
                            // find jump
                            wd.Water_Consumption = ((Water_data)currentReading.water_data_Reading_List[y - 1]).Water_Consumption;
                            wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y]).Water_Consumption;


                            wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y - 1]).Month_Maximum_Debi,
                            ((Water_data)currentReading.water_data_Reading_List[y]).Month_Maximum_Debi);

                            if (y < currentReading.water_data_Reading_List.Count - 1)
                            {
                                wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y + 1]).Month_Maximum_Debi,
                                    wd.Month_Maximum_Debi);
                                wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y + 1]).Water_Consumption;
                            }

                            wd.IsValidData = false;

                            if (!HighLow(wd))
                                continue;
                            RemoveJumpFromAllReadingData(currentReading.water_data_Reading_List[y - 1], currentReading.water_data_Reading_List[y], currentReading.water_data_Reading_List[y + 1], wd);

                            currentReading.water_data_Reading_List[y - 1] = wd;
                            if (y == currentReading.water_data_Reading_List.Count - 1)
                            {
                                currentReading.water_data_Reading_List.RemoveAt(y);
                            }
                            else
                            {
                                currentReading.water_data_Reading_List.RemoveRange(y, 2);
                            }
                            y--;
                            if (y < 1)
                                y = 1;

                            JumpNumber--;
                            if (JumpNumber == 0)
                                break;
                        }
                    }
                    if (y > valid_WaterDataPosition_In_Current_Reading)
                        break;
                }
            }
            int cPosition = valid_WaterDataPosition_In_Current_Reading - (23 - currentReading.water_data_Reading_List.Count);

            for (int i = 1; currentReading.water_data_Reading_List.Count < 23; i++)
            {
                if (cPosition + i >= currentReading.water_data_Reading_List.Count)
                {
                    if (valid_WaterDataPosition_In_Previous_Reading + i < previousReading.water_data_Reading_List.Count)
                    {
                        currentReading.water_data_Reading_List.Add(previousReading.water_data_Reading_List[valid_WaterDataPosition_In_Previous_Reading + i]);
                    }
                }

                else
                    currentReading.water_data_Reading_List[cPosition + i] =
                    previousReading.water_data_Reading_List[valid_WaterDataPosition_In_Previous_Reading + i];
            }
        }
        private void RemoveJump(ref Water_Data_Reading_Class currentReading, Water_Data_Reading_Class previousReading, bool considerZero,
            int valid_WaterDataPosition_In_Current_Reading, int JumpNumber, int valid_WaterDataPosition_In_Previous_Reading, int number_of_InValid_Data_withOut_Zero)
        {
            for (int y = 1; y < currentReading.water_data_Reading_List.Count; y++)
            {
                if (Compute_Rate((Water_data)currentReading.water_data_Reading_List[y], considerZero))
                {
                    if (Compute_Month_Number(currentReading.Reading_Date, y) < 8)
                    {
                        Water_data wd = new Water_data();
                        // find jump
                        wd.Water_Consumption = ((Water_data)currentReading.water_data_Reading_List[y - 1]).Water_Consumption;
                        wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y]).Water_Consumption;


                        wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y - 1]).Month_Maximum_Debi,
                        ((Water_data)currentReading.water_data_Reading_List[y]).Month_Maximum_Debi);

                        if (y < currentReading.water_data_Reading_List.Count - 1)
                        {
                            wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y + 1]).Month_Maximum_Debi,
                                wd.Month_Maximum_Debi);
                            wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y + 1]).Water_Consumption;
                        }

                        wd.IsValidData = false;

                        if (!HighLow(wd))
                            continue;
                        RemoveJumpFromAllReadingData(currentReading.water_data_Reading_List[y - 1], currentReading.water_data_Reading_List[y], currentReading.water_data_Reading_List[y + 1], wd);

                        currentReading.water_data_Reading_List[y - 1] = wd;
                        if (y == currentReading.water_data_Reading_List.Count - 1)
                        {
                            currentReading.water_data_Reading_List.RemoveAt(y);
                        }
                        else
                        {
                            currentReading.water_data_Reading_List.RemoveRange(y, 2);
                        }
                        y--;
                        if (y < 1)
                            y = 1;

                        JumpNumber--;
                        if (JumpNumber == 0)
                            break;
                    }
                }
            }
            if (JumpNumber > 0)
            {
                for (int y = 1; y < currentReading.water_data_Reading_List.Count; y++)
                {
                    if (Compute_Rate((Water_data)currentReading.water_data_Reading_List[y], considerZero))
                    {
                        if (Compute_Month_Number(currentReading.Reading_Date, y) > 7)
                        {
                            Water_data wd = new Water_data();
                            // find jump
                            wd.Water_Consumption = ((Water_data)currentReading.water_data_Reading_List[y - 1]).Water_Consumption;
                            wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y]).Water_Consumption;


                            wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y - 1]).Month_Maximum_Debi,
                            ((Water_data)currentReading.water_data_Reading_List[y]).Month_Maximum_Debi);

                            if (y < currentReading.water_data_Reading_List.Count - 1)
                            {
                                wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y + 1]).Month_Maximum_Debi,
                                    wd.Month_Maximum_Debi);
                                wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y + 1]).Water_Consumption;
                            }

                            wd.IsValidData = false;

                            if (!HighLow(wd))
                                continue;
                            RemoveJumpFromAllReadingData(currentReading.water_data_Reading_List[y - 1], currentReading.water_data_Reading_List[y], currentReading.water_data_Reading_List[y + 1], wd);

                            currentReading.water_data_Reading_List[y - 1] = wd;
                            if (y == currentReading.water_data_Reading_List.Count - 1)
                            {
                                currentReading.water_data_Reading_List.RemoveAt(y);
                            }
                            else
                            {
                                currentReading.water_data_Reading_List.RemoveRange(y, 2);
                            }
                            y--;
                            if (y < 1)
                                y = 1;

                            JumpNumber--;
                            if (JumpNumber == 0)
                                break;
                        }
                    }
                }
            }
            int cPosition = valid_WaterDataPosition_In_Current_Reading - (23 - currentReading.water_data_Reading_List.Count);

            for (int i = 1; currentReading.water_data_Reading_List.Count < 23; i++)
            {
                if (cPosition + i >= currentReading.water_data_Reading_List.Count)
                    currentReading.water_data_Reading_List.Add(previousReading.water_data_Reading_List[valid_WaterDataPosition_In_Previous_Reading + i]);

                else
                    currentReading.water_data_Reading_List[cPosition + i] =
                    previousReading.water_data_Reading_List[valid_WaterDataPosition_In_Previous_Reading + i];
            }
        }

        private Water_Data_Reading_Class RemoveJump(Water_Data_Reading_Class currentReading, Water_Data_Reading_Class previousReading, int Month_diff, int first_unvalid_data_Position)
        {
            int JumpNumber = (23 - Month_diff) / 2;
            double maxDebi = 0, maxConsume = 0;
            double total_Water_In_current_Reading = 0;
            int number_Of_invalid_Data_Without_Zero = 0, number_Of_invalid_Data_With_Zero = 0;
            Water_Data_Reading_Class tempReading = new Water_Data_Reading_Class();
            tempReading = currentReading;

            #region Find Maximum of Debi and Maximum of consumption
            for (int i = currentReading.water_data_Reading_List.Count - 1; i > -1; i--)
            {
                total_Water_In_current_Reading += ((Water_data)currentReading.water_data_Reading_List[i]).Water_Consumption;
                if (maxDebi < ((Water_data)currentReading.water_data_Reading_List[i]).Month_Maximum_Debi)
                    maxDebi = ((Water_data)currentReading.water_data_Reading_List[i]).Month_Maximum_Debi;
                if (Compute_Rate(((Water_data)currentReading.water_data_Reading_List[i]), false))
                {
                    number_Of_invalid_Data_Without_Zero++;
                }
                if (Compute_Rate(((Water_data)currentReading.water_data_Reading_List[i]), true))
                {
                    number_Of_invalid_Data_With_Zero++;
                }
            }
            double unSaved_Consumption = double.Parse(currentReading.TotalWater, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo)
                - double.Parse(previousReading.TotalWater, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo) 
                - total_Water_In_current_Reading + ((Water_data)previousReading.water_data_Reading_List[0]).Water_Consumption;
            #endregion

            #region Find Jump
            if (JumpNumber > 1)
            {
                if (JumpNumber <= number_Of_invalid_Data_Without_Zero)
                {
                    RemoveJump(ref tempReading, previousReading, false, JumpNumber);
                }
                else
                {
                    if (JumpNumber <= number_Of_invalid_Data_With_Zero)
                        RemoveJump(ref tempReading, previousReading, true, number_Of_invalid_Data_With_Zero);
                    else
                        return RemoveAllUnvalidData(currentReading, previousReading, Month_diff, first_unvalid_data_Position);
                }
                for (int i = 0; i < tempReading.water_data_Reading_List.Count; i++)
                {
                    if (maxConsume < ((Water_data)tempReading.water_data_Reading_List[i]).Water_Consumption)
                        maxConsume = ((Water_data)tempReading.water_data_Reading_List[i]).Water_Consumption;
                }
                if (tempReading.water_data_Reading_List.Count > Month_diff + 1)
                {
                    //  LogFile.WriteLOG(serial + " ( " + currentReading.Reading_Date + " ): " + "نمی توان همه پرشها را تشخیص داد ",false);
                    return RemoveAllUnvalidData(currentReading, previousReading, Month_diff, first_unvalid_data_Position);
                }

                if ((((Water_data)tempReading.water_data_Reading_List[tempReading.water_data_Reading_List.Count - 1]).Water_Consumption
                    + unSaved_Consumption) * 1000 / maxDebi < Time_Diff)
                {
                    if ((((Water_data)tempReading.water_data_Reading_List[tempReading.water_data_Reading_List.Count - 1]).Water_Consumption +
                        unSaved_Consumption) <= 1.2 * maxConsume)
                    {
                        if (tempReading.water_data_Reading_List.Count > Month_diff)
                        {
                            ((Water_data)tempReading.water_data_Reading_List[tempReading.water_data_Reading_List.Count - 1]).Water_Consumption
                                += Math.Round(unSaved_Consumption, 2);
                        }
                        else
                        {
                            Water_data wd = new Water_data();
                            wd.Water_Consumption = Math.Round(unSaved_Consumption, 2);
                            wd.Month_Maximum_Debi = maxDebi;
                            tempReading.water_data_Reading_List.Add(wd);
                        }
                        while (tempReading.water_data_Reading_List.Count < 23)
                        {
                            Water_data wd = new Water_data();
                            wd.IsValidData = false;
                            tempReading.water_data_Reading_List.Add(wd);
                        }
                        return tempReading;
                    }
                }
            }
            #endregion

            return RemoveAllUnvalidData(currentReading, previousReading, Month_diff, first_unvalid_data_Position);
        }

        private Water_Data_Reading_Class RemoveAllUnvalidData(Water_Data_Reading_Class currentReading, Water_Data_Reading_Class previousReading, int Month_Diff, int first_unvalid_data_Position)
        {
            double sum = 0;

            RemoveJump(ref currentReading, previousReading, true, 23);

            for (int y = 0; y < first_unvalid_data_Position; y++)
                sum += ((Water_data)currentReading.water_data_Reading_List[y]).Water_Consumption;
            double consumeWater = 0;
            consumeWater = (double.Parse(currentReading.TotalWater, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo)) - (double.Parse(previousReading.TotalWater, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo)) - sum
                + ((Water_data)previousReading.water_data_Reading_List[0]).Water_Consumption;

            while (currentReading.water_data_Reading_List.Count < 23)
            {
                Water_data wd = new Water_data();
                wd.IsValidData = false;
                currentReading.water_data_Reading_List.Add(wd);
            }

            for (int m = first_unvalid_data_Position; m < 23; m++)
            {
                ((Water_data)currentReading.water_data_Reading_List[m]).IsValidData = false; ;
                ((Water_data)currentReading.water_data_Reading_List[m]).Month_Maximum_Debi = -2;
                ((Water_data)currentReading.water_data_Reading_List[m]).Water_Consumption = -2;
            }

            ((Water_data)currentReading.water_data_Reading_List[first_unvalid_data_Position]).Month_Maximum_Debi = -5;
            ((Water_data)currentReading.water_data_Reading_List[first_unvalid_data_Position]).Water_Consumption = -5;

            ((Water_data)currentReading.water_data_Reading_List[(Month_Diff + first_unvalid_data_Position) / 2]).Month_Maximum_Debi = -3;
            ((Water_data)currentReading.water_data_Reading_List[(Month_Diff + first_unvalid_data_Position) / 2]).Water_Consumption = -3;
            return currentReading;
        }

        private void RemoveJump(ref Water_Data_Reading_Class currentReading, Water_Data_Reading_Class previousReading, bool considerZero, int JumpNumber)
        {
            int y = 1;
            #region Remove NonZero Invaid Data From First six months of the year
            for (y = 1; y < currentReading.water_data_Reading_List.Count; y++)
            {
                if (Compute_Rate((Water_data)currentReading.water_data_Reading_List[y], false))
                {
                    if (Compute_Month_Number(currentReading.Reading_Date, y) < 7)
                    {
                        if (y < currentReading.water_data_Reading_List.Count - 1)
                        {
                            Water_data wd = new Water_data();
                            wd.IsValidData = false;
                            wd.Water_Consumption = ((Water_data)currentReading.water_data_Reading_List[y]).Water_Consumption;
                            wd.Month_Maximum_Debi = ((Water_data)currentReading.water_data_Reading_List[y]).Month_Maximum_Debi;

                            wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y - 1]).Water_Consumption;
                            wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y + 1]).Water_Consumption;

                            wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y - 1]).Month_Maximum_Debi,
                             wd.Month_Maximum_Debi);

                            wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y + 1]).Month_Maximum_Debi,
                                wd.Month_Maximum_Debi);

                            if (!HighLow(wd))
                                continue;
                            RemoveJumpFromAllReadingData(currentReading.water_data_Reading_List[y - 1], currentReading.water_data_Reading_List[y], currentReading.water_data_Reading_List[y + 1], wd);

                            currentReading.water_data_Reading_List[y - 1] = wd;

                            currentReading.water_data_Reading_List.RemoveRange(y, 2);
                            JumpNumber--;
                            if (JumpNumber == 0)
                                return;
                        }
                        if (y == currentReading.water_data_Reading_List.Count - 1)
                        {
                            Water_data wd = new Water_data();
                            wd.IsValidData = false;
                            wd.Water_Consumption = ((Water_data)currentReading.water_data_Reading_List[y]).Water_Consumption;
                            wd.Month_Maximum_Debi = ((Water_data)currentReading.water_data_Reading_List[y]).Month_Maximum_Debi;

                            wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y - 1]).Water_Consumption;

                            wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y - 1]).Month_Maximum_Debi,
                             wd.Month_Maximum_Debi);

                            if (!HighLow(wd))
                                continue;
                            RemoveJumpFromAllReadingData(currentReading.water_data_Reading_List[y - 1], currentReading.water_data_Reading_List[y], currentReading.water_data_Reading_List[y + 1], wd);

                            currentReading.water_data_Reading_List[y - 1] = wd;

                            currentReading.water_data_Reading_List.RemoveRange(y, 1);
                            JumpNumber--;
                            if (JumpNumber == 0)
                                return;
                        }
                    }
                }
            }
            #endregion

            #region Remove NonZero Invaid Data From Second six months of the year
            for (y = 1; y < currentReading.water_data_Reading_List.Count; y++)
            {
                if (Compute_Rate((Water_data)currentReading.water_data_Reading_List[y], false))
                {
                    if (Compute_Month_Number(currentReading.Reading_Date, y) > 6)
                    {
                        if (y < currentReading.water_data_Reading_List.Count - 1)
                        {
                            Water_data wd = new Water_data();
                            wd.IsValidData = false;
                            wd.Water_Consumption = ((Water_data)currentReading.water_data_Reading_List[y]).Water_Consumption;
                            wd.Month_Maximum_Debi = ((Water_data)currentReading.water_data_Reading_List[y]).Month_Maximum_Debi;

                            wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y - 1]).Water_Consumption;
                            wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y + 1]).Water_Consumption;

                            wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y - 1]).Month_Maximum_Debi,
                             wd.Month_Maximum_Debi);

                            wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y + 1]).Month_Maximum_Debi,
                                wd.Month_Maximum_Debi);

                            if (!HighLow(wd))
                                continue;
                            RemoveJumpFromAllReadingData(currentReading.water_data_Reading_List[y - 1], currentReading.water_data_Reading_List[y], currentReading.water_data_Reading_List[y + 1], wd);



                            currentReading.water_data_Reading_List[y - 1] = wd;

                            currentReading.water_data_Reading_List.RemoveRange(y, 2);
                            JumpNumber--;
                            if (JumpNumber == 0)
                                return;
                        }
                        if (y == currentReading.water_data_Reading_List.Count - 1)
                        {
                            Water_data wd = new Water_data();
                            wd.IsValidData = false;
                            wd.Water_Consumption = ((Water_data)currentReading.water_data_Reading_List[y]).Water_Consumption;
                            wd.Month_Maximum_Debi = ((Water_data)currentReading.water_data_Reading_List[y]).Month_Maximum_Debi;

                            wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y - 1]).Water_Consumption;

                            wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y - 1]).Month_Maximum_Debi,
                             wd.Month_Maximum_Debi);

                            if (!HighLow(wd))
                                continue;
                            RemoveJumpFromAllReadingData(currentReading.water_data_Reading_List[y - 1], currentReading.water_data_Reading_List[y], currentReading.water_data_Reading_List[y + 1], wd);

                            currentReading.water_data_Reading_List[y - 1] = wd;

                            currentReading.water_data_Reading_List.RemoveRange(y, 1);
                            JumpNumber--;
                            if (JumpNumber == 0)
                                return;
                        }
                    }
                }
            }
            #endregion

            #region Remove Zero Invalid Data From First six months of the year
            y = 1;
            while (y < currentReading.water_data_Reading_List.Count && JumpNumber > 0)
            {
                if (Compute_Rate((Water_data)currentReading.water_data_Reading_List[y], true))
                {
                    if (Compute_Month_Number(currentReading.Reading_Date, y) < 6)
                    {
                        if (y < currentReading.water_data_Reading_List.Count - 1)
                        {
                            Water_data wd = new Water_data();
                            // find jump
                            wd.Water_Consumption = ((Water_data)currentReading.water_data_Reading_List[y - 1]).Water_Consumption;
                            wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y]).Water_Consumption;
                            wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y + 1]).Water_Consumption;

                            wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y - 1]).Month_Maximum_Debi,
                            ((Water_data)currentReading.water_data_Reading_List[y]).Month_Maximum_Debi);

                            wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y + 1]).Month_Maximum_Debi,
                                wd.Month_Maximum_Debi);
                            wd.IsValidData = false;

                            if (!HighLow(wd))
                            {
                                y++;
                                continue;
                            }

                            RemoveJumpFromAllReadingData(currentReading.water_data_Reading_List[y - 1], currentReading.water_data_Reading_List[y], currentReading.water_data_Reading_List[y + 1], wd);

                            currentReading.water_data_Reading_List[y - 1] = wd;

                            currentReading.water_data_Reading_List.RemoveRange(y, 2);
                            JumpNumber--;
                            if (JumpNumber == 0)
                                break;
                        }
                        if (y == currentReading.water_data_Reading_List.Count - 1)
                        {
                            Water_data wd = new Water_data();
                            // find jump
                            wd.Water_Consumption = ((Water_data)currentReading.water_data_Reading_List[y - 1]).Water_Consumption;
                            wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y]).Water_Consumption;

                            wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y - 1]).Month_Maximum_Debi,
                            ((Water_data)currentReading.water_data_Reading_List[y]).Month_Maximum_Debi);

                            wd.IsValidData = false;

                            if (!HighLow(wd))
                            {
                                y++;
                                continue;
                            }
                            RemoveJumpFromAllReadingData(currentReading.water_data_Reading_List[y - 1], currentReading.water_data_Reading_List[y], currentReading.water_data_Reading_List[y + 1], wd);

                            currentReading.water_data_Reading_List[y - 1] = wd;

                            currentReading.water_data_Reading_List.RemoveRange(y, 1);
                            JumpNumber--;
                            if (JumpNumber == 0)
                                return;
                        }

                    }
                }
                y++;
            }
            #endregion

            #region Remove Zero Invalid Data From Second six months of the year
            y = 1;
            while (y < currentReading.water_data_Reading_List.Count && JumpNumber > 0)
            {
                if (Compute_Rate((Water_data)currentReading.water_data_Reading_List[y], true))
                {
                    if (Compute_Month_Number(currentReading.Reading_Date, y) > 6)
                    {
                        if (y < currentReading.water_data_Reading_List.Count - 1)
                        {
                            Water_data wd = new Water_data();
                            // find jump
                            wd.Water_Consumption = ((Water_data)currentReading.water_data_Reading_List[y - 1]).Water_Consumption;
                            wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y]).Water_Consumption;
                            wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y + 1]).Water_Consumption;

                            wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y - 1]).Month_Maximum_Debi,
                            ((Water_data)currentReading.water_data_Reading_List[y]).Month_Maximum_Debi);

                            wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y + 1]).Month_Maximum_Debi,
                                wd.Month_Maximum_Debi);
                            wd.IsValidData = false;

                            if (!HighLow(wd))
                            {
                                y++;
                                continue;
                            }
                            RemoveJumpFromAllReadingData(currentReading.water_data_Reading_List[y - 1], currentReading.water_data_Reading_List[y], currentReading.water_data_Reading_List[y + 1], wd);

                            currentReading.water_data_Reading_List[y - 1] = wd;

                            currentReading.water_data_Reading_List.RemoveRange(y, 2);
                            JumpNumber--;
                            if (JumpNumber == 0)
                                return;
                        }
                        if (y == currentReading.water_data_Reading_List.Count - 1)
                        {
                            Water_data wd = new Water_data();
                            // find jump
                            wd.Water_Consumption = ((Water_data)currentReading.water_data_Reading_List[y - 1]).Water_Consumption;
                            wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y]).Water_Consumption;

                            wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y - 1]).Month_Maximum_Debi,
                            ((Water_data)currentReading.water_data_Reading_List[y]).Month_Maximum_Debi);

                            wd.IsValidData = false;

                            if (!HighLow(wd))
                            {
                                y++;
                                continue;
                            }
                            RemoveJumpFromAllReadingData(currentReading.water_data_Reading_List[y - 1], currentReading.water_data_Reading_List[y], currentReading.water_data_Reading_List[y + 1], wd);

                            currentReading.water_data_Reading_List[y - 1] = wd;

                            currentReading.water_data_Reading_List.RemoveRange(y, 1);
                            JumpNumber--;
                            if (JumpNumber == 0)
                                return;
                        }

                    }
                }
                y++;
            }
            #endregion
        }


      
        private void Insert_PreviousReading_to_currentReading(Water_Data_Reading_Class currentReading, Water_Data_Reading_Class previousReading, int valid_WaterDataPosition_In_Previous_Reading, int position)
        {
            int start = position - valid_WaterDataPosition_In_Previous_Reading;
            int index_In_Orginal_Previous_Reading = 0;
            Water_Data_Reading_Class orginal_Previous_Reading = Get_Orginal_Previous_Reading(previousReading);
            for (index_In_Orginal_Previous_Reading = 0; index_In_Orginal_Previous_Reading < orginal_Previous_Reading.water_data_Reading_List.Count; index_In_Orginal_Previous_Reading++)
            {
                if (
                    ((Water_data)currentReading.water_data_Reading_List[position]).IsValidData == ((Water_data)orginal_Previous_Reading.water_data_Reading_List[index_In_Orginal_Previous_Reading]).IsValidData
                    && ((Water_data)currentReading.water_data_Reading_List[position]).Month_Maximum_Debi == ((Water_data)orginal_Previous_Reading.water_data_Reading_List[index_In_Orginal_Previous_Reading]).Month_Maximum_Debi
                    && ((Water_data)currentReading.water_data_Reading_List[position]).Water_Consumption == ((Water_data)orginal_Previous_Reading.water_data_Reading_List[index_In_Orginal_Previous_Reading]).Water_Consumption
                    )
                    break;
            }
            if (index_In_Orginal_Previous_Reading < 23)
            {
                int k = 1;
                for (int i = position - index_In_Orginal_Previous_Reading + 1; i < 23; i++)
                {
                    currentReading.water_data_Reading_List[i] = previousReading.water_data_Reading_List[k];
                    k++;
                    if (k > 23)
                        break;
                }
            }
        }

        private void Remove_Zero_Consumption(ref Water_Data_Reading_Class currentReading, Water_Data_Reading_Class previousReading, int valid_WaterDataPosition_In_Current_Reading, int JumpNumber, int valid_WaterDataPosition_In_Previous_Reading)
        {
            try
            {
                for (int y = 1; y < valid_WaterDataPosition_In_Current_Reading - 2; y++)
                {
                    if (currentReading.water_data_Reading_List.Count <= y)
                        break;
                    if (Compute_Rate((Water_data)currentReading.water_data_Reading_List[y], true))
                    {
                        if (Compute_Month_Number(currentReading.Reading_Date, y) < 6)
                        {
                            Water_data wd = new Water_data();
                            // find jump
                            wd.Water_Consumption = ((Water_data)currentReading.water_data_Reading_List[y - 1]).Water_Consumption;
                            wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y]).Water_Consumption;
                            wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y + 1]).Water_Consumption;

                            wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y - 1]).Month_Maximum_Debi,
                            ((Water_data)currentReading.water_data_Reading_List[y]).Month_Maximum_Debi);

                            wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y + 1]).Month_Maximum_Debi,
                                wd.Month_Maximum_Debi);
                            wd.IsValidData = false;

                            if (!HighLow(wd))
                                continue;
                            RemoveJumpFromAllReadingData(currentReading.water_data_Reading_List[y - 1], currentReading.water_data_Reading_List[y], currentReading.water_data_Reading_List[y + 1], wd);

                            currentReading.water_data_Reading_List[y - 1] = wd;

                            currentReading.water_data_Reading_List.RemoveRange(y, 2);
                            JumpNumber--;
                            y--;
                            if (y < 1) y = 1;
                            if (JumpNumber == 0)
                                break;
                        }
                    }
                }
                if (JumpNumber > 0)
                {
                    for (int y = 1; y < valid_WaterDataPosition_In_Current_Reading - 2; y++)
                    {
                        if (Compute_Rate((Water_data)currentReading.water_data_Reading_List[y], true))
                        {
                            Water_data wd = new Water_data();
                            // find jump
                            wd.Water_Consumption = ((Water_data)currentReading.water_data_Reading_List[y - 1]).Water_Consumption;
                            wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y]).Water_Consumption;
                            wd.Water_Consumption += ((Water_data)currentReading.water_data_Reading_List[y + 1]).Water_Consumption;

                            wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y - 1]).Month_Maximum_Debi,
                            ((Water_data)currentReading.water_data_Reading_List[y]).Month_Maximum_Debi);

                            wd.Month_Maximum_Debi = Math.Max(((Water_data)currentReading.water_data_Reading_List[y + 1]).Month_Maximum_Debi,
                                wd.Month_Maximum_Debi);
                            int zerocnt = 0;
                            if (((Water_data)currentReading.water_data_Reading_List[y + 1]).Water_Consumption == 0)
                                zerocnt++;
                            if (((Water_data)currentReading.water_data_Reading_List[y ]).Water_Consumption == 0)
                                zerocnt++;
                            if (((Water_data)currentReading.water_data_Reading_List[y - 1]).Water_Consumption == 0)
                                zerocnt++;
                            if (zerocnt != 2)
                                wd.IsValidData = false;
                            else
                                wd.IsValidData = true;

                            if (!HighLow(wd))
                                continue;
                            RemoveJumpFromAllReadingData(currentReading.water_data_Reading_List[y - 1], currentReading.water_data_Reading_List[y], currentReading.water_data_Reading_List[y + 1], wd);

                            currentReading.water_data_Reading_List[y - 1] = wd;

                            currentReading.water_data_Reading_List.RemoveRange(y, 2);
                            JumpNumber--;
                            y--;
                            if (y < 1) y = 1;
                            if (JumpNumber == 0)
                                break;

                        }
                    }
                }
                int cPosition = valid_WaterDataPosition_In_Current_Reading - (23 - currentReading.water_data_Reading_List.Count);

                for (int i = 1; currentReading.water_data_Reading_List.Count < 23; i++)
                {
                    if (cPosition + i >= currentReading.water_data_Reading_List.Count)
                        currentReading.water_data_Reading_List.Add(previousReading.water_data_Reading_List[valid_WaterDataPosition_In_Previous_Reading + i]);

                    else
                        currentReading.water_data_Reading_List[cPosition + i] =
                        previousReading.water_data_Reading_List[valid_WaterDataPosition_In_Previous_Reading + i];
                }
            }
            catch (Exception ex)
            {
            }
        }

       

              private int Compute_Month_Number(string Reading_Date, int mm)
        {
            try
            {
                PersianDate persianDate = new PersianDate();
                persianDate.ConvertToPersianDate(Reading_Date);

                if (persianDate.Month - mm == 0)
                {
                    return 12;
                }
                else if (persianDate.Month - mm < 0)
                {
                    int month = 12 + persianDate.Month - mm;
                    return month;
                }

                return persianDate.Month - mm;
            }
            catch
            {
                return -1;
            }
        }

        private bool HighLow(Water_data wd)
        {
            if (wd.Water_Consumption == 0)
            {
                if (wd.Month_Maximum_Debi == 0)
                    return true;
                else
                {
                    // LogFile.WriteLOG(serial + "  مصرف صفر با دبی بیشتر از صفر", true);    
                    return true;
                }
            }
            if (wd.Month_Maximum_Debi == 0)
            {
                // LogFile.WriteLOG(serial + "  مصرف غیر صفر با دبی   صفر", true);    
                return true;
            }

            double res = (wd.Water_Consumption * 1000) / (wd.Month_Maximum_Debi * 3600 * 24);
            if (res < 100)
            {
                return true;
            }
            else
            {
                // LogFile.WriteLOG(serial+"روزهای مصرف" +" = "+res+ "  مصرف= "+wd.Month_Water_Consumption.ToString(), true);    
                return false;
            }
        }

        
       


        private int Compute_Index_of_Valid_Zero_Consumption(Water_Data_Reading_Class previousReading)
        {
            int Index_Of_Valid_Zero = -1;
            for (int i = previousReading.water_data_Reading_List.Count - 1; i > 1; i--)
            {
                if (((Water_data)previousReading.water_data_Reading_List[i]).Water_Consumption == -2)
                    return -1;
            }
            for (int i = previousReading.water_data_Reading_List.Count - 1; i > 1; i--)
            {
                if (((Water_data)previousReading.water_data_Reading_List[i]).Water_Consumption == 0)
                    if (((Water_data)previousReading.water_data_Reading_List[i - 1]).Water_Consumption == 0)
                        if (((Water_data)previousReading.water_data_Reading_List[i - 2]).Water_Consumption != 0)
                        {
                            Index_Of_Valid_Zero = i - 1;
                            break;
                        }
                if (((Water_data)previousReading.water_data_Reading_List[i]).Water_Consumption != 0)
                    if (((Water_data)previousReading.water_data_Reading_List[i - 1]).Water_Consumption != 0)
                    {
                        Index_Of_Valid_Zero = -2;
                        break;
                    }

            }

            if (Index_Of_Valid_Zero == -1)
                if (((Water_data)previousReading.water_data_Reading_List[previousReading.water_data_Reading_List.Count - 1]).Water_Consumption == 0)
                    if (((Water_data)previousReading.water_data_Reading_List[previousReading.water_data_Reading_List.Count - 2]).Water_Consumption == 0)
                        if (((Water_data)previousReading.water_data_Reading_List[previousReading.water_data_Reading_List.Count - 3]).Water_Consumption == 0)
                            Index_Of_Valid_Zero = previousReading.water_data_Reading_List.Count - 3;

            return Index_Of_Valid_Zero;
        }

        private int Find_Nearest_Valid_Water_Data(Water_Data_Reading_Class previousReading, int Index_Of_Valid_Zero, ref Water_data nearest_Valid_Water_Data)
        {
            nearest_Valid_Water_Data.IsValidData = false;
            int valid_WaterDataPosition_In_previousReading = 0;
            for (valid_WaterDataPosition_In_previousReading = 0; valid_WaterDataPosition_In_previousReading < previousReading.water_data_Reading_List.Count;
                valid_WaterDataPosition_In_previousReading++)
            {
                if (!((Water_data)previousReading.water_data_Reading_List[valid_WaterDataPosition_In_previousReading]).IsValidData)
                    continue;
                if (((Water_data)previousReading.water_data_Reading_List[valid_WaterDataPosition_In_previousReading]).Water_Consumption == -2)
                    return -1;
                // داده های غیر صفر معتبر پیدا شود
                if (!Compute_Rate(((Water_data)previousReading.water_data_Reading_List[valid_WaterDataPosition_In_previousReading]), false))
                {

                    if (((Water_data)previousReading.water_data_Reading_List[valid_WaterDataPosition_In_previousReading]).Water_Consumption > 0)
                    {
                        // اگر داده مشابه پیدا کردیم داده های جدیدتر را بررسی می کنیم. معتبر بودنش اهمیت ندارد
                        //if (((Water_data)previousReading.water_data_Reading_List[valid_WaterDataPosition_In_previousReading]).IsValidData)                        
                        {
                            nearest_Valid_Water_Data = ((Water_data)previousReading.water_data_Reading_List[valid_WaterDataPosition_In_previousReading]);
                            return valid_WaterDataPosition_In_previousReading;
                        }
                    }
                    
                }
            }
            // آب مصرفی صفر هم داده معتبر در نظر گرفته شد
            for (valid_WaterDataPosition_In_previousReading = 1; valid_WaterDataPosition_In_previousReading < previousReading.water_data_Reading_List.Count;
                valid_WaterDataPosition_In_previousReading++)
            {
                if (((Water_data)previousReading.water_data_Reading_List[valid_WaterDataPosition_In_previousReading]).Water_Consumption == -2)
                    return -1;
                if (!Compute_Rate(((Water_data)previousReading.water_data_Reading_List[valid_WaterDataPosition_In_previousReading]), false))
                {
                     if (((Water_data)previousReading.water_data_Reading_List[valid_WaterDataPosition_In_previousReading]).Water_Consumption == 0)
                    {
                        if (valid_WaterDataPosition_In_previousReading == Index_Of_Valid_Zero)
                        {
                            nearest_Valid_Water_Data = ((Water_data)previousReading.water_data_Reading_List[valid_WaterDataPosition_In_previousReading]);
                            return valid_WaterDataPosition_In_previousReading;
                        }
                    }
                }
            }
            return -1;
        }

        private bool Compute_Rate(Water_data wd, bool considerZero)
        {
            if (considerZero && wd.Month_Maximum_Debi == 0 && wd.Water_Consumption == 0)
                return true;


            if (wd.Water_Consumption * 1000 / wd.Month_Maximum_Debi < 300)
            {
                return true;
            }
            return false;
        }

        private bool VerifyDate(string currentDate)
        {
            try
            {
                //  char ch = '/';
                //  string[] d1 = currentDate.Split(ch);

                //  int y = Convert.ToInt16(d1[0]);

                //   int m = Convert.ToInt16(d1[1]);
                //  int d = Convert.ToInt16(d1[2]);

                //if (!(y <= 98 && y > 80))
                //    return false;

                //if (m < 13 && m > 0)
                //{
                //    if (m < 7)
                //        if (!(d < 32 && d > 0))
                //            return false;
                //    if (m > 6)
                //        if (!(d < 31 && d > 0))
                //            return false;
                //}
                //else
                //    return false;
                
                
                PersianDate persianDate = new PersianDate();
                persianDate.ConvertToPersianDate(currentDate);

                PersianDate now = new PersianDate(DateTime.Now);
                if (persianDate.Year > now.Year && persianDate.Year < 1380)
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
            return false;
        }

        private int DiffOfShamsiDate(string previousDate, string currentDate)
        {
            int dif = 0;
            //char ch = '/';
            //string[] d1 = currentDate.Split(ch);
            //string[] d0 = previousDate.Split(ch);
            //int yy1 = Convert.ToInt16(d1[0]);
            //int yy0 = Convert.ToInt16(d0[0]);

            //int mm1 = Convert.ToInt16(d1[1]);
            //int mm0 = Convert.ToInt16(d0[1]);

            //int dd1 = Convert.ToInt16(d1[2]);
            //int dd0 = Convert.ToInt16(d0[2]);
            //if (yy1 > yy0)
            //    dif = 12 * (yy1 - yy0 - 1) + (12 - mm0) + mm1;
            //else
            //    dif = mm1 - mm0;

            //return dif;

            try
            {
                PersianDate previousDateTime = new PersianDate();
                previousDateTime.ConvertToPersianDate(previousDate);

                PersianDate currentDateTime = new PersianDate();
                currentDateTime.ConvertToPersianDate(currentDate);

                var dt1 = previousDateTime.ConvertToGeorgianDateTime();
                var dt2 = currentDateTime.ConvertToGeorgianDateTime();
                
                
                if (dt2.Year > dt1.Year)
                    dif = 12 * (dt2.Year - dt1.Year - 1) + (12 - dt1.Month) + dt2.Month;
                else
                    dif = dt2.Month - dt1.Month;
                if (dt1.Day < 20 && dt2.Day > 19)
                    dif++;
                if (dt1.Day>= 20  && dt2.Day < 19)
                    dif--;

            }
            catch(System.Exception)
            {

            }
            
            return dif;
         
        }

        private int Search_Nearest_Common_Valid_Data(Water_data valid_water_Data, int valid_data_Position, Water_Data_Reading_Class currentReading, int MonthDiff, ref int position, Water_Data_Reading_Class previousReading)
        {
            List<Water_data> currentReadingConsumedData = currentReading.water_data_Reading_List;
            int currentCounter = -1;
            int pos = -1;
            for (currentCounter = MonthDiff + valid_data_Position; currentCounter < currentReadingConsumedData.Count; currentCounter++)
            {
                if (valid_water_Data.Water_Consumption == ((Water_data)currentReadingConsumedData[currentCounter]).Water_Consumption                     )
                    return currentCounter;
            }
            if (currentCounter == 23 && valid_water_Data.Water_Consumption > 0)
            {
                // موقیعت داده معتبر قبلی در قرائت جدید
                for (pos = 0; pos < MonthDiff + valid_data_Position; pos++)
                {
                    if (valid_water_Data.Water_Consumption == ((Water_data)currentReadingConsumedData[pos]).Water_Consumption                   )
                    {
                        position = pos;
                        break;
                    }
                }
            }
            if (pos == -1)
            {
                for (currentCounter = 0; currentCounter < currentReadingConsumedData.Count; currentCounter++)
                {
                    if (valid_water_Data.Water_Consumption == ((Water_data)currentReadingConsumedData[currentCounter]).Water_Consumption  )
                        return currentCounter;
                }
            }
            
            if (pos == -1)
                return -1;
            if (MonthDiff + valid_data_Position > position)
            {
                currentReading.Error = "-3";
                //Vee_207.hasError = true;
                // احتمالا کنتور خاموش بوده است و یا اینکه ساعت تغییر کرده است.
                // اگر فقط یک کپچر اضافه بود ساعت تغییر کرده 
                //MonthDiff+valid_data_Position داده معتبر باید در موقعیت  قرار بگیرد 
                if (MonthDiff == 1)
                {
                    if (_listTransversal - 2 >= 0)
                    {
                        var nextReading = (Water_Data_Reading_Class)_allReadingDdata[_listTransversal - 2];
                        for (int i = 0; i < nextReading.water_data_Reading_List.Count-1; i++)
                        {
                            if (nextReading.water_data_Reading_List[i].Water_Consumption == currentReading.water_data_Reading_List[0].Water_Consumption &&
                                nextReading.water_data_Reading_List[i].Month_Maximum_Debi == currentReading.water_data_Reading_List[0].Month_Maximum_Debi &&
                                nextReading.water_data_Reading_List[i + 1].Water_Consumption == currentReading.water_data_Reading_List[1].Water_Consumption &&
                                nextReading.water_data_Reading_List[i + 1].Month_Maximum_Debi == currentReading.water_data_Reading_List[1].Month_Maximum_Debi
                                )
                            {
                                nextReading.water_data_Reading_List.RemoveAt(i);
                                nextReading.water_data_Reading_List.Insert(23, new Water_data("0", "0", false));
                                currentReadingConsumedData.Insert(position, currentReading.water_data_Reading_List[0]);
                                currentReadingConsumedData.RemoveAt(0);
                                currentReadingConsumedData.Insert(0, new Water_data("0", "0", false));
                                
                                return -3;
                            }
                        }                        
                        
                    }
                }
                for (int i = 0; i <MonthDiff+valid_data_Position- position; i++)
                {
                    if (position == 1)
                    {
                        var d = currentReadingConsumedData[position].GetWaterData();
                        d.IsValidData = false;

                        currentReadingConsumedData.Insert(position, d);
                    }
                    else 
                    {
                        var d = currentReadingConsumedData[0].GetWaterData();
                        d.IsValidData = false;
                         
                        currentReadingConsumedData.Insert(1, d); 
                    }


                }
                return -3;
            }
            if (pos != MonthDiff + valid_data_Position) // Data&Time of Meter has changed
                return -2;
            return -1;
        }

        
        private int Search_Nearest_Common_Data(Water_Data_Reading_Class previousReading, ref Water_data valid_water_Data, ref int valid_data_Position,
             Water_Data_Reading_Class currentReading, int MonthDiff, ref int position)
        {

            int currentCounter = -1;
            int pos = -1;
            var  currentReadingConsumedList = currentReading.water_data_Reading_List;
            if (valid_water_Data.Water_Consumption == 0)
            {
                valid_data_Position = 1;
                valid_water_Data = previousReading.water_data_Reading_List[1];
            }
            
            if (valid_water_Data.Water_Consumption == 0)
            {
                int k = 1;
                while (valid_water_Data.Water_Consumption == 0)
                {
                    valid_water_Data = previousReading.water_data_Reading_List[k++];
                }
            }

            for (currentCounter = MonthDiff + valid_data_Position; currentCounter < currentReadingConsumedList.Count; currentCounter++)
            {
                if (valid_water_Data.Water_Consumption == ((Water_data)currentReadingConsumedList[currentCounter]).Water_Consumption &&
                    valid_water_Data.Month_Maximum_Debi == ((Water_data)currentReadingConsumedList[currentCounter]).Month_Maximum_Debi)
                    return currentCounter;
            }
            for (currentCounter = MonthDiff + valid_data_Position; currentCounter < currentReadingConsumedList.Count; currentCounter++)
            {
                if (valid_water_Data.Water_Consumption == ((Water_data)currentReadingConsumedList[currentCounter]).Water_Consumption &&
                    valid_water_Data.Month_Maximum_Debi == ((Water_data)currentReadingConsumedList[currentCounter]).Month_Maximum_Debi)
                    return currentCounter;
            }

            // داده اولین ماه قرائت قبل در قرائت جدید وجود ندارد؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟
            if (currentCounter == 23 && valid_water_Data.Water_Consumption > 0)
            {
                // موقیعت داده معتبر قبلی در قرائت جدید
                for (pos = 0; pos < MonthDiff + valid_data_Position; pos++)
                {
                    if (valid_water_Data.Water_Consumption == ((Water_data)currentReadingConsumedList[pos]).Water_Consumption &&
                   valid_water_Data.Month_Maximum_Debi == ((Water_data)currentReadingConsumedList[pos]).Month_Maximum_Debi)
                    {
                        position = pos;
                        break;
                    }
                }
            }
            if (pos == -1)
            {
                for (currentCounter = 0; currentCounter < currentReadingConsumedList.Count; currentCounter++)
                {
                    if (valid_water_Data.Water_Consumption == ((Water_data)currentReadingConsumedList[currentCounter]).Water_Consumption &&
                        valid_water_Data.Month_Maximum_Debi == ((Water_data)currentReadingConsumedList[currentCounter]).Month_Maximum_Debi)
                        return currentCounter;
                }
            }

            if (pos == -1)
                return -1;


            if (MonthDiff + valid_data_Position > position)
            {
               // Vee_207.hasError = true;
                currentReading.Error = "-3";
                // احتمالا کنتور خاموش بوده است و یا اینکه ساعت تغییر کرده است.
                // اگر فقط یک کپچر اضافه بود ساعت تغییر کرده 
                //MonthDiff+valid_data_Position داده معتبر باید در موقعیت  قرار بگیرد 

                if (valid_data_Position == 1)
                {
                    for (int i = 0; i < MonthDiff + valid_data_Position - position; i++)
                    {
                        if (position == 1)
                        {
                            currentReadingConsumedList.Insert(position, new Water_data("0", "0", false));
                        }
                        else
                            currentReadingConsumedList.Insert(2, new Water_data("0", "0", false));


                    }
                }
                else
                {
                    var temp = new List<Water_data>(previousReading.water_data_Reading_List.Select(x => x.GetWaterData()));
                    temp[0] = currentReading.water_data_Reading_List[0].GetWaterData();
                    int validZeroNumber = 1, inValidZeroNumber = 0; 
                    for (int i = 1; i < valid_data_Position; i++)
                    {
                        if (temp[i].Water_Consumption == 0)
                        {
                            if (temp[i].IsValidData)
                                validZeroNumber++;
                            else
                                inValidZeroNumber++;
                        }
                    }
                    if (validZeroNumber > 1)
                    {
                        for (int i = position - validZeroNumber; i > 0; i--)
                        {
                            temp.Insert(1, currentReadingConsumedList[i]);
                        }
                        for (int i = 0; i < position - validZeroNumber - MonthDiff; i++)
                        {
                            if (position == 1)
                            {
                                temp.Insert(position, new Water_data("0", "0", false));
                            }
                            else
                                temp.Insert(2, new Water_data("0", "0", false));


                        }
                    }

                    else
                    {
                        if (position-1 == MonthDiff)
                        {
                            for (int i = MonthDiff; i >0; i--)
                            {
                                temp.Insert(1, currentReadingConsumedList[i]);
                            }
                        }
                        else if (position-1 < MonthDiff)
                        {
                            
                        }
                        else 
                        {
                            for (int i = position-1; i >0; i--)
                            {
                                for (int k = 1; k < valid_data_Position; k++)
                                {
                                    if (temp[k].Water_Consumption == currentReading.water_data_Reading_List[i].Water_Consumption &&
                                        temp[k].Month_Maximum_Debi == currentReading.water_data_Reading_List[i].Month_Maximum_Debi &&
                                        temp[k].IsValidData == currentReading.water_data_Reading_List[i].IsValidData
                                        )
                                    {
                                        //داده مورد نظر در قرائت قبلی هم وجود دارد
                                        // ممکنه همه اطلاعات صفر باشه و به جاش یک صفرداشته باشیم 
                                        i--;
                                        break;
                                    }
                                }
                                temp.Insert(2, currentReading.water_data_Reading_List[i]);
                            }
                        }
                        // جاش رو پیدا کن
                        for (int i = 0; i < MonthDiff + valid_data_Position - position-inValidZeroNumber; i++)
                        {
                            if (position == 1)
                            {
                                temp.Insert(position, new Water_data("0", "0", false));
                            }
                            else
                                temp.Insert(2, new Water_data("0", "0", false));


                        }
                    }
                    currentReading.water_data_Reading_List = temp;

                }
                return -3;
            }
            if (pos != MonthDiff + valid_data_Position) // Data&Time of Meter has changed
                return -2;
            return -1;
        }




       void RemoveJumpFromAllReadingData(Water_data w1, Water_data w2, Water_data w3, Water_data newData)
        { 
             
            for (int n = _listTransversal-1; n > 0; n--)
            {
                for (int j = 0; j < _allReadingDdata[n].water_data_Reading_List.Count; j++)
                {
                    if (_allReadingDdata[n].water_data_Reading_List[j].Water_Consumption == w2.Water_Consumption && 
                        _allReadingDdata[n].water_data_Reading_List[j].Month_Maximum_Debi == w2.Month_Maximum_Debi)
                    {
                        if (w3 != null && w1 != null)
                        {
                            if (j == 0)
                                continue;
                            if (_allReadingDdata[n].water_data_Reading_List[j - 1].Water_Consumption == w3.Water_Consumption &&
                             _allReadingDdata[n].water_data_Reading_List[j - 1].Month_Maximum_Debi == w3.Month_Maximum_Debi &&
                             _allReadingDdata[n].water_data_Reading_List[j + 1].Water_Consumption == w1.Water_Consumption &&
                             _allReadingDdata[n].water_data_Reading_List[j + 1].Month_Maximum_Debi == w1.Month_Maximum_Debi
                             )
                            {
                                _allReadingDdata[n].water_data_Reading_List[j - 1] = newData;
                                _allReadingDdata[n].water_data_Reading_List.RemoveRange(j, 2);
                            }
                        }
                        else if (w3 != null)
                        {
                            if (j == 0)
                                continue;
                            if (_allReadingDdata[n].water_data_Reading_List[j - 1].Water_Consumption == w3.Water_Consumption &&
                             _allReadingDdata[n].water_data_Reading_List[j - 1].Month_Maximum_Debi == w3.Month_Maximum_Debi)
                            {
                                _allReadingDdata[n].water_data_Reading_List[j + 1] = newData;
                                _allReadingDdata[n].water_data_Reading_List.RemoveRange(j, 1);
                            }
                        }
                        else if (w1 != null)
                        {
                            if (_allReadingDdata[n].water_data_Reading_List[j + 1].Water_Consumption == w1.Water_Consumption &&
                             _allReadingDdata[n].water_data_Reading_List[j + 1].Month_Maximum_Debi == w1.Month_Maximum_Debi)
                            {
                                if (j == 0)
                                {
                                    continue;
                                }

                                 
                                _allReadingDdata[n].water_data_Reading_List[j - 1] = newData;
                                _allReadingDdata[n].water_data_Reading_List.RemoveRange(j, 1);
                            }
                        
                        }
                        else
                        {

                        }

                    }
                }
            }
        }





    }
}
