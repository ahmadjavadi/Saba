using RsaDateTime;
using SABA_CH.Models.Credit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SABA_CH.DataBase
{
    public class DataBaseManager
    {
        private static DataBaseManager _instance;
        SabaNewEntities db;
        public static DataBaseManager Instance
        {
            get { return _instance ?? (_instance = new DataBaseManager()); }
        }


        internal List<User> GetAllUsers()
        {
            try
            {
                using (SabaNewEntities db = new SabaNewEntities())
                {

                    return db.Users.ToList();
                    // the rest
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal void InsUser(string userName, string userPass, ref decimal userID)
        {
            db = new SabaNewEntities();
            var user = db.Users.FirstOrDefault(x => x.UserName == userName);
            if (user == null)
            {
                user = new User() { UserName = userName, UserPass = userPass };
                db.Users.Add(user);
                db.SaveChanges();
                userID = user.UserID;
            }
            else
            {
                userID = user.UserID;
            }
        }


        internal void DelUser(string userName)
        {
            db = new SabaNewEntities();
            var user = db.Users.First(x => x.UserName == userName);
            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
            }
        }


        internal void InsModem(string modemUID, ref decimal modemId)
        {
            db = new SabaNewEntities();
            Modem m = new Modem() { ModemNumber = modemUID };

            try
            {
                if (db.Modems.Any(x => x.ModemNumber == modemUID))
                {
                    var modem = db.Modems.First(x => x.ModemNumber == modemUID);
                    modemId = modem.ModemID;

                }

                else
                {
                    db.Modems.Add(m);
                    db.SaveChanges();
                    modemId = m.ModemID;
                }

            }
            catch (Exception EX)
            {

            }
        }

        internal void FindModemID(string modemNumber, ref decimal? modemID, ref string errMSG)
        {
            try
            {
                db = new SabaNewEntities();

                var M = db.Modems.FirstOrDefault(x => x.ModemNumber == modemNumber);
                if (M != null)
                {
                    modemID = M.ModemID;
                    return;
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                errMSG = ex.Message;
            }
        }

        internal void FindCustomerID(string CustomerNumber, ref decimal CustomerID, ref string errMSG)
        {
            try
            {
                db = new SabaNewEntities();

                var M = db.Customers.FirstOrDefault(x => x.WatersubscriptionNumber == CustomerNumber);
                if (M != null)
                {
                    CustomerID = M.CustomerID;
                    return;
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                errMSG = ex.Message;
            }
        }

        internal WaterSubscriberGrid WaterSubscriberNoSearching(string WaterSubscriberNo)
        {
            try
            {
                string errmsg = null;
                decimal customerid = 0;
                db = new SabaNewEntities();

                FindCustomerID(WaterSubscriberNo, ref customerid, ref errmsg);
                var creditValue = db.Credits.FirstOrDefault(x => x.CustomerId == customerid);
                if (creditValue != null)
                {
                    var result = (from Cr in db.Credits
                                  join Cstm in db.Customers on Cr.CustomerId equals Cstm.CustomerID
                                  into subscriber
                                  from m in subscriber.DefaultIfEmpty()
                                  where Cr.IsValid == true && Cr.CustomerId == creditValue.CustomerId
                                  select new
                                  {
                                      WaterSubscriberNo = m.WatersubscriptionNumber,
                                      SubscriberName = m.CustomerName,
                                      SubscriberFamily = m.Customerfamily,
                                      LastAnnualCredit = (Cr.Credit1).ToString(),
                                      CreditType = Cr.Type,
                                      StartDate = Cr.StartDate,
                                      EndDate = Cr.EndDate,
                                      TransferedToMeter = (Cr.UsedByMeter).ToString(),
                                      TransferedToCard = (Cr.TransferedToCard).ToString()

                                  }).ToList();
                    if (result != null)
                    {
                        var GridOutPut = new WaterSubscriberGrid(result[0].WaterSubscriberNo,
                                                                 result[0].SubscriberName,
                                                                 result[0].SubscriberFamily,
                                                                 result[0].LastAnnualCredit,
                                                                 result[0].StartDate,
                                                                 result[0].EndDate,
                                                                 result[0].TransferedToMeter,
                                                                 result[0].TransferedToCard
                                                                 );

                        return GridOutPut;
                    }



                }
                return null;
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        internal void InsCredit(string CreditValue, string WaterCustomerNum, string StartDateValue, string userid)
        {
            try
            {
                db = new SabaNewEntities();

                string date_now = System.DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                var M = db.AnnualCredits.FirstOrDefault(x => x.WaterSubscriberNumber == WaterCustomerNum);
                if (M != null)
                {
                    if ((M.IsValid == true) && ((M.Credit != Convert.ToInt32(CreditValue)) || (M.StartAnnualDate != StartDateValue)))
                    {
                        AnnualCredit Cr = new AnnualCredit() { Credit = Convert.ToInt32(CreditValue), WaterSubscriberNumber = WaterCustomerNum, StartAnnualDate = StartDateValue, IsValid = true };
                        db.AnnualCredits.Add(Cr);
                        M.IsValid = false;
                        M.ChangeDate = date_now;
                        db.SaveChanges();
                        return;
                    }

                }
                else
                {
                    AnnualCredit Cr = new AnnualCredit() { Credit = Convert.ToInt32(CreditValue), WaterSubscriberNumber = WaterCustomerNum, StartAnnualDate = StartDateValue, IsValid = true };
                    db.AnnualCredits.Add(Cr);

                    db.SaveChanges();
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        internal void CreditTableUpdate()
        {
            try
            {
                db = new SabaNewEntities();
                PersianDate pd = new PersianDate();
                var currentDate = pd.CurrentDate().Substring(4, 6);
                var year = pd.Now().Year;
                List<AnnualCredit> M = db.AnnualCredits.Where(x => x.IsValid == true).ToList();
                pd.AddDays(-100);
                var threemonthsago_1 =pd.ConvertToGeorgianDateTime().ToString("yyyy/MM/dd");
                var threemonthsago = DateTime.ParseExact(threemonthsago_1, "yyyy/MM/dd", null);

                if (M.Count > 0)
                {
                    bool flag = false;
                    List<Credit> c = db.Credits.Where(x => x.IsValid == true).ToList();
                    foreach (var annualItem in M)
                    {
                        flag = false;
                        for (int i = 0; i < c.Count; i++)
                        {
                            // var b = Convert.ToDateTime(year + c[i].StartDate).ToString("yyyy/MM/dd");
                            var CreditStartDate = DateTime.ParseExact(Convert.ToDateTime(year + c[i].StartDate).ToString("yyyy/MM/dd"), "yyyy/MM/dd", null);


                            var creditcustomerid = c[i].CustomerId;
                            var watersubscribernumber = db.Customers.FirstOrDefault(x => x.CustomerID == creditcustomerid).WatersubscriptionNumber;
                            if (watersubscribernumber != null)
                            {
                                if (watersubscribernumber == annualItem.WaterSubscriberNumber && annualItem.StartAnnualDate != c[i].StartDate && c[i].IsValid == true)
                                {

                                    if (c[i].Type == 1)   // type=1 -> annual credit
                                    {
                                        Credit Cr = new Credit()
                                        {
                                            Credit1 = (Int32)annualItem.Credit,
                                            IsConfirmed = true,
                                            IsValid = true,
                                            StartDate = annualItem.StartAnnualDate,
                                            EndDate = c[i].EndDate,
                                            CardNumber = c[i].CardNumber,
                                            CreateDate = currentDate,
                                            Customer = c[i].Customer,
                                            CustomerId = c[i].CustomerId,
                                            MeterNumber = c[i].MeterNumber,
                                            Type = c[i].Type,
                                            User = c[i].User,
                                            UserId = c[i].UserId,
                                            UsedByMeter = c[i].UsedByMeter,
                                            TransferedToCard = c[i].TransferedToCard
                                        };
                                        c[i].IsValid = false;
                                        db.Credits.Add(Cr);
                                        db.SaveChanges();
                                        flag = true;
                                    }
                                    else if (c[i].Type == 2 && c[i].UsedByMeter == true && CreditStartDate > threemonthsago)  // type=2 -> credit reduction from next year 
                                    {
                                        Credit Cr = new Credit()
                                        {
                                            Credit1 = (Int32)annualItem.Credit - c[i].Credit1,
                                            IsConfirmed = true,
                                            IsValid = true,
                                            StartDate = annualItem.StartAnnualDate,
                                            EndDate = c[i].EndDate,
                                            CardNumber = c[i].CardNumber,
                                            CreateDate = currentDate,
                                            Customer = c[i].Customer,
                                            CustomerId = c[i].CustomerId,
                                            MeterNumber = c[i].MeterNumber,
                                            Type = c[i].Type,
                                            User = c[i].User,
                                            UserId = c[i].UserId,
                                            UsedByMeter = c[i].UsedByMeter,
                                            TransferedToCard = c[i].TransferedToCard
                                        };
                                        c[i].IsValid = false;
                                        db.Credits.Add(Cr);
                                        db.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        var customers = db.Customers.FirstOrDefault(x => x.WatersubscriptionNumber == annualItem.WaterSubscriberNumber);
                                        if (customers != null)
                                        {
                                            Credit Cr = new Credit()
                                            {
                                                Credit1 = (Int32)annualItem.Credit,
                                                IsConfirmed = true,
                                                IsValid = true,
                                                StartDate = annualItem.StartAnnualDate,
                                                CreateDate = annualItem.StartAnnualDate,
                                                Type = 2,
                                                UserId = 1,
                                                CustomerId = customers.CustomerID
                                            };
                                            db.Credits.Add(Cr);
                                            db.SaveChanges();
                                        }
                                        else
                                        {
                                            MessageBox.Show("The customer '" + annualItem.WaterSubscriberNumber.ToString() + "' is not exist. please define the customer first and then try again");
                                        }
                                    }

                                }
                            }


                        }

                        if (flag == false && !ExistInCredits(annualItem.WaterSubscriberNumber))
                        {
                            var customers = db.Customers.FirstOrDefault(x => x.WatersubscriptionNumber == annualItem.WaterSubscriberNumber);
                            if (customers != null)
                            {
                                Credit Cr = new Credit()
                                {
                                    Credit1 = (Int32)annualItem.Credit,
                                    IsConfirmed = true,
                                    IsValid = true,
                                    StartDate = annualItem.StartAnnualDate,
                                    CreateDate = annualItem.StartAnnualDate,
                                    Type = 1,
                                    UserId = 1,
                                    CustomerId = customers.CustomerID
                                };
                                db.Credits.Add(Cr);
                                db.SaveChanges();
                            }
                            else
                            {
                                MessageBox.Show("The customer '" + annualItem.WaterSubscriberNumber.ToString() + "' is not exist. please define the customer first and then try again");
                            }


                        }

                    }

                    return;
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {

            }
        }


        internal List<ShowCustomers_Result> GetAllCustomer()
        {
            List<ShowCustomers_Result> res = new List<ShowCustomers_Result>();
            try
            {
                using (db = new SabaNewEntities())
                {
                   var a= db.MeterToCustomers;

                }
                

                    
            }
            catch (Exception e)
            {
            }
            return res;
        }

        bool ExistInCredits(string watersubscriberno)
        {

            try
            {
                var customer = db.Customers.FirstOrDefault(vv => vv.WatersubscriptionNumber == watersubscriberno);
                if (customer != null)
                {

                    return db.Credits.Any(x => x.CustomerId == customer.CustomerID);
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
}