using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DbManager.Security;
using DbManager;
using DbManager.Parser;

namespace SecurityParsingTests
{
    public class UpdateTests
    {
        [Fact]
        public void updateTableDontExist()
        {
            Database db = Database.CreateTestDatabase();
            List<SetValue> setValues = new List<SetValue>() { new SetValue("Name", "Fiona") };
            Update updateQuery = new Update("TablaInventada", setValues, null);
            string result = updateQuery.Execute(db);
            Assert.Equal(Constants.TableDoesNotExistError, result);
        }
       


    }
}
