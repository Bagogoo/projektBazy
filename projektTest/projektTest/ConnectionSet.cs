using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektTest
{
    public class ConnectionSet
    {
        public ConnectionSet()
        {
            dataSource = "projektbazy.database.windows.net";
            initialCatalog = "BazaDziennik";
            userId = "projektbazy";
            password = "zaq1@WSX";
            connectTimeout = "60";
            encrypt = "True";
            trustServerCertificate = "False";
            applicationIntent = "ReadWrite";
            multiSubnetFailover = "False";
        }



        private string dataSource;
        private string initialCatalog;
        private string userId;
        private string password;
        private string connectTimeout;
        private string encrypt;
        private string trustServerCertificate;
        private string applicationIntent;
        private string multiSubnetFailover;

        public string get_dataSource() { return dataSource; }
        public string get_initialCatalog() { return initialCatalog; }
        public string get_userId() { return userId; }
        public string get_password() { return password; }
        public string get_connectTimeout() { return connectTimeout; }
        public string get_encrypt() { return encrypt; }
        public string get_trustServerCertificate() { return trustServerCertificate; }
        public string get_applicationIntent() { return applicationIntent; }
        public string get_multiSubnetFailover() { return multiSubnetFailover; }

        public void set_dataSource(string val) { dataSource = val; }
        public void set_initialCatalog(string val) { initialCatalog = val; }
        public void set_userId(string val) { userId = val; }
        public void set_password(string val) { password = val; }
        public void set_connectTimeout(string val) { connectTimeout = val; }
        public void set_encrypt(string val) { encrypt = val; }
        public void set_trustServerCertificate(string val) { trustServerCertificate = val; }
        public void set_applicationIntent(string val) { applicationIntent = val; }
        public void set_multiSubnetFailover(string val) { multiSubnetFailover = val; }



    }
}
