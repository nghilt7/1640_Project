using System.Web.Mvc;

namespace _1640_Project.Areas.QAC
{
    public class QACAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "QAC";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "QAC_default",
                "QAC/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}