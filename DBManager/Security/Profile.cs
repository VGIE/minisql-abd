using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Security
{
    public class Profile
    {
        public const string AdminProfileName = "Admin";
        public string Name { get; set; }
        public List<User> Users { get; set; } = new List<User>();

        public Dictionary<string, List<Privilege>> PrivilegesOn { get; private set; } = new Dictionary<string, List<Privilege>>();

        public bool GrantPrivilege(string table, Privilege privilege)
        {
            if (string.IsNullOrWhiteSpace(table))
                return false;

            if (!PrivilegesOn.TryGetValue(table, out List<Privilege> list) || list == null)
            {
                list = new List<Privilege>();
                PrivilegesOn[table] = list;
            }

            if (!list.Contains(privilege))
                list.Add(privilege);

            return true;
        }

        public bool RevokePrivilege(string table, Privilege privilege)
        {
            if (string.IsNullOrWhiteSpace(table))
                return false;

            if (!PrivilegesOn.TryGetValue(table, out List<Privilege> list) || list == null)
                return false;

            bool removed = list.Remove(privilege);

            if (list.Count == 0)
                PrivilegesOn.Remove(table);

            return removed;
        }

        public bool IsGrantedPrivilege(string table, Privilege privilege)
        {
            if (string.IsNullOrWhiteSpace(table))
                return false;

            if (!PrivilegesOn.TryGetValue(table, out List<Privilege> list) || list == null)
                return false;

            return list.Contains(privilege);
        }
    }
}