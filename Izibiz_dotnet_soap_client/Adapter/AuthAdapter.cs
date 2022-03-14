using Izibiz.AuthenticationWS;

namespace Izibiz.Adapter
{
    public class AuthAdapter
    {


        public LoginResponse Login(LoginRequest loginRequest)
        {
            AuthenticationServicePortClient authenticationPortClient = new AuthenticationServicePortClient();
            return authenticationPortClient.Login(loginRequest);

        }

        public LogoutResponse Logout(LogoutRequest logoutRequest)
        {
            AuthenticationServicePortClient authenticationPortClient = new AuthenticationServicePortClient();
            return authenticationPortClient.Logout(logoutRequest);

        }
        public CheckUserResponse CheckUserInvoice(CheckUserRequest checkInvoiceRequest)
        {
            AuthenticationServicePortClient authenticationPortClient = new AuthenticationServicePortClient();
            return authenticationPortClient.CheckUser(checkInvoiceRequest);

        }
        public CheckUserResponse CheckUserDespatchadvice(CheckUserRequest checkDespatchadviceRequest)
        {
            AuthenticationServicePortClient authenticationPortClient = new AuthenticationServicePortClient();
            return authenticationPortClient.CheckUser(checkDespatchadviceRequest);

        }
        public GetGibUserListResponse GetGibUserList(GetGibUserListRequest gibuserRequest)
        {
            AuthenticationServicePortClient authenticationPortClient = new AuthenticationServicePortClient();
            return authenticationPortClient.GetGibUserList(gibuserRequest);

        }




    }
}