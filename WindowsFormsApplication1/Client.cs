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
    /// <summary>
    /// This class uses singleton pattern;
    /// </summary>
    class Client
    {
        string host;
        string password;
        string login;
        RedmineManager manager;
        User user;
        IList<Issue> cacheissue;
        IList<Issue> totalissue;
        IList<Project> totalproject;
        private static Client _instance;
        public static Client Instance
        {
            get
            {
                return _instance;
            }
        }
        public Client(string login, string password, string host)
        {
            this.login = login;
            this.password = password;
            this.host = host;
            manager = new RedmineManager(host, login, password);
            cacheissue = this.GetTotalIssue();
            totalissue = this.GetTotalIssue();
            totalproject = this.GetTotalProject();
            _instance = this;
        }
        public IList<Issue> GetTotalIssue()
        {
            user = manager.GetCurrentUser();
            var par = new NameValueCollection { { "assigned_to_id", user.Id.ToString() } };//set filter for Issues               
            var total = manager.GetTotalObjectList<Issue>(par);
            return total;
        }
        public IList<Project> GetTotalProject()
        {
            user = manager.GetCurrentUser();
            var total = manager.GetTotalObjectList<Project>(null);
            return total;
        }
        public IList<Issue> CacheIssue
        {
            get { return cacheissue; }
            set { cacheissue = value; }
        }
        public IList<Issue> TotalIssue
        {
            get { return totalissue; }
            set { totalissue = value; }
        }
        public IList<Project> TotalProject
        {
            get { return totalproject; }
            set { totalproject = value; }
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
