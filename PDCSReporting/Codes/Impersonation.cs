using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using static MRS.GlobalVars;

namespace MRS
{
    public class Impersonation : IDisposable
    {

        // Add Below Code to Login Button Click Event
        //try
        //    {
        //        Impersonation imp = new Impersonation(txtUsername.Text, "SL", txtPassword.Text);

        //Response.Redirect("StyleTransfer.aspx");
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write("<script>alert('Username or Password is incorrect. Please Try With Different Credentials')</script>");
        //    }
#region Dll Imports

[DllImport("kernel32.dll")]
        private static extern Boolean CloseHandle(IntPtr hObject);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool LogonUser(string username, string domain,
                                              string password, LogonType logonType,
                                              LogonProvider logonProvider,
                                              out IntPtr userToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool DuplicateToken(IntPtr token, int impersonationLevel,
            ref IntPtr duplication);

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool ImpersonateLoggedOnUser(IntPtr userToken);
        #endregion

        #region Private members

        private bool _disposed;

        private System.Security.Principal.WindowsImpersonationContext _impersonationContext;

        #endregion

        #region Constructors

        public Impersonation(String username, String domain, String password)
        {
            IntPtr userToken = IntPtr.Zero;
            IntPtr userTokenDuplication = IntPtr.Zero;

            // Logon with user and get token.
            bool loggedOn = LogonUser(username, domain, password,
                LogonType.Interactive, LogonProvider.Default,
                out userToken);

            if (loggedOn)
            {
                try
                {
                    // Create a duplication of the usertoken, this is a solution
                    // for the known bug that is published under KB article Q319615.
                    if (DuplicateToken(userToken, 2, ref userTokenDuplication))
                    {
                        // Create windows identity from the token and impersonate the user.
                        System.Security.Principal.WindowsIdentity identity = new System.Security.Principal.WindowsIdentity(userTokenDuplication);
                        _impersonationContext = identity.Impersonate();
                    }
                    else
                    {
                        // Token duplication failed!
                        // Use the default ctor overload
                        // that will use Mashal.GetLastWin32Error();
                        // to create the exceptions details.
                        throw new Exception("Could not copy token");
                    }
                    GlobalVars.LOGIN_NAME = Environment.UserName.ToString();
                }
                finally
                {
                    // Close usertoken handle duplication when created.
                    if (!userTokenDuplication.Equals(IntPtr.Zero))
                    {
                        // Closes the handle of the user.
                        CloseHandle(userTokenDuplication);
                        userTokenDuplication = IntPtr.Zero;
                    }

                    // Close usertoken handle when created.
                    if (!userToken.Equals(IntPtr.Zero))
                    {
                        // Closes the handle of the user.
                        CloseHandle(userToken);
                        userToken = IntPtr.Zero;
                    }
                }
            }
            else
            {
                throw new Exception("Login failed");
                //HttpContext.Current.Response.Write("<script>alert('Username or Password is incorrect. Please try again')</script>");
            }
        }

        ~Impersonation()
        {
            Dispose(false);
        }
        #endregion

        #region Public methods

        public void Revert()
        {
            if (_impersonationContext != null)
            {
                // Revert to previous user.
                _impersonationContext.Undo();
                _impersonationContext = null;
            }
        }
        #endregion

        #region IDisposable implementation.

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                Revert();

                _disposed = true;
            }
        }
        #endregion
    }
}