namespace WolfeReiter.AspNetCore.Authentication.AzureAD
{
    public static class AzureClaimTypes
    {
        const string _objectIdentifier = "http://schemas.microsoft.com/identity/claims/objectidentifier";
        const string _upn = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn";
        const string _name = "name";
        const string _given = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";
        const string _surname = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";
        public static string ObjectIdentifier { get { return _objectIdentifier; } }
        public static string UserPrincipalName { get { return _upn; } }
        public static string UserName { get { return _name; } }
        public static string FirstName { get { return _given; } }
        public static string LastName { get { return _surname; } }
    }
}