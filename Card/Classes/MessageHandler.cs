using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Card.Classes
{
    internal  class MessageHandler
    {       
        public event dataReceived DataReceived;
        public void showCardMessage(string messageCode)
        {
            string messageDesc;
            switch (messageCode)
            {
                case "0":
                    messageDesc = "";
                    break;


                #region CARD Message

                case "3001":
                    messageDesc = "عمليات تبادل اطلاعات با کارت با موفقيت انجام شد";
                    break;
                case "3002":
                    messageDesc = "ورودي معتبر نمي باشد";
                    break;
                case "3003":
                    messageDesc = "کارت در کارتخوان موجود نمي باشد";
                    break;
                case "3004":
                    messageDesc = "کارت نامعتبر است و يا کارت اشتباه قرارداده شده است";
                    break;
                case "3005":
                    messageDesc = "(3005) مشكل در كارت";
                    break;
                case "3006":
                    messageDesc = "کارتخوان معتبر نمي باشد";
                    break;
                case "3007":
                    messageDesc = "کارت نامعتبر";
                    break;
                case "3008":
                    messageDesc = "(3008) بروز خطا در خواندن اطلاعات از کارت";
                    break;
                case "3009":
                    messageDesc = "بروز خطا در نوشتن اطلاعات در کارت";
                    break;
                case "3010":
                    messageDesc = "(3010) کارت نامعتبر";
                    break;
                case "3011":
                    messageDesc = "(3011) کارت نامعتبر";
                    break;
                case "3012":
                    messageDesc = "ذخيره اطلاعات با مشکل مواجه گرديد";
                    break;
                case "3013":
                    messageDesc = "(3013) مشكل در كارت";
                    break;
                case "3014":
                    messageDesc = "ذخيره کد با مشکل مواجه گرديد";
                    break;
                case "3015":
                    messageDesc = "(3015) کارت نامعتبر";
                    break;
                case "3016":
                    messageDesc = "(3016) بروز خطا در خواندن اطلاعات از کارت";
                    break;
                case "3017":
                    messageDesc = "(3017)بروز خطا در خواندن اطلاعات از کارت";
                    break;
                case "4100":
                    messageDesc = "عمليات تبادل اطلاعات با کارت با موفقيت انجام شد";
                    break;
                case "4101":
                    messageDesc = "کارت در کارتخوان موجود نمي باشد";
                    break;
                case "4102":
                    messageDesc = "کارت نامعتبر است و يا کارت اشتباه قرارداده شده است";
                    break;
                case "4103":
                    messageDesc = "کارتخوان متصل نمي باشد";
                    break;
                case "4104":
                    messageDesc = "(4104)  خطا در قرائت از کارت . مجددا سعی کنید";
                    break;
                case "4200":
                    messageDesc = "نوشتن كد بر روي كات";
                    break;
                case "4201":
                    messageDesc = "كارت راه انداز در كارتخوان قرار دارد. لطفا كارت مربوط به كنتور را در كارتخوان قرار دهيد ودوباره اطلاعات را ارسال نماييد.";
                    break;
                case "4202":
                    messageDesc = "كارت راه انداز در كارتخوان قرار دارد. لطفا كارت مربوط به كنتور را در كارتخوان قرار دهيد .";
                    break;
                case "4203":
                    messageDesc = "صدور کارت المثنی با موفقیت به اتمام رسید";
                    break;


                case "6581": messageDesc = "Memory error (e.g. during a write operation)."; break;
                case "65xx": messageDesc = "Execution error; state of non-volatile memory changed."; break;
                case "6700": messageDesc = "Length incorrect."; break;
                case "6800": messageDesc = "Functions in the class byte not supported (general)."; break;
                case "6881": messageDesc = "Logical channels not supported."; break;
                case "6882": messageDesc = "Secure messaging not supported."; break;
                case "6900": messageDesc = "Command not allowed (general)"; break;
                case "6981": messageDesc = "Command incompatible with file structure."; break;
                case "6982": messageDesc = "Security state not satisfied."; break;
                case "6983": messageDesc = "Authentication method blocked."; break;
                case "6984": messageDesc = "Referenced data reversibly blocked (invalidated)."; break;
                case "6985": messageDesc = "Usage conditions not satisfied."; break;
                case "6986": messageDesc = "Command not allowed (no EF selected)."; break;
                case "6987": messageDesc = "Expected secure messaging data objects missing."; break;
                case "6988": messageDesc = "Secure messaging data objects incorrect."; break;
                case "6A00": messageDesc = "Incorrect P1 or P2 parameters (general)."; break;
                case "6A80": messageDesc = "Parameters in the data portion are incorrect."; break;
                case "6A81": messageDesc = "Function not supported."; break;
                //case "6A82": errDesc = "File not found."; break;
                case "6A83": messageDesc = "Record not found."; break;
                case "6A84": messageDesc = "Insufficient memory."; break;
                case "6A85": messageDesc = "Lc inconsistent with TLV structure"; break;
                case "6A86": messageDesc = "Incorrect P1or P2 parameter."; break;
                case "6A87": messageDesc = "Lc inconsistent with P1 or P2."; break;
                case "6A88": messageDesc = "Referenced data not found."; break;
                case "6B00": messageDesc = "Parameter 1 or 2 incorrect."; break;
                case "6Cxx": messageDesc = "Bad length value in Le; ‘xx’ is the correct length."; break;
                case "6D00": messageDesc = "Command (instruction) not supported."; break;
                case "6E00": messageDesc = "Class not supported."; break;
                case "6F00": messageDesc = "Command aborted – more exact diagnosis not possible (e.g., operating system error)."; break;
                case "9000": messageDesc = "Command successfully executed."; break;
                case "920x": messageDesc = "Writing to EEPROM successful after ‘x’ attempts."; break;
                case "9210": messageDesc = "Insufficient memory."; break;
                case "9240": messageDesc = "Writing to EEPROM not successful."; break;
                case "9400": messageDesc = "No EF selected."; break;
                case "9402": messageDesc = "Address range exceeded."; break;
                case "9404": messageDesc = "FID not found, record not found or comparison pattern not found."; break;
                case "9408": messageDesc = "Selected file type does not match command."; break;
                case "9802": messageDesc = "No PIN defined."; break;
                case "9804": messageDesc = "Access conditions not satisfied, authentication failed."; break;
                case "9835": messageDesc = "ASK RANDOM or GIVE RANDOM not executed."; break;
                case "9840": messageDesc = "PIN verification not successful."; break;
                case "9850": messageDesc = "INCREASE or DECREASE could not be executed because a limit has been reached."; break;
                case "9Fxx": messageDesc = "Command successfully executed; ‘xx’ bytes of data are available and can be requested using GET RESPONSE"; break;


                case "9808":
                    messageDesc = "in contradiction with CHV status";
                    break;
                case "9810":
                    messageDesc = "in contradiction with invalidation status";
                    break;

                case "6281":
                    messageDesc = "execution error, memory unchanged - returned data corrupted";
                    break;
                case "6282":
                    messageDesc = "execution error, memory unchanged - returned data short";
                    break;
                case "6283":
                    messageDesc = "execution error, memory unchanged - selected file invalid";
                    break;
                case "6284":
                    messageDesc = "execution error, memory unchanged - FCI format invalid";
                    break;
                case "6381":
                    messageDesc = "execution error, memory changed - file filled up";
                    break;
                case "63C0":
                    messageDesc = "execution error, memory changed - counter 0";
                    break;
                case "63C1":
                    messageDesc = "execution error, memory changed - counter 1";
                    break;
                case "63C2":
                    messageDesc = "execution error, memory changed - counter 2";
                    break;
                case "63C3":
                    messageDesc = "execution error, memory changed - counter 3";
                    break;
                case "63C4":
                    messageDesc = "execution error, memory changed - counter 4";
                    break;
                case "63C5":
                    messageDesc = "execution error, memory changed - counter 5";
                    break;
                case "63C6":
                    messageDesc = "execution error, memory changed - counter 6";
                    break;
                case "63C7":
                    messageDesc = "execution error, memory changed - counter 7";
                    break;
                case "63C8":
                    messageDesc = "execution error, memory changed - counter 8";
                    break;
                case "63C9":
                    messageDesc = "execution error, memory changed - counter 9";
                    break;
                case "63CA":
                    messageDesc = "execution error, memory changed - counter 10";
                    break;
                case "63CB":
                    messageDesc = "execution error, memory changed - counter 11";
                    break;
                case "63CC":
                    messageDesc = "execution error, memory changed - counter 12";
                    break;
                case "63CD":
                    messageDesc = "execution error, memory changed - counter 13";
                    break;
                case "63CE":
                    messageDesc = "execution error, memory changed - counter 14";
                    break;
                case "63CF":
                    messageDesc = "execution error, memory changed - counter 15";
                    break;

                case "6A82":
                    messageDesc = "incorrect P1 or P2 - file not found";
                    break;
                #endregion End of CARD Errors

                #region Security Message
                case "500":
                    messageDesc = "كد كارت مربوط به اين شهرستان نمي باشد";
                    break;
                case "501":
                    messageDesc = "كد كارت مربوط به اين استان نمي باشد";
                    break;
                case "502":
                    messageDesc = "كد كارت نامعتبر است";
                    break;
                case "503":
                    messageDesc = "لطفا كارت راه انداز را در كارت خوان قرار دهيد";
                    break;
                case "504":
                    messageDesc = "نام كاربر صحيح نمي باشد";
                    break;
                case "505":
                    messageDesc = "كلمه عبور صحيح نمي باشد";
                    break;
                #endregion Security Message

                #region General Message
                case "1":
                    messageDesc = " پايان انجام فرمان ";
                    break;
                case "2":
                    messageDesc = " اين فرآيند تعريف نشده است ";
                    break;

                case "3":
                    messageDesc = "در حال برقراری ارتباط با کارت ";
                    break;

                case "4":
                    messageDesc = " در حال بررسی اعتبار کارت ";
                    break;


                case "5":
                    messageDesc = " دریافت اطلاعات از کارت ";
                    break;


                case "6":
                    messageDesc = " خطا در برقراری ارتباط با کارت ";
                    break;


                case "7":
                    messageDesc = "خطا در نوشتن اطلاعات روی کارت";
                    break;


                case "8":
                    messageDesc = "  ";
                    break;

                case "9":
                    messageDesc = "  ";
                    break;


                #endregion of General Message

                default:
                    messageDesc = "خطاي ناشناخته در اجراي فرمان " + messageCode;
                    break;
            } 
            
            if (messageDesc != "")
            {
                MessageEventArgs args = new MessageEventArgs(messageDesc, MessageEventsType.CardError);
                this.DataReceived(this, args);
            }
        }


        public void shoWMeterMessage(string messageCode)
        {
            string messageDesc = "";
            switch (messageCode)
            {
                case "0":
                    messageDesc = "خطا: شماره کنتور ثبت شده در کارت 0 می باشد";
                    break;
                case "-1":
                    messageDesc = "خطا: شماره کنتور ثبت شده در کارت معتبر نیست";
                    break;

                case "-2":
                    messageDesc = "اطلاعاتی در کارت ثبت نشده است";
                    break;
                default:
                    break;

            }
            if (messageDesc != "")
            {
                MessageEventArgs args = new MessageEventArgs(messageDesc, MessageEventsType.MeterError);
                this.DataReceived(this, args);
            }
        }
    }
}
