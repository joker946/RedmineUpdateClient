using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using System.Xml;
using System.Threading;
using System.Diagnostics;
namespace WindowsFormsApplication1
{
    class Client
    {
        string host;
        string password;
        string login;
        RedmineManager manager;
        User user;
        IList<Issue> cache;
        IList<Issue> total;
        public Client(string login, string password, string host)
        {
            this.login = login;
            this.password = password;
            this.host = host;
            manager = new RedmineManager(host, login, password);
            cache = this.GetUpdatedTotal();
            total = this.GetUpdatedTotal();
        }
        public IList<Issue> GetUpdatedTotal()
        {
                user = manager.GetCurrentUser();
                var par = new NameValueCollection { { "assigned_to_id", user.Id.ToString() } };//set filter for Issues
                var total = manager.GetTotalObjectList<Issue>(par);
                return total;
        }
        public IList<Issue> Cache
        {
            get { return cache; }
            set { cache = value; }
        }
        public IList<Issue> Total
        {
            get { return total; }
            set { total = value; }
        }
        public User User
        {
            get { return user; }
            set { user = value; }
        }
        public RedmineManager Manager
        {
            get { return manager; }
            set { manager = value; }
        }
    }
}
