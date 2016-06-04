namespace DevTreks.Models
{
    public class AccountHelper
    {
        //club sets public authorization level in own agreement (at accounttoservice node)
        //club sets subscribing clubs' authorization level 
        //in subscribing clubs' subscription agreement (at accounttoservice node)
        public enum AUTHORIZATION_LEVELS
        {
            //can not use data (can not view or edit data)
            none = 0,
            //can view but not edit data
            //when a club first subscribes to a service, 
            //they'll be switched from none to viewonly
            viewonly = 1,
            //can view and edit data 
            fulledits = 2
        }
        public static AUTHORIZATION_LEVELS GetAuthorizationLevel(int authorizationLevel)
        {
            AUTHORIZATION_LEVELS eAuthorizationLevel
                = (AUTHORIZATION_LEVELS)authorizationLevel;
            return eAuthorizationLevel;
        }
    }
}
