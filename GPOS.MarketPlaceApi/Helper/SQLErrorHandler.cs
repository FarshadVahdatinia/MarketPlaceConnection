using Microsoft.Data.SqlClient;

namespace GPOS.MarketPlaceApi.Helper
{
    public static class SQLErrorHandler
    {
        private const string DefaultErrorMessage = "خطایی در دیتابیس رخ داده است";
        public static Dictionary<string, string> InvRecieveConfirmProc = new Dictionary<string, string>()
        {
            {"50005","مبادله غیرمجاز" } ,
            {"50037","دوره مالی باز وجود ندارد" } ,
            {"50007","خطای وضعیت سند" } ,
            {"50001","سند پیش نویس نمی باشد" } ,
            {"50008","خطای تامین کننده فعال" } ,
            {"50011","عدم تطبیق رسید و سفارش" } ,
            {"50012","این سفارش قبلا رسید شده" } ,
            {"50014","کالای ناموجود فرستنده" }
        };
        public static Dictionary<string, string> InvPortalSendRequestDeliverySetProc = new Dictionary<string, string>()
        {
            {"50001","صدور غیر مجاز حواله برای درخواست های ناهمگون" } ,
            {"50002","خطای اقلام ناموجود" }
        };

        public static Dictionary<string, string> InvInventoryCountConfirmProc = new Dictionary<string, string>()
        {
            {"50001","خطای وضعیت سند" }
        };

        public static Dictionary<string, string> BuyPurchaseDocConfirmProc = new Dictionary<string, string>()
        {
            {"50037","خطای وضعیت سند" }
        };

        public static Dictionary<string, string> TrsRcvStoreCashConfirm = new Dictionary<string, string>()
        {
            {"50001","خطای عدم تطابق تاریخ سند با دوره مالی" }
        };

        public static Dictionary<string, string> TrsRcvStoreCashFromCGPOS = new Dictionary<string, string>()
        {
            {"50005","حساب بانکی مقصد نامشخص است" }
        };

        public static Dictionary<string, string> InvDeliveryConfirmProc = new Dictionary<string, string>()
        {
            {"50005","مبادله غیرمجاز" },
            {"50037","دوره مالی باز وجود ندارد" },
            {"50007","خطای وضعیت سند" },
            {"50001","این سند پیش نویس نمی باشد" },
            {"50011","انجام نشد" },
            {"50002","مصوبه این تامین کننده غیرقابل مرجوع است" }
        };

        public static Dictionary<string, string> InvRecieveConfirmUndo = new Dictionary<string, string>()
        {
            {"50001","اين دوره مالي بسته شده است" },
            {"50002","برای این سند، حسابداری انبار اجرا شده است" },
            {"50003","نوع سند غیرمجاز" },
            {"50004","سند حسابداری قطعی شده است"},
        };

        public static Dictionary<string, string> InvDeliveryConfirmUndo = new Dictionary<string, string>()
        {
            {"50001","اين دوره مالي بسته شده است" },
            {"50002","برای این سند، حسابداری انبار اجرا شده است" },
            {"50003","نوع سند غیرمجاز" },
            {"50004","سند حسابداری قطعی شده است"},
        };

        public static Dictionary<string, string> InvDeliveryConfirmControl = new Dictionary<string, string>()
        {
            {"50002","مصوبه این تامین کننده غیرقابل مرجوع است" }
        };

        public static string SQLErrorConvert(this SqlException ex)
        {
            var dic = typeof(SQLErrorHandler).GetFields().FirstOrDefault(x => x.Name.ToLower() == ex.Procedure.ToLower());
            if (dic != null)
            {
                if ((((Dictionary<string, string>)dic.GetValue(null)).TryGetValue(ex.Number.ToString(), out var result)))
                    return result;
            }

            return DefaultErrorMessage;
        }
    }
}
