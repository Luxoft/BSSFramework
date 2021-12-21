using SampleSystem.Domain;

namespace SampleSystem.BLL
{
    public partial class RegularJobResultBLL
    {
        public void SaveTestValue(string testValue)
        {
            this.Save(new RegularJobResult { TestValue = testValue });
        }
    }
}
