using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeterStatus
{
    public enum MeterType
    {
        _207, _303
    }
    public class StatusContext
    {

        internal static string GetContext(int messageID, string language, MeterType meterType)
        {
            switch (language)
            {
                case "Farsi":
                    if (meterType == MeterType._303)
                        return PersianContext_303(messageID);
                    else
                        return PersianContext_207(messageID);
                case "English":
                    if (meterType == MeterType._303)
                        return EnglishContext_303(messageID);
                    else
                        return EnglishContext_207(messageID);

                default:
                    break;
            }
            return string.Empty;
        }

        private static string EnglishContext_207(int messageID)
        {           
            switch (messageID)
            {
                case 0:
                    return "Information does not accessible.";//"اطلاعات موردنظر در دسترس نیست";

                case 1:
                    return "Battery is Out";
                case 2:
                    return "Credit is Expired";
                case 3:
                    return "Pump Performance is Low";
                case 4:
                    return "Meter Misusing";
                case 5:
                    return "Relay Disconnects Completely";
                case 6:
                    return "Relay Disconnects  When Turn On Pump";                     
                case 7:
                    return "Positive Credit Gets Transferred";

                case 8:
                    return "Negative Credit Gets Ignored";

                case 9:
                    return "Relay Disconnects On Credit";
                case 10:
                    return "Relay Disconnects On Expiration";
                case 11:
                    return "Over Pumping";
                default:
                    return "";
            }
        }

        private static string PersianContext_207(int messageID)
        {
            switch (messageID)
            {
                case 0:
                    return "اطلاعات موردنظر در دسترس نیست";
                case 1:
                    return "باتري تمام شده است";
                case 2:
                    return "تاريخ اعتبار به پايان رسيده‌است";
                case 3:
                    return "كارآيي پمپ پايين است";
                case 4:
                    return "خطا در استفاده از كنتور";
                case 5:
                    return "رله برق را كامل قطع مي كند";
                case 6:
                    return "با روشن شدن پمپ رله برق را قطع مي‌كند";
                case 7:
                    return "اعتبار مثبت منتقل مي‌شود";
                case 8:
                    return "اعتبار مثبت منتقل نمي شود ";
                case 9:
                    return "اعتبار منفي بخشيده مي‌شود";
                case 10:
                    return "اعتبار منفي بخشيده نمي‌شود";
                case 11:
                    return "رله با اتمام اعتبار برق را قطع مي‌كند";
                case 12:
                    return "رله در پايان تاريخ اعتبار برق را قطع مي‌كند";
                case 13:
                    return "اضافه برداشت";
                
                default:
                    return "";
            }
        }

        internal static string PersianContext_303(int messageID)
        {
            switch (messageID)
            {

                case 0:
                    return "اطلاعات موردنظر در دسترس نیست";
                //Status 2 
                #region 2
                case 1:
                    return "جریان اصلی قطع می باشد.";
                case 2:
                    return "رله اصلی ، در حالت قطع از مرکز می باشد";
                case 3:
                    return "رله اصلی ، در حالت وصل می باشد.";

                //case 4:
                //    return "";
                case 4:
                    return "رله اصلی در حالت آماده به وصل می باشد.";
                case 6:
                    //  return "قطع خازنی";
                    return "";
                case 7:
                    //  return "رله خازنی در حالت قطع می باشد";
                    return "";
                case 8:
                    //return "رله خازنی در حالت وصل می باشد";
                    return "";
                case 9:
                    // return "رله خازنی در حالت آماده به وصل می باشد";
                    return "";
                case 10:
                    return "";
                case 11:
                    return "قطع رله به علت اعتبار منفی اتفاق افتاده است";
                case 12:
                    return "قطع رله به علت اتمام تاریخ پایان اعتبار می باشد.";
                case 13:
                    return "قطع رله به علت قرار گرفتن در بازه زمانی پیک می باشد.";
                case 14:
                    return "قطع رله به علت محدودیت مصرف می باشد.";
                case 15:
                    return "رله به علت انجام عمل soft starter قطع می باشد.";
                case 16:
                    return "رله به علت stabilizer قطع می باشد.";
                case 17:
                    return "ر له به علت دمای پایین از کار افتاده است.";
                case 18: return "";
                case 19: return "";
                case 20: return "";
                case 21: return "";
                case 22: return "";
                case 23: return "";
                case 24: return "";
                case 25: return "";
                case 26: return "";
                case 27: return "";
                case 28: return "";
                case 29: return "";
                case 30: return "";
                case 31: return "";
                #endregion

                //Status 3  32/64
                #region 3
                case 32:
                    return "Fatal Device Error";
                case 33:
                    return "Unreliable operating conditions";
                case 34:
                    return "Unreliable external conditions";
                case 35:
                    return "Self Test error";
                case 36:
                    return "Network Timeout";
                case 37:
                    return "Program Memory Error";
                case 38:
                    return "NV memory error";
                case 39:
                    return "NV memory error";// i insert
                case 40:
                    return "Flash Log memory error";
                case 41:
                    return "RAM backup memory CRC error";
                case 42:
                    return "EEPROM memory CRC error";
                case 43:
                    return "RAM Error";
                case 44:
                    return ""; //i insert
                case 45:
                    return "measuring values disturbed";
                //case 46:
                //    return "";
                case 46:
                    return "measuring process stopped";
                case 47:
                    return "energy reversed";
                case 48:
                    return ""; // i insert
                case 49:
                    return "Invalid Time";
                case 50:
                    return "";
                case 51:
                    return "DST started";
                case 52:
                    return ""; // i insert
                case 53:
                    return "Doubtful Time";
                case 54:
                    return "";
                case 55:
                    return ""; // i insert
                case 56:
                    return "RTC battery operation failure";
                //case 57:
                //    return "";
                case 57:
                    return "RWP battery operation failure";
                //case 59:
                //    return "";
                //case 60:
                //    return "";               
                case 58:
                    return "RTC battery low voltage";
                case 59:
                    return "RTC battery replacement need";
                case 60:
                    return "Backup RTC low voltage";
                case 61:
                    return "Backup RTC battery replacement need";
                case 62:
                    return "Backup RTC battery replacement need";
                #endregion

                // Status 4       64/96
                #region 4
                case 64:
                    return "";//" L1 power failure ";
                case 65:
                    return "";//" L2 power failure";
                case 66:
                    return "";//"L3 power failure";
                case 67:
                    return "";
                case 68:
                    return "";//"RWP request ";
                case 69:
                    return "";//"Tamper detected";
                case 70:
                    return "";//"Terminal cover opened ";
                case 71:
                    return "";//"Meter cover opened ";
                case 72:
                    return "";//"Magnetic tamper detected ";
                case 73:
                    return "";//"Relay tamper detected ";
                case 74:
                    return "";//"Wrong password tamper detected ";
                case 75:
                    return "";//"Current without voltage ";
                case 76:
                    return "اعتبار منفی است";//"Negative credit ";
                case 77:
                    return "تاریخ پایان اعتبار پایان یافته است";// "Expired credit ";
                case 78:
                    return "عملکرد اعتباری فعال است";//"Credit capability activated ";
                case 79:
                    return "بدهی فعال است";// "Debit activated ";
                case 80:
                    return "";// "Negative credit limitation ";
                case 81:
                    return "";//"Expired credit limitation ";
                case 82:
                    return "";
                case 83:
                    return "";
                case 84:
                    return "";// "Red code ";
                case 85:
                    return "";
                case 86:
                    return "";//  "Electro-pump on/off ";
                case 87:
                    return "";// "Electro-pump low efficiency ";
                case 88:
                    return "";// "Flow more than maximum expected ";
                case 89:
                    return "";// "Flow less than minimum expected ";
                case 90:
                    return "";// "Electro-pump curve adjusted ";
                case 91:
                    return "";// "Reactive curve is configured ";
                case 92:
                    return "";// "Reactive curve is estimating ";
                case 93:
                    return "";// "Electro-pump has been changed ";
                #endregion

                // Status 5:    96/128
                #region 5
                case 96:
                    return "";// "Customizes tariff is activated";
                case 97:
                    return "";//"Tariff is customized via negative credit";
                case 98:
                    return "";//"Tariff is customized via expired credit"; ;
                case 99:
                    return "";//"Tariff is customized via red code";
                case 100:
                    return "";//"Tariff is customized via exceeding limitations";
                case 101:
                    return "";
                case 102:
                    return "جریان اصلی قطع می باشد";// "Main relay disconnect";
                case 103:
                    return "رله اصلی در حالت قطع از مرکز می باشد";// "Main relay disconnected state";
                case 104:
                    return "رله اصلی در حالت وصل می باشد";// "Main relay connected state";
                case 105:
                    return "رله اصلی در حالت آماده به وصل می باشد";//"Main relay ready for reconnection state";
                case 106:
                    return "";// "Capacitor relay disconnect";
                case 107:
                    return "";//"Capacitor relay disconnected state";
                case 108:
                    return "";//"Capacitor relay connected state";
                case 109:
                    return "";//"Capacitor relay ready for reconnection state";
                case 110:
                    return "";
                case 111:
                    return "";
                case 112:
                    return "قطع رله به علت اعتبار منفی اتفاق افتاده است ";//"Disconnection request via negative credit";
                case 113:
                    return "قطع رله به علت اتمام تاریخ پایان اعتبار می باشد";//"Disconnection request via expired credit";
                case 114:
                    return "قطع رله به علت قرار گرفتن در بازه زمانی پیک می باشد";//"Scheduled disconnect request";
                case 115:
                    return "قطع رله به علت محدودیت مصرف می باشد";//"Disconnection request via limiter objects";
                case 116:
                    return "";//"Disconnection request via soft starter";
                case 117:
                    return "";//"Disconnection request via relay stabilizer";
                case 118:
                    return "رله به علت دمای پایین از کار افتاده است";//"Relay access denied via low temperature";
                #endregion
                // Statuse 6 192 /224
                #region 6
                case 192:
                    return "Emergency Profile of absolute active energy limitation activated";
                case 193:
                    return "Emergency Profile of positive active energy limitation activated";
                case 194:
                    return "Emergency Profile of negative active energy limitation activated";
                case 195:
                    return "Exceeding absolute active energy threshold";
                case 196:
                    return "Exceeding positive active energy threshold";
                case 197:
                    return "Exceeding negative active energy threshold";
                case 198:
                    return "Emergency Profile of absolute active power limitation activated";
                case 199:
                    return "Emergency Profile of positive active power limitation activated";
                case 200:
                    return "Emergency Profile of negative active power limitation activated";
                case 201:
                    return "Exceeding absolute active power threshold";
                case 202:
                    return "Exceeding positive active power threshold";
                case 203:
                    return "Exceeding negative active power threshold";
                //case 204:
                //    return "Exceeding negative active power threshold";
                case 204:
                    return "Emergency Profile of positive reactive power limitation activated";
                case 205:
                    return "Emergency Profile of negative reactive power limitation activated";
                case 206:
                    return "Exceeding positive reactive power threshold";
                case 207:
                    return "Exceeding negative reactive power threshold";
                case 208:
                    return "Emergency Profile of apparent power limitation activated";
                case 209:
                    return "Exceeding apparent power threshold";
                case 210:
                    return "Emergency Profile of water limitation activated";
                case 211:
                    return "Exceeding water threshold";
                case 212:
                    return "Emergency Profile of water flow limitation activated";
                case 213:
                    return "Exceeding water flow threshold";
                case 214:
                    return "Emergency Profile of time of pump working limitation activated";
                case 215:
                    return "Exceeding time of pump working threshold"; // i insert
                case 216:
                    return "Exceeding average current emergency up threshold";
                case 217:
                    return "Exceeding L1 current emergency up threshold";
                case 218:
                    return "Exceeding L2 current emergency up threshold";
                case 219:
                    return "Exceeding L3 current emergency up threshold";
                case 220:
                    return "Exceeding average current normal up threshold";
                case 221:
                    return "Exceeding L1 current normal up threshold";
                case 222:
                    return "Exceeding L2 current normal up threshold";
                case 223:
                    return "Exceeding L3 current normal up threshold";
                #endregion

                // status 7  224/256
                #region 7
                case 224:
                    return "Exceeding power factor emergency up threshold";
                case 225:
                    return "Exceeding power factor normal up threshold";
                case 226:
                    return "Exceeding power factor normal down threshold";
                case 227:
                    return "Exceeding power factor emergency down threshold";
                case 228:
                    return "Exceeding average voltage emergency up threshold";
                case 229:
                    return "Exceeding L1 voltage emergency up threshold";
                case 230:
                    return "Exceeding L2 voltage emergency up threshold";
                case 231:
                    return "Exceeding L3 voltage emergency up threshold";
                case 232:
                    return "Exceeding average voltage normal up threshold";
                case 233:
                    return "Exceeding L1 voltage normal up threshold";
                case 234:
                    return "Exceeding L2 voltage normal up threshold";
                case 235:
                    return "Exceeding L3 voltage normal up threshold";
                case 236:
                    return "Exceeding average voltage normal down threshold";
                case 237:
                    return "Exceeding L1 voltage normal down threshold";
                case 238:
                    return "Exceeding L2 voltage normal down threshold";
                case 239:
                    return "Exceeding L3 voltage normal down threshold";
                case 240:
                    return "Exceeding average voltage emergency down threshold";
                case 241:
                    return "Exceeding L1 voltage emergency down threshold";
                case 242:
                    return "Exceeding L2 voltage emergency down threshold";
                case 243:
                    return "Exceeding L3 voltage emergency down threshold";
                case 244:
                    return "Asymmetrical voltage";
                case 245:
                    return "Reverse phase sequence";
                case 246:
                    return "Null-phase misconnected";

                #endregion
                // Status 8: 256/288:               
                #region 8
                case 256:
                    return "";
                case 257:
                    return "";
                case 258:
                    return "";
                case 259:
                    return "";
                case 260:
                    return "";
                case 261:
                    return "";
                case 262:
                    return "";
                case 263:
                    return "";
                case 264:
                    return "";
                case 265:
                    return "";
                case 266:
                    return "";
                case 267:
                    return "";
                case 268:
                    return "";
                case 269:
                    return "";
                case 270:
                    return "";
                case 271:
                    return "";
                #endregion

                // Status 9 288/320
                #region 9
                case 288:
                    return "";
                case 289:
                    return "";
                case 290:
                    return "";
                case 291:
                    return "";
                case 292:
                    return "";
                case 293:
                    return "";
                case 294:
                    return "";
                case 295:
                    return "";
                case 296:
                    return "";
                case 297:
                    return "";
                case 298:
                    return "";
                case 299:
                    return "";
                case 300:
                    return "";
                case 301:
                    return "";
                case 302:
                    return "";
                case 303:
                    return "";
                case 304:
                    return "";
                case 305:
                    return "";
                case 306:
                    return "";
                case 307:
                    return "";
                case 308:
                    return "";
                case 309:
                    return "";
                case 310:
                    return "";
                case 311:
                    return "";
                #endregion

                // Status 10: 320/352
                #region 10
                case 320:
                    return "";
                case 321:
                    return "";
                case 322:
                    return "";
                case 323:
                    return "";
                case 324:
                    return "";
                case 325:
                    return "";
                case 326:
                    return "";
                case 327:
                    return "";
                case 328:
                    return "";
                case 329:
                    return "";
                case 330:
                    return "";
                case 331:
                    return "";
                case 332:
                    return "";
                case 333:
                    return "";
                case 334:
                    return "";
                case 335:
                    return "";
                case 336:
                    return "";
                case 337:
                    return "";

                #endregion

                // Status 11: 352/384
                #region 11
                case 352:
                    return "Fatal Device Error";
                case 353:
                    return "Unreliable operating conditions";
                case 354:
                    return "Unreliable external conditions";
                case 355:
                    return "Self Test error";
                case 356:
                    return "Network Timeout";
                case 357:
                    return "No reliable code exist";
                case 358:
                    return "Flash serious memory error";
                case 359:
                    return "Flash normal memory error";
                case 360:
                    return "Flash Log memory error";
                case 361:
                    return "RAM backup memory CRC error";
                case 362:
                    return "EEPROM memory CRC error";
                case 363:
                    return "RAM Error";
                case 364:
                    return "Invalid Time";
                case 365:
                    return "";
                case 366:
                    return "Doubtful Time";

                #endregion


                // Status 12: 384/ 416       
                #region 12
                case 384:
                    return "Tamper occurred";
                case 385:
                    return "Terminal cover opened";
                case 386:
                    return "Meter cover opened";
                case 387:
                    return "Magnetic tamper detected";
                case 388:
                    return "Relay tamper detected";
                case 389:
                    return "Unauthorized access attempt";
                case 390:
                    return "Current without voltage";
                #endregion


                // Status 13: 416/448
                #region 13
                case 416:
                    return "RTC battery operation fail ";
                case 417:
                    return "RWP battery operation fail ";
                case 418:
                    return "RTC battery low voltage";
                case 419:
                    return "RTC battery replace need ";
                case 420:
                    //return "Common battery low voltage"; // i changed
                    return "Backup RTC battery low voltage";
                case 421:
                    //return "Common battery replace need";// i changed
                    return "Backup RTC battery replace need";
                case 422:
                    return "RWP battery replace need";
                case 423:
                    //return "RWP battery  replace need";// i changed
                    return "RWP request";
                #endregion


                // Status 14: 448/480

                case 448:
                    return "Energy status";


                // Status 15: 480/512
                #region 15
                case 480:
                    return "L1 Power failure";
                case 481:
                    return "L2 Power failure";
                case 482:
                    return "L3 Power failure";
                case 483:
                    return "Null Failure";
                case 484:
                    return "";
                case 485:
                    return "";
                case 486:
                    return "";
                case 487:
                    return "";
                case 488:
                    return "Average Voltage Emergency Up Threshold";
                case 489:
                    return "L1 Voltage Emergency Up Threshold";
                case 490:
                    return "L2 Voltage Emergency Up Threshold";
                case 491:
                    return "L3 Voltage Emergency Up Threshold";
                case 492:
                    return "Average Voltage Normal Up Threshold";
                case 493:
                    return "L1 Voltage Normal Up Threshold";
                case 494:
                    return "L2 Voltage Normal Up Threshold";
                case 495:
                    return "L3 Voltage Normal Up Threshold";
                case 496:
                    return "Average Voltage Normal Down Threshold";
                case 497:
                    return "L1 Voltage Normal Down Threshold";
                case 498:
                    return "L2 Voltage Normal Down Threshold";
                case 499:
                    return "L3 Voltage Normal Down Threshold";
                case 500:
                    return "Average Voltage Emergency Down Threshold";
                case 501:
                    return "L1 Voltage Emergency Down Threshold";
                case 502:
                    return "L2 Voltage Emergency Down Threshold";
                case 503:
                    return "L3 Voltage Emergency Down Threshold";
                case 504:
                    return "";
                case 505:
                    return "";
                case 506:
                    return "";
                case 507:
                    return "";
                case 508:
                    return "Asymmetrical Voltage";
                case 509:
                    return "Reverse Phase Sequence";
                case 510:
                    return "Null and Phase misconnected";
                #endregion
                // Status 16: 512/544
                #region 16
                case 512:
                    return "Average Current Emergency Up Threshold";
                case 513:
                    return "L1 Current Emergency Up Threshold";
                case 514:
                    return "L2 Current Emergency Up Threshold";
                case 515:
                    return "L3 Current Emergency Up Threshold";
                case 516:
                    return "Average Current Normal Up Threshold";
                case 517:
                    return "L1 Current Normal Up Threshold";
                case 518:
                    return "L2 Current Normal Up Threshold";
                case 519:
                    return "L3 Current Normal Up Threshold";

                #endregion
                // Status 17: 544/576
                #region 17
                case 544:
                    return "Measuring values disturbed";
                case 545:
                    return "Measuring process stopped";
                #endregion

                // Status 18: 576/608
                #region 18
                case 576:
                    return "پمپ روشن است .";
                case 577:
                    return "راندمان پمپ پایین است.";
                case 578:
                    return "د بی لحظه ای ، بیشتر از دبی ماکزیمم منحنی می باشد.";
                case 579:
                    return "د بی لحظه ای ، کمتر از دبی مینیمم منحنی می باشد.";
                case 580:
                    return "منحنی اکتیو پمپ، تنظیم شده است.";
                case 581:
                    return "";// "Pump reactive curve is configured";
                case 582:
                    return "";// "Pump reactive curve is estimating";
                case 583:
                    return "";// "Pump curve changed";
                #endregion

                // Status 19: 608/640
                #region 19
                case 608:
                    return "Consumption limitation due to negative credit";
                case 609:
                    return "Consumption limitation due to expired credit";
                case 610:
                    return "Red Code Activated";
                case 611:
                    return "Emergency Profile of absolute active energy limitation activated";
                case 612:
                    return "Emergency Profile of positive active energy limitation activated";
                case 613:
                    return "Emergency Profile of negative active energy limitation activated";
                case 614:
                    return "Exceeding absolute active energy threshold";
                case 615:
                    return "Exceeding positive active energy threshold";
                case 616:
                    return "Exceeding negative active energy threshold";
                case 617:
                    return "Emergency Profile of absolute active power limitation activated";
                case 618:
                    return "Emergency Profile of positive active power limitation activated";
                case 619:
                    return "Emergency Profile of negative active power limitation activated";
                case 620:
                    return "Exceeding absolute active power threshold";
                case 621:
                    return "Exceeding positive active power threshold";
                case 622:
                    return "Exceeding negative active power threshold";
                case 623:
                    return "Emergency Profile of positive reactive power limitation activated";
                case 624:
                    return "Emergency Profile of negative reactive power limitation activated";
                case 625:
                    return "Exceeding positive reactive power threshold";
                case 626:
                    return "Exceeding negative reactive power threshold";
                case 627:
                    return "Emergency Profile of apparent power limitation activated";
                case 628:
                    return "Exceeding apparent power threshold";  // Insert
                case 629:
                    return "Emergency Profile of water limitation activated";
                case 630:
                    return "Exceeding water threshold";
                case 631:
                    return "Emergency Profile of flow limitation activated";
                case 632:
                    return "Exceeding flow threshold";
                case 633:
                    return "Emergency Profile of time of pump working limitation activated";
                case 634:
                    return "Exceeding time of pump working threshold";
                case 635:
                    return "Exceeding power factor emergency up threshold";
                case 636:
                    return "Exceeding power factor normal up threshold";
                case 637:
                    return "Exceeding power factor normal down threshold";
                case 638:
                    return "Exceeding power factor emergency down threshold";
                #endregion

                // Status 20: 640/672
                #region 20
                case 640:
                    return "اعتبار منفی است.";
                case 641:
                    return "تاریخ اعتبار، پایان یافته است.";
                case 642:
                    return "عملکرد اعتباری فعال می باشد.";
                case 643:
                    return "بدهی فعال است.";
                #endregion

                // Status 21:  672/704
                #region 21
                case 672:
                    return "اعتبار جدید به اعتبار قبلی اضافه خواهد شد.";
                case 673:
                    return "اعتبار جدید جایگزین اعتبار قبلی می شود.";

                case 674:
                    return "بدهی و اضافه برداشت قبلی از اعتبار جدید کم می شود.";
                case 675:
                    return "با فعال شدن اعتبار جدید بدهی و اضافه برداشت قبلی نادیده گرفته می شود.";
                case 676:
                    return "اعتبار جدید به صورت خودکار در زمان شروع فعال می شود.";
                case 677:
                    return "اعتبار جدید به صورت دستی  در زمان شروع فعال خواهد شد.";
                case 678:
                    return "اعتبار جدید از نوع اعتبار اصلی است.";
                case 679:
                    return "اعتبار جدید از نوع اعتبار اضطراری است.";
                case 680:
                    return "درصورتیکه تاریخ پایان اعتبار توکن از تاریخ پایان اعتبار کنتور دورتر باشد تاریخ پایان اعتبار توکن جایگزین می شود.";
                case 681:
                    return "تاریخ پایان اعتبار جدید، جایگزین تاریخ پایان اعتبار کنتور می شود.";
                #endregion

                #region 1

                // Status 1:   704/736
                case 704:
                    return "مدت زمانی که کنتور قبل از قطع رله ، به علت اتمام تاریخ پایان اعتبار ، در نظر می گیرد.";
                case 705:
                    return "با منفی شدن اعتبار رله قطع می شود.";
                case 706:
                    return "با اتمام تاریخ پایان اعتبار رله قطع می شود.";
                //case 707:
                //    return " This is an original credit.";
                //case 708:
                //    return "Credit end time will be most far of current end time and new arrived token end time.";

                #endregion

                default:
                    break;
            }
            return string.Empty;
        }

        internal static string EnglishContext_303(int messageID)
        {
            switch (messageID)
            {
                case 0:
                    return "Information does not accessible.";//"اطلاعات موردنظر در دسترس نیست";

                //Status 2 
                #region 2
                case 1:
                    return "Disconnect main load.";
                case 2:
                    return "Main Relay is in disconnected state";
                case 3:
                    return "Main Relay is in connected state.";
                case 4:
                    return "Main Relay is in ready for reconnected state";
                case 5:
                    return "";
                case 6:
                    return "Disconnect capacitor.";
                case 7:
                    //  return "Capacitor Relay is in disconnected state";
                    return "";
                case 8:
                    //  return "Capacitor Relay is in connected state";
                    return "";
                case 9:
                    //return "Capacitor Relay is in ready for reconnected state ";
                    return "";
                case 10:
                    // return "رله خازنی در حالت آماده به وصل می باشد";
                    return "";
                case 11:
                    //return "Disconnection due to negative credit";
                    return "";
                case 12:
                    return "Disconnection due to expired credit";
                case 13:
                    return "Scheduled load disconnection.";
                case 14:
                    return "Disconnection due to consumption limitation.";
                case 15:
                    return "Disconnection by soft starter.";
                case 16:
                    return "Disconnection by relay stabilizer.";
                case 17:
                    return "Relay access denied via low temperature.";
                case 18:
                    //return "Disconnection by water user controller";
                    return ""; //"ر له به علت دمای پایین از کار افتاده است.";

                #endregion

                //Status 3  32/64
                #region 3
                case 32:
                    return "Fatal Device Error";
                case 33:
                    return "Unreliable operating conditions";
                case 34:
                    return "Unreliable external conditions";
                case 35:
                    return "Self Test error";
                case 36:
                    return "Network Timeout";
                case 37:
                    return "Program Memory Error";
                case 38:
                    return "NV memory error";
                case 39:
                    return "NV memory error";// i insert
                case 40:
                    return "Flash Log memory error";
                case 41:
                    return "RAM backup memory CRC error";
                case 42:
                    return "EEPROM memory CRC error";
                case 43:
                    return "RAM Error";
                case 44:
                    return ""; //i insert
                case 45:
                    return "measuring values disturbed";
                //case 46:
                //    return "";
                case 46:
                    return "measuring process stopped";
                case 47:
                    return "energy reversed";
                case 48:
                    return ""; // i insert
                case 49:
                    return "Invalid Time";
                case 50:
                    return "";
                case 51:
                    return "DST started";
                case 52:
                    return ""; // i insert
                case 53:
                    return "Doubtful Time";
                case 54:
                    return "";
                case 55:
                    return ""; // i insert
                case 56:
                    return "RTC battery operation failure";
                //case 57:
                //    return "";
                case 57:
                    return "RWP battery operation failure";
                //case 59:
                //    return "";
                //case 60:
                //    return "";               
                case 58:
                    return "RTC battery low voltage";
                case 59:
                    return "RTC battery replacement need";
                case 60:
                    return "Backup RTC low voltage";
                case 61:
                    return "Backup RTC battery replacement need";
                case 62:
                    return "Backup RTC battery replacement need";
                //case 62:
                //    return "Backup RTC battery replacement need";
                //case 63:
                //    return "Backup RTC battery replacement need";
                #endregion

                // Status 4       64/96
                #region 4
                case 64:
                    return " L1 power failure ";
                case 65:
                    return " L2 power failure";
                case 66:
                    return "L3 power failure";
                case 67:
                    return "";
                case 68:
                    return "RWP request ";
                case 69:
                    return "Tamper detected";
                case 70:
                    return "Terminal cover opened ";
                case 71:
                    return "Meter cover opened ";
                case 72:
                    return "Magnetic tamper detected ";
                case 73:
                    return "Relay tamper detected ";
                case 74:
                    return "Wrong password tamper detected ";
                case 75:
                    return "Current without voltage ";
                case 76:
                    return "Negative credit ";
                case 77:
                    return "Expired credit ";
                case 78:
                    return "Credit capability activated ";
                case 79:
                    return "Debit activated ";
                case 80:
                    return "Negative credit limitation ";
                case 81:
                    return "Expired credit limitation ";
                case 82:
                    return "";
                case 83:
                    return "";
                case 84:
                    return "Red code ";
                case 85:
                    return "";
                case 86:
                    return "Electro-pump on/off ";
                case 87:
                    return "Electro-pump low efficiency ";
                case 88:
                    return "Flow more than maximum expected ";
                case 89:
                    return "Flow less than minimum expected ";
                case 90:
                    return "Electro-pump curve adjusted ";
                case 91:
                    return "Reactive curve is configured ";
                case 92:
                    return "Reactive curve is estimating ";
                case 93:
                    return "Electro-pump has been changed ";
                #endregion

                // Status 5:    96/128
                #region 5
                case 96:
                    return "Customizes tariff is activated";
                case 97:
                    return "Tariff is customized via negative credit";
                case 98:
                    return "Tariff is customized via expired credit"; ;
                case 99:
                    return "Tariff is customized via red code";
                case 100:
                    return "Tariff is customized via exceeding limitations";
                case 101:
                    return "";
                case 102:
                    return "Main relay disconnect";
                case 103:
                    return "Main relay disconnected state";
                case 104:
                    return "Main relay connected state";
                case 105:
                    return "Main relay ready for reconnection state";
                case 106:
                    return "Capacitor relay disconnect";
                case 107:
                    return "Capacitor relay disconnected state";
                case 108:
                    return "Capacitor relay connected state";
                case 109:
                    return "Capacitor relay ready for reconnection state";
                case 110:
                    return "";
                case 111:
                    return "";
                case 112:
                    return "Disconnection request via negative credit";
                case 113:
                    return "Disconnection request via expired credit";
                case 114:
                    return "Scheduled disconnect request";
                case 115:
                    return "Disconnection request via limiter objects";
                case 116:
                    return "Disconnection request via soft starter";
                case 117:
                    return "Disconnection request via relay stabilizer";
                case 118:
                    return "Relay access denied via low temperature";
                #endregion
                // Statuse 6 192 /224
                #region 6
                case 192:
                    return "Emergency Profile of absolute active energy limitation activated";
                case 193:
                    return "Emergency Profile of positive active energy limitation activated";
                case 194:
                    return "Emergency Profile of negative active energy limitation activated";
                case 195:
                    return "Exceeding absolute active energy threshold";
                case 196:
                    return "Exceeding positive active energy threshold";
                case 197:
                    return "Exceeding negative active energy threshold";
                case 198:
                    return "Emergency Profile of absolute active power limitation activated";
                case 199:
                    return "Emergency Profile of positive active power limitation activated";
                case 200:
                    return "Emergency Profile of negative active power limitation activated";
                case 201:
                    return "Exceeding absolute active power threshold";
                case 202:
                    return "Exceeding positive active power threshold";
                case 203:
                    return "Exceeding negative active power threshold";
                //case 204:
                //    return "Exceeding negative active power threshold";
                case 204:
                    return "Emergency Profile of positive reactive power limitation activated";
                case 205:
                    return "Emergency Profile of negative reactive power limitation activated";
                case 206:
                    return "Exceeding positive reactive power threshold";
                case 207:
                    return "Exceeding negative reactive power threshold";
                case 208:
                    return "Emergency Profile of apparent power limitation activated";
                case 209:
                    return "Exceeding apparent power threshold";
                case 210:
                    return "Emergency Profile of water limitation activated";
                case 211:
                    return "Exceeding water threshold";
                case 212:
                    return "Emergency Profile of water flow limitation activated";
                case 213:
                    return "Exceeding water flow threshold";
                case 214:
                    return "Emergency Profile of time of pump working limitation activated";
                case 215:
                    return "Exceeding time of pump working threshold"; // i insert
                case 216:
                    return "Exceeding average current emergency up threshold";
                case 217:
                    return "Exceeding L1 current emergency up threshold";
                case 218:
                    return "Exceeding L2 current emergency up threshold";
                case 219:
                    return "Exceeding L3 current emergency up threshold";
                case 220:
                    return "Exceeding average current normal up threshold";
                case 221:
                    return "Exceeding L1 current normal up threshold";
                case 222:
                    return "Exceeding L2 current normal up threshold";
                case 223:
                    return "Exceeding L3 current normal up threshold";
                #endregion

                // status 7  224/256
                #region 7
                case 224:
                    return "Exceeding power factor emergency up threshold";
                case 225:
                    return "Exceeding power factor normal up threshold";
                case 226:
                    return "Exceeding power factor normal down threshold";
                case 227:
                    return "Exceeding power factor emergency down threshold";
                case 228:
                    return "Exceeding average voltage emergency up threshold";
                case 229:
                    return "Exceeding L1 voltage emergency up threshold";
                case 230:
                    return "Exceeding L2 voltage emergency up threshold";
                case 231:
                    return "Exceeding L3 voltage emergency up threshold";
                case 232:
                    return "Exceeding average voltage normal up threshold";
                case 233:
                    return "Exceeding L1 voltage normal up threshold";
                case 234:
                    return "Exceeding L2 voltage normal up threshold";
                case 235:
                    return "Exceeding L3 voltage normal up threshold";
                case 236:
                    return "Exceeding average voltage normal down threshold";
                case 237:
                    return "Exceeding L1 voltage normal down threshold";
                case 238:
                    return "Exceeding L2 voltage normal down threshold";
                case 239:
                    return "Exceeding L3 voltage normal down threshold";
                case 240:
                    return "Exceeding average voltage emergency down threshold";
                case 241:
                    return "Exceeding L1 voltage emergency down threshold";
                case 242:
                    return "Exceeding L2 voltage emergency down threshold";
                case 243:
                    return "Exceeding L3 voltage emergency down threshold";
                case 244:
                    return "Asymmetrical voltage";
                case 245:
                    return "Reverse phase sequence";
                case 246:
                    return "Null-phase misconnected";

                #endregion
                // Status 8: 256/288:               
                #region 8
                case 256:
                    return "";
                case 257:
                    return "";
                case 258:
                    return "";
                case 259:
                    return "";
                case 260:
                    return "";
                case 261:
                    return "";
                case 262:
                    return "";
                case 263:
                    return "";
                case 264:
                    return "";
                case 265:
                    return "";
                case 266:
                    return "";
                case 267:
                    return "";
                case 268:
                    return "";
                case 269:
                    return "";
                case 270:
                    return "";
                case 271:
                    return "";
                #endregion

                // Status 9 288/320
                #region 9
                case 288:
                    return "";
                case 289:
                    return "";
                case 290:
                    return "";
                case 291:
                    return "";
                case 292:
                    return "";
                case 293:
                    return "";
                case 294:
                    return "";
                case 295:
                    return "";
                case 296:
                    return "";
                case 297:
                    return "";
                case 298:
                    return "";
                case 299:
                    return "";
                case 300:
                    return "";
                case 301:
                    return "";
                case 302:
                    return "";
                case 303:
                    return "";
                case 304:
                    return "";
                case 305:
                    return "";
                case 306:
                    return "";
                case 307:
                    return "";
                case 308:
                    return "";
                case 309:
                    return "";
                case 310:
                    return "";
                case 311:
                    return "";
                #endregion

                // Status 10: 320/352
                #region 10
                case 320:
                    return "";
                case 321:
                    return "";
                case 322:
                    return "";
                case 323:
                    return "";
                case 324:
                    return "";
                case 325:
                    return "";
                case 326:
                    return "";
                case 327:
                    return "";
                case 328:
                    return "";
                case 329:
                    return "";
                case 330:
                    return "";
                case 331:
                    return "";
                case 332:
                    return "";
                case 333:
                    return "";
                case 334:
                    return "";
                case 335:
                    return "";
                case 336:
                    return "";
                case 337:
                    return "";

                #endregion

                // Status 11: 352/384
                #region 11
                case 352:
                    return "Fatal Device Error";
                case 353:
                    return "Unreliable operating conditions";
                case 354:
                    return "Unreliable external conditions";
                case 355:
                    return "Self Test error";
                case 356:
                    return "Network Timeout";
                case 357:
                    return "No reliable code exist";
                case 358:
                    return "Flash serious memory error";
                case 359:
                    return "Flash normal memory error";
                case 360:
                    return "Flash Log memory error";
                case 361:
                    return "RAM backup memory CRC error";
                case 362:
                    return "EEPROM memory CRC error";
                case 363:
                    return "RAM Error";
                case 364:
                    return "Invalid Time";
                case 365:
                    return "";
                case 366:
                    return "Doubtful Time";

                #endregion


                // Status 12: 384/ 416       
                #region 12
                case 384:
                    return "Tamper occurred";
                case 385:
                    return "Terminal cover opened";
                case 386:
                    return "Meter cover opened";
                case 387:
                    return "Magnetic tamper detected";
                case 388:
                    return "Relay tamper detected";
                case 389:
                    return "Unauthorized access attempt";
                case 390:
                    return "Current without voltage";
                #endregion


                // Status 13: 416/448
                #region 13
                case 416:
                    return "RTC battery operation fail ";
                case 417:
                    return "RWP battery operation fail ";
                case 418:
                    return "RTC battery low voltage";
                case 419:
                    return "RTC battery replace need ";
                case 420:
                    //return "Common battery low voltage"; // i changed
                    return "Backup RTC battery low voltage";
                case 421:
                    //return "Common battery replace need";// i changed
                    return "Backup RTC battery replace need";
                case 422:
                    return "RWP battery replace need";
                case 423:
                    //return "RWP battery  replace need";// i changed
                    return "RWP request";
                #endregion


                // Status 14: 448/480

                case 448:
                    return "Energy status";


                // Status 15: 480/512
                #region 15
                case 480:
                    return "L1 Power failure";
                case 481:
                    return "L2 Power failure";
                case 482:
                    return "L3 Power failure";
                case 483:
                    return "Null Failure";
                case 484:
                    return "";
                case 485:
                    return "";
                case 486:
                    return "";
                case 487:
                    return "";
                case 488:
                    return "Average Voltage Emergency Up Threshold";
                case 489:
                    return "L1 Voltage Emergency Up Threshold";
                case 490:
                    return "L2 Voltage Emergency Up Threshold";
                case 491:
                    return "L3 Voltage Emergency Up Threshold";
                case 492:
                    return "Average Voltage Normal Up Threshold";
                case 493:
                    return "L1 Voltage Normal Up Threshold";
                case 494:
                    return "L2 Voltage Normal Up Threshold";
                case 495:
                    return "L3 Voltage Normal Up Threshold";
                case 496:
                    return "Average Voltage Normal Down Threshold";
                case 497:
                    return "L1 Voltage Normal Down Threshold";
                case 498:
                    return "L2 Voltage Normal Down Threshold";
                case 499:
                    return "L3 Voltage Normal Down Threshold";
                case 500:
                    return "Average Voltage Emergency Down Threshold";
                case 501:
                    return "L1 Voltage Emergency Down Threshold";
                case 502:
                    return "L2 Voltage Emergency Down Threshold";
                case 503:
                    return "L3 Voltage Emergency Down Threshold";
                case 504:
                    return "";
                case 505:
                    return "";
                case 506:
                    return "";
                case 507:
                    return "";
                case 508:
                    return "Asymmetrical Voltage";
                case 509:
                    return "Reverse Phase Sequence";
                case 510:
                    return "Null and Phase misconnected";
                #endregion
                // Status 16: 512/544
                #region 16
                case 512:
                    return "Average Current Emergency Up Threshold";
                case 513:
                    return "L1 Current Emergency Up Threshold";
                case 514:
                    return "L2 Current Emergency Up Threshold";
                case 515:
                    return "L3 Current Emergency Up Threshold";
                case 516:
                    return "Average Current Normal Up Threshold";
                case 517:
                    return "L1 Current Normal Up Threshold";
                case 518:
                    return "L2 Current Normal Up Threshold";
                case 519:
                    return "L3 Current Normal Up Threshold";

                #endregion
                // Status 17: 544/576
                #region 17
                case 544:
                    return "Measuring values disturbed";
                case 545:
                    return "Measuring process stopped";
                #endregion

                // Status 18: 576/608
                #region 18
                case 576:
                    return "Pump ON";// "پمپ روشن است .";
                case 577:
                    return "Pump low efficiency";// "راندمان پمپ پایین است.";
                case 578:
                    return "Flow more than maximum expected";//"دبی لحظه ای ، بیشتر از دبی ماکزیمم منحنی می باشد.";
                case 579:
                    return "Flow less than minimum expected";//"د بی لحظه ای ، کمتر از دبی مینیمم منحنی می باشد.";
                case 580:
                    return "Pump active curve adjusted";//"منحنی اکتیو پمپ، تنظیم شده است.";
                //case 581:
                //    return"";
                case 581:
                    return "Pump reactive curve is configured";
                case 582:
                    return "Pump reactive curve is estimating";
                case 583:
                    return "Pump curve changed";
                #endregion

                // Status 19: 608/640
                #region 19
                case 608:
                    return "Consumption limitation due to negative credit";
                case 609:
                    return "Consumption limitation due to expired credit";
                case 610:
                    return "Red Code Activated";
                case 611:
                    return "Emergency Profile of absolute active energy limitation activated";
                case 612:
                    return "Emergency Profile of positive active energy limitation activated";
                case 613:
                    return "Emergency Profile of negative active energy limitation activated";
                case 614:
                    return "Exceeding absolute active energy threshold";
                case 615:
                    return "Exceeding positive active energy threshold";
                case 616:
                    return "Exceeding negative active energy threshold";
                case 617:
                    return "Emergency Profile of absolute active power limitation activated";
                case 618:
                    return "Emergency Profile of positive active power limitation activated";
                case 619:
                    return "Emergency Profile of negative active power limitation activated";
                case 620:
                    return "Exceeding absolute active power threshold";
                case 621:
                    return "Exceeding positive active power threshold";
                case 622:
                    return "Exceeding negative active power threshold";
                case 623:
                    return "Emergency Profile of positive reactive power limitation activated";
                case 624:
                    return "Emergency Profile of negative reactive power limitation activated";
                case 625:
                    return "Exceeding positive reactive power threshold";
                case 626:
                    return "Exceeding negative reactive power threshold";
                case 627:
                    return "Emergency Profile of apparent power limitation activated";
                case 628:
                    return "Exceeding apparent power threshold";  // Insert
                case 629:
                    return "Emergency Profile of water limitation activated";
                case 630:
                    return "Exceeding water threshold";
                case 631:
                    return "Emergency Profile of flow limitation activated";
                case 632:
                    return "Exceeding flow threshold";
                case 633:
                    return "Emergency Profile of time of pump working limitation activated";
                case 634:
                    return "Exceeding time of pump working threshold";
                case 635:
                    return "Exceeding power factor emergency up threshold";
                case 636:
                    return "Exceeding power factor normal up threshold";
                case 637:
                    return "Exceeding power factor normal down threshold";
                case 638:
                    return "Exceeding power factor emergency down threshold";
                #endregion

                // Status 20: 640/672
                #region 20
                case 640:
                    return "Negative Credit";//"اعتبار منفی است.";
                case 641:
                    return "Credit Expired";//"تاریخ اعتبار، پایان یافته است.";
                case 642:
                    return "Credit Capability Activated";//"عملکرد اعتباری فعال می باشد.";
                case 643:
                    return "Debit Activated";//"بدهی فعال است.";
                #endregion

                // Status 21:  672/704
                #region 21
                case 672:
                    return "previous credit will be added with new one.";
                case 673:
                    return "previous credit will be substituted by new one.";
                case 674:
                    return "previous debt will be subtracted from new credit.";
                case 675:
                    return "previous debt will be ignored by activating new credit";
                case 676:
                    return "credit will be activated on its start time, automatically";
                case 677:
                    return "this credit will only be activated after its start time and before its end time,manually.";
                case 678:
                    return " This is an original credit.";
                case 679:
                    return "this is an emergency credit; i.e. this credit will be subtracted from first upcoming credit.";
                case 680:
                    return "Credit end time will be most far of current end time and new arrived token end time.";

                #endregion

                #region 1

                // Status 1:   704/736
                case 704:
                    return "Break Time Before Disconnection"; // "مدت زمانی که کنتور قبل از قطع رله ، به علت اتمام تاریخ پایان اعتبار ، در نظر می گیرد.";
                case 705:
                    return "Disconnectivity On Negative Credit";// "با منفی شدن اعتبار رله قطع می شود.";
                case 706:
                    return "Disconnectivity On Expired Credit";//"با اتمام تاریخ پایان اعتبار رله قطع می شود.";
                //case 707:
                //    return " This is an original credit.";
                //case 708:
                //    return "Credit end time will be most far of current end time and new arrived token end time.";

                #endregion
            }
            return string.Empty;
        }







        //////////////////////////////////
        //internal static string PersianContext_303(int messageID)
        //{
        //    int num = messageID;
        //    if (num > 800)
        //    {
        //        if (num > 900)
        //        {
        //            if (num == 0x385)
        //            {
        //                return "فعال شدن عملیات اعتباری";
        //            }
        //            else
        //            {
        //                switch (num)
        //                {
        //                    case 0x3e8:
        //                        return "اعتبار منفی است ";

        //                    case 0x3e9:
        //                        return "تاریخ اعتبار، پایان یافته است";

        //                    case 0x3ea:
        //                        return "عملکرد اعتباری فعال می باشد";

        //                    case 0x3eb:
        //                        return "بدهی فعال است";

        //                    case 0x3ec:
        //                        return "با منفی شدن اعتبار رله قطع می شود";

        //                    case 0x3ed:
        //                        return "با اتمام تاریخ پایان اعتبار رله قطع می شود";

        //                    case 0x3ee:
        //                        return "پمپ روشن است";

        //                    case 0x3ef:
        //                        return "راندمان پمپ پایین است";

        //                    case 0x3f0:
        //                        return "د بی لحظه ای ، بیشتر از دبی ماکزیمم منحنی می باشد";

        //                    case 0x3f1:
        //                        return "د بی لحظه ای ، کمتر از دبی مینیمم منحنی می باشد";

        //                    case 0x3f2:
        //                        return "منحنی اکتیو پمپ، تنظیم شده است";

        //                    default:
        //                        switch (num)
        //                        {
        //                            case 0x41a:
        //                                return "جریان اصلی قطع می باشد.";

        //                            case 0x41b:
        //                                return "رله اصلی ، در حالت قطع از مرکز می باشد";

        //                            case 0x41c:
        //                                return "رله اصلی ، در حالت وصل می باشد";

        //                            case 0x41d:
        //                                return "رله اصلی در حالت آماده به وصل می باشد";

        //                            case 0x41e:
        //                                return "قطع رله به علت اعتبار منفی اتفاق افتاده است";

        //                            case 0x41f:
        //                                return "قطع رله به علت اتمام تاریخ پایان اعتبار می باشد";

        //                            case 0x420:
        //                                return "قطع رله به علت قرار گرفتن در بازه زمانی پیک می باشد";

        //                            case 0x421:
        //                                return "قطع رله به علت محدودیت مصرف می باشد";

        //                            case 0x422:
        //                                return "رله به علت انجام عمل soft starter قطع می باشد";

        //                            case 0x423:
        //                                return "رله به علت stabilizer قطع می باشد";

        //                            case 0x424:
        //                                return "رله به علت دمای پایین از کار افتاده است";

        //                            default:
        //                                break;
        //                        }
        //                        break;
        //                }
        //            }
        //        }
        //        else if (num == 0x321)
        //        {
        //            return "فعال شدن عملیات اعتباری";
        //        }
        //        else if (num == 900)
        //        {
        //            return " قابلیت اعتبار";
        //        }
        //    }
        //    else if (num > 0x2a9)
        //    {
        //        switch (num)
        //        {
        //            case 0x2c0:
        //                return "مدت زمانی که کنتور قبل از قطع رله ، به علت اتمام تاریخ پایان اعتبار ، در نظر می گیرد.";

        //            case 0x2c1:
        //                return "با منفی شدن اعتبار رله قطع می شود.";

        //            case 0x2c2:
        //                return "با اتمام تاریخ پایان اعتبار رله قطع می شود.";

        //            default:
        //                if (num != 800)
        //                {
        //                    break;
        //                }
        //                return " قابلیت اعتبار";
        //        }
        //    }
        //    else
        //    {
        //        switch (num)
        //        {
        //            case 0:
        //                return "اطلاعات موردنظر در دسترس نیست";

        //            case 1:
        //                return "جریان اصلی قطع می باشد.";

        //            case 2:
        //                return "رله اصلی ، در حالت قطع از مرکز می باشد";

        //            case 3:
        //                return "رله اصلی ، در حالت وصل می باشد.";

        //            case 4:
        //                return "رله اصلی در حالت آماده به وصل می باشد.";

        //            case 5:
        //                return "";

        //            case 6:
        //                return "";

        //            case 7:
        //                return "";

        //            case 8:
        //                return "";

        //            case 9:
        //                return "";




        //        }
        //    }
        //}
        ///////////////////////////////////
















                                        /////////////////////////////////////////////////////////////////////////////


                                }
                            }
