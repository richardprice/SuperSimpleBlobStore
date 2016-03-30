namespace SuperSimpleBlobStore.Common
{
    public enum Claims
    {
        CanLogin = 1,
        CanLogout = 2,
        CanManageClaims = 3,
        CanManageUsers = 4,
        CanResetPasswords = 5,
        ChangePasswordOnLogin = 6,
        AccountIsActivated = 7,
        AccountIsDisabled = 8,
        AccountIsLocked = 9
    }
}
