using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using logger;

namespace NavisionServiceApp
{

    class SessionCookieHandler
    {
        private static Dictionary<string, Cookie> SessionCookies = new Dictionary<string, Cookie>();
        private static object _lock = new object();

        public void Add(Cookie cookie)
        {
            lock (_lock)
            {
                SessionCookies.Add(cookie.Value, cookie);
            }
        }

        public void  Remove(Cookie cookie)
        {
            if (cookie == null)
                return;

            lock (_lock)
            {
                SessionCookies.Remove(cookie.Value);
            }
        }

        public bool IsValid(Cookie cookie)
        {
            if (cookie == null)
                return false;
            return IsValid(cookie.Value);
        }

        public bool IsValid(string guid)
        {
            if(guid == "")
            {
                return false;
            }

            lock (_lock)
            {
                cleanup();
                Cookie c = SessionCookies[guid];
                if (c == null)
                {
                    return false;
                }
                else if (c.Expired)
                {
                    SessionCookies.Remove(guid);
                    return false;
                }
                return true;
            }
        }
        /// <summary>
        /// Extends cookie lifetime
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool UpdateCookie(Cookie c, int extendHours = 2 )
        {
            if (c == null)
                return false;

            lock (_lock)
            {
                cleanup();
                try
                {
                    Cookie cookie = SessionCookies[c.Value];
                    if (cookie != null)
                    {
                        Logger.Info(String.Format("Cookie guid:{0} prolonged for {1} hours.", c.Value, extendHours));
                        cookie.Expires.AddHours(extendHours);
                    }
                }
                catch
                {
                    // This cookie couldn't be found.
                    return false;
                }
            }
            return true ;
        }

        /// <summary>
        /// Cleanups timed out cookies.
        /// </summary>
        private void cleanup()
        {
            foreach(var sc in SessionCookies)
            {
                if (sc.Value.Expired)
                    SessionCookies.Remove(sc.Key);
           
            }
        }
    }
}
