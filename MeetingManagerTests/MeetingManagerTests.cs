using MeetingManager;
namespace MeetingManagerTests
{
    [TestClass]
    public class MeetingManagerTests
    {
        [TestMethod]
        public void TestDateLogicSuccessful()
        {
            // arranging data for tests
            var meeting = new Meeting();
            meeting.StartDate = Convert.ToDateTime("2022-01-01");
            meeting.EndDate = Convert.ToDateTime("2022-03-01");

            // act
            bool a = meeting.StartDate < meeting.EndDate;

            // assert
            Assert.IsTrue(a);
        }
        [TestMethod]
        public void TestDateLogicFlawed()
        {
            // arranging data for tests
            var meeting = new Meeting();
            meeting.StartDate = Convert.ToDateTime("2022-01-01");
            meeting.EndDate = Convert.ToDateTime("2022-03-01");

            // act
            bool a = meeting.StartDate < meeting.EndDate;

            // assert
            Assert.IsTrue(a);
        }
    }
}