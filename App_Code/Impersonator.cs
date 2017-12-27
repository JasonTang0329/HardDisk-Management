#region Using directives.
// ----------------------------------------------------------------------

using System;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.ComponentModel;

// ----------------------------------------------------------------------
#endregion

/////////////////////////////////////////////////////////////////////////

/// <summary>
/// Impersonation of a user. Allows to execute code under another
/// user context.
/// Please note that the account that instantiates the Impersonator class
/// needs to have the 'Act as part of operating system' privilege set.
/// </summary>
/// <remarks> 
/// This class is based on the information in the Microsoft knowledge base
/// article http://support.microsoft.com/default.aspx?scid=kb;en-us;Q306158
/// 
/// Encapsulate an instance into a using-directive like e.g.:
/// 
/// ...
/// using ( new Impersonator( "myUsername", "myDomainname", "myPassword" ) )
/// {
/// ...
/// [code that executes under the new context]
/// ...
/// }
/// ...
/// 
/// Please contact the author Uwe Keim (mailto:uwe.keim@zeta-software.de)
/// for questions regarding this class.
/// </remarks>
public class Impersonator :
    IDisposable
{
    #region Public methods.
    // ------------------------------------------------------------------

    /// <summary>
    /// Constructor. Starts the impersonation with the given credentials.
    /// Please note that the account that instantiates the Impersonator class
    /// needs to have the 'Act as part of operating system' privilege set.
    /// </summary>
    /// <param name="userName">The name of the user to act as.</param>
    /// <param name="domainName">The domain name of the user to act as.</param>
    /// <param name="password">The password of the user to act as.</param>
    public Impersonator(
        string userName,
        string domainName,
        string password)
    {
        ImpersonateValidUser(userName, domainName, password);
    }

    // ------------------------------------------------------------------
    #endregion

    #region IDisposable member.
    // ------------------------------------------------------------------

    public void Dispose()
    {
        UndoImpersonation();
    }

    // ------------------------------------------------------------------
    #endregion

    #region P/Invoke.
    // ------------------------------------------------------------------

    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern int LogonUser(
        string lpszUserName,
        string lpszDomain,
        string lpszPassword,
        int dwLogonType,
        int dwLogonProvider,
        ref IntPtr phToken);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int DuplicateToken(
        IntPtr hToken,
        int impersonationLevel,
        ref IntPtr hNewToken);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool RevertToSelf();

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    private static extern bool CloseHandle(
        IntPtr handle);

    private const int LOGON32_LOGON_INTERACTIVE = 2;
    private const int LOGON32_PROVIDER_DEFAULT = 0;
    private const int LOGON32_LOGON_NETWORK = 3;
    private const int LOGON32_LOGON_BATCH = 4;
    private const int LOGON32_LOGON_SERVICE = 5;
    private const int LOGON32_LOGON_UNLOCK = 7;
    private const int LOGON32_LOGON_NETWORK_CLEARTEXT = 8; // Only for Win2K or higher 
    private const int LOGON32_LOGON_NEW_CREDENTIALS = 9; // Only for Win2K or higher 


    // ------------------------------------------------------------------
    #endregion

    #region Private member.
    // ------------------------------------------------------------------

    /// <summary>
    /// Does the actual impersonation.
    /// </summary>
    /// <param name="userName">The name of the user to act as.</param>
    /// <param name="domainName">The domain name of the user to act as.</param>
    /// <param name="password">The password of the user to act as.</param>
    private void ImpersonateValidUser(
        string userName,
        string domain,
        string password)
    {
        WindowsIdentity tempWindowsIdentity = null;
        IntPtr token = IntPtr.Zero;
        IntPtr tokenDuplicate = IntPtr.Zero;

        try
        {
            if (RevertToSelf())
            {//LOGON32_LOGON_NETWORK_CLEARTEXT通常是在Server為了配合多平台的檔案寫入（例如：linux）
                //才會用這個降低安全性的選項，讓server允許直接帳號密碼登入並且寫入檔案系統
                //LOGON32_LOGON_NEW_CREDENTIALS才是一般server會採用的安全性作法，使用者需利用帳密取得一組credential
                //再用credential去登入server, 方可寫入server的檔案系統
                //完整的LogonType解釋詳見msdn
                //https://msdn.microsoft.com/en-us/library/windows/desktop/aa378184(v=vs.85).aspx
                //Server的這個設定在 程式集=>系統管理工具=>本機安全性原則裡面，此設定為進階設定，非mis人員通常很少設定他
                if (LogonUser(
                    userName,
                    domain,
                    password,
                    LOGON32_LOGON_NEW_CREDENTIALS,
                    LOGON32_PROVIDER_DEFAULT,
                    ref token) != 0)
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        impersonationContext = tempWindowsIdentity.Impersonate();
                    }
                    else
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                }
                else
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            else
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
        finally
        {
            if (token != IntPtr.Zero)
            {
                CloseHandle(token);
            }
            if (tokenDuplicate != IntPtr.Zero)
            {
                CloseHandle(tokenDuplicate);
            }
        }
    }

    /// <summary>
    /// Reverts the impersonation.
    /// </summary>
    private void UndoImpersonation()
    {
        if (impersonationContext != null)
        {
            impersonationContext.Undo();
        }
    }

    private WindowsImpersonationContext impersonationContext = null;

    // ------------------------------------------------------------------
    #endregion
}

/////////////////////////////////////////////////////////////////////////